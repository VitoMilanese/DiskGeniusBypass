using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiskGeniusBypass.Properties.Enums;

namespace DiskGeniusBypass
{
    public partial class LicenseFileHandler
    {
        private List<MemStoreItem> _memstore { get; set; }
        private List<FileStoreItem> _filestore { get; set; }
        private string _tempFolder { get; set; }
        private StorageMethod _storageMethod { get; set; }

        public LicenseFileHandler(StorageMethod storageMethod, List<string> inputFilePaths)
        {
            Console.WriteLine(string.Join(Environment.NewLine, inputFilePaths));
            _storageMethod = storageMethod;
            if (storageMethod == StorageMethod.Memory)
                _memstore = inputFilePaths.Select(p => new MemStoreItem(Path.GetFileName(p), p)).ToList();
            else if (storageMethod == StorageMethod.MoveFiles)
            {
                _filestore = inputFilePaths.Select(p => new FileStoreItem(Path.GetFileName(p), p)).ToList();
                _tempFolder = "temp" + Guid.NewGuid().ToString().Split(char.Parse("-")).FirstOrDefault();
            }
        }

        public void Store()
        {
            if (_storageMethod == StorageMethod.Memory)
                _memstore.ForEach(p =>
                {
                    p.Data = File.ReadAllBytes(p.Filepath);
                    File.Delete(p.Filepath);
                });
            else if (_storageMethod == StorageMethod.MoveFiles)
            {
                Console.WriteLine("move files method");
                if (!File.Exists(_tempFolder))
                    Directory.CreateDirectory(_tempFolder);
                Console.WriteLine(_filestore.Count);
                _filestore.ForEach(p =>
                {
                    Console.WriteLine($"Storing file {p.Filename}");
                    var newPath = Path.Combine(_tempFolder, p.Filename);
                    File.Move(p.OriginalFilepath, newPath);
                    p.CurrentFilepath = newPath;
                });
            }
        }

        public void Restore()
        {
            if (_storageMethod == StorageMethod.Memory)
                _memstore.ForEach(p => File.WriteAllBytes(p.Filepath, p.Data));
            else if (_storageMethod == StorageMethod.MoveFiles)
            {
                _filestore.ForEach(p =>
                {
                    Console.WriteLine("Restoring file {0}", p.Filename);
                    Console.WriteLine(p.CurrentFilepath);
                    File.Move(p.CurrentFilepath, p.OriginalFilepath);
                    p.CurrentFilepath = p.OriginalFilepath;
                });
                Directory.Delete(_tempFolder, true);
            }
        }
    }
}
