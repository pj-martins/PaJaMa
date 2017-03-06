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
var gridview_1 = require('../../pajama/gridview/gridview');
var GridViewBasicDemoComponent = (function () {
    function GridViewBasicDemoComponent() {
        this.initGrid();
    }
    GridViewBasicDemoComponent.prototype.ngOnInit = function () {
        this.gridDemo.data = EVENTS;
    };
    GridViewBasicDemoComponent.prototype.initGrid = function () {
        this.gridDemo = new gridview_1.GridView();
        this.gridDemo.filterVisible = true;
        this.gridDemo.allowColumnOrdering = true;
        this.gridDemo.saveGridStateToStorage = true;
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
        var coordinatorCol = new gridview_1.DataColumn("coordinator");
        coordinatorCol.filterMode = gridview_1.FilterMode.DistinctList;
        coordinatorCol.sortable = true;
        this.gridDemo.columns.push(coordinatorCol);
        var phoneNumberCol = new gridview_1.DataColumn("phoneNumber");
        phoneNumberCol.width = "180px";
        this.gridDemo.columns.push(phoneNumberCol);
        var evtTypeCol = new gridview_1.DataColumn("hallEventType.eventTypeName", "Event Type");
        evtTypeCol.filterMode = gridview_1.FilterMode.Contains;
        evtTypeCol.allowSizing = true;
        this.gridDemo.columns.push(evtTypeCol);
        var requestedByCol = new gridview_1.DataColumn("requestedBy");
        requestedByCol.filterMode = gridview_1.FilterMode.DistinctList;
        requestedByCol.sortable = true;
        this.gridDemo.columns.push(requestedByCol);
    };
    GridViewBasicDemoComponent.prototype.pageChanged = function () {
    };
    GridViewBasicDemoComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'gridview-basic',
            template: "\n<gridview [grid]='gridDemo' (pageChanged)='pageChanged()'></gridview>\n"
        }), 
        __metadata('design:paramtypes', [])
    ], GridViewBasicDemoComponent);
    return GridViewBasicDemoComponent;
}());
exports.GridViewBasicDemoComponent = GridViewBasicDemoComponent;
//# sourceMappingURL=gridviewbasicdemo.component.js.map