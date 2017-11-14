using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace COMPE571HW3
{
    class TaskScheduler
    {
        static void Main(string[] textFile)
        {
            //Comment these two lines out for running program from command line
            string schedulerType = "EDF";
            textFile = System.IO.File.ReadAllLines(@"input.txt");

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
                case "EDF": ScheduleEDF(data); break;
                case "RM": ScheduleRM(data); break;
                default: Console.WriteLine("Could not find the correct scheduler."); break;
            }

            Console.WriteLine("Processing and completing Schedule...");

            Console.WriteLine("\nPress Enter to exit:");
            Console.ReadLine();
        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleEDF(List<List<string>> data)
        {
            
            EDFScheduler EDFTask = new EDFScheduler();
            EDFTask.EDFAnalysis(data);
            Console.WriteLine("Schedule EDF: ");
            

        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleRM(List<List<string>> data)
        {



        }
    }
}
