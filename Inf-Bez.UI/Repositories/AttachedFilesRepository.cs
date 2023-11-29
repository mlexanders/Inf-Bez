using Actions.Server;
using InfBez.Ui.Models;
using Microsoft.EntityFrameworkCore;

namespace InfBez.Ui.Repositories
{
    public class AttachedFilesRepository : CrudRepository<AttachedFile, int>
    {
        public AttachedFilesRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<AttachedFile?> FindByPath(string path)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(f => f.FullPath == path);
        }
    }
}
