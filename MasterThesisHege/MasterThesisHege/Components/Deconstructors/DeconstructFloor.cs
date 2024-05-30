using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components.Deconstructors
{
    public class DeconstructFloor : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructFloor class.
        /// </summary>
        public DeconstructFloor()
          : base("DeconstructFloor", "Nickname",
              "Description",
              "MasterThesis", "Deconstruct")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Floors", "flr", "Class Floor", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Materials", "mat", "Material for each floor", GH_ParamAccess.tree);
            pManager.AddTextParameter("Sections", "sec", "Section for each floor", GH_ParamAccess.list);
            pManager.AddBrepParameter("Geometry", "geo", "Geometry of each floor", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Floor> floors = new List<Floor>();
            DA.GetDataList(0, floors);



            DataTree<string> materials = new DataTree<string>();
            List<string> sections = new List<string>();
            List<Brep> geometry = new List<Brep>();
            int i = 0;
            foreach (Floor floor in floors)
            {
                List<string> material = new List<string>();
                GH_Path path = new GH_Path(i);
                material.Add(floor.material.name);
                material.Add(floor.material.classification);
                material.Add(floor.material.density.ToString());
                materials.AddRange(material, path);
                i++;

                string sec = Convert.ToString(floor.section.height);
                sections.Add(sec);

                geometry.Add(floor.geometry);
            }



            DA.SetDataTree(0, materials);
            DA.SetDataList(1, sections);
            DA.SetDataList(2, geometry);
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
            get { return new Guid("515EB48D-F06E-4BF2-826D-B6E0FD44F5A0"); }
        }
    }
}