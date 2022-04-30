using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLib;
 //todo: dropdown max Height prop beállítása window height alapján
namespace Zeneszerk_2._0
{
    /// <summary>
    /// Interaction logic for MusicSetter.xaml
    /// </summary>
    public partial class MusicSetter : Window
    {
        private MainWindow MainWindow;
        /// <summary>
        /// Ez a változó tárólja azt az indexet mely arra a zenére mutat melyet szerkesztünk
        /// </summary>
        private int Index;
        public MusicSetter()
        {
            InitializeComponent();
            this.Background = Datas.PrimaryColorB;
            this.Resources["TextColor"] = Datas.SecondaryColorC;
            this.Resources["MainColor"] = Datas.PrimaryColorB;
            this.Resources["SupColor"] = Datas.SecondaryColorB;
            this.FontFamily = new FontFamily("Century Gothic");
            
            
        }
        public MusicSetter(MainWindow mainWindow, int index) : this()
        {
            MainWindow = mainWindow;
            Index = index;
            this.TextBoxFileName.Text = OperatorEditor.Songs[Index].Filename;
            this.TextBoxPerformers.Text = MainWindow.ListToString(OperatorEditor.Songs[Index].Performers);
            this.TextBoxSongTitle.Text = OperatorEditor.Songs[Index].Title;
            this.TextBoxGenres.Text = MainWindow.ListToString(OperatorEditor.Songs[Index].Genres);
            this.TextBoxAlbum.Text = OperatorEditor.Songs[Index].Album;
            this.TextBoxLyrics.Text = OperatorEditor.Songs[Index].Lyrics;
        }

        /// <summary>
        /// A vissza gomb eventje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Back();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Back();
        }

        /// <summary>
        /// A mentés gomb eventje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Save();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

       
        private void Back()
        {
            Exit();
        }
        private void Save()
        {
            //System.Windows.Forms.FolderBrowserDialog opendFile = new System.Windows.Forms.FolderBrowserDialog();
            //var result = opendFile.ShowDialog();
            //if (0 == 1)
            //{

            //}
            Exit();
        }
        private void Exit()
        {
            MainWindow.Opacity = 1;
            MainWindow.IsEnabled = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Ez az érték kell :DD</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TextBoxPerformers_GotFocus(object sender, RoutedEventArgs e)
        {
            ISMegyen.Visibility = Visibility.Visible;
            
        }
    }
}
