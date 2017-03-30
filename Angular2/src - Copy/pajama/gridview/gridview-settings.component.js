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
var gridview_1 = require("./gridview");
var GridViewSettingsComponent = (function () {
    function GridViewSettingsComponent() {
        this.menuDown = false;
        this.customizingColumns = false;
    }
    GridViewSettingsComponent.prototype.resetGridState = function () {
        this.parentGridView.resetGridState();
        this.menuDown = false;
    };
    GridViewSettingsComponent.prototype.customizeColumns = function () {
        this.customizingColumns = true;
        this.menuDown = false;
    };
    return GridViewSettingsComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.GridView)
], GridViewSettingsComponent.prototype, "parentGridView", void 0);
GridViewSettingsComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'gridview-settings',
        styleUrls: ['gridview-settings.css'],
        template: "\n<div class='gridview-settings'>\n\t<div *ngIf='parentGridView.saveGridStateToStorage || parentGridView.allowColumnCustomization' class='dropup'>\n\t\t<button (click)='menuDown = !menuDown' class='btn btn-default'><span class='glyphicon glyphicon-cog'></span> Settings</button>\n\t\t<ul class='dropdown-menu' [style.display]=\"menuDown ? 'block' : 'none'\">\n\t\t\t<li class='dropdown-item' (click)='customizeColumns()'><span class='glyphicon glyphicon-wrench'></span> Customize Columns</li>\n\t\t\t<li class='dropdown-item' *ngIf='parentGridView.saveGridStateToStorage' (click)='resetGridState()'><span class='glyphicon glyphicon-repeat'></span> Reset Grid</li>\n\t\t</ul>\n\t</div>\n</div>\n<div *ngIf='parentGridView' [hidden]='!customizingColumns' class='column-menu'>\n\t<div class='customize-header'>\n\t\t<div class='column-close-button' (click)='customizingColumns = false'><span class='glyphicon glyphicon-remove'></span></div>\n\t</div>\n\t<ul [style.display]=\"customizingColumns ? 'block' : 'none'\">\n\t\t<li *ngFor='let col of parentGridView.columns' class='column-menu-item'>\n\t\t\t&nbsp;&nbsp;&nbsp;<input type='checkbox' [(ngModel)]='col.visible' (ngModelChange)='parentGridView.saveGridState()' />\n\t\t\t<span (click)='col.visible = !col.visible'>&nbsp;&nbsp;{{col.getCaption()}}</span>\n\t\t</li>\n\t</ul>\n</div>\n"
    })
], GridViewSettingsComponent);
exports.GridViewSettingsComponent = GridViewSettingsComponent;
//# sourceMappingURL=gridview-settings.component.js.map