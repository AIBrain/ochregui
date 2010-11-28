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
    public enum PigmentType
    {
        Window,
        Tooltip,
        DragItem,
        FrameHasFocus,
        FrameInactive,
        FrameMouseOver,
        FrameNormal,
        FramePushing,
        FrameSelected,
        ViewHasFocus,
        ViewInactive,
        ViewMouseOver,
        ViewNormal,
        ViewPushing,
        ViewSelected

    };

    public class DefaultPigments : StaticDictionary<PigmentType, Pigment>
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

                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameHasFocus,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameNormal,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameInactive,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameMouseOver,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FramePushing,new Pigment(0x6D3D00, 0x3E1D00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.FrameSelected,new Pigment(0x6D3D00, 0x3E1D00)),

                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewHasFocus,new Pigment(0xFFFFAA, 0x723E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewNormal,new Pigment(0xDDDDDD, 0x622E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewInactive,new Pigment(0x7C7C7C, 0x2E2E2E)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewMouseOver,new Pigment(0xFFFFAA, 0x723E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewPushing,new Pigment(0x6B6B6B, 0x431E00)),
                new KeyValuePair<PigmentType,Pigment>(PigmentType.ViewSelected,new Pigment(0x0098F4, 0x622E00)),


            });
        }
    }

    public class PigmentAlternatives : AlternativeMap<PigmentType, Pigment>
    {
        public PigmentAlternatives(DefaultPigments defaults)
            : base(defaults)
        {
        }

        public PigmentAlternatives(DefaultPigments defaults,
            Dictionary<PigmentType, Pigment> alternatives)
            : base(defaults, alternatives)
        {
        }
    }

    ///// <summary>
    ///// Stores all of the default pigments used by the framework.
    ///// </summary>
    //public class PigmentList : IDisposable
    //{
    //    /// <summary>
    //    /// The default pigments for the frame element.
    //    /// </summary>
    //    public PigmentList FramePigments { get; set; }

    //    /// <summary>
    //    /// The default pigments for the main view element.
    //    /// </summary>
    //    public PigmentList MainPigments { get; set; }

    //    /// <summary>
    //    /// The default pigment for the main area of the window
    //    /// </summary>
    //    public Pigment WindowView { get; set; }

    //    /// <summary>
    //    /// The default pigment for an item that is being dragged
    //    /// </summary>
    //    public Pigment DragItem { get; set; }

    //    /// <summary>
    //    /// The default pigment for a tooltip.
    //    /// </summary>
    //    public Pigment Tooltip { get; set; }

    //    /// <summary>
    //    /// Returns a copy of this instance.
    //    /// </summary>
    //    /// <returns></returns>
    //    public PigmentList Copy()
    //    {
    //        PigmentList ret = new PigmentList();
    //        ret.DragItem = this.DragItem;
    //        ret.FramePigments = this.FramePigments.Copy();
    //        ret.MainPigments = this.MainPigments.Copy();
    //        ret.Tooltip = this.Tooltip;
    //        ret.WindowView = this.WindowView;

    //        return ret;
            
    //    }
        
    //    /// <summary>
    //    /// Returns the framework default pigments.
    //    /// </summary>
    //    public static PigmentList FrameworkDefaults
    //    {
    //        get
    //        {
    //            return frameworkDefaults.Copy();
    //        }
    //    }

    //    readonly static PigmentList frameworkDefaults;

    //    static PigmentList()
    //    {
    //        frameworkDefaults = new PigmentList();

    //        frameworkDefaults.WindowView = new Pigment(0xDDDDDD, 0x000000);
    //        frameworkDefaults.DragItem = new Pigment(0xD6AC8B, 0xF45B00);
    //        frameworkDefaults.Tooltip = new Pigment(0x2B2B8F, 0xCCEEFF);

    //        frameworkDefaults.FramePigments = new PigmentList()
    //        {
    //            HasFocus = new Pigment(0x6D3D00, 0x3E1D00),
    //            Normal = new Pigment(0x6D3D00, 0x3E1D00),
    //            Inactive = new Pigment(0x6D3D00, 0x3E1D00),
    //            MouseOver = new Pigment(0x6D3D00, 0x3E1D00),
    //            Pushing = new Pigment(0x6D3D00, 0x3E1D00),
    //            Selected = new Pigment(0x6D3D00, 0x3E1D00)
    //        };

    //        frameworkDefaults.MainPigments = new PigmentList()
    //        {
    //            HasFocus = new Pigment(0xFFFFAA, 0x723E00),
    //            Normal = new Pigment(0xDDDDDD, 0x622E00),
    //            Inactive = new Pigment(0x7C7C7C, 0x2E2E2E),
    //            MouseOver = new Pigment(0xFFFFAA, 0x723E00),
    //            Pushing = new Pigment(0x6B6B6B, 0x431E00),
    //            Selected = new Pigment(0x0098F4, 0x622E00)
    //        };
    //    }

    //    #region Dispose
    //    private bool _alreadyDisposed;

    //    /// <summary>
    //    /// Default finalizer calls Dispose.
    //    /// </summary>
    //    ~PigmentList()
    //    {
    //        Dispose(false);
    //    }

    //    /// <summary>
    //    /// Safely dispose this object and all of its contents.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    /// <summary>
    //    /// Override to add custom disposing code.
    //    /// </summary>
    //    /// <param name="isDisposing"></param>
    //    protected virtual void Dispose(bool isDisposing)
    //    {
    //        if (_alreadyDisposed)
    //            return;
    //        if (isDisposing)
    //        {
    //            WindowView.Dispose();
    //            Tooltip.Dispose();
    //            DragItem.Dispose();
    //            MainPigments.Dispose();
    //            FramePigments.Dispose();
    //        }
    //        _alreadyDisposed = true;
    //    }
    //    #endregion
    //}


    ///// <summary>
    ///// This class stores a control's pigments according to state.
    ///// </summary>
    //public class PigmentList : IDisposable
    //{
    //    /// <summary>
    //    /// Pigment for an inactive state.
    //    /// </summary>
    //    public Pigment Inactive { get; set; }

    //    /// <summary>
    //    /// Pigment for a normal state (when none of the other states apply)
    //    /// </summary>
    //    public Pigment Normal { get; set; }

    //    /// <summary>
    //    /// Pigment for a mouse over state
    //    /// </summary>
    //    public Pigment MouseOver { get; set; }

    //    /// <summary>
    //    /// Pigment for a pushing state
    //    /// </summary>
    //    public Pigment Pushing { get; set; }

    //    /// <summary>
    //    /// Pigment for a has focus state
    //    /// </summary>
    //    public Pigment HasFocus { get; set; }

    //    /// <summary>
    //    /// Pigment for a selected state
    //    /// </summary>
    //    public Pigment Selected { get; set; }

    //    /// <summary>
    //    /// Copies this PigmentList instance.
    //    /// </summary>
    //    /// <returns></returns>
    //    public PigmentList Copy()
    //    {
    //        PigmentList ret = new PigmentList();

    //        ret.Inactive = this.Inactive;
    //        ret.Normal = this.Normal;
    //        ret.MouseOver = this.MouseOver;
    //        ret.Pushing = this.Pushing;
    //        ret.HasFocus = this.HasFocus;
    //        ret.Selected = this.Selected;

    //        return ret;
    //    }

    //    #region Dispose
    //    private bool _alreadyDisposed;

    //    /// <summary>
    //    /// Default finalizer calls Dispose.
    //    /// </summary>
    //    ~PigmentList()
    //    {
    //        Dispose(false);
    //    }

    //    /// <summary>
    //    /// Safely dispose this object and all of its contents.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    /// <summary>
    //    /// Override to add custom disposing code.
    //    /// </summary>
    //    /// <param name="isDisposing"></param>
    //    protected virtual void Dispose(bool isDisposing)
    //    {
    //        if (_alreadyDisposed)
    //            return;
    //        if (isDisposing)
    //        {
    //            Inactive.Dispose();
    //            Normal.Dispose();
    //            MouseOver.Dispose();
    //            Pushing.Dispose();
    //            HasFocus.Dispose();
    //            Selected.Dispose();
    //        }
    //        _alreadyDisposed = true;
    //    }
    //    #endregion
    //}
}

