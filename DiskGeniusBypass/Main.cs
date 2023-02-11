﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static DiskGeniusBypass.LicenseFileHandler;

namespace DiskGeniusBypass
{
    class DiskGeniusBypass
    {
        private static string DiskGeniusPath = Directory.GetCurrentDirectory();
        private static string[] LicenseFiles =
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
            formatLicensePaths();

            Bypass();
        }

        public static void formatLicensePaths()
        {
            for (int i = 0; i < LicenseFiles.Length; i++)
            {
                LicenseFiles[i] = Path.Combine(DiskGeniusPath,LicenseFiles[i]);
            }
        }
        public static void Bypass()
        {
            string path = Path.Combine(DiskGeniusPath, DiskGeniusExecutable);
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
                LicenseFileHandler fileHandler = new LicenseFileHandler(StoreType, LicenseFiles);
                fileHandler.Store();

                // Start it
                DGProcess.Start();

                Thread.Sleep(3000);
                if (DGexited)
                {
                    MessageBox.Show("The bypass most likely failed since DiskGenius exited early.", "Bypass failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                fileHandler.Restore();
            }
            else
            {
                MessageBox.Show("DiskGenius_.exe could not be found. Did you forget to rename it? Are you in the right folder?", "Warning");
            }
        }

        // Handle Exited event and display process information.
        private static void DiskGenius_Exited(object sender, EventArgs e)
        {
            
        }

        // This method stores the license files in 
    }
}
