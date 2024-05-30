using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;

namespace MasterThesisHege.Components.BrepConstructors
{
    public class CreateBrep2 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBrep2 class.
        /// </summary>
        public CreateBrep2()
          : base("CreateBrep2", "Nickname",
              "Description",
              "MasterThesis", "Breps")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("PlotBrep", "plot", "Brep of the plot", GH_ParamAccess.item);
            pManager.AddNumberParameter("RotationRadians", "rad", "Rotation of building in radians", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Width1", "w1", "Width 1 of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Length1", "l1", "Length 1 of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Height1", "h1", "Height 1 of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Width2", "w2", "Width 2 of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Length2", "l2", "Length 2 of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Height2", "h2", "Height 2 of the brep", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "breps", "Breps on the plot", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep plot = new Brep();
            double rad = 0;
            double w1 = 0;
            double l1 = 0;
            double h1 = 0;
            double w2 = 0;
            double l2 = 0;
            double h2 = 0;

            DA.GetData(0, ref plot);
            DA.GetData(1, ref rad);
            DA.GetData(2, ref w1);
            DA.GetData(3, ref l1);
            DA.GetData(4, ref h1);
            DA.GetData(5, ref w2);
            DA.GetData(6, ref l2);
            DA.GetData(7, ref h2);



            List<double> widths = new List<double>();
            List<double> lengths = new List<double>();
            List<double> heights = new List<double>();

            widths.Add(w1);
            lengths.Add(l1);
            heights.Add(h1);
            widths.Add(w2);
            lengths.Add(l2);
            heights.Add(h2);

            (List<Brep> breps, bool areaWithinPlot) = Breps.GetBrep2(plot, rad, widths, lengths, heights);



            DA.SetDataList(0, breps);
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
            get { return new Guid("44523939-31FA-42A8-AB8C-BF2625405C24"); }
        }
    }
}