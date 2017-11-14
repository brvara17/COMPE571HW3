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

            Console.Write(" Task List 0 of 0 =  " + taskList[0][1]);

            Console.WriteLine("finishedScheduler");
        }

        private void scheduleRM(List<List<string>> data)
        {
            var rmSchedule = new int[1000];

            for (int i = 0; i < 1000; i++)
            {


            }
        }
    }
}
