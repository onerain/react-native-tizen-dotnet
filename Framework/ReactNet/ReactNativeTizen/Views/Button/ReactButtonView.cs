using System;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using System.Collections;
using ReactNative.Tracing;
using ElmSharp;
using System.Runtime.InteropServices;

namespace ReactNative.Views.ReactButton
{
    public class ReactButtonUpdate
    {
        private readonly string mText;
        private readonly int mJsEventCounter;
        private readonly bool mContainsImages;
        private readonly float mPaddingLeft;
        private readonly float mPaddingTop;
        private readonly float mPaddingRight;
        private readonly float mPaddingBottom;
        private readonly int mTextAlign;

        public ReactButtonUpdate(
          string text,
          int jsEventCounter,
          bool containsImages,
          float paddingStart,
          float paddingTop,
          float paddingEnd,
          float paddingBottom,
          int textAlign)
        {
            mText = text;
            mJsEventCounter = jsEventCounter;
            mContainsImages = containsImages;
            mPaddingLeft = paddingStart;
            mPaddingTop = paddingTop;
            mPaddingRight = paddingEnd;
            mPaddingBottom = paddingBottom;
            mTextAlign = textAlign;
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
    }
}