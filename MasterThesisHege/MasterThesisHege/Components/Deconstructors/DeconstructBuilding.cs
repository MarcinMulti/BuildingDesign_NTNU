using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components.Deconstructors
{
    public class DeconstructBuilding : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructBuilding class.
        /// </summary>
        public DeconstructBuilding()
          : base("DeconstructBuilding", "Nickname",
              "Description",
              "MasterThesis", "Deconstruct")
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
            pManager.AddGenericParameter("Floors", "flr", "Class Floor", GH_ParamAccess.list);
            pManager.AddGenericParameter("Walls", "wl", "Class Wall", GH_ParamAccess.list);
            pManager.AddGenericParameter("Columns", "col", "Class Column", GH_ParamAccess.list);
            pManager.AddGenericParameter("Beams", "bm", "Class Beam", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            DA.GetData(0, ref building);


            DA.SetDataList(0, building.floors);
            DA.SetDataList(1, building.walls);
            DA.SetDataList(2, building.columns);
            DA.SetDataList(3, building.beams);
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
            get { return new Guid("8F677A61-CE32-4E44-98BE-EC568A945E89"); }
        }
    }
}