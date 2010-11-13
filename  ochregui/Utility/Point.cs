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
    /// Immutable pair of int values, representing an x,y coordinate.
    /// </summary>
    [Serializable]
	public struct Point
	{
		readonly int x,y;

        public static Point Origin = new Point(0, 0);

		#region Constructors
		public Point(int x,int y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion
		
		#region Properties
		public int X
		{
			get {return x;}
		}
		
		public int Y
		{
			get { return y; }
		}
		#endregion
        /// <summary>
        /// Returns a new Point instance by adding dx to this.X and dy to this.Y.  This method
        /// does not modify this Point instance.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public Point Shift(int dx, int dy)
        {
            return new Point(x + dx, y + dy);
        }
		
		#region Overrides
		public override bool Equals (object obj)
		{
			if(obj == null)
				return false;
			
			if(this.GetType() != obj.GetType())
				return false;
			
			return Equals((Point)obj);
		}
		
		public bool Equals(Point point)
		{
			return (this.x == point.x && this.y == point.y);
		}
		
		public override int GetHashCode ()
		{
            int hash = 7;
            hash = hash*13 + x.GetHashCode();
			hash = hash*13 + y.GetHashCode();
			
			return hash;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0},{1}", x, y);
		}
		
		public static bool operator ==(
				Point left,Point right)
		{
			return(left.Equals(right));
		}
		
		public static bool operator != (
				Point left, Point right)
		{
			return !(left == right);
		}
		#endregion
		
	}
}

