"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Rx_1 = require("rxjs/Rx");
var DemoEditorsComponent = (function () {
    function DemoEditorsComponent() {
        var _this = this;
        this.multiTextboxItems = ['Item 1', 'Item 2'];
        this.multiTypeaheadItems = [];
        this.dataSource = ['Alpha', 'Bravo', 'Charlie', 'Delta', 'Echo', 'Foxtrot', 'Tango', 'Zulu'];
        this.selectedRoomIDs = [];
        this.selectedRooms = [];
        this.rooms = ROOMS;
        this.getRooms = function (partial) {
            var rooms = [];
            for (var _i = 0, _a = _this.rooms; _i < _a.length; _i++) {
                var r = _a[_i];
                if (r.roomName.toLowerCase().indexOf(partial.toLowerCase()) >= 0) {
                    rooms.push(r);
                }
            }
            return rooms;
        };
        this.getRoomsObservable = function (partial) {
            var rooms = _this.getRooms(partial);
            return Rx_1.Observable.create(function (o) { return o.next(rooms); });
        };
    }
    return DemoEditorsComponent;
}());
DemoEditorsComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'demo-editors',
        templateUrl: './demo-editors.component.html'
    })
], DemoEditorsComponent);
exports.DemoEditorsComponent = DemoEditorsComponent;
//# sourceMappingURL=demo-editors.component.js.map