// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DejaVu.Collections.Generic
{
    public class ObservableUndoRedoList<T> : IUndoRedoMember, IList<T>, IList, IChangedNotification, INotifyPropertyChanged
    {
        List<T> _list;

        #region IUndoRedoMember Members

        void IUndoRedoMember.OnCommit(object change)
        {
            Debug.Assert(change != null);
            ((Change<List<T>>)change).NewState = _list;
        }

        void IUndoRedoMember.OnUndo(object change)
        {
            Debug.Assert(change != null);
            _list = ((Change<List<T>>)change).OldState;
        }

        void IUndoRedoMember.OnRedo(object change)
        {
            Debug.Assert(change != null);
            _list = ((Change<List<T>>)change).NewState;
        }

		public object Owner
		{
			get
			{
				return UndoRedoMemberExtender.GetOwner(this);
			}
			set
			{
				UndoRedoMemberExtender.SetOwner(this, value);
			}
		}

		public string Name
		{
			get
			{
				return UndoRedoMemberExtender.GetName(this);
			}
			set
			{
				UndoRedoMemberExtender.SetName(this, value);
			}
		}

		/// <summary>
		/// This event is fired if the list was changed during a command. 
		/// MemberChangedEventArgs.NewValue contains readonly version of new list.
		/// MemberChangedEventArgs.OldValue contains readonly version of old list.
		/// </summary>
		public event EventHandler<MemberChangedEventArgs> Changed
		{
			add { UndoRedoMemberExtender.SubscribeChanges(this, value); }
			remove { UndoRedoMemberExtender.UnsubscribeChanges(this, value);  }
		}

        #endregion

        void Enlist()
        {
            Enlist(true);
        }
        void Enlist(bool copyItems)
        {
			UndoRedoArea.AssertCommand();
			Command command = UndoRedoArea.CurrentArea.CurrentCommand;
			if (!command.IsEnlisted(this))
			{
				var change = new Change<List<T>>();
				change.OldState = _list;
				command[this] = change;
				if (copyItems)
					_list = new List<T>(_list);
				else
					_list = new List<T>();
			}
        }        

        ///<summary>
        /// Initializes a new instance of the System.Collections.Generic.List<T> class
        /// that is empty and has the default initial capacity.
        ///</summary>
        public ObservableUndoRedoList()
        {
            _list = new List<T>();
        }
        
        ///<summary>
        // Initializes a new instance of the System.Collections.Generic.List<T> class
        // that contains elements copied from the specified collection and has sufficient
        // capacity to accommodate the number of elements copied.
        //
        // Parameters:
        //   collection:
        // The collection whose elements are copied to the new list.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        // collection is null.
        ///</summary>
        public ObservableUndoRedoList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        ///<summary>
        // Gets or sets the total number of elements the internal data structure can
        // hold without resizing.
        //
        // Returns:
        // The number of elements that the System.Collections.Generic.List<T> can contain
        // before resizing is required.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // System.Collections.Generic.List<T>.Capacity is set to a value that is less
        // than System.Collections.Generic.List<T>.Count.
        ///</summary>
        public int Capacity 
        {
            get { return _list.Capacity; }
            set { _list.Capacity = value; }
        }
        
        ///<summary>
        // Gets the number of elements actually contained in the System.Collections.Generic.List<T>.
        //
        // Returns:
        // The number of elements actually contained in the System.Collections.Generic.List<T>.
        ///</summary>
        public int Count 
        {
            get { return _list.Count; }
        }

        ///<summary>
        // Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        // The zero-based index of the element to get or set.
        //
        // Returns:
        // The element at the specified index.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<T>.Count.
        ///</summary>
        public T this[int index] 
        {
            get { return _list[index]; }
            set
            {
                Enlist();
                _list[index] = value;
            }
        }

        ///<summary>
        // Adds an object to the end of the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   item:
        // The object to be added to the end of the System.Collections.Generic.List<T>.
        // The value can be null for reference types.
        ///</summary>
        public void Add(T item)
        {
            Enlist();
            _list.Add(item);
            OnPropertyChanged();
        }
        //
        ///<summary>
        // Adds the elements of the specified collection to the end of the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   collection:
        // The collection whose elements should be added to the end of the System.Collections.Generic.List<T>.
        // The collection itself cannot be null, but it can contain elements that are
        // null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        // collection is null.
        ///</summary>
        public void AddRange(IEnumerable<T> collection)
        {
            Enlist();
            _list.AddRange(collection);
            OnPropertyChanged();
        }
        //
        ///<summary>
        // Returns a read-only System.Collections.Generic.IList<T> wrapper for the current
        // collection.
        //
        // Returns:
        // A System.Collections.Generic.ReadOnlyCollection`1 that acts as a read-only
        // wrapper around the current System.Collections.Generic.List<T>.
        ///</summary>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(_list);
        }
        //
        ///<summary>
        // Searches the entire sorted System.Collections.Generic.List<T> for an element
        // using the default comparer and returns the zero-based index of the element.
        //
        // Parameters:
        //   item:
        // The object to locate. The value can be null for reference types.
        //
        // Returns:
        // The zero-based index of item in the sorted System.Collections.Generic.List<T>,
        // if item is found; otherwise, a negative number that is the bitwise complement
        // of the index of the next element that is larger than item or, if there is
        // no larger element, the bitwise complement of System.Collections.Generic.List<T>.Count.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        // The default comparer System.Collections.Generic.Comparer<T>.Default cannot
        // find an implementation of the System.IComparable<T> generic interface or
        // the System.IComparable interface for type T.
        ///</summary>
        public int BinarySearch(T item)
        {
            return _list.BinarySearch(item);
        }
        //
        ///<summary>
        // Searches the entire sorted System.Collections.Generic.List<T> for an element
        // using the specified comparer and returns the zero-based index of the element.
        //
        // Parameters:
        //   item:
        // The object to locate. The value can be null for reference types.
        //
        //   comparer:
        // The System.Collections.Generic.IComparer<T> implementation to use when comparing
        // elements.-or-null to use the default comparer System.Collections.Generic.Comparer<T>.Default.
        //
        // Returns:
        // The zero-based index of item in the sorted System.Collections.Generic.List<T>,
        // if item is found; otherwise, a negative number that is the bitwise complement
        // of the index of the next element that is larger than item or, if there is
        // no larger element, the bitwise complement of System.Collections.Generic.List<T>.Count.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        // comparer is null, and the default comparer System.Collections.Generic.Comparer<T>.Default
        // cannot find an implementation of the System.IComparable<T> generic interface
        // or the System.IComparable interface for type T.
        ///</summary>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(item, comparer);
        }
        
        ///<summary>
        // Searches a range of elements in the sorted System.Collections.Generic.List<T>
        // for an element using the specified comparer and returns the zero-based index
        // of the element.
        //
        // Parameters:
        //   count:
        // The length of the range to search.
        //
        //   item:
        // The object to locate. The value can be null for reference types.
        //
        //   index:
        // The zero-based starting index of the range to search.
        //
        //   comparer:
        // The System.Collections.Generic.IComparer<T> implementation to use when comparing
        // elements, or null to use the default comparer System.Collections.Generic.Comparer<T>.Default.
        //
        // Returns:
        // The zero-based index of item in the sorted System.Collections.Generic.List<T>,
        // if item is found; otherwise, a negative number that is the bitwise complement
        // of the index of the next element that is larger than item or, if there is
        // no larger element, the bitwise complement of System.Collections.Generic.List<T>.Count.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-count is less than 0.
        //
        //   System.InvalidOperationException:
        // comparer is null, and the default comparer System.Collections.Generic.Comparer<T>.Default
        // cannot find an implementation of the System.IComparable<T> generic interface
        // or the System.IComparable interface for type T.
        //
        //   System.ArgumentException:
        // index and count do not denote a valid range in the System.Collections.Generic.List<T>.
        ///</summary>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        { 
            return _list.BinarySearch(index, count, item, comparer); 
        }
        
        ///<summary>
        // Removes all elements from the System.Collections.Generic.List<T>.
        ///</summary>
        public void Clear()
        {
			UndoRedoArea.AssertCommand();
			Command command = UndoRedoArea.CurrentArea.CurrentCommand;
			if (!command.IsEnlisted(this))
			{
				Enlist(false);
			}
			else
				_list.Clear();

            OnPropertyChanged();
        }
        
        ///<summary>
        // Determines whether an element is in the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        // Returns:
        // true if item is found in the System.Collections.Generic.List<T>; otherwise,
        // false.
        ///</summary>
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }
                
        ///<summary>
        // Copies the entire System.Collections.Generic.List<T> to a compatible one-dimensional
        // array, starting at the beginning of the target array.
        //
        // Parameters:
        //   array:
        // The one-dimensional System.Array that is the destination of the elements
        // copied from System.Collections.Generic.List<T>. The System.Array must have
        // zero-based indexing.
        //
        // Exceptions:
        //   System.ArgumentException:
        // The number of elements in the source System.Collections.Generic.List<T> is
        // greater than the number of elements that the destination array can contain.
        //
        //   System.ArgumentNullException:
        // array is null.
        ///</summary>
        public void CopyTo(T[] array)
        {
            _list.CopyTo(array);
        }
        
        ///<summary>
        // Copies the entire System.Collections.Generic.List<T> to a compatible one-dimensional
        // array, starting at the specified index of the target array.
        //
        // Parameters:
        //   array:
        // The one-dimensional System.Array that is the destination of the elements
        // copied from System.Collections.Generic.List<T>. The System.Array must have
        // zero-based indexing.
        //
        //   arrayIndex:
        // The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ArgumentException:
        // arrayIndex is equal to or greater than the length of array.-or-The number
        // of elements in the source System.Collections.Generic.List<T> is greater than
        // the available space from arrayIndex to the end of the destination array.
        //
        //   System.ArgumentOutOfRangeException:
        // arrayIndex is less than 0.
        //
        //   System.ArgumentNullException:
        // array is null.
        ///</summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        
        
        ///<summary>
        // Returns an enumerator that iterates through the System.Collections.Generic.List<T>.
        //
        // Returns:
        // A System.Collections.Generic.List<T>.Enumerator for the System.Collections.Generic.List<T>.
        ///</summary>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        
        ///<summary>
        // Creates a shallow copy of a range of elements in the source System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   count:
        // The number of elements in the range.
        //
        //   index:
        // The zero-based System.Collections.Generic.List<T> index at which the range
        // starts.
        //
        // Returns:
        // A shallow copy of a range of elements in the source System.Collections.Generic.List<T>.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        // index and count do not denote a valid range of elements in the System.Collections.Generic.List<T>.
        ///</summary>
        public List<T> GetRange(int index, int count)
        {
            return _list.GetRange(index, count);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // first occurrence within the entire System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        // Returns:
        // The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List<T>,
        ///</summary>
        // if found; otherwise, –1.
        public virtual int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // first occurrence within the range of elements in the System.Collections.Generic.List<T>
        // that extends from the specified index to the last element.
        //
        // Parameters:
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        //   index:
        // The zero-based starting index of the search.
        //
        // Returns:
        // The zero-based index of the first occurrence of item within the range of
        // elements in the System.Collections.Generic.List<T> that extends from index
        // to the last element, if found; otherwise, –1.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is outside the range of valid indexes for the System.Collections.Generic.List<T>.
        ///</summary>
        public int IndexOf(T item, int index)
        {
            return _list.IndexOf(item, index);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // first occurrence within the range of elements in the System.Collections.Generic.List<T>
        // that starts at the specified index and contains the specified number of elements.
        //
        // Parameters:
        //   count:
        // The number of elements in the section to search.
        //
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        //   index:
        // The zero-based starting index of the search.
        //
        // Returns:
        // The zero-based index of the first occurrence of item within the range of
        // elements in the System.Collections.Generic.List<T> that starts at index and
        // contains count number of elements, if found; otherwise, –1.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is outside the range of valid indexes for the System.Collections.Generic.List<T>.-or-count
        // is less than 0.-or-index and count do not specify a valid section in the
        // System.Collections.Generic.List<T>.
        ///</summary>
        public int IndexOf(T item, int index, int count)
        {
            return _list.IndexOf(item, index, count);
        }
        
        ///<summary>
        // Inserts an element into the System.Collections.Generic.List<T> at the specified
        // index.
        //
        // Parameters:
        //   item:
        // The object to insert. The value can be null for reference types.
        //
        //   index:
        // The zero-based index at which item should be inserted.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-index is greater than System.Collections.Generic.List<T>.Count.
        ///</summary>
        public void Insert(int index, T item)
        {
            Enlist();
            _list.Insert(index, item);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Inserts the elements of a collection into the System.Collections.Generic.List<T>
        // at the specified index.
        //
        // Parameters:
        //   collection:
        // The collection whose elements should be inserted into the System.Collections.Generic.List<T>.
        // The collection itself cannot be null, but it can contain elements that are
        // null, if type T is a reference type.
        //
        //   index:
        // The zero-based index at which the new elements should be inserted.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-index is greater than System.Collections.Generic.List<T>.Count.
        //
        //   System.ArgumentNullException:
        // collection is null.
        ///</summary>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            Enlist();
            _list.InsertRange(index, collection);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // last occurrence within the entire System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        // Returns:
        // The zero-based index of the last occurrence of item within the entire the
        // System.Collections.Generic.List<T>, if found; otherwise, –1.
        ///</summary>
        public int LastIndexOf(T item)
        {
            return _list.LastIndexOf(item);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // last occurrence within the range of elements in the System.Collections.Generic.List<T>
        // that extends from the first element to the specified index.
        //
        // Parameters:
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        //   index:
        // The zero-based starting index of the backward search.
        //
        // Returns:
        // The zero-based index of the last occurrence of item within the range of elements
        // in the System.Collections.Generic.List<T> that extends from the first element
        // to index, if found; otherwise, –1.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is outside the range of valid indexes for the System.Collections.Generic.List<T>.
        ///</summary>
        public int LastIndexOf(T item, int index)
        {
            return _list.LastIndexOf(item, index);
        }
        
        ///<summary>
        // Searches for the specified object and returns the zero-based index of the
        // last occurrence within the range of elements in the System.Collections.Generic.List<T>
        // that contains the specified number of elements and ends at the specified
        // index.
        //
        // Parameters:
        //   count:
        // The number of elements in the section to search.
        //
        //   item:
        // The object to locate in the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        //   index:
        // The zero-based starting index of the backward search.
        //
        // Returns:
        // The zero-based index of the last occurrence of item within the range of elements
        // in the System.Collections.Generic.List<T> that contains count number of elements
        // and ends at index, if found; otherwise, –1.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is outside the range of valid indexes for the System.Collections.Generic.List<T>.-or-count
        // is less than 0.-or-index and count do not specify a valid section in the
        // System.Collections.Generic.List<T>.
        ///</summary>
        public int LastIndexOf(T item, int index, int count)
        {
            return _list.LastIndexOf(item, index, count);
        }
        
        ///<summary>
        // Removes the first occurrence of a specific object from the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   item:
        // The object to remove from the System.Collections.Generic.List<T>. The value
        // can be null for reference types.
        //
        // Returns:
        // true if item is successfully removed; otherwise, false.  This method also
        // returns false if item was not found in the System.Collections.Generic.List<T>.
        ///</summary>
        public bool Remove(T item)
        {
            Enlist();
            var success = _list.Remove(item);
            OnPropertyChanged();
            return success;
        }
        
        ///<summary>
        // Removes the all the elements that match the conditions defined by the specified
        // predicate.
        //
        // Parameters:
        //   match:
        // The System.Predicate<T> delegate that defines the conditions of the elements
        // to remove.
        //
        // Returns:
        // The number of elements removed from the System.Collections.Generic.List<T>
        // .
        //
        // Exceptions:
        //   System.ArgumentNullException:
        // match is null.
        ///</summary>
        public int RemoveAll(Predicate<T> match)
        {
            Enlist();
            var success = _list.RemoveAll(match);
            OnPropertyChanged();
            return success;
        }
        
        ///<summary>
        // Removes the element at the specified index of the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   index:
        // The zero-based index of the element to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<T>.Count.
        ///</summary>
        public void RemoveAt(int index)
        {
            Enlist();
            _list.RemoveAt(index);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Removes a range of elements from the System.Collections.Generic.List<T>.
        //
        // Parameters:
        //   count:
        // The number of elements to remove.
        //
        //   index:
        // The zero-based starting index of the range of elements to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        // index and count do not denote a valid range of elements in the System.Collections.Generic.List<T>.
        ///</summary>
        public void RemoveRange(int index, int count)
        {
            Enlist();
            _list.RemoveRange(index, count);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Reverses the order of the elements in the entire System.Collections.Generic.List<T>.
        ///</summary>
        public void Reverse()
        {
            Enlist();
            _list.Reverse(); 
            OnPropertyChanged();
        }
        
        ///<summary>
        // Reverses the order of the elements in the specified range.
        //
        // Parameters:
        //   count:
        // The number of elements in the range to reverse.
        //
        //   index:
        // The zero-based starting index of the range to reverse.
        //
        // Exceptions:
        //   System.ArgumentException:
        // index and count do not denote a valid range of elements in the System.Collections.Generic.List<T>.
        //
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-count is less than 0.
        ///</summary>
        public void Reverse(int index, int count)
        {
            Enlist();
            _list.Reverse(index, count);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Sorts the elements in the entire System.Collections.Generic.List<T> using
        // the default comparer.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        // The default comparer System.Collections.Generic.Comparer<T>.Default cannot
        // find an implementation of the System.IComparable<T> generic interface or
        // the System.IComparable interface for type T.
        ///</summary>
        public void Sort()
        {
            Enlist();
            _list.Sort();
            OnPropertyChanged();
        }
        
        ///<summary>
        // Sorts the elements in the entire System.Collections.Generic.List<T> using
        // the specified System.Comparison<T>.
        //
        // Parameters:
        //   comparison:
        // The System.Comparison<T> to use when comparing elements.
        //
        // Exceptions:
        //   System.ArgumentException:
        // The implementation of comparison caused an error during the sort. For example,
        // comparison might not return 0 when comparing an item with itself.
        //
        //   System.ArgumentNullException:
        // comparison is null.
        ///</summary>
        public void Sort(Comparison<T> comparison)
        {
            Enlist();
            _list.Sort(comparison);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Sorts the elements in the entire System.Collections.Generic.List<T> using
        // the specified comparer.
        //
        // Parameters:
        //   comparer:
        // The System.Collections.Generic.IComparer<T> implementation to use when comparing
        // elements, or null to use the default comparer System.Collections.Generic.Comparer<T>.Default.
        //
        // Exceptions:
        //   System.ArgumentException:
        // The implementation of comparer caused an error during the sort. For example,
        // comparer might not return 0 when comparing an item with itself.
        //
        //   System.InvalidOperationException:
        // comparer is null, and the default comparer System.Collections.Generic.Comparer<T>.Default
        // cannot find implementation of the System.IComparable<T> generic interface
        // or the System.IComparable interface for type T.
        ///</summary>
        public void Sort(IComparer<T> comparer)
        {
            Enlist();
            _list.Sort(comparer);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Sorts the elements in a range of elements in System.Collections.Generic.List<T>
        // using the specified comparer.
        //
        // Parameters:
        //   count:
        // The length of the range to sort.
        //
        //   index:
        // The zero-based starting index of the range to sort.
        //
        //   comparer:
        // The System.Collections.Generic.IComparer<T> implementation to use when comparing
        // elements, or null to use the default comparer System.Collections.Generic.Comparer<T>.Default.
        //
        // Exceptions:
        //   System.ArgumentException:
        // index and count do not specify a valid range in the System.Collections.Generic.List<T>.-or-The
        // implementation of comparer caused an error during the sort. For example,
        // comparer might not return 0 when comparing an item with itself.
        //
        //   System.ArgumentOutOfRangeException:
        // index is less than 0.-or-count is less than 0.
        //
        //   System.InvalidOperationException:
        // comparer is null, and the default comparer System.Collections.Generic.Comparer<T>.Default
        // cannot find implementation of the System.IComparable<T> generic interface
        // or the System.IComparable interface for type T.
        ///</summary>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            Enlist();
            _list.Sort(index, count, comparer);
            OnPropertyChanged();
        }
        
        ///<summary>
        // Copies the elements of the System.Collections.Generic.List<T> to a new array.
        //
        // Returns:
        // An array containing copies of the elements of the System.Collections.Generic.List<T>.
        ///</summary>
        public T[] ToArray()
        {
            return _list.ToArray();
        }
        
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return ((ICollection<T>)_list).IsReadOnly;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        
        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return _list.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_list).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_list).SyncRoot; }
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            Enlist();
            return ((IList)_list).Add((T)value);
            //return l
        }

        void IList.Clear()
        {
            Enlist(false);
            ((IList)_list).Clear();            
        }

        bool IList.Contains(object value)
        {
            return ((IList)_list).Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)_list).IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            Enlist();
            ((IList)_list).Insert(index, (T)value);
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)_list).IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList)_list).IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            Enlist();
            ((IList)_list).Remove((T)value);
        }

        void IList.RemoveAt(int index)
        {
            Enlist();
            _list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                Enlist();
                _list[index] = (T)value;
            }
        }

        #endregion

		#region IChangedNotification Members

		void IChangedNotification.OnChanged(CommandDoneType type, IChange change)
		{
			UndoRedoMemberExtender.OnChanged(this, type, new ReadOnlyCollection<T>((IList<T>) change.NewObject), new ReadOnlyCollection<T>((IList<T>) change.OldObject));
		}

		#endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
