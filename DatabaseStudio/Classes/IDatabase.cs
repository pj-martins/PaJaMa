using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Classes
{
	public interface IDatabase
	{
		string ConnectionString { get; }
		string DatabaseName { get; }
	}
}
