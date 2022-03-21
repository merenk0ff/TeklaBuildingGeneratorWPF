using System;
using System.Collections.Generic;
using Tekla.Structures.Model;

namespace WPFPlugin.Profiles
{
    public class ProfileSizes
    {
        public double Heigth { get; set; }
        public double Width { get; set; }
        /// <summary>
        /// Толщина стенки для двутавра.
        /// </summary>
        public double WebT { get; set; }
        /// <summary>
        /// Толщина верхней полки для двутавра.
        /// </summary>
        public double FlangeUpperT { get; set; }
        /// <summary>
        /// Толщина нижней полки двутавра
        /// </summary>
        public double FlangeLowerT { get; set; }
        /// <summary>
        /// Радиус скругления внутренний
        /// </summary>
        public double Radius { get; set; }


        /// <summary>
        /// Возвращает геометрические размеры сечения
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="profileType"></param>
        /// <returns></returns>
        public static ProfileSizes GetProfileSizes(List<Beam> beam, ProfileType profileType)
        {
            ProfileSizes profileSizes = new ProfileSizes();
            switch (profileType)
            {
                case ProfileType.Ibeam:
                    profileSizes = getIBeamProfileSizes(beam[0]);
                    break;
                case ProfileType.U1Beam:
                case ProfileType.U2Beam:
                    profileSizes = getUBeamProfileSizes(beam[0]);
                    break;
                case ProfileType.Lbeam:
                    profileSizes = getLBeamProfileSizes(beam[0]);
                    break;
                case ProfileType.WeldedI:
                    var list = new List<Part>() { beam[0], beam[1], beam[2] };
                    profileSizes = getWeldedIsizes(list);
                    break;
                case ProfileType.contourPlateEn:
                    //  
                    break;
                case ProfileType.SqareTube:
                    profileSizes = getSquareTubeBeamProfileSizes(beam[0]);
                    break;
                default:
                    break;
            }
            return profileSizes;
        }

        /// <summary>
        /// Размеры сечения для сварного двутавра
        /// </summary>
        /// <param name="weldedBeam"></param>
        /// <returns></returns>
        public static ProfileSizes getWeldedIsizes(List<Part> weldedBeam)
        {
            var ps = new ProfileSizes();
            ps.Heigth = GetProfileHeight(weldedBeam[0], ProfileType.contourPlateEn) + GetPlateT(weldedBeam[1]) + GetPlateT(weldedBeam[2]);
            ps.Width = Math.Max(GetProfileHeight(weldedBeam[1], ProfileType.contourPlateEn), GetProfileHeight(weldedBeam[2], ProfileType.contourPlateEn));
            ps.WebT = GetPlateT(weldedBeam[0]);
            ps.FlangeUpperT = GetPlateT(weldedBeam[1]);
            ps.FlangeLowerT = GetPlateT(weldedBeam[2]);
            ps.Radius = 0;
            return ps;
        }
        /// <summary>
        /// Размеры сечения для прокатного двутавра
        /// </summary>
        /// <param name="beam"></param>
        /// <returns></returns>
        public static ProfileSizes getIBeamProfileSizes(Beam beam)
        {
            var ps = new ProfileSizes
            {
                Heigth = GetProfileHeight((Part)beam, ProfileType.Ibeam),
                Width = GetProfileWidth((Part)beam, ProfileType.Ibeam),
                WebT = GetProfileWebT((Part)beam, ProfileType.Ibeam),
                FlangeUpperT = GetProfileFlangeT((Part)beam, ProfileType.Ibeam),
                FlangeLowerT = GetProfileFlangeT((Part)beam, ProfileType.Ibeam),
                Radius = GetR((Part)beam)
            };


            return ps;
        }

        /// <summary>
        /// Размеры сечения для уголка
        /// </summary>
        /// <param name="_part"></param>
        /// <returns></returns>
        public static ProfileSizes getLBeamProfileSizes(Part _part)
        {
            var _ps = new ProfileSizes();
            _ps.Heigth = GetProfileHeight(_part, ProfileType.Lbeam);
            _ps.Width = GetProfileWidth(_part, ProfileType.Lbeam);
            _ps.WebT = GetProfileWebT(_part, ProfileType.Lbeam);
            _ps.FlangeUpperT = GetProfileFlangeT(_part, ProfileType.Lbeam);
            _ps.FlangeLowerT = _ps.FlangeUpperT;
            _ps.Radius = GetR(_part);
            return _ps;
        }

        /// <summary>
        /// Размеры сечения для квадратной трубы
        /// </summary>
        /// <param name="_part"></param>
        /// <returns></returns>
        public static ProfileSizes getSquareTubeBeamProfileSizes(Part _part)
        {
            var _ps = new ProfileSizes();
            _ps.Heigth = GetProfileHeight(_part, ProfileType.SqareTube);
            _ps.Width = GetProfileWidth(_part, ProfileType.SqareTube);
            _ps.WebT = GetProfileWebT(_part, ProfileType.SqareTube);
            _ps.FlangeUpperT = GetProfileFlangeT(_part, ProfileType.SqareTube);
            _ps.FlangeLowerT = GetProfileFlangeT(_part, ProfileType.SqareTube);
            _ps.Radius = GetR(_part);

            return _ps;
        }

