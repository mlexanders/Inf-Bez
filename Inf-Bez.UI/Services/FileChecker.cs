using InfBez.Ui.Exceptions;
using InfBez.Ui.Repositories;

namespace InfBez.Ui.Services
{
    public class FileChecker
    {
        private AttachedFilesRepository repository;

        public FileChecker(AttachedFilesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Check(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists) throw new NotImplementedException("File could not be opened");

            var file = await repository.FindByPath(fileInfo.FullName) ?? throw new FileNotValidException("File not found");

            if (file.LastAccessTime != fileInfo.LastAccessTime) throw new FileNotValidException("File has been modified from outside");
            return true;
        }

        public async Task OnSaveFile(string fullPath)
        {
            var fileModel = await repository.FindByPath(fullPath);
            var fileInfo = new FileInfo(fullPath);

            if (!fileInfo.Exists || fileModel is null) throw new FileNotValidException("File not found");

            fileModel.LastAccessTime = fileInfo.LastAccessTime;
            fileModel.CreationTime = fileInfo.CreationTime;

            await repository.Update(fileModel);
        }

        public async Task OnCreateFile(string fullPath)
        {
            var fileModel = await repository.FindByPath(fullPath);
            var fileInfo = new FileInfo(fullPath);

            if (!fileInfo.Exists) throw new FileNotValidException("File not found");
            if (fileModel != null)
            {
                await OnSaveFile(fullPath);
                return;
            }

            fileModel = new(fileInfo);
            await repository.Create(fileModel);
        }
    }
}
