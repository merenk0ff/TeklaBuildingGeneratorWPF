using Tekla.Structures.Model;
using WPFPlugin.Profiles;

namespace WPFPlugin.Extensions
{
    public static class PartExtension
    {
        /// <summary>
        /// Определяет тип профиля
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static ProfileType GetProfileType(this Part part)
        {
            var str = "";
            part.GetReportProperty("PROFILE_TYPE", ref str);
            switch (str)
            {
                case "I":
                    return ProfileType.Ibeam;
                case "L":
                    return ProfileType.Lbeam;
                case "U":
                    var flangeThickness = 0.0;
                    part.GetReportProperty("PROFILE.FLANGE_THICKNESS", ref flangeThickness);
                    return flangeThickness != 0.0 ? ProfileType.U1Beam : ProfileType.U2Beam;

                case "T":
                    return ProfileType.Tbeam;
                case "B":
                    if (part is Beam || part is PolyBeam)
                        return ProfileType.WeldedI;
                    if (part is ContourPlate)
                        return ProfileType.contourPlateEn;
                    double height = 0.0;
                    double width = 0.0;
                    part.GetReportProperty("PROFILE.HEIGHT", ref height);
                    part.GetReportProperty("PROFILE.WIDTH", ref width);
                    if (width == height)
                        return ProfileType.SquarePlateProfile;
                    break;
                case "RO":
                    return ProfileType.RoundTube;
                case "RU":
                    return ProfileType.Round;
            }
            return str == "M" ? ProfileType.SqareTube : ProfileType.anotherProfile;
        }
    }
}
