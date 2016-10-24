import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { AppSettings } from '../constants/appsettings';

@Injectable()
export class DataService {
    constructor(private http: Http) { }

    buildGetEntitiesUrl<TEntity>(entityType: string, args: Arguments): string {
        let url = '';
        if (args && args.baseUrl)
            url += args.baseUrl;
        url += AppSettings.API_ENDPOINT + entityType + (args && args.includeCount ? '/entitiesWithCount' : '/entities');
        let firstIn = true;
        if (args && args.parameters) {
            for (let p of args.parameters) {
                url += (firstIn ? '?' : '&') + p.name + '=' + p.value;
                firstIn = false;
            }

        }

        if (args && args.filter) {
            url += (firstIn ? '?' : '&') + '$filter=' + args.filter;
            firstIn = false;
        }

        if (args && args.pageNumber && args.pageNumber > 0 && args.pageSize && args.pageSize > 0) {
            url += (firstIn ? '?' : '&') + '$top=' + args.pageSize + '&$skip=' + ((args.pageNumber - 1) * args.pageSize);
            firstIn = false;
        }

        if (args && args.orderBy)
            url += (firstIn ? '?' : '&') + '$orderby=' + args.orderBy;

        return url;
    }

    getEntities<TEntity>(entityType: string, args: Arguments): Observable<Entities<TEntity>> {
        let url = this.buildGetEntitiesUrl<TEntity>(entityType, args);
        return this.http.get(url).map((res) => {
            let entities = new Entities<TEntity>();
            entities.Entities = res.json();
            entities.TotalResults = parseInt(res.headers.get("X-InlineCount"));
            return entities;
        });
    }

    //factory.buildGetEntityUrl = function (entityType, id, args) {
    //    var url = '';
    //    if (args && args.baseUrl)
    //        url += args.baseUrl;
    //    url += serviceBase + entityType + '/entity/' + id;
    //    return url;
    //};

    //factory.getEntity = function (entityType, id, args) {
    //    var url = factory.buildGetEntityUrl(entityType, id, args);
    //    return $http.get(url).then(function (response) {
    //        return response.data;
    //    });
    //};

    //factory.newEntity = function () {
    //    return $q.when({ id: 0 });
    //};

    //factory.insertEntity = function (entityType, entity) {
    //    return $http.post(serviceBase + entityType + '/postEntity', entity).then(function (results) {
    //        entity.id = results.data.id;
    //        return results.data;
    //    });
    //};

    //factory.updateEntity = function (entityType, entity) {
    //    return $http.put(serviceBase + entityType + '/putEntity/' + entity.id, entity).then(function (status) {
    //        return status.data;
    //    });
    //};

    //factory.deleteEntity = function (entityType, id) {
    //    return $http.delete(serviceBase + entityType + '/deleteEntity/' + id).then(function (status) {
    //        return status.data;
    //    });
    //};
}
export class Parameter {
    constructor(public name: string, public value: string) { }
}
export class Arguments {
    parameters: Array<Parameter>;
    filter: string;
    baseUrl: string;
    includeCount: boolean;
    pageNumber: number;
    pageSize: number;
    orderBy: string;
}
export class Entities<TEntity> {
    Entities: Array<TEntity>;
    TotalResults: number;
}