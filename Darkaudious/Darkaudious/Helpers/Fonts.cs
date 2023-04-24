using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Darkaudious.Helpers
{
    public static class Fonts
    {
        public enum FontName
        {
            TechnologyRegular,
            TechnologyBold,
            Aggressor,
            XTypewriterRegular,
            XTypewriterBold,
            Mech,
            GhostClan,
            Techfont
        }
        static Dictionary<FontName, string> fontDictionary;

        public static void Init()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                fontDictionary = new Dictionary<FontName, string>
                {
                    [FontName.TechnologyRegular] = "Technology.ttf#Technology",
                    [FontName.TechnologyBold] = "Technology-Bold.ttf#TechnologyBold",
                    [FontName.Aggressor] = "Aggressor.ttf#Aggressor",
                    [FontName.XTypewriterRegular] = "XTypewriterRegular.ttf#XTypewriterRegular",
                    [FontName.XTypewriterBold] = "XTypewriterBold.ttf#XTypewriterBold",
                    [FontName.GhostClan] = "ghostclanleft.ttf#ghostclanleft",
                    [FontName.Mech] = "mech.ttf#mech",
                    [FontName.Techfont] = "Techfont.ttf#Techfont"
                };
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                fontDictionary = new Dictionary<FontName, string>
                {
                    [FontName.TechnologyRegular] = "Technology",
                    [FontName.TechnologyBold] = "Technology-Bold",
                    [FontName.Aggressor] = "Aggressor",
                    [FontName.XTypewriterRegular] = "XTypewriterRegular",
                    [FontName.XTypewriterBold] = "XTypewriterBold",
                    [FontName.GhostClan] = "ghostclanleft",
                    [FontName.Mech] = "mech",
                    [FontName.Techfont] = "Techfont"

                };
            }
        }

        public static string GetFont(FontName font)
        {
            return fontDictionary[font];
        }

        public static string GetRegularAppFont()
        {
            return GetFont(FontName.Mech);
            //return GetFont(FontName.TechnologyRegular);
        }

        public static string GetBoldAppFont()
        {
            return GetRegularAppFont();// GetFont(FontName.TechnologyBold);
        }

        public static string GetHeaderFont()
        {
            return GetFont(FontName.Mech);
        }
    }
}

