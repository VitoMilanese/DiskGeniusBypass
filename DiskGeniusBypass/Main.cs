using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiskGeniusBypass.Properties.Enums;

namespace DiskGeniusBypass
{
    class DiskGeniusBypass
    {
        private static string DiskGeniusPath = Directory.GetCurrentDirectory();
        private static List<string> LicenseFiles { get; set; } = new List<string>
        {
            "msimg32.dll",
            "OfflineReg.exe",
            "Options.ini"
        };

        private static Process DGProcess;

        private static bool DGexited = false;

        private static string DiskGeniusExecutable = "DiskGenius_.exe";

        // Memory or MoveFiles
        private static StorageMethod StoreType = StorageMethod.MoveFiles;

        public static void Main(string[] args)
        {
            var licenseFiles = new[]
            {
                "msimg32.dll",
                "OfflineReg.exe",
                "Options.ini"
            };
            LicenseFiles = licenseFiles.Select(p => Path.Combine(DiskGeniusPath, p)).ToList();

            Bypass();
        }

        public static void Bypass()
        {
            var path = Path.Combine(DiskGeniusPath, DiskGeniusExecutable);
            if (File.Exists(path))
            {
                // Check if there's a previous process and kill it/dispose of it.
                if (DGProcess != null)
                {
                    //(Process DGProcess.HasExited) ? DGProcess.Kill() : DGProcess.Dispose();
                    if (!DGProcess.HasExited) 
                        DGProcess.Kill();
                    DGProcess.Dispose();
                    DGexited = false;
                }

                // Create the process
                DGProcess = new Process
                {
                    StartInfo = new ProcessStartInfo(path),
                    EnableRaisingEvents = true
                };
                DGProcess.Exited += new EventHandler(DiskGenius_Exited);

                // Store the files
                var fileHandler = new LicenseFileHandler(StoreType, LicenseFiles);
                fileHandler.Store();

                // Start it
                DGProcess.Start();

                Task.Delay(3000).Wait();
                
                if (DGexited)
                    MessageBox.Show("The bypass most likely failed since DiskGenius exited early.", "Bypass failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                
                fileHandler.Restore();
            }
            else
                MessageBox.Show("DiskGenius_.exe could not be found. Did you forget to rename it? Are you in the right folder?", "Warning");
        }

        // Handle Exited event and display process information.
        private static void DiskGenius_Exited(object sender, EventArgs e)
        {
        }

        // This method stores the license files in 
    }
}
