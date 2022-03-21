using Tekla.Structures.Model;
using WPFPlugin.Profiles;

namespace WPFPlugin.Extensions
{
    public static class BeamExtension
    {
        /// <summary>
        /// Get beam height
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static double GetHeight(this Beam beam)
        {
            var profileType = beam.GetProfileType();
            var height = ProfileSizes.GetProfileHeight(beam, profileType);
            return height;
        }
    }
}
