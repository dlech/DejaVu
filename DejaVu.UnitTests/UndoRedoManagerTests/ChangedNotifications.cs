using System;
using System.Collections.Generic;
using DejaVu.Collections.Generic;
using NUnit.Framework;

namespace DejaVu.UnitTests.UndoRedoManagerTests
{
	[TestFixture]
	public class ChangedNotifications
	{
		[TearDown]
		public void TearDown()
		{
			UndoRedoManager.Reset();
		}
		[Test]
		public void BasicFlow_CommitUndoRedo()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;

				Assert.IsFalse(data.prop1Changed);
				Assert.IsFalse(data.prop2Changed);

				UndoRedoManager.Commit();

				Assert.IsTrue(data.prop1Changed);
				Assert.IsFalse(data.prop2Changed);
			}

			data.prop1Changed = false;
			data.prop2Changed = false;

			UndoRedoManager.Undo();

			Assert.IsTrue(data.prop1Changed);
			Assert.IsFalse(data.prop2Changed);

			data.prop1Changed = false;
			data.prop2Changed = false;

			UndoRedoManager.Redo();

			Assert.IsTrue(data.prop1Changed);
			Assert.IsFalse(data.prop2Changed);
		}

		[Test]
		public void BasicFlow_Cancel()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;

				Assert.IsFalse(data.prop1Changed);
				Assert.IsFalse(data.prop2Changed);
			}

			Assert.IsFalse(data.prop1Changed);
			Assert.IsFalse(data.prop2Changed);
		}

		[Test]
		public void Nested_Commit()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;

				using (UndoRedoManager.Start("2"))
				{
					data.Prop1 = 2;

					using (UndoRedoManager.Start("3"))
					{
						data.Prop1 = 3;
						data.Prop2 = 3;

						Assert.IsFalse(data.prop1Changed);
						Assert.IsFalse(data.prop2Changed);
						
						UndoRedoManager.Commit();
					}
					Assert.IsFalse(data.prop1Changed);
					Assert.IsFalse(data.prop2Changed);
					
					UndoRedoManager.Commit();
				}

				Assert.IsFalse(data.prop1Changed);
				Assert.IsFalse(data.prop2Changed);
				
				UndoRedoManager.Commit();
			}

			Assert.IsTrue(data.prop1Changed);
			Assert.IsTrue(data.prop2Changed);
		}

		[Test]
		public void Nested_Cancel()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;

				using (UndoRedoManager.Start("2"))
				{
					data.Prop1 = 2;

					using (UndoRedoManager.Start("3"))
					{
						data.Prop1 = 3;
						data.Prop2 = 3;

						Assert.IsFalse(data.prop1Changed);
						Assert.IsFalse(data.prop2Changed);

						UndoRedoManager.Cancel();
					}
					Assert.IsFalse(data.prop1Changed);
					Assert.IsFalse(data.prop2Changed);

					UndoRedoManager.Commit();
				}

				Assert.IsFalse(data.prop1Changed);
				Assert.IsFalse(data.prop2Changed);

				UndoRedoManager.Commit();
			}

			Assert.IsTrue(data.prop1Changed);
			Assert.IsFalse(data.prop2Changed);
		}

		[Test]
		public void EventPostponedTillDataMadeConsistent_Undo()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;
				data.Prop2 = 1;
				
				UndoRedoManager.Commit();
			}

			UndoRedoManager.CommandDone += delegate(object s, CommandDoneEventArgs ea)
			{
				Assert.AreEqual(ea.CommandDoneType, CommandDoneType.Undo);

				Assert.AreEqual(data.Prop1, 0);
				Assert.AreEqual(data.Prop2, 0);
			};

			UndoRedoManager.Undo();
		}

		[Test]
		public void EventPostponedTillDataMadeConsistent_Redo()
		{
			var data = new DataClass_Basic();

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;
				data.Prop2 = 1;

				UndoRedoManager.Commit();
			}

			UndoRedoManager.Undo();

			UndoRedoManager.CommandDone += delegate(object s, CommandDoneEventArgs ea)
			{
				Assert.AreEqual(ea.CommandDoneType, CommandDoneType.Redo);

				Assert.AreEqual(data.Prop1, 1);
				Assert.AreEqual(data.Prop2, 1);
			};

			UndoRedoManager.Redo();
		}

		[Test]
		public void CheckOldNewValue()
		{
			var data = new DataClass_Basic();

			data.prop1.Changed += delegate(object s, MemberChangedEventArgs ea)
			{
				Assert.AreEqual(0, ea.OldValue);
				Assert.AreEqual(2, ea.NewValue);
				Assert.AreEqual(CommandDoneType.Commit, ea.CommandDoneType);
			};

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;
				data.Prop2 = 1;

				data.Prop1 = 2;
				data.Prop2 = 2;

				UndoRedoManager.Commit();
			}
		}

		[Test]
		public void Collections()
		{
			var data = new DataCollections();
			var listChanged = false;
			var dictChanged = true;

			using (UndoRedoManager.Start("1"))
			{
				data.Dict["item 1"] = "1";
				data.Dict["item 1"] = "2";
				data.Dict["item 2"] = "2";

				data.List.Add("item A");
				data.List.Add("item B");
				data.List.Add("item C");
				data.List.RemoveAt(0);

				data.List.Changed += delegate(object sender, MemberChangedEventArgs ea)
				{
					Assert.AreEqual(2, ((IList<string>)ea.NewValue).Count);
					Assert.AreEqual(0, ((IList<string>)ea.OldValue).Count);
					listChanged = true;
				};
				data.Dict.Changed += delegate(object sender, MemberChangedEventArgs ea)
				{
					Assert.AreEqual(3, (int)ea.NewValue);
					Assert.AreEqual(3, (int)ea.OldValue);
					dictChanged = true;
				};

				UndoRedoManager.Commit();
			}

			Assert.IsTrue(listChanged);
			Assert.IsTrue(dictChanged);
		}

		[Test]
		public void Unsubscribe()
		{
			var data = new DataClass_Basic();
			var handled = false;
			EventHandler<MemberChangedEventArgs> handler1 = delegate(object sender, MemberChangedEventArgs e)
			{
				handled = true;
			};
			EventHandler<MemberChangedEventArgs> handler2 = delegate(object sender, MemberChangedEventArgs e)
			{
				
			};
			data.prop1.Changed += handler1;
			data.prop1.Changed += handler2;

			data.prop1.Changed -= handler1;

			using (UndoRedoManager.Start("1"))
			{
				data.Prop1 = 1;
				UndoRedoManager.Commit();
			}

			Assert.IsFalse(handled);
		}
	}

	class DataClass_Basic
	{
		public DataClass_Basic()
		{
			prop1.Owner = this;
			prop1.Name = "Prop1";

			prop2.Owner = this;
			prop2.Name = "Prop2";

			prop1.Changed += new EventHandler<MemberChangedEventArgs>(prop1_Changed);
			prop2.Changed += new EventHandler<MemberChangedEventArgs>(prop2_Changed);
		}
		public bool prop1Changed = false;
		void prop1_Changed(object sender, MemberChangedEventArgs e)
		{
			Assert.IsTrue(e.Member == prop1);
			Assert.AreEqual(e.Member.Owner, this);
			Assert.AreEqual(e.Member.Name, "Prop1");
			prop1Changed = true;
		}
		public bool prop2Changed = false;
		void prop2_Changed(object sender, MemberChangedEventArgs e)
		{
			Assert.IsTrue(e.Member == prop2);
			Assert.AreEqual(e.Member.Owner, this);
			Assert.AreEqual(e.Member.Name, "Prop2");
			prop2Changed = true;
		}

		internal readonly UndoRedo<int> prop1 = new UndoRedo<int>(0);
		public int Prop1
		{
			get { return prop1.Value; }
			set { prop1.Value = value; }
		}

		internal readonly UndoRedo<int> prop2 = new UndoRedo<int>(0);
		public int Prop2
		{
			get { return prop2.Value; }
			set { prop2.Value = value; }
		}		
	}

	class DataCollections
	{
		public readonly UndoRedoList<string> List = new UndoRedoList<string>();
		public readonly UndoRedoDictionary<string, string> Dict = new UndoRedoDictionary<string, string>();
	}
}
