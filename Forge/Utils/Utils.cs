using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forge
{
    internal static class Utils
    {
        //Wrap angle between 0 and 360 degrees
        public static double WrapAngle(double x)
        {
            while (x >= 360)
            {
                x -= 360;
            }
            while (x < 0)
            {
                x += 360;
            }
            return x;
        }

        //Limit angle between min and max
        public static double LimitAngle(double x, double min, double max)
        {
            if (x < min)
            {
                return min;
            } else if (x > max)
            {
                return max;
            } else
            {
                return x;
            }
        }
    }
}
