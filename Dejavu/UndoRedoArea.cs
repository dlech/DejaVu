// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DejaVu
{
	/// <summary>
	/// Provides undo/redo commands with isolation. 
	/// Changes made during a command are isolated inside the given area.
	/// </summary>
	/// <remarks>
	/// Developer is responsinle to guarantie that every data instance is changed always in same area.
	/// Data consistency can be unpredictably tampered if an instance is changed in one area but then changed in another one.
	/// </remarks>
	[DebuggerDisplay("{Name}")]
	public class UndoRedoArea
	{
		/// <summary>This field serves primarily for debugging purposes</summary>
		public readonly string Name;
		object affinityOwner;
		Log log = new Log(UndoRedoManager.MaxLogSize);

		/// <summary>
		/// Initializes new area
		/// </summary>
		/// <param name="name">Name of the area (for debugging and tracing purposes)</param>
		public UndoRedoArea(string name)
		{
			Name = name;
			commandsStack.Push(null);
		}
		[ThreadStatic]
		private static UndoRedoArea currentArea = null;
		internal static UndoRedoArea CurrentArea
		{
			get { return currentArea; }
		}

		private List<Command> history = new List<Command>();
		private int currentPosition = -1;

		Command _currentCommand; // this field made for performance reasons to eliminate excessive access to commands stack
		internal Command CurrentCommand
		{
			get 
			{
				// no check required. stack always contains at least one item (that is always null especially to eliminate excessive count checks).
				return _currentCommand; 
			}
		}

		private Stack<Command> commandsStack = new Stack<Command>();
		private void PushCommand(Command command)
		{
			commandsStack.Push(command);
			_currentCommand = command;
		}

		private Command PopCommand()
		{
			if (_currentCommand != null)
			{
				Command command = commandsStack.Pop();
				_currentCommand = commandsStack.Peek();
				return command;
			}
			else
			{
				Debug.Fail("Commands stack is empty");
				return null;
			}
		}
		private void ClearCommands()
		{
			while ((_currentCommand = commandsStack.Pop()) != null)
			{
				_currentCommand.Finished = true;
			}
			commandsStack.Push(null);
		}

		private bool IsAnyParentCommand()
		{
			return _currentCommand != null;
		}

		#region Undo/Redo stuff
		/// <summary>Returns true if history has command that can be undone</summary>
		public bool CanUndo
		{
			get { return currentPosition >= 0; }
		}
		/// <summary>Returns true if history has command that can be redone</summary>
		public bool CanRedo
		{
			get { return currentPosition < history.Count - 1; }
		}
		/// <summary>Undo last command from history list</summary>
		public void Undo()
		{
			AssertNoCommand();
			if (CanUndo)
			{
				affinityOwner = null;
				Command command = history[currentPosition--];
				log.Add("[Undo '" + command.Caption + "']");
				command.Undo();
				
				OnCommandDone(command, CommandDoneType.Undo);
			}
		}
		/// <summary>Repeats command that was undone before</summary>
		public void Redo()
		{
			AssertNoCommand();
			if (CanRedo)
			{
				affinityOwner = null;
				Command command = history[++currentPosition];
				log.Add("[Redo '" + command.Caption + "']");
				command.Redo();
				
				OnCommandDone(command, CommandDoneType.Redo);
			}
		}
		#endregion

		/// <summary>Start a command. Any data changes must be done within a command.</summary>
		/// <param name="commandCaption"></param>
		/// <returns>Interface that allows properly finish the command with 'using' statement</returns>
		public IDisposable Start(string commandCaption)
		{
			this.affinityOwner = null;
			log.Add("'" + commandCaption + "'");
			return Start(commandCaption, true);
		}
		/// <summary>
		/// Start a command with affinity checking. 
		/// If several commands with equal captions and owners follow each other, 
		/// they are affined and will be merged into single command.
		/// This method is useful when you want a bunch of similar routine actions looks like a single command. 
		/// E.g. user moves a rectangle 10 times and then sees one Move command in the undo list.
		/// </summary>
		/// <param name="commandCaption">Caption of the command</param>
		/// <param name="owner">
		/// Owner is used as an identifier to check affinity of commands. Any object can be an owner. 
		/// If command has no owner (null), it never has affinity with any other command.
		/// </param>
		/// <returns>Interface that allows properly finish the command with 'using' statement</returns>
		public IDisposable Start(string commandCaption, object owner)
		{
			if (owner == this.affinityOwner && // owners are equal
				owner != null && // owners are not null
				currentPosition >= 0 && // history has a command to check affinity
				history[currentPosition].Caption == commandCaption) // captions are equal
			{
				log.Add("'" + commandCaption + "' (affined)");
				return Start(commandCaption, false);
			}
			else
			{
				this.affinityOwner = owner;
				log.Add("'" + commandCaption + "'");
				return Start(commandCaption, true);
			}
		}
		/// <summary>
		/// Start invisible command. 
		/// Any data changes must be done within a command. 
		/// This command will never appear in the history. 
		/// It will be undone/redone in bundle with previous visible command.</summary>
		/// <param name="commandCaption">Caption of invisible command. Serves for tracking purposes only.</param>
		/// <remarks><para>
		/// Invisible commands are useful if you need to do some changes by some event 
		/// but do not expose them to user as a standalone command. </para>
		/// <para>For example, when user clicks on object, we could change SelectedObject property.
		/// However, it is redundant to show this operation in history and allow to undo/redo it as a valuable command.
		/// Instead of that, we can start invisible command and its results will be joined to previous command. 
		/// Thus, when the previuos command will be undone, the selection will be undone too.
		/// </remarks>
		/// <returns>Interface that allows properly finish the command with 'using' statement</returns>
		public IDisposable StartInvisible(string commandCaption)
		{
			log.Add("'" + commandCaption + "' (invisible)");
			return Start(commandCaption, false);
		}
		private IDisposable Start(string commandCaption, bool visible)
		{
			currentArea = this;
			Command command = new Command(commandCaption, this, visible);
			PushCommand(command);
			return command;
		}
		/// <summary>Commits current command and saves changes into history</summary>
		public void Commit()
		{
			AssertCurrentCommand();
			log.Add("    [Commit]");

			Command commitedCommand = PopCommand();
			if (commitedCommand.HasChanges)
			{
				commitedCommand.Commit();

				if (IsAnyParentCommand())
				{	// this is nested command - merge with parent
					CurrentCommand.Merge(commitedCommand);
				}
				else 
				{	// put command into the history
					// remove all redo records
					int count = history.Count - currentPosition - 1;
					history.RemoveRange(currentPosition + 1, count);

					// add command to history 
					if (commitedCommand.Visible)
					{
						history.Add(commitedCommand);
						currentPosition++;
						TruncateHistory();
					}
					else
					{
						// merge with previous command
						if (currentPosition >= 0)
							history[currentPosition].Merge(commitedCommand);
					}

					OnCommandDone(commitedCommand, CommandDoneType.Commit);
				}
			}
		}
		/// <summary>
		/// Rollback current command. It does not saves any changes done in current command.
		/// </summary>
		public void Cancel()
		{
			AssertCurrentCommand();
			log.Add("    [Cancel]");
			Command cancelledCommand = PopCommand();
			cancelledCommand.Undo();
		}	

		/// <summary>
		/// Clears all history. It does not affect current data but history only. 
		/// It is usefull after any data initialization if you want forbid user to undo this initialization.
		/// </summary>
		public void ClearHistory()
		{
			ClearCommands();
			currentPosition = -1;
			history.Clear();
			log.Add("[Clear History]");
		}

		/// <summary>Checks that there is no command started in current thread</summary>
		internal void AssertNoCommand()
		{
			// check command in this area
			if (CurrentCommand != null)
				throw new InvalidOperationException("Previous command is not completed. Use UndoRedoManager.Commit() to complete current command.");
			// check command in area that is current now 
			if (currentArea != null && currentArea.CurrentCommand != null)
				throw new InvalidOperationException("A command of another area has already started in current thread.");
		}

		/// <summary>Checks that command had been started</summary>
		internal static void AssertCommand()
		{
			if (currentArea == null || currentArea.CurrentCommand == null)
				throw new InvalidOperationException("Command is not started. Use methods UndoRedoManager.Start() or UndoRedoArea.Start() before changing any data.");
		}
		/// <summary>Checks that command had been started in given area</summary>
		internal void AssertCurrentCommand()
		{
			if (CurrentCommand == null)
				throw new InvalidOperationException("Command in given area is not started.");
		}

		public bool IsCommandStarted
		{
			get { return CurrentCommand != null; }
		}

		#region Commands Lists
		/// <summary>Gets an enumeration of commands captions that can be undone.</summary>
		/// <remarks>The first command in the enumeration will be undone first</remarks>
		public IEnumerable<string> UndoCommands
		{
			get
			{
				for (int i = currentPosition; i >= 0; i--)
					if (history[i].Visible)
						yield return history[i].Caption;
			}
		}
		/// <summary>Gets an enumeration of commands captions that can be redone.</summary>
		/// <remarks>The first command in the enumeration will be redone first</remarks>
		public IEnumerable<string> RedoCommands
		{
			get
			{
				for (int i = currentPosition + 1; i < history.Count; i++)
					if (history[i].Visible)
						yield return history[i].Caption;
			}
		}
		#endregion

		#region History Size

		private int maxHistorySize = 0;

		/// <summary>
		/// Gets/sets max commands stored in history. 
		/// Zero value (default) sets unlimited history size.
		/// </summary>
		public int MaxHistorySize
		{
			get { return maxHistorySize; }
			set
			{
				if (IsCommandStarted)
					throw new InvalidOperationException("Max size may not be set while command is run.");
				if (value < 0)
					throw new ArgumentOutOfRangeException("Value may not be less than 0");
				maxHistorySize = value;
				TruncateHistory();
			}
		}

		private void TruncateHistory()
		{
			if (maxHistorySize > 0)
				if (history.Count > maxHistorySize)
				{
					int count = history.Count - maxHistorySize;
					history.RemoveRange(0, count);
					currentPosition -= count;
				}
		}
		#endregion

		public event EventHandler<CommandDoneEventArgs> CommandDone;
		void OnCommandDone(Command command, CommandDoneType type)
		{
			command.NotifyOnChanges(type);
			if (CommandDone != null)
				CommandDone(null, new CommandDoneEventArgs(type));
		}

		internal string GetLog()
		{
			return log.ToString();
		}

		internal void WriteLog(string message)
		{
			// add with indentation
			log.Add("    " + message);
		}

		internal void ClearLog()
		{
			log = new Log(UndoRedoManager.MaxLogSize);
		}


		/*public void Subscribe(EventHandler<MemberChangedEventArgs> handler, IUndoRedoMember member, string memberName, object memberOwner)
		{
			MemberChangedSubscription subscription;
			if (subscriptions.ContainsKey(member))
			{
				subscription = subscriptions[member];
			}
			else
			{
				subscription = new MemberChangedSubscription(memberOwner, memberName);
				subscriptions.Add(member, subscription);
			}
			subscription.Changed += handler;
		}*/		
	}

	static class UndoRedoMemberExtender
	{
		static Dictionary<IUndoRedoMember, object> owners = new Dictionary<IUndoRedoMember, object>();
		public static void SetOwner(IUndoRedoMember member, object owner)
		{
			owners[member] = owner;
		}
		public static object GetOwner(IUndoRedoMember member)
		{
			return owners[member];
		}

		static Dictionary<IUndoRedoMember, string> names = new Dictionary<IUndoRedoMember, string>();
		public static void SetName(IUndoRedoMember member, string name)
		{
			names[member] = name;
		}
		public static string GetName(IUndoRedoMember member)
		{
			return names[member];
		}

		static Dictionary<IUndoRedoMember, EventHandler<MemberChangedEventArgs>> subscriptions = new Dictionary<IUndoRedoMember, EventHandler<MemberChangedEventArgs>>();
		public static void SubscribeChanges(IUndoRedoMember member, EventHandler<MemberChangedEventArgs> handler)
		{
			EventHandler<MemberChangedEventArgs> subscription;
			if (subscriptions.ContainsKey(member))
			{
				subscription = subscriptions[member];
				subscription += handler;
			}
			else
			{
				subscription = new EventHandler<MemberChangedEventArgs>(handler);
				subscriptions.Add(member, subscription);
			}			
		}
		public static void UnsubscribeChanges(IUndoRedoMember member, EventHandler<MemberChangedEventArgs> handler)
		{
			if (subscriptions.ContainsKey(member))
			{
				EventHandler<MemberChangedEventArgs> subscription = subscriptions[member];
				subscription -= handler;
			}
		}

		internal static void OnChanged(IUndoRedoMember member, CommandDoneType commandType, object newObject, object oldObject)
		{
			if (subscriptions.ContainsKey(member))
			{
				subscriptions[member](member, new MemberChangedEventArgs(member, commandType, newObject, oldObject));
			}
		}

	}

}
