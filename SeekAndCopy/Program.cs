using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SeekAndCopy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sourceFile = ConfigurationManager.AppSettings["file"];
            var file = new StreamReader(sourceFile);

            var listOfFiles = new List<string>();
            var listOfFoundFiles = new List<string>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                listOfFiles.Add(line);
            }

            Console.WriteLine($"{sourceFile} loaded!");

            var sourceDirectory = ConfigurationManager.AppSettings["source"];
            var directoryInfo = new DirectoryInfo(sourceDirectory);
            var destinationDirectory = ConfigurationManager.AppSettings["destination"];

            Console.WriteLine("Copied files:");
            CopyFiles(directoryInfo, listOfFiles, destinationDirectory, listOfFoundFiles);
            Console.WriteLine("Not found files:");
            foreach (var filename in listOfFiles.Except(listOfFoundFiles))
            {
                Console.WriteLine(filename);
            }
            Console.WriteLine("done");
            Console.ReadLine();
        }

        private static void CopyFiles(DirectoryInfo directoryInfo, List<string> listOfFiles, string destinationDirectory, List<string> listOfFoundFiles)
        {
            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                if (listOfFiles.Contains(fileInfo.Name))
                {
                    var destination = $"{destinationDirectory}\\{fileInfo.Name}";
                    File.Copy(fileInfo.FullName, destination, true);
                    Console.WriteLine($"{fileInfo.FullName} -> {destination}");
                    listOfFoundFiles.Add(fileInfo.Name);
                }
            }

            foreach (var dirInfo in directoryInfo.EnumerateDirectories())
            {
                CopyFiles(dirInfo, listOfFiles, destinationDirectory, listOfFoundFiles);
            }
        }
    }
}
