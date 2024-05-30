using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Xml.Schema;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types.Transforms;
using MasterThesisHege.Classes;
using MasterThesisHege.Classes.Elements;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;

namespace MasterThesisHege.Components.BuildingConstructors
{
    public class CreateBuilding : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBuilding class.
        /// </summary>
        public CreateBuilding()
          : base("CreateBuilding", "Nickname",
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
            pManager.AddBrepParameter("BuildingSurface", "bldsrf", "Surface of building", GH_ParamAccess.item);
            pManager.AddIntegerParameter("xSpacing", "xs", "Distance between nodes in x-direction", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("ySpacing", "ys", "Distance between nodes in y-direction", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("FloorHeight", "fh", "Height of each floor", GH_ParamAccess.item, 2.7);
            pManager.AddIntegerParameter("NumberOfFloors", "nof", "Number of floors", GH_ParamAccess.item, 1);
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
            double fheight = 0;
            int nfl = 0;
            double rad = 0;


            DA.GetData(0, ref type);
            DA.GetData(1, ref brep);
            DA.GetData(2, ref xspace);
            DA.GetData(3, ref yspace);
            DA.GetData(4, ref fheight);
            DA.GetData(5, ref nfl);
            DA.GetData(6, ref rad);

            int numfloor = nfl + 1;




            Building bld = new Building();

            Surface srf = brep.Surfaces[0];

            Point3d cpt = AreaMassProperties.Compute(srf).Centroid;
            Rhino.Geometry.Plane cplane;
            srf.TryGetPlane(out cplane);

            Vector3d vec1 = cplane.XAxis;
            Vector3d vec2 = cplane.YAxis;

            cplane.Origin = cpt;

            vec1.Unitize();
            vec2.Unitize();

            vec1 = vec1 * xspace;
            vec2 = vec2 * yspace;

            Vector3d rotAxis = new Vector3d(0, 0, 1);

            vec1.Rotate(rad, rotAxis);
            vec2.Rotate(rad, rotAxis);
            brep.Rotate(rad, rotAxis, cpt);

            List<Curve> curves = new List<Curve>();
            foreach (var be in brep.Edges)
                curves.Add(be.ToNurbsCurve());

            Curve perimeter = Curve.JoinCurves(curves)[0];
            perimeter.Scale(0.99);




            var allNodes = Nodes.GetNodes(perimeter, numfloor, fheight, cpt, vec1, vec2);




            List<Brep> brepLst = new List<Brep>();
            brepLst.Add(brep);

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
            get { return new Guid("5ACF767A-3DDC-4025-BCB4-549C563F1170"); }
        }
    }
}