using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MasterThesisHege.Classes
{
    public class Support
    {
        public Point3d supportPoint;
        public Line supportLine;




        public static List<Line> GetSupportLines(List<List<List<Point3d>>> allNodes)
        {
            List<Line> supportLines = new List<Line>();
            for (int j = 0; j < allNodes[0].Count; j++)
            {
                for (int k = 0; k < allNodes[0][j].Count - 1; k++)
                {
                    Line sLine = new Line(allNodes[0][j][k], allNodes[0][j][k + 1]);
                    supportLines.Add(sLine);
                }
            }
            return supportLines;
        }




        public static List<Line> GetInnerSupportLines(List<List<List<Point3d>>> allNodes)
        {
            List<Line> supportLines = new List<Line>();
            for (int j = 1; j < allNodes[0].Count - 1; j++)
            {
                for (int k = 0; k < allNodes[0][j].Count - 1; k++)
                {
                    Line sLine = new Line(allNodes[0][j][k], allNodes[0][j][k + 1]);
                    supportLines.Add(sLine);
                }
            }
            return supportLines;
        }




        public static List<Point3d> GetSupportPoints(List<List<List<Point3d>>> allNodes)
        {
            List<Point3d> supportPoints = new List<Point3d>();
            for (int j = 0; j < allNodes[0].Count; j++)
            {
                for (int k = 0; k < allNodes[0][j].Count; k++)
                {
                    supportPoints.Add(allNodes[0][j][k]);
                }
            }
            return supportPoints;
        }




        public static List<Point3d> GetOuterSupportPoints(List<List<List<Point3d>>> allNodes)
        {
            List<Point3d> supportPoints = new List<Point3d>();
            int jcnt = allNodes[0].Count - 1;
            for (int j = 0; j < allNodes[0].Count; j += jcnt)
            {
                for (int k = 0; k < allNodes[0][j].Count; k++)
                {
                    supportPoints.Add(allNodes[0][j][k]);
                }
            }
            return supportPoints;
        }
    }
}
