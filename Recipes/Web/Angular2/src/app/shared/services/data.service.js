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
require('rxjs/add/operator/map');
var appsettings_1 = require('../constants/appsettings');
var DataService = (function () {
    function DataService(http) {
        this.http = http;
    }
    DataService.prototype.buildGetEntitiesUrl = function (entityType, args) {
        var url = '';
        if (args && args.baseUrl)
            url += args.baseUrl;
        url += appsettings_1.AppSettings.API_ENDPOINT + entityType + (args && args.includeCount ? '/entitiesWithCount' : '/entities');
        var firstIn = true;
        if (args && args.parameters) {
            for (var _i = 0, _a = args.parameters; _i < _a.length; _i++) {
                var p = _a[_i];
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
    };
    DataService.prototype.getEntities = function (entityType, args) {
        var url = this.buildGetEntitiesUrl(entityType, args);
        return this.http.get(url).map(function (res) {
            var entities = new Entities();
            entities.Entities = res.json();
            entities.TotalResults = parseInt(res.headers.get("X-InlineCount"));
            return entities;
        });
    };
    DataService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], DataService);
    return DataService;
}());
exports.DataService = DataService;
var Parameter = (function () {
    function Parameter(name, value) {
        this.name = name;
        this.value = value;
    }
    return Parameter;
}());
exports.Parameter = Parameter;
var Arguments = (function () {
    function Arguments() {
    }
    return Arguments;
}());
exports.Arguments = Arguments;
var Entities = (function () {
    function Entities() {
    }
    return Entities;
}());
exports.Entities = Entities;
//# sourceMappingURL=data.service.js.map