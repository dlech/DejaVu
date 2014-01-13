// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Threading;
using NUnit.Framework;

namespace DejaVu.UnitTests.UndoRedoAreaTests
{
	[TestFixture]
	public class GeneralTests
	{
		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Interference_1AreaIn1Thread()
		{
			var area1 = new UndoRedoArea("area1");

			using (area1.Start("Command1"))
			{
				area1.Start("Command2");
			}
		}

		[Test]
		public void Interference_2AreasIn1Thread()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");

			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);
			using (area1.Start("Command1"))
			{
				i1.Value = 1;
				using (area2.Start("Command1"))
				{
					i2.Value = 1;
					Assert.AreEqual(i1.Value, 1);
					Assert.AreEqual(i2.Value, 1);
				}
				area1.Commit();
			}
			Assert.AreEqual(i1.Value, 1);
			Assert.AreEqual(i2.Value, 0);
		}

        [Test]
        public void Interference_1AreaIn2Threads()
        {
            var area1 = new UndoRedoArea("area1");
            Thread t1 = new Thread(delegate()
            {
                area1.Start("Command1");
                Thread.Sleep(1000);
            });
            t1.Start();
            Thread.Sleep(500);
            area1.Start("Command2");
        }

		[Test]
		public void SequenceOf2Areas()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");
			
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);

			using (area1.Start("command1"))
			{
				i1.Value = 1;
				area1.Commit();
			}

			using (area2.Start("command2"))
			{
				i2.Value = 2;
				area2.Commit();
			}

			Assert.AreEqual(1, i1.Value, "value from area1 is wrong");
			Assert.AreEqual(2, i2.Value, "value from area2 is wrong");

			area1.Undo();
			area2.Undo();

			Assert.AreEqual(0, i1.Value, "value from area1 is wrong");
			Assert.AreEqual(0, i2.Value, "value from area2 is wrong");

			area1.Redo();
			area2.Redo();

			Assert.AreEqual(1, i1.Value, "value from area1 is wrong");
			Assert.AreEqual(2, i2.Value, "value from area2 is wrong");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WrongCommit()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");

			using (area1.Start("command1"))
			{
				area2.Commit();
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WrongCancel()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");

			using (area1.Start("command1"))
			{
				area2.Cancel();
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WrongUndo()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");

			var i = new UndoRedo<int>(0);
			using (area1.Start("command1"))
			{
				i.Value = 1;
				area1.Commit();
			}

			using (area2.Start("command2"))
			{
				area1.Undo();
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WrongRedo()
		{
			var area1 = new UndoRedoArea("area1");
			var area2 = new UndoRedoArea("area2");

			var i = new UndoRedo<int>(0);
			using (area1.Start("command1"))
			{
				i.Value = 1;
				area1.Commit();
			}

			using (area2.Start("command2"))
			{
				area1.Redo();
			}
		}
	}
}
