"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var typeahead_component_1 = require("./typeahead.component");
var multi_typeahead_component_1 = require("./multi-typeahead.component");
var multi_textbox_module_1 = require("../multi-textbox/multi-textbox.module");
var parser_service_1 = require("../services/parser.service");
var TypeaheadModule = (function () {
    function TypeaheadModule() {
    }
    return TypeaheadModule;
}());
TypeaheadModule = __decorate([
    core_1.NgModule({
        imports: [
            common_1.CommonModule,
            forms_1.FormsModule,
            multi_textbox_module_1.MultiTextboxModule
        ],
        providers: [parser_service_1.ParserService],
        declarations: [
            typeahead_component_1.TypeaheadComponent,
            multi_typeahead_component_1.MultiTypeaheadComponent
        ],
        exports: [
            typeahead_component_1.TypeaheadComponent,
            multi_typeahead_component_1.MultiTypeaheadComponent
        ]
    })
], TypeaheadModule);
exports.TypeaheadModule = TypeaheadModule;
//# sourceMappingURL=typeahead.module.js.map