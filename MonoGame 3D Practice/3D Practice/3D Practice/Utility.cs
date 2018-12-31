using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Practice
{
    public static class Utility
    {
        public static bool IsApproximate(float val1, float val2, float val)
        {
            float diff = Math.Abs(val1 - val2);

            return (diff <= val);
        }
    }
}
