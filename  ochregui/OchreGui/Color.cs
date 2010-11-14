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
        #endregion
        #region Dispose
        private bool _alreadyDisposed;

        ~Color()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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


    #region ColorStyle Class
    /// <summary>
    /// Stores forground color, background color, and background flag in a convenient
    /// single immutable data type
    /// </summary>
    public class ColorStyle : IDisposable
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a ColorStyle given foreground and background colors and background flag
        /// </summary>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="bgFlag"></param>
        public ColorStyle(Color foreground, Color background, TCODBackgroundFlag bgFlag)
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
        public ColorStyle(Color foreground, Color background)
            : this(foreground, background, TCODBackgroundFlag.Set)
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a ColorStyle given foreground and background colors and background flag.
        /// </summary>
        public ColorStyle(long foreground, long background, TCODBackgroundFlag bgFlag)
            : this(new Color(foreground), new Color(background), bgFlag)
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// BGFlag defaults to TCODBackgroundFlag.Set
        /// </summary>
        public ColorStyle(long foreground, long background)
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
        /// Swaps a ColorStyles's foreground and background.  Returns a new ColorStyle instance,
        /// this instance is unchanged.
        /// </summary>
        /// <returns></returns>
        public ColorStyle Invert()
        {
            return new ColorStyle(Background, Foreground);
        }

        public ColorStyle ReplaceForeground(Color newFGColor)
        {
            return new ColorStyle(newFGColor, Background);
        }

        public ColorStyle ReplaceBackground(Color newBGColor)
        {
            return new ColorStyle(Foreground, newBGColor);
        }

        public ColorStyle ReplaceBGFlag(TCODBackgroundFlag newBGFlag)
        {
            return new ColorStyle(Foreground, Background, newBGFlag);
        }
        // /////////////////////////////////////////////////////////////////////////////////
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

        ~ColorStyle()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
