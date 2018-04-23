using System;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using System.Collections;
using ReactNative.Tracing;
using ElmSharp;
using System.Runtime.InteropServices;

using Tizen;
using ReactNative.Common;

namespace ReactNative.Views.Text
{
    public class ReactTextView : Label
    {
        public ReactTextView(EvasObject parent) : base(parent)
        {
        }

        public static string filterText(string strText)
        {
            strText.Replace("&", "&amp;");
            strText.Replace("<", "&lt;");
            strText.Replace(">", "&gt;");
            return strText;
        }
    }
}