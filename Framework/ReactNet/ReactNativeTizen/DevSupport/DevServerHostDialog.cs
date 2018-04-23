using System;
using ElmSharp;
using ReactNative.Tracing;
using ReactNative.Common;
using ReactNative.Bridge;
using ReactNative.Views.Text;

namespace ReactNative.DevSupport
{
    class DevServerHostDialog
    {
        Popup _popup;
        ReactTextView _label;
        Entry _entry;
        Button _button;
        Box _box;

        static DevServerHostDialog _instance = null;
        public event EventHandler Closed = null;
        public string text = null;

        public DevServerHostDialog()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _popup = new Popup(ReactProgram.RctWindow)
                {

                };
                //_conformant = new Conformant(_popup);
                _box = new Box(_popup)
                {
                    AlignmentX = -1,
                    AlignmentY = -1,
                    WeightX = 1,
                    WeightY = 1,
                };

                _label = new ReactTextView (ReactProgram.RctWindow)
                {
                    Text = "Enter IP Address",
                };

                _entry = new Entry(ReactProgram.RctWindow)
                {
                    IsSingleLine = true,
                    Text = "Please type IP here...",
                    IsEditable = true

                };
                _entry.Focused += (s, e) =>
                {
                    _entry.Text = "";
                };
                _entry.CursorChanged += (s, e) =>
                {
                    _entry.MinimumWidth = 400;
                    //_entry.MinimumHeight = 100;
                };

                _button = new Button(ReactProgram.RctWindow)
                {
                    Text = "OK",
                };

                _button.Clicked += (s, e) =>
                {
                    text = _entry.Text;
                    Close();
                };
            });
        }

        static public DevServerHostDialog GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DevServerHostDialog();
            }
            return _instance;
        }


        public void Show()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _popup.SetContent(_box);
                _popup.Show();

                _box.PackEnd(_label);
                _box.PackEnd(_entry);
                _box.PackEnd(_button);

                _box.Show();

                _label.Show();
                _entry.Show();
                _button.Show();

                _label.AlignmentX = 0.5;
                _label.AlignmentY = 0.25;
                _label.WeightX = 1;
                _label.WeightY = 1;

                _entry.AlignmentX = 0.5;
                _entry.AlignmentY = 0.5;
                _entry.WeightX = 1;
                _entry.WeightY = 1;

                _button.AlignmentX = 0.5;
                _button.AlignmentY = 0.65;
                _button.WeightX = 1;
                _button.WeightY = 1;
            });
        }

        public void Close()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _label.Hide();
                _box.Hide();
                _entry.Hide();
                _button.Hide();
                _popup.Hide();
                Closed?.Invoke(this, null);
            });
        }

        public void ResetCloseEvent()
        {
            Closed = null;
        }
    }
}