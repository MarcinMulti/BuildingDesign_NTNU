using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components.Deconstructors
{
    public class DeconstructColumn : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructColumn class.
        /// </summary>
        public DeconstructColumn()
          : base("DeconstructColumn", "Nickname",
              "Description",
              "MasterThesis", "Deconstruct")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Columns", "col", "Class Column", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Materials", "mat", "Material for each column", GH_ParamAccess.tree);
            pManager.AddTextParameter("Sections", "sec", "Section for each column", GH_ParamAccess.list);
            pManager.AddLineParameter("Geometry", "geo", "Geometry of each column", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Column> columns = new List<Column>();
            DA.GetDataList(0, columns);



            DataTree<string> materials = new DataTree<string>();
            List<string> sections = new List<string>();
            List<Line> geometry = new List<Line>();
            int i = 0;
            foreach (Column column in columns)
            {
                List<string> material = new List<string>();
                GH_Path path = new GH_Path(i);
                material.Add(column.material.name);
                material.Add(column.material.classification);
                material.Add(column.material.density.ToString());
                materials.AddRange(material, path);
                i++;

                string w = Convert.ToString(column.section.width);
                string h = Convert.ToString(column.section.height);
                string t = Convert.ToString(column.section.thickness);
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

                geometry.Add(column.axis);
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
            get { return new Guid("E99511BD-D285-4203-8B07-0D937B9A76A2"); }
        }
    }
}