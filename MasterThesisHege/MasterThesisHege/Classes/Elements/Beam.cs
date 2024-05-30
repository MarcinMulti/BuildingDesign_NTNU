using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MasterThesisHege.Classes.Elements
{
    public class Beam
    {
        public Line axis;
        public Brep geometryCS;
        public Material material;
        public Section section;




        public static List<Beam> GetBeams(List<List<List<Point3d>>> allNodes)
        {
            List<Beam> beams = new List<Beam>();
            for (int i = 1; i < allNodes.Count; i++)
            {
                for (int j = 0; j < allNodes[i].Count - 1; j++)
                {
                    List<Point3d> nodes1 = allNodes[i][j];
                    List<Point3d> nodes2 = allNodes[i][j + 1];

                    if (nodes1.Count == nodes2.Count)
                    {
                        int l = nodes1.Count - 1;

                        Beam beam1 = new Beam();
                        Line bm1 = new Line(nodes1[0], nodes2[0]);
                        beam1.axis = bm1;

                        Material mat = Material.GetMaterial();
                        beam1.material = mat;

                        Section sec = Section.GetBeamSection();
                        beam1.section = sec;

                        beams.Add(beam1);


                        Beam beam2 = new Beam();
                        Line bm2 = new Line(nodes1[l], nodes2[l]);
                        beam2.axis = bm2;

                        beam2.material = mat;
                        beam2.section = sec;

                        beams.Add(beam2);
                    }

                    else if (nodes1.Count < nodes2.Count)
                    {
                        int l = nodes1.Count - 1;

                        int idx = 0;
                        double len = 1000;
                        for (int x = 0; x < nodes2.Count; x++)
                        {
                            double dist = nodes2[x].DistanceTo(nodes1[0]);
                            if (dist < len)
                            {
                                idx = x;
                                len = dist;
                            }
                        }
                        

                        Beam beam1 = new Beam();
                        Line bm1 = new Line(nodes1[0], nodes2[idx]);
                        beam1.axis = bm1;

                        Material mat = Material.GetMaterial();
                        beam1.material = mat;

                        Section sec = Section.GetBeamSection();
                        beam1.section = sec;

                        beams.Add(beam1);


                        Beam beam2 = new Beam();
                        Line bm2 = new Line(nodes1[l], nodes2[idx+l]);
                        beam2.axis = bm2;

                        beam2.material = mat;
                        beam2.section = sec;

                        beams.Add(beam2);
                    }

                    else if (nodes1.Count > nodes2.Count)
                    {
                        int l = nodes2.Count - 1;

                        int idx = 0;
                        double len = 1000;
                        for (int x = 0; x < nodes1.Count; x++)
                        {
                            double dist = nodes1[x].DistanceTo(nodes2[0]);
                            if (dist < len)
                            {
                                idx = x;
                                len = dist;
                            }

                        }


                        Beam beam1 = new Beam();
                        Line bm1 = new Line(nodes1[idx], nodes2[0]);
                        beam1.axis = bm1;

                        Material mat = Material.GetMaterial();
                        beam1.material = mat;

                        Section sec = Section.GetBeamSection();
                        beam1.section = sec;

                        beams.Add(beam1);


                        Beam beam2 = new Beam();
                        Line bm2 = new Line(nodes1[idx+l], nodes2[l]);
                        beam2.axis = bm2;

                        beam2.material = mat;
                        beam2.section = sec;

                        beams.Add(beam2);
                    }
                }

                List<int> idxs = new List<int>();
                List<int> kVals = new List<int>();
                for (int j = 0; j < allNodes[i].Count - 1; j++)
                {
                    if (allNodes[i][j].Count == allNodes[i][j + 1].Count)
                    {

                    }

                    else if (allNodes[i][j].Count < allNodes[i][j + 1].Count)
                    {
                        idxs.Add(j + 1);

                        List<Point3d> ptsShort = allNodes[i][j];
                        List<Point3d> ptsLong = allNodes[i][j + 1];

                        double l = 1000;
                        List<int> idxsLong = new List<int>();
                        for (int x = 0; x < ptsShort.Count; x++)
                        {
                            int idx = 0;
                            for (int y = 0; y < ptsLong.Count; y++)
                            {
                                double dist = ptsShort[x].DistanceTo(ptsLong[y]);
                                if (dist < l)
                                {
                                    l = dist;
                                    idx = y;
                                }
                            }
                            idxsLong.Add(idx);
                        }

                        if (idxsLong[0] > 0)
                        {
                            int last = idxsLong.Count - 1;
                            if (idxsLong[last] == ptsLong.Count - 1)
                            {
                                for (int w = 0; w < idxsLong[0]; w++)
                                {
                                    kVals.Add(w);
                                }
                            }

                            else if (idxsLong[last] < ptsLong.Count - 1)
                            {
                                for (int w = 0; w < idxsLong[0]; w++)
                                {
                                    kVals.Add(w);
                                }

                                int strt = idxsLong[0] + idxsLong.Count - 1;
                                for (int w = strt; w < ptsLong.Count -1; w++)
                                {
                                    kVals.Add(w);
                                }
                            }
                        }

                        else if (idxsLong[0] == 0)
                        {
                            int lenidx = idxsLong.Count - 1;
                            for (int w = lenidx; w < ptsLong.Count - 1; w++)
                            {
                                kVals.Add(w);
                            }
                        }

                    }

                    else if (allNodes[i][j].Count > allNodes[i][j + 1].Count)
                    {
                        idxs.Add(j);

                        List<Point3d> ptsShort = allNodes[i][j + 1];
                        List<Point3d> ptsLong = allNodes[i][j];

                        double l = 1000;
                        List<int> idxsLong = new List<int>();
                        for (int x = 0; x < ptsShort.Count; x++)
                        {
                            int idx = 0;
                            for (int y = 0; y < ptsLong.Count; y++)
                            {
                                double dist = ptsShort[x].DistanceTo(ptsLong[y]);
                                if (dist < l)
                                {
                                    l = dist;
                                    idx = y;
                                }
                            }
                            idxsLong.Add(idx);
                        }

                        if (idxsLong[0] > 0)
                        {
                            int last = idxsLong.Count - 1;
                            if (idxsLong[last] == ptsLong.Count - 1)
                            {
                                for (int w = 0; w < idxsLong[0]; w++)
                                {
                                    kVals.Add(w);
                                }
                            }

                            else if (idxsLong[last] < ptsLong.Count - 1)
                            {
                                for (int w = 0; w < idxsLong[0]; w++)
                                {
                                    kVals.Add(w);
                                }

                                int strt = idxsLong[0] + idxsLong.Count - 1;
                                for (int w = strt; w < ptsLong.Count - 1; w++)
                                {
                                    kVals.Add(w);
                                }
                            }
                        }

                        else if (idxsLong[0] == 0)
                        {
                            int lenidx = idxsLong.Count - 1;
                            for (int w = lenidx; w < ptsLong.Count - 1; w++)
                            {
                                kVals.Add(w);
                            }
                        }
                    }


                    foreach (int a in idxs)
                    {
                        foreach (int b in kVals)
                        {
                            Beam beam = new Beam();
                            Line bm = new Line(allNodes[i][a][b], allNodes[i][a][b + 1]);
                            beam.axis = bm;

                            Material mat = Material.GetMaterial();
                            beam.material = mat;

                            Section sec = Section.GetBeamSection();
                            beam.section = sec;

                            beams.Add(beam);
                        }
                    }
                    idxs = new List<int>();
                    kVals = new List<int>();
                }

                int jcount = allNodes[i].Count - 1;
                for (int c = 0; c < allNodes[i].Count; c+=jcount)
                {
                    for (int d = 0; d < allNodes[i][c].Count-1; d++)
                    {
                        Beam beam = new Beam();
                        Line bm = new Line(allNodes[i][c][d], allNodes[i][c][d + 1]);
                        beam.axis = bm;

                        Material mat = Material.GetMaterial();
                        beam.material = mat;

                        Section sec = Section.GetBeamSection();
                        beam.section = sec;

                        beams.Add(beam);
                    }
                }
            }
            return beams;
        }




        public static Brep CreateCSGeometry(Line axis, double width, double height, double thickness, string material)
        {
            Interval i = axis.ToNurbsCurve().Domain;

            Plane pln1 = axis.ToNurbsCurve().GetPerpendicularFrames(new List<double>() { i.Min, i.Max })[0];
            Plane pln2 = axis.ToNurbsCurve().GetPerpendicularFrames(new List<double>() { i.Min, i.Max })[1];

            Interval iw = new Interval(-width / 2, width / 2);
            Interval ih = new Interval(-height / 2, height / 2);

            Rectangle3d rec1 = new Rectangle3d(pln1, iw, ih);
            Rectangle3d rec2 = new Rectangle3d(pln2, iw, ih);

            Brep face1 = Brep.CreatePlanarBreps(rec1.ToNurbsCurve(), 0.0001)[0];
            Brep face2 = Brep.CreatePlanarBreps(rec2.ToNurbsCurve(), 0.0001)[0];

            List<Point3d> cornerPts = new List<Point3d>();
            for (int j = 0; j < face1.Edges.Count; j++)
            {
                cornerPts.Add(face1.Edges[j].PointAtStart);
                cornerPts.Add(face2.Edges[j].PointAtStart);
            }

            BoundingBox outerBox = new BoundingBox(cornerPts);
            Brep outerBeamCS = outerBox.ToBrep();


            Brep beamCS = new Brep();
            if (material == "steel")
            {
                double innerWidth = width - (thickness * 2);
                double innerHeight = height - (thickness * 2);

                Interval iIw = new Interval(-innerWidth / 2, innerWidth / 2);
                Interval iIh = new Interval(-innerHeight / 2, innerHeight / 2);

                Rectangle3d rec3 = new Rectangle3d(pln1, iIw, iIh);
                Rectangle3d rec4 = new Rectangle3d(pln2, iIw, iIh);

                Brep face3 = Brep.CreatePlanarBreps(rec3.ToNurbsCurve(), 0.0001)[0];
                Brep face4 = Brep.CreatePlanarBreps(rec4.ToNurbsCurve(), 0.0001)[0];

                List<Point3d> cornrPts = new List<Point3d>();
                for (int j = 0; j < face3.Edges.Count; j++)
                {
                    cornrPts.Add(face3.Edges[j].PointAtStart);
                    cornrPts.Add(face4.Edges[j].PointAtStart);
                }

                BoundingBox innerBox = new BoundingBox(cornrPts);
                Brep innerBeamCS = innerBox.ToBrep();

                Brep[] brepsOut = outerBeamCS.Split(innerBeamCS, 0.01);
                Brep[] brepsIn = innerBeamCS.Split(outerBeamCS, 0.01);
                Brep b1 = brepsOut[2];
                Brep b2 = brepsIn[2];

                List<Brep> breps = new List<Brep>();
                breps.Add(b1);
                breps.Add(b2);

                beamCS = Brep.JoinBreps(breps, 0.01)[0];
            }
            else
            {
                beamCS = outerBeamCS;
            }


            return beamCS;
        }




        public static double GetSteelVolume(double width, double height, double thickness, Line axis)
        {
            double v = 0;

            double length = axis.Length;
            double iwidth = (width - (thickness * 2)) / 100;
            double iheight = (height - (thickness * 2)) / 100;
            height = height / 100;
            width = width / 100;

            double outVol = length * width * height;
            double inVol = length * iwidth * iheight;

            v = outVol - inVol;

            return v;
        }
    }
}
