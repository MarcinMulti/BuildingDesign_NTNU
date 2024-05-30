using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterThesisHege.Classes
{
    public class Section
    {
        public string type;
        public double width;
        public double height;
        public double thickness;
        public string name;
        public int id;




        public static Section GetFloorSection()
        {
            Section sec = new Section();
            sec.type = "floor";
            sec.width = 0;
            sec.height = 20;
            sec.thickness = 0;
            sec.name = "concrete";
            sec.id = 0;

            return sec;
        }




        public static Section GetWallSection()
        {
            Section sec = new Section();
            sec.type = "wall";
            sec.width = 0;
            sec.height = 20;
            sec.thickness = 0;
            sec.name = "concrete";
            sec.id = 1;

            return sec;
        }




        public static Section GetColumnSection()
        {
            Section sec = new Section();
            sec.type = "column";
            sec.width = 20;
            sec.height = 20;
            sec.thickness= 0;
            sec.name = "concrete";
            sec.id = 2;

            return sec;
        }




        public static Section GetBeamSection()
        {
            Section sec = new Section();
            sec.type = "beam";
            sec.width = 20;
            sec.height = 20;
            sec.thickness = 0;
            sec.name = "concrete";
            sec.id = 3;

            return sec;
        }
    }
}
