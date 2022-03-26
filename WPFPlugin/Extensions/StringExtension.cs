
namespace WPFPlugin.Extensions
{
    public static class StringExtension
    {
        public static int ToInt(this string str)
        {
            var res = 0;
            int.TryParse(str.Replace(",","."), out res);
            return res;
        }
        public static double ToDouble(this string str)
        {
            var res = 0.0;
            double.TryParse(str.Replace(",", "."), out res);
            return res;
        }
    }
}
