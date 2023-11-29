using Actions.Common.Base;

namespace InfBez.Ui.Models
{
    public class AttachedFile : Entity<int>
    {
        public string FullPath { get; set; } = null!;
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        public AttachedFile(FileInfo fileInfo)
        {
            CreationTime = fileInfo.CreationTime;
            LastAccessTime = fileInfo.LastAccessTime;
            FullPath = fileInfo.FullName;
        }

        public AttachedFile()
        {

        }
    }
}
