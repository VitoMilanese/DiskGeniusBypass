using System.IO;

namespace DiskGeniusBypass
{
    public class MemStoreItem
    {
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public byte[] Data { get; set; }

        public MemStoreItem(string name, string path)
        {
            Filename = name;
            Filepath = path;
            Data = File.ReadAllBytes(Filepath);
        }
    }
}
