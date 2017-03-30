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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SandboxComponent = (function () {
    function SandboxComponent() {
        this.minBeginDate = new Date(2010, 1, 1);
        this.maxDate = new Date(2020, 1, 1);
        this.beginDate = new Date();
    }
    return SandboxComponent;
}());
SandboxComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'sandbox',
        template: "\n<form #editForm=\"ngForm\">\n{{editForm.controls.test?.invalid}}\n<!--<even-validator [(ngModel)]=\"numericValue\" [formControl]='test'></even-validator>-->\n<datetime-picker placeholder=\"From\" name=\"test\" class=\"daterange\" [hideTime]=\"true\" [(ngModel)]=\"beginDate\" required [selectOnCalendarClick]=\"true\" \n\t[minDate]=\"minBeginDate\" [maxDate]=\"maxDate\"></datetime-picker>\n</form>\n"
    }),
    __metadata("design:paramtypes", [])
], SandboxComponent);
exports.SandboxComponent = SandboxComponent;
//# sourceMappingURL=sandbox.component.js.map