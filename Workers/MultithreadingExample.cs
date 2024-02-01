using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympusExample.Workers
{
    public class MultithreadingExample : IMultithreadingExample
    {
        private readonly IConfiguration _config;

        private FileStream file;
        private StreamWriter writer;
        private object mutex;

        private static int row = 0;

        public MultithreadingExample()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public async void DoWork()
        {
            string logFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
            string fileName = Path.Combine(logFileDirectory, "out.txt");

            try
            {
                file = new FileStream(fileName, FileMode.OpenOrCreate);
                writer = new StreamWriter(file);
                mutex = new object();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Filestream error see message. {ex.Message}");
                throw;
            }

            Console.WriteLine($"{fileName}");

            int maxNumberOfThreads = Convert.ToInt32(_config["maxNumberOfThreads"]);

            DateTime currentDate = DateTime.Now;

            writer.Write($"0, 0, {currentDate.ToString("HH:mm:ss:fff")}\n");

            ConcurrentBag<Task> ts = new ConcurrentBag<Task>();

            for (int i = 0; i < maxNumberOfThreads; i++)
            {
                ts.Add(Task.Factory.StartNew(new Action(InsertTime)));
            }

            Task.WaitAll(Task.WhenAll(ts));

            writer.Dispose();

            //Make sure only characters are pressed to exit application
            Console.WriteLine("Press a character key to exit");

            bool waitforKeyPress = true;

            while (waitforKeyPress)
            {
                var keyPressed = Console.ReadKey().KeyChar;

                if (char.IsLetter(keyPressed))
                {
                    waitforKeyPress = false;
                }
            }

            //await GetKeyPressed();

            Console.WriteLine("Application closed");
        }

        //private Task GetKeyPressed()
        //{
        //    bool waitforKeyPress = true;

        //    while (waitforKeyPress)
        //    {
        //        var keyPressed = Console.ReadKey().KeyChar;

        //        if (char.IsLetter(keyPressed))
        //        {
        //            waitforKeyPress = true;
        //        }
        //    }
        //}

        private void InsertTime()
        {
            int maxNumberOfLoops = Convert.ToInt32(_config["maxNumberOfLoops"]);

            for (int i = 0; i < maxNumberOfLoops; i++)
            {
                // Criteria number 2 <current_time_stamp> is a string of the form HH:MM:SS.mmm (HH = hours, MM = minutes, SS=seconds, mmm = milliseconds to 3 decimal places.)
                //is wrong as MM = months mm= minutes, SS is not valid needs to be ss, mmm is also not valid fff = miliseconds to 3 decimals
                Write($"{Task.CurrentId}, {DateTime.Now.ToString("HH:mm:ss:fff")}");
            }
        }

        public void Write(string message)
        {
            lock (mutex)
            {
                try
                {
                    writer.Write($"\n{Interlocked.Increment(ref row)}, {message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not write to file. See error message: {ex.Message}");
                    throw;
                }
                
            }
        }
    }
}
