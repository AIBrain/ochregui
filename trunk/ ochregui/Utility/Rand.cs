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
using System.Collections.Generic;
using System.Text;

namespace OchreGui.Utility
{
    /// <summary>
    /// Utility (static) class with random number helper functions
    /// </summary>
    public static class Rand
    {
        // /////////////////////////////////////////////////////////////////////////////////
        static Random rnd;

        // /////////////////////////////////////////////////////////////////////////////////
        static Rand()
        {
            rnd = new Random();
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns 0 or 1
        /// </summary>
        /// <returns></returns>
        static public int Int32()
        {
            return rnd.Next(0, 2);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a random value from minimum to maximum, inclusive
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        static public int Int32(int minimum, int maximum)
        {
            return rnd.Next(minimum, maximum + 1);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a randomly generated integer based on the specified Dice object.
        /// </summary>
        /// <param name="dice"></param>
        /// <returns></returns>
        static public int Int32(Dice dice)
        {
            if (dice.NumberOfDice < 1 || dice.NumberOfSides < 1)
            {
                return dice.Modifier;
            }

            int ret = 0;

            for (int i = 0; i < dice.NumberOfDice; i++)
            {
                ret += rnd.Next(1, dice.NumberOfSides + 1);
            }

            return ret + dice.Modifier;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Has an equal chance of return true or false.
        /// </summary>
        /// <returns></returns>
        static public bool Boolean()
        {
            return (Int32() == 0);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Random float between 0.0 and 1.0
        /// </summary>
        /// <returns></returns>
        static public float Float()
        {
            return (float)rnd.NextDouble();
        }
    }
}
