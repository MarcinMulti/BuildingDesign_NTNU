using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Geometry;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace MasterThesisHege.Classes
{
    public class Nodes
    {
        public static List<List<List<Point3d>>> GetNodes(Curve perimeter, int numfloor, double fheight, Point3d cpt, Vector3d vec1, Vector3d vec2)
        {
            List<List<List<Point3d>>> allNodes = new List<List<List<Point3d>>>();
            for (int i = 0; i < numfloor; i++)
            {
                List<List<Point3d>> nodesOnSurface = new List<List<Point3d>>();
                for (int j = -50; j < 50; j++)
                {
                    List<Point3d> nodesInRow = new List<Point3d>();
                    for (int k = -50; k < 50; k++)
                    {
                        Point3d pt = Point3d.Add(cpt, j * vec1);
                        pt = Point3d.Add(pt, k * vec2);

                        Point3d ptend = new Point3d(pt.X + 1000, pt.Y, 0);

                        List<Point3d> plist = new List<Point3d>();
                        plist.Add(pt);
                        plist.Add(ptend);

                        Curve intcrv = Curve.CreateControlPointCurve(plist, 1);

                        var ints = Intersection.CurveCurve(perimeter, intcrv, 0.0, 0.0);
                        if (ints.Count == 1)
                        {
                            pt.Z = i * fheight;
                            nodesInRow.Add(pt);
                        }
                    }
                    nodesOnSurface.Add(nodesInRow);
                }
                allNodes.Add(nodesOnSurface);
            }

            List<int> idxs1 = new List<int>();
            List<int> idxs2 = new List<int>();
            for (int i = 0; i < allNodes.Count; i++)
            {
                for (int j = 0; j < allNodes[i].Count; j++)
                {
                    List<Point3d> lst = new List<Point3d>();
                    if (allNodes[i][j].Count == lst.Count)
                    {
                        idxs1.Add(i);
                        idxs2.Add(j);
                    }
                }
            }

            for (int m = idxs1.Count - 1; m > -1; m -= 1)
            {
                allNodes[idxs1[m]].RemoveAt(idxs2[m]);
            }

            List<int> idxs3 = new List<int>();
            for (int i = 0; i < allNodes.Count; i++)
            {
                List<List<Point3d>> lst1 = new List<List<Point3d>>();
                if (allNodes[i].Count == lst1.Count)
                {
                    idxs3.Add(i);
                }
            }

            for (int n = idxs3.Count - 1; n > -1; n -= 1)
            {
                allNodes.RemoveAt(idxs3[n]);
            }

            return allNodes;
        }




        public static List<List<List<Point3d>>> GetComplexNodes(List<Brep> breps, double totHeight, double minflrheight, Point3d cptLowerSrf, Vector3d vec1, Vector3d vec2)
        {
            List<List<List<Point3d>>> allNodes = new List<List<List<Point3d>>>();
            for (double i = cptLowerSrf.Z; i < totHeight; i+=minflrheight)
            {
                List<List<Point3d>> nodesOnSurface = new List<List<Point3d>>();
                for (int j = -50; j < 50; j++)
                {
                    List<Point3d> nodesInRow = new List<Point3d>();
                    for (int k = -50; k < 50; k++)
                    {
                        Point3d pt = Point3d.Add(cptLowerSrf, j * vec1);
                        pt = Point3d.Add(pt, k * vec2);
                        pt.Z = i;

                        for (int l = 0; l < breps.Count; l++)
                        {
                            if (breps[l].IsPointInside(pt, 0.0001, false))
                            {
                                nodesInRow.Add(pt);
                            }
                        }
                    }
                    nodesOnSurface.Add(nodesInRow);
                }
                allNodes.Add(nodesOnSurface);
            }

            List<int> idxs1 = new List<int>();
            List<int> idxs2 = new List<int>();
            for (int i = 0; i < allNodes.Count;  i++)
            {
                for (int j = 0; j < allNodes[i].Count; j++)
                {
                    List<Point3d> lst2 = new List<Point3d>();
                    if (allNodes[i][j].Count == lst2.Count)
                    {
                        idxs1.Add(i);
                        idxs2.Add(j);
                    }
                }
            }
            
            for (int m = idxs1.Count - 1; m > -1; m -= 1)
            {
                allNodes[idxs1[m]].RemoveAt(idxs2[m]);
            }

            List<int> idxs3 = new List<int>();
            for (int i = 0; i < allNodes.Count; i++)
            {
                List<List<Point3d>> lst1 = new List<List<Point3d>>();
                if (allNodes[i].Count == lst1.Count)
                {
                    idxs3.Add(i);
                }
            }

            for (int n = idxs3.Count -1; n > -1; n -= 1)
            {
                allNodes.RemoveAt(idxs3[n]);
            }
            
            return allNodes;
        }
    }
}
