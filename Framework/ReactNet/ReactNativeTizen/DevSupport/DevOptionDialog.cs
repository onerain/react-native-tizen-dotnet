using System;
using ElmSharp;
using ReactNative.Tracing;
using ReactNative.Common;
using ReactNative.Bridge;
using Tizen;

namespace ReactNative.DevSupport
{
    class DevOptionHandler
    {
        private readonly Action _onSelect;

        public DevOptionHandler(string name, Action onSelect)
        {
            Name = name;
            _onSelect = onSelect;
        }

        public string Name { get; }

        public Action HideDialog { get; set; }

        public void OnSelect()
        {
            HideDialog?.Invoke();

            _onSelect();
        }
    }

    class DevOptionDialog: IDisposable
    {
        Popup _popup;
        GenList _list;
        //Conformant _conformant;
        static DevOptionDialog _instance = null;

        public Window Owner;

        public event EventHandler Closed = null;
        private DevOptionDialog()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _popup = new Popup(ReactProgram.RctWindow)
                {

                };
                //_conformant = new Conformant(_popup);
                _list = new GenList(_popup)
                {
                    Homogeneous = true,
                    ListMode = GenListMode.Expand,
                };
                _list.ItemSelected += ItemSelected;
            });
        }

        public void Dispose()
        {
            Log.Info(ReactConstants.Tag, "## Dispose --> DevOptionDialog ##");
        }

        static public DevOptionDialog GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DevOptionDialog();
            }
            return _instance;
        }

        GenItemClass defaultClass = new GenItemClass("default")
        {
            GetTextHandler = (obj, part) =>
            {
                return string.Format("{0}", ((DevOptionHandler)obj).Name);
            }
        };

        private void ItemSelected(object sender, GenListItemEventArgs e)
        {
            Log.Info(ReactConstants.Tag, ((DevOptionHandler)e.Item.Data).Name + " Item was selected");
            ((DevOptionHandler)e.Item.Data).OnSelect();
        }

        public void Show()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _popup.Show();
                //_conformant.Show();
                //_list.Show();
                _popup.SetContent(_list);
                Log.Info(ReactConstants.Tag, "_list.show");
            });
        }

        public void Close()
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                //_list.Hide();
                _popup.Hide();
                //_conformant.Hide();
                _list.Clear();
                Closed?.Invoke(this, null);
            });
        }

        public void Add(DevOptionHandler option)
        {
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                Log.Info(ReactConstants.Tag, "Add item:" + option.Name);
                _list.Append(defaultClass, option);
            });
        }

        public void ResetCloseEvent()
        {
            Closed = null;
        }
    }
}