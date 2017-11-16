using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPE571HW3
{
    class EDFEEScheduler:EDFScheduler
    {
        //Holds all deadlines in the system
        List<int> listDeadlines = new List<int>();
        List<List<int>> listCPUPowerPerTask = new List<List<int>>();
        List<List<int>> listTaskETUsed = new List<List<int>>();
        List<string> cpuFrequency = new List<string>(new string[] { "1188MHz", "918MHz", "648MHz", "384MHz" });
        static int timeToExecute = new int();
        int IdleActivePower = new int();


        public void EDFEEAnalysis(List<List<string>> data)
            {
            //GetCombinationSample();
            timeToExecute = Convert.ToInt32(data[0][1]);
            TaskScheduler.GetData(data, timeToExecute);
            //FindCombinations(data);
            Console.WriteLine("Running EDF EE Analysis:");

            //Storing relevant General information for the system in the generalTaskInformation var
            //Has <# of tasks> <amount of time to execute in seconds> 
            //<active power @ 1188 Mhz > < active power @ 918 Mhz > 
            //<active power @ 648 Mhz > < active power @ 384 Mhz > < idle power @ lowest frequency>
            List<string> generalTaskInformation = data[0];
            IdleActivePower = Convert.ToInt32(generalTaskInformation[generalTaskInformation.Count - 1]);
            //Gets total number of tasks in the system.
            var numberOfTasks = Convert.ToInt32(generalTaskInformation[0]);
            Console.WriteLine("Total Tasks = " + numberOfTasks);
            data.RemoveAt(0);
            
            foreach(List<string> ls in data)
            {
                listDeadlines.Add(Convert.ToInt32(ls[1]));
                listCPUPowerPerTask.Add(ls.GetRange(2, 4).ConvertAll(int.Parse));
            }
            listCPUPowerPerTask.Add(generalTaskInformation.GetRange(2, 4).ConvertAll(int.Parse));
            var taskList = TaskScheduler.GetData(data, timeToExecute);
            
            //Number of tasks and Time to execute to be used in scheduling
            //Defined as time to execute
            int[] array = ToArray(data, numberOfTasks);

            List<List<double>> TaskPosibilities = new List<List<double>>();
            //GetCombinationSample(array);
            //FindCombinations(array, 0, TaskPosibilities);
            

            timeToExecute = Convert.ToInt32(generalTaskInformation[1]);

            TaskPosibilities = FindEnergyEfficiencyValues(data, generalTaskInformation);
            //Formats task data in usable format.

            taskList = CreateLowestEDF(TaskPosibilities, data);

            //Schedules all tasks in the system
            int[] arrayEDFEESchedule = this.scheduleEDF(taskList, numberOfTasks, timeToExecute);

            PrintEDFSchedule(arrayEDFEESchedule);

        }
        
        public List<List<double>> FindEnergyEfficiencyValues(List<List<string>> taskList, List<string> generalInformation)
        {
            List<List<double>> energyInts = new List<List<double>>();

            //Execution Time for whole system;
            int tasksInSystem = Convert.ToInt32(generalInformation[0]);
            //int timeToExecute = Convert.ToInt32(generalInformation[1]);

            foreach(List<string> ls in taskList)
            {
                List<double> tempList = new List<double>();
                
                for(int i = 1; i < tasksInSystem; i++)
                {
                    double activePower = Convert.ToDouble(generalInformation[i + 1]);
                    double execTime = Convert.ToDouble(ls[i - 1]);
                    double powerConsumpt = (activePower / timeToExecute) * execTime;
                    tempList.Add(powerConsumpt);
                }
                energyInts.Add(tempList);
            }

            return energyInts;

        }

        private List<List<int>> CreateLowestEDF(List<List<double>> taskPosibilities, List<List<string>> data)
        {
            List<List<int>> tasksInSystem;
            bool systemFailed = true;
            do
            {
                int counter = 0;
                if (!systemFailed)
                {
                    double tempMinTaskEnergy = double.MaxValue;
                    double MinTaskEnergy = double.MaxValue;
                    foreach (List<double> joules in taskPosibilities)
                    {
                        tempMinTaskEnergy = Math.Min(tempMinTaskEnergy, joules.Min());
                        if (MinTaskEnergy > tempMinTaskEnergy)
                        {
                            MinTaskEnergy = tempMinTaskEnergy;
                        }
                    }
                    
                    foreach (List<double> lowJ in taskPosibilities)
                    {
                        int col = lowJ.IndexOf(MinTaskEnergy);
                        if (col != -1)
                        {
                            taskPosibilities[counter][col] = double.MaxValue;
                            break;
                        }
                        else
                        {
                            counter++;
                        }
                    }


                }
                
                tasksInSystem = new List<List<int>>();
                counter = 0;
                foreach (List<double> joules in taskPosibilities)
                {
                    //Find min EnergyConsumption for that task
                    int col = joules.IndexOf(joules.Min());
                    List<int> taskInfo = new List<int>();
                    taskInfo.Add(listDeadlines[counter]);
                    taskInfo.Add(Convert.ToInt32(data[counter][col]));
                    tasksInSystem.Add(taskInfo);
                    counter++;
                }
                listTaskETUsed = tasksInSystem;
                tasksInSystem = GetData(tasksInSystem);

               systemFailed = CheckEDF(tasksInSystem);
            }
            while (!systemFailed);



            return tasksInSystem;
        }

        public static bool CheckEDF(List<List<int>> tasksInSystem)
        {
            double sum = 0;

            for (int i = 1; i < tasksInSystem.Count; i++)
            {
                List<int> s = tasksInSystem[i];
                sum += Convert.ToDouble(s[1]) / Convert.ToDouble(s[2]);
            }

            //If sum <=1 then the EDF cannot be scheduled
            if (sum <= 1)
                return true;
            else
                return false;

        }

        public static List<List<int>> GetData(List<List<int>> data)
        {
            List<List<int>> TaskList = new List<List<int>>();

            foreach (List<int> s in data)
            {
                List<int> tempList = new List<int>();

                //Creating lists for each task with their deadlines and time to execute
                //For the whole time sequence.
                tempList.Add(0);
                tempList.Add(Convert.ToInt32(s[1]));
                if (tempList[0] < timeToExecute)
                {
                    //This new equation adds the ending task deadline even if it is over 1000 because it is still used to schedule 
                    //each task in the 1000 seconds to run.
                    for (int i = 1; i * Convert.ToInt32(s[0]) < (timeToExecute + Convert.ToInt32(s[0])); i++)
                    {
                        tempList.Add(Convert.ToInt32(s[0]) * i);
                        tempList.Add(tempList[1]);
                    }
                }
                TaskList.Add(tempList);
            }

            return TaskList;
        }

        public int[] ToArray(List<List<string>> data, int numberOfTasks)
        {
            
            int counter = 0;
            int[] array = new int[numberOfTasks * 4];
            List<List<int>> dataInt = new List<List<int>>();
            string space = "";
            foreach(List<string> ls in data)
            {
                ls.RemoveRange(0,2);

                //ls = ls.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                ls.RemoveAll(string.IsNullOrWhiteSpace);
                List<int> intList = ls.ConvertAll(int.Parse);
                dataInt.Add(intList);
                counter++;
            }

            int initialize = 0;
            counter = 0;
            foreach(List<int> iL in dataInt)
            {
                for(int i = 0; i<4;i++)
                {
                        array[counter] = iL[i];
                        counter++;
                    
                }
            }

            return array;
        }

        /// <summary>
        /// Prints elements in array in following format:
        /// Task    Frequency   Execution Time  Total Time
        /// </summary>
        /// <param name="arrayEDFSchedule"></param>
        public void PrintEDFSchedule(int[] arrayEDFSchedule)
        {
            int tempArrayEDF = 0;
            int counter = -1;
            int totalTime = 0;
            double activePower;
            double energyConsumption;
            double totalEnergyConsumption = 0;
            double totalIdleTime = 0;

            Console.WriteLine("Task   Frequency     Execution Time    Total Time    Energy Consumed(J)");
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

                    List<int> tempCPUList = listCPUPowerPerTask[tempArrayEDF-1];
                    int col = listTaskETUsed[tempArrayEDF-1][1];
                    col = listCPUPowerPerTask[tempArrayEDF-1].IndexOf(col);
                    activePower = listCPUPowerPerTask[5][col];
                    energyConsumption = activePower / timeToExecute;
                    

                    if (tempArrayEDF == -1)
                    {
                        Console.WriteLine("IDLE" + "  IDLE                  " + counter + "            " + totalTime + "            " + counter * (IdleActivePower / timeToExecute));//TODO add dynamic J calc
                        totalEnergyConsumption += counter * (IdleActivePower / timeToExecute);
                        totalIdleTime += counter;
                    }
                    else
                    {
                        Console.WriteLine("w" + tempArrayEDF + "    " + cpuFrequency[col] + "              " + counter + "            " + totalTime + "           " + counter * energyConsumption);
                        totalEnergyConsumption += counter * energyConsumption;
                    }
                        counter = 0;
                }
                tempArrayEDF = arrayEDFSchedule[i];

                //print last set for array
                if (i == (timeToExecute - 1))
                {
                    counter++;
                    totalTime += counter;
                    List<int> tempCPUList = listCPUPowerPerTask[tempArrayEDF];
                    int col = listTaskETUsed[tempArrayEDF][1];
                    col = listCPUPowerPerTask[tempArrayEDF].IndexOf(col);
                    activePower = listCPUPowerPerTask[tempArrayEDF][col];
                    energyConsumption = activePower / timeToExecute;
                    if (tempArrayEDF == -1)
                    {
                        Console.WriteLine("IDLE" + "  IDLE                  " + counter + "            " + totalTime + "            " + counter * (IdleActivePower / timeToExecute));
                        totalEnergyConsumption += counter * (IdleActivePower / timeToExecute);
                        totalIdleTime += counter;
                    }
                    else
                    {
                        Console.WriteLine("w" + tempArrayEDF + "    " + cpuFrequency[col] + "              " + counter + "            " + totalTime + "           " + counter * energyConsumption);
                        totalEnergyConsumption += counter * energyConsumption;
                    }

               }
            }

            Console.WriteLine("Total Energy Consumption = " + totalEnergyConsumption + "J");
            Console.WriteLine("Percent Idle Time vs Total Time = " + totalIdleTime / timeToExecute + "%");
            Console.WriteLine("Total Execution Time = " + timeToExecute + " seconds");
        }




        static List<List<int>> comb;
        static bool[] used;
        void GetCombinationSample( int[] arr)
        {
            //int[] arr = { 10, 50, 3, 1, 2 };
            used = new bool[arr.Length];
            used.Initialize();
            comb = new List<List<int>>();
            List<int> c = new List<int>();
            FindCombinations(arr, 0, c);
            foreach (var item in comb)
            {
                foreach (var x in item)
                {
                    Console.Write(x + ",");
                }
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Gets all combinations of an array and returns it to the user. 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="colindex"></param>
        /// <param name="c"></param>
        private void FindCombinations(int[] arr, int colindex, List<int> c)
        {

            if (colindex >= arr.Length)
            {
                comb.Add(new List<int>(c));
                return;
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    c.Add(arr[i]);
                    FindCombinations(arr, colindex + 1, c);
                    c.RemoveAt(c.Count - 1);
                    used[i] = false;
                }
            }
        }
    }
}
