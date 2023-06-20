using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseTestInConsole
{
    public abstract class Control
    {
        public int minWidth = 10, minHeight = 5, maxWidth = 25, maxHeight = 25;
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public int X = 0, Y = 0;
        public abstract string ControlType { get; }

        //public abstract event EventHandler OnMouseHover;
        //public abstract event EventHandler OnMouseDown;

        public virtual void Draw(byte MouseState)
        {
            return;
        }
        public virtual void Draw()
        {
            return;
        }

        public virtual void MouseDown()
        {
            return;
        }

    }
    public class Label : Control
    {
        
        public override string ControlType
        {
            get
            {
                return "label";
            }
        }
        public int minWidth = 5, minHeight = 1, maxWidth = 50;
        private int _width = 10;//, _height = 5;
        public string Caption = "label text";
        public ConsoleColor BGColor = ConsoleColor.Black,
                            FGColor = ConsoleColor.White;
        public override int Width { 
            get
            {
                return _width;
            }
            set
            {
                if (value < minWidth)
                    _width = minWidth;
                else if (value > maxWidth)
                    _width = maxWidth;
                else
                    _width = value;
            }
        }
        public override int Height { get { return 1; } set { ; } }

        
        public override void Draw( byte MouseState)
        {
            
            int _CL = Console.CursorLeft, _CT = Console.CursorTop;
            ConsoleColor TempBGC = Console.BackgroundColor, TempFGC = Console.ForegroundColor;
            //draw text
            string Text = Caption.Length > Width ? 
                Caption.Substring(0, Width) :
                Caption.PadRight(Width - Caption.Length);
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = BGColor; Console.ForegroundColor = FGColor;
            Console.Write(Text);

            //Console.CursorVisible = true;
            Console.CursorLeft = _CL; Console.CursorTop = _CT;
            Console.BackgroundColor = TempBGC; Console.ForegroundColor = TempFGC;
        }

    }
    public class Button : Control
    {
        private static int _width = 5, _height = 5;
        private byte _currentState = 0;
        public bool Borders = false;

        public event EventHandler OnMouseDown;

        public override string ControlType
        {
            get
            {
                return "button";
            }
        }
        public  byte CurrentState
        {
            get { return _currentState; }
        }
        
        public override int Width
        {
            get { return _width; }
            set
            {
                if (value < minWidth)
                    _width = minWidth;
                else if (value > maxWidth)
                    _width = maxWidth;
                else
                    _width = value;
            }
        }
        public override int Height
        {
            get { return _height; }
            set
            {
                if (value < minHeight)
                    _height = minHeight;
                else if (value > maxHeight)
                    _height = maxHeight;
                else
                    _height = value;
            }
        }


        private string _caption = "button";
        public ConsoleColor BGColor = ConsoleColor.DarkBlue,
                            HoverColor = ConsoleColor.DarkCyan,
                            DownColor = ConsoleColor.DarkGray,
                            BorderColor = ConsoleColor.White,
                            CaptionColor = ConsoleColor.White;

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (Borders)
                    _caption = value.Substring(0, value.Length > _width - 3 ? _width - 3 : value.Length);
                else
                    _caption = value.Substring(0, value.Length > _width - 1 ? _width - 1 : value.Length);
            }
        }

        public Button() { }
        public override void Draw(byte MouseState)
        {
            _currentState = MouseState;
            int _CL = Console.CursorLeft, _CT = Console.CursorTop;
            ConsoleColor TempBGC = Console.BackgroundColor, TempFGC = Console.ForegroundColor;
            if (MouseState == 2)
            {
                Console.ForegroundColor = DownColor;
            }
            else if (MouseState == 1)
            {
                Console.ForegroundColor = HoverColor;
            }
            else if (MouseState == 0)
            {
                Console.ForegroundColor = BGColor;
            }
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Console.SetCursorPosition(X + x, Y + y);
                    Console.Write((char)0x2588);
                }
            }
            if (Borders)
            {

                //Console.BackgroundColor = MouseState == 1 ? BGColor : DownColor;

                if (MouseState == 2) // down
                {
                    Console.BackgroundColor = DownColor;
                }
                else if (MouseState == 1) //hover
                {
                    Console.BackgroundColor = HoverColor;
                }
                else if (MouseState == 0) //steady
                {
                    Console.BackgroundColor = BGColor;
                }
                Console.ForegroundColor = BorderColor;
                //draw corners
                Console.SetCursorPosition(X, Y);
                Console.Write((char)0x250F);

                Console.SetCursorPosition(X + Width - 1, Y);
                Console.Write((char)0x2513);

                Console.SetCursorPosition(X, Y + Height - 1);
                Console.Write((char)0x2517);

                Console.SetCursorPosition(X + Width - 1, Y + Height - 1);
                Console.Write((char)0x251B);



                //draw horizontal lines
                for (int i = 1; i < Width - 1; i++)
                {
                    Console.CursorTop = Y;
                    Console.CursorLeft = i + X;
                    Console.Write((char)0x2501);

                    Console.CursorTop = Y + Height - 1;
                    Console.CursorLeft = i + X;
                    Console.Write((char)0x2501);

                }

                //draw vertrical lines
                Console.CursorLeft = X;
                for (int i = 1; i < Height - 1; i++)
                {
                    Console.CursorLeft = X;
                    Console.CursorTop = i + Y;
                    Console.Write((char)0x2503);
                    Console.CursorLeft = X + Width - 1;
                    Console.Write((char)0x2503);
                }

            }
            //draw caption
            Console.SetCursorPosition(
                X + ((Width / 2) - (_caption.Length / 2)),
                Y + (Height / 2));
            if (MouseState == 2) // down
            {
                Console.BackgroundColor = DownColor;
            }
            else if (MouseState == 1) //hover
            {
                Console.BackgroundColor = HoverColor;
            }
            else if (MouseState == 0) //steady
            {
                Console.BackgroundColor = BGColor;
            }
            Console.ForegroundColor = BorderColor;
            Console.Write(_caption);

            Console.CursorLeft = _CL; Console.CursorTop = _CT;
            Console.BackgroundColor = TempBGC; Console.ForegroundColor = TempFGC;
            if (MouseState == 2 && OnMouseDown != null) { Console.Clear();  OnMouseDown(this, new EventArgs() ); }
        }
        public override void MouseDown()
        {
            Draw(1);
        }
    }
    public class Thermometer : Control
    {
        public int Left = 0, Top = 0, MinValue = 0, MaxValue = 100;
        public const int MaxWidth = 10, MaxHeight = 15;
        public ConsoleColor BGColor, FGColor, BarColor, BarBGColor, BracketColor;
        public bool UseMultiColorBar = false;
        public char BarFill = (char)0x2588;
        private int ww, hh, vv;
        
        

        public int Value
        {
            get { return vv; }
            set
            {
                if (value < MinValue)
                    vv = MinValue;
                else if (value > MaxValue)
                    vv = MaxValue;
                else
                    vv = value;
            }
        }
        public struct BarSection
        {
            public int Value;
            public ConsoleColor Color;
        }
        public BarSection Low, Middle, High;
        public override int Width
        {
            get { return ww; }
            set
            {
                if (value < 1)
                    ww = 1;
                else if (value > MaxWidth)
                    ww = MaxWidth;
                else
                    ww = value;
            }
        }
        public override int Height
        {
            get { return hh; }
            set
            {
                if (value < 1)
                    hh = 1;
                if (value > MaxHeight)
                    hh = MaxHeight;
                else
                    hh = value;

            }
        }

        public override string ControlType {
            get { return "thermometer"; }
        }

        public static int vMap(int Fmin, int Fmax, int Tmin, int Tmax, int value)
        {
            int m = (Fmax - Fmin) / (Tmax - Tmin);
            return ((Fmin + value) / m) + Tmin;
        }
        public Thermometer()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Low.Color = ConsoleColor.DarkGreen;
            Low.Value = 70;
            Middle.Color = ConsoleColor.DarkYellow;
            Middle.Value = 85;
            High.Color = ConsoleColor.DarkRed;
            High.Value = 100;

            ww = 2; hh = 10;
            //set defs
            BGColor = ConsoleColor.Black; FGColor = ConsoleColor.Gray; BarColor = ConsoleColor.Green; BarBGColor = ConsoleColor.DarkGray; BracketColor = ConsoleColor.DarkGray;
        }
        public override void Draw( byte MouseState)
        {

            Console.CursorVisible = false;
            int _CL = Console.CursorLeft, _CT = Console.CursorTop;
            ConsoleColor TempBGC = Console.BackgroundColor, TempFGC = Console.ForegroundColor;




            Console.ForegroundColor = TempBGC;
            //reset area
            //draw bar bg
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Console.CursorLeft = x + Left;
                    Console.CursorTop = y + Top;
                    Console.Write('x');
                }
            }

            Console.BackgroundColor = BGColor;
            Console.ForegroundColor = BracketColor;

            //draw corners
            Console.SetCursorPosition(Left, Top);
            Console.Write((char)0x250F);

            Console.SetCursorPosition(Left + Width, Top);
            Console.Write((char)0x2513);

            Console.SetCursorPosition(Left, Top + Height);
            Console.Write((char)0x2517);

            Console.SetCursorPosition(Left + Width, Top + Height);
            Console.Write((char)0x251B);


            //draw horizontal lines
            for (int i = 1; i < Width; i++)
            {
                Console.CursorTop = Top;
                Console.CursorLeft = i + Left;
                Console.Write((char)0x2501);

                Console.CursorTop = Top + Height;
                Console.CursorLeft = i + Left;
                Console.Write((char)0x2501);

            }

            //draw vertrical lines
            Console.CursorLeft = Left;
            for (int i = 1; i < Height; i++)
            {
                Console.CursorLeft = Left;
                Console.CursorTop = i + Top;
                Console.Write((char)0x2503);
                Console.CursorLeft = Left + Width;
                Console.Write((char)0x2503);
            }

            //draw bar bg
            Console.ForegroundColor = BarBGColor;
            for (int x = 1; x < Width; x++)
            {
                for (int y = 1; y < Height; y++)
                {
                    Console.CursorLeft = x + Left;
                    Console.CursorTop = y + Top;
                    Console.Write((char)0x2592);


                }
            }

            //calculate value
            int barVal = vMap(MinValue, MaxValue, 0, Height, vv);

            if (!UseMultiColorBar)
            {
                //draw bar single color
                Console.ForegroundColor = BarColor;
                for (int x = 1; x < Width; x++)
                {
                    for (int y = 2 + Height - barVal; y < Height; y++)
                    {
                        Console.CursorLeft = x + Left;
                        Console.CursorTop = y + Top;
                        Console.Write(BarFill);
                    }
                }
            }
            else
            {
                //values for colors
                int mLow = vMap(MinValue, MaxValue, 0, Height, Low.Value);
                int mMid = vMap(MinValue, MaxValue, 0, Height, Middle.Value);
                int mHigh = vMap(MinValue, MaxValue, 0, Height, High.Value);

                //draw bar multi color
                Console.ForegroundColor = BarColor;
                for (int x = 1; x < Width; x++)
                {
                    for (int y = 2 + Height - barVal; y < Height; y++)
                    {
                        Console.CursorLeft = x + Left;
                        Console.CursorTop = y + Top;
                        int ct = Console.CursorTop;
                        if (ct >= Top + Height - mLow)
                        {
                            Console.ForegroundColor = Low.Color;
                        }
                        if (ct >= Top + Height - mMid && ct <= Top + Height - mLow)
                        {
                            Console.ForegroundColor = Middle.Color;
                        }
                        if (ct >= Top + Height - mHigh && ct <= Top + Height - mMid)
                        {
                            Console.ForegroundColor = High.Color;
                        }


                        Console.Write(BarFill);
                    }
                }

            }
            Console.CursorLeft = _CL; Console.CursorTop = _CT;
            Console.BackgroundColor = TempBGC; Console.ForegroundColor = TempFGC;

        }
    }

    internal class Program
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShowCursor);
        public struct LPoint
        {
            public long X; public long Y;
        }
        [DllImport("user32.dll")]
        static extern LPoint GetCursorPos();


       
        public static int vMap(int Fmin, int Fmax, int Tmin, int Tmax, int value)
        {
            int m = (Fmax - Fmin) / (Tmax - Tmin);
            return ((Fmin + value) / m) + Tmin;
        }
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Button btn = new Button()
            {
                Width = 20,
                Height = 9,
                X = 50,
                Y = 8,
                Caption = "Exit",
                BGColor = ConsoleColor.DarkRed,
                HoverColor = ConsoleColor.Red,
                Borders = true
            };
            btn.Draw(0);
            Button btn1 = new Button()
            {
                Width = 20,
                Height = 4,
                X = 10,
                Y = 8,
                Caption = "Borderless",
                Borders = false
            };
            btn1.Draw(0);
            Label lbl = new Label()
            {
                BGColor = ConsoleColor.DarkBlue,
                FGColor = ConsoleColor.Cyan,
                Width = 30,
                X = 9,
                Y = 5,
                Caption = "Hello"
            };
            Thermometer tmr = new Thermometer()
            {
                Left = 2,
                Top = 2,
                Width = 5,
                Height = 25,
                UseMultiColorBar = true
            };

            List<Control> Controls = new List<Control>
            {
                btn,
                btn1,
                lbl,
                tmr
            };

            ((Button)btn).OnMouseDown += (o, e) => { Environment.Exit(0); };
            ((Button)btn1).OnMouseDown += (o, e) => { 
                lbl.Width = 40; lbl.Caption = "You just clicked on borderless Button.";
                Random xx = new Random();
                tmr.Value = xx.Next(100);
            };

            LPoint f, lastpos = GetCursorPos();
            Console.CursorSize = 100;
            while (true)
            {
                f = GetCursorPos();

                long cMX = Convert.ToInt64(Convert.ToString(f.X, toBase: 2).PadLeft(64, '0').Substring(32, 32), 2);
                long lMX = Convert.ToInt64(Convert.ToString(lastpos.X, toBase: 2).PadLeft(64, '0').Substring(32, 32), 2);
                long cMY = f.X >> 32;
                long lMY = lastpos.X >> 32;

                Thread.Sleep(100);
                int cx = Console.CursorLeft;
                int cy = Console.CursorTop;
                Console.Title = $"{cx} - {cy}";

                Console.CursorLeft = (vMap(0, 1920, 0, Console.WindowWidth, ((int)cMX)));
                Console.CursorTop = (vMap(0, 1080, 0, Console.WindowHeight, ((int)cMY)));
                Console.CursorVisible = false;
                foreach (Control ctrl in Controls)
                {

                    if (cx > ctrl.X && cx < ctrl.X + ctrl.Width && cy > ctrl.Y && cy < ctrl.Y + ctrl.Height)
                    {
                        if (GetAsyncKeyState(0x01) < 0)

                        {
                            if (ctrl.ControlType == "button")
                            {
                                if (((Button)ctrl).CurrentState != 2)
                                {
                                    ctrl.Draw(2);
                                }
                            }
                            
                        }
                        else
                        {
                            if (ctrl.ControlType == "button")
                            {
                                if (((Button)ctrl).CurrentState != 1)
                                {
                                    ctrl.Draw(1);
                                }
                            }

                            //break;
                        }
                    }
                    else
                    {
                        if (cMX != lMX || cMY != lMY)
                        {
                            ctrl.Draw(0);
                            Console.CursorVisible = true;
                        }

                    }
                }
                
                lastpos = f;
            }
        }
    }
}
