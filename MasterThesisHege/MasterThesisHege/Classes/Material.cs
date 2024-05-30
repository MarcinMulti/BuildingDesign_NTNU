using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MasterThesisHege.Classes
{
    public class Material
    {
        public string name;
        public string classification;
        public double density;




        public static Material GetMaterial()
        {
            Material mat = new Material();
            mat.name = "concrete";
            mat.classification = "Prefab C30/37";
            mat.density = 2500;

            return mat;
        }
    }
}
