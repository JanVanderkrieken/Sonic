using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TestWPF
{
    partial class IEnemy
    {
        protected double XPos, YPos, BeginXPos, BeginYPos, EndXPos, EndYPos = 0;
        public double MySpeedX, MySpeedY = 0;
        /* Faster/slower speed
         * A = diff between BeginPos and actual pos
         * B = diff between EndPos and Actual pos
         * C = diff between BeginPos and EndPos
         * 
         * if (B>A)
         * {
         *      speed *= C/B
         * }
         * else if (B<A)
         * {
         *      speed *= C/A
         * }
         */

        protected bool IsFlipped = true;

        protected BitmapImage MyImage;
        public System.Windows.Controls.Image MyImageControl = new System.Windows.Controls.Image();
        protected double MyWidth, MyHeight = 0;

        protected int AnimationSteps = 1;
        protected double TickTeller = 0;

        protected Rect clipRect = new Rect(0, 0, 0, 0); // Rectangle for the specific part of the image that must been shown

        //var MyHitBox


        /// <summary>
        /// Create a new enemy with a Begin Position, an End Position and a speed
        /// </summary>
        /// <param name="BeginX">The Starting X Position</param>
        /// <param name="BeginY">The Starting Y Position</param>
        /// <param name="EndX">The Ending X Position</param>
        /// <param name="EndY">The Ending Y Position</param>
        /// <param name="SpeedX">The Horizontal Speed</param>
        /// <param name="SpeedY">The Vertical Speed</param>
        public IEnemy(double BeginX, double BeginY, double EndX, double EndY, double SpeedX, double SpeedY)
        {
            XPos = BeginX;
            YPos = BeginY;
            MySpeedX = SpeedX;
            MySpeedY = SpeedY;
            BeginXPos = BeginX;
            BeginYPos = BeginY;
            EndXPos = EndX;
            EndYPos = EndY;
        }


        /// <summary>
        /// Create a new enemy with a static Position
        /// </summary>
        /// <param name="XPosition">The X Position</param>
        /// <param name="YPosition">The Y Position</param>
        public IEnemy(double XPosition, double YPosition) : this(XPosition, YPosition, XPosition, YPosition, 0, 0)
        {
        }

        //public double,double GetPos()
        //{
        //    return XPos,YPos;
        //}

        public void SetPos(double NewX, double NewY)
        {
            XPos = NewX;
            YPos = NewY;
        }

        /*
         public bool GetHit(var otherHitBox)
         {
            return MyHitbox v otherHitbox;
         }
         
         
         */
        public virtual void Tick(double deltaTime)
        {
            TickTeller++;

            XPos += deltaTime * MySpeedX;
            YPos += deltaTime * MySpeedY;

            if (TickTeller % AnimationSteps == 0)
            {
                TickTeller = 0;
            }

            clipRect.X = TickTeller * MyWidth;
            

            MyImageControl.Margin = new Thickness(XPos, YPos, MyWidth, MyHeight);

            if (XPos >= EndXPos) //from right to left
            {
                IsFlipped = false;
                MySpeedX *= -1;
            }
            if (XPos <= BeginXPos) // from left to right
            {
                IsFlipped = true;
                MySpeedX *= -1;
            }

            //if (EndXPos - XPos < XPos - BeginXPos)
            //{
            //    MySpeedX *= (EndXPos - BeginXPos) / (XPos - BeginXPos);
            //}
            //else
            //{
            //    MySpeedX *= (EndXPos - BeginXPos) / (EndXPos - XPos);
            //}
            //if (MySpeedX >= 20)
            //{
            //    MySpeedX = 20;
            //}
            //else if (MySpeedX <= -20)
            //{
            //    MySpeedX = -20;
            //}
            

            //if (YPos <= EndYPos) //from bottom to top
            //{
            //    IsFlipped = true;
            //    MySpeedX *= -1;
            //}
            //if (YPos >= BeginYPos) // from top to bottom
            //{
            //    IsFlipped = false;
            //    MySpeedX *= -1;
            //}



            if (IsFlipped)
            {
                MyImageControl.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                MyImageControl.FlowDirection = FlowDirection.LeftToRight;
            }
        }


    }
}
