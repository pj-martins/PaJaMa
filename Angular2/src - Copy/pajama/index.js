"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var checklist_module_1 = require("./checklist/checklist.module");
var datetime_picker_module_1 = require("./datetime-picker/datetime-picker.module");
var expand_collapse_module_1 = require("./expand-collapse/expand-collapse.module");
var gridview_module_1 = require("./gridview/gridview.module");
var multi_textbox_module_1 = require("./multi-textbox/multi-textbox.module");
var overlay_module_1 = require("./overlay/overlay.module");
var typeahead_module_1 = require("./typeahead/typeahead.module");
__export(require("./checklist/checklist.module"));
__export(require("./checklist/checklist.component"));
__export(require("./datetime-picker/datetime-picker.module"));
__export(require("./datetime-picker/datetime-picker.component"));
__export(require("./expand-collapse/expand-collapse.module"));
__export(require("./expand-collapse/expand-collapse.component"));
__export(require("./gridview/gridview.module"));
__export(require("./gridview/gridview.component"));
__export(require("./gridview/gridview"));
__export(require("./multi-textbox/multi-textbox.module"));
__export(require("./multi-textbox/multi-textbox.component"));
__export(require("./overlay/overlay.module"));
__export(require("./overlay/overlay.component"));
__export(require("./typeahead/typeahead.module"));
var typeahead_component_1 = require("./typeahead/typeahead.component");
exports.TypeaheadComponent = typeahead_component_1.TypeaheadComponent;
__export(require("./typeahead/multi-typeahead.component"));
__export(require("./services/data.service"));
__export(require("./services/parser.service"));
__export(require("./pipes/pipes.module"));
__export(require("./pipes/enum-to-list.pipe"));
__export(require("./pipes/moment.pipe"));
__export(require("./pipes/order-by.pipe"));
__export(require("./pipes/to-camel-case.pipe"));
var PaJaMaModule = (function () {
    function PaJaMaModule() {
    }
    return PaJaMaModule;
}());
PaJaMaModule = __decorate([
    core_1.NgModule({
        imports: [
            checklist_module_1.CheckListModule,
            datetime_picker_module_1.DateTimePickerModule,
            expand_collapse_module_1.ExpandCollapseModule,
            gridview_module_1.GridViewModule,
            multi_textbox_module_1.MultiTextboxModule,
            overlay_module_1.OverlayModule,
            typeahead_module_1.TypeaheadModule
        ]
    })
], PaJaMaModule);
exports.PaJaMaModule = PaJaMaModule;
//# sourceMappingURL=index.js.map