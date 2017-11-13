using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPE571HW3
{
    class MathLCM
    {
        /// <summary>
        /// Finds the LCM of two integers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>LCM of both input parameters</returns>
        public static int findLCM(int a, int b) 
        {
            int num1, num2;                         
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i <= num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num2;
        }
    }
}
