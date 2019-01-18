using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF
{
    class LadyBug : IEnemy
    {
        public LadyBug(double XPosition, double YPosition) : base(XPosition, YPosition)
        {
        }

        public LadyBug(double BeginX, double BeginY, double EndX, double EndY, double SpeedX, double SpeedY) : base(BeginX, BeginY, EndX, EndY, SpeedX, SpeedY)
        {
            AnimationSteps = 2;
            MyImage = MainWindow.RemoveBackGroundColor(@"pack://application:,,,/Resources/LadyBug.bmp");
            //MyImage = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/LadyBug.bmp"));
            MyImageControl.Source = MyImage;
            MyWidth = MyImage.Width / AnimationSteps;
            MyHeight = MyImage.Height;
            clipRect.Width = MyWidth;
            clipRect.Height = MyHeight;
        }

        public override void Tick(double deltaTime)
        {
            base.Tick(deltaTime);
            
            MainWindow.ClipImage(MyImageControl, clipRect, IsFlipped);
        }
    }
}
