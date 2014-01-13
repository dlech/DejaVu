// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using NUnit.Framework;

namespace DejaVu.Tests.UndoRedoManagerTests
{
	[TestFixture]
	public class LoggingTests
	{
		[SetUp]
		[TearDown]
		public void TearDown()
		{
			UndoRedoManager.Reset();
		}
		[Test]
		public void EmptyCommand()
		{
			UndoRedoManager.Start("Empty");
			UndoRedoManager.Commit();

			string s = UndoRedoManager.GetLog();
			var sample = "'Empty'\r\n    [Commit]\r\n";
			Assert.AreEqual(sample, s);
		}

		[Test]		
		public void StandardFlow()
		{
			var i = new UndoRedo<int>();
			// commits
			UndoRedoManager.Start("Command 1");
			i.Value = 1;
			UndoRedoManager.Commit();

			UndoRedoManager.Start("Command 2");
			i.Value = 2;
			UndoRedoManager.Commit();
			// cancel
			UndoRedoManager.Start("Command 3");
			i.Value = 3;
			UndoRedoManager.Cancel();
			// undo
			UndoRedoManager.Undo();
			// redo
			UndoRedoManager.Redo();

			string s = UndoRedoManager.GetLog();
			var sample =
@"'Command 1'
    [Commit]
'Command 2'
    [Commit]
'Command 3'
    [Cancel]
[Undo 'Command 2']
[Redo 'Command 2']
";
			Assert.AreEqual(sample, s);
		}

		[Test]
		public void ExtendedFlow()
		{
			var i = new UndoRedo<int>();
			var o = new object();
			// clear history
			UndoRedoManager.ClearHistory();
			// first affined command
			UndoRedoManager.Start("Command 1", o);
			i.Value = 1;
			UndoRedoManager.Commit();
			// second affined command
			UndoRedoManager.Start("Command 1", o);
			i.Value = 2;
			UndoRedoManager.Commit();
			// invisible command
			UndoRedoManager.StartInvisible("Command 2");
			i.Value = 3;
			UndoRedoManager.Commit();
			string s = UndoRedoManager.GetLog();
			var sample =
@"[Clear History]
'Command 1'
    [Commit]
'Command 1' (affined)
    [Commit]
'Command 2' (invisible)
    [Commit]
";
			Assert.AreEqual(sample, s);
		}

		[Test]
		public void CustomLogging()
		{
			var i = new UndoRedo<int>();
			UndoRedoManager.Start("Command 1");
			i.Value = 1;
			UndoRedoManager.Log("i = " + i.Value);
			UndoRedoManager.Commit();

			string s = UndoRedoManager.GetLog();
			var sample =
@"'Command 1'
    i = 1
    [Commit]
";
			Assert.AreEqual(sample, s);
		}

		[Test]
		public void TruncatedLog()
		{
			var count = (int)(UndoRedoManager.MaxLogSize * 2.5f);
			for (var i = 0; i < count; i++)
			{
				UndoRedoManager.Start("Command " + i);
				UndoRedoManager.Commit();
			}
			// check total number of lines
			string s = UndoRedoManager.GetLog();
			var records = s.Split(Environment.NewLine[0]);
			Assert.AreEqual(UndoRedoManager.MaxLogSize + 2, records.Length);

			// check 1st record: it must contain number of the last record
			var t = count * 2 - UndoRedoManager.MaxLogSize;
			Assert.IsTrue(records[0].Contains("#" + t + "..."));
		}

		
	}
}
