"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
var Rx_1 = require('rxjs/Rx');
require('rxjs/add/operator/map');
require('rxjs/add/operator/catch');
var appsettings_1 = require('../constants/appsettings');
var DataService = (function () {
    function DataService(http) {
        this.http = http;
    }
    DataService.prototype.post = function (url, body) {
        if (body === void 0) { body = null; }
        return this.http.post(appsettings_1.AppSettings.API_ENDPOINT + url, body)
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.put = function (url, body) {
        return this.http.put(appsettings_1.AppSettings.API_ENDPOINT + url, body)
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.delete = function (url) {
        return this.http.delete(appsettings_1.AppSettings.API_ENDPOINT + url + "/deleteEntity")
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.getItems = function (url) {
        return this.http.get(appsettings_1.AppSettings.API_ENDPOINT + url)
            .map(function (res) {
            var results = new Items();
            results.results = res.json();
            results.totalRecords = +res.headers.get('X-InlineCount');
            return results;
        })
            .catch(this.handleError);
    };
    DataService.prototype.getItem = function (url) {
        return this.http.get(appsettings_1.AppSettings.API_ENDPOINT + url)
            .map(function (res) {
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.getEntitiesUrl = function (entityType, odata, args) {
        var url = entityType + (odata ? '/entitiesOData' : '/entities');
        if (args) {
            var firstIn = true;
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
    };
    DataService.prototype.getEntitiesOData = function (entityType, args) {
        return this.getItems(this.getEntitiesUrl(entityType, true, args));
    };
    DataService.prototype.getEntities = function (entityType, args) {
        return this.getItems(this.getEntitiesUrl(entityType, false, args));
    };
    DataService.prototype.getEntity = function (entityType, id) {
        return this.getItem(entityType + "/entity/" + id);
    };
    DataService.prototype.deleteEntity = function (entityType, id) {
        return this.delete(entityType + "/deleteEntity/" + id);
    };
    DataService.prototype.insertEntity = function (entityType, entity) {
        return this.post(entityType + "/postEntity", entity);
    };
    DataService.prototype.updateEntity = function (entityType, id, entity) {
        return this.put(entityType + ("/putEntity/" + id), entity);
    };
    DataService.prototype.handleError = function (error) {
        var errMessage = 'Error occured!';
        if (error) {
            if (!error.exceptionMessage && !error.message && error._body) {
                try {
                    var parsed = JSON.parse(error._body);
                    if (parsed.exceptionMessage || parsed.message)
                        error = parsed;
                }
                catch (e) {
                }
            }
            errMessage = error.exceptionMessage || error.message || error._body || error;
        }
        console.error(errMessage);
        return Rx_1.Observable.throw(errMessage);
    };
    DataService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], DataService);
    return DataService;
}());
exports.DataService = DataService;
var Items = (function () {
    function Items() {
    }
    return Items;
}());
exports.Items = Items;
var GetArguments = (function () {
    function GetArguments() {
        this.params = {};
    }
    return GetArguments;
}());
exports.GetArguments = GetArguments;
//# sourceMappingURL=data.service.js.map