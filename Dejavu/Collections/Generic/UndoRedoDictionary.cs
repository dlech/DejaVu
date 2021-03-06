// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Collections.Generic;

namespace DejaVu.Collections.Generic
{
    public class UndoRedoDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IUndoRedoMember, IChangedNotification
    {
        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        /// class that is empty, has the default initial capacity, and uses the default
        /// equality comparer for the key type.
        /// </summary>
        public UndoRedoDictionary() 
        {
        }
        
        /// <summary>
        //     Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        //     class that contains elements copied from the specified System.Collections.Generic.IDictionary<TKey,TValue>
        //     and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">
        /// The System.Collections.Generic.IDictionary<TKey,TValue> whose elements are
        /// copied to the new System.Collections.Generic.Dictionary<TKey,TValue>.
        /// </param>
        public UndoRedoDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {}

        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        ///     class that is empty, has the default initial capacity, and uses the specified
        ///     System.Collections.Generic.IEqualityComparer<T>.
        /// </summary>
        /// <param name="comparer">
        /// The System.Collections.Generic.IEqualityComparer<T> implementation to use
        /// when comparing keys, or null to use the default System.Collections.Generic.EqualityComparer<T>
        /// for the type of the key.
        /// </param>
        public UndoRedoDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        { }
        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        /// class that is empty, has the specified initial capacity, and uses the default
        /// equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the System.Collections.Generic.Dictionary<TKey,TValue> can contain.</param>
        public UndoRedoDictionary(int capacity) : base(capacity)
        {}
        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        /// class that contains elements copied from the specified System.Collections.Generic.IDictionary<TKey,TValue>
        /// and uses the specified System.Collections.Generic.IEqualityComparer<T>.
        /// </summary>
        /// <param name="dictionary">The System.Collections.Generic.IDictionary<TKey,TValue> whose elements are copied to the new System.Collections.Generic.Dictionary<TKey,TValue>.</param>
        /// <param name="comparer">The System.Collections.Generic.IEqualityComparer<T> implementation to use when comparing keys, or null to use the default System.Collections.Generic.EqualityComparer<T> for the type of the key.</param>
        public UndoRedoDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {}
        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.Dictionary<TKey,TValue>
        /// class that is empty, has the specified initial capacity, and uses the specified
        /// System.Collections.Generic.IEqualityComparer<T>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the System.Collections.Generic.Dictionary<TKey,TValue> can contain.</param>
        /// <param name="comparer">The System.Collections.Generic.IEqualityComparer<T> implementation to use when comparing keys, or null to use the default System.Collections.Generic.EqualityComparer<T> for the type of the key.</param>
        public UndoRedoDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {}

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation creates a new element with the specified key.</returns>
        public new TValue this[TKey key] 
        {
            get { return base[key];  }
            set 
            {
                if (key != null)
                {
                    var changes = Enlist();
					if (changes != null)
					{
						if (!ContainsKey(key))
						{
							changes.Add(
								delegate { base[key] = value; },
								delegate { base.Remove(key); });
						}
						else
						{
							var oldValue = base[key];
							changes.Add(
								delegate { base[key] = value; },
								delegate { base[key] = oldValue; });
						}
					}
                }
                base[key] = value;
            }
        }

        /// <summary>Adds the specified key and value to the dictionary.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public new void Add(TKey key, TValue value)
        {
            if (key != null && !ContainsKey(key))
            {
                var changes = Enlist();
				if (changes != null)
					changes.Add(
					    () => base.Add(key, value),
					    () => base.Remove(key));
            }
            base.Add(key, value);
        }
        /// <summary>
        /// Removes all keys and values from the System.Collections.Generic.Dictionary<TKey,TValue>.
        /// </summary>
        public new void Clear() 
        {
            var changes = Enlist();
			if (changes != null)
			{
				var copy = new Dictionary<TKey, TValue>(this);
				changes.Add(
				    () => base.Clear(),
						delegate { foreach (var key in copy.Keys) { base.Add(key, copy[key]); } });
			}
            base.Clear();
        }

        /// <summary>
        /// Removes the value with the specified key from the System.Collections.Generic.Dictionary<TKey,TValue>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the System.Collections.Generic.Dictionary<TKey,TValue>.</returns>
        public new bool Remove(TKey key)
        {
            TValue value;
            if (base.TryGetValue(key, out value))
            {
                var changes = Enlist();
				if (changes != null)
					changes.Add(
					    () => base.Remove(key),
					    () => base.Add(key, value));

                return base.Remove(key);
            }
            else
                return false;
        }

        delegate void OperationInvoker();

		class ChangesList : Change<List<OperationInvoker>>
		{
			public ChangesList()
			{
				NewState = new List<OperationInvoker>();
				OldState = new List<OperationInvoker>();
			}
			public void Add(OperationInvoker doChange, OperationInvoker undoChange)
			{
				OldState.Add(undoChange);
				NewState.Add(doChange);
			}
		}

        ChangesList Enlist()
        {
			UndoRedoArea.AssertCommand();
			var command = UndoRedoArea.CurrentArea.CurrentCommand;
			if (!command.IsEnlisted(this))
			{
				var changes = new ChangesList();
				command[this] = changes;
				return changes;
			}
			else
				return (ChangesList)command[this];
        }

        #region IUndoRedoMember Members

        void IUndoRedoMember.OnCommit(object change)
        {}

        void IUndoRedoMember.OnUndo(object change)
        {
            var changesList = (ChangesList)change;
            for (var i = changesList.OldState.Count - 1; i >= 0; i--)
                changesList.OldState[i]();
        }

        void IUndoRedoMember.OnRedo(object change)
        {
			var changesList = (ChangesList)change;
			for (var i = 0; i <= changesList.NewState.Count - 1; i++)
				changesList.NewState[i]();
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
		/// This event is fired if the dictionary was changed during a command. 
		/// MemberChangedEventArgs.NewValue contains int count of how many times dictionary was changed.
		/// MemberChangedEventArgs.OldValue contains the same value.
		/// </summary>
		public event EventHandler<MemberChangedEventArgs> Changed
		{
			add { UndoRedoMemberExtender.SubscribeChanges(this, value); }
			remove { UndoRedoMemberExtender.UnsubscribeChanges(this, value); }
		}

        #endregion

		#region IChangedNotification Members

		void IChangedNotification.OnChanged(CommandDoneType type, IChange change)
		{
			UndoRedoMemberExtender.OnChanged(this, type, ((ChangesList)change).NewState.Count, ((ChangesList)change).OldState.Count);
		}

		#endregion
	}
}
