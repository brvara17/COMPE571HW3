using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace COMPE571HW3
{
    class Program
    {
        static void Main(string[] textFile)
        {
            //Comment these two lines out for running program from command line
            string schedulerType = "EDF";
            textFile = System.IO.File.ReadAllLines(@"D:\Visual Studio\Projects\COMPE571HW3\COMPE571HW3\bin\Debug\input.txt");

            //Used to hold all data from input file.
            List<List<string>> data = new List<List<string>>();

            if (textFile != null)
            {
                foreach (string line in textFile)
                {
                    data.Add(line.Split(' ').ToList());
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Text File is empty please retry");
            }

            switch(schedulerType)
            {
                case "EDF": scheduleEDF(data); break;
                case "RM": scheduleRM(data); break;
                default: Console.WriteLine("Could not find the correct scheduler."); break;
            }


            Console.WriteLine("\nPress Enter to exit:");
            Console.ReadLine();
        }

        static void scheduleEDF(List<List<string>> data)
        {
            Console.WriteLine("Schedule EDF: ");

        }

        static void scheduleRM(List<List<string>> data)
        {

        }
    }
}
