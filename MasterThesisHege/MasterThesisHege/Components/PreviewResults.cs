using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class PreviewResults : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PreviewResults class.
        /// </summary>
        public PreviewResults()
          : base("PreviewResults", "Nickname",
              "Description",
              "MasterThesis", "Preview")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("VolumeFloors", "VolFlrs", "Total volume of floors [m3]", GH_ParamAccess.item);
            pManager.AddNumberParameter("VolumeWalls", "VolWalls", "Total volume of walls [m3]", GH_ParamAccess.item);
            pManager.AddNumberParameter("VolumeColumns", "VolCols", "Total volume of columns [m3]", GH_ParamAccess.item);
            pManager.AddNumberParameter("VolumeBeams", "VolBms", "Total volume of beams [m3]", GH_ParamAccess.item);
            pManager.AddNumberParameter("CO2Floors", "CO2flrs", "CO2 emissions from floors [kg CO2]", GH_ParamAccess.item);
            pManager.AddNumberParameter("CO2Walls", "CO2wls", "CO2 emissions from walls [kg CO2]", GH_ParamAccess.item);
            pManager.AddNumberParameter("CO2Columns", "CO2cols", "CO2 emissions from columns [kg CO2]", GH_ParamAccess.item);
            pManager.AddNumberParameter("CO2Beams", "CO2bms", "CO2 emissions from beams [kg CO2]", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            DA.GetData(0, ref building);



            double VolFlrs = 0;
            double CO2flrs = 0;
            foreach (Floor floor in building.floors)
            {
                Brep flr = floor.geometryCS;
                double v = VolumeMassProperties.Compute(flr).Volume;
                VolFlrs += v;
                double e = Emissions.GetCO2Emission(floor.material.name, floor.material.classification);
                CO2flrs += e*v;
            }

            double VolWls = 0;
            double CO2wls = 0;
            foreach (Wall wall in building.walls)
            {
                Brep wl = wall.geometryCS;
                double v = VolumeMassProperties.Compute(wl).Volume;
                VolWls += v;
                double e = Emissions.GetCO2Emission(wall.material.name, wall.material.classification);
                CO2wls += e*v;
            }

            double VolCols = 0;
            double CO2cols = 0;
            foreach (Column column in building.columns)
            {
                double v = 0;
                if (column.material.name == "steel")
                {
                    v = Column.GetSteelVolume(column.section.width , column.section.height, column.section.thickness, column.axis);
                }
                else
                {
                    Brep col = column.geometryCS;
                    v = VolumeMassProperties.Compute(col).Volume;
                }
                VolCols += v;

                double e = Emissions.GetCO2Emission(column.material.name, column.material.classification);
                CO2cols += e*v;
            }

            double VolBms = 0;
            double CO2bms = 0;
            foreach (Beam beam in building.beams)
            {
                double v = 0;
                if (beam.material.name == "steel")
                {
                    v = Beam.GetSteelVolume(beam.section.width, beam.section.height,beam.section.thickness, beam.axis);
                }
                else
                {
                    Brep bm = beam.geometryCS;
                    v = VolumeMassProperties.Compute(bm).Volume;
                }
                VolBms += v;

                double e = Emissions.GetCO2Emission(beam.material.name, beam.material.classification);
                CO2bms += e*v;
            }



            DA.SetData(0, VolFlrs);
            DA.SetData(1, VolWls);
            DA.SetData(2, VolCols);
            DA.SetData(3, VolBms);
            DA.SetData(4, CO2flrs);
            DA.SetData(5, CO2wls);
            DA.SetData(6, CO2cols);
            DA.SetData(7, CO2bms);
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
            get { return new Guid("5D071723-E3F1-45E3-B682-F3F2DDD46A96"); }
        }
    }
}