using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zeneszerk_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //this.Resources["TextColor"] = (Color)ColorConverter.ConvertFromString("#FC766A"); - ezzel tudok szöveg színt változtatni 
        //this.Resources["MainColor"] = (Brush)new BrushConverter().ConvertFrom("#FF5B84B1");
        //this.Resources["SupColor"] = (Brush) new BrushConverter().ConvertFrom("#FFFC766A");
        #region Build Infos
        enum States
        {
            Alpha,
            Beta,
            Released,
            Stable
        }
        static readonly States states = States.Alpha;
        static readonly string buildnbr = "1.0.0";
        #endregion Build Infos

        //Ezek a listák tárolják az elérési út beállítási menüpont alatti beviteli mezőket
        private static List<TextBox> TBPathFrom = new List<TextBox>();
        private static List<TextBox> TBPathTo = new List<TextBox>();

        //Ezek a listák tárolják az elérési út beállítási menüpont alatti sor törlő gombokat
        private static List<Button> BDeleteFrom = new List<Button>();
        private static List<Button> BDeleteTo = new List<Button>();

        // Ez a lista tartalmazza azokat a grideket melyek a témákat tartalmazzák
        private static List<Grid> GThemes = new List<Grid>();

        // És itt vannak azok a gombok, keretek melyeket a grid tartalmaz és néhány dolgot át kell rajtuk állítani runtime
        private static List<Button> BSetColorActive = new List<Button>();
        private static List<Button> BPrimaryColor = new List<Button>();
        private static List<Button> BSecundaryColor = new List<Button>();
        private static List<Border> BoPrimeryColor = new List<Border>();
        private static List<Border> BoSecundaryColor = new List<Border>();
        private static List<Button> BDeleteColor = new List<Button>();

        /// <summary>
        /// Ez a változó tárólja a ColorSetter osztály pédányát
        /// </summary>

        #region MenüVáltó
        enum ActiveMenu
        {
            Menu,
            Add,
            Edit,
            SetPaths,
            SetThemes,
            Info
        }
        private ActiveMenu activeMenus = ActiveMenu.Info;
        ActiveMenu ActiveMenus
        {
            get
            {
                return activeMenus;
            }
            set
            {
                if (activeMenus != value)
                {
                    activeMenus = value;
                    switch (activeMenus)
                    {
                        case ActiveMenu.Menu:
                            this.GridInfo.Visibility = Visibility.Hidden;
                            this.GridThemes.Visibility = Visibility.Hidden;
                            this.GridPaths.Visibility = Visibility.Hidden;
                            this.GridMenu.Visibility = Visibility.Visible;
                            this.GridMusicEditor.Visibility = Visibility.Hidden;
                            break;
                        case ActiveMenu.Add:
                            this.GridMenu.Visibility = Visibility.Hidden;
                            this.GridMusicEditor.Visibility = Visibility.Visible;

                            break;
                        case ActiveMenu.Edit:
                            this.GridMenu.Visibility = Visibility.Hidden;
                            this.GridMusicEditor.Visibility = Visibility.Visible;
                            break;
                        case ActiveMenu.SetPaths:
                            this.GridInfo.Visibility = Visibility.Hidden;
                            this.GridThemes.Visibility = Visibility.Hidden;
                            this.GridPaths.Visibility = Visibility.Visible;
                            this.GridMusicEditor.Visibility = Visibility.Hidden;
                            break;
                        case ActiveMenu.SetThemes:
                            this.GridInfo.Visibility = Visibility.Hidden;
                            this.GridThemes.Visibility = Visibility.Visible;
                            this.GridPaths.Visibility = Visibility.Hidden;
                            this.GridMusicEditor.Visibility = Visibility.Hidden;
                            break;
                        case ActiveMenu.Info:
                            this.GridInfo.Visibility = Visibility.Visible;
                            this.GridThemes.Visibility = Visibility.Hidden;
                            this.GridPaths.Visibility = Visibility.Hidden;
                            this.GridMusicEditor.Visibility = Visibility.Hidden;
                            break;
                    }
                }
            }
        }
        #endregion MenüVáltó

        public MainWindow()
        {
            OperatorThemes.ThemesTextReader();
            OperatorThemes.mainWindow = this;
            InitializeComponent();
            SetXamlColorVariablesValue();
            Setts();
        }
        private void Setts()
        {
            this.FontFamily = new FontFamily("Century Gothic");
            this.VersionDP.Content = $"{states} {buildnbr}";
            this.Foreground = Datas.PrimaryColorB;

            TBPathFrom.Add(this.TextBoxPathsFrom1);
            TBPathFrom.Add(this.TextBoxPathsFrom2);
            TBPathFrom.Add(this.TextBoxPathsFrom3);
            TBPathFrom.Add(this.TextBoxPathsFrom4);

            TBPathTo.Add(this.TextBoxPathsTo1);
            TBPathTo.Add(this.TextBoxPathsTo2);
            TBPathTo.Add(this.TextBoxPathsTo3);
            TBPathTo.Add(this.TextBoxPathsTo4);

            BDeleteFrom.Add(this.ButtonpathsFromDelete1);
            BDeleteFrom.Add(this.ButtonpathsFromDelete2);
            BDeleteFrom.Add(this.ButtonpathsFromDelete3);
            BDeleteFrom.Add(this.ButtonpathsFromDelete4);

            BDeleteTo.Add(this.ButtonpathsToDelete1);
            BDeleteTo.Add(this.ButtonpathsToDelete2);
            BDeleteTo.Add(this.ButtonpathsToDelete3);
            BDeleteTo.Add(this.ButtonpathsToDelete4);

            OperatorPaths.PathsTextReader();
            OperatorPaths.LenthCorrector();
            TextBoxTextSetter();
            TextBoxSorter();

            for (int i = 0; i < 10; i++)
            {
                Grid a = new Grid();
                Grid.SetRow(a, i + 1);
                a.Name = $"GridTheme_{i}";
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                columnDefinition1.Width = new GridLength(0, GridUnitType.Auto);
                ColumnDefinition columnDefinition2 = new ColumnDefinition();
                columnDefinition2.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition columnDefinition3 = new ColumnDefinition();
                columnDefinition3.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition columnDefinition4 = new ColumnDefinition();
                columnDefinition4.Width = new GridLength(0, GridUnitType.Auto);

                a.ColumnDefinitions.Add(columnDefinition1);
                a.ColumnDefinitions.Add(columnDefinition2);
                a.ColumnDefinitions.Add(columnDefinition3);
                a.ColumnDefinitions.Add(columnDefinition4);

                Button button1 = new Button();
                button1.Style = (Style)this.Resources["ButtonsPathsDelete"];
                button1.Content = new ImageAwesome
                {
                    Icon = FontAwesomeIcon.CheckCircle,
                    Foreground = Datas.SecondaryColorB
                };
                button1.Click += ButtonsThemesToSetActive;
                button1.Name = $"ButtonActivator_{i}";
                button1.Margin = new Thickness(0, 3, 3, 3);
                if (OperatorThemes.colorCodes[i].Active == true)
                    button1.IsEnabled = false;
                Grid.SetColumn(button1, 0);
                a.Children.Add(button1);

                Button button2 = new Button();
                button2.Style = (Style)this.Resources["ButtonThemesColorChange"];
                button2.Click += ButtonsThemesToChangeColor;
                button2.Name = $"ButtonChangePrimaryColor_{i}";
                if (OperatorThemes.colorCodes[i].PrimaryColor != "-")
                    button2.Background = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].PrimaryColor);
                else
                {
                    button2.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                    button2.Opacity = 0.5;
                }
                button2.BorderThickness = new Thickness(3);

                Border border1 = new Border();
                border1.Name = $"BorderChangePrimaryColor_{i}";
                border1.Child = button2;
                border1.BorderThickness = new Thickness(2);
                if (OperatorThemes.colorCodes[i].SecundaryColor != "-")
                    border1.BorderBrush = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].SecundaryColor);
                else
                    border1.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                border1.Style = (Style)this.Resources["BorderThemesColorChange"];
                border1.MouseLeftButtonDown += ButtonsThemesToChangeColor;
                Grid.SetColumn(border1, 1);
                a.Children.Add(border1);

                Button button3 = new Button();
                button3.Style = (Style)this.Resources["ButtonThemesColorChange"];
                button3.Click += ButtonsThemesToChangeColor;
                button3.Name = $"ButtonChangeSecondaryColor_{i}";
                if (OperatorThemes.colorCodes[i].SecundaryColor != "-")
                    button3.Background = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].SecundaryColor);
                else
                {
                    button3.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                    button3.Opacity = 0.5;
                }
                button3.BorderThickness = new Thickness(3);

                Border border2 = new Border();
                border2.Name = $"BorderChangeSecondaryColor_{i}";
                border2.Child = button3;
                border2.BorderThickness = new Thickness(2);
                if (OperatorThemes.colorCodes[i].PrimaryColor != "-")
                    border2.BorderBrush = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].PrimaryColor);
                else
                    border2.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                border2.Style = (Style)this.Resources["BorderThemesColorChange"];
                border2.MouseLeftButtonDown += ButtonsThemesToChangeColor;
                Grid.SetColumn(border2, 2);
                a.Children.Add(border2);

                Button button4 = new Button();
                button4.Style = (Style)this.Resources["ButtonsPathsDelete"];
                button4.Content = new ImageAwesome
                {
                    Icon = FontAwesomeIcon.TrashOutline,
                    Foreground = Datas.SecondaryColorB
                };
                button4.Click += ButtonsThemesToDeleteAt;
                button4.Name = $"ButtonDelete_{i}";
                Grid.SetColumn(button4, 3);
                a.Children.Add(button4);

                Label label1 = new Label();
                label1.Style = (Style)this.Resources["LabelThemes"];
                label1.Content = "Fő szín:";
                Grid.SetColumn(label1, 1);
                a.Children.Add(label1);

                Label label2 = new Label();
                label2.Style = (Style)this.Resources["LabelThemes"];
                label2.Content = "Mellék szín:";
                Grid.SetColumn(label2, 2);
                a.Children.Add(label2);

                if (OperatorThemes.colorCodes[i].PrimaryColor == "-" && OperatorThemes.colorCodes[i].SecundaryColor == "-")
                    a.Visibility = Visibility.Hidden;
                else
                {
                    if (OperatorThemes.colorCodes[i].Active == false)
                        a.Opacity = 0.7;
                    if ((OperatorThemes.colorCodes[i].PrimaryColor == "-" || OperatorThemes.colorCodes[i].SecundaryColor == "-") && !(OperatorThemes.colorCodes[i].PrimaryColor == "-" && OperatorThemes.colorCodes[i].SecundaryColor == "-"))
                        button1.IsEnabled = false;
                }
                this.GridThemes.Children.Add(a);

                GThemes.Add(a);
                BSetColorActive.Add(button1);
                BPrimaryColor.Add(button2);
                BoPrimeryColor.Add(border1);
                BSecundaryColor.Add(button3);
                BoSecundaryColor.Add(border2);
                BDeleteColor.Add(button4);
            }

            int ActiveEments = OperatorThemes.colorCodes.Count(x => x.PrimaryColor != "-" || x.SecundaryColor != "-");
            if (ActiveEments == 10)
                this.ButtonsThemesToAdd.Visibility = Visibility.Hidden;
            else
            {
                this.ButtonsThemesToAdd.Visibility = Visibility.Visible;
                Grid.SetRow(this.ButtonsThemesToAdd, ActiveEments + 1);
            }
            Checker();
            CheckerForDelete();
            ActiveMenus = ActiveMenu.Menu;
            OperatorEditor.mainWindow = this;
        }

        /// <summary>
        /// Beállítja az Xaml kódban szereplő szín típusú változók értékét
        /// </summary>
        public void SetXamlColorVariablesValue()
        {
            this.Resources["TextColor"] = Datas.SecondaryColorC;
            this.Resources["MainColor"] = Datas.PrimaryColorB;
            this.Resources["SupColor"] = Datas.SecondaryColorB;
            this.Background = Datas.PrimaryColorB;
        }

        /// <summary>
        /// Beállítja azon elemek színét melyeket a c# kódban hoztak létre és a megfelelő kiemeléseket elvégzi
        /// </summary>
        public void SetColorsAndAppear()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i != OperatorThemes.ActiveThemeIndex && OperatorThemes.colorCodes[i].PrimaryColor != "-" && OperatorThemes.colorCodes[i].SecundaryColor != "-")
                    BSetColorActive[i].IsEnabled = true;
                ((ImageAwesome)BSetColorActive[i].Content).Foreground = Datas.SecondaryColorB;
                ((ImageAwesome)BDeleteColor[i].Content).Foreground = Datas.SecondaryColorB;
                if (OperatorThemes.ActiveThemeIndex == i)
                    GThemes[i].Opacity = 1.0;
                else
                    GThemes[i].Opacity = 0.7;
            }
        }

        #region Menü
        //
        //
        //  Menü gobok eventjei:
        //
        //

        private void ButtonMenuExitMusic_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMenuNewMusic_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.Add;
            OperatorEditor.ReadSongs(OperatorEditor.EditorType.Add);
        }

        private void ButtonMenuEditMusic_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.Edit;
            OperatorEditor.ReadSongs(OperatorEditor.EditorType.Edit);
        }
        private void ButtonMenuInfoMusic_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.Info;
        }

        private void ButtonMenuSettingsMusic_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.Menu;
        }

        private void ButtonSideMenuSettingsThemes_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.SetThemes;
        }

        private void ButtonSideMenuSettingsPaths_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.SetPaths;
        }
        #endregion Menü
        #region PathsSettings
        //
        //
        //  Beállítások: Elérési utak
        //
        //
        #region Beállítások: Elérési utak - eventjei
        //  A "honnan" beállítások 1. sora
        private void TextBoxPathsFrom1_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterFrom(0);
        }
        private void ButtonpathsFromDelete1_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterFrom(0);
        }

        //  A "honnan" beállítások 2. sora
        private void TextBoxPathsFrom2_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterFrom(1);
        }

        private void ButtonpathsFromDelete2_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterFrom(1);
        }

        //  A "honnan" beállítások 3. sora
        private void TextBoxPathsFrom3_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterFrom(2);
        }

        private void ButtonpathsFromDelete3_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterFrom(2);
        }

        //  A "honnan" beállítások 4. sora
        private void TextBoxPathsFrom4_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterFrom(3);
        }

        private void ButtonpathsFromDelete4_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterFrom(3);
        }

        //  A "honnan" hozzáadó gombja
        private void ButtonsPathsFromAdd_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (TBPathFrom[i].Visibility == Visibility.Hidden)
                {
                    TBPathFrom[i].Visibility = Visibility.Visible;
                    TBPathFrom[i].Text = "";
                    BDeleteFrom[i].Visibility = Visibility.Visible;
                    TBPathFrom[i].BorderThickness = new Thickness(0, 0, 0, 0);
                    break;
                }
            }
            AddButtonSetter();
        }

        //  A "hova" beállítások 1. sora
        private void TextBoxPathsTo1_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterTo(0);
        }

        private void ButtonpathsToDelete1_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterTo(0);
        }


        //  A "hova" beállítások 2. sora
        private void TextBoxPathsTo2_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterTo(1);
        }

        private void ButtonpathsToDelete2_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterTo(1);
        }

        //  A "hova" beállítások 3. sora
        private void TextBoxPathsTo3_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterTo(2);
        }

        private void ButtonpathsToDelete3_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterTo(2);
        }

        //  A "hova" beállítások 4. sora
        private void TextBoxPathsTo4_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathSetterTo(3);
        }

        private void ButtonpathsToDelete4_Click(object sender, RoutedEventArgs e)
        {
            PathDeleterTo(3);
        }

        //  A "hova" hozzáadó gombja
        private void ButtonsPathsToAdd_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (TBPathTo[i].Visibility == Visibility.Hidden)
                {
                    TBPathTo[i].Visibility = Visibility.Visible;
                    TBPathTo[i].Text = "";
                    BDeleteTo[i].Visibility = Visibility.Visible;
                    TBPathTo[i].BorderThickness = new Thickness(0, 0, 0, 0);
                    break;
                }
            }
            AddButtonSetter();
        }
        #endregion Beállítások: Elérési utak - eventjei




        private void PathDeleterFrom(int i)
        {
            TBPathFrom[i].Text = "_";
            OperatorPaths.Sorter();
            TextBoxTextSetter();
            TextBoxSorter();
            CheckerForDelete();
        }
        private void PathDeleterTo(int i)
        {
            TBPathTo[i].Text = "_";
            OperatorPaths.Sorter();
            TextBoxTextSetter();
            TextBoxSorter();
            CheckerForDelete();
        }

        private void PathSetterFrom(int i)
        {
            OperatorPaths.PathsFrom[i] = TBPathFrom[i].Text;
        }

        private void PathSetterTo(int i)
        {
            OperatorPaths.PathsTo[i] = TBPathTo[i].Text;
        }
        /// <summary>
        /// Rendezi a listán belül a beviteli mezők értéket, hogy ne legyen üres mező két feltőltött mező között
        /// </summary>
        void TextBoxSorter()
        {
            for (int i = 0; i < 4; i++)
            {
                if (TBPathFrom[i].Text == "_")
                {
                    TBPathFrom[i].Visibility = Visibility.Hidden;
                    BDeleteFrom[i].Visibility = Visibility.Hidden;
                }
                else if (TBPathFrom[i].Visibility == Visibility.Hidden)
                {
                    TBPathFrom[i].Visibility = Visibility.Visible;
                    BDeleteFrom[i].Visibility = Visibility.Visible;
                }

                if (TBPathTo[i].Text == "_")
                {
                    TBPathTo[i].Visibility = Visibility.Hidden;
                    BDeleteTo[i].Visibility = Visibility.Hidden;
                }
                else if (TBPathTo[i].Visibility == Visibility.Hidden)
                {
                    TBPathTo[i].Visibility = Visibility.Visible;
                    BDeleteTo[i].Visibility = Visibility.Visible;
                }
            }
            AddButtonSetter();
        }

        /// <summary>
        /// Kiteszi a beviteli mezőkbe a fileból beolvasott szövegeket
        /// </summary>
        void TextBoxTextSetter()
        {
            for (int i = 0; i < OperatorPaths.PathsFrom.Count; i++)
                TBPathFrom[i].Text = OperatorPaths.PathsFrom[i];
            for (int i = 0; i < OperatorPaths.PathsTo.Count; i++)
                TBPathTo[i].Text = OperatorPaths.PathsTo[i];
        }
        /// <summary>
        /// A gombokat mozgatja, hogy a megfelelő sorban legyenek
        /// </summary>
        void AddButtonSetter()
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                if (TBPathFrom[i].Visibility == Visibility.Hidden)
                    break;
            }
            if (i != 4)
            {
                Grid.SetRow(this.ButtonsPathsFromAdd, i + 2);
                this.ButtonsPathsFromAdd.Visibility = Visibility.Visible;
            }
            else
                this.ButtonsPathsFromAdd.Visibility = Visibility.Hidden;
            for (i = 0; i < 4; i++)
            {
                if (TBPathTo[i].Visibility == Visibility.Hidden)
                    break;
            }
            if (i != 4)
            {
                Grid.SetRow(this.ButtonsPathsToAdd, i + 7);
                this.ButtonsPathsToAdd.Visibility = Visibility.Visible;
            }
            else
                this.ButtonsPathsToAdd.Visibility = Visibility.Hidden;
            CheckerForDelete();
        }

        /// <summary>
        /// Ez a függvény lecsekkolja, hogy mennyi elem törölhető
        /// </summary>
        private void CheckerForDelete()
        {
            if (TBPathFrom.Count(x => x.Visibility == Visibility.Visible) < 3)
                for (int i = 0; i < TBPathFrom.Count(x => x.Visibility == Visibility.Visible); i++)
                    BDeleteFrom[i].IsEnabled = false;
            else
                for (int i = 0; i < BDeleteFrom.Count; i++)
                    BDeleteFrom[i].IsEnabled = true;

            if (TBPathTo.Count(x => x.Visibility == Visibility.Visible) < 2)
                for (int i = 0; i < TBPathTo.Count(x => x.Visibility == Visibility.Visible); i++)
                    BDeleteTo[i].IsEnabled = false;
            else
                for (int i = 0; i < TBPathTo.Count; i++)
                    BDeleteTo[i].IsEnabled = true;
        }

        private void TextBoxPaths_LostFocus(object sender, RoutedEventArgs e)
        {
            OperatorPaths.Checker();
            for (int i = 0; i < TBPathFrom.Count; i++)
            {
                if (OperatorPaths.PathsFrom[i] == "-")
                {
                    TBPathFrom[i].Text = "Ez az elérési út nem elérhető!";
                    TBPathFrom[i].BorderThickness = new Thickness(2, 2, 2, 3);
                    ActiveMenus = ActiveMenu.SetPaths;
                }
                else
                    TBPathFrom[i].BorderThickness = new Thickness(0, 0, 0, 0);
                if (OperatorPaths.PathsTo[i] == "-")
                {
                    TBPathTo[i].Text = "Ez az elérési út nem elérhető!";
                    TBPathTo[i].BorderThickness = new Thickness(2, 2, 2, 3);
                    ActiveMenus = ActiveMenu.SetPaths;
                }
                else
                    TBPathTo[i].BorderThickness = new Thickness(0, 0, 0, 0);
            }
        }
        #endregion PathsSettings

        #region ThemesSettings
        //
        //
        //  Beállítások: Témák
        //
        //


        //  Téma hozzáadás gombja
        private void ButtonsthemesToAdd_Click(object sender, RoutedEventArgs e)
        {
            int ActiveThemes = GThemes.Count(x => x.Visibility == Visibility.Visible);
            GThemes[ActiveThemes].Visibility = Visibility.Visible;
            if (ActiveThemes < 9)
            {
                this.ButtonsThemesToAdd.Visibility = Visibility;
                Grid.SetRow(this.ButtonsThemesToAdd, ActiveThemes + 2);
            }
            else
                this.ButtonsThemesToAdd.Visibility = Visibility.Hidden;
        }

        //Az összes olyan gomb eventje mely egy adott téma csomagot töröl
        //todo: Mindenképp maradnia kell 1 JÓ témának, mi van ha olyat törlünk ami aktív volt,
        private void ButtonsThemesToDeleteAt(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int indexofdelete = Convert.ToInt32(button.Name.Split('_')[1]);
            //if (OperatorThemes.colorCodes[indexofdelete].Active == true)

            for (int i = indexofdelete; i < 10 - indexofdelete; i++)
            {
                BSetColorActive[i].IsEnabled = BSetColorActive[i + 1].IsEnabled;
                BPrimaryColor[i].Background = BPrimaryColor[i + 1].Background;
                BPrimaryColor[i].Opacity = BPrimaryColor[i + 1].Opacity;
                BoPrimeryColor[i].BorderBrush = BoPrimeryColor[i + 1].BorderBrush;
                BSecundaryColor[i].Background = BSecundaryColor[i + 1].Background;
                BSecundaryColor[i].Opacity = BSecundaryColor[i + 1].Opacity;
                BoSecundaryColor[i].BorderBrush = BoSecundaryColor[i + 1].BorderBrush;
                GThemes[i].Visibility = GThemes[i + 1].Visibility;
                OperatorThemes.colorCodes[i].Active = OperatorThemes.colorCodes[i + 1].Active;
                OperatorThemes.colorCodes[i].PrimaryColor = OperatorThemes.colorCodes[i + 1].PrimaryColor;
                OperatorThemes.colorCodes[i].SecundaryColor = OperatorThemes.colorCodes[i + 1].SecundaryColor;
            }
            BSetColorActive[9].IsEnabled = false;
            BPrimaryColor[9].Background = Brushes.White;
            BPrimaryColor[9].Opacity = 0.7;
            BoPrimeryColor[9].BorderBrush = Brushes.White;
            BSecundaryColor[9].Background = Brushes.White;
            BSecundaryColor[9].Opacity = 0.7;
            BoSecundaryColor[9].BorderBrush = Brushes.White;
            GThemes[9].Visibility = Visibility.Hidden;
            OperatorThemes.colorCodes[9].Active = false;
            OperatorThemes.colorCodes[9].PrimaryColor = "-";
            OperatorThemes.colorCodes[9].SecundaryColor = "-";
            Grid.SetRow(this.ButtonsThemesToAdd, GThemes.Count(x => x.Visibility == Visibility.Visible) + 1);
            if (OperatorThemes.colorCodes.Count(x => x.Active == true) == 0)
                OperatorThemes.ActiveThemeIndex = 0;
            Checker();
        }

        //Az összes olyan gomb eventje, mellyel egy új színt állítunk be
        private void ButtonsThemesToChangeColor(object sender, RoutedEventArgs e)
        {
            Colorsetter colorsetter;
            string ButtonName = "";
            try
            {
                Button button = (Button)sender;
                ButtonName = button.Name;
            }
            catch (InvalidCastException)
            {
                Border border = (Border)sender;
                ButtonName = border.Name;
            }
            if (ButtonName.Contains("Primary"))
                colorsetter = new Colorsetter(this, Colorsetter.EnumOrder.Main, Convert.ToInt32(ButtonName.Remove(0, ButtonName.IndexOf('_') + 1)));
            else
                colorsetter = new Colorsetter(this, Colorsetter.EnumOrder.Sup, Convert.ToInt32(ButtonName.Remove(0, ButtonName.IndexOf('_') + 1)));
            colorsetter.Show();
            this.IsEnabled = false;
            this.Opacity = 0.5;
        }

        //Az összes olyan gomb eventje, mely aktívizálja az adott témát
        private void ButtonsThemesToSetActive(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            OperatorThemes.ActiveThemeIndex = Convert.ToInt32(button.Name.Split('_')[1]);
        }

        /// <summary>
        /// Ez a függvény felel azért, hogy a szín változásokat felvigye a fő menüre
        /// </summary>
        public void ButtonsColorChange()
        {
            for (int i = 0; i < 10; i++)
            {
                if (OperatorThemes.colorCodes[i].PrimaryColor != "-")
                {
                    BPrimaryColor[i].Background = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].PrimaryColor);
                    BoSecundaryColor[i].BorderBrush = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].PrimaryColor);
                    BPrimaryColor[i].Opacity = 1.0;
                }
                if (OperatorThemes.colorCodes[i].SecundaryColor != "-")
                {
                    BoPrimeryColor[i].BorderBrush = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].SecundaryColor);
                    BSecundaryColor[i].Background = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.colorCodes[i].SecundaryColor);
                    BSecundaryColor[i].Opacity = 1.0;
                }
            }
        }

        /// <summary>
        /// Lecsekkol mindent és a gombok nyomhatóságát szabályozza
        /// </summary>
        public void Checker()
        {
            //Azt tárólja, hogy az adott sorban aktiválható e az adott téma (ehhez az kell, hogy mindkét szín meg legyen addva)
            List<bool> IsCorrect = new List<bool>();

            for (int i = 0; i < GThemes.Count(x => x.Visibility == Visibility.Visible); i++)
                IsCorrect.Add(OperatorThemes.colorCodes[i].PrimaryColor != "-" && OperatorThemes.colorCodes[i].SecundaryColor != "-");
            if (IsCorrect.Count(x => x == true) < 2)
                BDeleteColor[IsCorrect.IndexOf(true)].IsEnabled = false;
            else
                for (int i = 0; i < BDeleteColor.Count; i++)
                    BDeleteColor[i].IsEnabled = true;
        }
        #endregion ThemesSettings

        #region MusicEditor
        private void ButtonBackFromMusicEditor_Click(object sender, RoutedEventArgs e)
        {
            ActiveMenus = ActiveMenu.Menu;
        }
        public void SongDisplayer()
        { 
            this.StackPanelSongs.Children.Clear();
            for (int i = 0; i < OperatorEditor.DisplayedSongs.Count; i++)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() //Fájl név
                {
                    Width = new GridLength(5, GridUnitType.Star)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition() //Előadók
                {
                    Width = new GridLength(2, GridUnitType.Star)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition() //Cím
                {
                    Width = new GridLength(3, GridUnitType.Star)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition() //Műfajok
                {
                    Width = new GridLength(2, GridUnitType.Star)
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition() //Album
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                grid.Name = "_" + i;
                grid.MouseLeftButtonDown += LeftMouseButtonDownOnMusicGrid_Click;
                grid.Cursor = Cursors.Hand;
                grid.MouseEnter += MusicGrid_MouseEnter;
                grid.MouseLeave += MusicGrid_MouseLeave;

                TextBlock textBlock1 = new TextBlock();
                Grid.SetColumn(textBlock1, 0);
                textBlock1.Text = OperatorEditor.Songs[OperatorEditor.DisplayedSongs[i]].Filename;
                textBlock1.Style = (Style)this.Resources["TextBlockSongEdit"];
                grid.Children.Add(textBlock1);

                TextBlock textBlock2 = new TextBlock();
                Grid.SetColumn(textBlock2, 1);
                textBlock2.Text = ListToString(OperatorEditor.Songs[OperatorEditor.DisplayedSongs[i]].Performers);
                textBlock2.Style = (Style)this.Resources["TextBlockSongEdit"];
                grid.Children.Add(textBlock2);

                TextBlock textBlock3 = new TextBlock();
                Grid.SetColumn(textBlock3, 2);
                textBlock3.Text = OperatorEditor.Songs[OperatorEditor.DisplayedSongs[i]].Title;
                textBlock3.Style = (Style)this.Resources["TextBlockSongEdit"];
                grid.Children.Add(textBlock3);

                TextBlock textBlock4 = new TextBlock();
                Grid.SetColumn(textBlock4, 3);
                textBlock4.Text = ListToString(OperatorEditor.Songs[OperatorEditor.DisplayedSongs[i]].Genres);
                textBlock4.Style = (Style)this.Resources["TextBlockSongEdit"];
                grid.Children.Add(textBlock4);

                TextBlock textBlock5 = new TextBlock();
                Grid.SetColumn(textBlock5, 4);
                textBlock5.Text = OperatorEditor.Songs[OperatorEditor.DisplayedSongs[i]].Album;
                textBlock5.Style = (Style)this.Resources["TextBlockSongEdit"];
                grid.Children.Add(textBlock5);

                this.StackPanelSongs.Children.Add(grid);
            }
        }

        

        public static string ListToString(List<string> lista)
        {
            if (lista.Count > 0)
            {
                string back = lista[0];
                for (int i = 1; i < lista.Count; i++)
                {
                    back += ", " + lista[i];
                }
                return back;
            }
            else
                return "";
        }

        private void LeftMouseButtonDownOnMusicGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.7;
            this.IsEnabled = false;
            MusicSetter musicSetter = new MusicSetter(this, Convert.ToInt32(((Grid)sender).Name.Remove(0, 1)));
            musicSetter.Show();
        }
        private void MusicGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Opacity = 0.5;
        }
        private void MusicGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Opacity = 1;
        }
    }
    #endregion MusicEditor

}

