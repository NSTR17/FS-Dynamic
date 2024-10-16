using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic
{
    internal class Result
    {
        public int id { get; set; }
        private string TeamName, Time, Overall;
        private int Busts, Skips;

        public string teamname 
        { 
          get { return TeamName; }
           set{ TeamName = value; } 
        }

        public string time
        {
            get { return Time; }
            set { Time = value; }
        }
        public int busts
        {
            get { return Busts; }
            set { Busts = value; }
        }
        public int skips
        {
            get { return Skips; }
            set { Skips = value; }
        }
        public string overall
        {
            get { return Overall; }
            set { Overall = value; }
        }



        public Result() { }

        public Result(string TeamName, string Time, int Busts, int Skips, string Overall)
        {
            this.TeamName = TeamName;
            this.Time = Time;
            this.Busts = Busts;
            this.Skips = Skips;
            this.Overall = Overall;
        }

        public override string ToString()
        { 
        return TeamName + " " + Time;
        }

    } 
}
