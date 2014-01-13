using System;
using System.Text;

namespace DejaVu
{
	/// <summary>
	/// Logging class optimized for effective collecting of log records.
	/// </summary>
	class Log
	{
		string[] logRecords;
		int size;
		int count;
		int head;

		public Log(int maxRecords)
		{
			this.size = Math.Max(1, maxRecords);
			logRecords = new string[size];
		}
		/// <summary>
		/// Add record to log
		/// </summary>
		/// <param name="record"></param>
		public void Add(string record)
		{
			logRecords[head] = record;
			head = (head + 1) % size;
			count++;
		}
		/// <summary>
		/// Get log contents as a single string
		/// </summary>
		/// <returns></returns>
		public new string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if (count > size)
			{
				builder.Append("...Log had been truncated after record #" + (count - size) + "..." + Environment.NewLine);
			}
			for (int i = 0; i < size; i++)
			{
				int j = (head + i) % size;
				string record = logRecords[j];
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
