// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Collections.Generic;
using DejaVu.Collections.Generic;
using NUnit.Framework;

namespace DejaVu.UnitTests.UndoRedoManagerTests
{
	[TestFixture]
	public class GeneralTests
	{
		[TearDown]
		public void TearDown()
		{
			UndoRedoManager.Reset();
		}

		[Test]
		public void BasicScenario()
		{
			var i = new UndoRedo<int>(0);

			UndoRedoManager.Start("");
			i.Value = 1;
			UndoRedoManager.Commit();
			Assert.AreEqual(1, i.Value);
			Assert.IsTrue(UndoRedoManager.CanUndo);
			Assert.IsFalse(UndoRedoManager.CanRedo);

			UndoRedoManager.Undo();
			Assert.AreEqual(0, i.Value);
			Assert.IsFalse(UndoRedoManager.CanUndo);
			Assert.IsTrue(UndoRedoManager.CanRedo);

			UndoRedoManager.Redo();
			Assert.AreEqual(1, i.Value);
		}

		[Test]
		public void ClearHistory()
		{
			var i = new UndoRedo<int>(0);
			
			// start + commit + flush
			UndoRedoManager.Start("");
			i.Value = 1;
			UndoRedoManager.Commit();
			Assert.AreEqual(1, i.Value);

			UndoRedoManager.ClearHistory();
			Assert.IsFalse(UndoRedoManager.CanUndo); // history must be empty
			Assert.AreEqual(1, i.Value); // data must be intact
			
			// start + flush
			UndoRedoManager.Start("");
			i.Value = 2;
			UndoRedoManager.ClearHistory();
			Assert.IsFalse(UndoRedoManager.CanUndo); // history must be empty
			Assert.AreEqual(2, i.Value); // data must be intact
		}

		[Test]
		public void ManualCancel()
		{
			var i = new UndoRedo<int>(0);
			var list = new UndoRedoList<int>(new int[] {1, 2, 3});
			var dict = new UndoRedoDictionary<int, string>();

			UndoRedoManager.Start("");
			i.Value = 1;
			list.Add(4);
			dict[1] = "One";
			UndoRedoManager.Cancel();

			Assert.AreEqual(0, i.Value);
			Assert.AreEqual(3, list.Count);
			Assert.IsFalse(dict.ContainsKey(1));

			// run next command to make sure that framework works well after cancel
			UndoRedoManager.Start("");
			i.Value = 1;
			UndoRedoManager.Commit();

			Assert.AreEqual(1, i.Value);
		}
		
