using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.FileIO;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class PreviewBuilding : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PreviewBuilding class.
        /// </summary>
        public PreviewBuilding()
          : base("PreviewBuilding", "Nickname",
              "Description",
              "MasterThesis", "Preview")
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
            pManager.AddBrepParameter("Floors", "flrs", "Floors in the building", GH_ParamAccess.list);
            pManager.AddBrepParameter("Walls", "wls", "Walls in the building", GH_ParamAccess.list);
            pManager.AddLineParameter("Columns", "cols", "Columns in the building", GH_ParamAccess.list);
            pManager.AddLineParameter("Beams", "bms", "Beams in the building", GH_ParamAccess.list);
            pManager.AddBrepParameter("FloorsCS", "flrsCS", "Cross-section of floors in the building", GH_ParamAccess.list);
            pManager.AddBrepParameter("WallsCS", "wlsCS", "Cross-section of walls in the building", GH_ParamAccess.list);
            pManager.AddBrepParameter("ColumnsCS", "colsCS", "Cross-section of columns in the building", GH_ParamAccess.list);
            pManager.AddBrepParameter("BeamsCS", "bmsCS", "Cross-section of beams in the building", GH_ParamAccess.list);
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            DA.GetData(0, ref building);



            List<Brep> floors = new List<Brep>();
            List<Brep> walls = new List<Brep>();
            List<Line> columns = new List<Line>();
            List<Line> beams = new List<Line>();

            List<Brep> floorsCS = new List<Brep>();
            List<Brep> wallsCS = new List<Brep>();
            List<Brep> columnsCS = new List<Brep>();
            List<Brep> beamsCS = new List<Brep>();

            foreach (Floor floor in building.floors)
            {
                floors.Add(floor.geometry);


                double t = floor.section.height / 100;
                floor.geometryCS = Floor.CreateCSGeometry(floor.geometry, t);

                floorsCS.Add(floor.geometryCS);
            }

            foreach (Wall wall in building.walls)
            {
                walls.Add(wall.geometry);


                double t = wall.section.height / 100;
                wall.geometryCS = Wall.CreateCSGeometry(wall.geometry, t);
                
                wallsCS.Add(wall.geometryCS);
            }

            foreach (Column column in building.columns)
            {
                columns.Add(column.axis);


                double width = column.section.width / 100;
                double height = column.section.height / 100;
                double thickness = column.section.thickness / 100;
                string material = column.material.name;
                column.geometryCS = Column.CreateCSGeometry(column.axis, width, height, thickness, material);
                
                columnsCS.Add(column.geometryCS);
            }

            foreach (Beam beam in building.beams)
            {
                beams.Add(beam.axis);


                double width = beam.section.width / 100;
                double height = beam.section.height / 100;
                double thickness = beam.section.thickness / 100;
                string material = beam.material.name;
                beam.geometryCS = Beam.CreateCSGeometry(beam.axis, width, height, thickness, material);
                
                beamsCS.Add(beam.geometryCS);
            }



            DA.SetDataList(0, floors);
            DA.SetDataList(1, walls);
            DA.SetDataList(2, columns);
            DA.SetDataList(3, beams);
            DA.SetDataList(4, floorsCS);
            DA.SetDataList(5, wallsCS);
            DA.SetDataList(6, columnsCS);
            DA.SetDataList(7, beamsCS);
            DA.SetData(8, building);
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
            get { return new Guid("8EF21E9B-0988-41D6-8D32-0979875D9CD2"); }
        }
    }
}