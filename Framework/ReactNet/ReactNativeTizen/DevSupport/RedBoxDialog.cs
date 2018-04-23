using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactNative.Tracing;
using ReactNative.Common;
using ReactNative.Bridge;
using ElmSharp;
using ReactNative.Views.Text;
using Tizen;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ReactNative.DevSupport
{
    /// <summary>
    /// The content dialog for red box exception display.
    /// </summary>
    class RedBoxDialog : IDisposable
    {
        private readonly Action _onClick;
        private string _message;
        private IList<IStackFrame> _stackTrace;

        public event EventHandler Closed = null;

        ReactTextView redboxTitle = null;
        ReactTextView reloadBg = null;
        Button reloadBtn = null;
        Box background = null;
        Scroller scroller = null;
        Box scrollBox = null;
        List<ReactTextView> labelList = null;


        /// <summary>
        /// Instantiates the <see cref="RedBoxDialog"/>.
        /// </summary>
        /// <param name="onClick">
        /// Action to take when primary button is clicked.
        /// </param>
        public RedBoxDialog(Action onClick)
        {
            _onClick = onClick;
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                if (redboxTitle == null)
                {
                    redboxTitle = new ReactTextView(ReactProgram.RctWindow)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1,
                        BackgroundColor = Color.Red,
                        MinimumHeight = 120,
                    };
                    redboxTitle.Resize(1160, 120);
                    redboxTitle.Move(240, 60);

                }

                if (reloadBg == null)
                {
                    reloadBg = new ReactTextView(ReactProgram.RctWindow)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1,
                        BackgroundColor = Color.Red,
                        MinimumHeight = 120,
                    };
                    reloadBg.Resize(280, 120);
                    reloadBg.Move(1400, 60);
                }

                if (reloadBtn == null)
                {
                    reloadBtn = new Button(ReactProgram.RctWindow)
                    {
                        Text = "<span color=#000000>Reload JS</span>",
                    };
                    reloadBtn.SetPartColor("bg", Color.FromRgb(120, 120, 120));
                    reloadBtn.Resize(160, 60);
                    reloadBtn.Move(1450, 90);
                    reloadBtn.Clicked += (s, e) =>
                    {
                        _onClick();
                    };
                }

                if (background == null)
                {
                    background = new Box(ReactProgram.RctWindow)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1
                    };

                }

                if (scroller == null)
                {
                    scroller = new Scroller(ReactProgram.RctWindow)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1,
                        ScrollBlock = ScrollBlock.None
                    };
                    scroller.BackgroundColor = Color.Red;
                    background.SetContent(scroller);
                    background.SetLayoutCallback(() =>
                    {
                        scroller.Resize(1440, 840);
                        scroller.Move(240, 180);
                    });
                }

                if (scrollBox == null)
                {
                    scrollBox = new Box(ReactProgram.RctWindow)
                    {
                        AlignmentX = -1,
                        AlignmentY = -1,
                        WeightX = 1,
                        WeightY = 1
                    };
                    scroller.SetContent(scrollBox);
                }
                if (labelList == null)
                {
                    labelList = new List<ReactTextView>();
                }
            });
        }

        /// <summary>
        /// The error cookie.
        /// </summary>
        public int ErrorCookie
        {
            get;
            set;
        }

        /// <summary>
        /// The exception message.
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;

            }
        }

        /// <summary>
        /// The stack trace.
        /// </summary>
        public IList<IStackFrame> StackTrace
        {
            get
            {
                return _stackTrace;
            }
            set
            {
                _stackTrace = value;
            }
        }

        public void Dispose()
        {
            Log.Info(ReactConstants.Tag, "## Dispose --> RedBoxDialog ##");
        }

        ////////////////////////////////////////////////////////////////////////////////
        //static public RedBoxDialog GetInstance(Action onClick)
        //{
        //    if (_instance == null)
        //    {
        //        _instance = new RedBoxDialog(onClick);
        //    }
        //    return _instance;
        //}


        public void Show()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                if (redboxTitle != null)
                {
                    _message = _message.Replace("&", "&amp;");
                    _message = _message.Replace("<", "&lt;");
                    _message = _message.Replace(">", "&gt;");
                    string strfontsize = "0";
                    if (_message.Length < 200)
                    {
                        strfontsize = "30";
                    }
                    else if (_message.Length < 400)
                    {
                        strfontsize = "26";
                    }
                    else if (_message.Length < 600)
                    {
                        strfontsize = "22";
                    }
                    else if (_message.Length < 800)
                    {
                        strfontsize = "18";
                    }
                    else
                    {
                        strfontsize = "14";
                    }
                    redboxTitle.Text = $"<span font_weight=bold font_size={strfontsize} color=#ffffff>" + _message + "</span>";
                    redboxTitle.LineWrapType = WrapType.Mixed;
                    redboxTitle.Show();
                }
                if (reloadBg != null)
                {
                    reloadBg.Show();
                }
                if (reloadBtn != null)
                {
                    reloadBtn.Show();
                }
                if (background != null)
                {
                    background.Show();
                }
                if (scroller != null)
                {
                    scroller.Show();
                }
                if (scrollBox != null)
                {
                    scrollBox.Show();
                }
                if (_stackTrace != null && scrollBox != null)
                {
                    foreach (var item in _stackTrace)
                    {
                        ReactTextView slideLine = new ReactTextView(ReactProgram.RctWindow)
                        {
                            AlignmentX = -1,
                            AlignmentY = -1,
                            WeightX = 1,
                            WeightY = 1,
                            BackgroundColor = Color.White,
                            MinimumHeight = 5,
                        };
                        scrollBox.PackEnd(slideLine);
                        if (labelList != null)
                        {
                            labelList.Add(slideLine);
                        }
                        else
                        {
                            RNTracer.Write(ReactConstants.Tag, "Fatal Error in Redbox add slideLine in list, list is null");
                        }

                        ReactTextView colorBox = new ReactTextView(ReactProgram.RctWindow)
                        {
                            AlignmentX = -1,
                            AlignmentY = -1,
                            WeightX = 1,
                            WeightY = 1,
                            BackgroundColor = Color.Red,
                            MinimumHeight = 100,
                        };
                        var tempFilename = item.FileName.Replace("&", "&amp;");
                        colorBox.Text = "<span font_size=30 color=#ffffff>" + item.Method + "</span><span font_size=20 color=#ffffff><br>" + tempFilename + ":" + item.Line + ":" + item.Column + "</span>";
                        scrollBox.PackEnd(colorBox);
                        if (labelList != null)
                        {
                            labelList.Add(colorBox);
                        }
                        else
                        {
                            RNTracer.Write(ReactConstants.Tag, "Fatal Error in Redbox add colorBox in list, list is null");
                        }
                    }
                }

                if (labelList != null)
                {
                    foreach (var item in labelList)
                    {
                        item.Show();
                    }
                }
            });
        }

        public void Close()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                if (scrollBox != null)
                {
                    scrollBox.Hide();
                    scrollBox.Unrealize();
                    scrollBox = null;
                }
                if (scroller != null)
                {
                    scroller.Hide();
                    scroller.Unrealize();
                    scroller = null;
                }
                if (background != null)
                {
                    background.Hide();
                    background.Unrealize();
                    background = null;
                }
                if (reloadBtn != null)
                {
                    reloadBtn.Hide();
                    reloadBtn.Unrealize();
                    reloadBtn = null;
                }
                if (reloadBg != null)
                {
                    reloadBg.Hide();
                    reloadBg.Unrealize();
                    reloadBg = null;
                }
                if (redboxTitle != null)
                {
                    redboxTitle.Hide();
                    redboxTitle.Unrealize();
                    redboxTitle = null;
                }
                if (labelList != null)
                {
                    foreach (var item in labelList)
                    {
                        item.Hide();
                        item.Unrealize();
                    }
                    labelList.Clear();
                    labelList = null;
                }
                Closed?.Invoke(this, null);
            });
        }
    }
}
