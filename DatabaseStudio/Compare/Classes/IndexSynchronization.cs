using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class IndexSynchronization : DatabaseObjectSynchronizationBase<Index>
	{
		public IndexSynchronization(Index index)
			: base(index)
		{
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP INDEX [{0}].[{1}].[{2}]", databaseObject.Table.Schema.SchemaName, databaseObject.Table.TableName,
				databaseObject.IndexName));
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(GetCreateScript(false).ToString());
		}

		public StringBuilder GetCreateScript(bool hasTarget)
		{
			var indexCols = databaseObject.IndexColumns.Where(i => i.Ordinal != 0);
			var includeCols = databaseObject.IndexColumns.Where(i => i.Ordinal == 0);
			var sb = new StringBuilder();

			sb.AppendLineFormat(@"CREATE {0} {1} INDEX [{2}] ON [{3}].[{4}]
(
	{5}
){6}", (bool)databaseObject.IsUnique ? "UNIQUE" : "",
databaseObject.IndexType,
databaseObject.IndexName,
databaseObject.Table.Schema.SchemaName,
databaseObject.Table.TableName,
string.Join(",\r\n\t",
indexCols.OrderBy(c => c.Ordinal).Select(c =>
	string.Format("[{0}] {1}", c.ColumnName, c.Descending ? "DESC" : "ASC")).ToArray()),
	!includeCols.Any() ? string.Empty : string.Format(@"
INCLUDE (
	{0}
)", string.Join(",\r\n\t",
includeCols.Select(c =>
	string.Format("[{0}]", c.ColumnName).ToString()))
	));

			return sb;
		}
	}
}
