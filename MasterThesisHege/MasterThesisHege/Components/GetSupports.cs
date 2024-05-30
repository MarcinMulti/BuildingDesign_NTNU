using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class GetSupports : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetSupports class.
        /// </summary>
        public GetSupports()
          : base("GetSupports", "Nickname",
              "Description",
              "MasterThesis", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("SupportPoints", "sprtPts", "Support points from columns", GH_ParamAccess.list);
            pManager.AddLineParameter("SupportLines", "sprtLns", "Support lines from walls", GH_ParamAccess.list);
            pManager.AddPointParameter("SupportPointsFromLines", "sprtLnPts", "Support points from walls", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            DA.GetData(0, ref building);



            List<Point3d> SupPoints = new List<Point3d>();
            List<Line> SupLines = new List<Line>();
            List<Point3d> SupPointsFromLines = new List<Point3d>();

            foreach (Support support in building.supports)
            {
                if (support.supportPoint != Point3d.Unset)
                {
                    SupPoints.Add(support.supportPoint);
                }
                if (support.supportLine != Line.Unset)
                {
                    SupLines.Add(support.supportLine);
                }
            }

            List<Point3d> SPFL = new List<Point3d>();
            for (int i = 0; i < SupLines.Count; i++)
            {
                Line l = SupLines[i];
                var stpt = l.PointAt(0);
                var endpt = l.PointAt(1);
                List<Point3d> cpts = new List<Point3d>() { stpt, endpt };

                var crv = Curve.CreateControlPointCurve(cpts, 1);
                var pts = crv.DivideEquidistant(1.0);
                for (int j = 0; j < pts.Length; j++)
                {
                    SPFL.Add(pts[j]);
                }
            }
            SupPointsFromLines = SPFL.Distinct().ToList();



            DA.SetDataList(0, SupPoints);
            DA.SetDataList(1, SupLines);
            DA.SetDataList(2, SupPointsFromLines);
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
            get { return new Guid("A9F8B287-9B0A-4A2E-89F3-29E0D9AE95CD"); }
        }
    }
}