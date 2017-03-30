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
exports.PRE_INPUT = "<div class='multi-textbox-container'>\n\t<div class='multi-textbox-item-container'>\n\t\t<div *ngFor='let item of items || []' class='multi-textbox-item'>\n\t\t\t{{getObjectValue(item)}}\n\t\t\t<div class='glyphicon glyphicon-remove multi-textbox-remove' (click)='removeItem(item)'></div>\n\t\t</div>\n\t</div>\n\t<div class='multi-textbox-input-container'>\n";
exports.POST_INPUT = "\n\t</div>\n\t<div class='multi-textbox-button-container' [style.display]=\"currText ? 'inline' : 'none'\">\n\t\t<button (click)='addItem()' class=\"multi-textbox-button\" tabindex=\"-1\">\n\t\t\t<div class=\"glyphicon glyphicon-plus\"></div>\n\t\t</button>\n\t</div>\n</div>";
var MultiTextboxComponent = (function () {
    function MultiTextboxComponent(elementRef) {
        this.elementRef = elementRef;
        this._items = [];
        this.itemsChanged = new core_1.EventEmitter();
        this._originalPaddingLeft = 0;
        this._paddingLeft = 0;
    }
    Object.defineProperty(MultiTextboxComponent.prototype, "items", {
        get: function () {
            return this._items;
        },
        set: function (v) {
            this._items = v || [];
        },
        enumerable: true,
        configurable: true
    });
    MultiTextboxComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(MultiTextboxComponent.prototype, "paddingLeft", {
        get: function () {
            return this._paddingLeft;
        },
        set: function (p) {
            this._originalPaddingLeft = p;
            this._paddingLeft = p;
        },
        enumerable: true,
        configurable: true
    });
    MultiTextboxComponent.prototype.resize = function () {
        var items = this.elementRef.nativeElement.getElementsByClassName("multi-textbox-item");
        var totWidth = 0;
        for (var _i = 0, items_1 = items; _i < items_1.length; _i++) {
            var item = items_1[_i];
            totWidth += item.offsetWidth + 1;
        }
        this._paddingLeft = this._originalPaddingLeft + totWidth;
    };
    MultiTextboxComponent.prototype.removeItem = function (item) {
        for (var i = this._items.length - 1; i >= 0; i--) {
            if (this.itemsAreEqual(this._items[i], item)) {
                this._items.splice(i, 1);
                this.itemsChanged.emit(null);
                break;
            }
        }
        this.resize();
    };
    MultiTextboxComponent.prototype.itemsAreEqual = function (item1, item2) {
        return item1 == item2;
    };
    MultiTextboxComponent.prototype.addItem = function () {
        if (this.currText) {
            this._items.push(this.currText);
            this.currText = "";
            this.itemsChanged.emit(null);
        }
        this.resize();
    };
    MultiTextboxComponent.prototype.keyDown = function (event, ignoreEnter) {
        var _this = this;
        var charCode = event.which || event.keyCode;
        if (charCode == 8) {
            if (!this.currText && this._items.length > 0) {
                this._items.splice(this._items.length - 1, 1);
                this.itemsChanged.emit(null);
            }
        }
        else if (charCode == 13 && this.currText && !ignoreEnter) {
            this.addItem();
            event.preventDefault();
        }
        window.setTimeout(function () { return _this.resize(); }, 100);
    };
    MultiTextboxComponent.prototype.blur = function () {
        this.addItem();
        this.resize();
    };
    MultiTextboxComponent.prototype.getObjectValue = function (item) {
        return item;
    };
    return MultiTextboxComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Array),
    __metadata("design:paramtypes", [Array])
], MultiTextboxComponent.prototype, "items", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], MultiTextboxComponent.prototype, "itemsChanged", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Number])
], MultiTextboxComponent.prototype, "paddingLeft", null);
MultiTextboxComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'multi-textbox',
        template: exports.PRE_INPUT + "\n\t\t<input type='text' [style.padding-left]=\"paddingLeft + 'px'\" class='multi-textbox-input' [(ngModel)]='currText' (keydown)=\"keyDown($event, false)\" (blur)='blur()' (focus)=\"resize()\" />\n\t" + exports.POST_INPUT,
        styleUrls: ['multi-textbox.css']
    }),
    __metadata("design:paramtypes", [core_1.ElementRef])
], MultiTextboxComponent);
exports.MultiTextboxComponent = MultiTextboxComponent;
//# sourceMappingURL=multi-textbox.component.js.map