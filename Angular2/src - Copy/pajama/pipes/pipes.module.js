"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var enum_to_list_pipe_1 = require("./enum-to-list.pipe");
var order_by_pipe_1 = require("./order-by.pipe");
var to_camel_case_pipe_1 = require("./to-camel-case.pipe");
var moment_pipe_1 = require("./moment.pipe");
var PipesModule = (function () {
    function PipesModule() {
    }
    return PipesModule;
}());
PipesModule = __decorate([
    core_1.NgModule({
        declarations: [
            enum_to_list_pipe_1.EnumToListPipe,
            order_by_pipe_1.OrderByPipe,
            to_camel_case_pipe_1.ToCamelCasePipe,
            moment_pipe_1.MomentPipe
        ],
        exports: [
            enum_to_list_pipe_1.EnumToListPipe,
            order_by_pipe_1.OrderByPipe,
            to_camel_case_pipe_1.ToCamelCasePipe,
            moment_pipe_1.MomentPipe
        ]
    })
], PipesModule);
exports.PipesModule = PipesModule;
//# sourceMappingURL=pipes.module.js.map