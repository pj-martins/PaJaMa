import { GridView, DataColumn, FilterMode } from './gridview/gridview';
import { ODataArguments, FilterGroup, OrderBy, FilterOperator, BinaryFilter, FilterType } from './services/data.service';

export enum SortDirection {
	None,
	Asc,
	Desc
}

export class DataServiceUtils {
	static getODataArgumentsForGridView(gridView: GridView): ODataArguments {
		let args = new ODataArguments();
		args.pageNumber = gridView.currentPage;
		args.pageSize = gridView.pageSize;
		let sortedColumns: Array<DataColumn> = [];
		for (let c of gridView.getDataColumns()) {
			if (c.sortDirection != SortDirection.None) {
				sortedColumns.push(c);
			}
		}

		sortedColumns.sort((a, b) => {
			if (a.sortIndex > b.sortIndex)
				return 1;

			if (a.sortIndex < b.sortIndex)
				return -1;

			return 0;
		});

		for (let sc of sortedColumns) {
			args.orderBy.push(new OrderBy(sc.getODataField(),
				sc.sortDirection));
		}

		if (gridView.filterVisible) {
			let grp = new FilterGroup();

			for (let col of gridView.getDataColumns()) {
				if (col.filterMode == FilterMode.DynamicList && col.filterValue) {
					// TODO: why was this here?
					//if (col.filterValue.length == 0) {
					//	this.loading = false;
					//	gridView.totalRecords = 0;
					//	gridView.data = [];
					//	return;
					//}

					if (col.filterValue.length != col.filterOptions.length) {
						let cgrp = new FilterGroup(FilterOperator.Or);
						for (let c of col.filterValue) {
							cgrp.filters.push(new BinaryFilter(col.getODataField(), c, FilterType.Contains));
						}
						grp.filters.push(cgrp);
					}
				}
				else if (col.filterMode == FilterMode.DateRange && col.filterValue && (col.filterValue.fromDate || col.filterValue.toDate)) {
					let cgrp = new FilterGroup(FilterOperator.And);
					if (col.filterValue.fromDate) {
						let fromDate = new Date(col.filterValue.fromDate);
						fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth(), fromDate.getDate());
						cgrp.filters.push(new BinaryFilter(col.getODataField(), fromDate, FilterType.GreaterThanOrEqual));
					}
					if (col.filterValue.toDate) {
						let toDate = new Date(col.filterValue.toDate);
						toDate = new Date(toDate.getFullYear(), toDate.getMonth(), toDate.getDate());
						toDate.setDate(toDate.getDate() + 1);
						cgrp.filters.push(new BinaryFilter(col.getODataField(), toDate, FilterType.LessThan));
					}
					grp.filters.push(cgrp);
				}
			}

			if (grp.filters.length > 0) {
				args.filter = grp;
			}
		}

		return args;
	}
}