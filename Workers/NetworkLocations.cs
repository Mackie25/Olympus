using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympusExample.Workers
{
    public class NetworkLocations : INetworkLocations
    {
        public NetworkLocations()
        {
            CreateFolderStructure();
        }

        public async void CreateFolderStructure()
        {
            string logFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");

            if (CreateDirectory(logFileDirectory))
            {
                string fileName = Path.Combine(logFileDirectory, "out.txt");

                CreateFile(fileName);
            }
        }

        bool CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                //Create directory
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    //Could not create folder
                    Console.WriteLine($"Could not create folder: {directory}, see error message {ex.Message}");
                    throw;
                }
            }

            return true;
        }

        bool CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                //Create file
                try
                {
                    //I use this close here as i wanted a different class to create directory and the file
                    //If it is not close here then the stream writer in the MultithreadingExample class will get an error
                    File.Create(file).Close();
                }
                catch (Exception ex)
                {
                    //Could not create file
                    Console.WriteLine($"Could not create file: {file}, see error message {ex.Message}");
                    throw;
                }
            }

            return true;
        }
    }
}
