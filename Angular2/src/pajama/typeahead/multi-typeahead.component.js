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
var forms_1 = require('@angular/forms');
var parser_service_1 = require('../services/parser.service');
var typeahead_component_1 = require('./typeahead.component');
var noop = function () {
};
exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR = {
    provide: forms_1.NG_VALUE_ACCESSOR,
    useExisting: core_1.forwardRef(function () { return MultiTypeaheadComponent; }),
    multi: true
};
var MultiTypeaheadComponent = (function () {
    function MultiTypeaheadComponent(parserService) {
        var _this = this;
        this.parserService = parserService;
        this.onTouchedCallback = noop;
        this.onChangeCallback = noop;
        this.getItems = function (partial) {
            var parts = partial.split(',');
            if (parts.length < 1)
                return [];
            var lastItem = parts[parts.length - 1];
            var matches = new Array();
            for (var _i = 0, _a = _this.dataSource; _i < _a.length; _i++) {
                var ds = _a[_i];
                if (ds.toLowerCase().indexOf(lastItem.toLowerCase()) >= 0) {
                    matches.push(ds);
                }
            }
            return matches;
        };
    }
    MultiTypeaheadComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(MultiTypeaheadComponent.prototype, "itemString", {
        get: function () {
            return this._itemString;
        },
        set: function (v) {
            this._itemString = v;
            this.childTypeAhead.textValue = this.lastItem;
            var val = [];
            if (this.itemString)
                val = this.itemString.split(',');
            this.onChangeCallback(val);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiTypeaheadComponent.prototype, "lastItem", {
        get: function () {
            if (!this.itemString)
                return '';
            var parts = this.itemString.split(',');
            return parts[parts.length - 1];
        },
        set: function (i) {
            // do nothing, typeahead is hidden
        },
        enumerable: true,
        configurable: true
    });
    // TODO: valuemember, displaymember
    MultiTypeaheadComponent.prototype.itemSelected = function (item) {
        for (var i = this.itemString.length - 1; i >= 0; i--) {
            if (i == 0) {
                this.itemString = item;
            }
            else if (this.itemString[i] == ',') {
                this.itemString = this.itemString.substring(0, i + 1) + item;
                break;
            }
        }
    };
    MultiTypeaheadComponent.prototype.writeValue = function (value) {
        this.itemString = value ? value.join(',') : '';
    };
    MultiTypeaheadComponent.prototype.registerOnChange = function (fn) {
        this.onChangeCallback = fn;
    };
    MultiTypeaheadComponent.prototype.registerOnTouched = function (fn) {
        this.onTouchedCallback = fn;
    };
    MultiTypeaheadComponent.prototype.keydown = function (evt) {
        var childDropdownVisible = this.childTypeAhead.dropdownVisible;
        var charCode = evt.which || evt.keyCode;
        var isComma = false;
        if (charCode == 188) {
            this.childTypeAhead.processSelection(9);
            isComma = true;
        }
        else {
            this.childTypeAhead.keydown(evt);
        }
        if (!isComma && childDropdownVisible && charCode == 9)
            evt.preventDefault();
    };
    MultiTypeaheadComponent.prototype.valueChanged = function (evt) {
        this.childTypeAhead.valueChanged(evt);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], MultiTypeaheadComponent.prototype, "dataSource", void 0);
    __decorate([
        core_1.ViewChild("childTypeahead"), 
        __metadata('design:type', typeahead_component_1.TypeaheadComponent)
    ], MultiTypeaheadComponent.prototype, "childTypeAhead", void 0);
    MultiTypeaheadComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'multi-typeahead',
            template: "<input type='text' class='form-control' (keydown)='keydown($event)' (keyup)='valueChanged($event)' [(ngModel)]='itemString' />\n<typeahead #childTypeahead [(ngModel)]='lastItem' [limitToList]='false' [isForMulti]='true' class='multi-typeahead' [dataSource]='getItems' (itemSelected)='itemSelected($event)'></typeahead>",
            providers: [exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
        }), 
        __metadata('design:paramtypes', [parser_service_1.ParserService])
    ], MultiTypeaheadComponent);
    return MultiTypeaheadComponent;
}());
exports.MultiTypeaheadComponent = MultiTypeaheadComponent;
//# sourceMappingURL=multi-typeahead.component.js.map