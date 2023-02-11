namespace DiskGeniusBypass
{
    public class FileStoreItem
    {
        public string Filename { get; set; }
        public string OriginalFilepath { get; set; }
        public string CurrentFilepath { get; set; }

        public FileStoreItem(string name, string path)
        {
            Filename = name;
            OriginalFilepath = CurrentFilepath = path;
        }
    }
}
