import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { EntityBase } from '../dto/entity-base';

declare var appSettings: any;

@Injectable()
export abstract class DataService {

	constructor(protected http: Http) { }

	post<TObject>(url: string, body: TObject = null): Observable<TObject> {
		return this.http.post(appSettings.API_ENDPOINT + url, body)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	put<TObject>(url: string, body: TObject): Observable<TObject> {
		return this.http.put(appSettings.API_ENDPOINT + url, body)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	delete(url: string): Observable<boolean> {
		return this.http.delete(appSettings.API_ENDPOINT + url + `/deleteEntity`)
			.map((res: Response) => {
				if (!res.text())
					return null;
				return res.json();
			})
			.catch(this.handleError);
	}

	getItems<TObject>(url: string): Observable<Items<TObject>> {
		return this.http.get(appSettings.API_ENDPOINT + url)
			.map((res: Response) => {
				let results = new Items<TObject>();
				results.results = res.json();
				results.totalRecords = +res.headers.get('X-InlineCount');
				return results;
			})
			.catch(this.handleError);
	}

	getItem<TObject>(url: string): Observable<TObject> {
		return this.http.get(appSettings.API_ENDPOINT + url)
			.map((res: Response) => {
				return res.json();
			})
			.catch(this.handleError);
	}

	private getEntitiesUrl(entityType: string, odata: boolean, args?: GetArguments): string {
		let url = entityType + (odata ? '/entitiesOData' : '/entities');
		if (args) {
			let firstIn = true;
			if (args.params) {
				for (var p in args.params) {
					url += (firstIn ? '?' : '&') + p + '=' + args.params[p];
					firstIn = false;
				}
			}

			if (args.filter) {
				url += (firstIn ? '?' : '&') + '$filter=' + args.filter;
				firstIn = false;
			}

			if (args.pageSize) {
				if (!args.pageNumber)
					args.pageNumber = 1;
				url += (firstIn ? '?' : '&') + '$top=' + args.pageSize + '&$skip=' + ((args.pageNumber - 1) * args.pageSize);
				firstIn = false;
			}

			if (args.orderBy) {
				url += (firstIn ? '?' : '&') + '$orderby=' + args.orderBy;
				firstIn = false;
			}
			if (odata)
				url += (firstIn ? '?' : '&') + '$inlinecount=allpages';
		}
		return url;
	}

	getEntitiesOData<TEntity>(entityType: string, args?: GetArguments): Observable<Items<TEntity>> {
		return this.getItems<TEntity>(this.getEntitiesUrl(entityType, true, args));
	}

	getEntities<TEntity>(entityType: string, args?: GetArguments): Observable<Items<TEntity>> {
		return this.getItems<TEntity>(this.getEntitiesUrl(entityType, false, args));
	}

	getEntity<TEntity>(entityType: string, id: number): Observable<TEntity> {
		return this.getItem<TEntity>(`${entityType}/entity/${id}`);
	}

	deleteEntity(entityType: string, id: number): Observable<boolean> {
		return this.delete(`${entityType}/deleteEntity/${id}`);
	}

	insertEntity<TEntity>(entityType: string, entity: TEntity) {
		return this.post<TEntity>(entityType + `/postEntity`, entity);
	}
	
	updateEntity<TEntity>(entityType: string, id: number, entity: TEntity) {
		return this.put<TEntity>(entityType + `/putEntity/${id}`, entity);
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
	pageSize: number;
	pageNumber: number;
	params: any = {};
	filter: string;
	orderBy: string;
}