		[Test]
		public void AutoCancel()
		{
			var i = new UndoRedo<int>(0);

			// "successful" scenario
			using (UndoRedoManager.Start(""))
			{
				i.Value = 1;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(1, i.Value);

			// "failed" scenario
			try
			{
				using (UndoRedoManager.Start(""))
				{
					i.Value = 2;
					throw new Exception("Some exception");
                    // this code is never reached in this scenario (compiler warning is disabled)
					#pragma warning disable
					UndoRedoManager.Commit(); 
					#pragma warning restore
				}
			}
			catch { }
			Assert.AreEqual(1, i.Value);
		}

		[Test]
		public void CommandsCaptions()
		{
			var i = new UndoRedo<int>(0);

			UndoRedoManager.Start("1");
			i.Value = 1;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("2");
			i.Value = 2;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("3");
			i.Value = 3;
			UndoRedoManager.Commit();
			
			UndoRedoManager.Start("4");
			i.Value = 4;
			UndoRedoManager.Commit();

			UndoRedoManager.Undo();
			UndoRedoManager.Undo();

			var undo = new List<string>(UndoRedoManager.UndoCommands);
			Assert.AreEqual(2, undo.Count);
			Assert.AreEqual("2", undo[0]);
			Assert.AreEqual("1", undo[1]);

			var redo = new List<string>(UndoRedoManager.RedoCommands);
			Assert.AreEqual(2, redo.Count);
			Assert.AreEqual("3", redo[0]);
			Assert.AreEqual("4", redo[1]);
		}

		[Test]
		public void CommandsCaptions_WithInvisible()
		{
			var i = new UndoRedo<int>(0);

			UndoRedoManager.StartInvisible("0i");
			i.Value = 100;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("1");
			i.Value = 1;
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("1i");
			i.Value = 10;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("2");
			i.Value = 2;
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("2i");
			i.Value = 20;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("3");
			i.Value = 3;
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("3i");
			i.Value = 30;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("4");
			i.Value = 4;
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("4i");
			i.Value = 40;
			UndoRedoManager.Commit();

			UndoRedoManager.Undo();
			UndoRedoManager.Undo();

			var undo = new List<string>(UndoRedoManager.UndoCommands);
			Assert.AreEqual(2, undo.Count);
			Assert.AreEqual("2", undo[0]);
			Assert.AreEqual("1", undo[1]);

			var redo = new List<string>(UndoRedoManager.RedoCommands);
			Assert.AreEqual(2, redo.Count);
			Assert.AreEqual("3", redo[0]);
			Assert.AreEqual("4", redo[1]);
		}

        [Test]
        public void HistorySize()
        {
            var i = new UndoRedo<int>(0);

            UndoRedoManager.Start("1");
            i.Value = 1;
            UndoRedoManager.Commit();

            UndoRedoManager.Start("2");
            i.Value = 2;
            UndoRedoManager.Commit();

            UndoRedoManager.Start("3");
            i.Value = 3;
            UndoRedoManager.Commit();

            UndoRedoManager.Start("4");
            i.Value = 4;
            UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("4+i1");
			i.Value = 4;
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("4+i2");
			i.Value = 4;
			UndoRedoManager.Commit();
            
            Assert.AreEqual(4, new List<string>(UndoRedoManager.UndoCommands).Count);
            
            UndoRedoManager.MaxHistorySize = 3;

            Assert.AreEqual(3, new List<string>(UndoRedoManager.UndoCommands).Count);

            UndoRedoManager.Start("5");
            i.Value = 5;
            UndoRedoManager.Commit();

            Assert.AreEqual(3, new List<string>(UndoRedoManager.UndoCommands).Count);
        }

		[Test]
		public void EmptyCommand()
		{
			var fired = false; 
			UndoRedoManager.CommandDone += delegate { fired = true; };
			var c1 = new List<string>(UndoRedoManager.UndoCommands).Count;
			UndoRedoManager.Start("Empty");
			// do nothing
			UndoRedoManager.Commit();
			var c2 = new List<string>(UndoRedoManager.UndoCommands).Count;

			Assert.AreEqual(c1, c2, "Error: Empty command was put into the commands history");
			Assert.IsFalse(fired, "Error: Event CommandDone was fired for empty command");
		}

		[Test]
		public void CommandDoneEvent()
		{
			bool? success = null;
			UndoRedoManager.CommandDone += delegate { success = UndoRedoManager.IsCommandStarted; };
			var i = new UndoRedo<int>();
			UndoRedoManager.Start("a command");
			i.Value++;
			UndoRedoManager.Commit();

			Assert.IsTrue(success.HasValue, "Error: CommandDone event was not fired");
			Assert.IsFalse(success.Value, "Error: CommandDone event was fired before changes had been commited");
		}

		[Test]
		public void InvisibleCommands()
		{
			var i = new UndoRedo<string>("");

			UndoRedoManager.StartInvisible("0i");
			i.Value = "0i";
			UndoRedoManager.Commit();

			UndoRedoManager.Start("1");
			i.Value = "1";
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("1i");
			i.Value = "1i";
			UndoRedoManager.Commit();

			UndoRedoManager.Start("2");
			i.Value = "2";
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("2i");
			i.Value = "2i";
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("2ii");
			i.Value = "2ii";
			UndoRedoManager.Commit();

			UndoRedoManager.Start("3");
			i.Value = "3";
			UndoRedoManager.Commit();

			UndoRedoManager.Start("4");
			i.Value = "4";
			UndoRedoManager.Commit();

			UndoRedoManager.StartInvisible("4i");
			i.Value = "4i";
			UndoRedoManager.Commit();

			Assert.AreEqual("4i", i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual("3", i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual("2ii", i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual("1i", i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual("0i", i.Value);

			Assert.IsFalse(UndoRedoManager.CanUndo);

			UndoRedoManager.Redo();
			Assert.AreEqual("1i", i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual("2ii", i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual("3", i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual("4i", i.Value);

			Assert.IsFalse(UndoRedoManager.CanRedo);
		}		
	}
}
