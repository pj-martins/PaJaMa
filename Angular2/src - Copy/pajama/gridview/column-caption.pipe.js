"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ColumnCaptionPipe = (function () {
    function ColumnCaptionPipe() {
    }
    ColumnCaptionPipe.prototype.transform = function (column) {
        if (column.caption)
            return column.caption;
        var fieldName = column.fieldName;
        if (!fieldName || fieldName == '')
            return '';
        if (fieldName.lastIndexOf('.') > 0) {
            fieldName = fieldName.substring(fieldName.lastIndexOf('.') + 1, fieldName.length);
        }
        return fieldName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) {
            return str.toUpperCase();
        });
    };
    return ColumnCaptionPipe;
}());
ColumnCaptionPipe = __decorate([
    core_1.Pipe({ name: 'columnCaption' })
], ColumnCaptionPipe);
exports.ColumnCaptionPipe = ColumnCaptionPipe;
//# sourceMappingURL=column-caption.pipe.js.map