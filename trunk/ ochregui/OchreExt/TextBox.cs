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




        public void AddText(string text)
        {
            string[] words = Explode(text);

            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if ((currVirtualPos + word.Length >= LineLength) &&
                        (word[0] != '\n'))
                    {
                        buffer.Enqueue('\n');
                        currVirtualPos = 0;
                    }

                    foreach (char c in word)
                    {
                        buffer.Enqueue(c);
                        currVirtualPos++;

                        if (c == '\n')
                            currVirtualPos = 0;
                    }

                    if (currVirtualPos != 0)
                    {
                        buffer.Enqueue(' ');
                        currVirtualPos++;
                    }
                }
            }
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
            textCanvasPos = textRect.UpperLeft;
        }


        protected override void Redraw()
        {
            base.Redraw();

            Canvas.Blit(textCanvas, textCanvasPos);
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

            char next = buffer.Dequeue();

            if (next == '\n')
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
                    textCanvas.PrintChar(currPos, currLine, next);
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

        private Queue<char> buffer = new Queue<char>();
    }
}
