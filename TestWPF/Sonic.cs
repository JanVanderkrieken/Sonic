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
    public class Sonic
    {
        /*
 * STATE_WAITING: teller = 0 && height = 0
 * STATE_WALKING: teller van 0 tot en met 4 && height = 1
 * STATE_RUNNING: teller van 0 tot en met 3 && height = 2
 * STATE_PUSHING: teller van 0 tot en met 3 && height = 3
 * STATE_BREAKING: teller 0 en 1 && height = 4
 * STATE_AFK: teller = 1 en 2 && height = 0
 * STATE_UP teller = 4 && height = 0
 * STATE_DOWN teller = 5 && height = 0
 * STATE_ROLLING Different Image height = 0
 */
        public enum CURRENT_STATE
        {
            WAITING = 0,
            WALKING = 1,
            RUNNING = 2,
            PUSHING = 3,
            BREAKING = 4,
            AFK = -1,
            ROLLING = 5,
            //    INTHEAIR = 6, // This is for when sonic In the air so he couldn't jump
            LOOKDOWN = -6,
            LOOKUP = -7,
            SPRINGJUMP = 8,
            DAMAGE = 9,
            DEAD = 10,
            BALANCING = 11,
        }

        double MyWidth, MyHeight = 0;

        double MyBreakingXPos, MyBreakingYPos = 0;

        double MyXSpeed, MyYSpeed = 0;

        bool IsIntheAir = true; // a bool to check if he is in the air so he couldn't jump (again)
                                // would set to true if SPACE would be pressed or he is using a spring up or fall from a cliff
                                // can be used to check if he needs gravity or not
        public bool IsFlipped = false;

        double Tickteller, teller = 0;

        private CURRENT_STATE currentState;
        public Sonic()
        {
            sonicImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Sonic1.png", UriKind.Absolute));
            sonicRollingImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Sonic_Rolling.bmp", UriKind.Absolute));
            //sonicImage = MainWindow.RemoveBackGroundColor("pack://application:,,,/Resources/Sonic1.png");
            SonicImageControl = new System.Windows.Controls.Image();
            //sonicRollingImage = MainWindow.RemoveBackGroundColor("pack://application:,,,/Resources/Sonic_Rolling.bmp");
            currentState = CURRENT_STATE.WAITING;
            MyWidth = sonicImage.Width / 6;
            MyHeight = sonicImage.Height / 5;

            MyRect = new Rect(0, 0, MyWidth, MyHeight);
            SonicImage.Margin = new Thickness(0, 200, MyWidth, MyHeight);

        }
        private BitmapImage sonicImage;
        private BitmapImage sonicRollingImage;

        private System.Windows.Controls.Image SonicImageControl;

        private Rect MyRect;
        public System.Windows.Controls.Image SonicImage
        {
            get
            {
                SonicImageControl.Source = sonicImage;
                if (currentState == CURRENT_STATE.ROLLING)
                {
                    SonicImageControl.Source = sonicRollingImage;
                }

                SonicImageControl.Stretch = Stretch.None;
                SonicImageControl.HorizontalAlignment = HorizontalAlignment.Left;
                SonicImageControl.VerticalAlignment = VerticalAlignment.Top;


                return SonicImageControl;
            }
        }
        public CURRENT_STATE GetCurrentState => currentState;

        public void Tick(double Deltatime, Dictionary<string, bool> KeysDown)
        {
            Tickteller++;
            teller++;
            switch (currentState)
            {
                case CURRENT_STATE.AFK:
                    if (teller < 3 && teller > 0) { goto default; }
                    teller = 1;
                    break;
                case CURRENT_STATE.WAITING:
                    //if (teller < 5) { goto default; }
                    teller = 0;
                    break;
                case CURRENT_STATE.WALKING:
                    if (teller < 4)
                    {
                        goto default;
                    }
                    teller = 0;
                    break;

                case CURRENT_STATE.RUNNING:
                    if (teller < 3)
                    {
                        goto default;
                    }
                    teller = 0;
                    break;

                case CURRENT_STATE.PUSHING:
                    if (teller < 3)
                    {
                        goto default;
                    }
                    teller = 0;
                    break;

                case CURRENT_STATE.BREAKING:
                    if (!IsFlipped && SonicImageControl.Margin.Left < MyBreakingXPos + 50)
                    {
                        MyXSpeed = 10;
                        MoveMe(1, MyXSpeed, MyYSpeed);
                    }
                    else if (IsFlipped && SonicImageControl.Margin.Left > MyBreakingXPos - 50)
                    {
                        MyXSpeed = -10;
                        MoveMe(1, MyXSpeed, MyYSpeed);
                    }
                    else if (SonicImageControl.Margin.Left <= MyBreakingXPos - 50 || SonicImageControl.Margin.Left >= MyBreakingXPos + 50)
                    {
                        currentState = CURRENT_STATE.WAITING;

                    }
                    if (teller < 1)
                    {
                        goto default;
                    }
                    teller = 0;
                    break;

                case CURRENT_STATE.LOOKDOWN:
                    teller = 5;
                    break;

                case CURRENT_STATE.LOOKUP:
                    teller = 4;
                    break;
                default:
                    teller++;
                    break;
            }

            if (KeysDown.ContainsValue(true))
            {
                if (KeysDown["D"])
                {
                    if (KeysDown["Q"])
                    {
                        if (currentState == CURRENT_STATE.RUNNING || (currentState == CURRENT_STATE.ROLLING && !IsIntheAir))
                        {
                            currentState = CURRENT_STATE.BREAKING;
                            MyBreakingXPos = SonicImageControl.Margin.Left;
                        }
                        else if (currentState == CURRENT_STATE.WALKING)
                        {
                            currentState = CURRENT_STATE.WAITING;
                        }
                    }
                    else
                    {
                        MyXSpeed = 10;
                        IsFlipped = false;
                        MoveMe(1, MyXSpeed, MyYSpeed);
                        currentState = CURRENT_STATE.WALKING;
                    }
                }
                if (KeysDown["Z"])
                {
                    if (currentState == CURRENT_STATE.WAITING || currentState == CURRENT_STATE.AFK || currentState == CURRENT_STATE.LOOKUP)
                    {
                        currentState = CURRENT_STATE.LOOKUP;
                    }
                }
                if (KeysDown["Q"])
                {
                    if (KeysDown["D"])
                    {
                        if (currentState == CURRENT_STATE.RUNNING)
                        {
                            currentState = CURRENT_STATE.BREAKING;
                        }
                        else if (currentState == CURRENT_STATE.WALKING)
                        {
                            currentState = CURRENT_STATE.WAITING;
                        }
                    }
                    else
                    {
                        MyXSpeed = -10;
                        IsFlipped = true;
                        MoveMe(1, MyXSpeed, MyYSpeed);
                        currentState = CURRENT_STATE.WALKING;
                    }

                }
                if (KeysDown["S"])
                {
                    if (currentState == CURRENT_STATE.WAITING || currentState == CURRENT_STATE.AFK || currentState == CURRENT_STATE.LOOKDOWN)
                    {
                        currentState = CURRENT_STATE.LOOKDOWN;
                    }
                }
                if (KeysDown["Space"])
                {
                    if (!IsIntheAir)
                    {
                        currentState = CURRENT_STATE.ROLLING;
                    }
                }

            }
            else
            {
                currentState = CURRENT_STATE.WAITING;
            }

            MyRect.Y = (int)currentState * MyHeight;
            if ((int)currentState < 0)
            {
                MyRect.Y = 0;
            }
            MyRect.X = teller * MyWidth;

            if (Tickteller >= 30 && (currentState == CURRENT_STATE.WAITING || currentState == CURRENT_STATE.AFK))
            {
                //currentState = CURRENT_STATE.AFK;
                //Animate_Right();
                //dikte.Left = teller * SONICWIDHT;
                //dikte.Top = (int)currentState * (MyHeight * -1);
                if (currentState == CURRENT_STATE.AFK)
                {
                    MyRect.Y = 0;
                    //dikte.Top = 0;
                }
            }

            /*
                Gravity norm = 9.811 m/s^2
                In Sonic.cpp m_Velocity += m_Acceleration * deltatime
                m_Pos.x += m_Velocity * deltatime
             */


            ClipImage(SonicImageControl, MyRect, IsFlipped);
            //ClipImage(SonicImageControl, new Rect(0,0,sonicImage.Width,sonicImage.Height), IsFlipped);
            MainWindow.DebugText(new Dictionary<string, string> { { "My Left", SonicImage.Margin.Bottom.ToString() }, { "My Right", SonicImage.Margin.Top.ToString() } });
        }

        public void MoveMe(double deltatime, double Xspeed, double Yspeed)
        {
            var thick = SonicImageControl.Margin;
            thick.Left += Xspeed * deltatime;// This is m = m/s *s
            //thick.Right += Xspeed * deltatime;
            if (thick.Left >= MainWindow.WidthWindow / 2 && !IsFlipped)
            {
                thick.Left = MainWindow.WidthWindow / 2;
                Map.MoveMap(-Xspeed, -Yspeed);
            }
            if (thick.Left <= 0 && IsFlipped)
            {
                thick.Left = 0;
                if (Map.MapImageControl1.Margin.Left < 0)
                {
                    Map.MoveMap(-Xspeed, -Yspeed);
                }
                else
                {
                    currentState = CURRENT_STATE.PUSHING;
                }
            }
            thick.Top += Yspeed * deltatime;
            SonicImageControl.Margin = thick;
            MyXSpeed = 0;
            MyYSpeed = 0;
        }
    }
}
