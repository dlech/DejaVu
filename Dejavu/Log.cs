using System;
using System.Text;

namespace DejaVu
{
	/// <summary>
	/// Logging class optimized for effective collecting of log records.
	/// </summary>
	class Log
	{
	    readonly string[] _logRecords;
	    readonly int _size;
		int _count;
		int _head;

		public Log(int maxRecords)
		{
			_size = Math.Max(1, maxRecords);
			_logRecords = new string[_size];
		}
		/// <summary>
		/// Add record to log
		/// </summary>
		/// <param name="record"></param>
		public void Add(string record)
		{
			_logRecords[_head] = record;
			_head = (_head + 1) % _size;
			_count++;
		}
		/// <summary>
		/// Get log contents as a single string
		/// </summary>
		/// <returns></returns>
		public new string ToString()
		{
			var builder = new StringBuilder();

			if (_count > _size)
			{
				builder.Append("...Log had been truncated after record #" + (_count - _size) + "..." + Environment.NewLine);
			}
			for (var i = 0; i < _size; i++)
			{
				var j = (_head + i) % _size;
				var record = _logRecords[j];
				if (record != null)
				{
					builder.Append(record);
					builder.Append(Environment.NewLine);
				}
			}

			return builder.ToString();
		}

	}

}
