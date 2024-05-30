using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components.Deconstructors
{
    public class DeconstructWall : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructWall class.
        /// </summary>
        public DeconstructWall()
          : base("DeconstructWall", "Nickname",
              "Description",
              "MasterThesis", "Deconstruct")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Walls", "wl", "Class Wall", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Materials", "mat", "Material for each wall", GH_ParamAccess.tree);
            pManager.AddTextParameter("Sections", "sec", "Section for each wall", GH_ParamAccess.list);
            pManager.AddBrepParameter("Geometry", "geo", "Geometry of each wall", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Wall> walls = new List<Wall>();
            DA.GetDataList(0, walls);


            DataTree<string> materials = new DataTree<string>();
            List<string> sections = new List<string>();
            List<Brep> geometry = new List<Brep>();
            int i = 0;
            foreach (Wall wall in walls)
            {
                List<string> material = new List<string>();
                GH_Path path = new GH_Path(i);
                material.Add(wall.material.name);
                material.Add(wall.material.classification);
                material.Add(wall.material.density.ToString());
                materials.AddRange(material, path);
                i++;

                string sec = Convert.ToString(wall.section.height);
                sections.Add(sec);

                geometry.Add(wall.geometry);
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
            get { return new Guid("76193AC3-3B39-4BE4-AA31-5D7F800E5843"); }
        }
    }
}