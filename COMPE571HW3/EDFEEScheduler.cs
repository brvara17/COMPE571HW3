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
            FindCombinations(data);

            }
        

        private List<List<string>> FindCombinations(List<List<string>> data)
        {
            return data;
        }

        public override void PrintEDFSchedule(int[] arrayEDFSchedule, int timeToExecute)
        {
            base.PrintEDFSchedule(arrayEDFSchedule, timeToExecute);
        }
    }
}
