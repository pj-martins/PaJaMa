using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PaJaMaCommon
{
	public class PaJaMaBindingList<T> : BindingList<T>
	{
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			List<T> lst = this.Items as List<T>;
			lst.Sort(delegate(T item1, T item2)
			{
				if (direction == ListSortDirection.Ascending)
					return ((IComparable)prop.GetValue(item1)).CompareTo((IComparable)prop.GetValue(item2));
				return ((IComparable)prop.GetValue(item2)).CompareTo((IComparable)prop.GetValue(item1));
			});
		}

		protected override bool SupportsSortingCore
		{
			get
			{
				return true;
			}
		}
	}
}
