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
using OchreGui.Utility;

namespace OchreGui
{
    /// <summary>
    /// The various UI Pigment types used by the framework.
    /// </summary>
    public enum PigmentType
    {
        /// <summary>
        /// The pigment used for the window drawing area
        /// </summary>
        Window,
        /// <summary>
        /// The pigment used for a tooltip
        /// </summary>
        Tooltip,
        /// <summary>
        /// The pigment used for an item that is being dragged
        /// </summary>
        DragItem,
        /// <summary>
        /// The pigment for the frame of a control that has the keyboard focus
        /// </summary>
        FrameFocus,
        /// <summary>
        /// The pigment used for the frame of a control that is inactive
        /// </summary>
        FrameInactive,
        /// <summary>
        /// The pigment used for the frame of a control that is hilighted (e.g. when
        /// hilighted by mouse over state)
        /// </summary>
        FrameHilight,
        /// <summary>
        /// The pigment used for the frame of a control when no other states are applicable.
        /// </summary>
        FrameNormal,
        /// <summary>
        /// The pigment used for the frame of a control when it is depressed (e.g. when
        /// a button is being pusehd)
        /// </summary>
        FrameDepressed,
        /// <summary>
        /// The pigment used for the frame of a control when it is selected.
        /// </summary>
        FrameSelected,

        /// <summary>
        /// The pigment used for the main area of a control that is inactive
        /// </summary>
        ViewFocus,
        /// <summary>
        /// The pigment used for the main area of a control that is inactive
        /// </summary>
        ViewInactive,
        /// <summary>
        /// The pigment used for the main area of a control that is hilighted (e.g. when
        /// hilighted by mouse over state)
        /// </summary>
        ViewHilight,
        /// <summary>
        /// The pigment used for the main area of a control that is hilighted (e.g. when
        /// hilighted by mouse over state)
        /// </summary>
        ViewNormal,
        /// <summary>
        /// The pigment used for the main area of a control when it is depressed (e.g. when
        /// a button is being pusehd)
        /// </summary>
        ViewDepressed,
        /// <summary>
        /// The pigment used for the main area of a control when it is selected.
        /// </summary>
        ViewSelected

    };

    /// <summary>
    /// This class is simply a wrapper around a Dictionary using PigmentType as keys
    /// and Pigment as values.  Also provides a Copy() method to make a copy
    /// of this instance.
    /// This class is used to pass a collection of alternative pigments to a widget in the
    /// template.
    /// </summary>
    public class PigmentAlternatives : Dictionary<PigmentType, Pigment>
    {
        /// <summary>
        /// Performs a copy of this instance.  Since the Pigment class is
        /// immutable, this is conceptually a deep copy.
        /// </summary>
        /// <returns></returns>
        public PigmentAlternatives Copy()
        {
            PigmentAlternatives ret = new PigmentAlternatives();

            foreach (var itm in this)
            {
                ret.Add(itm.Key, itm.Value);
            }

            return ret;
        }
    }

    /// <summary>
    /// Simply wraps an AlternativeMap object for code cleanliness.
    /// This class is used to store the pigments for a UI object.
    /// </summary>
    public class PigmentMap : AlternativeMap<PigmentType, Pigment>
    {
        /// <summary>
        /// Construct a PigmentMap instance given the defaults.
        /// </summary>
        /// <param name="defaults"></param>
        public PigmentMap(IStaticDictionary<PigmentType, Pigment> defaults)
            : base(defaults)
        {
        }

        /// <summary>
        /// Construct a PigmentMap instance given the defaults and a
        /// Dictionary of alternatives.
        /// </summary>
        /// <param name="defaults"></param>
        /// <param name="alternatives"></param>
        public PigmentMap(IStaticDictionary<PigmentType, Pigment> defaults,
            Dictionary<PigmentType, Pigment> alternatives)
            : base(defaults, alternatives)
        {
        }
    }
}
