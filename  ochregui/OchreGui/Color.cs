//Copyright (c) 2010 Shane Baker
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//

using System;

using libtcod;

namespace OchreGui
{
    #region Color Class
    /// <summary>
    /// This class wraps a TCODColor in an immutable data type.  Provides nearly identical
    /// functionality as TCODColor.
    /// </summary>
    public class Color : IDisposable
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a Color from specified tcodColor.  Makes a copy of the tcodColor instead
        /// of keeping a reference.
        /// </summary>
        /// <param name="tcodColor"></param>
        public Color(TCODColor tcodColor)
        {
            if (tcodColor == null)
            {
                throw new ArgumentNullException("tcodColor");
            }

            red = tcodColor.Red;
            green = tcodColor.Green;
            blue = tcodColor.Blue;

            color = new TCODColor(red, green, blue);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a Color from the provided reg, green and blue values (0-255 for each)
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;

            color = new TCODColor(red, green, blue);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct color given 3 byte integer (ex. 0xFFFFFF = white)
        /// </summary>
        /// <param name="packedColor"></param>
        public Color(long packedColor)
        {
            long r, g, b;

            r = packedColor & 0xff0000;
            g = packedColor & 0x00ff00;
            b = packedColor & 0x0000ff;

            r = r >> 16;
            g = g >> 8;


            this.red = (byte)r;
            this.green = (byte)g;
            this.blue = (byte)b;

            color = new TCODColor(red, green, blue);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the red value of color, 0-255
        /// </summary>
        public byte Red
        {
            get { return red; }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the green value of color, 0-255
        /// </summary>
        public byte Green
        {
            get { return green; }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the blue value of the color, 0-255
        /// </summary>
        public byte Blue
        {
            get { return blue; }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Scales saturation by given amount (0.0 --> 1.0)
        /// Returns new instance - original instance is unchanged
        /// </summary>
        public Color ScaleSaturation(float scale)
        {
            TCODColor ret = new TCODColor();

            float h, s, v;
            color.getHSV(out h, out s, out v);

            ret.setHSV(h, s * scale, v);

            return new Color(ret);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Scales value (brightness) by given amount (0.0 --> 1.0)
        /// Returns new instance - original instance is unchanged
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Color ScaleValue(float scale)
        {
            TCODColor ret = new TCODColor();

            float h, s, v;
            color.getHSV(out h, out s, out v);

            ret.setHSV(h, s, v * scale);

            return new Color(ret);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replaces hue with given hue (0.0 --> 360.0)
        /// Returns new instance - original instance is unchanged
        /// </summary>
        /// <param name="hue"></param>
        /// <returns></returns>
        public Color ReplaceHue(float hue)
        {
            TCODColor ret = new TCODColor();

            float h, s, v;
            color.getHSV(out h, out s, out v);

            ret.setHSV(hue, s, v);

            return new Color(ret);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replaces saturation with given saturation (0.0 --> 1.0)
        /// Returns new instance - original instance is unchanged
        /// </summary>
        /// <param name="saturation"></param>
        /// <returns></returns>
        public Color ReplaceSaturation(float saturation)
        {
            TCODColor ret = new TCODColor();

            float h, s, v;
            color.getHSV(out h, out s, out v);

            ret.setHSV(h, saturation, v);

            return new Color(ret);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replaces value (brightness) with given value (0.0 --> 1.0)
        /// Returns new instance - original instance is unchanged
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Color ReplaceValue(float value)
        {
            TCODColor ret = new TCODColor();

            float h, s, v;
            color.getHSV(out h, out s, out v);

            ret.setHSV(h, s, value);

            return new Color(ret);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns hue (0.0 --> 360.0)
        /// </summary>
        /// <returns></returns>
        public float GetHue()
        {
            float h, s, v;
            color.getHSV(out h, out s, out v);

            return h;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns saturation (0.0 --> 1.0)
        /// </summary>
        /// <returns></returns>
        public float GetSaturation()
        {
            float h, s, v;
            color.getHSV(out h, out s, out v);

            return s;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns value (brightness) (0.0 --> 1.0)
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            float h, s, v;
            color.getHSV(out h, out s, out v);

            return v;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts to a new instance of TCODColor
        /// </summary>
        /// <returns></returns>
        public TCODColor GetTCODColor()
        {
            return new TCODColor(color.Red, color.Green, color.Blue);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns a string that will change the foreground color of the text to this color.
        /// </summary>
        /// <returns></returns>
        public string DoForegroundCode()
        {
            return CodeForeground + GetCode();
        }

        /// <summary>
        /// Returns a string that will change the background color of the text to this color.
        /// </summary>
        /// <returns></returns>
        public string DoBackgroundCode()
        {
            return CodeBackground + GetCode();
        }

        /// <summary>
        /// Returns a string that will set the colors of the text back to default
        /// </summary>
        /// <returns></returns>
        public string DoDefaultColors()
        {
            return Color.StopColorCode;
        }

        private string GetCode()
        {
            char r = (char)(Math.Max(this.red, (byte)1));
            char g = (char)(Math.Max(this.green, (byte)1));
            char b = (char)(Math.Max(this.blue, (byte)1));

            string str = r.ToString() + g.ToString() + b.ToString();

            return str;
        }

        /// <summary>
        /// Returns the foreground color code string.
        /// </summary>
        public static string CodeForeground
        {
            get
            {
                return colorCodeFore;
            }
        }
        static readonly string colorCodeFore = "\x06";

        /// <summary>
        /// Returns the background color code string.
        /// </summary>
        public static string CodeBackground
        {
            get
            {
                return colorCodeBack;
            }
        }
        static readonly string colorCodeBack = "\x07";

        /// <summary>
        /// Returns the stop color code string.
        /// </summary>
        public static string StopColorCode
        {
            get
            {
                return colorCodeStop;
            }
        }
        static readonly string colorCodeStop = "\x08";

        public override string ToString()
        {
            return red.ToString("x2") + green.ToString("x2") + blue.ToString("x2");
        }
        #endregion
        #region Private Fields
        // /////////////////////////////////////////////////////////////////////////////////
        private readonly byte red, green, blue;
        private readonly TCODColor color;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Static
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Wrapper around TCODColor.Interpolate
        /// </summary>
        /// <param name="sourceColor"></param>
        /// <param name="destinationColor"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static Color Lerp(Color sourceColor, Color destinationColor, float coefficient)
        {
            TCODColor color = TCODColor.Interpolate(sourceColor.GetTCODColor(),
                destinationColor.GetTCODColor(), coefficient);

            return new Color(color);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #region Predefined Colors
        public static Color BLACK = new Color( 0,0,0);
        public static Color DARKEST_GREY = new Color( 31,31,31);
        public static Color DARKER_GREY = new Color( 63,63,63);
        public static Color DARK_GREY = new Color( 95,95,95);
        public static Color GREY = new Color( 127,127,127);
        public static Color LIGHT_GREY = new Color( 159,159,159);
        public static Color LIGHTER_GREY = new Color( 191,191,191);
        public static Color LIGHTEST_GREY = new Color( 223,223,223);
        public static Color WHITE = new Color( 255,255,255);        
        
        public static Color DARKEST_SEPIA  = new Color(31,24,15);
        public static Color DARKER_SEPIA  = new Color(63,50,31);
        public static Color DARK_SEPIA  = new Color(94,75,47);
        public static Color SEPIA  = new Color(127,101,63);
        public static Color LIGHT_SEPIA  = new Color(158,134,100);
        public static Color LIGHTER_SEPIA  = new Color(191,171,143);
        public static Color LIGHTEST_SEPIA  = new Color(222,211,195);

        public static Color DESATURATED_RED  = new Color(127,63,63);
        public static Color DESATURATED_FLAME  = new Color(127,79,63);
        public static Color DESATURATED_ORANGE  = new Color(127,95,63);
        public static Color DESATURATED_AMBER  = new Color(127,111,63);
        public static Color DESATURATED_YELLOW  = new Color(127,127,63);
        public static Color DESATURATED_LIME  = new Color(111,127,63);
        public static Color DESATURATED_CHARTREUSE  = new Color(95,127,63);
        public static Color DESATURATED_GREEN  = new Color(63,127,63);
        public static Color DESATURATED_SEA  = new Color(63,127,95);
        public static Color DESATURATED_TURQUOISE  = new Color(63,127,111);
        public static Color DESATURATED_CYAN  = new Color(63,127,127);
        public static Color DESATURATED_SKY  = new Color(63,111,127);
        public static Color DESATURATED_AZURE  = new Color(63,95,127);
        public static Color DESATURATED_BLUE  = new Color(63,63,127);
        public static Color DESATURATED_HAN  = new Color(79,63,127);
        public static Color DESATURATED_VIOLET  = new Color(95,63,127);
        public static Color DESATURATED_PURPLE  = new Color(111,63,127);
        public static Color DESATURATED_FUCHSIA  = new Color(127,63,127);
        public static Color DESATURATED_MAGENTA  = new Color(127,63,111);
        public static Color DESATURATED_PINK  = new Color(127,63,95);
        public static Color DESATURATED_CRIMSON  = new Color(127,63,79 );       

        public static Color LIGHTEST_RED  = new Color(255,191,191);
        public static Color LIGHTEST_FLAME  = new Color(255,207,191);
        public static Color LIGHTEST_ORANGE  = new Color(255,223,191);
        public static Color LIGHTEST_AMBER  = new Color(255,239,191);
        public static Color LIGHTEST_YELLOW  = new Color(255,255,191);
        public static Color LIGHTEST_LIME = new Color( 239,255,191);
        public static Color LIGHTEST_CHARTREUSE  = new Color(223,255,191);
        public static Color LIGHTEST_GREEN  = new Color(191,255,191);
        public static Color LIGHTEST_SEA  = new Color(191,255,223);
        public static Color LIGHTEST_TURQUOISE  = new Color(191,255,239);
        public static Color LIGHTEST_CYAN  = new Color(191,255,255);
        public static Color LIGHTEST_SKY  = new Color(191,239,255);
        public static Color LIGHTEST_AZURE  = new Color(191,223,255);
        public static Color LIGHTEST_BLUE  = new Color(191,191,255);
        public static Color LIGHTEST_HAN  = new Color(207,191,255);
        public static Color LIGHTEST_VIOLET  = new Color(223,191,255);
        public static Color LIGHTEST_PURPLE  = new Color(239,191,255);
        public static Color LIGHTEST_FUCHSIA  = new Color(255,191,255);
        public static Color LIGHTEST_MAGENTA  = new Color(255,191,239);
        public static Color LIGHTEST_PINK  = new Color(255,191,223);
        public static Color LIGHTEST_CRIMSON  = new Color(255,191,207);

        public static Color LIGHTER_RED = new Color(255,127,127);
        public static Color LIGHTER_FLAME = new Color(255,159,127);
        public static Color LIGHTER_ORANGE = new Color(255,191,127);
        public static Color LIGHTER_AMBER = new Color(255,223,127);
        public static Color LIGHTER_YELLOW = new Color(255,255,127);
        public static Color LIGHTER_LIME = new Color(223,255,127);
        public static Color LIGHTER_CHARTREUSE = new Color(191,255,127);
        public static Color LIGHTER_GREEN = new Color(127,255,127);
        public static Color LIGHTER_SEA = new Color(127,255,191);
        public static Color LIGHTER_TURQUOISE = new Color(127,255,223);
        public static Color LIGHTER_CYAN = new Color(127,255,255);
        public static Color LIGHTER_SKY = new Color(127,223,255);
        public static Color LIGHTER_AZURE = new Color(127,191,255);
        public static Color LIGHTER_BLUE = new Color(127,127,255);
        public static Color LIGHTER_HAN = new Color(159,127,255);
        public static Color LIGHTER_VIOLET = new Color(191,127,255);
        public static Color LIGHTER_PURPLE = new Color(223,127,255);
        public static Color LIGHTER_FUCHSIA = new Color(255,127,255);
        public static Color LIGHTER_MAGENTA = new Color(255,127,223);
        public static Color LIGHTER_PINK = new Color(255,127,191);
        public static Color LIGHTER_CRIMSON = new Color(255,127,159);

        public static Color LIGHT_RED = new Color(255,63,63);
        public static Color LIGHT_FLAME = new Color(255,111,63);
        public static Color LIGHT_ORANGE = new Color(255,159,63);
        public static Color LIGHT_AMBER = new Color(255,207,63);
        public static Color LIGHT_YELLOW = new Color(255,255,63);
        public static Color LIGHT_LIME = new Color(207,255,63);
        public static Color LIGHT_CHARTREUSE = new Color(159,255,63);
        public static Color LIGHT_GREEN = new Color(63,255,63);
        public static Color LIGHT_SEA = new Color(63,255,159);
        public static Color LIGHT_TURQUOISE = new Color(63,255,207);
        public static Color LIGHT_CYAN = new Color(63,255,255);
        public static Color LIGHT_SKY = new Color(63,207,255);
        public static Color LIGHT_AZURE = new Color(63,159,255);
        public static Color LIGHT_BLUE = new Color(63,63,255);
        public static Color LIGHT_HAN = new Color(111,63,255);
        public static Color LIGHT_VIOLET = new Color(159,63,255);
        public static Color LIGHT_PURPLE = new Color(207,63,255);
        public static Color LIGHT_FUCHSIA = new Color(255,63,255);
        public static Color LIGHT_MAGENTA = new Color(255,63,207);
        public static Color LIGHT_PINK = new Color(255,63,159);
        public static Color LIGHT_CRIMSON = new Color(255,63,111);

        public static Color RED = new Color(255,0,0);
        public static Color FLAME = new Color(255,63,0);
        public static Color ORANGE = new Color(255,127,0);
        public static Color AMBER = new Color(255,191,0);
        public static Color YELLOW = new Color(255,255,0);
        public static Color LIME = new Color(191,255,0);
        public static Color CHARTREUSE = new Color(127,255,0);
        public static Color GREEN = new Color(0,255,0);
        public static Color SEA = new Color(0,255,127);
        public static Color TURQUOISE = new Color(0,255,191);
        public static Color CYAN = new Color(0,255,255);
        public static Color SKY = new Color(0,191,255);
        public static Color AZURE = new Color(0,127,255);
        public static Color BLUE = new Color(0,0,255);
        public static Color HAN = new Color(63,0,255);
        public static Color VIOLET = new Color(127,0,255);
        public static Color PURPLE = new Color(191,0,255);
        public static Color FUCHSIA = new Color(255,0,255);
        public static Color MAGENTA = new Color(255,0,191);
        public static Color PINK = new Color(255,0,127);
        public static Color CRIMSON = new Color(255,0,63);

        public static Color DARK_RED = new Color(191,0,0);
        public static Color DARK_FLAME = new Color(191,47,0);
        public static Color DARK_ORANGE = new Color(191,95,0);
        public static Color DARK_AMBER = new Color(191,143,0);
        public static Color DARK_YELLOW = new Color(191,191,0);
        public static Color DARK_LIME = new Color(143,191,0);
        public static Color DARK_CHARTREUSE = new Color(95,191,0);
        public static Color DARK_GREEN = new Color(0,191,0);
        public static Color DARK_SEA = new Color(0,191,95);
        public static Color DARK_TURQUOISE= new Color( 0,191,143);
        public static Color DARK_CYAN = new Color(0,191,191);
        public static Color DARK_SKY = new Color(0,143,191);
        public static Color DARK_AZURE = new Color(0,95,191);
        public static Color DARK_BLUE = new Color(0,0,191);
        public static Color DARK_HAN = new Color(47,0,191);
        public static Color DARK_VIOLET = new Color(95,0,191);
        public static Color DARK_PURPLE = new Color(143,0,191);
        public static Color DARK_FUCHSIA = new Color(191,0,191);
        public static Color DARK_MAGENTA = new Color(191,0,143);
        public static Color DARK_PINK = new Color(191,0,95);
        public static Color DARK_CRIMSON = new Color(191,0,47);

        public static Color DARKER_RED = new Color(127,0,0);
        public static Color DARKER_FLAME = new Color(127,31,0);
        public static Color DARKER_ORANGE = new Color(127,63,0);
        public static Color DARKER_AMBER = new Color(127,95,0);
        public static Color DARKER_YELLOW = new Color(127,127,0);
        public static Color DARKER_LIME = new Color(95,127,0);
        public static Color DARKER_CHARTREUSE = new Color(63,127,0);
        public static Color DARKER_GREEN = new Color(0,127,0);
        public static Color DARKER_SEA = new Color(0,127,63);
        public static Color DARKER_TURQUOISE = new Color(0,127,95);
        public static Color DARKER_CYAN = new Color(0,127,127);
        public static Color DARKER_SKY = new Color(0,95,127);
        public static Color DARKER_AZURE = new Color(0,63,127);
        public static Color DARKER_BLUE = new Color(0,0,127);
        public static Color DARKER_HAN = new Color(31,0,127);
        public static Color DARKER_VIOLET = new Color(63,0,127);
        public static Color DARKER_PURPLE = new Color(95,0,127);
        public static Color DARKER_FUCHSIA = new Color(127,0,127);
        public static Color DARKER_MAGENTA = new Color(127,0,95);
        public static Color DARKER_PINK = new Color(127,0,63);
        public static Color DARKER_CRIMSON = new Color(127,0,31);

        public static Color DARKEST_RED = new Color(63,0,0);
        public static Color DARKEST_FLAME = new Color(63,15,0);
        public static Color DARKEST_ORANGE = new Color(63,31,0);
        public static Color DARKEST_AMBER = new Color(63,47,0);
        public static Color DARKEST_YELLOW = new Color(63,63,0);
        public static Color DARKEST_LIME = new Color(47,63,0);
        public static Color DARKEST_CHARTREUSE = new Color(31,63,0);
        public static Color DARKEST_GREEN = new Color(0,63,0);
        public static Color DARKEST_SEA = new Color(0,63,31);
        public static Color DARKEST_TURQUOISE = new Color(0,63,47);
        public static Color DARKEST_CYAN = new Color(0,63,63);
        public static Color DARKEST_SKY = new Color(0,47,63);
        public static Color DARKEST_AZURE = new Color(0,31,63);
        public static Color DARKEST_BLUE = new Color(0,0,63);
        public static Color DARKEST_HAN = new Color(15,0,63);
        public static Color DARKEST_VIOLET = new Color(31,0,63);
        public static Color DARKEST_PURPLE = new Color(47,0,63);
        public static Color DARKEST_FUCHSIA = new Color(63,0,63);
        public static Color DARKEST_MAGENTA = new Color(63,0,47);
        public static Color DARKEST_PINK = new Color(63,0,31);
        public static Color DARKEST_CRIMSON = new Color(63,0,15);

        public static Color BRASS = new Color(191,151,96);
        public static Color COPPER = new Color(197,136,124);
        public static Color GOLD = new Color(229,191,0);
        public static Color SILVER = new Color(203,203,203);

        public static Color CELADON = new Color(172,255,175);
        public static Color PEACH = new Color(255, 159, 127);
        #endregion

        #endregion
        #region Dispose
        private bool _alreadyDisposed;

        /// <summary>
        /// Default finalizer calls Dispose.
        /// </summary>
        ~Color()
        {
            Dispose(false);
        }

        /// <summary>
        /// Safely dispose this object and all of its contents.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override to add custom disposing code.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                color.Dispose();
            }
            _alreadyDisposed = true;
        }

        #endregion
    }
    #endregion


    #region Pigment Class
    /// <summary>
    /// Stores forground color, background color, and background flag in a convenient
    /// single immutable data type
    /// </summary>
    public class Pigment : IDisposable
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Pigment given foreground and background colors and background flag
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="bgFlag"></param>
        public Pigment(Color foreground, Color background, TCODBackgroundFlag bgFlag)
        {
            fgColor = foreground;
            bgColor = background;
            this.bgFlag = bgFlag;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// BGFlag defaults to TCODBackgroundFlag.Set
        /// </summary>
        public Pigment(Color foreground, Color background)
            : this(foreground, background, TCODBackgroundFlag.Set)
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Pigment given foreground and background colors and background flag.
        /// </summary>
        public Pigment(long foreground, long background, TCODBackgroundFlag bgFlag)
            : this(new Color(foreground), new Color(background), bgFlag)
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// BGFlag defaults to TCODBackgroundFlag.Set
        /// </summary>
        public Pigment(long foreground, long background)
            : this(foreground, background, TCODBackgroundFlag.Set)
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the foreground color
        /// </summary>
        public Color Foreground
        {
            get { return fgColor; }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the background color
        /// </summary>
        public Color Background
        {
            get { return bgColor; }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the background flag;
        /// </summary>
        public TCODBackgroundFlag BackgroundFlag
        {
            get { return bgFlag; }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swaps a Pigments's foreground and background.  Returns a new Pigment instance,
        /// this instance is unchanged.
        /// </summary>
        /// <returns></returns>
        public Pigment Invert()
        {
            return new Pigment(Background, Foreground);
        }

        /// <summary>
        /// Returns a new Pigment by replacing the foreground color.  This isntance remains
        /// unchanged.
        /// </summary>
        /// <param name="newFGColor"></param>
        /// <returns></returns>
        public Pigment ReplaceForeground(Color newFGColor)
        {
            return new Pigment(newFGColor, Background);
        }

        /// <summary>
        /// Returns a new Pigment by replacing the background color.  This isntance remains
        /// unchanged.
        /// </summary>
        /// <param name="newBGColor"></param>
        /// <returns></returns>
        public Pigment ReplaceBackground(Color newBGColor)
        {
            return new Pigment(Foreground, newBGColor);
        }

        /// <summary>
        /// Returns a new Pigment by replacing the background flag.  This isntance remains
        /// unchanged.
        /// </summary>
        /// <param name="newBGFlag"></param>
        /// <returns></returns>
        public Pigment ReplaceBGFlag(TCODBackgroundFlag newBGFlag)
        {
            return new Pigment(Foreground, Background, newBGFlag);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the embedded string code for this color.
        /// <note>Embedded colors are currently not working correctly</note>
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            string str = string.Format("{0}{1}",
                Foreground.DoForegroundCode(),
                Background.DoBackgroundCode());

            return str;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Foreground.ToString(), Background.ToString());
        }
        #endregion
        #region Private Fields
        // /////////////////////////////////////////////////////////////////////////////////
        private readonly Color fgColor;
        private readonly Color bgColor;
        private readonly TCODBackgroundFlag bgFlag;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Dispose
        private bool _alreadyDisposed;

        /// <summary>
        /// Default finalizer calls Dispose.
        /// </summary>
        ~Pigment()
        {
            Dispose(false);
        }

        /// <summary>
        /// Safely dispose this object and its contents.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override to add custom disposing code.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                bgColor.Dispose();
                fgColor.Dispose();
            }
            _alreadyDisposed = true;
        }
        #endregion

    }
    #endregion
}
