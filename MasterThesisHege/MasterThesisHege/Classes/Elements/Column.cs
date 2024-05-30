using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MasterThesisHege.Classes.Elements
{
    public class Column
    {
        public Line axis;
        public Brep geometryCS;
        public Material material;
        public Section section;




        public static List<Column> GetColumns(List<List<List<Point3d>>> allNodes)
        {
            List<Column> columns = new List<Column>();
            for (int i = 0; i < allNodes.Count - 1; i++)
            {
                if (allNodes[i].Count == allNodes[i + 1].Count)
                {
                    for (int j = 0; j < allNodes[i].Count; j++)
                    {
                        List<Point3d> nodes1 = allNodes[i][j];
                        List<Point3d> nodes2 = allNodes[i + 1][j];
                        for (int k = 0; k < nodes1.Count; k++)
                        {
                            for (int l = 0; l < nodes2.Count; l++)
                            {
                                Point3d p = new Point3d(nodes1[k].X, nodes1[k].Y, nodes2[l].Z);
                                if (p == nodes2[l])
                                {
                                    Column column = new Column();
                                    Line col = new Line(nodes1[k], nodes2[l]);
                                    column.axis = col;

                                    Material mat = Material.GetMaterial();
                                    column.material = mat;

                                    Section sec = Section.GetColumnSection();
                                    column.section = sec;

                                    columns.Add(column);
                                }
                            }
                        }
                    }
                }

                else if (allNodes[i].Count != allNodes[i + 1].Count)
                {
                    int count1 = allNodes[i].Count;
                    int count2 = allNodes[i + 1].Count;
                    List<int> count = new List<int>() { count1, count2 };
                    int countMax = count.Max();

                    if (countMax == count1)
                    {
                        List<Point3d> pts = new List<Point3d>();
                        for (int x = 0; x < allNodes[i].Count; x++)
                        {
                            for (int y = 0; y < allNodes[i][x].Count; y++)
                            {
                                pts.Add(allNodes[i][x][y]);
                            }
                        }


                        List<List<Point3d>> lowerRow = new List<List<Point3d>>();
                        for (int a = 0; a < allNodes[i + 1].Count; a++)
                        {
                            List<Point3d> lowerPts = new List<Point3d>();
                            for (int b = 0; b < allNodes[i + 1][a].Count; b++)
                            {
                                for (int c = 0; c < pts.Count; c++)
                                {
                                    double zVal = allNodes[i + 1][a][b].Z;
                                    Point3d p = new Point3d(pts[c].X, pts[c].Y, zVal);

                                    if (allNodes[i + 1][a][b] == p)
                                    {
                                        lowerPts.Add(pts[c]);
                                    }
                                }

                            }
                            lowerRow.Add(lowerPts);
                        }

                        for (int d = 0; d < lowerRow.Count; d++)
                        {
                            List<Point3d> nodes1 = lowerRow[d];
                            List<Point3d> nodes2 = allNodes[i + 1][d];
                            for (int e = 0; e < nodes1.Count; e++)
                            {
                                for (int l = 0; l < nodes2.Count; l++)
                                {
                                    Point3d p = new Point3d(nodes1[e].X, nodes1[e].Y, nodes2[l].Z);
                                    if (p == nodes2[l])
                                    {
                                        Column column = new Column();
                                        Line col = new Line(nodes1[e], nodes2[l]);
                                        column.axis = col;

                                        Material mat = Material.GetMaterial();
                                        column.material = mat;

                                        Section sec = Section.GetColumnSection();
                                        column.section = sec;

                                        columns.Add(column);
                                    }
                                }
                            }
                        }
                    }

                    else if (countMax == count2)
                    {
                        List<Point3d> pts = new List<Point3d>();
                        for (int x = 0; x < allNodes[i + 1].Count; x++)
                        {
                            for (int y = 0; y < allNodes[i + 1][x].Count; y++)
                            {
                                pts.Add(allNodes[i + 1][x][y]);
                            }
                        }


                        List<List<Point3d>> upperRow = new List<List<Point3d>>();
                        for (int a = 0; a < allNodes[i].Count; a++)
                        {
                            List<Point3d> upperPts = new List<Point3d>();
                            for (int b = 0; b < allNodes[i][a].Count; b++)
                            {
                                for (int c = 0; c < pts.Count; c++)
                                {
                                    double zVal = allNodes[i][a][b].Z;
                                    Point3d p = new Point3d(pts[c].X, pts[c].Y, zVal);

                                    if (allNodes[i][a][b] == p)
                                    {
                                        upperPts.Add(pts[c]);
                                    }
                                }

                            }
                            upperRow.Add(upperPts);
                        }

                        for (int d = 0; d < upperRow.Count; d++)
                        {
                            List<Point3d> nodes1 = allNodes[i][d];
                            List<Point3d> nodes2 = upperRow[d];
                            
                            for (int e = 0; e < nodes1.Count; e++)
                            {
                                for (int l = 0; l < nodes2.Count; l++)
                                {
                                    Point3d p = new Point3d(nodes1[e].X, nodes1[e].Y, nodes2[l].Z);
                                    if (p == nodes2[l])
                                    {
                                        Column column = new Column();
                                        Line col = new Line(nodes1[e], nodes2[l]);
                                        column.axis = col;

                                        Material mat = Material.GetMaterial();
                                        column.material = mat;

                                        Section sec = Section.GetColumnSection();
                                        column.section = sec;

                                        columns.Add(column);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return columns;
        }




        public static List<Column> GetOuterColumns(List<List<List<Point3d>>> allNodes)
        {
            List<Column> columns = new List<Column>();
            for (int i = 0; i < allNodes.Count - 1; i++)
            {
                int jcount = allNodes[i].Count - 1;
                if (jcount > 0)
                {
                    for (int j = 0; j < allNodes[i].Count; j += jcount)
                    {
                        List<Point3d> nodes1 = allNodes[i][j];
                        List<Point3d> nodes2 = allNodes[i + 1][j];
                        for (int k = 0; k < allNodes[i][j].Count; k++)
                        {
                            Column column = new Column();
                            Line col = new Line(nodes1[k], nodes2[k]);
                            column.axis = col;

                            Material mat = Material.GetMaterial();
                            column.material = mat;

                            Section sec = Section.GetColumnSection();
                            column.section = sec;

                            columns.Add(column);
                        }
                    }
                }

                else if( jcount == 0)
                {
                    List<Point3d> nodes1 = allNodes[i][0];
                    List<Point3d> nodes2 = allNodes[i + 1][0];
                    for (int k = 0; k < allNodes[i][0].Count; k++)
                    {
                        Column column = new Column();
                        Line col = new Line(nodes1[k], nodes2[k]);
                        column.axis = col;

                        Material mat = Material.GetMaterial();
                        column.material = mat;

                        Section sec = Section.GetColumnSection();
                        column.section = sec;

                        columns.Add(column);
                    }
                }
            }
            return columns;
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
            Brep outerColumnCS = outerBox.ToBrep();


            Brep columnCS = new Brep();
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
                Brep innerColumnCS = innerBox.ToBrep();

                Brep[] brepsOut = outerColumnCS.Split(innerColumnCS, 0.01);
                Brep[] brepsIn = innerColumnCS.Split(outerColumnCS, 0.01);
                Brep b1 = brepsOut[2];
                Brep b2 = brepsIn[2];

                List<Brep> breps = new List<Brep>();
                breps.Add(b1);
                breps.Add(b2);

                columnCS = Brep.JoinBreps(breps, 0.01)[0];
            }
            else
            {
                columnCS = outerColumnCS;
            }


            return columnCS;
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
