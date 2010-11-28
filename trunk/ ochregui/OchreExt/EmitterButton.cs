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
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Extended
{
    /// <summary>
    /// This object builds on ButtonTemplate, providing Speed and StartEmittingDelay
    /// properties to control the emit cycle timing.
    /// </summary>
    public class EmitterButtonTemplate : ButtonTemplate
    {
        /// <summary>
        /// Construct an EmitterButtonTemplate using the defaults.
        /// </summary>
        public EmitterButtonTemplate()
        {
        }

        /// <summary>
        /// The speed of the emit event cycle, in millisecond delay between each event.
        /// Defaults to 0.
        /// </summary>
        public uint Speed { get; set; }

        /// <summary>
        /// The number of milliseconds of delay between when the button is initially pushed
        /// to when it starts the emit cycle.  Defaults to 0.
        /// </summary>
        public uint StartEmittingDelay { get; set; }
    }

    /// <summary>
    /// When being pushed (by the left mouse button), an EmitterButton will raise the Emit event
    /// continuously at a specified rate.  This button is used by the Spin control to simplify the timing
    /// of the spin cycle.
    /// </summary>
    public class EmitterButton : Button
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised repeatedly at a rate controlled by the Speed property when this button
        /// is being pushed.
        /// </summary>
        public event EventHandler Emit;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct an EmitterButton from the specified template.
        /// </summary>
        /// <param name="template"></param>
        public EmitterButton(EmitterButtonTemplate template)
            : base(template)
        {
            this.Speed = template.Speed;
            this.StartEmittingDelay = template.StartEmittingDelay;

            CountdownSchedule = new Schedule(DoCountDownEnded, StartEmittingDelay);
            EmitSchedule = new Schedule(DoEmit, Speed);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The speed of the emit event cycle, in millisecond delay between each event.
        /// </summary>
        protected uint Speed { get; private set; }

        /// <summary>
        /// The number of milliseconds of delay between when the button is initially pushed
        /// to when it starts the emit cycle.
        /// </summary>
        protected uint StartEmittingDelay { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the framework when a mouse button is pressed while over this button.
        /// Base method creates and starts the schedules for the emit cycle.
        /// </summary>
        /// <param name="mouseData"></param>
        protected override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                DoStartEmitting();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the framework when a mouse button is released while over this control.
        /// Base method removes the schedules for the emit cycle.
        /// </summary>
        /// <param name="mouseData"></param>
        protected override void OnMouseButtonUp(MouseData mouseData)
        {
            base.OnMouseButtonUp(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                RemoveSchedule(EmitSchedule);
                RemoveSchedule(CountdownSchedule);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the framework when the mouse pointer has left the region of this control.
        /// Base method removes the schedules for the emit cycle.
        /// </summary>
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();

            RemoveSchedule(EmitSchedule);
            RemoveSchedule(CountdownSchedule);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the scheduler when this control is to fire the Emit event.  Base method
        /// raises the Emit event.
        /// </summary>
        protected virtual void OnEmitEvent()
        {
            if (Emit != null)
            {
                Emit(this, EventArgs.Empty);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private Schedule EmitSchedule;
        private Schedule CountdownSchedule;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoStartEmitting()
        {
            OnEmitEvent();

            AddSchedule(CountdownSchedule);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoCountDownEnded()
        {
            RemoveSchedule(CountdownSchedule);
            AddSchedule(EmitSchedule);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoEmit()
        {
            OnEmitEvent();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
