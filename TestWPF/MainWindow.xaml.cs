using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TestWPF
{
    // bewegen met acceleratie(m/s²) ipv x +5
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //double teller = 0;
        // 1 Rect hebben en die gebruiken
        // 240 hoog 280 breed
        //const double SONICWIDHT = 46.2;
        //const double SONICHEIGHT = 48;
        //Rect rectClip = new Rect(0.5, 0, SONICWIDHT, SONICHEIGHT);
        //Rect rectClipBug = new Rect(0, 0, 0, 0);
        //double tellerFrames = 0;
        //Thickness dikte, dikteBug = new Thickness(0, 0, 0, 0);// margin voor animatie
        //CURRENT_STATE currentState = CURRENT_STATE.WAITING;
        Dictionary<string, bool> KeysDown = new Dictionary<string, bool>(5);
        //public bool IsQdown, IsSdown, IsDdown, IsZdown, IsSpacedown = false;
        //Geometry resetClip;
        //Transform resetRenderTransform;
        //System.Windows.Controls.Image sonicImage = new System.Windows.Controls.Image();
        DispatcherTimer disTimer = new DispatcherTimer();
        //int TimerTeller = 0;
        Sonic sonic = new Sonic();

        public static double WidthWindow;

        //double widthBug;
        //double TellerTick, TellerKeyDown = 0;
        //double TellerMarginBug = 600;
        //int tellerMarginBugFactor = 10;
        //bool flipBug = false;

        bool isPauzed = false;
        //maybe a menu where the keybindings can change
        List<IEnemy> listEnemies = new List<IEnemy>();



        /*
         * STATE_WAITING: teller = 0 && teller2 = 0
         * STATE_WALKING: teller van 0 tot en met 4 && teller2 = 1
         * STATE_RUNNING: teller van 0 tot en met 3 && teller2 = 2
         * STATE_PUSHING: teller van 0 tot en met 3 && teller2 = 3
         * STATE_BRAKING: teller 0 en 1 && teller2 = 4
         * STATE_AFK: teller = 1 en 2 && teller2 = 0
         */
        // public enum CURRENT_STATE
        // {
        //     WAITING = 0,
        //     WALKING = 1,
        //     RUNNING = 2,
        //     PUSHING = 3,
        //     BREAKING = 4,
        //     AFK = -1,
        //     ROLLING = 5,
        // }

        public MainWindow()
        {
            InitializeComponent();
            disTimer.Tick += new EventHandler(DisTimer_Tick);
            disTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);//ca 25 FPS
            disTimer.Start();

            WidthWindow = Width;

            StartGrid.Children.Add(Map.MapImageControl1);
            //StartGrid.Children.Add(Map.MapImageControl2);
            //StartGrid.Children.Add(Map.MapImageControl3);
            StartGrid.Children.Add(Map.MapImageControlSVG);

            //BitmapImage bug = new BitmapImage(new Uri("pack://application:,,,/Resources/LadyBug.bmp", UriKind.Absolute));
            //sonicImage.Source = sonic.SonicImage;
            //sonicImage.Stretch = Stretch.None;
            //sonicImage.HorizontalAlignment = HorizontalAlignment.Left;
            //sonicImage.VerticalAlignment = VerticalAlignment.Top;

            StartGrid.Children.Add(sonic.SonicImage);

            //Animatie.Source = bug;
            //widthBug = bug.Width / 2;
            //dikteBug.Left = this.Width - widthBug - 140;
            //rectClipBug.Width = widthBug;
            //rectClipBug.Height = bug.Height;
            //resetClip = Animatie.Clip;
            //resetRenderTransform = Animatie.RenderTransform;
            //ClipImage(sonicImage, rectClip, false);
            //ClipImage(Animatie, rectClipBug);
            /*
                class Keyboard
                    IsKeyDown(Key) && IsKeyUp(Key)
                    ||
                    IsKeyToggled (Key)
             */


            KeysDown.Add("Q", false);

            KeysDown.Add("Z", false);
            KeysDown.Add("S", false);
            KeysDown.Add("D", false);
            KeysDown.Add("Space", false);
            CreateEnemies();
            Map.Create();
        }

        private void CreateEnemies()
        {
            listEnemies.Add(new LadyBug(0, 200, 600, 200, 10, 0));
            foreach (IEnemy enemy in listEnemies)
            {
                StartGrid.Children.Add(enemy.MyImageControl);
                enemy.MyImageControl.Stretch = Stretch.None;
                enemy.MyImageControl.VerticalAlignment = VerticalAlignment.Top;
                enemy.MyImageControl.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }


        private void DisTimer_Tick(object sender, EventArgs e)
        {
            sonic.Tick(1, KeysDown);
            // TellerTick++;
            // TimerTeller++;
            // if (IsDdown)
            // {
            //     Animate_Right();
            // }
            // else if (IsQdown)
            // {
            //     Animate_Left();
            // }
            // if (IsZdown)
            // {
            //     Animate_Up();
            // }
            // if (IsSdown)
            // {
            //     Animate_Down();
            // }

            //lblTeller.Content = "sonic height / 5 = " +sonic.SonicImage.Height/5;
            //lblTeller.Content = "Ticks = " + TellerTick.ToString() + " Key_downs = " + TellerKeyDown.ToString() + " diff = " + (TellerTick - TellerKeyDown);
            //if (TimerTeller >= 30 && (currentState == CURRENT_STATE.WAITING || currentState == CURRENT_STATE.AFK))
            //{
            //    currentState = CURRENT_STATE.AFK;
            //   Animate_Right();
            //   rectClip.Y = (int)currentState * SONICHEIGHT;
            //   rectClip.X = teller * SONICWIDHT;
            //dikte.Left = teller * SONICWIDHT;
            //  dikte.Top = (int)currentState * (SONICHEIGHT * -1);
            //  if (currentState == CURRENT_STATE.AFK)
            // {
            //      rectClip.Y = 0;
            //     dikte.Top = 0;
            //  }
            // ClipImage(sonicImage, rectClip, false);
            //}
            //if (TellerTick % 2 == 0)
            //{
            //    rectClipBug.X = widthBug;
            //}
            //else
            //{
            //   rectClipBug.X = 0;
            //}
            //dikteBug.Left++;
            //if (TellerMarginBug >= 600)
            //{
            //   tellerMarginBugFactor = -10;
            //   flipBug = false;
            //}
            // else if (TellerMarginBug == 0)
            // {
            //     tellerMarginBugFactor = 10;
            //     flipBug = true;
            // }
            // TellerMarginBug += tellerMarginBugFactor;
            // dikteBug.Left = TellerMarginBug;
            // Animatie.Margin = dikteBug;
            // ClipImage(Animatie, rectClipBug, flipBug);
            //if (flipBug)
            //{
            //    ScaleTransform flipTrans = new ScaleTransform(-1, 0,0.5,0.5);
            //    Animatie.RenderTransform = flipTrans;

            //}

            foreach (IEnemy enemy in listEnemies)
            {
                enemy.Tick(1);
            }
        }

        public void Menu()
        {
            /*
             * Change the keybindings
             * For the Dictionary use Remove(key) and Add(key,bool)
             */
            //  var MenuStackPanel = new System.Windows.Controls.Stackpanel();
            // StartGrid.Children.Add(MenuStackPanel);
        }

        public static void ClipImage(System.Windows.Controls.Image image, Rect visibleRect, bool isFlipped)
        {
            Thickness thickMargin = new Thickness(image.Margin.Left, image.Margin.Top, image.Margin.Right, image.Margin.Bottom);
            image.RenderTransform = new TranslateTransform(-visibleRect.X,visibleRect.Height - visibleRect.Y);
            if (isFlipped)
            {
                //    ScaleTransform flipTrans = new ScaleTransform(-1, 0, dikteBug.Left + Animatie.ActualWidth / 2, dikteBug.Top);
                //    Animatie.RenderTransform = flipTrans;
                image.FlowDirection = FlowDirection.RightToLeft;
                image.RenderTransform = new TranslateTransform(visibleRect.X-(image.ActualWidth-visibleRect.Width),visibleRect.Height - visibleRect.Y);
                //thickMargin.Left -= image.ActualWidth / 2;
                //thickMargin.Right = thickMargin.Left;
                //thickMargin.Left = 0;
                //thickMargin.Left = -image.ActualWidth - visibleRect.Width;
            }
            else if (image.FlowDirection == FlowDirection.RightToLeft)
            {
                image.FlowDirection = FlowDirection.LeftToRight;
                image.RenderTransform = new TranslateTransform(-visibleRect.X,  visibleRect.Height - visibleRect.Y);
            }
            //image.Margin.Left = visibleRect.X;
            image.Clip = new RectangleGeometry
            {
                Rect = new Rect(
                visibleRect.X,
                visibleRect.Y,
                visibleRect.Width,
                visibleRect.Height)
            };
            image.Margin = thickMargin;
            //Animatie.Margin = dikteBug;
            //lblTeller.Content = Animatie.Margin.Left.ToString();
            //lblTeller.Content = "left = " + sonicImage.Margin.Left + " top = " + sonicImage.Margin.Top;

        }

        // private void Animate_Down()
        // {
        //     IsSdown = false;
        //     if ((int)currentState * SONICHEIGHT < sonicImage.Source.Height - 50)
        //     {
        //         switch (currentState)
        //         {
        //             case CURRENT_STATE.WAITING:
        //                 if (teller >= 5)
        //                 {
        //                     teller = 4;
        //                 }
        //                 break;
        //             case CURRENT_STATE.WALKING:
        //                 if (teller >= 4)
        //                 {
        //                     teller = 3;
        //                 }
        //                 break;
        //             case CURRENT_STATE.RUNNING:
        //                 if (teller >= 4)
        //                 {
        //                     teller = 3;
        //                 }
        //                 break;
        //             case CURRENT_STATE.PUSHING:
        //                 if (teller >= 2)
        //                 {
        //                     teller = 1;
        //                 }
        //                 break;
        //             case CURRENT_STATE.BREAKING:
        //                 break;
        //             default:

        //                 break;
        //         }
        //         currentState++;
        //         //rectClip.Y = (int)currentState * SONICHEIGHT;
        //         //rectClip.X = teller * SONICWIDHT;
        //         //dikte.Left = teller * SONICWIDHT;
        //         //dikte.Top = (int)currentState * SONICHEIGHT;


        //     }
        // }

        // private void Animate_Up()
        // {
        //     IsZdown = false;

        //     if ((int)currentState > 0)
        //     {
        //         currentState--;
        //         //rectClip.Y = (int)currentState * SONICHEIGHT;
        //         //dikte.Top = (int)currentState * SONICHEIGHT;
        //     }
        // }

        // private void Animate_Left()
        // {
        //     IsQdown = false;
        //     if (teller > 0)
        //     {
        //         teller--;
        //         //rectClip.X = teller * SONICWIDHT;
        //         //dikte.Left = teller * SONICWIDHT;
        //     }
        // }

        // private void Animate_Right()
        // {
        //     IsDdown = false;
        //     if (tellerFrames == 0 || currentState == CURRENT_STATE.AFK)
        //     {
        //         tellerFrames++;

        //         switch (currentState)
        //         {
        //             case CURRENT_STATE.AFK:
        //                 if (teller < 3 && teller > 0) { goto default; }
        //                 teller = 1;
        //                 break;
        //             case CURRENT_STATE.WAITING:
        //                 //if (teller < 5) { goto default; }
        //                 teller = 0;
        //                 break;
        //             case CURRENT_STATE.WALKING:
        //                 if (teller < 4)
        //                 {
        //                     goto default;
        //                 }
        //                 teller = 0;
        //                 break;

        //             case CURRENT_STATE.RUNNING:
        //                 if (teller < 3)
        //                 {
        //                     goto default;
        //                 }
        //                 teller = 0;
        //                 break;

        //             case CURRENT_STATE.PUSHING:
        //                 if (teller < 3)
        //                 {
        //                     goto default;
        //                 }
        //                 teller = 0;
        //                 break;

        //             case CURRENT_STATE.BREAKING:
        //                 if (teller < 1)
        //                 {
        //                     goto default;
        //                 }
        //                 teller = 0;
        //                 break;

        //             default:
        //                 teller++;
        //                 break;
        //         }
        //         //rectClip.X = teller * SONICWIDHT;
        //         //dikte.Left = teller * SONICWIDHT;
        //         ////Animatie.Margin.Left = Xpos + SONICWIDHT;

        //     }
        //     else
        //     {
        //         tellerFrames = 0;
        //     }

        // }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //TellerKeyDown++;
            //lblTeller.Content = "Ticks = " + TellerTick.ToString() + " Key_downs = " + TellerKeyDown.ToString() + " diff = " + (TellerTick - TellerKeyDown);
            if (Keyboard.IsKeyDown(e.Key))
            {
                KeysDown[e.Key.ToString()] = true;
                switch (e.Key)
                {
                    //case Key.D:
                    //    //KeysDown["D"] = true;
                    //    break;
                    //case Key.Q:
                    //    IsQdown = true;
                    //    break;
                    //case Key.S:
                    //    IsSdown = true;
                    //    break;
                    //case Key.Z:
                    //    IsZdown = true;
                    //    break;
                    //case Key.Space:
                    //    IsSpacedown = true;
                    //    break;
                    case Key.Escape:
                        isPauzed = !isPauzed;
                        if (isPauzed)
                        {
                            disTimer.Stop();
                            Menu();
                        }
                        else
                        {
                            disTimer.Start();
                        }
                        break;
                    case Key.Up:
                        Map.MoveMap(0, -10);
                        break;
                    case Key.Down:
                        Map.MoveMap(0, 10);
                        break;
                    case Key.Left:
                        Map.MoveMap(-10, 0);
                        break;
                    case Key.Right:
                        Map.MoveMap(10, 0);
                        break;

                    default:
                        break;
                }

                // rectClip.Y = (int)currentState * SONICHEIGHT;
                // rectClip.X = teller * SONICWIDHT;
                // //dikte.Left = teller * SONICWIDHT;
                // dikte.Top = (int)currentState * (SONICHEIGHT * -1);
                // if (currentState == CURRENT_STATE.AFK)
                // {
                //     rectClip.Y = 0;
                //     dikte.Top = 0;
                // }
                // TimerTeller = 0;
                // ClipImage(sonicImage, rectClip, false);
                //ClipImage(Animatie, rectClip);

            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            KeysDown[e.Key.ToString()] = false;

            //switch (e.Key)
            //{
            //    case Key.Z:
            //        IsZdown = false;
            //        break;
            //    case Key.Q:
            //        IsQdown = false;
            //        break;
            //    case Key.S:
            //        IsSdown = false;
            //        break;
            //    case Key.D:
            //        KeysDown["D"] = false;
            //        break;
            //    case Key.Space:
            //        IsSpacedown = false;
            //        break;
            //    default:
            //        break;
            //}
            //MessageBox.Show(e.Key.ToString());
            //Window_KeyDown(sender, e);
        }

        ///<summary>
        ///Remove the backgroundcolor
        /// </summary>
        /// <param name="path"> the path of the file in string</param>
        public static BitmapImage RemoveBackGroundColor(string path, byte Red = 255, byte Green = 0, byte Blue = 255)
        {

            Bitmap myBitmap = BitmapImage2Bitmap(new BitmapImage(new Uri(path, UriKind.Absolute)));
            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    System.Drawing.Color c = myBitmap.GetPixel(i, j);
                    if (c.R == Red && c.G == Green && c.B == Blue)
                    {
                        myBitmap.SetPixel(i, j, System.Drawing.Color.Transparent);
                    }
                }
            }
            return Bitmapp.ToBitmapImage(myBitmap);

        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Put something in the DebugLabel
        /// </summary>
        /// <param name="whatToDebug">key = what text the label must say, value = what data the label must say. Dont use space and/or '=' in the end and the beginning of the key and value</param>
        public static void DebugText(Dictionary<string, string> whatToDebug)
        {
            string newText = "";
            if (whatToDebug.Count > 0)
            {
                foreach (var keyvalue in whatToDebug)
                {
                    newText += keyvalue.Key + " = " + keyvalue.Value + " ";
                }
            }
            TestWPF.MainWindow item = Application.Current.Windows[0] as TestWPF.MainWindow;
            if (newText != item.lblTeller.Content.ToString())
            {
                item.lblTeller.Content = newText;
            }
        }
    }
    public static class Bitmapp
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

    }
}
/// <summary>
/// Resize the image to the specified width and height.
/// </summary>
/// <param name="image">The image to resize.</param>
/// <param name="width">The width to resize to.</param>
/// <param name="height">The height to resize to.</param>
/// <returns>The resized image.</returns>
//public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
//{
//    var destRect = new System.Drawing.Rectangle(0, 0, width, height);
//    var destImage = new Bitmap(width, height);

//    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

//    using (var graphics = Graphics.FromImage(destImage))
//    {
//        graphics.CompositingMode = CompositingMode.SourceCopy;
//        graphics.CompositingQuality = CompositingQuality.HighQuality;
//        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
//        graphics.SmoothingMode = SmoothingMode.HighQuality;
//        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

//        using (var wrapMode = new ImageAttributes())
//        {
//            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
//            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
//        }
//    }

//    return destImage;
//}
