// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.
using System;

namespace DejaVu
{
    public interface IUndoRedoMember
    {
        void OnCommit(object change);
        void OnUndo(object change);
        void OnRedo(object change);

		object Owner { get; set; }
		string Name { get; set; }
		event EventHandler<MemberChangedEventArgs> Changed;
    }
	internal interface IChangedNotification
	{
		void OnChanged(CommandDoneType type, IChange change);
	}
}
