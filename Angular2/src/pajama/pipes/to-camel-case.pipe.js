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
var ToCamelCasePipe = (function () {
    function ToCamelCasePipe() {
    }
    ToCamelCasePipe.prototype.transform = function (value) {
        var regex = /([A-Z])/g;
        return value.replace(regex, " $1");
    };
    ToCamelCasePipe = __decorate([
        core_1.Pipe({ name: 'toCamelCase' }), 
        __metadata('design:paramtypes', [])
    ], ToCamelCasePipe);
    return ToCamelCasePipe;
}());
exports.ToCamelCasePipe = ToCamelCasePipe;
//# sourceMappingURL=to-camel-case.pipe.js.map