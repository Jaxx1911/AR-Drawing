// Filename: DirectoryUtils.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 3:11 PM 03/11/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System.IO;

namespace GplaySDK.Core.BaseLib.Utils
{
    public static class DirectoryUtils
    {
        public static void CopyDirectory(string sourceDir, string destinationDir,bool cleanCopy)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
            {
                $"Source directory does not exist or could not be found: {sourceDir}".LogError();
                return;
            }

            if (cleanCopy)
            {
                if (Directory.Exists(destinationDir))
                {
                    Directory.Delete(destinationDir, true);
                }
            }

            RecursiveCopy(sourceDir, destinationDir);


            void RecursiveCopy(string source, string target)
            {
                Directory.CreateDirectory(target);

                foreach (var file in Directory.GetFiles(source))
                {
                    var fileName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(target, fileName), true);
                }

                foreach (var directory in Directory.GetDirectories(source))
                {
                    var dirName = Path.GetFileName(directory);
                    RecursiveCopy(directory, Path.Combine(target, dirName));
                }
            }
        }
    }
}