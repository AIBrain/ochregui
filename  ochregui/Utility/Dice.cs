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
    /// An immutable type representing a set of rpg dice along with a modifier.  Use this with
    /// Rand.Int32(Dice) to produce randomly generated numbers.
    /// </summary>
    [Serializable]
	public struct Dice
	{
		readonly int ndice;
		readonly int nsides;
		readonly int mod;
		
		#region Constructors
        /// <summary>
        /// Construct a Dice instanced given the number of dice and number of sides for each die.
        /// </summary>
        /// <example>
        /// To create a Dice object representing two six-sided dice:
        /// <code>
        /// Dice d = new Dice(2,6);
        /// </code>
        /// </example>
        /// <param name="numberOfDice"></param>
        /// <param name="numberOfSides"></param>
		public Dice (int numberOfDice, int numberOfSides)
		{
			ndice = numberOfDice;
			nsides = numberOfSides;
			mod = 0;
		}

        /// <summary>
        /// Construct a Dice instance given the number of dice, number sides per die, and a
        /// positive or negative modifier.
        /// </summary>
        /// <example>To construct a Dice object representing one six-sided die that gets a +2 modifier
        /// (which would return a random range of 3 thru 8):
        /// <code>Dice d = new Dice(1,6,2);</code></example>
        /// <param name="numberOfDice"></param>
        /// <param name="numberOfSides"></param>
        /// <param name="Modifier"></param>
        public Dice(int numberOfDice, int numberOfSides, int Modifier)
        {
            ndice = numberOfDice;
            nsides = numberOfSides;
            mod = Modifier;
        }

        /// <summary>
        /// Coonstructs a Dice object from a string that follows common RPG conventions 
        /// when specifying dice.
        /// </summary>
        /// <example>To create a dice object representing two six sided dice with a -2 modifier:
        /// <code>Dice d = new Dice("2d6-2");</code></example>
        /// <param name="formatString">
        /// In the form of {n}["d"|"D"][s]{"+"|"-"m}, where n=number of dice (optional), s = number of sides,
        /// and m=modifier (optional)
        /// </param>
		public Dice (string formatString)
		{
            string ndiceString="";
            string nsidesString="";
            string modString="";

			ndice = 0;
			nsides = 0;
			mod = 0;
			
			string[] tokens = formatString.Split (new char[] { 'd', 'D' });
            ndiceString = tokens[0];

            if(tokens.Length > 1)
            {
                tokens = tokens[1].Split(new char[]{'+','-'});

                nsidesString = tokens[0];
                
                if (tokens.Length > 1)
                {
                    modString = tokens[1];
                }
            }

            int ns;
            if (int.TryParse(nsidesString, out ns))
            {
                ndice = 1;
                nsides = ns;
            }

			int nd;
			if (int.TryParse (ndiceString, out nd))
			{
				ndice = nd;
			}

            int m;
            if (int.TryParse(modString, out m))
            {
                mod = m;
                if(formatString.Contains("-"))
                    mod = -mod;
            }
		}
		#endregion
		
		#region Properties
        /// <summary>
        /// The number of dice this Dice object represents
        /// </summary>
		public int NumberOfDice
		{
			get { return ndice; }
		}
		
        /// <summary>
        /// The number of sides per each die.
        /// </summary>
		public int NumberOfSides
		{
			get { return nsides; }
		}
		
        /// <summary>
        /// The linear modifier of the random range obtained with this Dice object.
        /// </summary>
		public int Modifier
		{
			get { return mod; }
		}
		#endregion
		
		#region Overrides
		public override bool Equals (object obj)
		{
            //if (obj == null)
            //    return false;
			
			if (this.GetType () != obj.GetType ())
				return false;
			
			return Equals ((Dice)obj);
		}

		public bool Equals (Dice d)
		{
			return (this.ndice == d.ndice && this.nsides == d.nsides);
		}
		
		public override int GetHashCode ()
		{
            int hash = 7;
            hash = hash * 13 + nsides.GetHashCode();
			hash = hash * 13 + ndice.GetHashCode ();
            hash = hash * 13 + mod.GetHashCode();
			
			return hash;
		}

		public override string ToString ()
		{
			System.Text.StringBuilder ret = new System.Text.StringBuilder (
				string.Format ("{0}d{1}", ndice, nsides));
			
			if (mod != 0)
			{
				if (mod > 0)
				{
					ret.AppendFormat ("+{0}", mod);
				}
				else
				{
					ret.AppendFormat ("-{0}", mod);
				}
			}
			
			return ret.ToString ();
			
		}

		public static bool operator == (Dice left, Dice right)
		{
			if (ReferenceEquals (left, null)) {
				return ReferenceEquals (right, null);
			}
			
			return (left.Equals (right));
		}

		public static bool operator != (Dice left, Dice right)
		{
			return !(left == right);
		}
		#endregion
		
	}
	
	
}
