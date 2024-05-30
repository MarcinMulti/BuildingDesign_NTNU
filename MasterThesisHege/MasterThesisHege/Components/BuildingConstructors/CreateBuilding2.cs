using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MasterThesisHege.Classes.Elements;
using MasterThesisHege.Classes;
using Rhino.Geometry;
using static Rhino.DocObjects.PhysicallyBasedMaterial;

namespace MasterThesisHege.Components.BuildingConstructors
{
    public class CreateBuilding2 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBuilding2 class.
        /// </summary>
        public CreateBuilding2()
          : base("CreateBuilding2", "Nickname",
              "Description",
              "MasterThesis", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("BuildingType", "bt", "Integer deciding type of building", GH_ParamAccess.item, 0);
            pManager.AddBrepParameter("BuildingBrep", "bldbrp", "Brep outlining the building", GH_ParamAccess.item);
            pManager.AddIntegerParameter("xSpacing", "xs", "Distance between nodes in x-direction", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("ySpacing", "ys", "Distance between nodes in y-direction", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("FloorHeight", "mfh", "Height of each floor", GH_ParamAccess.item, 2.7);
            pManager.AddNumberParameter("RotationRadians", "rad", "Rotation of the building", GH_ParamAccess.item, 0);
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
            int type = 0;
            Brep brep = new Brep();
            int xspace = 0;
            int yspace = 0;
            double flrHeight = 0;
            double rad = 0;


            DA.GetData(0, ref type);
            DA.GetData(1, ref brep);
            DA.GetData(2, ref xspace);
            DA.GetData(3, ref yspace);
            DA.GetData(4, ref flrHeight);
            DA.GetData(5, ref rad);



            Building bld = new Building();

            Point3d cpt = AreaMassProperties.Compute(brep).Centroid;

            int idx = 0;
            for (int i = 0; i < brep.Surfaces.Count; i++)
            {
                Point3d cptSrf = AreaMassProperties.Compute(brep.Surfaces[i]).Centroid;
                if (cptSrf.Z < cpt.Z)
                {
                    idx = i;
                }
            }

            Plane pln;
            brep.Surfaces[idx].TryGetPlane(out pln);

            Vector3d vec1 = pln.XAxis;
            Vector3d vec2 = pln.YAxis;

            Point3d cptSurface = AreaMassProperties.Compute(brep.Surfaces[idx]).Centroid;
            pln.Origin = cptSurface;

            vec1.Unitize();
            vec2.Unitize();

            vec1 = vec1 * xspace;
            vec2 = vec2 * yspace;

            Vector3d rotAxis = new Vector3d(0, 0, 1);

            vec1.Rotate(rad, rotAxis);
            vec2.Rotate(rad, rotAxis);
            brep.Rotate(rad, rotAxis, cpt);


            double totHeight = 0;
            for (int i = 0; i < brep.Surfaces.Count; i++)
            {
                Point3d cptSrf = AreaMassProperties.Compute(brep.Surfaces[i]).Centroid;
                if (cptSrf.Z > cpt.Z)
                {
                    totHeight = cptSrf.Z;
                }
            }




            List<Brep> brepLst = new List<Brep>();
            brepLst.Add(brep);

            var allNodes = Nodes.GetComplexNodes(brepLst, totHeight, flrHeight, cptSurface, vec1, vec2);




            List<Floor> floors = Floor.GetFloors(brepLst, allNodes);
            bld.floors = floors;




            List<Beam> beams = Beam.GetBeams(allNodes);
            bld.beams = beams;




            List<Wall> walls = new List<Wall>();
            List<Line> supportLines = new List<Line>();

            if (type == 0)
            {

            }
            else if (type == 1)
            {
                walls = Wall.GetWalls(allNodes);
                supportLines = Support.GetSupportLines(allNodes);
            }
            else if (type == 2)
            {
                walls = Wall.GetInnerWalls(allNodes);
                supportLines = Support.GetInnerSupportLines(allNodes);
            }

            bld.walls = walls;





            List<Column> columns = new List<Column>();
            List<Point3d> supportPoints = new List<Point3d>();

            if (type == 0)
            {
                columns = Column.GetColumns(allNodes);
                supportPoints = Support.GetSupportPoints(allNodes);
            }
            else if (type == 1)
            {

            }
            else if (type == 2)
            {
                columns = Column.GetOuterColumns(allNodes);
                supportPoints = Support.GetOuterSupportPoints(allNodes);
            }

            bld.columns = columns;




            List<Support> supports = new List<Support>();

            for (int i = 0; i < supportLines.Count; i++)
            {
                Support support = new Support();
                support.supportLine = supportLines[i];
                support.supportPoint = Point3d.Unset;
                supports.Add(support);
            }
            for (int j = 0; j < supportPoints.Count; j++)
            {
                Support support = new Support();
                support.supportPoint = supportPoints[j];
                support.supportLine = Line.Unset;
                supports.Add(support);
            }

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
            get { return new Guid("F86D1EE9-291C-402E-AAC3-F54C3D614AAF"); }
        }
    }
}