using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GH_IO.Serialization;
using MasterThesisHege.Components.BrepConstructors;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Render.ChangeQueue;
using Rhino.UI.ObjectProperties;
using static Rhino.DocObjects.PhysicallyBasedMaterial;

namespace MasterThesisHege.Classes
{
    public class Breps
    {
        public static (List<Brep>, bool) GetBrep1(Brep plot, double rad, double width, double length, double height)
        {
            Point3d cpt = AreaMassProperties.Compute(plot).Centroid;

            Plane pln;
            plot.Surfaces[0].TryGetPlane(out pln);
            pln.Translate(new Vector3d(cpt.X, cpt.Y, 0));

            Plane pln1 = pln;
            Plane pln2 = pln;
            pln2.Translate(new Vector3d(0, 0, height));

            Interval iw = new Interval(-width / 2, width / 2);
            Interval il = new Interval(-length / 2, length / 2);

            Rectangle3d rec1 = new Rectangle3d(pln1, iw, il);
            Rectangle3d rec2 = new Rectangle3d(pln2, iw, il);

            Brep face1 = Brep.CreatePlanarBreps(rec1.ToNurbsCurve(), 0.0001)[0];
            Brep face2 = Brep.CreatePlanarBreps(rec2.ToNurbsCurve(), 0.0001)[0];



            List<Point3d> cornerPts = new List<Point3d>();
            for (int i = 0; i < face1.Edges.Count; i++)
            {
                cornerPts.Add(face1.Edges[i].PointAtStart);
                cornerPts.Add(face2.Edges[i].PointAtStart);
            }

            BoundingBox box = new BoundingBox(cornerPts);
            Brep brep = box.ToBrep();



            Vector3d rotAxis = new Vector3d(0, 0, 1);
            brep.Rotate(rad, rotAxis, cpt);

            List<Brep> BREP = new List<Brep>();
            BREP.Add(brep);



            List<double> zs = new List<double>();
            for (int i = 0; i < brep.Faces.Count; i++)
            {
                Point3d facecpt = AreaMassProperties.Compute(brep.Faces[i]).Centroid;
                zs.Add(facecpt.Z);
            }

            double minz = zs.Min();
            int idx = zs.IndexOf(minz);

            Brep b = brep.Faces[idx].ToBrep();

            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < b.Edges.Count; i++)
            {
                pts.Add(b.Edges[i].PointAtStart);
            }


            List<Curve> curves = new List<Curve>();
            foreach (var e in plot.Edges)
                curves.Add(e.ToNurbsCurve());

            Curve perimeter = Curve.JoinCurves(curves)[0];
            perimeter.Scale(0.99);


            bool areaWithinPlot = true;
            for (int i = 0; i < pts.Count; i++)
            {
                Point3d ptend = new Point3d(pts[i].X + 1000, pts[i].Y, 0);

                List<Point3d> plist = new List<Point3d>();
                plist.Add(pts[i]);
                plist.Add(ptend);
                Curve intcrv = Curve.CreateControlPointCurve(plist, 1);

                var ints = Intersection.CurveCurve(perimeter, intcrv, 0.0, 0.0);
                if (ints.Count != 1)
                {
                    areaWithinPlot = false;
                }
            }


            return (BREP, areaWithinPlot);
        }





