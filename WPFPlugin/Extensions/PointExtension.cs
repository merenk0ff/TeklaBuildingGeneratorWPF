using Tekla.Structures.Geometry3d;

namespace WPFPlugin.Extensions
{
    public static class PointExtension
    {
        public static Point Move(this Point point, Vector direction, double distance)
        {
            var vector = new Vector(direction.X, direction.Y, direction.Z);
            var outPoint = new Point(point);
            vector.Normalize(distance);
            outPoint.Translate(vector.X, vector.Y, vector.Z);
            return outPoint;
        }
        public static Point ProjectionOnPlane(this Point point, GeometricPlane gp)
        {
            return Projection.PointToPlane(point, gp);
        }
        public static Point New(this Point point)
        {
            return new Point(point.X + 0.0, point.Y + 0.0, point.Z + 0.0);
        }
    }
}
