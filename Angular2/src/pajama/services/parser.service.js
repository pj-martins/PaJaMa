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
// Waiting for replacement from $parse
var ParserService = (function () {
    function ParserService() {
    }
    ParserService.prototype.getObjectValue = function (prop, obj) {
        if (!prop)
            return null;
        var parts = prop.split('.');
        var tempObj = obj;
        for (var _i = 0, parts_1 = parts; _i < parts_1.length; _i++) {
            var part = parts_1[_i];
            var match = part.match(/^(.*)\[(\d*)\]$/);
            if (match && match.length == 3) {
                tempObj = tempObj[match[1]][parseInt(match[2])];
            }
            else {
                tempObj = tempObj[part];
            }
            // if there are no periods then simply return the raw value
            if (!tempObj)
                return parts.length == 1 ? tempObj : undefined;
        }
        return tempObj;
    };
    ParserService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], ParserService);
    return ParserService;
}());
exports.ParserService = ParserService;
//# sourceMappingURL=parser.service.js.map