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
var checklist_module_1 = require('./checklist/checklist.module');
var gridview_module_1 = require('./gridview/gridview.module');
var overlay_module_1 = require('./overlay/overlay.module');
var typeahead_module_1 = require('./typeahead/typeahead.module');
var PaJaMaModule = (function () {
    function PaJaMaModule() {
    }
    PaJaMaModule = __decorate([
        core_1.NgModule({
            imports: [
                gridview_module_1.GridViewModule,
                overlay_module_1.OverlayModule,
                checklist_module_1.CheckListModule,
                typeahead_module_1.TypeAheadModule
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], PaJaMaModule);
    return PaJaMaModule;
}());
exports.PaJaMaModule = PaJaMaModule;
//# sourceMappingURL=pajama.module.js.map