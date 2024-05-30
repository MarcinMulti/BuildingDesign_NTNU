using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterThesisHege.Classes.Elements;

namespace MasterThesisHege.Classes
{
    internal class Emissions
    {
        public static double GetCO2Emission(string material, string classification)
        {
            double emission = 0;

            if (material == "concrete")
            {
                List<string> HDclass = new List<string>() { "HD265mm C30/37", "HD265mm C30/37 LB", "HD265mm C30/37 LA", "HD265mm C40/50", "HD265mm C40/50 LB", "HD265mm C40/50 LA"};
                List<double> HDCO2 = new List<double>() { 153.03, 139.60, 126.17, 206.37, 188.46, 170.55 };

                List<string> Prefabclass = new List<string>() { "Prefab C30/37", "Prefab C30/37 LB", "Prefab C30/37 LA", "Prefab C40/50", "Prefab C40/50 LB", "Prefab C40/50 LA" };
                List<double> PrefabCO2 = new List<double>() { 305.68, 282.33, 258.98, 398.41, 367.28, 336.14 };

                List<string> CIPclass1 = new List<string>() { "Mager B10", "Plasstøpt B20", "Plasstøpt B20LB", "Plasstøpt B20LA", "Plasstøpt B25", "Plasstøpt B25LB", "Plasstøpt B25LA", "Plasstøpt B30", "Plasstøpt B30LB", "Plasstøpt B30LA", "Plasstøpt B30LP", "Plasstøpt B30LE", "Plasstøpt B35", "Plasstøpt B35LB", "Plasstøpt B35LA", "Plasstøpt B35LP", "Plasstøpt B35LE" };
                List<double> CIPCO21 = new List<double>() { 184.82, 240.00, 190.00, 170.00, 260.00, 210.00, 180.00, 280.00, 230.00, 200.00, 150.00, 110.00, 330.00, 280.00, 210.00, 160.00, 120.00 };

                List<string> CIPclass2 = new List<string>() { "Plasstøpt B45", "Plasstøpt B45LB", "Plasstøpt B45LA", "Plasstøpt B45LP", "Plasstøpt B45LE", "Plasstøpt B55", "Plasstøpt B55LB", "Plasstøpt B55LA", "Plasstøpt B55LP", "Plasstøpt B55LE", "Plasstøpt B65", "Plasstøpt B65LB", "Plasstøpt B65LA", "Plasstøpt B65LP", "Plasstøpt B65LE" };
                List<double> CIPCO22 = new List<double>() { 360.00, 290.00, 220.00, 170.00, 130.00, 370.00, 300.00, 230.00, 180.00, 140.00, 380.00, 310.00, 240.00, 190.00, 150.00 };

                List<string> classes = new List<string>();
                List<double> CO2 = new List<double>();
                for (int i = 0; i < HDclass.Count; i++)
                {
                    classes.Add(HDclass[i]);
                    CO2.Add(HDCO2[i]);
                }
                for (int i = 0; i < Prefabclass.Count; i++)
                {
                    classes.Add(Prefabclass[i]);
                    CO2.Add(PrefabCO2[i]);
                }
                for (int i = 0; i < CIPclass1.Count; i++)
                {
                    classes.Add(CIPclass1[i]);
                    CO2.Add(CIPCO21[i]);
                }
                for (int i = 0; i < CIPclass2.Count; i++)
                {
                    classes.Add(CIPclass2[i]);
                    CO2.Add(CIPCO22[i]);
                }


                for (int i = 0; i < classes.Count; i++)
                {
                    if (classes[i] == classification)
                    {
                        emission = CO2[i];
                    }
                }
            }

            else if (material == "steel")
            {
                List<string> classes = new List<string>() { "S355 VF", "S355 VF20", "S355 VF40", "S355 VF60", "S355 VF80", "S355 VF90", "S355 KF10", "S355 KF15", "S355 KF20", "S355 KF30" };
                List<double> CO2 = new List<double>() { 24849.16, 19312.25, 17702.94, 16300.35, 10350.00, 5452.37, 28383.15, 22436.74, 21272.45, 20077.20 };

                for (int i = 0; i < classes.Count; i++)
                {
                    if (classes[i] == classification)
                    {
                        emission = CO2[i];
                    }
                }
            }

            else if (material == "timber")
            {
                List<string> classes = new List<string>() { "C14", "C16", "C18", "C22", "C24", "C27","C30","C35","C40", "GL24c", "GL24h", "GL28c", "GL28h", "GL32c", "GL32h", "GL36c", "GL36h" };
                List<double> CO2 = new List<double>() { 53, 53, 53, 53, 53, 53, 53, 53, 53, 72, 72, 72, 72, 72, 72, 72, 72 };

                for (int i = 0; i < classes.Count; i++)
                {
                    if (classes[i] == classification)
                    {
                        emission = CO2[i];
                    }
                }
            }

            return emission;
        }
    }
}
