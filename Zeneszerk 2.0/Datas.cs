using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zeneszerk_2._0
{
    class Datas
    {
        private static Brush primaryColorB;
        public static Brush PrimaryColorB
        {
            get 
            {
                primaryColorB = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.GetActiveTheme()[0]);
                return primaryColorB;
            }
            set { primaryColorB = value; }
        }

        private static Brush secondaryColorB;
        public static Brush SecondaryColorB
        {
            get 
            {
                secondaryColorB = (Brush)new BrushConverter().ConvertFromString(OperatorThemes.GetActiveTheme()[1]);
                return secondaryColorB;
            }
            set { secondaryColorB = value; }
        }

        private static Color primaryColorC;
        public static Color PrimaryColorC
        {
            get
            {
                primaryColorC = (Color)ColorConverter.ConvertFromString(OperatorThemes.GetActiveTheme()[0]);
                return primaryColorC;
            }
            set { primaryColorC = value; }
        }

        private static Color secondaryColorC;
        public static Color SecondaryColorC
        {
            get
            {
                secondaryColorC = (Color)ColorConverter.ConvertFromString(OperatorThemes.GetActiveTheme()[1]);
                return secondaryColorC;
            }
            set { secondaryColorC = value; }
        }

    }
}
