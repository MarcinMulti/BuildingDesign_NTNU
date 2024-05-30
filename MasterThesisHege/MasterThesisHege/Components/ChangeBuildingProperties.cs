using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class ChangeBuildingProperties : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ChangeBuildingProperties class.
        /// </summary>
        public ChangeBuildingProperties()
          : base("ChangeBuildingProperties", "Nickname",
              "Description",
              "MasterThesis", "Modify")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
            pManager.AddTextParameter("FloorMaterial", "flrMat", "Material for floors", GH_ParamAccess.list, new List<string>() { "0" });
            pManager.AddNumberParameter("FloorSection", "flrSec", "Section for floors in cm", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("WallMaterial", "wallMat", "Material for walls", GH_ParamAccess.list, new List<string>() { "0" });
            pManager.AddNumberParameter("WallSection", "wallSec", "Section for walls in cm", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("ColumnMaterial", "colMat", "Material for columns", GH_ParamAccess.list, new List<string>() { "0" });
            pManager.AddTextParameter("ColumnSection", "colSec", "Section for columns in cm", GH_ParamAccess.item, "0");
            pManager.AddTextParameter("BeamMaterial", "beamMat", "Material for beams", GH_ParamAccess.list, new List<string>() { "0" });
            pManager.AddTextParameter("BeamSection", "beamSec", "Section for beams in cm", GH_ParamAccess.item, "0");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building bld = new Building();
            List<string> flrMat = new List<string>();
            double flrSec = 0;
            List<string> wallMat = new List<string>();
            double wallSec = 0;
            List<string> colMat = new List<string>();
            string colSec = "";
            List<string> beamMat = new List<string>();
            string beamSec = "";

            DA.GetData(0, ref bld);
            DA.GetDataList(1, flrMat);
            DA.GetData(2, ref flrSec);
            DA.GetDataList(3, wallMat);
            DA.GetData(4, ref wallSec);
            DA.GetDataList(5, colMat);
            DA.GetData(6, ref colSec);
            DA.GetDataList(7, beamMat);
            DA.GetData(8, ref beamSec);



            if (flrMat[0] != "0")
            {
                bld = ModifyBuilding.ModifyFloorMaterial(bld, flrMat);
            }

            if (flrSec != 0)
            {
                bld = ModifyBuilding.ModifyFloorSection(bld, flrSec);
            }

            if (wallMat[0] != "0")
            {
                bld = ModifyBuilding.ModifyWallMaterial(bld, wallMat);
            }
            if (wallSec != 0)
            {
                bld = ModifyBuilding.ModifyWallSection(bld, wallSec);
            }

            if (colMat[0] != "0")
            {
                bld = ModifyBuilding.ModifyColumnMaterial(bld, colMat);
            }
            if (colSec != "0")
            {
                bld = ModifyBuilding.ModifyColumnSection(bld, colSec);
            }

            if (beamMat[0] != "0")
            {
                bld = ModifyBuilding.ModifyBeamMaterial(bld, beamMat);
            }
            if (beamSec != "0")
            {
                bld = ModifyBuilding.ModifyBeamSection(bld, beamSec);
            }



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
            get { return new Guid("33422A2E-503E-4A01-A05E-BC0C1FBB1BDA"); }
        }
    }
}