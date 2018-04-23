using System;
using System.Collections.Generic;
using ElmSharp;

using Tizen;
using ReactNative.Common;

using ReactNative.UIManager;

namespace ReactNativeTizen.ElmSharp.Extension
{
    /// <summary>
    /// Extends the ElmSharp.ReactPicker class with functionality useful to React Native renderer.
    /// </summary>
    /// <remarks>
    /// This class overrides the layout mechanism. Instead of using the native layout,
    /// <c>LayoutUpdated</c> event is sent.
    /// </remarks>
    public class ReactPicker : Hoversel
	{
        Dictionary<int, int> _items = new Dictionary<int, int>();
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="parent">The parent EvasObject.</param>
        public ReactPicker(EvasObject parent) : base(parent)
		{
            
        }

        public void AddItemPlus(string label)
        {
            var item = base.AddItem(label);
            _items.Add(item.Id, _items.Count);
        }

        public void ClearPlus()
        {
            _items.Clear();
            base.Clear();
        }

        public int GetIndex(int id)
        {
            return _items[id];
        }

    }
}