using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Zeneszerk_2._0
{
    /// <summary>
    /// Interaction logic for Colorsetter.xaml
    /// </summary>
    public partial class Colorsetter : Window
    {
        public Colorsetter()
        {
            InitializeComponent();
            this.Background = Datas.PrimaryColorB;
            this.Resources["TextColor"] = Datas.SecondaryColorC;
            this.Resources["MainColor"] = Datas.PrimaryColorB;
            this.Resources["SupColor"] = Datas.SecondaryColorB;
            this.FontFamily = new FontFamily("Century Gothic");
            this.TextBlockError.Foreground = Datas.SecondaryColorB;
        }

        public Colorsetter(MainWindow mainWindow, EnumOrder colorType, int columnNbr) : this()
        {
            MainWindow = mainWindow;
            ColorWillWrited = new DataTypeOrder { Colortype = colorType, Column = columnNbr };

            // Megnézzük, hogy a beállítani kívánt szín párja létezik e, ha igen akkor engedélyezve van a BALANCE opció ellenkező esetben a gomb inaktív
            if (ColorWillWrited.Colortype == EnumOrder.Main && OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor == "-")
                BorderBalancer.IsEnabled = false;
            else if (ColorWillWrited.Colortype == EnumOrder.Sup && OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor == "-")
                BorderBalancer.IsEnabled = false;
            else
                BorderBalancer.IsEnabled = true;

            if ((colorType == EnumOrder.Main && OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor != "-") || (colorType == EnumOrder.Sup && OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor != "-"))
            {
                if (colorType == EnumOrder.Main)
                {
                    HEX = OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor;
                    TextBoxHEX.Text = HEX;
                }
                else
                {
                    HEX = OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor;
                    TextBoxHEX.Text = HEX;
                }
            }
        }

        public MainWindow MainWindow { get; }

        private string red = "0";
        public string RED
        {
            get { return red; }
            private set
            {
                red = value;
                RGBToHEXConvert();
            }
        }

        private string green = "0";
        public string GREEN
        {
            get { return green; }
            private set
            {
                green = value;
                RGBToHEXConvert();
            }
        }

        private string blue = "0";
        public string BLUE
        {
            get { return blue; }
            private set
            {
                blue = value;
                RGBToHEXConvert();
            }
        }

        private string hex = "0";
        public string HEX
        {
            get { return hex; }
            private set
            {
                hex = value;
                HEXToRGB();
            }
        }

        public enum EnumOrder
        {
            Main,
            Sup,
            None
        }
        public struct DataTypeOrder
        {
            /// <summary>
            /// Hányadik téma
            /// </summary>
            public int Column;
            /// <summary>
            /// Melyik 
            /// </summary>
            public EnumOrder Colortype;
        }

        /// <summary>
        /// Az a szín helye amelyet írni fogunk
        /// </summary>
        public DataTypeOrder ColorWillWrited;

        private void TextBoxRGBRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckerRGB((TextBox)sender);
        }

        private void TextBoxRGBGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckerRGB((TextBox)sender);
        }

        private void TextBoxRGBblue_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckerRGB((TextBox)sender);
        }

        private void TextBoxHEX_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckerHeX((TextBox)sender);
        }

        private void ButtonSetColorBack_Click(object sender, RoutedEventArgs e)
        {
            ExitTasks();
        }

        private void ButtonSetColorSave_Click(object sender, RoutedEventArgs e)
        {
            if (!hex.Contains("#"))
                hex = "#" + hex;
           
            if (ColorWillWrited.Colortype == EnumOrder.Main)
            {
                if (OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor == "-")
                {
                    OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor = hex;
                    OperatorThemes.FileWriter();
                    ExitTasks();
                }
                else
                {
                    Color color = (Color)ColorConverter.ConvertFromString(OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor);
                    if (Math.Abs(color.R - Convert.ToInt32(red)) > 10 || Math.Abs(color.G - Convert.ToInt32(green)) > 10 || Math.Abs(color.B - Convert.ToInt32(blue)) > 10)
                    {
                        OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor = hex;
                        OperatorThemes.FileWriter();
                        ExitTasks();
                    }
                    else
                    {
                        this.TextBlockError.Visibility = Visibility.Visible;
                    }
                }               
            }
            else if (ColorWillWrited.Colortype == EnumOrder.Sup)
            {
                if (OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor == "-")
                {
                    OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor = hex;
                    OperatorThemes.FileWriter();
                    ExitTasks();
                }
                else
                {
                    Color color = (Color)ColorConverter.ConvertFromString(OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor);
                    if (Math.Abs(color.R - Convert.ToInt32(red)) > 10 || Math.Abs(color.G - Convert.ToInt32(green)) > 10 || Math.Abs(color.B - Convert.ToInt32(blue)) > 10)
                    {
                        OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor = hex;
                        OperatorThemes.FileWriter();
                        ExitTasks();
                    }
                    else
                    {
                        this.TextBlockError.Visibility = Visibility.Visible;
                    }
                }              
            }
        }
        private void ExitTasks()
        {
            MainWindow.IsEnabled = true;
            this.Visibility = Visibility.Hidden;
            MainWindow.Opacity = 1;
            MainWindow.ButtonsColorChange();
            MainWindow.Checker();
            this.Close();
        }
        /// <summary>
        /// Ez a függvény felel azért, hogy az rgbs beviteli mezőkbe csak a megfelelő karakterek kerűljenek
        /// </summary>
        /// <param name="textBox">Ez a változó tárólja az adott beviteli mező adatait</param>
        private void CheckerRGB(TextBox textBox)
        {
            string text = textBox.Text;
            bool IsTextCorrect = false;
            
            // Végig nézzük, hogy minden karakter jó e
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    Convert.ToInt32(text[i].ToString());
                    IsTextCorrect = true;
                }
                catch (FormatException)
                {
                    switch (textBox.Name)
                    {
                        case "TextBoxRGBRed":
                            textBox.Text = RED;
                            break;
                        case "TextBoxRGBBlue":
                            textBox.Text = BLUE;
                            break;
                        case "TextBoxRGBGreen":
                            textBox.Text = GREEN;
                            break;
                    }
                    break;
                }
            }

            // Majd az egész szöveget leellenőrizzük
            if (IsTextCorrect)
            {
                if (Convert.ToInt32(text) > 255)
                {
                    textBox.Text = "255";
                    switch (textBox.Name)
                    {
                        case "TextBoxRGBRed":
                            RED = "255";
                            break;
                        case "TextBoxRGBBlue":
                            BLUE = "255";
                            break;
                        case "TextBoxRGBGreen":
                            GREEN = "255";
                            break;
                    }
                }
                else
                {
                    switch (textBox.Name)
                    {
                        case "TextBoxRGBRed":
                            RED = textBox.Text;
                            break;
                        case "TextBoxRGBBlue":
                            BLUE = textBox.Text;
                            break;
                        case "TextBoxRGBGreen":
                            GREEN = textBox.Text;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Ez a függvény felel azért, hogy az HEXs beviteli mezőkbe csak a megfelelő karakterek kerűljenek
        /// </summary>
        /// <param name="textBox">Ez a változó tárólja az adott beviteli mező adatait</param>
        private void CheckerHeX(TextBox textBox)
        {
            string text = textBox.Text;
            bool IsTextCorrect = false;

            // Karakterenként megvizsgáljuk a szöveget, hogy stimmel e
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    Convert.ToInt32(text[i].ToString());
                    IsTextCorrect = true;
                }
                catch (FormatException)
                {
                    switch (text[i].ToString().ToUpper())
                    {
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                            IsTextCorrect = true;
                            break;
                        default:
                            if (!(i == 0 && text[i].ToString() == "#"))
                            {
                                textBox.Text = HEX;
                                IsTextCorrect = false;
                            }
                            else
                                IsTextCorrect = true;
                            break;
                    }
                }
                if (!IsTextCorrect)
                    break;
            }

            // Majd az egész szöveget megvizsgáljuk, hogy rendben van e
            if (IsTextCorrect)
            {
                if (text.Contains("#") && text.Length < 8)
                {
                    HEX = text;
                }
                else if(text.Length < 7)
                {
                    HEX = "#" + text;
                }
                else
                {
                    textBox.Text = HEX;
                }
            }
        }
        private void RGBToHEXConvert()
        {
            Color myColor = Color.FromRgb(Convert.ToByte(RED), Convert.ToByte(GREEN), Convert.ToByte(BLUE));
            hex = "#" + myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            this.TextBoxHEX.Text = HEX;
            SetDisplayColor();
        }

        private void HEXToRGB()
        {
            if (this.TextBoxHEX.Text != "#")
            {
                string h = hex;
                for (int i = 0; i < 7 - hex.Length; i++)
                {
                    h += "0";
                }
                Color myColor2 = (Color)ColorConverter.ConvertFromString(h.ToUpper());
                red = myColor2.R.ToString();
                blue = myColor2.B.ToString();
                green = myColor2.G.ToString();
                this.TextBoxRGBBlue.Text = BLUE;
                this.TextBoxRGBGreen.Text = GREEN;
                this.TextBoxRGBRed.Text = RED;
                SetDisplayColor();
            }
        }
        private void SetDisplayColor()
        {
            this.RectangleColorShower.Fill = (Brush)new BrushConverter().ConvertFromString(HEX);
        }

        /// <summary>
        /// A textbox kinézetét változtatja, ha az egér fölötte van
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlockError_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            textBlock.Foreground = Datas.PrimaryColorB;
            textBlock.Background = Datas.SecondaryColorB;
        }

        /// <summary>
        /// A textbox kinézetét változtatja, ha az egér nincs fölötte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlockError_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            textBlock.Foreground = Datas.SecondaryColorB;
            textBlock.Background = Datas.PrimaryColorB;
        }

        /// <summary>
        /// A BALANCE gomb lenyomására a beállítani kívánt színt a párjához képest úgy állítja, hogy 255-ből kivonja a megfelelő szín értékeket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBalancer_Click(object sender, RoutedEventArgs e)
        {
            if (ColorWillWrited.Colortype == EnumOrder.Main)
            {
                Color myColor = (Color)ColorConverter.ConvertFromString(OperatorThemes.colorCodes[ColorWillWrited.Column].SecundaryColor);
                TextBoxRGBRed.Text = (255 - myColor.R).ToString();
                TextBoxRGBGreen.Text = (255 - myColor.G).ToString();
                TextBoxRGBBlue.Text = (255 - myColor.B).ToString();
            }
            else
            {
                Color myColor = (Color)ColorConverter.ConvertFromString(OperatorThemes.colorCodes[ColorWillWrited.Column].PrimaryColor);
                TextBoxRGBRed.Text = (255 - myColor.R).ToString();
                TextBoxRGBGreen.Text = (255 - myColor.G).ToString();
                TextBoxRGBBlue.Text = (255 - myColor.B).ToString();
            }
        }
    }
}
