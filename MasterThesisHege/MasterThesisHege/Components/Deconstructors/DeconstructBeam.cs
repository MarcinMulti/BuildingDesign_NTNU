using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components.Deconstructors
{
    public class DeconstructBeam : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructBeam class.
        /// </summary>
        public DeconstructBeam()
          : base("DeconstructBeam", "Nickname",
              "Description",
              "MasterThesis", "Deconstruct")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Beams", "bm", "Class Beam", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Materials", "mat", "Material for each beam", GH_ParamAccess.tree);
            pManager.AddTextParameter("Sections", "sec", "Section for each beam", GH_ParamAccess.list);
            pManager.AddLineParameter("Geometry", "geo", "Geometry of each beam", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Beam> beams = new List<Beam>();
            DA.GetDataList(0, beams);



            DataTree<string> materials = new DataTree<string>();
            List<string> sections = new List<string>();
            List<Line> geometry = new List<Line>();
            int i = 0;
            foreach (Beam beam in beams)
            {
                List<string> material = new List<string>();
                GH_Path path = new GH_Path(i);
                material.Add(beam.material.name);
                material.Add(beam.material.classification);
                material.Add(beam.material.density.ToString());
                materials.AddRange(material, path);
                i++;

                string w = Convert.ToString(beam.section.width);
                string h = Convert.ToString(beam.section.height);
                string t = Convert.ToString(beam.section.thickness);
                if (t == "0")
                {
                    string sec = w + "x" + h;
                    sections.Add(sec);
                }
                else
                {
                    string sec = w + "x" + h + "x" + t;
                    sections.Add(sec);
                }

                geometry.Add(beam.axis);
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
            get { return new Guid("572121A5-4FD6-4C02-A26E-3D1C01CB29EF"); }
        }
    }
}