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

        public BuildingModel()
        {
            body = new Body();
            columns = new List<Beam>();
            beams = new List<Beam>();
        }

        public void InsertBody(Model model)
        {
            /* var yDist = 0.0;

             for (var y = 0; y < body.ySteps.Count; y++)
             {
                 var xDist = 0.0;
                 var curZ = body.columnH_0;
                 yDist += body.ySteps[y];
                 for (var x = 0; x < body.xSteps.Count; x++)
                 {
                     xDist += body.xSteps[x];
                     var firstPoint = new Point(xDist, yDist, 0);
                     var secondPoint = firstPoint.Move(new Vector(0, 0, 1), curZ);
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
                 curZ = body.columnH_1;
             }*/
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
                    var secondPoint = firstPoint.Move(new Vector(0, 0, 1), y%2==0?body.columnH_0:body.columnH_1);
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





            for (var i = 0; i < columns.Count; i += 2)
            {
                var beam = new Beam(columns[i].EndPoint, columns[i + 1].EndPoint)
                {
                    Name = $"Beam {beams.Count + 1}",
                    Profile = {ProfileString = body.beamProfileString},
                    Material = {MaterialString = body.materialString},
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

    }
}
