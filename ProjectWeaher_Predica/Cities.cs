using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeather
{
    public class Cities : IComparer<Cities>
    {
        public int _temperature;
        public string _name;

        public int Compare(Cities x, Cities y)
        {
            if (x._temperature < y._temperature)
                return 1;

            if (x._temperature > y._temperature)
                return -1;

            else
                return 0;
        }
    }
}
