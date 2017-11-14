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

        
            scheduleEDF(data);


            Console.WriteLine("finishedScheduler");
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
        private void scheduleEDF(List<List<string>> data)
        {
            int []edfSchedule = new int[520];

            for(int i = 0; i < 520; i++)
            {
                

            }
        }
    }
}
