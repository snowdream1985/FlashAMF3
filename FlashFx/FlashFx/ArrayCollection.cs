using FlashFx.AMF3;
using System;
using System.Collections;
using System.Collections.Generic;

namespace flash.utils
{
    public class ArrayCollection : IExternalizable, IList
    {
        private IList _list;

        public ArrayCollection()
        {
            _list = new List<object>();
        }

        public ArrayCollection(IList list)
        {
            _list = list;
        }

        public int Count
        {
            get { return _list == null ? 0 : _list.Count; }
        }

        public IList List
        {
            get { return _list; }
        }

        public object[] ToArray()
        {
            if (_list != null)
            {
                if (_list is ArrayList)
                {
                    return ((ArrayList)_list).ToArray();
                }

                if (_list is List<object>)
                {
                    return ((List<object>)_list).ToArray();
                }

                object[] objArray = new object[_list.Count];

                for (int i = 0; i < _list.Count; i++)
                {
                    objArray[i] = _list[i];
                }

                return objArray;
            }

            return null;
        }

        #region IExternalizable Members

        public void readExternal(IDataInput input)
        {
            _list = input.readObject() as IList;
        }

        public void writeExternal(IDataOutput output)
        {
            output.writeObject(ToArray());
        }

        #endregion

        #region IList Members

        public bool IsReadOnly { get { return _list.IsReadOnly; } }

        public object this[int index]
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

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
        }

        public void Remove(object value)
        {
            _list.Remove(value);
        }

        public bool Contains(object value)
        {
            return _list.Contains(value);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int IndexOf(object value)
        {
            return _list.IndexOf(value);
        }

        public int Add(object value)
        {
            return _list.Add(value);
        }

        public bool IsFixedSize { get { return _list.IsFixedSize; } }

        #endregion

        #region ICollection Members

        public bool IsSynchronized { get { return _list.IsSynchronized; } }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        public object SyncRoot { get { return _list.SyncRoot; } }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
