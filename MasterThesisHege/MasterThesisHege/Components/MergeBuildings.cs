using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class MergeBuildings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MergeBuildings class.
        /// </summary>
        public MergeBuildings()
          : base("MergeBuildings", "Nickname",
              "Description",
              "MasterThesis", "Modify")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Buildings", "bld", "Classes building", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Building> buildings = new List<Building> ();
            DA.GetDataList(0, buildings);



            Building bld = new Building();

            List<Floor> floors = new List<Floor>();
            List<Wall> walls = new List<Wall>();
            List<Column> columns = new List<Column>();
            List<Beam> beams = new List<Beam>();
            List<Support> supports = new List<Support>();

            for (int i = 0; i < buildings.Count; i++)
            {
                foreach (Floor f in buildings[i].floors)
                {
                    floors.Add(f);
                }

                foreach (Wall w in buildings[i].walls)
                {
                    walls.Add(w);
                }

                foreach (Column c in buildings[i].columns)
                {
                    columns.Add(c);
                }

                foreach (Beam b  in buildings[i].beams)
                {
                    beams.Add(b);
                }
                
                foreach (Support s  in buildings[i].supports)
                {
                    supports.Add(s);
                }
            }

            bld.floors = floors;
            bld.walls = walls;
            bld.columns = columns;
            bld.beams = beams;
            bld.supports = supports;



            DA.SetData(0, bld);
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
            get { return new Guid("B577DBD7-A23D-43FF-9705-EA71C4715153"); }
        }
    }
}