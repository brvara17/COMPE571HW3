using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPE571HW3
{
    class EDFScheduler
    {
        /// <summary>
        /// Will take in data related to power and execution time of tasks in a system.
        /// It will then calculate the EDF schedule for the tasks in the system. 
        /// </summary>
        /// <param name="data">Task Relevant Information</param>
        public void EDFAnalysis(List<List<string>> data)
        {
            Console.WriteLine("Running EDF Analysis:");

            //Storing relevant General information for the system in the generalTaskInformation var
            //Has <# of tasks> <amount of time to execute in seconds> 
            //<active power @ 1188 Mhz > < active power @ 918 Mhz > 
            //<active power @ 648 Mhz > < active power @ 384 Mhz > < idle power @ lowest frequency>
            var generalTaskInformaion = data[0];
            Console.WriteLine("totalTasks = " + generalTaskInformaion[0]);
            data.RemoveAt(0);

            //Finding hyper period of all tasks in the system
            //Defined as time to execute
            int hyperPeriod = 1000;
            int numberOfTasks = Convert.ToInt32(generalTaskInformaion[0]);


            var taskList = TaskScheduler.GetData(data);

        
            int []arrayEDFSchedule = scheduleEDF(taskList);
            int r = 0;
            for(int i = 0; i<1000; i++)
            {
                if(r < 50)
                {
                    Console.Write(arrayEDFSchedule[i] + " ");
                }
                else
                {
                    Console.WriteLine(arrayEDFSchedule[i]);
                    r = 0;
                }
                r++;
            }

            Console.WriteLine("\nfinishedScheduler");
        }

        /// <summary>
        /// Finds the hyper period for all tasks in the system
        /// </summary>
        /// <param name="taskInfo">All specfics related to each task</param>
        /// <returns>Hyper Period of all Tasks in the system</returns>
        public int FindHyperPeriod(List<List<string>> taskInfo)
        {
            int lcm = 1;

            //Runs through each task in the system using the deadline of each
            //task to find the hyperperiod using the lcm function.
            foreach(List<string> s in taskInfo)
            {
                lcm = MathLCM.findLCM(lcm, Convert.ToInt32(s[1]));
            }

            return lcm;
        }


        /// <summary>
        /// Schedules EDF for all tasks with time to execute 1000seconds.
        /// </summary>
        private int[] scheduleEDF(List<List<int>> taskList)
        {
            int []edfSchedule = new int[1000];
            int minDeadlineTask = int.MaxValue;
            int taskCounter = 0;
            int minDeadlineTaskNumber = 0;
            int tempMinDeadlineTask = int.MaxValue;

            for (int i = 0; i < 1000; i++)
            {
                taskCounter = 0;
                minDeadlineTaskNumber = 0;
                minDeadlineTask = int.MaxValue;
                foreach (List<int> task in taskList)
                {
                    if (i < task[0])
                    {
                        tempMinDeadlineTask = Math.Min(minDeadlineTask, task[0]);
                        if (minDeadlineTask > tempMinDeadlineTask)
                        {
                            minDeadlineTask = tempMinDeadlineTask;
                            minDeadlineTaskNumber = taskCounter;
                        }

                        taskCounter++;
                    }
                    else
                    {
                        taskCounter++;
                    }

                 }

                if (!(minDeadlineTask == int.MaxValue))
                {
                    edfSchedule[i] = minDeadlineTaskNumber + 1;
                    if (taskList[minDeadlineTaskNumber][1] - 1 == 0)
                    {
                        if (taskList[minDeadlineTaskNumber].Count > 2)
                        {
                            taskList[minDeadlineTaskNumber].RemoveRange(0, 2);
                        }
                        else
                        {
                            taskList.RemoveAt(minDeadlineTaskNumber);
                        }

                    }
                    else
                    {
                        taskList[minDeadlineTaskNumber][1]--;
                    }


                }
                else
                {
                    edfSchedule[i] = -1;
                }

                //if (taskList.Count == 0)
                //{
                //    i = 1000;
                //}


            }

            return edfSchedule;
        }
    }
}
