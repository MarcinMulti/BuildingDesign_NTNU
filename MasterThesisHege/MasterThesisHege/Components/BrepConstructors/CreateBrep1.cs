using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components.BrepConstructors
{
    public class CreateBrep1 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBrep1 class.
        /// </summary>
        public CreateBrep1()
          : base("CreateBrep1", "Nickname",
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
            pManager.AddNumberParameter("RotationRadians", "rad", "Rotation of the building in radians", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Width","w","Width of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Length","l","Length of the brep", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Height", "h", "Height of the brep", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep","brep","Brep on the plot", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep plot = new Brep();
            double rad = 0;
            double width = 0;
            double length = 0;
            double height = 0;

            DA.GetData(0, ref plot);
            DA.GetData(1, ref rad);
            DA.GetData(2, ref width);
            DA.GetData(3, ref length);
            DA.GetData(4, ref height);



            (List<Brep> brep, bool areaWithinPlot) = Breps.GetBrep1(plot, rad, width, length, height);



            DA.SetDataList(0, brep);
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
            get { return new Guid("98FDCB20-D98B-4881-85DC-F1FE9AA636A5"); }
        }
    }
}