using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using ElmSharp;

namespace ReactNativeTizen.ElmSharp.Extension
{
    internal class VisualElementCollection<T> : IList<T>, INotifyCollectionChanged 
        where T : Widget
    {
        readonly ObservableCollection<T> _list;

        public VisualElementCollection(ObservableCollection<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            _list = list;
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }

            set
            {
                _list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly { get; internal set; }


        public virtual void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (IsReadOnly)
            {
                throw new NotSupportedException("The collection is readonly.");
            }
            if (_list.Contains(item))
            {
                return;
            }
            _list.Add(item);
        }

        public void Clear()
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("The collection is readonly.");
            }
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
            }
            foreach (T item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (IsReadOnly)
            {
                throw new NotSupportedException("The collection is read-only.");
            }
            _list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (IsReadOnly)
            {
                throw new NotSupportedException("The collection is read-only.");
            }
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("The collection is read-only.");
            }
            _list.RemoveAt(index);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                _list.CollectionChanged += value;
            }
            remove
            {
                _list.CollectionChanged -= value;
            }
        }
    }
}
