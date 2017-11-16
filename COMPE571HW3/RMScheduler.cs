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

            //Number of tasks and Time to execute to be used in scheduling
            //Defined as time to execute
            var timeToExecute = Convert.ToInt32(generalTaskInformaion[1]);

            //Finding hyper period of all tasks in the system
            //Defined as time to execute
            int hyperPeriod = timeToExecute;
            int numberOfTasks = Convert.ToInt32(generalTaskInformaion[0]);


            var taskList = GetData(data, timeToExecute);
            int[] scheduleArray = scheduleRM(taskList, timeToExecute);
            PrintEDFSchedule(scheduleArray, timeToExecute);

            Console.WriteLine("\nFinished RM Scheduler!");
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
                tempList.Add(Convert.ToInt32(s[1]));
                tempList.Add(0);
                tempList.Add(Convert.ToInt32(s[2]));
                if (tempList[0] < timeToExecute)
                {
                    //This new equation adds the ending task deadline even if it is over 1000 because it is still used to schedule 
                    //each task in the 1000 seconds to run.
                    for (int i = 1; i * Convert.ToInt32(s[1]) < (timeToExecute + Convert.ToInt32(s[1])); i++)
                    {
                        tempList.Add(Convert.ToInt32(s[1]) * i);
                        tempList.Add(tempList[2]);
                    }
                }
                TaskList.Add(tempList);
            }

            return TaskList;
        }

        private int[] scheduleRM(List<List<int>> taskList, int timeToExecute)
        {
            //int[] edfSchedule = new int[timeToExecute];
            var rmSchedule = new int[timeToExecute];
            int capacity = taskList.Capacity;
            int compareFlag;
            List<int> taskTemp = new List<int>();
            int minDeadlineTaskNumber = 0;
            int minDeadlineTask = int.MaxValue;
            int tempMinDeadlineTask = int.MaxValue;
            int taskCounter = 0;
            int[] taskFirstRunThru = new int[5];

            //Checking all deadlines to determine which task has the earliest deadline 
            //Held in task[0]
            for (int i = 0; i < timeToExecute; i++)
            {
                taskCounter = 0;
                minDeadlineTaskNumber = 0;
                minDeadlineTask = int.MaxValue;

                //Checking all deadlines to determine which task has the earliest deadline 
                //Held in task[0]
                foreach (List<int> task in taskList)
                {
                    //Statement determines
                    if (i < task[1] && !(taskFirstRunThru[taskCounter] == 1) || i >= task[1])
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
                    rmSchedule[i] = minDeadlineTaskNumber + 1; //Min deadline task number + 1 => increments to the correct task running and adds to scheduled tasks

                    //if the task has no more time to be executed then remove that task from the system.
                    if (taskList[minDeadlineTaskNumber][2] - 1 == 0) //grabs the tasks corresponding execution time
                    {
                        //Removes task from system
                        taskList[minDeadlineTaskNumber].RemoveRange(1, 2);
                        taskFirstRunThru[minDeadlineTaskNumber] = 1;
                    }
                    else
                    {
                        //Subtract time to execute by 1 for task that just executed
                        taskList[minDeadlineTaskNumber][2]--;
                    }


                }
                else
                {
                    //If there isnt a task to be executed put in a -1;
                    rmSchedule[i] = -1;
                }

            }
            return rmSchedule;
        }

        /// <summary>
        /// Prints elements in array in following format:
        /// Task    Frequency   Execution Time  Total Time
        /// </summary>
        /// <param name="arrayEDFSchedule"></param>
        virtual public void PrintEDFSchedule(int[] arrayEDFSchedule, int timeToExecute)
        {
            int tempArrayEDF = 0;
            int counter = -1;
            double totalTime = 0;
            double idleTime = 0;
            double totalEnergyConsumption = 0;
            double totalExecutionTime = 0;

            Console.WriteLine("Task\t\t Frequency\t\t Execution Time\t\t Total Time\t\t Energy Consumed(J)");
            for (int i = 0; i < timeToExecute; i++)
            {
                //For each series found in the array counter++
                if ((arrayEDFSchedule[i] == tempArrayEDF) || (tempArrayEDF == 0))
                {
                    counter++;
                }
                else
                {
                    //Prints counter and total time to promptD
                    counter++;
                    totalTime += counter;
                    if (tempArrayEDF == -1)
                    {
                        idleTime += counter;
                        totalExecutionTime += counter;
                        totalEnergyConsumption += (counter * 0.084);
                        Console.WriteLine("IDLE\t\t " + "IDLE\t" + "\t\t " + counter + " \t\t\t" + totalTime + "\t\t\t" + counter * 0.084);//TODO add dynamic J calc
                    }
                    else
                    {
                        totalExecutionTime += counter;
                        totalEnergyConsumption += (counter * 0.625);
                        Console.WriteLine("w" + tempArrayEDF + "\t\t1188MHz " + "\t\t " + counter + " \t\t\t " + totalTime + "\t\t\t" + counter * 0.625);
                    }
                        counter = 0;
                }
                tempArrayEDF = arrayEDFSchedule[i];

                //print last set for array
                if (i == (timeToExecute - 1))
                {
                    counter++;
                    totalTime += counter;
                    if (tempArrayEDF == -1)
                    {
                        idleTime += counter;
                        totalExecutionTime += counter;
                        totalEnergyConsumption += (counter * 0.084);
                        Console.WriteLine("IDLE\t\t " + "IDLE\t" + "\t\t" + counter + " \t\t\t" + totalTime + "\t\t\t" + counter * 0.084);//TODO add dynamic J calc
                    }
                    else
                    {
                        totalEnergyConsumption += (counter * 0.625);
                        totalExecutionTime += counter;
                        Console.WriteLine("w" + tempArrayEDF + "\t\t1188MHz " + "\t\t" + counter + " \t\t\t " + totalTime + "\t\t\t" + counter * 0.625);
                    }

                }
            }
            Console.WriteLine("\nTotal Energy Consumption =  " + totalEnergyConsumption );
            Console.WriteLine("Total Execution Time = " + totalExecutionTime);
            Console.WriteLine("Percent Idle Time = " + (100 * (float)idleTime/timeToExecute) + "%\n");
           


        }


    }
}
