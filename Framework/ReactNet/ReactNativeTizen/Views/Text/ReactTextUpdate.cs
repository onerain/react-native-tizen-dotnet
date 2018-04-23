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
    public class ReactTextUpdate
    {
        private readonly string mText;
        private readonly int mJsEventCounter;
        private readonly bool mContainsImages;
        private readonly float mPaddingLeft;
        private readonly float mPaddingTop;
        private readonly float mPaddingRight;
        private readonly float mPaddingBottom;
        private readonly int mTextAlign;
        private readonly double mTextVAlign;

        public ReactTextUpdate(
          string text,
          int jsEventCounter,
          bool containsImages,
          float paddingStart,
          float paddingTop,
          float paddingEnd,
          float paddingBottom,
          int textAlign,
          double textVAlign)
        {
            mText = text;
            mJsEventCounter = jsEventCounter;
            mContainsImages = containsImages;
            mPaddingLeft = paddingStart;
            mPaddingTop = paddingTop;
            mPaddingRight = paddingEnd;
            mPaddingBottom = paddingBottom;
            mTextAlign = textAlign;
            mTextVAlign = textVAlign;
        }

        public string getText()
        {
            return mText;
        }

        public int getJsEventCounter()
        {
            return mJsEventCounter;
        }

        public bool containsImages()
        {
            return mContainsImages;
        }

        public float getPaddingLeft()
        {
            return mPaddingLeft;
        }

        public float getPaddingTop()
        {
            return mPaddingTop;
        }

        public float getPaddingRight()
        {
            return mPaddingRight;
        }

        public float getPaddingBottom()
        {
            return mPaddingBottom;
        }

        public int getTextAlign()
        {
            return mTextAlign;
        }

        public double getTextVAlign()
        {
            return mTextVAlign;
        }
    }
}