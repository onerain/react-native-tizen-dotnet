using ReactNative.Modules.DevSupport;
using System.Collections.Generic;

using System;
//using System.Configuration;
using static System.FormattableString;
using ReactNative.Common;
using ReactNative.Tracing;

namespace ReactNative.DevSupport
{
    class DevInternalSettings : IDeveloperSettings
    {
        private const string FpsDebugKey = "fps_debug";
        private const string DebugServerHostKey = "debug_http_host";
        private const string JsDevModeDebugKey = "js_dev_mode_debug";
        private const string AnimationsDebugKey = "animations_debug";
        private const string JsMinifyDebugKey = "js_minify_debug";
        private const string ReloadOnJSChangeKey = "reload_on_js_change";
        private const string HotModuleReplacementKey = "hot_module_replacement";

        private static readonly HashSet<string> s_triggerReload = new HashSet<string>
        {
            FpsDebugKey,
            ReloadOnJSChangeKey,
            JsDevModeDebugKey,
            JsMinifyDebugKey,
        };

        private readonly IDevSupportManager _debugManager;

        private readonly IDictionary<string, object> _localSettings = new Dictionary<string, object>();

        public DevInternalSettings(IDevSupportManager debugManager)
        {
            _debugManager = debugManager;
        }

		//TODO: shoushou set true temp, need to modify
        public bool IsJavaScriptDevModeEnabled
        {
            get
            {
#if DEBUG
                return GetSetting(JsDevModeDebugKey, true);
#else
                return GetSetting(JsDevModeDebugKey, false);
#endif
            }
            set
            {
                SetSetting(JsDevModeDebugKey, value);
            }
        }

        public string DebugServerHost
        {
            get
            {
                return GetSetting(DebugServerHostKey, "string"); //defaultValue is string cannot judge default(string) on tizen
            }
            set
            {
                SetSetting(DebugServerHostKey, value);
            }
        }

        public bool IsAnimationFpsDebugEnabled
        {
            get
            {
                return GetSetting(ReloadOnJSChangeKey, false);
            }
            set
            {
                SetSetting(ReloadOnJSChangeKey, value);
            }
        }

        public bool IsElementInspectorEnabled { get; set; }

        public bool IsFpsDebugEnabled
        {
            get
            {
                return GetSetting(FpsDebugKey, false);
            }
            set
            {
                SetSetting(FpsDebugKey, value);
            }
        }

		//TODO: shoushou temp set HMR opened forcely
        public bool IsHotModuleReplacementEnabled
        {
            get
            {
                return GetSetting(HotModuleReplacementKey, false);
            }
            set
            {
                SetSetting(HotModuleReplacementKey, value);
            }
        }

        public bool IsJavaScriptMinifyEnabled
        {
            get
            {
                return GetSetting(JsMinifyDebugKey, false);
            }
            set
            {
                SetSetting(JsMinifyDebugKey, value);
            }
        }

        public bool IsReloadOnJavaScriptChangeEnabled
        {
            get
            {
                return GetSetting(ReloadOnJSChangeKey, false);
            }
            set
            {
                SetSetting(ReloadOnJSChangeKey, value);
            }
        }


        //TODO: Microsoft had not support permanent storage yet, 
        //so use our implement instead temp

        private T GetSetting<T>(string key, T defaultValue)
        {
            if (defaultValue is bool)
            {
                var ret = ReactConfig.GetValue(key);
                if (ret != null)
                {
                    if (ret == "true")
                    {
                        Object data = true;
                        return (T)data;
                    }
                    else
                    {
                        Object data = false;
                        return (T)data;
                    }
                }

            }
            else if (defaultValue is string)
            {
                var ret = ReactConfig.GetValue(key);
                if (ret != null)
                {
                    Object data = ret;
                    return (T)data;
                }
            }
            else
            {
                RNTracer.Write(ReactConstants.Tag, "[RN_EXCEPTION]ERROR! ReactNative.DevSupport.DevInternalSettings GetSetting encounter unexpected type, key is "+ key);
            }

            return defaultValue;        
		}

		//TODO: Microsoft had not support permanent storage yet, 
		//so use our implement instead temp
        private void SetSetting<T>(string key, T value)
        {
            if (value is bool)
            {
                ReactConfig.SetValue(key, value.ToString().ToLower());
            }
            else if (value is string)
            {
                ReactConfig.SetValue(key, value.ToString());
            }
            else
            {
                RNTracer.Write(ReactConstants.Tag, "[RN_EXCEPTION]ERROR! ReactNative.DevSupport.DevInternalSettings GetSetting encounter unexpected type");
                return;
            }

            if (s_triggerReload.Contains(key))
            {
                _debugManager.ReloadSettings();
            }
        }
    }
}
