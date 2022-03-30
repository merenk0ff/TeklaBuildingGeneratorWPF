using System.Collections.Generic;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using WPFPlugin.Extensions;

namespace WPFPlugin.BuildingModel
{
    public class BuildingModel
    {
        public Body body { get; set; }
        public List<Beam> columns { get; set; }
        public List<Beam> beams { get; set; }
        public List<Beam> bracesColumnHorizontal { get; set; }
        public List<Beam> xBracesVertical { get; set; }
        public List<Beam> xBracesHorizontal { get; set; }

        public BuildingModel()
        {
            body = new Body();
            columns = new List<Beam>();
            beams = new List<Beam>();
            bracesColumnHorizontal = new List<Beam>();
            xBracesHorizontal = new List<Beam>();
            xBracesVertical = new List<Beam>();
        }

        public void InsertBody(Model model)
        {
            InsertColumns();
            InsertBeams();
            InsertColumnHorizontalBraces();

            //InsertColumnXBraces
            if (body.xSteps.Count < 3)
                return;
            var startStepForXBraces = 0;
            startStepForXBraces = body.xSteps.Count / 2;

            var p1 = columns[startStepForXBraces * 2].StartPoint;
            var p2 = columns[startStepForXBraces * 2 + 2].EndPoint;
            var p4 = columns[startStepForXBraces * 2].EndPoint;
            var p3 = columns[startStepForXBraces * 2 + 2].StartPoint;
            var line1 = new Line(p1, p2);
            var line2 = new Line(p3, p4);

            var p5 = Intersection.LineToLine(line1, line2).Point1;

            xBracesVertical.Add(insertXBrace(p1, p5));
            xBracesVertical.Add(insertXBrace(p2, p5));
            xBracesVertical.Add(insertXBrace(p3, p5));
            xBracesVertical.Add(insertXBrace(p4, p5));


            var p2_1 = columns[startStepForXBraces * 2 + 1].StartPoint;
            var p2_2 = columns[startStepForXBraces * 2 + 2 + 1].EndPoint;
            var p2_4 = columns[startStepForXBraces * 2 + 1].EndPoint;
            var p2_3 = columns[startStepForXBraces * 2 + 2 + 1].StartPoint;
            var line2_1 = new Line(p2_1, p2_2);
            var line2_2 = new Line(p2_3, p2_4);

            var p2_5 = Intersection.LineToLine(line2_1, line2_2).Point1;

            xBracesVertical.Add(insertXBrace(p2_1, p2_5));
            xBracesVertical.Add(insertXBrace(p2_2, p2_5));
            xBracesVertical.Add(insertXBrace(p2_3, p2_5));
            xBracesVertical.Add(insertXBrace(p2_4, p2_5));

        }

        private Beam insertXBrace(Point p1, Point p5)
        {
            var xBrace_1 = new Beam(p1, p5)
            {
                Name = "Vertical X brace",
                Class = "8",
                Position = new Position()
                {
                    Rotation = Position.RotationEnum.BACK,
                    Depth = Position.DepthEnum.MIDDLE,
                    Plane = Position.PlaneEnum.MIDDLE
                },
                Material = {MaterialString = body.materialString},
                Profile = {ProfileString = body.xBraceVerticalProfileString}
            };
            xBrace_1.Insert();
            return xBrace_1;
        }

        private void InsertColumnHorizontalBraces()
        {
            for (var i = 0; i < beams.Count - 1; i++)
            {
                var brace = new Beam();
                brace.StartPoint = beams[i].StartPoint;
                brace.EndPoint = beams[i + 1].StartPoint;
                brace.Material.MaterialString = body.materialString;
                brace.Profile.ProfileString = body.braceProfileString;
                brace.Class = "7";
                brace.Position = new Position()
                {
                    Rotation = Position.RotationEnum.BACK,
                    Depth = Position.DepthEnum.MIDDLE,
                    Plane = Position.PlaneEnum.MIDDLE
                };

                brace.Insert();

                var brace2 = new Beam();
                brace2.StartPoint = beams[i].EndPoint;
                brace2.EndPoint = beams[i + 1].EndPoint;
                brace2.Material.MaterialString = body.materialString;
                brace2.Profile.ProfileString = body.braceProfileString;
                brace2.Class = "7";
                brace2.Position = new Position()
                {
                    Rotation = Position.RotationEnum.BACK,
                    Depth = Position.DepthEnum.MIDDLE,
                    Plane = Position.PlaneEnum.MIDDLE
                };

                brace2.Insert();
                bracesColumnHorizontal.Add(brace2);
            }
        }

        private void InsertBeams()
        {
            for (var i = 0; i < columns.Count; i += 2)
            {
                var beam = new Beam(columns[i].EndPoint, columns[i + 1].EndPoint)
                {
                    Name = $"Beam {beams.Count + 1}",
                    Profile = { ProfileString = body.beamProfileString },
                    Material = { MaterialString = body.materialString },
                    Position =
                    {
                        Rotation = Position.RotationEnum.BELOW,
                        Depth = Position.DepthEnum.MIDDLE,
                        Plane = Position.PlaneEnum.MIDDLE,
                    },
                    Class = "3",
                };
                beam.Insert();

                beams.Add(beam);
            }
        }

        protected virtual void InsertColumns()
        {
            var xDist = 0.0;

            for (var x = 0; x < body.xSteps.Count; x++)
            {
                var yDist = 0.0;
                var curZ = body.columnH_0;
                xDist += body.xSteps[x];
                for (var y = 0; y < body.ySteps.Count; y++)
                {
                    yDist += body.ySteps[y];
                    var firstPoint = new Point(xDist, yDist, 0);
                    var secondPoint = firstPoint.Move(new Vector(0, 0, 1), y % 2 == 0 ? body.columnH_0 : body.columnH_1);
                    var column = new Beam(firstPoint, secondPoint)
                    {
                        Name = $"Column {columns.Count + 1}",
                        Profile = { ProfileString = body.columnProfileString },
                        Material = { MaterialString = body.materialString },
                        Class = "4",
                        Position =
                        {
                            Rotation = Position.RotationEnum.BACK,
                            Depth = Position.DepthEnum.MIDDLE,
                            Plane = Position.PlaneEnum.MIDDLE,
                        },
                    };
                    column.Insert();

                    columns.Add(column);
                }
            }
        }
    }
}
