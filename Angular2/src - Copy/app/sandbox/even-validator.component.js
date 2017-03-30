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
var forms_1 = require("@angular/forms");
var noop = function () {
};
exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR = {
    provide: forms_1.NG_VALUE_ACCESSOR,
    useExisting: core_1.forwardRef(function () { return EvenValidatorComponent; }),
    multi: true
};
exports.CUSTOM_INPUT_CONTROL_VALIDATOR = {
    provide: forms_1.NG_VALIDATORS,
    useExisting: core_1.forwardRef(function () { return EvenValidatorDirective; }),
    multi: true
};
var EvenValidatorComponent = (function () {
    function EvenValidatorComponent() {
        this.formControl = new forms_1.FormControl();
        this.onTouchedCallback = noop;
        this.onChangeCallback = noop;
    }
    EvenValidatorComponent.prototype.writeValue = function (value) {
        if (value !== this.innerValue) {
            this.innerValue = value;
        }
    };
    EvenValidatorComponent.prototype.registerOnChange = function (fn) {
        this.onChangeCallback = fn;
    };
    EvenValidatorComponent.prototype.registerOnTouched = function (fn) {
        this.onTouchedCallback = fn;
    };
    return EvenValidatorComponent;
}());
__decorate([
    core_1.Input("formControl"),
    __metadata("design:type", Object)
], EvenValidatorComponent.prototype, "formControl", void 0);
EvenValidatorComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'even-validator',
        template: "<div>\n<input type=\"number\" [(ngModel)]=\"innerValue\" even-validator-directive [formControl]=\"formControl\" />\n</div>",
        providers: [exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
    })
], EvenValidatorComponent);
exports.EvenValidatorComponent = EvenValidatorComponent;
var EvenValidatorDirective = (function () {
    function EvenValidatorDirective() {
    }
    EvenValidatorDirective.prototype.validate = function (c) {
        console.log(c);
        if (!c.value || c.value % 2 != 0)
            return {
                notEven: true
            };
        return null;
    };
    return EvenValidatorDirective;
}());
EvenValidatorDirective = __decorate([
    core_1.Directive({
        selector: '[even-validator-directive]',
        providers: [exports.CUSTOM_INPUT_CONTROL_VALIDATOR]
    })
], EvenValidatorDirective);
exports.EvenValidatorDirective = EvenValidatorDirective;
//# sourceMappingURL=even-validator.component.js.map