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
var common_1 = require('@angular/common');
var forms_1 = require('@angular/forms');
var gridview_component_1 = require('./gridview.component');
var detail_gridview_component_1 = require('./detail-gridview.component');
var gridview_rowtemplate_1 = require('./gridview.rowtemplate');
var gridview_filtercell_component_1 = require('./gridview-filtercell.component');
var gridview_cell_component_1 = require('./gridview-cell.component');
var gridview_cell_template_component_1 = require('./gridview-cell-template.component');
var gridview_cell_template_component_2 = require('./gridview-cell-template.component');
var gridview_headercell_component_1 = require('./gridview-headercell.component');
var gridview_pager_component_1 = require('./gridview-pager.component');
var pipes_module_1 = require('../pipes/pipes.module');
var typeahead_module_1 = require('../typeahead/typeahead.module');
var checklist_module_1 = require('../checklist/checklist.module');
var GridViewModule = (function () {
    function GridViewModule() {
    }
    GridViewModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                forms_1.FormsModule,
                pipes_module_1.PipesModule,
                checklist_module_1.CheckListModule,
                typeahead_module_1.TypeAheadModule
            ],
            declarations: [
                gridview_component_1.GridViewComponent,
                gridview_cell_component_1.GridViewCellComponent,
                gridview_filtercell_component_1.GridViewFilterCellComponent,
                gridview_pager_component_1.GridViewPagerComponent,
                gridview_headercell_component_1.GridViewHeaderCellComponent,
                gridview_rowtemplate_1.GridViewRowTemplateComponent,
                gridview_cell_template_component_2.GridViewCellTemplateComponent,
                detail_gridview_component_1.DetailGridViewComponent
            ],
            exports: [
                gridview_component_1.GridViewComponent,
                gridview_cell_component_1.GridViewCellComponent,
                gridview_filtercell_component_1.GridViewFilterCellComponent,
                gridview_rowtemplate_1.GridViewRowTemplateComponent,
                gridview_cell_template_component_2.GridViewCellTemplateComponent
            ],
            providers: [gridview_rowtemplate_1.GridViewRowTemplateBuilder, gridview_cell_template_component_1.GridViewCellTemplateBuilder] //, GridViewFilterCellTemplateBuilder]
        }), 
        __metadata('design:paramtypes', [])
    ], GridViewModule);
    return GridViewModule;
}());
exports.GridViewModule = GridViewModule;
//# sourceMappingURL=gridview.module.js.map