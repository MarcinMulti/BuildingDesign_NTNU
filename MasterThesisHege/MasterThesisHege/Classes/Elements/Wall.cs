using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MasterThesisHege.Classes.Elements
{
    public class Wall
    {
        public Brep geometry;
        public Brep geometryCS;
        public Material material;
        public Section section;




        public static List<Wall> GetWalls(List<List<List<Point3d>>> allNodes)
        {
            List<Wall> walls = new List<Wall>();
            for (int i = 0; i < allNodes.Count - 1; i++)
            {
                if (allNodes[i].Count == allNodes[i + 1].Count)
                {
                    for (int j = 0; j < allNodes[i].Count; j++)
                    {
                        List<Point3d> nds1 = allNodes[i][j];
                        List<Point3d> nds2 = allNodes[i+1][j];
                        for (int k = 0; k < nds1.Count - 1; k++)
                        {
                            for (int l = 0; l < nds2.Count-1; l++)
                            {
                                Point3d p1 = new Point3d(nds1[k].X, nds1[k].Y, nds2[l].Z);
                                Point3d p2 = new Point3d(nds1[k+1].X, nds1[k+1].Y, nds2[l+1].Z);
                                if (p1 == nds2[l] && p2 == nds2[l+1])
                                {
                                    Wall wall = new Wall();
                                    List<Point3d> nds = new List<Point3d>();
                                    nds.Add(nds1[k]);
                                    nds.Add(nds1[k + 1]);
                                    nds.Add(nds2[l]);
                                    nds.Add(nds2[l + 1]);

                                    NurbsSurface w = NurbsSurface.CreateFromPoints(nds, 2, 2, 1, 1);
                                    Brep wl = w.ToBrep();
                                    wall.geometry = wl;

                                    Material mat = Material.GetMaterial();
                                    wall.material = mat;

                                    Section sec = Section.GetWallSection();
                                    wall.section = sec;

                                    walls.Add(wall);
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
                            List<Point3d> nds1 = lowerRow[d];
                            List<Point3d> nds2 = allNodes[i + 1][d];
                            for (int e = 0; e < nds1.Count - 1; e++)
                            {
                                for (int l = 0; l < nds2.Count - 1; l++)
                                {
                                    Point3d p1 = new Point3d(nds1[e].X, nds1[e].Y, nds2[l].Z);
                                    Point3d p2 = new Point3d(nds1[e + 1].X, nds1[e + 1].Y, nds2[l + 1].Z);
                                    if (p1 == nds2[l] && p2 == nds2[l + 1])
                                    {
                                        Wall wall = new Wall();
                                        List<Point3d> nds = new List<Point3d>();
                                        nds.Add(nds1[e]);
                                        nds.Add(nds1[e + 1]);
                                        nds.Add(nds2[l]);
                                        nds.Add(nds2[l + 1]);

                                        NurbsSurface w = NurbsSurface.CreateFromPoints(nds, 2, 2, 1, 1);
                                        Brep wl = w.ToBrep();
                                        wall.geometry = wl;

                                        Material mat = Material.GetMaterial();
                                        wall.material = mat;

                                        Section sec = Section.GetWallSection();
                                        wall.section = sec;

                                        walls.Add(wall);
                                    }
                                }
                            }
                        }
                    }

                    else if (countMax == count2)
                    {
                        List<Point3d> pts = new List<Point3d>();
                        for (int x = 0; x < allNodes[i+1].Count; x++)
                        {
                            for (int y = 0; y < allNodes[i+1][x].Count; y++)
                            {
                                pts.Add(allNodes[i+1][x][y]);
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
                            List<Point3d> nds1 = allNodes[i][d];
                            List<Point3d> nds2 = upperRow[d];
                            for (int e = 0; e < nds1.Count - 1; e++)
                            {
                                for (int l = 0; l < nds2.Count - 1; l++)
                                {
                                    Point3d p1 = new Point3d(nds1[e].X, nds1[e].Y, nds2[l].Z);
                                    Point3d p2 = new Point3d(nds1[e + 1].X, nds1[e + 1].Y, nds2[l + 1].Z);
                                    if (p1 == nds2[l] && p2 == nds2[l + 1])
                                    {
                                        Wall wall = new Wall();
                                        List<Point3d> nds = new List<Point3d>();
                                        nds.Add(nds1[e]);
                                        nds.Add(nds1[e + 1]);
                                        nds.Add(nds2[l]);
                                        nds.Add(nds2[l + 1]);

                                        NurbsSurface w = NurbsSurface.CreateFromPoints(nds, 2, 2, 1, 1);
                                        Brep wl = w.ToBrep();
                                        wall.geometry = wl;

                                        Material mat = Material.GetMaterial();
                                        wall.material = mat;

                                        Section sec = Section.GetWallSection();
                                        wall.section = sec;

                                        walls.Add(wall);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return walls;
        }




        public static List<Wall> GetInnerWalls(List<List<List<Point3d>>> allNodes)
        {
            List<Wall> walls = new List<Wall>();
            for (int i = 0; i < allNodes.Count - 1; i++)
            {
                if (allNodes[i].Count > 2)
                {
                    for (int j = 1; j < allNodes[i].Count - 1; j++)
                    {
                        for (int k = 0; k < allNodes[i][j].Count - 1; k++)
                        {
                            if (allNodes[i].Count == allNodes[i + 1].Count)
                            {
                                Wall wall = new Wall();
                                List<Point3d> nds = new List<Point3d>();
                                nds.Add(allNodes[i][j][k]);
                                nds.Add(allNodes[i][j][k + 1]);
                                nds.Add(allNodes[i + 1][j][k]);
                                nds.Add(allNodes[i + 1][j][k + 1]);

                                NurbsSurface w = NurbsSurface.CreateFromPoints(nds, 2, 2, 1, 1);
                                Brep wl = w.ToBrep();
                                wall.geometry = wl;

                                Material mat = Material.GetMaterial();
                                wall.material = mat;

                                Section sec = Section.GetWallSection();
                                wall.section = sec;

                                walls.Add(wall);
                            }
                        }
                    }
                }
            }
            return walls;
        }




        public static Brep CreateCSGeometry(Brep surface, double thickness)
        {
            Surface srf = surface.Surfaces[0];
            Plane pln;
            srf.TryGetPlane(out pln);

            Vector3d vec = pln.ZAxis;
            vec.Unitize();
            double T = thickness / 2;

            Brep face1 = surface.DuplicateBrep();
            Brep face2 = surface.DuplicateBrep();

            face1.Translate(vec * T);
            face2.Translate(vec * -T);

            List<Point3d> cornerPts = new List<Point3d>();
            for (int i = 0; i < face1.Edges.Count; i++)
            {
                cornerPts.Add(face1.Edges[i].PointAtStart);
                cornerPts.Add(face2.Edges[i].PointAtStart);
            }

            BoundingBox box = new BoundingBox(cornerPts);
            Brep wallCS = box.ToBrep();

            return wallCS;
        }
    }
}
