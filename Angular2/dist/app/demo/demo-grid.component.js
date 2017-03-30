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
var gridview_1 = require("pajama/gridview/gridview");
var typeahead_module_1 = require("pajama/typeahead/typeahead.module");
var multi_textbox_module_1 = require("pajama/multi-textbox/multi-textbox.module");
var DemoGridComponent = (function () {
    function DemoGridComponent() {
        this.initGrid();
    }
    DemoGridComponent.prototype.ngOnInit = function () {
        this.gridDemo.data = EVENTS;
        this._coordinatorColumn.customProps["coordinators"] = this.gridDemo.getDistinctValues(this._coordinatorColumn);
    };
    DemoGridComponent.prototype.initGrid = function () {
        this.gridDemo = new gridview_1.GridView();
        this.gridDemo.filterVisible = true;
        this.gridDemo.allowColumnOrdering = true;
        this.gridDemo.saveGridStateToStorage = true;
        this.gridDemo.allowColumnCustomization = true;
        this.gridDemo.name = "gridDemo";
        var custCol = new gridview_1.DataColumn("customer.customerName");
        custCol.filterMode = gridview_1.FilterMode.DistinctList;
        custCol.sortable = true;
        custCol.allowSizing = true;
        this.gridDemo.columns.push(custCol);
        var startCol = new gridview_1.DataColumn("eventStartDT", "Start");
        startCol.fieldType = gridview_1.FieldType.Date;
        startCol.sortable = true;
        startCol.sortDirection = gridview_1.SortDirection.Desc;
        startCol.width = "110px";
        this.gridDemo.columns.push(startCol);
        var endCol = new gridview_1.DataColumn("eventEndDT", "End");
        endCol.fieldType = gridview_1.FieldType.Date;
        endCol.width = "110px";
        this.gridDemo.columns.push(endCol);
        this._coordinatorColumn = new gridview_1.DataColumn("coordinator");
        this._coordinatorColumn.filterMode = gridview_1.FilterMode.Equals;
        this._coordinatorColumn.filterTemplate = new gridview_1.GridViewTemplate("\n<typeahead [(ngModel)]='column.filterValue' [dataSource]='column.customProps.coordinators'\n\t (itemSelected)=\"parentGridViewFilterCellComponent.filterChanged()\"\n\t (ngModelChange)=\"parentGridViewFilterCellComponent.filterChanged()\">\n</typeahead>", [typeahead_module_1.TypeaheadModule]);
        this._coordinatorColumn.sortable = true;
        this._coordinatorColumn.allowSizing = true;
        this.gridDemo.columns.push(this._coordinatorColumn);
        var phoneNumberCol = new gridview_1.DataColumn("phoneNumber");
        phoneNumberCol.width = "160px";
        phoneNumberCol.filterMode = gridview_1.FilterMode.Contains;
        this.gridDemo.columns.push(phoneNumberCol);
        var evtTypeCol = new gridview_1.DataColumn("hallEventType.eventTypeName", "Event Type");
        evtTypeCol.filterMode = gridview_1.FilterMode.DynamicList;
        evtTypeCol.filterValue = [];
        evtTypeCol.filterTemplate = new gridview_1.GridViewTemplate("\n<multi-textbox [items]='column.filterValue' (itemsChanged)=\"parentGridViewFilterCellComponent.filterChanged()\"></multi-textbox>\n", [multi_textbox_module_1.MultiTextboxModule]);
        evtTypeCol.allowSizing = true;
        this.gridDemo.columns.push(evtTypeCol);
        var requestedByCol = new gridview_1.DataColumn("requestedBy");
        requestedByCol.filterMode = gridview_1.FilterMode.DistinctList;
        requestedByCol.filterValue = [];
        requestedByCol.filterTemplate = new gridview_1.GridViewTemplate("\n<multi-typeahead [items]='column.filterValue' (itemsChanged)=\"parentGridViewFilterCellComponent.filterChanged()\" [dataSource]='column.filterOptions'></multi-typeahead>\n", [typeahead_module_1.TypeaheadModule]);
        requestedByCol.sortable = true;
        this.gridDemo.columns.push(requestedByCol);
        this.gridDemo.loadGridState();
    };
    DemoGridComponent.prototype.pageChanged = function () {
    };
    return DemoGridComponent;
}());
DemoGridComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'demo-grid',
        template: "\n<gridview [grid]='gridDemo' (pageChanged)='pageChanged()'></gridview>\n"
    }),
    __metadata("design:paramtypes", [])
], DemoGridComponent);
exports.DemoGridComponent = DemoGridComponent;
//# sourceMappingURL=demo-grid.component.js.map