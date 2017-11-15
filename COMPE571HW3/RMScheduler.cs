using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPE571HW3
{
    class RMScheduler
    {
        /// <summary>
        /// Will take in data related to power and execution time of tasks in a system.
        /// It will then calculate the EDF schedule for the tasks in the system. 
        /// </summary>
        /// <param name="data">Task Relevant Information</param>
        public void RMAnalysis(List<List<string>> data)
        {
            Console.WriteLine("Running RM Analysis:");

            //Storing relevant General information for the system in the generalTaskInformation var
            //Has <# of tasks> <amount of time to execute in seconds> 
            //<active power @ 1188 Mhz > < active power @ 918 Mhz > 
            //<active power @ 648 Mhz > < active power @ 384 Mhz > < idle power @ lowest frequency>
            var generalTaskInformaion = data[0];
            Console.WriteLine("ALL DATA   = " + generalTaskInformaion[0]);

            Console.WriteLine("totalTasks = " + generalTaskInformaion[0]);
            data.RemoveAt(0);

            //Finding hyper period of all tasks in the system
            //Defined as time to execute
            int hyperPeriod = 1000;
            int numberOfTasks = Convert.ToInt32(generalTaskInformaion[0]);


            var taskList = TaskScheduler.GetData(data);
            scheduleRM(taskList);

            Console.Write(" Task List 0 of 0 =  " + taskList[0][1]);

            Console.WriteLine("finishedScheduler");
        }

        private int[] scheduleRM(List<List<int>> data)
        {
            int[] edfSchedule = new int[1000];
            var rmSchedule = new int[1000];
            int capacity = data.Capacity;

            int compareFlag;
            List<int> taskTemp = new List<int>();
            int minDeadlineTaskNumber = 0;
            int minDeadlineTask = int.MaxValue;
            int tempMinDeadlineTask = int.MaxValue;
            int taskCounter = 0;
            int[] taskFirstRunThru = new int[5];

            //Checking all deadlines to determine which task has the earliest deadline 
            //Held in task[0]
            for (int i = 0; i < 1000; i++)
            {
                taskCounter = 0;
                minDeadlineTaskNumber = 0;
                minDeadlineTask = int.MaxValue;

                //Checking all deadlines to determine which task has the earliest deadline 
                //Held in task[0]
                foreach (List<int> task in data)
                {
                    //TODO remove this statement
                    if (i < task[0])
                    {
                        //Finds minDeadlineTask for all tasks in the system
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

                if (!(minDeadlineTask == int.MaxValue)) //&& !((taskFirstRunThru[minDeadlineTaskNumber] == 1) && (i < taskList[minDeadlineTaskNumber][0])))
                {
                    edfSchedule[i] = minDeadlineTaskNumber + 1;

                    //if the task has no more time to be executed then remove that task from the system.
                    if (data[minDeadlineTaskNumber][1] - 1 == 0) //grabs the tasks corresponding execution time
                    {
                        //Removes task from system
                        data[minDeadlineTaskNumber].RemoveRange(0, 2);

                    }
                    else
                    {
                        //Subtract time to execute by 1 for task that just executed
                        data[minDeadlineTaskNumber][1]--;
                    }


                }

            }
            return edfSchedule;
        }
        }
    }
