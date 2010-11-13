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

namespace OchreGui.Utility
{
    /// <summary>
    /// Immuatable data type representing a rectangle.
    /// <remarks>that the terms Upper, Left, Right, Bottom and Top are meaningful only when
    /// Size.Width and Size.Height are both positive</remarks>
    /// </summary>
    [Serializable]
	public struct Rect
	{
		readonly Point upperLeft;
		readonly Size size;
		
		#region Constructors
		public Rect(Point upperLeft, Size size)
		{
			this.upperLeft = upperLeft;
			this.size = size;
		}
		
		public Rect (Point upperLeft, Point lowerRight)
		{
			this.upperLeft = upperLeft;
			this.size = new Size(lowerRight.X - upperLeft.X + 1,
			    lowerRight.Y - upperLeft.Y + 1);

		}

        public Rect(int x1, int y1, int x2, int y2)
        :this(new Point(x1,y1),new Point(x2,y2))
        {
        }
		#endregion
		
		#region Properties
		public Size Size
		{
			get {return size;}
		}
		
		public Point UpperLeft
		{
			get { return upperLeft; }
		}

        public int Top
        {
            get { return upperLeft.Y; }
        }

        public int Left
        {
            get { return upperLeft.X; }
        }

        public int Bottom
        {
            get { return upperLeft.Y + size.Height - 1; }
        }

        public int Right
        {
            get { return upperLeft.X + size.Width - 1; }
        }
		
		public Point LowerRight
		{
			get {
                return new Point(Right, Bottom);
			}
		}

        public Point UpperRight
        {
            get { return new Point(Right,Top); }
        }

        public Point LowerLeft
        {
            get { return new Point(Left,Bottom); }
        }

        public Point Center
        {
            get
            {
                int hCtr = (Left + Right) / 2;
                int vCtr = (Bottom + Top) / 2;

                return new Point(hCtr, vCtr);
            }
        }

        public Point TopCenter
        {
            get
            {
                return new Point(Center.X, Top);
            }
        }

        public Point RightCenter
        {
            get
            {
                return new Point(Right, Center.Y);
            }
        }

        public Point BottomCenter
        {
            get
            {
                return new Point(Center.X, Bottom);
            }
        }

        public Point LeftCenter
        {
            get
            {
                return new Point(Left, Center.Y);
            }
        }

		#endregion
		
		#region Public Methods
		
		public bool Contains (Point point)
		{
			if ((point.X >= upperLeft.X) && (point.Y >= upperLeft.Y) && 
				(point.X <= LowerRight.X) && (point.Y <= LowerRight.Y))
				return true;
			
			return false;
		}
		#endregion
        
        #region Static Methods
        public static Rect MoveBy(Rect rect, Size delta)
        {
            Point newUpperleft = new Point(rect.upperLeft.X + delta.Width,
                rect.upperLeft.Y+delta.Height);

            return new Rect(newUpperleft,rect.size);
        }

        public static Rect MoveTo(Rect rect, Point upperLeft)
        {
            return new Rect(upperLeft, rect.size);
        }

        /// <summary>
        /// Adds dx to left and right, and dy to top and bottom
        /// New width += dx*2, new height = dy*2
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static Rect Inflate(Rect source, int dx, int dy)
        {
            Rect ret;
            Point newUpperLeft;
            Size newSize;

            newUpperLeft = new Point(source.upperLeft.X - dx,
                source.upperLeft.Y - dy);

            newSize = new Size(source.size.Width + dx*2, source.size.Height + dy*2);

            ret = new Rect(newUpperLeft,newSize);

            return ret;
        }
        #endregion

        #region Overrides
        public override bool Equals (object obj)
		{
			if(obj == null)
				return false;
			
			if(this.GetType() != obj.GetType())
				return false;
			
			return Equals((Rect)obj);
		}
		
		public bool Equals(Rect rect)
		{
			return (this.upperLeft == rect.upperLeft && this.size == rect.size);	
		}

        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }
		
		public override int GetHashCode ()
		{
            int hash = 7;
            hash = hash*13 + upperLeft.GetHashCode();
			hash = hash*13 + size.GetHashCode ();
			
			return hash;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0} : {1}", upperLeft, size);
		}
		#endregion
		

		

	}
}

