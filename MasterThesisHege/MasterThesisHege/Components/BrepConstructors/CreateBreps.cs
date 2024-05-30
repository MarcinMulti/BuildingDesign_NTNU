using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components.BrepConstructors
{
    public class CreateBreps : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBreps class.
        /// </summary>
        public CreateBreps()
          : base("CreateBreps", "Nickname",
              "Description",
              "MasterThesis", "Breps")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("BrepGeometryType", "type", "Type of geometry for the breps", GH_ParamAccess.item);
            pManager.AddBrepParameter("PlotBrep", "plot", "Brep of the plot", GH_ParamAccess.item);
            pManager.AddNumberParameter("RotationRadians", "rad", "Rotation of the buildings in radians", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Widths", "w", "Widths of the breps", GH_ParamAccess.list);
            pManager.AddNumberParameter("Lengths", "l", "Lengths of the breps", GH_ParamAccess.list);
            pManager.AddNumberParameter("Heights", "h", "Heights of the breps", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "breps", "Breps on the plot", GH_ParamAccess.list);
            pManager.AddNumberParameter("OutsideArea", "outArea", "Area of building outside the plot", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int type = 0;
            Brep plot = new Brep();
            double rad = 0;
            List<double> widths = new List<double>();
            List<double> lengths = new List<double>();
            List<double> heights = new List<double>();

            DA.GetData(0, ref type);
            DA.GetData(1, ref plot);
            DA.GetData(2, ref rad);
            DA.GetDataList(3, widths);
            DA.GetDataList(4, lengths);
            DA.GetDataList(5, heights);



            List<Brep> breps = new List<Brep>();
            double outsideArea = 0;
            if (type == 1)
            {
                (breps, bool areaWithinPlot) = Breps.GetBrep1(plot, rad, widths[0], lengths[0], heights[0]);

                if (areaWithinPlot == false)
                {
                    outsideArea = Breps.GetOutsideArea(plot, breps);
                }
            }
            else if (type == 2)
            {
                (breps, bool areaWithinPlot) = Breps.GetBrep2(plot, rad, widths, lengths, heights);

                if (areaWithinPlot == false)
                {
                    outsideArea = Breps.GetOutsideArea(plot, breps);
                }
            }
            else if (type == 3)
            {
                (breps, bool areaWithinPlot) = Breps.GetBrep3(plot, rad, widths, lengths, heights);

                if (areaWithinPlot == false)
                {
                    outsideArea = Breps.GetOutsideArea(plot, breps);
                }
            }



            DA.SetDataList(0, breps);
            DA.SetData(1, outsideArea);
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
            get { return new Guid("AABBBEB1-7717-460E-8469-4EA0E0520A9A"); }
        }
    }
}