        /// <summary>
        /// Размеры сечения для швеллера
        /// </summary>
        /// <param name="_part"></param>
        /// <returns></returns>
        public static ProfileSizes getUBeamProfileSizes(Part _part)
        {
            var _ps = new ProfileSizes();
            _ps.Heigth = GetProfileHeight(_part, ProfileType.U1Beam);
            _ps.Width = GetProfileWidth(_part, ProfileType.U1Beam);
            _ps.WebT = GetProfileWebT(_part, ProfileType.U1Beam);
            _ps.FlangeUpperT = GetProfileFlangeT(_part, ProfileType.U1Beam);
            _ps.FlangeLowerT = GetProfileFlangeT(_part, ProfileType.U1Beam);
            _ps.Radius = GetR(_part);

            return _ps;
        }

        /// <summary>
        /// Получить высоту профиля
        /// </summary>
        /// <param name="part">Деталь</param>
        /// <param name="profileType">Тип профиля</param>
        /// <returns></returns>
        public static double GetProfileHeight(Part part, ProfileType profileType)
        {
            var height = 0.0;
            switch (profileType)
            {
                case ProfileType.RoundTube:
                case ProfileType.Round:
                    part.GetReportProperty("PROFILE.DIAMETER", ref height);
                    break;
                case ProfileType.SqareTube:
                case ProfileType.SquarePlateProfile:
                    part.GetReportProperty("PROFILE.HEIGHT", ref height);
                    break;
                default:
                    part.GetReportProperty("PROFILE.HEIGHT", ref height);
                    break;
            }
            if (height == 0.0)
                part.GetReportProperty("HEIGHT", ref height);
            return height;
        }
        /// <summary>
        /// Получить ширину профиля
        /// </summary>
        /// <param name="part">Деталь</param>
        /// <param name="pofileType">Тип профиля</param>
        /// <returns></returns>
        public static double GetProfileWidth(Part part, ProfileType pofileType)
        {
            var width = 0.0;
            switch (pofileType)
            {
                case ProfileType.RoundTube:
                case ProfileType.Round:
                    part.GetReportProperty("PROFILE.DIAMETER", ref width);
                    break;
                case ProfileType.SqareTube:
                case ProfileType.SquarePlateProfile:
                    part.GetReportProperty("PROFILE.WIDTH", ref width);
                    break;
                default:
                    part.GetReportProperty("PROFILE.WIDTH", ref width);
                    break;
            }
            if (width == 0.0)
                part.GetReportProperty("WIDTH", ref width);
            return width;
        }
        /// <summary>
        /// Определить толщину стенки
        /// </summary>
        /// <param name="part">Деталь</param>
        /// <param name="profileType">Тип профиля</param>
        /// <returns></returns>
        public static double GetProfileWebT(Part part, ProfileType profileType)
        {
            var webThinkness = 0.0;

            switch (profileType)
            {
                case ProfileType.U2Beam:
                case ProfileType.RoundTube:
                case ProfileType.SqareTube:
                    part.GetReportProperty("PROFILE.PLATE_THICKNESS", ref webThinkness);
                    break;
                case ProfileType.Lbeam:
                    part.GetReportProperty("PROFILE.FLANGE_THICKNESS_1", ref webThinkness);
                    break;
                case ProfileType.WeldedI:
                    webThinkness = GetPlateT(part);
                    break;
                default:
                    part.GetReportProperty("PROFILE.WEB_THICKNESS", ref webThinkness);
                    break;
            }

            return webThinkness;
        }
        /// <summary>
        /// Определить толщину пластины
        /// </summary>
        /// <param name="plate">Пластина</param>
        /// <returns></returns>
        public static double GetPlateT(Part plate)
        {
            if (plate == null)
                return 0.0;
            var num1 = 0.0;
            var num2 = 0.0;
            plate.GetReportProperty("PROFILE.HEIGHT", ref num1);
            plate.GetReportProperty("PROFILE.WIDTH", ref num2);
            return num1 < num2 ? num1 : num2;
            }
        /// <summary>
        /// Определить толщину полки
        /// </summary>
        /// <param name="part">Деталь</param>
        /// <param name="profileType">Тип профиля</param>
        /// <returns></returns>
        public static double GetProfileFlangeT(Part part, ProfileType profileType)
        {
            var ft = 0.0;
            switch (profileType)
            {
                case ProfileType.U2Beam:
                case ProfileType.RoundTube:
                case ProfileType.SqareTube:
                    part.GetReportProperty("PROFILE.PLATE_THICKNESS", ref ft);
                    break;
                case ProfileType.Lbeam:
                    part.GetReportProperty("PROFILE.PLATE_THICKNESS", ref ft);
                    break;
                default:
                    part.GetReportProperty("PROFILE.FLANGE_THICKNESS", ref ft);
                    break;
            }
            return ft;
        }
        /// <summary>
        /// Определить радиус скругления профиля. Внутренний
        /// </summary>
        /// <param name="part">Деталь</param>
        /// <returns></returns>
        public static double GetR(Part part)
        {
            var _r = 0.0;
            part.GetReportProperty("PROFILE.ROUNDING_RADIUS_1", ref _r);
            return _r;
        }
    }
}
