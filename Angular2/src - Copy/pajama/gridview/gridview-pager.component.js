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
var gridview_component_1 = require("./gridview.component");
var GridViewPagerComponent = (function () {
    function GridViewPagerComponent() {
        this.pageChanged = new core_1.EventEmitter();
        this.pageChanging = new core_1.EventEmitter();
        this.pagingType = gridview_1.PagingType;
        this.moreToLeft = false;
        this.moreToRight = false;
        this.pageSizes = [{ size: 10, label: '10' }, { size: 25, label: '25' }, { size: 50, label: '50' }, { size: 100, label: '100' }, { size: 0, label: 'All' }];
    }
    GridViewPagerComponent.prototype.setPage = function (page) {
        if (this.pageChanging)
            this.pageChanging.emit(null);
        if (page !== undefined) {
            if (page < 1)
                page = 1;
            var totalPages = this.getTotalPages();
            if (page > totalPages)
                page = totalPages;
            this.parentGridView.currentPage = page;
        }
        this.parentGridViewComponent.resetDisplayData();
        this.pageChanged.emit(page);
    };
    GridViewPagerComponent.prototype.getTotalPages = function () {
        var totalItems = (this.parentGridView.pagingType == gridview_1.PagingType.Auto ? (this.parentGridViewComponent.unpagedData ? this.parentGridViewComponent.unpagedData.length : 0) : this.parentGridView.totalRecords);
        var totalPages = Math.ceil(totalItems / this.parentGridView.pageSize);
        return totalPages;
    };
    GridViewPagerComponent.prototype.getPageArray = function () {
        var pageArray = [];
        var totalPages = this.getTotalPages();
        var start = 1;
        var end = totalPages > 10 ? 10 : totalPages;
        if (totalPages > 10 && this.parentGridView.currentPage > 5) {
            end = this.parentGridView.currentPage + 4;
            if (end > totalPages) {
                end = totalPages;
            }
            start = end - 9;
        }
        for (var i = start; i <= end; i++) {
            pageArray.push(i);
        }
        this.moreToRight = totalPages > end;
        this.moreToLeft = start > 1;
        return pageArray;
    };
    GridViewPagerComponent.prototype.gotoLast = function () {
        this.setPage(this.getTotalPages());
    };
    return GridViewPagerComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_component_1.GridViewComponent)
], GridViewPagerComponent.prototype, "parentGridViewComponent", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", gridview_1.GridView)
], GridViewPagerComponent.prototype, "parentGridView", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewPagerComponent.prototype, "pageChanged", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], GridViewPagerComponent.prototype, "pageChanging", void 0);
GridViewPagerComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'gridview-pager',
        styleUrls: ['gridview-pager.css'],
        template: "\n<div class='show-hide-animation grid-pagination' [hidden]='!(parentGridView.pagingType < 2 && parentGridViewComponent.displayData && parentGridView.data && parentGridView.data.length > 0)'>\n    <div [hidden]='parentGridView.pageSize <= 0 || (parentGridView.pagingType == pagingType.Auto ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords) <= parentGridView.pageSize'>\n        <ul class=\"pagination\">\n            <li (click)='setPage(1)'>First</li>\n            <li (click)='setPage(parentGridView.currentPage - 1)'>Previous</li>\n\t\t\t<li [style.display]=\"!moreToLeft?'none':'inline-block'\" (click)=\"setPage(parentGridView.currentPage - parentGridView.pageSize)\">...</li>\n            <li *ngFor=\"let p of getPageArray()\" [ngClass]=\"{'pagination-selected' : p == parentGridView.currentPage}\" (click)='setPage(p)'>{{p}}</li>\n\t\t\t<li [style.display]=\"!moreToRight?'none':'inline-block'\" (click)=\"setPage(parentGridView.currentPage + parentGridView.pageSize)\">...</li>\n            <li (click)='setPage(parentGridView.currentPage + 1)'>Next</li>\n            <li (click)='gotoLast()'>Last</li>\n        </ul>\n        <br />\n    </div>\n\t<div [hidden]=\"(parentGridView.pagingType == 0 ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords) <= 10\">\n\t\tShow: <select [(ngModel)]='parentGridView.pageSize' (ngModelChange)='setPage(1)'><option *ngFor='let ps of pageSizes' [value]=\"ps.size\">{{ps.label}}</option></select>&nbsp;&nbsp;&nbsp;&nbsp;\n\t\tShowing {{ parentGridViewComponent.displayData.length }} of {{ parentGridView.pagingType == 0 ? parentGridViewComponent.unpagedData.length : parentGridView.totalRecords }} total records.\n\t</div>\n</div>\n"
    })
], GridViewPagerComponent);
exports.GridViewPagerComponent = GridViewPagerComponent;
//# sourceMappingURL=gridview-pager.component.js.map