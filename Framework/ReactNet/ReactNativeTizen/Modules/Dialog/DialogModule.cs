using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using ReactNative.Collections;
using System.Collections.Generic;


namespace ReactNative.Modules.Dialog
{
    class DialogModule : ReactContextNativeModuleBase
    {

        ReactDialog _dialog;
        public DialogModule(ReactContext reactContext)
            : base(reactContext)
        {
            _dialog = new ReactDialog();
        }

        public override string Name
        {
            get
            {
                return "DialogManagerTizen";
            }
        }

        public override IReadOnlyDictionary<string, object> Constants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { DialogModuleHelper.ActionButtonClicked, DialogModuleHelper.ActionButtonClicked },
                    { DialogModuleHelper.ActionDismissed, DialogModuleHelper.ActionDismissed },
                    { DialogModuleHelper.KeyButtonPositive, DialogModuleHelper.KeyButtonPositiveValue },
                    { DialogModuleHelper.KeyButtonNegative, DialogModuleHelper.KeyButtonNegativeValue },
                    { DialogModuleHelper.KeyButtonNeutral, DialogModuleHelper.KeyButtonNeutralValue },
                };
            }
        }

        [ReactMethod]
        public void showAlert(
            JObject config,
            ICallback errorCallback,
            ICallback actionCallback)
        {
            var message = config.Value<string>("message") ?? "";
            var title = config.Value<string>("title") ?? "";
            var buttons = new List<string>();

            if (config.ContainsKey(DialogModuleHelper.KeyButtonPositive))
                buttons.Add(config.Value<string>(DialogModuleHelper.KeyButtonPositive));
            if (config.ContainsKey(DialogModuleHelper.KeyButtonNegative))
                buttons.Add(config.Value<string>(DialogModuleHelper.KeyButtonNegative));
            if (config.ContainsKey(DialogModuleHelper.KeyButtonNeutral))
                buttons.Add(config.Value<string>(DialogModuleHelper.KeyButtonNeutral));

            //Run in UI Thread
            DispatcherHelpers.RunOnDispatcher(() =>
            {
                _dialog.showDialog(title, message, buttons, actionCallback);
            });
        }

    }
}