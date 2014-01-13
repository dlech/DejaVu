using System;

namespace DejaVu
{
	public enum CommandDoneType
	{
		Commit, Undo, Redo
	}

	public class CommandDoneEventArgs : EventArgs
	{
		public readonly CommandDoneType CommandDoneType;
		public CommandDoneEventArgs(CommandDoneType type)
		{
			CommandDoneType = type;
		}
	}

	public class MemberChangedEventArgs : EventArgs
	{
		public readonly CommandDoneType CommandDoneType;
		public readonly IUndoRedoMember Member;
		public readonly object NewValue;
		public readonly object OldValue;
		
		public MemberChangedEventArgs(IUndoRedoMember member, CommandDoneType type, object newValue, object oldValue)
		{
			CommandDoneType = type;
			Member = member;

			NewValue = newValue;
			OldValue = oldValue;
		}

	} 
}
