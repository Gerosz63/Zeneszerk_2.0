using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;
using System.Windows.Controls;
using System.Windows;

namespace Zeneszerk_2._0
{
    public static class OperatorEditor
    {

        public static MainWindow mainWindow;
        /// <summary>
        /// Ez a string tartalmazza az új számok elérérsi útját (Az első elérési út a honnan részlegből)
        /// </summary>
        public static string NewSongsPath;

        /// <summary>
        /// Ez a lista tartalmazza az eddigi számok elérésiútjait ( Az elsőn kívüli összes string a honnan részlegről)
        /// </summary>
        public static List<string> SongsPaths = new List<string>();

        /// <summary>
        /// Ez a lista atartalmazza a beolvasott számok összes adatát
        /// </summary>
        public static List<MusicData> Songs = new List<MusicData>();

        /// <summary>
        /// Ez a lista csak azon zenék Idjét tartalmazza melyeket meg kell jeleníteni.
        /// </summary>
        public static List<int> DisplayedSongs = new List<int>();

        /// <summary>
        /// enum mely a szerkesztő működésést határozza meg (Új számok hozzáadása vagy az eddigiek módosítása)
        /// </summary>
        public enum EditorType
        {
            Add,
            Edit
        }

        public struct MusicData
        {
            public int Id;
            public string Path;
            public string Filename;

            public string Title;
            public List<string> Performers;
            public List<string> Genres;
            public string Album;
            public string Lyrics;
        }

        /// <summary>
        /// Ez a függvény olvassa be az elérési utak fülön megadot utak alapján a zene fájlokat
        /// </summary>
        /// <param name="editorType">Ez a paaraméterhatározza meg, hogy a függvény milyen zenéket olvasson be
        /// Add - Új szám hozzáadása áthelyezéssel a többi számhoz
        /// Edit - régi számok szerkesztése</param>
        public static void ReadSongs(EditorType editorType)
        {
            MusicData x;
            Songs.Clear();
            NewSongsPath = OperatorPaths.PathsFrom[0];
            for (int i = 1; i < 4; i++)
                if (OperatorPaths.PathsFrom[i].Length > 4)
                    SongsPaths.Add(OperatorPaths.PathsFrom[i]);

            switch (editorType)
            {
                case EditorType.Add:
                    string[] NewSongsPaths = Directory.GetFiles(NewSongsPath);
                    NewSongsPaths = NewSongsPaths.Where(y => Path.GetExtension(y) == ".mp3" || Path.GetExtension(y) == ".m4a").ToArray();
                    for (int i = 0; i < NewSongsPaths.Length; i++)
                    {
                        var songtag = TagLib.File.Create(NewSongsPaths[i]);

                        x.Id = i;
                        x.Path = NewSongsPaths[i];
                        x.Filename = Path.GetFileNameWithoutExtension(NewSongsPaths[i]);

                        if (songtag.Tag.Title == null)
                            x.Title = "";
                        else
                            x.Title = songtag.Tag.Title;
                        if (songtag.Tag.Performers.Length == 0)
                            x.Performers = new List<string>();
                        else
                            x.Performers = songtag.Tag.Performers[0].Split('&').ToList();
                        if (songtag.Tag.Genres.Length == 0)
                            x.Genres = new List<string>();
                        else
                            x.Genres = songtag.Tag.Genres[0].Split('/').ToList();
                        if (songtag.Tag.Album == null)
                            x.Album = "";
                        else
                            x.Album = songtag.Tag.Album;
                        if (songtag.Tag.Lyrics == null)
                            x.Lyrics = "";
                        else
                            x.Lyrics = songtag.Tag.Lyrics;

                        Songs.Add(x);
                    }
                    for (int i = 0; i < Songs.Count; i++)
                        DisplayedSongs.Add(i);
                    mainWindow.SongDisplayer();
                    break;
                case EditorType.Edit:
                    int idhelper = 0;
                    for (int i = 0; i < SongsPaths.Count; i++)
                    {
                        string[] SongsPathss = Directory.GetFiles(SongsPaths[i]);
                        SongsPathss = SongsPathss.Where(y => Path.GetExtension(y) == ".mp3" || Path.GetExtension(y) == ".m4a").ToArray();
                        for (int a = 0; a < SongsPathss.Length; a++)
                        {
                            var songtag = TagLib.File.Create(SongsPathss[a]);

                            x.Id = i + idhelper;
                            x.Path = SongsPathss[a];
                            x.Filename = Path.GetFileNameWithoutExtension(SongsPathss[a]);

                            if (songtag.Tag.Title == null)
                                x.Title = "";
                            else
                                x.Title = songtag.Tag.Title;
                            if (songtag.Tag.Performers.Length == 0)
                                x.Performers = new List<string>();
                            else
                                x.Performers = songtag.Tag.Performers[0].Split('&').ToList();
                            if (songtag.Tag.Genres.Length == 0)
                                x.Genres = new List<string>();
                            else
                                x.Genres = songtag.Tag.Genres[0].Split('/').ToList();
                            if (songtag.Tag.Album == null)
                                x.Album = "";
                            else
                                x.Album = songtag.Tag.Album;
                            if (songtag.Tag.Lyrics == null)
                                x.Lyrics = "";
                            else
                                x.Lyrics = songtag.Tag.Lyrics;

                            Songs.Add(x);

                        }
                        idhelper += SongsPathss.Length;
                    }
                    for (int i = 0; i < Songs.Count; i++)
                        DisplayedSongs.Add(i);
                    mainWindow.SongDisplayer();
                    break;
            }
        }
    }
}
//var tfile = TagLib.File.Create(@"C:\My audio.mp3");
//string title = tfile.Tag.Title;
//TimeSpan duration = tfile.Properties.Duration;
//Console.WriteLine("Title: {0}, duration: {1}", title, duration);

//// change title in the file
//tfile.Tag.Title = "my new title";
//tfile.Save();