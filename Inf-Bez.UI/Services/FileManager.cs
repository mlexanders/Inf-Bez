using InfBez.Ui.Difinitions;
using System.Text;

namespace InfBez.Ui.Services
{
    public class FileManager : IDisposable
    {
        public string templateName;
        private readonly FileChecker fileChecker;
        private readonly ArchiveManager archiveManager;
        private readonly object locker;
        private bool isLocked;

        public FileManager(FileChecker fileChecker, ArchiveManager archiveManager)
        {
            locker = new();
            templateName = "file";
            this.fileChecker = fileChecker;
            this.archiveManager = archiveManager;
        }

        public async Task CreateFile(string? content = null, bool isUpdate = false)
        {
            isLocked = false;
            if (!isUpdate) currentFilePath = Path.Combine(Folder, FileName);
            currentFilePath ??= FilePath;

            // create html
            using FileStream fstream = new(archiveManager.GetBasePath(FilePath), FileMode.OpenOrCreate);
            var buffer = Encoding.UTF8.GetBytes(content ?? "");
            await fstream.WriteAsync(buffer);
            fstream.Close();

            // zipping and encryption
            archiveManager.CreateArchive(archiveManager.GetBasePath(FilePath));

            // save data in database
            if (isUpdate) await fileChecker.OnUpdateFile(archiveManager.GetEncryptionPath(FilePath));
            else await fileChecker.OnCreateFile(archiveManager.GetEncryptionPath(FilePath));

            LockFile();
        }

        public async Task<string> Change(string fileName)
        {
            isLocked = false;
            currentFilePath = Path.Combine(Folder, fileName);

            // check/validate
            await fileChecker.Check(archiveManager.GetEncryptionPath(FilePath));

            // decrypt unZipping
            archiveManager.ExtractFromArchive(FilePath);

            //return or read content
            using FileStream fstream = new(archiveManager.GetBasePath(FilePath), FileMode.OpenOrCreate);
            var buffer = new byte[fstream.Length];
            await fstream.ReadAsync(buffer);
            fstream.Close();
            var content = Encoding.UTF8.GetString(buffer);

            await Save(content);

            LockFile();
            return content;
        }

        public async Task Save(string content)
        {
            isLocked = false;
            currentFilePath ??= FilePath;

            // delete old file and cached
            File.Delete(FilePath);

            // create html
            // zipping and encryption
            // update validators data to database
            await CreateFile(content, isUpdate: true);
        }

        private void LockFile()
        {
            if (isLocked) return;
            isLocked = true;
            var t = new Thread(() =>
            {
                using FileStream fs = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                lock (locker)
                {
                    while (isLocked) { }
                }
                fs.Close();
            }); 
            t.IsBackground = true;
            t.Start();          
        }

        public void SetTemplateToFileName(string templateName) => this.templateName = templateName.ToLower();
        public string CurrentFileName
        {
            get
            {
                if (currentFilePath is null) return "";
                else return Path.GetFileName(currentFilePath);
            }
        }
        private string FileName => $"{templateName}-{DateTime.Now:hh-mm-ss}.html";
        private string? currentFilePath;
        private string FilePath => currentFilePath ?? Path.Combine(Folder, FileName);

        private string? _folder;
        private bool disposedValue;

        private string Folder
        {
            get
            {
                if (string.IsNullOrEmpty(_folder))
                {
                    _folder = AppSettings.BasePath;
                }
                return _folder;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) isLocked = false;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
