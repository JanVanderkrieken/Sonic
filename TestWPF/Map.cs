using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static TestWPF.MainWindow;

namespace TestWPF
{
    public static class Map
    {

        private static BitmapImage MapImage = new BitmapImage();
        
        public static System.Windows.Controls.Image MapImageControl = new System.Windows.Controls.Image();

        public static void Create()
        {
            MapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Mapdeel1.png",UriKind.Absolute));
            //MapImage = RemoveBackGroundColor("pack://application:,,,/Resources/Map.bmp",0,128,224);
            MapImageControl.Source = MapImage;
            MapImageControl.Stretch = Stretch.None;
            MapImageControl.HorizontalAlignment = HorizontalAlignment.Left;
            MapImageControl.VerticalAlignment = VerticalAlignment.Top;
            MapImageControl.Margin = new Thickness(0,-870,0,0);
        }

        /// <summary>
        /// Moves the map if Sonic is in the middle of the screen
        /// </summary>
        /// <param name="PosXDifferance">The ammount that the map must move, nothing to do with deltatime.</param>
        /// <param name="PosYDifferance">The ammount that the map must move, nothing to do with deltatime.</param>
        public static void MoveMap(double PosXDifferance, double PosYDifferance)
        {
            var thick = MapImageControl.Margin;
            thick.Left += PosXDifferance;
            thick.Top += PosYDifferance;
            MapImageControl.Margin = thick;
        }

        //public static bool HitMap(
    }
}