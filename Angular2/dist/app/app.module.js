"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var parser_service_1 = require("pajama/services/parser.service");
var app_component_1 = require("./app.component");
var gridview_module_1 = require("pajama/gridview/gridview.module");
var checklist_module_1 = require("pajama/checklist/checklist.module");
var overlay_module_1 = require("pajama/overlay/overlay.module");
var typeahead_module_1 = require("pajama/typeahead/typeahead.module");
var datetime_picker_module_1 = require("pajama/datetime-picker/datetime-picker.module");
var pipes_module_1 = require("pajama/pipes/pipes.module");
var app_routing_1 = require("./app.routing");
var demo_grid_component_1 = require("./demo/demo-grid.component");
var demo_editors_component_1 = require("./demo/demo-editors.component");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_1.HttpModule,
            gridview_module_1.GridViewModule,
            overlay_module_1.OverlayModule,
            typeahead_module_1.TypeaheadModule,
            pipes_module_1.PipesModule,
            checklist_module_1.CheckListModule,
            datetime_picker_module_1.DateTimePickerModule,
            app_routing_1.routing
        ],
        providers: [parser_service_1.ParserService],
        declarations: [
            app_component_1.AppComponent,
            demo_grid_component_1.DemoGridComponent,
            demo_editors_component_1.DemoEditorsComponent
        ],
        bootstrap: [app_component_1.AppComponent]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map