using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Geometry.Voronoi;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace MasterThesisHege.Components
{
    public class BuildingToMesh : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BuildingToMesh class.
        /// </summary>
        public BuildingToMesh()
          : base("BuildingToMesh", "Nickname",
              "Description",
              "MasterThesis", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "mesh", "Meshes from building", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            DA.GetData(0, ref building);



            List<Brep> wls = new List<Brep>();
            List<Brep> flrs = new List<Brep>();
            List<Line> cols = new List<Line>();
            List<Mesh> meshes = new List<Mesh>();

            foreach (Wall wall in building.walls)
            {
                wls.Add(wall.geometry);
            }

            foreach (Floor floor in building.floors)
            {
                flrs.Add(floor.geometry);  
            }

            foreach (Column column in building.columns)
            {
                cols.Add(column.axis);
            }


            foreach (Brep brpflr in flrs)
            {
                Vector3d zvec = new Vector3d(0, 0, 1);
                zvec.Unitize();
                List<Point3d> pts = new List<Point3d>();
                List<Curve> curves = new List<Curve>();

                if (wls.Count != 0)
                {
                    List<Curve> crvs = new List<Curve>();
                    foreach (Brep bw in wls)
                    {
                        for (int i = 0; i < bw.Edges.Count; i++)
                        {
                            double edgeMid = bw.Edges[i].GetLength() / 2;
                            Point3d p = bw.Edges[i].PointAtLength(edgeMid);
                            if (p.Z == 0.0)
                            {
                                crvs.Add(bw.Edges[i]);
                            }
                        }
                    }
                    Curve[] c = Curve.JoinCurves(crvs);

                    foreach (Curve crv in c)
                    {
                        Curve c2 = Curve.ProjectToBrep(crv, brpflr, zvec, 0.001)[0];
                        curves.Add(c2);
                        Point3d[] pts1 = c2.DivideEquidistant(1);
                        foreach (Point3d p in pts1)
                        {
                            pts.Add(p);
                        }
                    }
                }

                if (cols.Count != 0)
                {
                    foreach (Line col in cols)
                    {
                        Point3d p = col.PointAtLength(0);
                        if (p.Z == 0)
                        {
                            Point3d cpt = AreaMassProperties.Compute(brpflr).Centroid;
                            Point3d pnew = new Point3d(p.X, p.Y, cpt.Z);
                            pts.Add(pnew);
                        }
                    }
                }
                
                List<List<Point3d>> pts2 = new List<List<Point3d>>();
                foreach (Curve edge in brpflr.Edges)
                {
                    Point3d[] edgepts = edge.DivideEquidistant(1);
                    List<Point3d> linepts = new List<Point3d>();
                    foreach (Point3d p in edgepts)
                    {
                        linepts.Add(p);
                    }
                    pts2.Add(linepts);
                }

                if (wls.Count != 0)
                {

                    foreach (Curve c in curves)
                    {
                        c.Extend(CurveEnd.Both, 20, CurveExtensionStyle.Line);
                    }

                    Brep[] b = brpflr.Split(curves, 0.001);

                    Mesh m = new Mesh();
                    foreach (Brep brep in b)
                    {
                        Surface srf = brep.Surfaces[0];
                        Plane pln;
                        srf.TryGetPlane(out pln);

                        Mesh mesh = Mesh.CreateFromTessellation(pts, pts2, pln, true);
                        m.Append(mesh);
                        m.Compact();
                    }

                    meshes.Add(m);
                }

                else
                {
                    Surface srf = brpflr.Surfaces[0];
                    Plane pln;
                    srf.TryGetPlane(out pln);

                    Mesh mesh = Mesh.CreateFromTessellation(pts, pts2, pln, true);

                    meshes.Add(mesh);
                }
            }
            
            if (wls.Count != 0)
            {
                foreach (Brep brpwl in wls)
                {
                    List<Point3d> pts = new List<Point3d>();
                    int i = 0;
                    foreach (Curve edge in brpwl.Edges)
                    {
                        Curve c = edge;
                        double l = edge.GetLength() / 4;
                        if (i == 0 || i == 1)
                        {
                            if (c.PointAt(l * 2).Z == 0)
                            {
                                Point3d[] p = c.DivideAsContour(c.PointAtStart, c.PointAtEnd, 1);
                                foreach (Point3d pt in p)
                                {
                                    pts.Add(pt);
                                }
                            }
                            else
                            {
                                Point3d[] p = c.DivideAsContour(c.PointAtStart, c.PointAtEnd, l);
                                foreach (Point3d pt in p)
                                {
                                    pts.Add(pt);
                                }
                            }
                        }
                        else if (i == 2 || i == 3)
                        {
                            if (c.PointAt(l * 2).Z == 0)
                            {
                                Point3d[] p = c.DivideAsContour(c.PointAtEnd, c.PointAtStart, 1);
                                foreach (Point3d pt in p)
                                {
                                    pts.Add(pt);
                                }
                            }
                            else
                            {
                                Point3d[] p = c.DivideAsContour(c.PointAtEnd, c.PointAtStart, l);
                                foreach (Point3d pt in p)
                                {
                                    pts.Add(pt);
                                }
                            }
                        }
                        
                        i++;
                    }
                    Point3d cp = AreaMassProperties.Compute(brpwl).Centroid;
                    pts.Add(cp);

                    Surface srf = brpwl.Surfaces[0];
                    Plane pln;
                    srf.TryGetPlane(out pln);

                    Mesh mesh = Mesh.CreateFromTessellation(pts, null, pln, false);

                    meshes.Add(mesh);
                }
            }

                DA.SetDataList(0, meshes);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        /*
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }
        */
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0901823D-ED13-4D3E-82FC-3D694C655C52"); }
        }
    }
}