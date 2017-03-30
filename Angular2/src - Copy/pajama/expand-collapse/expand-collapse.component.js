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
var ExpandCollapseComponent = (function () {
    function ExpandCollapseComponent() {
        this.hidden = false;
    }
    return ExpandCollapseComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], ExpandCollapseComponent.prototype, "hidden", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], ExpandCollapseComponent.prototype, "headerText", void 0);
ExpandCollapseComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'expand-collapse',
        template: "\n<div class='expand-collapse'>\n    <div class='header-panel'>\n\t\t<strong>{{headerText}} </strong>\n\t\t<button (click)=\"hidden = !hidden\"><span class=\"glyphicon {{hidden ? 'glyphicon-plus' : 'glyphicon-minus'}}\"></span></button>\n\t</div>\n\t<div class=\"content {{hidden ? 'content-collapsed' : 'content-expanded'}}\">\n\t\t<ng-content></ng-content>\n\t\t<br /><br />\n\t</div>\n</div>\n",
        styleUrls: ['expand-collapse.css']
    })
], ExpandCollapseComponent);
exports.ExpandCollapseComponent = ExpandCollapseComponent;
//# sourceMappingURL=expand-collapse.component.js.map