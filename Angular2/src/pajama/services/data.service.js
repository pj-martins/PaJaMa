"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var DataService = (function () {
    function DataService(http) {
        this.http = http;
    }
    DataService.prototype.post = function (url, body) {
        if (body === void 0) { body = null; }
        return this.http.post(url, body)
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.put = function (url, body) {
        return this.http.put(url, body)
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.delete = function (url) {
        return this.http.delete(url)
            .map(function (res) {
            if (!res.text())
                return null;
            return res.json();
        })
            .catch(this.handleError);
    };
    DataService.prototype.getItems = function (url, args) {
        if (args) {
            var firstIn = true;
            if (args.params) {
                for (var p in args.params) {
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
            if (args.pageSize && args.pageSize > 0) {
                if (!args.pageNumber)
                    args.pageNumber = 1;
                url += (firstIn ? '?' : '&') + '$top=' + args.pageSize + '&$skip=' + ((args.pageNumber - 1) * args.pageSize);
                firstIn = false;
            }
            if (args.orderBy && args.orderBy.length > 0) {
                var orderFirstIn = true;
                url += (firstIn ? '?' : '&') + '$orderby=';
                for (var _i = 0, _a = args.orderBy; _i < _a.length; _i++) {
                    var o = _a[_i];
                    url += (orderFirstIn ? '' : ',') + o.sortField + ' ' + SortDirection[o.sortDirection].toLowerCase();
                    orderFirstIn = false;
                }
                firstIn = false;
            }
            if (args.includeCount)
                url += (firstIn ? '?' : '&') + '$inlinecount=allpages';
        }
        return this.http.get(url)
            .map(function (res) {
            var results = new Items();
            if (args && args.includeCount) {
                var odataResults = res.json();
                results.results = odataResults.items;
                results.totalRecords = odataResults.count;
            }
            else {
                results.results = res.json();
                results.totalRecords = +res.headers.get('X-InlineCount');
            }
            return results;
        })
            .catch(this.handleError);
    };
    DataService.prototype.getItem = function (url) {
        return this.http.get(url)
            .map(function (res) {
            return res.json();
        })
            .catch(this.handleError);
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
        this.orderBy = [];
        this.select = [];
    }
    return GetArguments;
}());
exports.GetArguments = GetArguments;
(function (SortDirection) {
    SortDirection[SortDirection["Asc"] = 0] = "Asc";
    SortDirection[SortDirection["Desc"] = 1] = "Desc";
})(exports.SortDirection || (exports.SortDirection = {}));
var SortDirection = exports.SortDirection;
var OrderBy = (function () {
    function OrderBy(sortField, sortDirection) {
        if (sortDirection === void 0) { sortDirection = SortDirection.Asc; }
        this.sortField = sortField;
        this.sortDirection = sortDirection;
    }
    return OrderBy;
}());
exports.OrderBy = OrderBy;
var FilterBase = (function () {
    function FilterBase() {
    }
    return FilterBase;
}());
var FilterGroup = (function (_super) {
    __extends(FilterGroup, _super);
    function FilterGroup(filterOperator) {
        if (filterOperator === void 0) { filterOperator = FilterOperator.And; }
        _super.call(this);
        this.filterOperator = filterOperator;
        this.filters = [];
    }
    FilterGroup.prototype.getFilterString = function () {
        var firstIn = true;
        var filt = "";
        for (var _i = 0, _a = this.filters; _i < _a.length; _i++) {
            var f = _a[_i];
            var filtString = f.getFilterString();
            if (!filtString) {
                filtString = "1 eq 1";
            }
            filt += (firstIn ? "" : this.filterOperator == FilterOperator.And ? " and " : " or ") + "(" + filtString + ")";
            firstIn = false;
        }
        return filt;
    };
    return FilterGroup;
}(FilterBase));
exports.FilterGroup = FilterGroup;
var BinaryFilter = (function (_super) {
    __extends(BinaryFilter, _super);
    function BinaryFilter(fieldName, filterValue, filterType) {
        if (filterType === void 0) { filterType = FilterType.EqualTo; }
        _super.call(this);
        this.fieldName = fieldName;
        this.filterValue = filterValue;
        this.filterType = filterType;
    }
    BinaryFilter.prototype.getFilterString = function () {
        var filt = "";
        var filtValue = this.filterValue;
        if (filtValue && typeof this.filterValue != "number") {
            filtValue = "'" + filtValue.toString() + "'";
        }
        var dbField = this.fieldName.substring(0, 1).toUpperCase() + this.fieldName.substring(1);
        switch (this.filterType) {
            case FilterType.EqualTo:
            case FilterType.NotEqualTo:
                filt += dbField + " " + (this.filterType == FilterType.EqualTo ? 'eq' : 'ne') + " " + (!this.filterValue ? "null" : filtValue);
                break;
            case FilterType.Contains:
                filt += "indexof(" + dbField + "," + filtValue + ") ge 0";
                break;
            case FilterType.StartsWith:
                filt += "startswith(" + dbField + ",'" + this.filterValue.toString() + "')";
                break;
            case FilterType.EndsWith:
                filt += "endswith(" + dbField + ",'" + this.filterValue.toString() + "')";
                break;
            case FilterType.LessThan:
            case FilterType.LessThanOrEqual:
                filt += dbField + " " + (this.filterType == FilterType.LessThan ? 'lt' : 'le') + " " + (!this.filterValue ? "null" : filtValue);
                break;
            case FilterType.GreaterThan:
            case FilterType.GreaterThanOrEqual:
                filt += dbField + " " + (this.filterType == FilterType.GreaterThan ? 'gt' : 'ge') + " " + (!this.filterValue ? "null" : filtValue);
                break;
            default:
                throw this.filterType;
        }
        return filt;
    };
    return BinaryFilter;
}(FilterBase));
exports.BinaryFilter = BinaryFilter;
(function (FilterType) {
    FilterType[FilterType["EqualTo"] = 0] = "EqualTo";
    FilterType[FilterType["NotEqualTo"] = 1] = "NotEqualTo";
    FilterType[FilterType["LessThan"] = 2] = "LessThan";
    FilterType[FilterType["GreaterThan"] = 3] = "GreaterThan";
    FilterType[FilterType["LessThanOrEqual"] = 4] = "LessThanOrEqual";
    FilterType[FilterType["GreaterThanOrEqual"] = 5] = "GreaterThanOrEqual";
    FilterType[FilterType["Contains"] = 6] = "Contains";
    FilterType[FilterType["StartsWith"] = 7] = "StartsWith";
    FilterType[FilterType["EndsWith"] = 8] = "EndsWith";
})(exports.FilterType || (exports.FilterType = {}));
var FilterType = exports.FilterType;
(function (FilterOperator) {
    FilterOperator[FilterOperator["And"] = 0] = "And";
    FilterOperator[FilterOperator["Or"] = 1] = "Or";
})(exports.FilterOperator || (exports.FilterOperator = {}));
var FilterOperator = exports.FilterOperator;
//# sourceMappingURL=data.service.js.map