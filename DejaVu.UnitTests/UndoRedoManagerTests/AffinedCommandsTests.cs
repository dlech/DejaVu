// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using NUnit.Framework;

namespace DejaVu.UnitTests.UndoRedoManagerTests
{
	[TestFixture]
	public class AffinedCommandsTests
	{
		[TearDown]
		public void TearDown()
		{
			UndoRedoManager.Reset();
		}

		[Test]		
		public void PureAffinedSequence()
		{
			var i = new UndoRedo<int>(0);
			var owner = new object();
			
			using (UndoRedoManager.Start("MyCommand", owner))
			{
				i.Value = 1;
				UndoRedoManager.Commit();
			}
			using (UndoRedoManager.Start("MyCommand", owner))
			{
				i.Value = 2;
				UndoRedoManager.Commit();
			}
			using (UndoRedoManager.Start("MyCommand", owner))
			{
				i.Value = 3;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(0, i.Value);
			Assert.IsFalse(UndoRedoManager.CanUndo);
			Assert.IsTrue(UndoRedoManager.CanRedo);

			UndoRedoManager.Redo();
			Assert.AreEqual(3, i.Value);
			Assert.IsTrue(UndoRedoManager.CanUndo);
			Assert.IsFalse(UndoRedoManager.CanRedo);

			using (UndoRedoManager.Start("MyCommand", owner))
			{
				i.Value = 4;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(4, i.Value);
			
			UndoRedoManager.Undo();
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(0, i.Value);

			UndoRedoManager.Redo();
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Redo();
			Assert.AreEqual(4, i.Value);

			using (UndoRedoManager.Start("MyCommand", owner))
			{
				i.Value = 5;
			} // auto rollback is here

			Assert.AreEqual(4, i.Value);
		}	

		[Test]
		public void DifferentOwners()
		{
			var owner1 = new object();
			var owner2 = new object();
			var i = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("MyCommand", owner1))
			{
				i.Value = 1;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(1, i.Value);

			using (UndoRedoManager.Start("MyCommand", owner2))
			{
				i.Value = 2;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(2, i.Value);

			using (UndoRedoManager.Start("MyCommand", owner1))
			{
				i.Value = 3;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(2, i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual(1, i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual(0, i.Value);

			UndoRedoManager.Redo();
			Assert.AreEqual(1, i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual(2, i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual(3, i.Value);
		}

		[Test]
		public void DifferentCommands()
		{
			var owner = new object();
			var i = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("MyCommand 1", owner))
			{
				i.Value = 1;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(1, i.Value);

			using (UndoRedoManager.Start("MyCommand 2", owner))
			{
				i.Value = 2;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(2, i.Value);

			using (UndoRedoManager.Start("MyCommand 1", owner))
			{
				i.Value = 3;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(2, i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual(1, i.Value);
			UndoRedoManager.Undo();
			Assert.AreEqual(0, i.Value);

			UndoRedoManager.Redo();
			Assert.AreEqual(1, i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual(2, i.Value);
			UndoRedoManager.Redo();
			Assert.AreEqual(3, i.Value);
		}	
	
		[Test]
		public void StartAffinedAfterUndo()
		{
			var owner = new object();
			var i = new UndoRedo<int>(0);
			using (UndoRedoManager.Start("MyCommand 1", owner))
			{
				i.Value = 1;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(1, i.Value);
			using (UndoRedoManager.Start("MyCommand 2", owner))
			{
				i.Value = 2;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(2, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(1, i.Value);

			using (UndoRedoManager.Start("MyCommand 1", owner))
			{
				i.Value = 3;
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(3, i.Value);

			UndoRedoManager.Undo();
			Assert.AreEqual(1, i.Value);

			UndoRedoManager.Redo();
			Assert.AreEqual(3, i.Value);
		}
	}
}
