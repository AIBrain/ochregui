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
    /// Immutable type representing anything that has a width and a height
    /// </summary>
    [Serializable]
	public struct Size
	{
		readonly int width,height;
		
		#region Constructors
		public Size(int width,int height)
		{
			this.width = width;
			this.height = height;
		}
		
		public Size(Size size)
		{
			this.width = size.width;
			this.height = size.height;
		}
		#endregion
		
		#region Properties
		public int Width
		{
			get {return width;}
		}
		
		public int Height
		{
			get { return height; }
		}

        public bool IsEmpty
        {
            get
            {
                if (Width == 0 && Height == 0)
                    return true;
                else
                    return false;
            }
        }
		#endregion
		
		#region Overrides
		public override bool Equals (object obj)
		{
			if(obj == null)
				return false;
			
			if(this.GetType() != obj.GetType())
				return false;
			
			return Equals((Size)obj);
		}
		
		public bool Equals(Size size)
		{
			return (this.width == size.width && this.height == size.height);
		}
		
		public override int GetHashCode ()
		{
            int hash = 7;
            hash = hash*13 + width.GetHashCode();
			hash = hash*13 + height.GetHashCode ();
			
			return hash;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0},{1})", width, height);
		}
		
		public static bool operator ==(Size left, Size right)
		{
			return left.Equals(right);
		}
		
		public static bool operator != (Size left, Size right)
		{
			return !(left == right);
		}
		#endregion
	}
}

