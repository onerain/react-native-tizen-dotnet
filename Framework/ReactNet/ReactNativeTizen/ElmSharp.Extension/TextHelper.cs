using System;
using ElmSharp;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// The Text Helper contains functions that assist in working with text-able objects.
    /// </summary>
    internal static class TextHelper
    {
        /// <summary>
        /// Gets the size of raw text block.
        /// </summary>
        /// <param name="textable">The <see cref="EvasObject"/> with text part.</param>
        /// <returns>Returns the size of raw text block.</returns>
        public static Size GetRawTextBlockSize(EvasObject textable)
        {
            return GetElmTextPart(textable).TextBlockNativeSize;
        }

        /// <summary>
        /// Gets the size of formatted text block.
        /// </summary>
        /// <param name="textable">The <see cref="ElmSharp.EvasObject"/> with text part.</param>
        /// <returns>Returns the size of formatted text block.</returns>
        public static Size GetFormattedTextBlockSize(EvasObject textable)
        {
            return GetElmTextPart(textable).TextBlockFormattedSize;
        }

        /// <summary>
        /// Gets the ELM text part of evas object.
        /// </summary>
        /// <param name="textable">The <see cref="ElmSharp.EvasObject"/> with text part.</param>
        /// <exception cref="ArgumentException">Throws exception when parameter <param name="textable"> isn't text-able object or doesn't have ELM text part.</exception>
        /// <returns>Requested <see cref="ElmSharp.EdjeTextPartObject"/> instance.</returns>
        static EdjeTextPartObject GetElmTextPart(EvasObject textable)
        {
            Layout widget = textable as Layout;
            if (widget == null)
            {
                throw new ArgumentException("textable should be ElmSharp.Layout", "textable");
            }
            EdjeTextPartObject textPart = widget.EdjeObject["elm.text"];
            if (textPart == null)
            {
                throw new ArgumentException("There is no elm.text part", "textable");
            }
            return textPart;
        }
    }
}