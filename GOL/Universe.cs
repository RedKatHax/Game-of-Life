using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    class Universe
    {
        Cell[,] universe;
        public Cell this[int index1, int index2]
        {
            get
            {
                return universe[index1, index2];
            }
            set
            {
                universe[index1, index2] = value;
            }
        }
    }
}
