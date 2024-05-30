using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterThesisHege.Classes.Elements;

namespace MasterThesisHege.Classes
{
    public class Building
    {
        public List<Floor> floors;
        public List<Wall> walls;
        public List<Column> columns;
        public List<Beam> beams;
        public List<Support> supports;
    }
}
