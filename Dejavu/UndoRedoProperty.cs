// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Diagnostics;

namespace DejaVu
{
    [DebuggerDisplay("{Value}")]
	public class UndoRedo<TValue> : IUndoRedoMember, IChangedNotification
    {
        public UndoRedo()
        {
            _tValue = default(TValue);
        }
        public UndoRedo(TValue defaultValue)
        {
            _tValue = defaultValue;
        }

        TValue _tValue;
        public TValue Value
        {
            get { return _tValue; }
            set 
            {
				UndoRedoArea.AssertCommand();
				Command command = UndoRedoArea.CurrentArea.CurrentCommand;
				if (!command.IsEnlisted(this))
				{
					var change = new Change<TValue>();
					change.OldState = _tValue;
					command[this] = change;
				}
				_tValue = value;
            }
        }

        #region IUndoRedoMember Members

        void IUndoRedoMember.OnCommit(object change)
        {
            Debug.Assert(change != null);
            ((Change<TValue>)change).NewState = _tValue;
        }

        void IUndoRedoMember.OnUndo(object change)
        {
            Debug.Assert(change != null);
            _tValue = ((Change<TValue>)change).OldState;
        }

        void IUndoRedoMember.OnRedo(object change)
        {
            Debug.Assert(change != null);
            _tValue = ((Change<TValue>)change).NewState;
        }
		// properties Owner, Name and event Changed are implemented on extender pattern but do not require .NET 3.0 
		// implementation optimised in favor of memory consumption sacrificing performance a little
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

		public event EventHandler<MemberChangedEventArgs> Changed
		{
			add { UndoRedoMemberExtender.SubscribeChanges(this, value); }
			remove { UndoRedoMemberExtender.UnsubscribeChanges(this, value); }
		}

		#endregion

		#region IChangedNotification Members

		void IChangedNotification.OnChanged(CommandDoneType type, IChange change)
		{
			UndoRedoMemberExtender.OnChanged(this, type, change.NewObject, change.OldObject);
		}

		#endregion
	}
}
