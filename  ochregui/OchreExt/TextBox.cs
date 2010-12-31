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
using OchreGui.Utility;

namespace OchreGui.Extended
{
    public class TextBoxTemplate : PanelTemplate
    {
        public TextBoxTemplate()
        {

        }

        public uint TextSpeed { get; set; }
    }

    public class TextBox : Panel
    {
        public TextBox(TextBoxTemplate template)
            : base(template)
        {
            //OwnerDraw = true;
            TextSpeed = template.TextSpeed;
        }

        public int NumberOfLines { get; private set; }

        public int LineLength { get; private set; }

        private uint _textSpeed;
        public uint TextSpeed
        {
            get { return _textSpeed; }
            set
            {
                _textSpeed = value;

                if (ContainsSchedule(typeSchedule))
                {
                    RemoveSchedule(typeSchedule);

                    typeSchedule = new Schedule(TypeNextChar, _textSpeed);
                    AddSchedule(typeSchedule);
                }
            }
        }

        public void AddText(string text,Pigment pigment=null)
        {
            string[] words = Explode(text);

            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if ((currVirtualPos + Canvas.TextLength(word) >= LineLength) &&
                        (word[0] != '\n'))
                    {
                        buffer.Enqueue(new Atom('\n'));
                        currVirtualPos = 0;
                    }

                    foreach (char c in word)
                    {
                        buffer.Enqueue(new Atom(c,pigment));
                        currVirtualPos++;

                        if (c == '\n')
                            currVirtualPos = 0;
                    }

                    if (currVirtualPos != 0)
                    {
                        buffer.Enqueue(new Atom(' ',pigment));
                        currVirtualPos++;
                    }
                }
            }
        }

        public void AddText(string text, Color foreground)
        {
            if (foreground == null)
            {
                throw new ArgumentNullException("foreground");
            }

            Pigment pigment = DetermineMainPigment().ReplaceForeground(foreground);

            AddText(text, pigment);
        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            Rect textRect;

            if (HasFrame)
            {
                textRect = Rect.Inflate(this.LocalRect, -1, -1);
            }
            else
            {
                textRect = this.LocalRect;
            }

            NumberOfLines = textRect.Size.Height;

            LineLength = textRect.Size.Width;

            currLine = 0;
            currPos = 0;
            currVirtualPos = currPos;

            typeSchedule = new Schedule(TypeNextChar, TextSpeed);
            AddSchedule(typeSchedule);

            textCanvas = new Canvas(textRect.Size);
            textCanvas.SetDefaultPigment(DetermineMainPigment());
            textCanvas.Clear();

            textCanvasPos = textRect.UpperLeft;
        }


        protected override void Redraw()
        {
            base.Redraw();

            Canvas.Blit(textCanvas, textCanvasPos);
        }

        protected override Pigment DetermineMainPigment()
        {
            return base.DetermineMainPigment();
        }


        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                if (textCanvas != null)
                    textCanvas.Dispose();
            }
            _alreadyDisposed = true;
        }


        private void TypeNextChar()
        {
            if (buffer.Count == 0)
            {
                return;
            }

            Atom next = buffer.Dequeue();

            if (next.c == '\n')
            {
                currPos = 0;
                currLine++;

                if (currLine >= NumberOfLines)
                {
                    this.textCanvas.Scroll(0, -1);
                    currLine--;
                    currPos = 0;
                }
            }
            else
            {
                if (currPos < LineLength && currLine < NumberOfLines)
                {
                    textCanvas.SetDefaultPigment(DetermineMainPigment());
                    textCanvas.PrintChar(currPos, currLine, next.c,next.pigment);
                }
                currPos++;
            }
        }

        private string[] Explode(string text)
        {
            //text = text.Replace("\n", " ");
            text = text.Replace("\r", " ");
            text = text.Replace(".", ". ");
            text = text.Replace("\t", " ");
            text = text.Replace(",", ", ");
            text = text.Replace(";", "; ");
            text = text.Replace("  ", " ");

            string[] Words = text.Split(' ');

            return Words;
        }

        private bool _alreadyDisposed;
        private Schedule typeSchedule;

        private int currPos;
        private int currLine;

        private Point textCanvasPos;

        Canvas textCanvas;

        private int currVirtualPos;

        private Queue<Atom> buffer = new Queue<Atom>();

        private class Atom
        {
            public Atom(char c,Pigment pigment=null)
            {
                this.c = c;
                this.pigment = pigment;
            }

            public char c;
            public Pigment pigment;
        }
    }

}
