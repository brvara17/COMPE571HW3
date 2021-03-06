﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace COMPE571HW3
{
    class TaskScheduler
    {
        static void Main(string[] args)
        {
            //Comment these two lines out for running program from command line
            //string schedulerType = "EDF EE";
            // textFile = System.IO.File.ReadAllLines(@"input2.txt");

            //Take in arguments from command line accordingingly
            string[] textFile;
            string schedulerType;
            
            if (args.Length > 2)
            {
                schedulerType = args[1] + " " + args[2];
                textFile = System.IO.File.ReadAllLines(@args[0]);
            }
            else
            {
                schedulerType = args[1];
                textFile = System.IO.File.ReadAllLines(@args[0]);
            }

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
                case "EDF":
                    {
                        if (CheckEDF(data))
                        {
                            ScheduleEDF(data); 
                        }
                        else
                        {
                            Console.WriteLine("\nEDF cannot be scheduled.");
                        }
                        break;
                    }
                case "RM":
                    {
                        if (CheckRM(data))
                        {
                            ScheduleRM(data);
                        }
                        else
                        {
                            Console.WriteLine("\nRM cannot be scheduled.");
                        }
                        break;
                    }
                case "EDF EE":
                    {
                        if (CheckEDF(data))
                        {
                            ScheduleEDFEE(data);
                        }
                        else
                        {
                            Console.WriteLine("EDF EE cannot be scheduled.");
                        }
                        break;
                    }
                case "RM EE":
                    {
                        if (CheckRM(data))
                        {
                            ScheduleRMEE(data);
                        }
                        else
                        {
                            Console.WriteLine("RM EE cannot be scheduled.");
                        }
                        break;
                    }
                default: Console.WriteLine("Could not find the correct scheduler."); break;
            }

            Console.WriteLine("\n\nPress Enter to exit:");
            Console.ReadLine();
        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleEDF(List<List<string>> data)
        {
            
            EDFScheduler EDFTask = new EDFScheduler();
            EDFTask.EDFAnalysis(data);
            

        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleRM(List<List<string>> data)
        {

            RMScheduler RMTask = new RMScheduler();
            RMTask.RMAnalysis(data);

        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleEDFEE(List<List<string>> data)
        {

            EDFEEScheduler EDFEETask = new EDFEEScheduler();
            EDFEETask.EDFEEAnalysis(data);


        }

        //Each Task Scheduler should call its own class to perform the scheduling alogrithm
        static void ScheduleRMEE(List<List<string>> data)
        {

            RMEEScheduler RMEETask = new RMEEScheduler();
            RMEETask.RMEEAnalysis(data);


        }

        /// <summary>
        /// Takes in the data object that holds all values from input file and parses 
        /// to get the Deadline and Execution Time
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<List<int>> GetData(List<List<string>> data, int timeToExecute)
        {
            List<List<int>> TaskList = new List<List<int>>();

            foreach (List<string> s in data)
            {
                List<int> tempList = new List<int>();

                //Creating lists for each task with their deadlines and time to execute
                //For the whole time sequence.
                tempList.Add(0);
                tempList.Add(Convert.ToInt32(s[2]));
                if(tempList[0] < timeToExecute)
                {
                    //This new equation adds the ending task deadline even if it is over 1000 because it is still used to schedule 
                    //each task in the 1000 seconds to run.
                    for(int i = 1; i*Convert.ToInt32(s[1]) < (timeToExecute + Convert.ToInt32(s[1])); i++)
                    {
                        tempList.Add(Convert.ToInt32(s[1]) * i);
                        tempList.Add(tempList[1]);
                    }
                }
                TaskList.Add(tempList);
            }

            return TaskList;
        }

        public static bool CheckEDF(List<List<string>> data)
        {
            double sum = 0;

            for(int i = 1; i < data.Count; i++)
            {
                List<string> s = data[i];
                sum += Convert.ToDouble(s[2]) / Convert.ToDouble(s[1]);
            }

            //If sum <=1 then the EDF cannot be scheduled
            if (sum <= 1)
                return true;
            else
                return false;

        }
        public static bool CheckRM(List<List<string>> data)
        {
            double sum = 0;
            double numOfTasks = Convert.ToDouble(data[0][0]);
            double utilization = 0;

            for (int i = 1; i < data.Count; i++)
            {
                List<string> s = data[i];
                sum += Convert.ToDouble(s[2]) / Convert.ToDouble(s[1]);
            }

            utilization =  numOfTasks * ((Math.Pow(2, 1 / numOfTasks)) - 1);

            //If sum <=1 then the RM cannot be scheduled
            if (sum <= utilization)
                return true;
            else
                return false;

        }
    }
}
