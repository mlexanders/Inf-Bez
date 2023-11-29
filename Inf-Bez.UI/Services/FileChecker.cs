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
    }
}
