using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic
{
    internal class Timer
    {
        public int id { get; set; }
        private int Flag;
        public int flag
        {
            get { return Flag; }
            set { Flag = value; }
        }

        public Timer() { }

        public Timer(int Flag)
        {
            this.Flag = Flag;
        }

        public override string ToString()
        {
            return "Timer " + Flag;
        }

    }
}
