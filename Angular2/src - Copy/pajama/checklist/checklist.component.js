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
var CheckListItem = (function () {
    function CheckListItem(item) {
        this.item = item;
    }
    return CheckListItem;
}());
exports.CheckListItem = CheckListItem;
var CheckListComponent = (function () {
    function CheckListComponent(zone) {
        this.zone = zone;
        this.showFilterIcon = false;
        this.disableAll = false;
        this.selectionChanged = new core_1.EventEmitter();
        this.showMultiplesEllipses = false;
        this.displayItems = [];
        this.allSelected = false;
        this._value = [];
        this._items = [];
        this.uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();
    }
    Object.defineProperty(CheckListComponent.prototype, "selectedItems", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            this._value = v || [];
            this.updateSelection(true);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CheckListComponent.prototype, "items", {
        get: function () {
            return this._items;
        },
        set: function (v) {
            this._items = v || [];
            this.displayItems = [];
            for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                var i = _a[_i];
                this.displayItems.push(new CheckListItem(i));
            }
            this.updateSelection(true);
        },
        enumerable: true,
        configurable: true
    });
    CheckListComponent.prototype.updateSelection = function (init) {
        if (init === void 0) { init = false; }
        if (init) {
            var allSelected = true;
            for (var _i = 0, _a = this.displayItems; _i < _a.length; _i++) {
                var di = _a[_i];
                di.selected = false;
                if (this._value) {
                    for (var _b = 0, _c = this._value; _b < _c.length; _b++) {
                        var i = _c[_b];
                        if (this.equals(di.item, i)) {
                            di.selected = true;
                            break;
                        }
                    }
                    if (!di.selected)
                        allSelected = false;
                }
                else {
                    di.selected = true;
                }
            }
            this.allSelected = allSelected;
        }
        this.selectedText = "";
        // TODO: validate all is in items
        if (this.selectedItems) {
            if (this.items) {
                if (this.selectedItems.length >= this.items.length) {
                    return;
                }
            }
            if (this.showMultiplesEllipses && this.selectedItems.length > 1)
                this.selectedText = "(...)";
            else {
                for (var i = 0; i < this.selectedItems.length; i++) {
                    this.selectedText += (i == 0 ? "" : ", ") + (this.displayMember ? this.selectedItems[i][this.displayMember] : this.selectedItems[i]);
                }
            }
        }
    };
    // TODO: share
    CheckListComponent.prototype.equals = function (x, y) {
        if (x === y)
            return true;
        // if both x and y are null or undefined and exactly the same
        if (!(x instanceof Object) || !(y instanceof Object))
            return false;
        // if they are not strictly equal, they both need to be Objects
        if (x.constructor !== y.constructor)
            return false;
        // they must have the exact same prototype chain, the closest we can do is
        // test there constructor.
        var p;
        for (p in x) {
            if (!x.hasOwnProperty(p))
                continue;
            // other properties were tested using x.constructor === y.constructor
            if (!y.hasOwnProperty(p))
                return false;
            // allows to compare x[ p ] and y[ p ] when set to undefined
            if (x[p] === y[p])
                continue;
            // if they have the same strict value or identity then they are equal
            if (typeof (x[p]) !== "object")
                return false;
            // Numbers, Strings, Functions, Booleans must be strictly equal
            if (!this.equals(x[p], y[p]))
                return false;
        }
        for (p in y) {
            if (y.hasOwnProperty(p) && !x.hasOwnProperty(p))
                return false;
        }
        return true;
    };
    CheckListComponent.prototype.updateSelectedCollection = function (items) {
        for (var _i = 0, items_1 = items; _i < items_1.length; _i++) {
            var item = items_1[_i];
            var existingIndex = -1;
            if (!this.selectedItems)
                throw "selectedItems cannot be null or undefined initially";
            for (var i = 0; i < this.selectedItems.length; i++) {
                if (this.equals(this.selectedItems[i], item.item)) {
                    existingIndex = i;
                    break;
                }
            }
            if (item.selected && existingIndex < 0) {
                this.selectedItems.push(item.item);
            }
            else if (!item.selected && existingIndex >= 0) {
                this.selectedItems.splice(existingIndex, 1);
            }
        }
    };
    CheckListComponent.prototype.selectItem = function (item) {
        item.selected = !item.selected;
        if (!item.selected)
            this.allSelected = false;
        this.updateSelectedCollection([item]);
        this.updateSelection();
        this.selectionChanged.emit(null);
    };
    CheckListComponent.prototype.selectAll = function () {
        this.allSelected = !this.allSelected;
        for (var _i = 0, _a = this.displayItems; _i < _a.length; _i++) {
            var i = _a[_i];
            i.selected = this.allSelected;
        }
        this.updateSelectedCollection(this.displayItems);
        this.updateSelection();
        this.selectionChanged.emit(null);
    };
    CheckListComponent.prototype.ngOnInit = function () {
        var _this = this;
        // TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
        // since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
        var self = this;
        this.currentonclick = document.onclick;
        document.onclick = function (event) {
            if (_this.currentonclick)
                _this.currentonclick(event);
            if (self.dropdownVisible && event.target && event.target.className.indexOf("id_" + _this.uniqueId) < 0 && (!event.target.offsetParent || event.target.offsetParent.className.indexOf("id_" + _this.uniqueId) < 0)) {
                self.zone.run(function () { return self.dropdownVisible = false; });
            }
        };
    };
    return CheckListComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], CheckListComponent.prototype, "displayMember", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], CheckListComponent.prototype, "showFilterIcon", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], CheckListComponent.prototype, "disableAll", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], CheckListComponent.prototype, "selectionChanged", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], CheckListComponent.prototype, "showMultiplesEllipses", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], CheckListComponent.prototype, "selectedItems", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array),
    __metadata("design:paramtypes", [Array])
], CheckListComponent.prototype, "items", null);
CheckListComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'checklist',
        template: "\n    <div class='checklist checklist-container id_{{uniqueId}}'>\n\t\t<div class='checklist-input-container'>\n\t\t\t<!--<button (click)='dropdownVisible = !dropdownVisible' class=\"checklist-input\" [style.text-align]=\"showMultiplesEllipses ? 'center' : ''\">\n\t\t\t\t{{selectedText ? selectedText : '&nbsp;'}}\n\t\t\t</button>-->\n\t\t\t<input type='text' (click)='dropdownVisible = !dropdownVisible' [(ngModel)]='selectedText' [style.text-align]=\"showMultiplesEllipses ? 'center' : ''\" readonly />\n\t\t</div>\n\t\t<div class='checklist-button-container'>\n\t\t\t<button (click)='dropdownVisible = !dropdownVisible' class=\"checklist-button\">\n\t\t\t\t<div class=\"drop-down-image glyphicon {{ allSelected || !showFilterIcon ? 'glyphicon-chevron-down' : 'glyphicon-filter'}}\"></div>\n\t\t\t</button>\n\t\t</div>\n        <div class='checklist-dropdown' [hidden]='!dropdownVisible'>\n            <div *ngIf='!disableAll' (click)='selectAll()' class='checklist-item checklist-item-all id_{{uniqueId}}'>\n                <div class=\"checklist-check glyphicon {{ allSelected ? 'glyphicon-ok' : 'glyphicon-none'}}\"></div>(Select All)</div>\n                <div *ngFor='let item of displayItems' (click)='selectItem(item)' class='checklist id_{{uniqueId}}'>\n                <div class='checklist-item id_{{uniqueId}}'>\n                    <div class=\"checklist-check glyphicon {{ item.selected ? 'glyphicon-ok' : 'glyphicon-none'}}\"></div>{{displayMember ? item.item[displayMember] : item.item}}\n                </div>\n            </div>\n        </div>\n    </div>\n",
        styleUrls: ['checklist.css']
    }),
    __metadata("design:paramtypes", [core_1.NgZone])
], CheckListComponent);
exports.CheckListComponent = CheckListComponent;
//# sourceMappingURL=checklist.component.js.map