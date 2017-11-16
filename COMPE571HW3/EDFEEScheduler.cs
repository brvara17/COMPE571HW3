using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPE571HW3
{
    class EDFEEScheduler:EDFScheduler
    {
       public void EDFEEAnalysis(List<List<string>> data)
            {
            //GetCombinationSample();
            //TaskScheduler.GetData(data);
            //FindCombinations(data);
            Console.WriteLine("Running EDF Analysis:");

            //Storing relevant General information for the system in the generalTaskInformation var
            //Has <# of tasks> <amount of time to execute in seconds> 
            //<active power @ 1188 Mhz > < active power @ 918 Mhz > 
            //<active power @ 648 Mhz > < active power @ 384 Mhz > < idle power @ lowest frequency>
            var generalTaskInformaion = data[0];

            //Gets total number of tasks in the system.
            var numberOfTasks = Convert.ToInt32(generalTaskInformaion[0]);
            Console.WriteLine("Total Tasks = " + generalTaskInformaion[0]);
            data.RemoveAt(0);

            //Number of tasks and Time to execute to be used in scheduling
            //Defined as time to execute
            int[] array = ToArray(data, numberOfTasks);

            List<int> TaskPosibilities = new List<int>();
            GetCombinationSample(array);
            //FindCombinations(array, 0, TaskPosibilities);

            var timeToExecute = Convert.ToInt32(generalTaskInformaion[1]);

            //Formats task data in usable format.
           // var taskList = TaskScheduler.GetData(data);

            //Schedules all tasks in the system
          //  int[] arrayEDFSchedule = this.scheduleEDF(taskList, numberOfTasks, timeToExecute);

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

        public override void PrintEDFSchedule(int[] arrayEDFSchedule, int timeToExecute)
        {
            base.PrintEDFSchedule(arrayEDFSchedule, timeToExecute);
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
