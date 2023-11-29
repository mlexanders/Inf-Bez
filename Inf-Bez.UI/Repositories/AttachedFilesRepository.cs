using Actions.Server;
using InfBez.Ui.Models;
using Microsoft.EntityFrameworkCore;

namespace InfBez.Ui.Repositories
{
    //public static class Extension
    //{
    //    public static void Map(this AttachedFile attachedFile, FileInfo fileInfo)
    //    {
    //        attachedFile.CreationTime = fileInfo.CreationTime;
    //        attachedFile.LastAccessTime = fileInfo.LastAccessTime;
    //        attachedFile.FullPath = fileInfo.FullName;
    //    }
    //}

    public class AttachedFilesRepository : CrudRepository<AttachedFile, int>
    {
        public AttachedFilesRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<AttachedFile?> FindByPath(string path)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(f => f.FullPath == path);
        }
    }
}
