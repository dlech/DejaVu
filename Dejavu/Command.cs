// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Collections.Generic;

namespace DejaVu
{
    class Command : IDisposable
    {
		readonly UndoRedoArea _parentArea;
        public readonly string Caption;
		internal readonly bool Visible;
        readonly Dictionary<IUndoRedoMember, IChange> _changes;

		public Command(string caption, UndoRedoArea parentArea, bool visible)
        {
            Caption = caption;
			_parentArea = parentArea;
			Visible = visible;
			_changes = new Dictionary<IUndoRedoMember, IChange>();
        }

		public bool IsEnlisted(IUndoRedoMember member)
		{
			// if command suspended, it will always return true to prevent changes registration
			return _changes.ContainsKey(member);
		}

		public IChange this[IUndoRedoMember member]
		{
			get 
			{
				return _changes[member];
			}
			set 
			{
				_changes[member] = value;
			}
			
		}
		internal bool Finished = false;
		internal void Commit()
		{
			foreach (IUndoRedoMember member in _changes.Keys)
				member.OnCommit(_changes[member]);
			Finished = true;
		}
		internal void Undo()
		{
			foreach (IUndoRedoMember member in _changes.Keys)
				member.OnUndo(_changes[member]);

			Finished = true;
		}
		internal void Redo()
		{
			foreach (IUndoRedoMember member in _changes.Keys)
				member.OnRedo(_changes[member]);
		}

		internal void NotifyOnChanges(CommandDoneType commandType)
		{
			foreach (IUndoRedoMember member in _changes.Keys)
				if (member is IChangedNotification)
					((IChangedNotification)member).OnChanged(commandType, _changes[member]);
		}

		public bool HasChanges
		{
			get { return _changes.Count > 0; }
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (!Finished && _parentArea != null)
			{
				if (_parentArea.CurrentCommand == this)
					_parentArea.Cancel();
				else
				{
					if (_parentArea.CurrentCommand != null)
						throw new InvalidOperationException("Command '" + _parentArea.CurrentCommand.Caption + "' was not commited/canceled within parent command '" + Caption + "'.");
					else
						throw new InvalidOperationException();
				}
			}
		}

		#endregion

		internal void Merge(Command mergedCommand)
		{
			foreach (IUndoRedoMember member in mergedCommand._changes.Keys)
			{
				if (_changes.ContainsKey(member))
				{
					_changes[member].NewObject = mergedCommand._changes[member].NewObject;
				}
				else
				{
					_changes[member] = mergedCommand._changes[member];
				}
			}
		}
	}
}
