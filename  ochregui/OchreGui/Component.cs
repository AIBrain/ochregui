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
using libtcod;


namespace OchreGui
{
    /// <summary>
    /// This class holds a delelgate and an associated timing value for use with the AddSchedule
    /// method in the Component class.
    /// </summary>
    public class Schedule
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Scheduler with the specified callback method, and number of
        /// milliseconds between each callback.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="delayMS"></param>
        public Schedule(Component.ScheduleCallback callback, uint delayMS)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            Callback = callback;
            DelayMS = delayMS;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update the Schedule according to the ellapsed milliseconds since the last call to
        /// this method.  This should be called every tick to provide accurate timing.
        /// </summary>
        /// <param name="ellapsedMS"></param>
        public void Update(uint ellapsedMS)
        {
            count += ellapsedMS;
            if (count >= DelayMS)
            {
                count = 0;
                Callback();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        internal void Reset()
        {
            count = 0;
        }

        private Component.ScheduleCallback Callback;
        private uint DelayMS;
        private uint count;
        // /////////////////////////////////////////////////////////////////////////////////
    }

	/// <summary>
	/// Base class for any objects that need to receive system and action messages.
	/// </summary>
	public abstract class Component
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This event is raised once during the a component's setup.  Various descendents
        /// of Component raise this event at different times during the application lifetime:
        /// see the various OnSettingUp methods for details.
        /// </summary>
        public event EventHandler SettingUp;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised every update tick of the application loop.
        /// </summary>
        public event EventHandler Tick;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when application is about to quit.
        /// </summary>
		public event EventHandler Quitting;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when a key has been pressed.  Controls will only raise
        /// this event if they have the current keyboard focus and are active.
        /// </summary>
		public event EventHandler<KeyboardEventArgs> KeyPressed;
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Raised when a key has been released.  Controls will only raise
        /// this event if they have the current keyboard focus and are active.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyReleased;
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the mouse has moved to a different position.  Active controls 
        /// will only raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.  The framework
        /// checks the pixel position to see if movement has occurred.
        /// </summary>
		public event EventHandler<MouseEventArgs> MouseMoved;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when a mouse button has been released.. Controls will only
        /// raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.
        /// </summary>
		public event EventHandler<MouseEventArgs> MouseButtonUp;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when a mouse button has been pressed.  Controls will only
        /// raise this event if the mouse is currently over the control and the
        /// control is topmost at that position.
        /// </summary>
		public event EventHandler<MouseEventArgs> MouseButtonDown;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the mouse has started hovering (has stayed still 
        /// for a short time).  Controls will only
        /// raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.
        /// </summary>
		public event EventHandler<MouseEventArgs> MouseHoverBegin;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the mouse has stopped hovering (has moved while
        /// previously hovering).  Controls will only
        /// raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.
        /// </summary>
		public event EventHandler<MouseEventArgs> MouseHoverEnd;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the mouse drag has started (mouse has moved a 
        /// small distance with the left button held down).  Controls will only
        /// raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.
        /// </summary>
		public event EventHandler<MouseDragEventArgs> MouseDragBegin;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the mouse has stopped a mouse drag (e.g. left button
        /// released).  Controls will only
        /// raise this event if the mouse is currently over the control and the control
        /// is topmost at that position.
        /// </summary>
        public event EventHandler<MouseDragEventArgs> MouseDragEnd;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A callback method for the Schedule object must conform to this delegate.
        /// </summary>
        public delegate void ScheduleCallback();
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a schedule to this component.  All schedule must be reference-unique to this component,
        /// or this method will throw an ArgumentException.  The schedule is actually  added
        /// to the list on the next tick - this ensures that schedules can be added/removed during
        /// another schedule's callback method.
        /// </summary>
        /// <example>
        /// <code>
        /// AddSchedule(new Scheduel(MyCallback,100));
        /// </code>
        /// This will cause the MyCallback method to be called every 100 milliseconds until removed with
        /// RemoveSchedule.  Elsewhere the MyCallback method must be defined as follows:
        /// <code>
        /// void MyCallback()
        /// {
        ///     // insert callback code here
        /// }
        /// </code>
        /// </example>
        /// <param name="schedule"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="schedule"/> is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="schedule"/> is already
        /// contained by this component</exception>
        public void AddSchedule(Schedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException("schedule");
            }

            if (ContainsSchedule(schedule) || scheduleAddList.Contains(schedule))
            {
                throw new ArgumentException("Schedule instances must be unique to this component");
            }

            schedule.Reset();
            scheduleAddList.Add(schedule);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove the specified schedule from this component.  The schedule is actually  removed
        /// from the list on the next tick - this ensures that schedules can be added/removed during
        /// another schedule's callback method.
        /// </summary>
        /// <param name="schedule"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="schedule"/> is null.</exception>
        public void RemoveSchedule(Schedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException("schedule");
            }

            if (ContainsSchedule(schedule))
            {
                scheduleRemoveList.Add(schedule);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if the specified schedule is currently contained in this component.
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public bool ContainsSchedule(Schedule schedule)
        {
            return scheduleList.Contains(schedule);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Holds the mouse position (in screen space) from the last MouseMove message this
        /// component has received.
        /// </summary>
        protected Point CurrentMousePos { get; set; }

        /// <summary>
        /// Total ellapsed time since start of application, in milliseconds.
        /// </summary>
        protected uint TotalEllapsed { get; private set; }

        /// <summary>
        /// Ellapsed time in milliseconds since the last tick message this component
        /// has received.
        /// </summary>
        protected uint LastTickEllapsed { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called during a compoment's setup, and is called only once.  See specific children
        /// for when this occurs.
        /// Override to add specific setup code.
        /// </summary>
        internal protected virtual void OnSettingUp()
        {
            if (isSetup)
                return;

            if (SettingUp != null)
                SettingUp(this, EventArgs.Empty);

            isSetup = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called each application loop iteration.  Override to add specific update code.
        /// </summary>
        internal protected virtual void OnTick()
        {
            uint milli = TCODSystem.getElapsedMilli();

            LastTickEllapsed = milli - TotalEllapsed;
            TotalEllapsed = milli;

            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
            }

            if (scheduleList.Count > 0)
            {
                foreach (Schedule s in scheduleList)
                {
                    s.Update(LastTickEllapsed);
                }
            }

            if (scheduleRemoveList.Count > 0)
            {
                foreach (Schedule s in scheduleRemoveList)
                {
                    scheduleList.Remove(s);
                }
                scheduleRemoveList.Clear();
            }

            if (scheduleAddList.Count > 0)
            {
                foreach (Schedule s in scheduleAddList)
                {
                    scheduleList.Add(s);
                }
                scheduleAddList.Clear();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called exactly once when the application is about to exit.  Override to add
        /// specific de-initialation code.  <Note>Release of unmanaged resource and
        /// typical finalizer code should go in the typical Dispose() or finalizer
        /// methods of the derivative class.</Note>
        /// </summary>
        internal protected virtual void OnQuitting()
		{
			
			if (Quitting != null)
			{
				Quitting(this, EventArgs.Empty);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the user has pressed a keyboard key.  Override to add specific keypress
        /// handling code.
        /// </summary>
        internal protected virtual void OnKeyPressed(KeyboardData keyData)
		{
			if (KeyPressed != null)
			{
				KeyPressed(this, new KeyboardEventArgs(keyData));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the user has released a previously pressed key.  Override to add
        /// key release handling code.
        /// </summary>
        /// <param name="keyData"></param>
        internal protected virtual void OnKeyReleased(KeyboardData keyData)
        {
            if (KeyReleased != null)
            {
                KeyReleased(this, new KeyboardEventArgs(keyData));
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the mouse has moved to a new cell position.  Override to add
        /// mouse move handling code.
        /// </summary>
        internal protected virtual void OnMouseMoved(MouseData mouseData)
		{
            CurrentMousePos = mouseData.Position;
			
			if (MouseMoved != null)
			{
				MouseMoved(this, new MouseEventArgs(mouseData));
			}
			
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when a mouse button has been pressed.  Override to add mouse button
        /// handling code.
        /// </summary>
        internal protected virtual void OnMouseButtonDown(MouseData mouseData)
		{
			if (MouseButtonDown != null)
			{
                MouseButtonDown(this, new MouseEventArgs(mouseData));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when a previously pressed mouse button has been released.  Override to
        /// add mouse button handling code.
        /// </summary>
        internal protected virtual void OnMouseButtonUp(MouseData mouseData)
		{
			if (MouseButtonUp != null)
			{
                MouseButtonUp(this, new MouseEventArgs(mouseData));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the mouse has entered a hover state.  Override to add mouse hover
        /// handling code.
        /// </summary>
        internal protected virtual void OnMouseHoverBegin(MouseData mouseData)
		{
			if (MouseHoverBegin != null)
			{
                MouseHoverBegin(this, new MouseEventArgs(mouseData));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the mouse has left a hover state.  Override to add
        /// mouse hover handling code.
        /// </summary>
        internal protected virtual void OnMouseHoverEnd(MouseData mouseData)
		{
			if (MouseHoverEnd != null)
			{
                MouseHoverEnd(this, new MouseEventArgs(mouseData));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the mouse has starting a drag action.  Override to add mouse drag
        /// handling code.
        /// </summary>
        internal protected virtual void OnMouseDragBegin(Point sPosOrigin)
		{
			if (MouseDragBegin != null)
			{
				MouseDragBegin(this, new MouseDragEventArgs(sPosOrigin));
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the drag state for the mouse has ended.  Override to add mouse drag
        /// handling code.
        /// </summary>
        internal protected virtual void OnMouseDragEnd(Point sPos)
		{
			if (MouseDragEnd != null)
			{
				MouseDragEnd(this, new MouseDragEventArgs(sPos));
			}
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        List<Schedule> scheduleList = new List<Schedule>();
        List<Schedule> scheduleRemoveList = new List<Schedule>();
        List<Schedule> scheduleAddList = new List<Schedule>();
        internal bool isSetup;
        #endregion
    }
}
