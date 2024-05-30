using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Components.BuildingConstructors
{
    public class CreateComplexBuilding : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateComplexBuilding class.
        /// </summary>
        public CreateComplexBuilding()
          : base("CreateComplexBuilding", "Nickname",
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
            pManager.AddBrepParameter("BuildingBreps", "bldbrps", "Breps outlining the buildings", GH_ParamAccess.list);
            pManager.AddIntegerParameter("xSpacing", "xs", "Distance between nodes in x-direction", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("ySpacing", "ys", "Distance between nodes in y-direction", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("MinFloorHeight", "mfh", "Minimum height for each floor", GH_ParamAccess.item, 2.7);
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
            List<Brep> breps = new List<Brep>();
            int xspace = 0;
            int yspace = 0;
            double minFlrHeight = 0;

            DA.GetData(0, ref type);
            DA.GetDataList(1, breps);
            DA.GetData(2, ref xspace);
            DA.GetData(3, ref yspace);
            DA.GetData(4, ref minFlrHeight);




            Building bld = new Building();


            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < breps.Count; i++)
            {
                Point3d cpt = AreaMassProperties.Compute(breps[i]).Centroid;
                pts.Add(cpt);
            }


            Point3d lowCpt = new Point3d();
            Point3d highCpt = new Point3d();
            Brep lowBrep = new Brep();
            Brep highBrep = new Brep();

            if (pts.Count == 1)
            {
                lowCpt = pts[0];
                lowBrep = breps[0];
                highCpt = pts[0];
                highBrep = breps[0];
            }
            else
            {
                List<double> zVals = new List<double>();
                for (int i = 0; i < pts.Count; i++)
                {
                    zVals.Add(pts[i].Z);
                }

                double zmin = zVals.Min();
                double zmax = zVals.Max();

                int idxmin = zVals.IndexOf(zmin);
                int idxmax = zVals.IndexOf(zmax);

                lowCpt = pts[idxmin];
                lowBrep = breps[idxmin];
                highCpt = pts[idxmax];
                highBrep = breps[idxmax];
            }


            int idx = 0;
            for (int i = 0; i < lowBrep.Surfaces.Count; i++)
            {
                Point3d cptSrf = AreaMassProperties.Compute(lowBrep.Surfaces[i]).Centroid;
                if (cptSrf.Z < lowCpt.Z)
                {
                    idx = i;
                }
            }

            Plane pln;
            lowBrep.Surfaces[idx].TryGetPlane(out pln);

            Vector3d vec1 = pln.XAxis;
            Vector3d vec2 = pln.YAxis;

            Point3d cptSurface = AreaMassProperties.Compute(lowBrep.Surfaces[idx]).Centroid;
            pln.Origin = cptSurface;

            vec1.Unitize();
            vec2.Unitize();

            vec1 = vec1 * xspace;
            vec2 = vec2 * yspace;


            double totHeight = 0;
            for (int i = 0; i < highBrep.Surfaces.Count; i++)
            {
                Point3d cptSrf = AreaMassProperties.Compute(highBrep.Surfaces[i]).Centroid;
                if (cptSrf.Z > highCpt.Z)
                {
                    totHeight = cptSrf.Z;
                }
            }




            var allNodes = Nodes.GetComplexNodes(breps, totHeight, minFlrHeight, cptSurface, vec1, vec2);




            List<Floor> floors = Floor.GetFloors(breps, allNodes);
            bld.floors = floors;



            List<Beam> beams = new List<Beam>();
            //List<Beam> beams = Beam.GetBeams(allNodes);
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
            get { return new Guid("BB563F12-55DE-4906-80FE-9D42E229F828"); }
        }
    }
}