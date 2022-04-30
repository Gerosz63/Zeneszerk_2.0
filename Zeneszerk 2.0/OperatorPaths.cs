using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.TextFormatting;

namespace Zeneszerk_2._0
{
    public static class OperatorPaths
    {
        /// <summary>
        /// Ez a lista tartalmazza a "Honnan" elérési utakat.
        /// Max 4 elem
        /// Ha az adott elem értéke "-" akkor az az elérési út nem hiteles kezelni kell!!!
        /// </summary>
        public static List<string> PathsFrom = new List<string>();

        /// <summary>
        /// Ez a lista tartalmazza a "Hova" elérési utakat.
        /// Max 4 elem
        /// Ha az adott elem értéke "-" akkor az az elérési út nem hiteles kezelni kell!!!
        /// </summary>
        public static List<string> PathsTo = new List<string>();

        /// <summary>
        /// Ez a változó tárolja a Checker() függvény eredményét.
        /// </summary>
        private static bool IsOk;

        /// <summary>
        /// Ez a változó tárolja a File elérési útját.
        /// </summary>
        private static readonly string TxtName = "Paths.txt";



        /// <summary>
        /// Ez a függvény olvassa be az adatokat a elérési utakat tároló txt file-ból, ha a file nem létezik akkor pedig elkészíti azt.
        /// </summary>
        public static void PathsTextReader()
        {
            PathsFrom.Clear();
            PathsTo.Clear();
            if (!File.Exists(TxtName))
            {               
                StreamWriter txt = File.CreateText(TxtName);
                string helper = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                txt.WriteLine(helper); // Alap új zenék elérési útja
                PathsFrom.Add(helper);
                helper = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                txt.WriteLine(helper);
                PathsFrom.Add(helper);
                txt.WriteLine(";");
                txt.WriteLine(helper);
                PathsTo.Add(helper);
                txt.Close();
            }
            else
            {
                StreamReader txt = new StreamReader(TxtName);
                string helper;

                while ((helper = txt.ReadLine()) != ";")
                    PathsFrom.Add(helper);

                while (!txt.EndOfStream)
                    PathsTo.Add(txt.ReadLine());

                txt.Close();
            }
        }
        /// <summary>
        /// Azokat a sorokat törli melyek semmit ("" vagy null) sem tartalmaznak
        /// </summary>
        public static void Sorter()
        {
            for (int i = 0; i < PathsFrom.Count; i++)
            {
                if (PathsFrom[i] == "_" || PathsFrom[i] == null)
                {
                    PathsFrom.RemoveAt(i);
                    i--;
                }

            }
            for (int i = 0; i < PathsTo.Count; i++)
            {
                if (PathsTo[i] == "_" || PathsTo[i] == null)
                {
                    PathsTo.RemoveAt(i);
                    i--;
                }
            }
            LenthCorrector();
        }

        public static void LenthCorrector()
        {
            for (int i = PathsFrom.Count; i < 4; i++)
                PathsFrom.Add("_");
            for (int i = PathsTo.Count; i < 4; i++)
                PathsTo.Add("_");
        }

        /// <summary>
        /// Ez a függvény végignézi az összes elérési utat és megvizsgálja őket, hogy léteznek e, amennyiben nem az elérési út értéket "-" állítja.
        /// </summary>
        public static void Checker()
        {
            IsOk = true;
            for (int i = 0; i < PathsFrom.Count; i++)
            {
                if (!Directory.Exists(PathsFrom[i]) && PathsFrom[i] != "_")
                {
                    PathsFrom[i] = "-";
                    IsOk = false;
                }
            }
            for (int i = 0; i < PathsTo.Count; i++)
            {
                if (!Directory.Exists(PathsTo[i]) && PathsTo[i] != "_")
                {
                    PathsTo[i] = "-";
                    IsOk = false;
                }
            }
            TextWriter();
        }

        /// <summary>
        /// Ez a függvény írja a txt filet ami tartalmazza az elérési uttal kapcsolatos adatokat.
        /// </summary>
        public static void TextWriter()
        {
            if (IsOk)
            {
                StreamWriter txt = new StreamWriter(TxtName);
                foreach (var item in PathsFrom)
                {
                    if (Directory.Exists(item))
                        txt.WriteLine(item);
                }
                txt.WriteLine(";");
                foreach (var item in PathsTo)
                {
                    if (Directory.Exists(item))
                        txt.WriteLine(item);
                }
                txt.Close();
            }
            else
                Checker();
        }
    }
}
