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
    class DefaultPigments : StaticDictionary<PigmentType, Pigment>
    {
        public DefaultPigments(KeyValuePair<PigmentType, Pigment>[] items)
            :base(items)
        {
        }

        static DefaultPigments frameworkDefaults;

        public static DefaultPigments FrameworkDefaults
        {
            get
            {
                return frameworkDefaults;
            }
        }

        static DefaultPigments()
        {
            frameworkDefaults = new DefaultPigments(new KeyValuePair<PigmentType, Pigment>[]
            {
                new KeyValuePair<PigmentType,Pigment>(PigmentType.Window,new Pigment(0xDDDDDD, 0x000000)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.DragItem,new Pigment(0xD6AC8B, 0xF45B00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.Tooltip,new Pigment(0x2B2B8F, 0xCCEEFF)),

                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameFocus,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameNormal,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameInactive,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameHilight,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameDepressed,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameSelected,new Pigment(0x6D3D00, 0x3E1D00)),

                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewFocus,new Pigment(0xFFFFAA, 0x723E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewNormal,new Pigment(0xDDDDDD, 0x622E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewInactive,new Pigment(0x7C7C7C, 0x2E2E2E)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewHilight,new Pigment(0xFFFFAA, 0x723E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewDepressed,new Pigment(0x6B6B6B, 0x431E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewSelected,new Pigment(0x0098F4, 0x622E00)),


            });
        }
    }
}

