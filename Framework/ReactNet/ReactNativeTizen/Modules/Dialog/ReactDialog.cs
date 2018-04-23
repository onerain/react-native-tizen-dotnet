using System.Collections.Generic;
using ReactNative.Bridge;

using ElmSharp;
using Tizen;

namespace ReactNative.Modules.Dialog
{

    class ReactDialog
    {
        private Popup _dialog;
        
        private List<Button> _buttonsHandle;

        public ReactDialog()
        {
            _dialog = null;
            _buttonsHandle = new List<Button>();
        }

        public void showDialog(string title, string message, List<string> buttons, ICallback actionCallback)
        {
            //dismiss existing dialog
            dismiss();

            lock (_buttonsHandle)
            {
                _dialog = new Popup(ReactProgram.RctWindow);
                _dialog.SetPartText("title,text", title);
                _dialog.Text = message;

                int count = 1;
                foreach (var button_title in buttons)
                {
                    var button = new Button(ReactProgram.RctWindow);
                    button.Text = button_title;
                    button.Clicked += (object sender, System.EventArgs e) =>
                    {
                        var temp = sender as Button;
                        var clicked_index = _buttonsHandle.IndexOf(temp);

                        actionCallback.Invoke(DialogModuleHelper.ActionButtonClicked, clicked_index);
                        dismiss();
                    };

                    _dialog.SetPartContent($"button{count}", button);

                    _buttonsHandle.Add(button);
                    count++;
                }

                _dialog.Show();
                _dialog.SetFocus(true);
            }
        }

        private void dismiss()
        {
            lock (_buttonsHandle)
            {
                if (_dialog != null)
                {
                    _dialog.Dismiss();
                    foreach (var button in _buttonsHandle)
                    {
                        button.Unrealize();
                    }
                    _buttonsHandle.Clear();
                    _dialog.Unrealize();
                    _dialog = null;
                }
            }
        }
    }
}
