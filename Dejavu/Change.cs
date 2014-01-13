// This source is under LGPL license. Sergei Arhipenko (c) 2006-2007. email: sbs-arhipenko@yandex.ru. This notice may not be removed.

namespace DejaVu
{
	// this interface provides untyped access to old and new values. its used by DejaVu framework
	interface IChange
	{
		object NewObject { get; set; }
		object OldObject { get; set; }
	}
    class Change<TState> : IChange
    {
        public TState OldState;
        public TState NewState;

		#region IChange Members

		object IChange.NewObject
		{
			get
			{
				return NewState;
			}
			set
			{
				NewState = (TState)value;
			}
		}

		object IChange.OldObject
		{
			get
			{
				return OldState;
			}
			set
			{
				OldState = (TState)value;
			}
		}

		#endregion
	} 
}
