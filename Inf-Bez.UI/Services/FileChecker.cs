using InfBez.Ui.Exceptions;
using InfBez.Ui.Models;
using InfBez.Ui.Repositories;
using System.Security.Cryptography;

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

            var hash = "";
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }

            if (file.LastWriteTime != fileInfo.LastWriteTime) throw new FileNotValidException("File has been modified from outside");
            if (hash != file.Hash) throw new FileNotValidException("Hash not valid");
            return true;
        }

        public async Task OnSaveFile(string fullPath)
        {
            var fileModel = await repository.ReadFirst(f => f.FullPath == fullPath); //FindByPath(fullPath);
            var fileInfo = new FileInfo(fullPath);

            if (!fileInfo.Exists || fileModel is null) throw new FileNotValidException("File not found");


            //fileModel = new AttachedFile(fileInfo);
            //fileModel.Id = fileModel.Id; ;
            fileModel.LastWriteTime = fileInfo.LastWriteTime;
            fileModel.CreationTime = fileInfo.CreationTime;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fullPath))
                {
                    fileModel.Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }

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
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fullPath))
                {
                    fileModel.Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }
            await repository.Create(fileModel);
        }
    }
}