        public static (List<Brep>, bool) GetBrep2(Brep plot, double rad, List<double> widths, List<double> lengths, List<double> heights)
        {
            Point3d cpt = AreaMassProperties.Compute(plot).Centroid;

            Plane pln;
            plot.Surfaces[0].TryGetPlane(out pln);
            pln.Translate(new Vector3d(cpt.X, cpt.Y, 0));
            pln.Translate(new Vector3d(0, -lengths[0] / 2, 0));

            Plane pln1 = pln;
            Plane pln2 = pln;
            pln2.Translate(new Vector3d(0, 0, heights[0]));

            Interval iw = new Interval(-widths[0] / 2, widths[0] / 2);
            Interval il = new Interval(-lengths[0] / 2, lengths[0] / 2);

            Rectangle3d rec1 = new Rectangle3d(pln1, iw, il);
            Rectangle3d rec2 = new Rectangle3d(pln2, iw, il);

            Brep face1 = Brep.CreatePlanarBreps(rec1.ToNurbsCurve(), 0.0001)[0];
            Brep face2 = Brep.CreatePlanarBreps(rec2.ToNurbsCurve(), 0.0001)[0];


            List<Point3d> cornerPts = new List<Point3d>();
            for (int i = 0; i < face1.Edges.Count; i++)
            {
                cornerPts.Add(face1.Edges[i].PointAtStart);
                cornerPts.Add(face2.Edges[i].PointAtStart);
            }

            BoundingBox box = new BoundingBox(cornerPts);
            Brep brep1 = box.ToBrep();



            Plane PLN;
            plot.Surfaces[0].TryGetPlane(out PLN);
            Point3d p = face1.Edges[1].PointAtStart;
            PLN.Translate(new Vector3d(p.X, p.Y, p.Z));

            Plane PLN1 = PLN;
            Plane PLN2 = PLN;
            PLN2.Translate(new Vector3d(0, 0, heights[1]));

            Rectangle3d REC1 = new Rectangle3d(PLN1, widths[1], lengths[1]);
            Rectangle3d REC2 = new Rectangle3d(PLN2, widths[1], lengths[1]);

            Brep FACE1 = Brep.CreatePlanarBreps(REC1.ToNurbsCurve(), 0.0001)[0];
            Brep FACE2 = Brep.CreatePlanarBreps(REC2.ToNurbsCurve(), 0.0001)[0];


            List<Point3d> CORNERPts = new List<Point3d>();
            for (int i = 0; i < FACE1.Edges.Count; i++)
            {
                CORNERPts.Add(FACE1.Edges[i].PointAtStart);
                CORNERPts.Add(FACE2.Edges[i].PointAtStart);
            }

            BoundingBox BOX = new BoundingBox(CORNERPts);
            Brep brep2 = BOX.ToBrep();



            Vector3d rotAxis = new Vector3d(0, 0, 1);
            brep1.Rotate(rad, rotAxis, cpt);
            brep2.Rotate(rad, rotAxis, cpt);

            List<Brep> breps = new List<Brep>();
            breps.Add(brep1);
            breps.Add(brep2);



            List<Point3d> pts = new List<Point3d>();
            foreach (Brep brep in breps)
            {
                List<double> zs = new List<double>();
                for (int i = 0; i < brep.Faces.Count; i++)
                {
                    Point3d facecpt = AreaMassProperties.Compute(brep.Faces[i]).Centroid;
                    zs.Add(facecpt.Z);
                }

                double minz = zs.Min();
                int idx = zs.IndexOf(minz);

                Brep b = brep.Faces[idx].ToBrep();

                for (int i = 0; i < b.Edges.Count; i++)
                {
                    pts.Add(b.Edges[i].PointAtStart);
                }
            }


            List<Curve> curves = new List<Curve>();
            foreach (var e in plot.Edges)
                curves.Add(e.ToNurbsCurve());

            Curve perimeter = Curve.JoinCurves(curves)[0];
            perimeter.Scale(0.99);


            bool areaWithinPlot = true;
            for (int i = 0; i < pts.Count; i++)
            {
                Point3d ptend = new Point3d(pts[i].X + 1000, pts[i].Y, 0);

                List<Point3d> plist = new List<Point3d>();
                plist.Add(pts[i]);
                plist.Add(ptend);
                Curve intcrv = Curve.CreateControlPointCurve(plist, 1);

                var ints = Intersection.CurveCurve(perimeter, intcrv, 0.0, 0.0);
                if (ints.Count != 1)
                {
                    areaWithinPlot = false;
                }
            }


            return (breps, areaWithinPlot);
        }





