using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eto.Forms;
using GH_IO.Serialization;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace MasterThesisHege.Classes.Elements
{
    public class Floor
    {
        public Brep geometry;
        public Brep geometryCS;
        public Material material;
        public Section section;




        public static List<Floor> GetFloors(List<Brep> breps, List<List<List<Point3d>>> allNodes)
        {
            List<Floor> floors = new List<Floor>();
            Vector3d zvec = new Vector3d(0, 0, 1);
            zvec.Unitize();

            if (breps[0].Surfaces.Count == 1)
            {
                for (int i = 1; i < allNodes.Count; i++)
                {
                    Floor floor = new Floor();
                    Brep flr = breps[0].Faces[0].DuplicateFace(true);
                    double zVal = allNodes[i][0][0].Z;
                    flr.Translate(zvec * zVal);
                    floor.geometry = flr;

                    Material mat = Material.GetMaterial();
                    floor.material = mat;

                    Section sec = Section.GetFloorSection();
                    floor.section = sec;

                    floors.Add(floor);
                }
            }

            else if (breps[0].Surfaces.Count > 1)
            {
                List<double> lowerLim = new List<double>();
                List<double> upperLim = new List<double>();
                List<Surface> lowerSrf = new List<Surface>();
                for (int i = 0; i < breps.Count; i++)
                {
                    double lowVal = 0;
                    double uppVal = 0;
                    Surface lowSrf = null;
                    Point3d cpt = AreaMassProperties.Compute(breps[i]).Centroid;
                    for (int j = 0; j < breps[i].Surfaces.Count; j++)
                    {
                        Point3d cptSrf = AreaMassProperties.Compute(breps[i].Surfaces[j]).Centroid;
                        if (cptSrf.Z < cpt.Z)
                        {
                            lowVal = cptSrf.Z;
                            lowSrf = breps[i].Surfaces[j];
                        }
                        else if (cptSrf.Z > cpt.Z)
                        {
                            uppVal = cptSrf.Z;
                        }
                    }
                    lowerLim.Add(lowVal);
                    upperLim.Add(uppVal);
                    lowerSrf.Add(lowSrf);
                }


                for (int i = 1; i < allNodes.Count; i++)
                {
                    double zVal = allNodes[i][0][0].Z;

                    List<Brep> flr = new List<Brep>();
                    for (int j = 0; j < breps.Count; j++)
                    {
                        if (lowerLim[j] < zVal & upperLim[j] > zVal)
                        {
                            Brep brp = lowerSrf[j].ToBrep();
                            brp.Translate(-zvec * lowerLim[j]);
                            flr.Add(brp);
                        }
                    }


                    if (flr.Count <= 1)
                    {
                        Floor floor = new Floor();

                        Brep fl = new Brep();
                        fl = flr[0];

                        fl.Translate(zvec * zVal);
                        floor.geometry = fl;

                        Material mat = Material.GetMaterial();
                        floor.material = mat;

                        Section sec = Section.GetFloorSection();
                        floor.section = sec;

                        floors.Add(floor);
                    }

                    else if (flr.Count > 1)
                    {
                        Plane pln;
                        flr[0].Surfaces[0].TryGetPlane(out pln);

                        Brep[] F = Brep.CreatePlanarUnion(flr, pln, 0.001);

                        foreach (Brep brp in F)
                        {
                            Floor floor = new Floor();

                            Brep fl = new Brep();
                            fl = brp;

                            fl.Translate(zvec * zVal);
                            floor.geometry = fl;

                            Material mat = Material.GetMaterial();
                            floor.material = mat;

                            Section sec = Section.GetFloorSection();
                            floor.section = sec;

                            floors.Add(floor);
                        }
                    }
                }
            }
            
            return floors;
        }




        public static Brep CreateCSGeometry(Brep surface, double thickness)
        {
            Vector3d zvec = new Vector3d(0, 0, 1);
            zvec.Unitize();
            double T = thickness / 2;

            Brep face1 = surface.DuplicateBrep();
            Brep face2 = surface.DuplicateBrep();

            face1.Translate(zvec * T);
            face2.Translate(zvec * -T);

            List<Brep> breps = new List<Brep>();
            breps.Add(face1);
            breps.Add(face2);

            for (int i = 0; i < face1.Edges.Count; i++)
            {
                List<Curve> crvs = new List<Curve>();
                crvs.Add(face1.Edges[i]);
                crvs.Add(face2.Edges[i]);
                Brep face3 = Brep.CreateFromLoft(crvs, Point3d.Unset, Point3d.Unset, LoftType.Normal, false)[0];
                breps.Add(face3);
            }

            Brep floorCS = Brep.JoinBreps(breps, 0.0001)[0];
            
            return floorCS;
        }
    }
}
