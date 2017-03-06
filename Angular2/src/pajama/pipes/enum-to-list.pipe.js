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
var to_camel_case_pipe_1 = require('./to-camel-case.pipe');
var EnumToListPipe = (function () {
    function EnumToListPipe() {
    }
    EnumToListPipe.prototype.transform = function (value) {
        var vals = [];
        var toCamelCasePipe = new to_camel_case_pipe_1.ToCamelCasePipe();
        for (var e in value) {
            vals.push(toCamelCasePipe.transform(e));
        }
        return vals;
    };
    EnumToListPipe = __decorate([
        core_1.Pipe({ name: 'enumToList' }), 
        __metadata('design:paramtypes', [])
    ], EnumToListPipe);
    return EnumToListPipe;
}());
exports.EnumToListPipe = EnumToListPipe;
//# sourceMappingURL=enum-to-list.pipe.js.map