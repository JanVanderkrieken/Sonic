using Svg;
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

        private static BitmapImage MapImage1 = new BitmapImage();
        private static BitmapImage MapImage2 = new BitmapImage();
        private static BitmapImage MapImage3 = new BitmapImage();
        private static BitmapImage MapImage = new BitmapImage();


        public static System.Windows.Controls.Image MapImageControl1 = new System.Windows.Controls.Image();
        //public static System.Windows.Controls.Image MapImageControl2 = new System.Windows.Controls.Image();
        //public static System.Windows.Controls.Image MapImageControl3 = new System.Windows.Controls.Image();
        public static OpaqueClickableImage MapImageControlSVG = new OpaqueClickableImage();

        public static void Create()
        {           
            //MapImage = RemoveBackGroundColor("pack://application:,,,/Resources/Map.bmp",0,128,224);
            MapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/map.bmp",UriKind.Absolute));
            //MapImage2 = new BitmapImage(new Uri("pack://application:,,,/Resources/Mapdeel2.png", UriKind.Absolute));
            //MapImage3 = new BitmapImage(new Uri("pack://application:,,,/Resources/Mapdeel3.png", UriKind.Absolute));
            
            MapImageControl1.Source = MapImage;
            MapImageControl1.Stretch = Stretch.None;
            MapImageControl1.HorizontalAlignment = HorizontalAlignment.Left;
            MapImageControl1.VerticalAlignment = VerticalAlignment.Top;
            MapImageControl1.Margin = new Thickness(0,-1190,0,0);
            //MapImageControl2.Source = MapImage2;
            //MapImageControl2.Stretch = Stretch.None;
            //MapImageControl2.HorizontalAlignment = HorizontalAlignment.Left;
            //MapImageControl2.VerticalAlignment = VerticalAlignment.Top;
            //MapImageControl2.Margin = new Thickness(3298, -870, 0, 0);
            ////MapImageControl3.Source = MapImage3;
            //MapImageControl3.Stretch = Stretch.None;
            //MapImageControl3.HorizontalAlignment = HorizontalAlignment.Left;
            //MapImageControl3.VerticalAlignment = VerticalAlignment.Top;
            //MapImageControl3.Margin = new Thickness(7374, -870, 0, 0);
            int maxwidth = 16025;
            SVGParser.MaximumSize = new System.Drawing.Size((int)MapImage.Width,(int)MapImage.Height);
            DebugText(new Dictionary<string, string> { { "max width", MapImage.Height.ToString() } });
            Bitmap bmp = SVGParser.GetBitmapFromSVG("E:/Jan Home/TestWPF/TestWPF/Resources/map.svg");
            //BitmapImage temp = RemoveBackGroundColor("pack://application:,,,/Resources/map.tif",255,255,255);
            //MapImageControlSVG.Source = bmp;
            MapImageControlSVG.Source = Bitmapp.ToBitmapImage(bmp);
            MapImageControlSVG.Stretch = Stretch.None;
            MapImageControlSVG.HorizontalAlignment = HorizontalAlignment.Left;
            MapImageControlSVG.VerticalAlignment = VerticalAlignment.Top;
            MapImageControlSVG.Margin = new Thickness(0, -1190, 0, 0);
            //MapImageControlSVG.Visibility = Visibility.Hidden;
            

        }



        /// <summary>
        /// Moves the map if Sonic is in the middle of the screen
        /// </summary>
        /// <param name="PosXDifferance">The ammount that the map must move, nothing to do with deltatime.</param>
        /// <param name="PosYDifferance">The ammount that the map must move, nothing to do with deltatime.</param>
        public static void MoveMap(double PosXDifferance, double PosYDifferance)
        {
            var thick = MapImageControl1.Margin;
            thick.Left += PosXDifferance;
            thick.Top += PosYDifferance;
            MapImageControl1.Margin = thick;
            //thick.Top += 500;
            MapImageControlSVG.Margin = thick;
            //thick.Left += 3298;
            //MapImageControl2.Margin = thick;
            //thick.Left += 4076;
            //MapImageControl3.Margin = thick;
            //MainWindow.DebugText(new Dictionary<string, string> { { "map left", MapImageControl1.Margin.Left.ToString() }, { "map top", MapImageControl1.Margin.Top.ToString() } });
        }

        public static bool HitMap(System.Windows.Point point)
        {
            var result = VisualTreeHelper.HitTest(Map.MapImageControlSVG, point);


            if (/*hitResultsList.Count >0*/ result != null)
            {
                // Perform action on hit visual object.
                //DebugText(new Dictionary<string, string> { { "is hit", "true" } });
                return true;
            }
                //DebugText(new Dictionary<string, string> { { "is hit", "false" } });

            return false;
        }
    }

    public class OpaqueClickableImage : System.Windows.Controls.Image
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            var source = (BitmapSource)Source;

            // Get the pixel of the source that was hit
            var x = (int)(hitTestParameters.HitPoint.X / ActualWidth * source.PixelWidth-Margin.Left);
            var y = (int)(hitTestParameters.HitPoint.Y / ActualHeight * source.PixelHeight-Margin.Top);

            // Copy the single pixel into a new byte array representing RGBA
            var pixel = new byte[4];
            source.CopyPixels(new Int32Rect(x, y, 1, 1), pixel, 4, 0);

            // Check the alpha (transparency) of the pixel
            // - threshold can be adjusted from 0 to 255
            if (pixel[3] > 10)
            {
                return new PointHitTestResult(this, hitTestParameters.HitPoint);

            }
                return null;

        }
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            var source = (BitmapSource)Source;

            var rect = new Int32Rect((int)hitTestParameters.HitGeometry.Bounds.Left,(int)hitTestParameters.HitGeometry.Bounds.Top,(int)hitTestParameters.HitGeometry.Bounds.Width,(int)hitTestParameters.HitGeometry.Bounds.Height);

            int stride = (int)source.PixelWidth * (source.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)rect.Height * stride];
            source.CopyPixels(rect, pixels,stride, 0);

            if (!pixels.Contains((byte)0))
            {

            }

            IntersectionDetail intersectionDetail = IntersectionDetail.NotCalculated;

            // Perform custom actions during the hit test processing.

            return new GeometryHitTestResult(this, intersectionDetail);
        }
    }

    public class SVGParser
    {
        /// <summary>
        /// The maximum image size supported.
        /// </summary>
        public static System.Drawing.Size MaximumSize { get; set; }

        /// <summary>
        /// Converts an SVG file to a Bitmap image.
        /// </summary>
        /// <param name="filePath">The full path of the SVG image.</param>
        /// <returns>Returns the converted Bitmap image.</returns>
        public static Bitmap GetBitmapFromSVG(string filePath)
        {
            SvgDocument document = GetSvgDocument(filePath);

            Bitmap bmp = document.Draw();
            return bmp;
        }

        /// <summary>
        /// Gets a SvgDocument for manipulation using the path provided.
        /// </summary>
        /// <param name="filePath">The path of the Bitmap image.</param>
        /// <returns>Returns the SVG Document.</returns>
        public static SvgDocument GetSvgDocument(string filePath)
        {
            SvgDocument document = SvgDocument.Open(filePath);
            return AdjustSize(document);
        }

        /// <summary>
        /// Makes sure that the image does not exceed the maximum size, while preserving aspect ratio.
        /// </summary>
        /// <param name="document">The SVG document to resize.</param>
        /// <returns>Returns a resized or the original document depending on the document.</returns>
        private static SvgDocument AdjustSize(SvgDocument document)
        {
            if (document.Height > MaximumSize.Height)
            {
                document.Width = (int)((document.Width / (double)document.Height) * MaximumSize.Height);
                document.Height = MaximumSize.Height;
            }
            return document;
        }

    }
}