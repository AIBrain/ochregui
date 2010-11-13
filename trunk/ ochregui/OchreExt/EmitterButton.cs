using System;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Extended
{
    public class EmitterButtonTemplate : ButtonTemplate
    {
        public EmitterButtonTemplate()
        {
        }

        public uint Speed { get; set; }

        public uint StartEmittingDelay { get; set; }
    }

    public class EmitterButton : Button
    {
        public event EventHandler Emit;

        public EmitterButton(EmitterButtonTemplate template)
            : base(template)
        {
            this.Speed = template.Speed;
            this.StartEmittingDelay = template.StartEmittingDelay;

            CountdownSchedule = new Schedule(DoCountDownEnded, StartEmittingDelay);
            EmitSchedule = new Schedule(DoEmit, Speed);
        }



        protected uint Speed { get; private set; }
        protected uint StartEmittingDelay { get; set; }



        protected override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                DoStartEmitting();
            }
        }

        protected override void OnMouseButtonUp(MouseData mouseData)
        {
            base.OnMouseButtonUp(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                RemoveSchedule(EmitSchedule);
                RemoveSchedule(CountdownSchedule);
            }
        }

        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();

            RemoveSchedule(EmitSchedule);
            RemoveSchedule(CountdownSchedule);
        }

        protected virtual void OnEmitEvent()
        {
            if (Emit != null)
            {
                Emit(this, EventArgs.Empty);
            }
        }




        private void DoStartEmitting()
        {
            OnEmitEvent();

            AddSchedule(CountdownSchedule);
        }

       
        private void DoCountDownEnded()
        {
            RemoveSchedule(CountdownSchedule);
            AddSchedule(EmitSchedule);
        }

        private void DoEmit()
        {
            OnEmitEvent();
        }




        private Schedule EmitSchedule;
        private Schedule CountdownSchedule; 


    }
}
