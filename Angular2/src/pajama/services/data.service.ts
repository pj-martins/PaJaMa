import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { SortDirection } from '../shared';

@Injectable()
export class DataService {

	constructor(protected http: Http) { }

	post<TObject>(url: string, body: TObject = null): Observable<TObject> {
		return this.http.post(url, body)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	put<TObject>(url: string, body: TObject): Observable<TObject> {
		return this.http.put(url, body)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	delete(url: string): Observable<boolean> {
		return this.http.delete(url)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	getItems<TObject>(url: string, args?: GetArguments): Observable<Items<TObject>> {
		if (args) {
			let firstIn = true;
			if (args.params) {
				for (let p in args.params) {
					url += (firstIn ? '?' : '&') + p + '=' + args.params[p];
					firstIn = false;
				}
			}
		}

		return this.http.get(url)
			.map((res: Response) => {
				let results = new Items<TObject>();
				results.results = res.json();
				let inlineCount = res.headers.get('X-InlineCount');
				if (inlineCount)
					results.totalRecords = +inlineCount;
				return results;
			})
			.catch(this.handleError);
	}

	getItemsOData<TObject>(url: string, args?: ODataArguments): Observable<Items<TObject>> {
		if (args) {
			let firstIn = true;
			if (args.params) {
				for (let p in args.params) {
					url += (firstIn ? '?' : '&') + p + '=' + args.params[p];
					firstIn = false;
				}
			}

			if (args.filter) {
				url += (firstIn ? '?' : '&') + '$filter=' + args.filter.getFilterString();
				firstIn = false;
			}

			if (args.select && args.select.length > 0) {
				url += (firstIn ? '?' : '&') + "$select=" + args.select.join(",");
				firstIn = false;
			}

			if (args.expand && args.expand.length > 0) {
				url += (firstIn ? '?' : '&') + "$expand=" + args.expand.join(",");
				firstIn = false;
			}

			if (args.pageSize && args.pageSize > 0) {
				if (!args.pageNumber)
					args.pageNumber = 1;
				url += (firstIn ? '?' : '&') + '$top=' + args.pageSize + '&$skip=' + ((args.pageNumber - 1) * args.pageSize);
				firstIn = false;
			}

			if (args.orderBy && args.orderBy.length > 0) {
				let orderFirstIn = true;
				url += (firstIn ? '?' : '&') + '$orderby=';
				for (let o of args.orderBy) {
					url += (orderFirstIn ? '' : ',') + o.sortField + ' ' + SortDirection[o.sortDirection].toLowerCase();
					orderFirstIn = false;
				}
				firstIn = false;
			}
			if (args.includeCount)
				url += (firstIn ? '?' : '&') + '$inlinecount=allpages';
		}
		return this.http.get(url)
			.map((res: Response) => {
				let results = new Items<TObject>();
				if (args && args.includeCount) {
					let odataResults = res.json();
					results.results = odataResults.items;
					results.totalRecords = odataResults.count;
				}
				else {
					results.results = res.json();
					let inlineCount = res.headers.get('X-InlineCount');
					if (inlineCount)
						results.totalRecords = +inlineCount;
				}
				return results;
			})
			.catch(this.handleError);
	}

	getItem<TObject>(url: string): Observable<TObject> {
		return this.http.get(url)
			.map((res: Response) => {
				return res.json();
			})
			.catch(this.handleError);
	}

	private handleError(error: any) {
		let errMessage = 'Error occured!';
		if (error) {
			if (!error.exceptionMessage && !error.message && error._body) {
				try {
					let parsed = JSON.parse(error._body);
					if (parsed.exceptionMessage || parsed.message)
						error = parsed;
				}
				catch (e) {
					// not valid JSON
				}
			}
			errMessage = error.exceptionMessage || error.message || error._body || error;
		}
		console.error(errMessage);
		return Observable.throw(errMessage);
	}
}

export class Items<TObject> {
	results: TObject[];
	totalRecords: number;
}

export class GetArguments {
	params: { [paramName: string]: string } = {};
}

export class ODataArguments extends GetArguments {
	pageSize: number;
	pageNumber: number;
	filter: FilterBase;
	orderBy: Array<OrderBy> = [];
	select: Array<string> = [];
	expand: Array<string> = [];
	includeCount: boolean;
}

export class OrderBy {
	constructor(public sortField: string, public sortDirection: SortDirection = SortDirection.Asc) { }
}

abstract class FilterBase {
	abstract getFilterString(): string;
}
export class FilterGroup extends FilterBase {
	operator: FilterOperator;
	filters: Array<FilterBase> = [];

	constructor(public filterOperator: FilterOperator = FilterOperator.And) {
		super();
	}

	getFilterString(): string {
		let firstIn = true;
		let filt = "";
		for (let f of this.filters) {
			let filtString = f.getFilterString();
			if (!filtString) {
				filtString = "1 eq 1";
			}
			filt += `${(firstIn ? "" : this.filterOperator == FilterOperator.And ? " and " : " or ")}(${filtString})`;
			firstIn = false;
		}
		return filt;
	}
}
export class BinaryFilter extends FilterBase {
	constructor(public fieldName: string, public filterValue: any, public filterType: FilterType = FilterType.EqualTo) {
		super();
	}

	getFilterString(): string {
		let filt = "";
		let filtValue = this.filterValue;
		if (filtValue && typeof this.filterValue != "number") {
			filtValue = `'${filtValue.toString()}'`;
		}
		let dbField = this.fieldName.substring(0, 1).toUpperCase() + this.fieldName.substring(1);
		switch (this.filterType) {
			case FilterType.EqualTo:
			case FilterType.NotEqualTo:
				filt += `${dbField} ${this.filterType == FilterType.EqualTo ? 'eq' : 'ne'} ${(!this.filterValue ? "null" : filtValue)}`;
				break;
			case FilterType.Contains:
				filt += `indexof(${dbField},${filtValue}) ge 0`;
				break;
			case FilterType.StartsWith:
				filt += `startswith(${dbField},'${this.filterValue.toString()}')`;
				break;
			case FilterType.EndsWith:
				filt += `endswith(${dbField},'${this.filterValue.toString()}')`;
				break;
			case FilterType.LessThan:
			case FilterType.LessThanOrEqual:
				filt += `${dbField} ${this.filterType == FilterType.LessThan ? 'lt' : 'le'} ${(!this.filterValue ? "null" : filtValue)}`;
				break;
			case FilterType.GreaterThan:
			case FilterType.GreaterThanOrEqual:
				filt += `${dbField} ${this.filterType == FilterType.GreaterThan ? 'gt' : 'ge'} ${(!this.filterValue ? "null" : filtValue)}`;
				break;
			default:
				throw this.filterType;
		}

		return filt;
	}
}

export enum FilterType {
	EqualTo,
	NotEqualTo,
	LessThan,
	GreaterThan,
	LessThanOrEqual,
	GreaterThanOrEqual,
	Contains,
	StartsWith,
	EndsWith,
}

export enum FilterOperator {
	And,
	Or
}