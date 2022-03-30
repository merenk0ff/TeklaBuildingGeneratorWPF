using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace WPFPlugin.Extensions
{
    public static class LineSegmentExtension
    {
        public static Point CenterPoint(this LineSegment lineSegment)
        {
            return new Point((lineSegment.Point1.X + lineSegment.Point2.X) / 2, (lineSegment.Point1.Y + lineSegment.Point2.Y) / 2, (lineSegment.Point1.Z + lineSegment.Point2.Z) / 2);
        }
        public static Line ToLine(this LineSegment ls)
        {
            return new Line(ls);
        }

        public static void Draw(this LineSegment ls, double extension = 0)
        {
            var cl = new ControlLine(ls, false);
            cl.Extension = extension;
            cl.Insert();
        }
    }
}
