using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterThesisHege.Classes.Elements;
using Rhino.Geometry;

namespace MasterThesisHege.Classes
{
    public class ModifyBuilding
    {
        public static Building ModifyFloorMaterial(Building building, List<string> floorMaterial)
        {
            foreach (Floor floor in building.floors)
            {
                floor.section.name = floorMaterial[0];
                floor.material.name = floorMaterial[0];
                floor.material.classification = floorMaterial[1];

                List<string> mats = new List<string>() { "concrete", "steel", "timber" };
                List<double> dens = new List<double>() { 2500, 7850, 400 };
                for (int i = 0; i < mats.Count; i++)
                {
                    if (floorMaterial[0] == mats[i])
                    {
                        floor.material.density = dens[i];
                    }
                }
            }

            return building;
        }




        public static Building ModifyFloorSection(Building building, double floorSection)
        {
            foreach (Floor floor in building.floors)
            {
                floor.section.height = floorSection;
            }

            return building;
        }




        public static Building ModifyWallMaterial(Building building, List<string> wallMaterial)
        {
            foreach (Wall wall in building.walls)
            {
                wall.section.name = wallMaterial[0];
                wall.material.name = wallMaterial[0];
                wall.material.classification = wallMaterial[1];

                List<string> mats = new List<string>() { "concrete", "steel", "timber" };
                List<double> dens = new List<double>() { 2500, 7850, 400 };
                for (int i = 0; i < mats.Count; i++)
                {
                    if (wallMaterial[0] == mats[i])
                    {
                        wall.material.density = dens[i];
                    }
                }
            }

            return building;
        }




        public static Building ModifyWallSection(Building building, double wallSection)
        {
            foreach (Wall wall in building.walls)
            {
                wall.section.height = wallSection;
            }

            return building;
        }




        public static Building ModifyColumnMaterial(Building building, List<string> columnMaterial)
        {
            foreach (Column column in building.columns)
            {
                column.section.name = columnMaterial[0];
                column.material.name = columnMaterial[0];
                column.material.classification = columnMaterial[1];

                List<string> mats = new List<string>() { "concrete", "steel", "timber" };
                List<double> dens = new List<double>() { 2500, 7850, 400 };
                for (int i = 0; i < mats.Count; i++)
                {
                    if (columnMaterial[0] == mats[i])
                    {
                        column.material.density = dens[i];
                    }
                }
            }

            return building;
        }




        public static Building ModifyColumnSection(Building building, string columnSection)
        {
            string[] secs = columnSection.Split('x');

            if (secs.Length == 2)
            {
                double width = Convert.ToDouble(secs[0]);
                double height = Convert.ToDouble(secs[1]);

                foreach (Column column in building.columns)
                {
                    column.section.width = width;
                    column.section.height = height;
                }
            }
            if (secs.Length == 3)
            {
                double width = Convert.ToDouble(secs[0]);
                double height = Convert.ToDouble(secs[1]);
                double thickness = Convert.ToDouble(secs[2]);

                foreach (Column column in building.columns)
                {
                    column.section.width = width;
                    column.section.height = height;
                    column.section.thickness = thickness;
                }
            }

            return building;
        }




        public static Building ModifyBeamMaterial(Building building, List<string> beamMaterial)
        {
            foreach (Beam beam in building.beams)
            {
                beam.section.name = beamMaterial[0];
                beam.material.name = beamMaterial[0];
                beam.material.classification = beamMaterial[1];

                List<string> mats = new List<string>() { "concrete", "steel", "timber" };
                List<double> dens = new List<double>() { 2500, 7850, 400 };
                for (int i = 0; i < mats.Count; i++)
                {
                    if (beamMaterial[0] == mats[i])
                    {
                        beam.material.density = dens[i];
                    }
                }
            }

            return building;
        }




        public static Building ModifyBeamSection(Building building, string beamSection)
        {
            string[] secs = beamSection.Split('x');

            if (secs.Length == 2)
            {
                double width = Convert.ToDouble(secs[0]);
                double height = Convert.ToDouble(secs[1]);

                foreach (Beam beam in building.beams)
                {
                    beam.section.width = width;
                    beam.section.height = height;
                }
            }
            if (secs.Length == 3)
            {
                double width = Convert.ToDouble(secs[0]);
                double height = Convert.ToDouble(secs[1]);
                double thickness = Convert.ToDouble(secs[2]);

                foreach (Beam beam in building.beams)
                {
                    beam.section.width = width;
                    beam.section.height = height;
                    beam.section.thickness = thickness;
                }
            }

            return building;
        }
    }
}
