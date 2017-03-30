"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var multi_textbox_component_1 = require("../multi-textbox/multi-textbox.component");
var typeahead_component_1 = require("./typeahead.component");
var parser_service_1 = require("../services/parser.service");
var MultiTypeaheadComponent = (function (_super) {
    __extends(MultiTypeaheadComponent, _super);
    function MultiTypeaheadComponent(elementRef, parserService) {
        var _this = _super.call(this, elementRef) || this;
        _this.elementRef = elementRef;
        _this.parserService = parserService;
        _this.minLength = 1;
        _this.waitMs = 0;
        return _this;
    }
    Object.defineProperty(MultiTypeaheadComponent.prototype, "dataSource", {
        get: function () {
            return this.childTypeahead.dataSource;
        },
        set: function (val) {
            this.childTypeahead.dataSource = val;
        },
        enumerable: true,
        configurable: true
    });
    MultiTypeaheadComponent.prototype.itemSelected = function (item) {
        var _this = this;
        if (item) {
            this.items.push(item);
            window.setTimeout(function () {
                return _this.currText = "";
            }, 10);
            this.itemsChanged.emit(null);
            this.resize();
        }
    };
    MultiTypeaheadComponent.prototype.itemsAreEqual = function (item1, item2) {
        if (!this.displayMember)
            return item1 == item2;
        return this.parserService.getObjectValue(this.displayMember, item1) ==
            this.parserService.getObjectValue(this.displayMember, item2);
    };
    MultiTypeaheadComponent.prototype.getObjectValue = function (item) {
        if (!this.displayMember)
            return item;
        return this.parserService.getObjectValue(this.displayMember, item);
    };
    return MultiTypeaheadComponent;
}(multi_textbox_component_1.MultiTextboxComponent));
__decorate([
    core_1.ViewChild("childTypeahead"),
    __metadata("design:type", typeahead_component_1.TypeaheadComponent)
], MultiTypeaheadComponent.prototype, "childTypeahead", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], MultiTypeaheadComponent.prototype, "matchOn", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], MultiTypeaheadComponent.prototype, "minLength", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MultiTypeaheadComponent.prototype, "displayMember", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], MultiTypeaheadComponent.prototype, "waitMs", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], MultiTypeaheadComponent.prototype, "dataSource", null);
MultiTypeaheadComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'multi-typeahead',
        template: multi_textbox_component_1.PRE_INPUT +
            "<typeahead #childTypeahead (keydown)=\"keyDown($event, true)\" [matchOn]=\"matchOn\" [padLeft]=\"paddingLeft + 'px'\" [dataSource]='dataSource' (focus)='resize()' [displayMember]='displayMember' \n\t\t\t[valueMember]='valueMember' [minLength]='minLength' [(ngModel)]='currText' (itemSelected)='itemSelected($event)' [waitMs]='waitMs'></typeahead>"
            + multi_textbox_component_1.POST_INPUT,
        styleUrls: ['../multi-textbox/multi-textbox.css', 'typeahead.css']
    }),
    __metadata("design:paramtypes", [core_1.ElementRef, parser_service_1.ParserService])
], MultiTypeaheadComponent);
exports.MultiTypeaheadComponent = MultiTypeaheadComponent;
//# sourceMappingURL=multi-typeahead.component.js.map