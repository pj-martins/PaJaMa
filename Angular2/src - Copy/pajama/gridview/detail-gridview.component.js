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
var gridview_component_1 = require("./gridview.component");
var gridview_1 = require("./gridview");
var DetailGridViewComponent = (function () {
    function DetailGridViewComponent() {
    }
    DetailGridViewComponent.prototype.ngOnInit = function () {
        this.detailGridViewInstance = this.detailGridView.createInstance();
        this.parentGridViewComponent.detailGridViewComponents[this.row[this.parentGridViewComponent.grid.keyFieldName]] = this;
    };
    DetailGridViewComponent.prototype.isExpanded = function () {
        return this._expanded;
    };
    DetailGridViewComponent.prototype.expandCollapse = function () {
        this._expanded = !this._expanded;
        if (!this._inited) {
            this._inited = true;
            this.detailGridView.setChildData.emit(new gridview_1.DetailGridViewDataEventArgs(this.row, this.detailGridViewInstance));
        }
    };
    return DetailGridViewComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_component_1.GridViewComponent)
], DetailGridViewComponent.prototype, "parentGridViewComponent", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.DetailGridView)
], DetailGridViewComponent.prototype, "detailGridView", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], DetailGridViewComponent.prototype, "row", void 0);
DetailGridViewComponent = __decorate([
    core_1.Component({
        selector: 'detail-gridview',
        template: "<gridview [grid]='detailGridViewInstance'></gridview>",
    })
], DetailGridViewComponent);
exports.DetailGridViewComponent = DetailGridViewComponent;
//# sourceMappingURL=detail-gridview.component.js.map