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
var forms_1 = require("@angular/forms");
var parser_service_1 = require("../services/parser.service");
var noop = function () {
};
exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR = {
    provide: forms_1.NG_VALUE_ACCESSOR,
    useExisting: core_1.forwardRef(function () { return TypeaheadComponent; }),
    multi: true
};
var TypeaheadComponent = (function () {
    function TypeaheadComponent(parserService, zone) {
        this.parserService = parserService;
        this.zone = zone;
        this.onTouchedCallback = noop;
        this.onChangeCallback = noop;
        this.valueWritten = false;
        this.isOpenByButton = false;
        this.loading = false;
        this.focus = new core_1.EventEmitter();
        this.items = [];
        this.activeIndex = -1;
        this.limitToList = true;
        this.minLength = 1;
        this.waitMs = 0;
        this.hideButton = false;
        this.itemSelected = new core_1.EventEmitter();
        this.typeaheadError = false;
        this.uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();
        this.dropdownVisible = false;
    }
    Object.defineProperty(TypeaheadComponent.prototype, "dataSource", {
        get: function () {
            return this._dataSource;
        },
        set: function (val) {
            this._dataSource = val;
            if (!val)
                return;
            if (val instanceof Array) {
                this.items = val;
            }
            else if (typeof val == "function") {
                this.dataSourceFunction = val;
            }
            else
                throw typeof val;
            if (this.valueWritten)
                this.setText();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TypeaheadComponent.prototype, "value", {
        get: function () {
            return this.innerValue;
        },
        set: function (v) {
            if (v !== this.innerValue) {
                this.innerValue = v;
                this.onChangeCallback(this.innerValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    TypeaheadComponent.prototype.ngOnInit = function () {
        var _this = this;
        // TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
        // since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
        var self = this;
        this.currentonclick = document.onclick;
        document.onclick = function (event) {
            if (_this.currentonclick)
                _this.currentonclick(event);
            if (self.dropdownVisible && event.target && event.target.className.indexOf("id_" + _this.uniqueId) < 0) {
                self.zone.run(function () {
                    self.dropdownVisible = false;
                    self.isOpenByButton = false;
                    var matchIndices = self.getMatchIndices();
                    if (matchIndices.length == 1) {
                        self.selectItem(self.items[matchIndices[0]]);
                    }
                    else if (_this.limitToList) {
                        self.value = null;
                        self.typeaheadError = true;
                    }
                    else {
                        self.value = _this.textValue;
                        _this.itemSelected.emit(_this.innerValue);
                    }
                });
            }
        };
    };
    TypeaheadComponent.prototype.selectItem = function (item) {
        this.typeaheadError = false;
        this.textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
        this.value = this.valueMember ? this.parserService.getObjectValue(this.valueMember, item) : item;
        this.isOpenByButton = false;
        this.dropdownVisible = false;
        this.itemSelected.emit(this.innerValue);
    };
    TypeaheadComponent.prototype.activateItem = function (index) {
        this.activeIndex = index;
    };
    TypeaheadComponent.prototype.openByButton = function () {
        this.dropdownVisible = !this.dropdownVisible;
        this.isOpenByButton = this.dropdownVisible;
        if (this.isOpenByButton && this.activeIndex < 0 && this.textValue) {
            for (var i = 0; i < this.items.length; i++) {
                var item = this.items[i];
                var objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
                if (objValue == this.textValue) {
                    this.activeIndex = i;
                    break;
                }
            }
        }
    };
    TypeaheadComponent.prototype.isMatch = function (item) {
        if (this.textValue) {
            var objValues = [];
            if (this.matchOn) {
                for (var _i = 0, _a = this.matchOn; _i < _a.length; _i++) {
                    var m = _a[_i];
                    var val = this.parserService.getObjectValue(m, item);
                    if (val)
                        objValues.push(val);
                }
            }
            else if (this.displayMember) {
                var val = this.parserService.getObjectValue(this.displayMember, item);
                if (val)
                    objValues.push(val);
            }
            else {
                objValues.push(item);
            }
            for (var _b = 0, objValues_1 = objValues; _b < objValues_1.length; _b++) {
                var objValue = objValues_1[_b];
                if (typeof objValue == "string" && typeof this.textValue == "string") {
                    var matchIndex = objValue.toLowerCase().indexOf(this.textValue.toLowerCase());
                    if (matchIndex >= 0)
                        return true;
                }
            }
        }
        return false;
    };
    TypeaheadComponent.prototype.getMatchIndices = function () {
        var indices = [];
        var matchCount = 0;
        for (var i = 0; i < this.items.length; i++) {
            var item = this.items[i];
            if (this.isMatch(item) || this.isOpenByButton)
                indices.push(i);
        }
        return indices;
    };
    TypeaheadComponent.prototype.valueChanged = function (event) {
        var _this = this;
        var charCode = event.which || event.keyCode;
        // which other char codes?
        if ((charCode >= 48 && charCode <= 90) || (charCode >= 96 && charCode <= 111) || (charCode >= 186 && charCode <= 222) || charCode == 8) {
            this._lastKeypress = new Date();
            this.isOpenByButton = false;
            this.value = null;
            var ddVisible_1 = this.textValue && this.textValue.length >= this.minLength;
            if (ddVisible_1 && this.dataSourceFunction != null) {
                var funcValue = this.dataSourceFunction(this.textValue);
                if (funcValue) {
                    if (funcValue instanceof Array) {
                        this.loading = true;
                        this.items = funcValue;
                        this.loading = false;
                        this.setActiveIndex(ddVisible_1);
                    }
                    else if (funcValue.subscribe) {
                        if (this.waitMs != 0) {
                            window.setTimeout(function () {
                                var now = new Date();
                                if (now.getTime() - _this._lastKeypress.getTime() >= _this.waitMs)
                                    _this.processObservableFunction(funcValue, ddVisible_1);
                            }, this.waitMs + 10);
                        }
                        else {
                            this.processObservableFunction(funcValue, ddVisible_1);
                        }
                    }
                    else
                        throw funcValue;
                }
            }
            else {
                this.setActiveIndex(ddVisible_1);
            }
        }
    };
    TypeaheadComponent.prototype.processObservableFunction = function (funcValue, ddVisible) {
        var _this = this;
        this.loading = true;
        funcValue.subscribe(function (r) {
            _this.items = r;
            _this.loading = false;
            _this.setActiveIndex(ddVisible);
        });
    };
    TypeaheadComponent.prototype.setActiveIndex = function (ddVisible) {
        if (ddVisible) {
            var indices = this.getMatchIndices();
            if (indices.length < 1)
                ddVisible = false;
            else if (indices.indexOf(this.activeIndex) < 0) {
                if (indices.length > 0)
                    this.activeIndex = indices[0];
                else
                    this.activeIndex = -1;
            }
        }
        this.dropdownVisible = ddVisible;
        if (!ddVisible) {
            this.activeIndex = -1;
        }
    };
    TypeaheadComponent.prototype.processSelection = function (charCode) {
        if (this.activeIndex >= 0) {
            this.selectItem(this.items[this.activeIndex]);
            if (charCode == 13)
                event.preventDefault();
        }
        else if (!this.limitToList) {
            this.value = this.textValue;
            this.itemSelected.emit(this.innerValue);
        }
        else if (!this.value && this.textValue) {
            this.typeaheadError = true;
        }
    };
    TypeaheadComponent.prototype.keydown = function (event) {
        this.typeaheadError = false;
        var charCode = event.which || event.keyCode;
        switch (charCode) {
            case 13:
            case 9:
                this.processSelection(charCode);
                break;
            case 40:
                if (!this.dropdownVisible && !this.isOpenByButton && !this.hideButton && !this.textValue) {
                    this.isOpenByButton = true;
                    this.dropdownVisible = true;
                }
                var indices = this.getMatchIndices();
                if (this.activeIndex < 0) {
                    this.activeIndex = indices[0];
                }
                else {
                    if (this.activeIndex == indices[indices.length - 1])
                        this.activeIndex = indices[0];
                    else {
                        var indicesIndex = 0;
                        for (var i = 0; i < indices.length; i++) {
                            if (indices[i] == this.activeIndex) {
                                this.activeIndex = indices[i + 1];
                                break;
                            }
                        }
                    }
                }
                event.preventDefault();
                break;
            case 38:
                if (!this.dropdownVisible && !this.isOpenByButton && !this.hideButton && !this.textValue) {
                    this.isOpenByButton = true;
                    this.dropdownVisible = true;
                }
                var indices2 = this.getMatchIndices();
                if (this.activeIndex < 0) {
                    this.activeIndex = indices[indices.length - 1];
                }
                else {
                    if (this.activeIndex == indices2[0])
                        this.activeIndex = indices2[indices2.length - 1];
                    else {
                        var indicesIndex = 0;
                        for (var i = 0; i < indices2.length; i++) {
                            if (indices2[i] == this.activeIndex) {
                                this.activeIndex = indices2[i - 1];
                                break;
                            }
                        }
                    }
                }
                event.preventDefault();
                break;
        }
    };
    TypeaheadComponent.prototype.itemHidden = function (item) {
        if (this.isOpenByButton)
            return false;
        return !this.isMatch(item);
    };
    TypeaheadComponent.prototype.getTextMatchIndex = function (item) {
        var objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
        if (this.textValue) {
            if (typeof objValue == "string" && typeof this.textValue == "string") {
                var matchIndex = objValue.toLowerCase().indexOf(this.textValue.toLowerCase());
                return matchIndex;
            }
        }
        return -1;
    };
    TypeaheadComponent.prototype.itemDisplay = function (item) {
        var objValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, item) : item;
        var matchIndex = this.getTextMatchIndex(item);
        if (matchIndex >= 0) {
            return objValue.substring(0, matchIndex) + '<strong>' + objValue.substring(matchIndex, matchIndex + this.textValue.length) + '</strong>'
                + objValue.substring(matchIndex + this.textValue.length);
        }
        return objValue;
    };
    TypeaheadComponent.prototype.hovered = function (index) {
        this.activeIndex = index;
    };
    TypeaheadComponent.prototype.writeValue = function (value) {
        if (value === undefined || value == null) {
            this.innerValue = null;
            this.setText();
        }
        else if (value !== this.innerValue) {
            this.innerValue = value;
            this.valueWritten = true;
            this.setText();
        }
    };
    TypeaheadComponent.prototype.setText = function () {
        if (this.innerValue === undefined || this.innerValue === null || this.innerValue === '') {
            this.textValue = '';
        }
        else if (!this.textValue && this.displayMember && !this.valueMember) {
            this.textValue = this.displayMember ? this.parserService.getObjectValue(this.displayMember, this.innerValue) : this.innerValue;
        }
        else if (!this.textValue && this.items && this.items.length > 0) {
            for (var _i = 0, _a = this.items; _i < _a.length; _i++) {
                var item = _a[_i];
                if (this.displayMember && this.valueMember) {
                    var val = this.parserService.getObjectValue(this.valueMember, item);
                    if (val == this.innerValue) {
                        this.textValue = this.parserService.getObjectValue(this.displayMember, item);
                        break;
                    }
                }
                else if (item == this.innerValue) {
                    this.textValue = item;
                    break;
                }
            }
        }
    };
    TypeaheadComponent.prototype.registerOnChange = function (fn) {
        this.onChangeCallback = fn;
    };
    TypeaheadComponent.prototype.registerOnTouched = function (fn) {
        this.onTouchedCallback = fn;
    };
    return TypeaheadComponent;
}());
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "focus", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], TypeaheadComponent.prototype, "required", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], TypeaheadComponent.prototype, "padLeft", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], TypeaheadComponent.prototype, "dataSource", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], TypeaheadComponent.prototype, "displayMember", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], TypeaheadComponent.prototype, "matchOn", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "limitToList", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], TypeaheadComponent.prototype, "valueMember", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "minLength", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "waitMs", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "hideButton", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], TypeaheadComponent.prototype, "itemSelected", void 0);
TypeaheadComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'typeahead',
        template: "\n\t\t<div class='typeahead-container'>\n\t\t\t<div class='typeahead-input-container'>\n\t\t\t\t<input type='text' [(ngModel)]='textValue' [required]='required' (keyup)='valueChanged($event)' (focus)='focus.emit(null)' [style.padding-left]='padLeft' (keydown)='keydown($event)' class='text_id_{{uniqueId}}' />\n\t\t\t</div>\n\t\t\t<div class='typeahead-button-container' *ngIf='!dataSourceFunction && !hideButton'>\n\t\t\t\t<button class='glyphicon glyphicon-chevron-down typeahead-button button_id_{{uniqueId}}' (click)='openByButton()' (keydown)='keydown($event)' tabindex=\"-1\"></button>\n\t\t\t</div>\n\t\t\t<div class='typeahead-button-container' *ngIf='dataSourceFunction && loading'>\n\t\t\t\t<span class='glyphicon glyphicon-refresh refresh-icon'></span>\n\t\t\t</div>\n\t\t</div>\n\t\t<div [hidden]='!dropdownVisible' class='typeahead-popup'>\n\t\t\t<div *ngFor='let item of items; let i = index' [hidden]='itemHidden(item)'>\n\t\t\t\t<div class='typeahead-item' class=\"typeahead-item {{activeIndex == i ? 'typeahead-item-selected' : ''}}\" (mouseover)='hovered(i)' (click)='selectItem(item)'>\n\t\t\t\t\t<div [innerHtml]='itemDisplay(item)'></div>\n\t\t\t\t</div>\n\t\t\t</div>\n        </div>\n\t\t<strong [hidden]='!typeaheadError' class='typeahead-error'>\n\t\t\tInvalid selection, please select item from list.\n\t\t</strong>\n        ",
        providers: [exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR],
        styleUrls: ['typeahead.css']
    }),
    __metadata("design:paramtypes", [parser_service_1.ParserService, core_1.NgZone])
], TypeaheadComponent);
exports.TypeaheadComponent = TypeaheadComponent;
//# sourceMappingURL=typeahead.component.js.map