        public static (List<Brep>, bool) GetBrep3(Brep plot, double rad, List<double> widths, List<double> lengths, List<double> heights)
        {
            Point3d cpt = AreaMassProperties.Compute(plot).Centroid;

            Plane pln;
            plot.Surfaces[0].TryGetPlane(out pln);
            pln.Translate(new Vector3d(cpt.X, cpt.Y, 0));
            pln.Translate(new Vector3d(0, -lengths[0] / 2, 0));

            Plane pln1 = pln;
            Plane pln2 = pln;
            pln2.Translate(new Vector3d(0, 0, heights[0]));

            Interval iw = new Interval(-widths[0] / 2, widths[0] / 2);
            Interval il = new Interval(-lengths[0] / 2, lengths[0] / 2);

            Rectangle3d rec1 = new Rectangle3d(pln1, iw, il);
            Rectangle3d rec2 = new Rectangle3d(pln2, iw, il);

            Brep face1 = Brep.CreatePlanarBreps(rec1.ToNurbsCurve(), 0.0001)[0];
            Brep face2 = Brep.CreatePlanarBreps(rec2.ToNurbsCurve(), 0.0001)[0];


            List<Point3d> cornerPts = new List<Point3d>();
            for (int i = 0; i < face1.Edges.Count; i++)
            {
                cornerPts.Add(face1.Edges[i].PointAtStart);
                cornerPts.Add(face2.Edges[i].PointAtStart);
            }

            BoundingBox box = new BoundingBox(cornerPts);
            Brep brep1 = box.ToBrep();



            Plane PLN;
            plot.Surfaces[0].TryGetPlane(out PLN);
            Point3d p = face1.Edges[1].PointAtStart;
            PLN.Translate(new Vector3d(p.X, p.Y, p.Z));

            Plane PLN1 = PLN;
            Plane PLN2 = PLN;
            PLN2.Translate(new Vector3d(0, 0, heights[1]));

            Rectangle3d REC1 = new Rectangle3d(PLN1, widths[1], lengths[1]);
            Rectangle3d REC2 = new Rectangle3d(PLN2, widths[1], lengths[1]);

            Brep FACE1 = Brep.CreatePlanarBreps(REC1.ToNurbsCurve(), 0.0001)[0];
            Brep FACE2 = Brep.CreatePlanarBreps(REC2.ToNurbsCurve(), 0.0001)[0];


            List<Point3d> CORNERPts = new List<Point3d>();
            for (int i = 0; i < FACE1.Edges.Count; i++)
            {
                CORNERPts.Add(FACE1.Edges[i].PointAtStart);
                CORNERPts.Add(FACE2.Edges[i].PointAtStart);
            }

            BoundingBox BOX = new BoundingBox(CORNERPts);
            Brep brep2 = BOX.ToBrep();



            Plane Pln;
            plot.Surfaces[0].TryGetPlane(out Pln);
            Point3d P = FACE1.Edges[3].PointAtStart;
            Pln.Translate(new Vector3d(P.X, P.Y, P.Z));

            Plane Pln1 = Pln;
            Plane Pln2 = Pln;
            Pln2.Translate(new Vector3d(0, 0, heights[2]));

            Rectangle3d Rec1 = new Rectangle3d(Pln1, widths[2], lengths[2]);
            Rectangle3d Rec2 = new Rectangle3d(Pln2, widths[2], lengths[2]);

            Brep Face1 = Brep.CreatePlanarBreps(Rec1.ToNurbsCurve(), 0.0001)[0];
            Brep Face2 = Brep.CreatePlanarBreps(Rec2.ToNurbsCurve(), 0.0001)[0];


            List<Point3d> CornerPts = new List<Point3d>();
            for (int i = 0; i < Face1.Edges.Count; i++)
            {
                CornerPts.Add(Face1.Edges[i].PointAtStart);
                CornerPts.Add(Face2.Edges[i].PointAtStart);
            }

            BoundingBox Box = new BoundingBox(CornerPts);
            Brep brep3 = Box.ToBrep();



            Vector3d rotAxis = new Vector3d(0, 0, 1);
            brep1.Rotate(rad, rotAxis, cpt);
            brep2.Rotate(rad, rotAxis, cpt);
            brep3.Rotate(rad, rotAxis, cpt);

            List<Brep> breps = new List<Brep>();
            breps.Add(brep1);
            breps.Add(brep2);
            breps.Add(brep3);



            List<Point3d> pts = new List<Point3d>();
            foreach (Brep brep in breps)
            {
                List<double> zs = new List<double>();
                for (int i = 0; i < brep.Faces.Count; i++)
                {
                    Point3d facecpt = AreaMassProperties.Compute(brep.Faces[i]).Centroid;
                    zs.Add(facecpt.Z);
                }

                double minz = zs.Min();
                int idx = zs.IndexOf(minz);

                Brep b = brep.Faces[idx].ToBrep();

                for (int i = 0; i < b.Edges.Count; i++)
                {
                    pts.Add(b.Edges[i].PointAtStart);
                }
            }


            List<Curve> curves = new List<Curve>();
            foreach (var e in plot.Edges)
                curves.Add(e.ToNurbsCurve());

            Curve perimeter = Curve.JoinCurves(curves)[0];
            perimeter.Scale(0.99);


            bool areaWithinPlot = true;
            for (int i = 0; i < pts.Count; i++)
            {
                Point3d ptend = new Point3d(pts[i].X + 1000, pts[i].Y, 0);

                List<Point3d> plist = new List<Point3d>();
                plist.Add(pts[i]);
                plist.Add(ptend);
                Curve intcrv = Curve.CreateControlPointCurve(plist, 1);

                var ints = Intersection.CurveCurve(perimeter, intcrv, 0.0, 0.0);
                if (ints.Count != 1)
                {
                    areaWithinPlot = false;
                }
            }


            return (breps, areaWithinPlot);
        }





        public static double GetOutsideArea(Brep plot, List<Brep> breps)
        {
            double outsideArea = 0;


            List<Curve> perimeter = new List<Curve>();
            foreach (var e in plot.Edges)
                perimeter.Add(e.ToNurbsCurve());


            List<Brep> bottomBreps = new List<Brep>();
            foreach (Brep brep in breps)
            {
                List<double> zs = new List<double>();
                for (int i = 0; i < brep.Faces.Count; i++)
                {
                    Point3d facecpt = AreaMassProperties.Compute(brep.Faces[i]).Centroid;
                    zs.Add(facecpt.Z);
                }

                double minz = zs.Min();
                int idx = zs.IndexOf(minz);

                Brep bottomBrep = brep.Faces[idx].ToBrep();
                bottomBreps.Add(bottomBrep);
            }

            
            foreach(Brep botBrep in bottomBreps)
            {
                var splitAreas = botBrep.Split(perimeter, 0.001);
                if (splitAreas.Length > 1)
                {
                    outsideArea += splitAreas[0].GetArea();
                }
            }


            return outsideArea;
        }
    }
}
