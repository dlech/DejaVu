// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Collections.Generic;

namespace DejaVu
{
    class Command : IDisposable
    {
		readonly UndoRedoArea parentArea;
        public readonly string Caption;
		internal readonly bool Visible;
		Dictionary<IUndoRedoMember, IChange> changes;

		public Command(string caption, UndoRedoArea parentArea, bool visible)
        {
            Caption = caption;
			this.parentArea = parentArea;
			this.Visible = visible;
			changes = new Dictionary<IUndoRedoMember, IChange>();
        }

		public bool IsEnlisted(IUndoRedoMember member)
		{
			// if command suspended, it will always return true to prevent changes registration
			return changes.ContainsKey(member);
		}

		public IChange this[IUndoRedoMember member]
		{
			get 
			{
				return changes[member];
			}
			set 
			{
				changes[member] = value;
			}
			
		}
		internal bool Finished = false;
		internal void Commit()
		{
			foreach (IUndoRedoMember member in changes.Keys)
				member.OnCommit(changes[member]);
			Finished = true;
		}
		internal void Undo()
		{
			foreach (IUndoRedoMember member in changes.Keys)
				member.OnUndo(changes[member]);

			Finished = true;
		}
		internal void Redo()
		{
			foreach (IUndoRedoMember member in changes.Keys)
				member.OnRedo(changes[member]);
		}

		internal void NotifyOnChanges(CommandDoneType commandType)
		{
			foreach (IUndoRedoMember member in changes.Keys)
				if (member is IChangedNotification)
					((IChangedNotification)member).OnChanged(commandType, changes[member]);
		}

		public bool HasChanges
		{
			get { return changes.Count > 0; }
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (!Finished && parentArea != null)
			{
				if (parentArea.CurrentCommand == this)
					parentArea.Cancel();
				else
				{
					if (parentArea.CurrentCommand != null)
						throw new InvalidOperationException("Command '" + parentArea.CurrentCommand.Caption + "' was not commited/canceled within parent command '" + Caption + "'.");
					else
						throw new InvalidOperationException();
				}
			}
		}

		#endregion

		internal void Merge(Command mergedCommand)
		{
			foreach (IUndoRedoMember member in mergedCommand.changes.Keys)
			{
				if (changes.ContainsKey(member))
				{
					changes[member].NewObject = mergedCommand.changes[member].NewObject;
				}
				else
				{
					changes[member] = mergedCommand.changes[member];
				}
			}
		}
	}
}
