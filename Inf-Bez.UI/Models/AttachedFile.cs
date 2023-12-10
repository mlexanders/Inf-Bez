using Actions.Common.Base;

namespace InfBez.Ui.Models
{
    public class AttachedFile : Entity<int>
    {
        public string FullPath { get; set; } = null!;
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Hash { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }

        public AttachedFile(FileInfo fileInfo)
        {
            CreationTime = fileInfo.CreationTime;
            LastWriteTime = fileInfo.LastAccessTime;
            FullPath = fileInfo.FullName;
        }

        public AttachedFile() { }
    }
}
