using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using Rhino.Geometry;

namespace MasterThesisHege.Components
{
    public class Loads : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Loads class.
        /// </summary>
        public Loads()
          : base("Loads", "Nickname",
              "Description",
              "MasterThesis", "Loads")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Building", "bld", "Class building", GH_ParamAccess.item);
            pManager.AddNumberParameter("Loads", "l", "Loads on each floor", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("LoadVectors", "lv", "Load vectors for each floor", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Building building = new Building();
            List<double> loadVal = new List<double>();

            DA.GetData(0, ref building);
            DA.GetDataList(1, loadVal);



            var bldFloors = building.floors;
            List<Vector3d> loadVec = new List<Vector3d>();

            if (loadVal.Count == bldFloors.Count)
            {
                for (int i = 0; i < loadVal.Count; i++)
                {
                    Vector3d vec = new Vector3d(0, 0, -1 * loadVal[i]);
                    loadVec.Add(vec);
                }
            }

            else if (loadVal.Count < bldFloors.Count)
            {
                for (int i = 0; i < loadVal.Count; i++)
                {
                    Vector3d vec = new Vector3d(0, 0, -1 * loadVal[i]);
                    loadVec.Add(vec);
                }
                for (int j = loadVal.Count; j < bldFloors.Count; j++)
                {
                    Vector3d vec = new Vector3d(0, 0, -1 * loadVal[loadVal.Count - 1]);
                    loadVec.Add(vec);
                }
            }

            else if (loadVal.Count > bldFloors.Count)
            {
                for (int i = 0; i < bldFloors.Count; i++)
                {
                    Vector3d vec = new Vector3d(0, 0, -1 * loadVal[i]);
                    loadVec.Add(vec);
                }
            }



            DA.SetDataList(0, loadVec);
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
            get { return new Guid("ED7AFE22-7312-4D69-B2E3-0E1F2DABA25A"); }
        }
    }
}