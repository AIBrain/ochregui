using System;
using System.Collections.Generic;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Extended
{
    internal class SpinManager : Manager
    {
        public SpinManager(int minValue,int maxValue,uint delay,uint speed,Point position)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.delay = delay;
            this.speed = speed;
            this.position = position;
        }



        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            
        }



        int minValue;
        int maxValue;
        uint delay;
        uint speed;
        Point position;
    }

















    public class SpinBoxTemplate : ControlTemplate
    {
        public SpinBoxTemplate()
        {
            MaximumValue = 1;
            Label = "";
        }

        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public uint SpinDelay { get; set; }
        public uint SpinSpeed { get; set; }
        public string Label { get; set; }

        public override Size CalculateSize()
        {
            throw new NotImplementedException();
        }
    }

    public class SpinBox : Control
    {
        public SpinBox(SpinBoxTemplate template)
            : base(template)
        {
            MinimumValue = template.MinimumValue;
            MaximumValue = template.MaximumValue;
            SpinDelay = template.SpinDelay;
            SpinSpeed = template.SpinSpeed;
            Label = template.Label;

            HasFrame = true;
            CanHaveKeyboardFocus = false;
            HilightWhenMouseOver = false;

        }


        protected int MinimumValue { get; private set; }
        protected int MaximumValue { get; private set; }
        protected uint SpinDelay { get; set; }
        protected uint SpinSpeed { get; set; }
        protected string Label { get; private set; }



        protected override void OnSettingUp()
        {
            base.OnSettingUp();


        }


    }
}
