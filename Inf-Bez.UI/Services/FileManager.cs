using System.Text;

namespace InfBez.Ui.Services
{
    public class FileManager
    {
        public string templateName;
        private readonly FileChecker fileChecker;
        private readonly ArchiveManager archiveManager;

        public FileManager(FileChecker fileChecker, ArchiveManager archiveManager)
        {
            templateName = "file";
            this.fileChecker = fileChecker;
            this.archiveManager = archiveManager;
        }

        public async Task CreateFile(string? content = null, bool isUpdate = false)
        {
            if (!isUpdate) currentFilePath = Path.Combine(Folder, FileName);
            currentFilePath ??= FilePath;

            // create html
            using FileStream fstream = new(FilePath, FileMode.OpenOrCreate);
            var buffer = Encoding.UTF8.GetBytes(content ?? "");
            await fstream.WriteAsync(buffer);
            fstream.Close();

            // zipping and encryption
            archiveManager.CreateArchive(FilePath);

            // save data in database
            if (isUpdate) await fileChecker.OnUpdateFile(archiveManager.GetEncryptionPath(FilePath));
            else await fileChecker.OnCreateFile(archiveManager.GetEncryptionPath(FilePath));
        }

        public async Task<string> Change(string fileName)
        {
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

            return content;
        }

        public async Task Save(string content)
        {
            currentFilePath ??= FilePath;

            // delete old file and cached
            File.Delete(FilePath);

            // create html
            // zipping and encryption
            // update validators data to database
            await CreateFile(content, isUpdate: true);
        }

        public void SetTemplateToFileName(string templateName) => this.templateName = templateName.ToLower();

        private string FileName => $"{templateName}-{DateTime.Now:hh-mm-ss}.html";
        private string? currentFilePath;
        private string FilePath => currentFilePath ?? Path.Combine(Folder, FileName);

        private string? _folder;
        private string Folder
        {
            get
            {
                if (string.IsNullOrEmpty(_folder))
                {
                    _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "inf-bez");
                }
                return _folder;
            }
        }
    }
}
