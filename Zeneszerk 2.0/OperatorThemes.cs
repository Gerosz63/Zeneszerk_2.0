using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Diagnostics;

namespace Zeneszerk_2._0
{
    public static class OperatorThemes
    {
        /// <summary>
        /// A témákat tartalmazó file elérési útja
        /// </summary>
        private static string path = "Themes.txt";

        public static MainWindow mainWindow;

        static private int activeThemeIndex;

        public static int ActiveThemeIndex
        {
            get
            {
                return activeThemeIndex;
            }
            set
            {
                colorCodes[activeThemeIndex].Active = false;
                if (colorCodes[value].PrimaryColor != "-" && colorCodes[value].SecundaryColor != "-")
                    activeThemeIndex = value;
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (colorCodes[i].PrimaryColor != "-" && colorCodes[i].SecundaryColor != "-")
                        {
                            activeThemeIndex = i;
                            break;
                        }
                    }
                }
                colorCodes[activeThemeIndex].Active = true;
                mainWindow.SetXamlColorVariablesValue();
                mainWindow.SetColorsAndAppear();
                FileWriter();
            }
        }


        /// <summary>
        /// Egy struktúra az elsődleges és másodlagos szín tárolására
        /// </summary>
        public struct ColorCodes
        {
            public string PrimaryColor;
            public string SecundaryColor;
            public bool Active;
        }
        /// <summary>
        /// Ez a tömb tartalmazza a témák színpárjait MAX 10!!
        /// </summary>
        public static ColorCodes[] colorCodes = new ColorCodes[10];


        /// <summary>
        /// Ez a függvény olvassa ki a témákat tároló fileból az adatokat, ha file nem létezik létrehoz egyet.
        /// </summary>
        public static void ThemesTextReader()
        {
            if (!File.Exists(path))
            {
                ColorCodes x;
                StreamWriter txt = File.CreateText(path);

                activeThemeIndex = 0;
                txt.WriteLine("#FC766A;#5B84B1;True");
                x.PrimaryColor = "#FC766A";
                x.SecundaryColor = "#5B84B1";
                x.Active = true;
                colorCodes[0] = x;
                for (int i = 0; i < 9; i++)
                {
                    txt.WriteLine("-;-;False"); // Eredeti: txt.WriteLine("#FFFFFF;#FFFFFF;false");
                    x.PrimaryColor = "-";
                    x.SecundaryColor = "-";
                    x.Active = false;
                    colorCodes[i + 1] = x;
                }                  
                txt.Close();
            }
            else
            {
                ColorCodes x;
                StreamReader txt = new StreamReader(path);
                for (int i = 0; i < 10; i++)
                {
                    string[] helper = txt.ReadLine().Split(';');
                    x.PrimaryColor = helper[0];
                    x.SecundaryColor = helper[1];
                    x.Active = helper[2] == "True" ? true : false;
                    if (x.Active)
                        activeThemeIndex = i;
                    colorCodes[i] = x;
                }
                if (colorCodes.Count(X => X.Active == true) != 1)
                {
                    colorCodes.FirstOrDefault(y => y.Active == true); //A kérdés az, hogy Null e default érték??? 
                }
                txt.Close();
            }
        }

        /// <summary>
        /// Megadja az éppen aktív téma színpárosát !!# jellel együtt!!
        /// </summary>
        /// <returns>Egy string tömb melynek első helyén az elsődleges, második helyén pedig a másodlagos szín HEX kódja található</returns>
        public static string[] GetActiveTheme()
        {
            string[] result = new string[2];
            result[0] = colorCodes[ActiveThemeIndex].PrimaryColor;
            result[1] = colorCodes[ActiveThemeIndex].SecundaryColor;
            return result;
        }
        public static void FileWriter()
        {
            StreamWriter txt = new StreamWriter(path);
            for (int i = 0; i < colorCodes.Length; i++)
            {
                txt.WriteLine(colorCodes[i].PrimaryColor + ";" + colorCodes[i].SecundaryColor + ";" + colorCodes[i].Active);
            }
            txt.Close();
        }
    }
}
