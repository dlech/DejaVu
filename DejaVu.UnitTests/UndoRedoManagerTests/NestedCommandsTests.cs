// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using NUnit.Framework;

namespace DejaVu.UnitTests.UndoRedoManagerTests
{
	[TestFixture]
	public class NestedCommandsTests
	{
		[TearDown]
		public void TearDown()
		{
			UndoRedoManager.Reset();
		}

		[Test]
		public void BasicScenario()
		{
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);
			var i3 = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				i2.Value = 1;
				i3.Value = 1;
				using (UndoRedoManager.Start("2"))
				{
					i2.Value = 2;
					i3.Value = 2;
					UndoRedoManager.Commit();
				}
				Assert.AreEqual(i1.Value, 1);
				Assert.AreEqual(i2.Value, 2);
				Assert.AreEqual(i3.Value, 2);

				i3.Value = 3;		

				UndoRedoManager.Commit();
			}
			Assert.AreEqual(i1.Value, 1);
			Assert.AreEqual(i2.Value, 2);
			Assert.AreEqual(i3.Value, 3);	
		}

		[Test]
		public void Commit_Cancel()
		{
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);

			// do test twice to exclude interference
			Commit_Cancel(i1, i2);
			Commit_Cancel(i1, i2);
		}

		private void Commit_Cancel(UndoRedo<int> i1, UndoRedo<int> i2)
		{
			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				i2.Value = 1;
				using (UndoRedoManager.Start("2"))
				{
					i1.Value = 2;
					UndoRedoManager.Commit();
				}
				Assert.AreEqual(i1.Value, 2);
				Assert.AreEqual(i2.Value, 1);
			}
			Assert.AreEqual(i1.Value, 0);
			Assert.AreEqual(i2.Value, 0);
		}

		[Test]
		public void CommitCancel_Commit()
		{
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				i2.Value = 1;
				using (UndoRedoManager.Start("2"))
				{
					i1.Value = 2;
					UndoRedoManager.Commit();
				}
				Assert.AreEqual(i1.Value, 2);
				Assert.AreEqual(i2.Value, 1);
				using (UndoRedoManager.Start("3"))
				{
					i1.Value = 3;
					i2.Value = 3;
					Assert.AreEqual(i1.Value, 3);
					Assert.AreEqual(i2.Value, 3);
				}
				Assert.AreEqual(i1.Value, 2);
				Assert.AreEqual(i2.Value, 1);

				UndoRedoManager.Commit();
			}
			Assert.AreEqual(i1.Value, 2);
			Assert.AreEqual(i2.Value, 1);
		}
		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void UndoAfterNestedCommit()
		{ 
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				i2.Value = 1;
				using (UndoRedoManager.Start("2"))
				{
					i1.Value = 2;
					UndoRedoManager.Commit();
				}
				UndoRedoManager.Undo();
			}
		}
		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void NoInternalCancel()
		{
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				UndoRedoManager.Start("2");
				i2.Value = 2;
			} // exception must be here 
		}

		[Test]
		public void NestedClearHistory()
		{
			var i1 = new UndoRedo<int>(0);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				UndoRedoManager.Start("2");
				UndoRedoManager.Start("3");

				UndoRedoManager.ClearHistory();
			} 
			Assert.AreEqual(i1.Value, 1);
			Assert.IsFalse(UndoRedoManager.CanRedo);
			Assert.IsFalse(UndoRedoManager.CanUndo);
		}

		[Test]
		public void DeepHierarchy()
		{
			var i1 = new UndoRedo<int>(0);
			var i2 = new UndoRedo<int>(0);
			var i3 = new UndoRedo<int>(0);
			var i4 = new UndoRedo<float>(0f);
			var i5 = new UndoRedo<float>(0f);

			using (UndoRedoManager.Start("1"))
			{
				i1.Value = 1;
				i4.Value = 5.0f;
				using (UndoRedoManager.Start("2"))
				{
					i2.Value = 2;
					using (UndoRedoManager.Start("3"))
					{
						i3.Value = 3;

						using (UndoRedoManager.Start("4.1"))
						{
							i4.Value = 4.1f;

							UndoRedoManager.Commit();
						}

						UndoRedoManager.Commit();
					}

					i4.Value = 4.0f;

					using (UndoRedoManager.Start("4.2"))
					{
						i4.Value = 4.2f;
						i5.Value = 5.2f;

						UndoRedoManager.Commit();
					}

					UndoRedoManager.Commit();
				}
				UndoRedoManager.Commit();
			}
			Assert.AreEqual(i1.Value, 1);
			Assert.AreEqual(i2.Value, 2);
			Assert.AreEqual(i3.Value, 3);
			Assert.AreEqual(i4.Value, 4.2f); 
			Assert.AreEqual(i5.Value, 5.2f);

			UndoRedoManager.Undo();

			Assert.AreEqual(i1.Value, 0);
			Assert.AreEqual(i2.Value, 0);
			Assert.AreEqual(i3.Value, 0);
			Assert.AreEqual(i4.Value, 0f);
			Assert.AreEqual(i5.Value, 0f);

			UndoRedoManager.Redo();

			Assert.AreEqual(i1.Value, 1);
			Assert.AreEqual(i2.Value, 2);
			Assert.AreEqual(i3.Value, 3);
			Assert.AreEqual(i4.Value, 4.2f);
			Assert.AreEqual(i5.Value, 5.2f);
		}
	}
}