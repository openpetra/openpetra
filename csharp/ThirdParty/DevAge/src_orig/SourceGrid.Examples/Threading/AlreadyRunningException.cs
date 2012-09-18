
using System;
using System.ComponentModel;
using System.Threading;

namespace SourceGrid.Examples.Threading
{
	public class AlreadyRunningException : System.ApplicationException
	{
	    public AlreadyRunningException() : base("Asynchronous operation already running")
	    { }
	}
}
