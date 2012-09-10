
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace SourceGrid.Examples.Threading
{
	public class AsyncOperationException : Exception
		{
			public AsyncOperationException(string message, Exception e)
				:base(message, e)
			{
				
			}
		}
}
