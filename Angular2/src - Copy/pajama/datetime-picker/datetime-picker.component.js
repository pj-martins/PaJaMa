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
var moment = require("moment");
var noop = function () {
};
exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR = {
    provide: forms_1.NG_VALUE_ACCESSOR,
    useExisting: core_1.forwardRef(function () { return DateTimePickerComponent; }),
    multi: true
};
var DateTimePickerComponent = (function () {
    function DateTimePickerComponent(zone) {
        this.zone = zone;
        this.onTouchedCallback = noop;
        this.onChangeCallback = noop;
        this.dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        this.weekNumbers = [0, 1, 2, 3, 4, 5];
        this.months = [
            { number: 1, name: "January" },
            { number: 2, name: "February" },
            { number: 3, name: "March" },
            { number: 4, name: "April" },
            { number: 5, name: "May" },
            { number: 6, name: "June" },
            { number: 7, name: "July" },
            { number: 8, name: "August" },
            { number: 9, name: "September" },
            { number: 10, name: "October" },
            { number: 11, name: "November" },
            { number: 12, name: "December" }
        ];
        this.uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();
    }
    Object.defineProperty(DateTimePickerComponent.prototype, "dateFormat", {
        get: function () {
            return this._dateFormat;
        },
        set: function (f) {
            this._dateFormat = f;
            this.formatDate();
        },
        enumerable: true,
        configurable: true
    });
    DateTimePickerComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (!this.dateFormat) {
            this.dateFormat = "";
            if (!this.hideDate)
                this.dateFormat = "MM/DD/YYYY";
            if (!this.hideTime)
                this.dateFormat += " h:mm A";
            this.dateFormat = this.dateFormat.trim();
        }
        // TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
        // since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
        var self = this;
        this.currentonclick = document.onclick;
        document.onclick = function (event) {
            if (_this.currentonclick)
                _this.currentonclick(event);
            if (self.dropdownVisible && event.target) {
                var isInPicker = false;
                var curr = 3;
                var el = event.target;
                while (curr-- > 0 && el != null) {
                    if (el.className && el.className.indexOf("id_" + _this.uniqueId) >= 0) {
                        isInPicker = true;
                        break;
                    }
                    el = el.offsetParent;
                }
                if (!isInPicker)
                    self.zone.run(function () { return self.dropdownVisible = false; });
            }
        };
    };
    DateTimePickerComponent.prototype.formatDate = function () {
        if (!this.innerValue)
            this.formattedDate = "";
        else
            this.formattedDate = moment(new Date(this.innerValue)).format(this.dateFormat);
    };
    DateTimePickerComponent.prototype.getMinuteInt = function () {
        var currMinute = parseInt(this.selectedMinute);
        if (isNaN(currMinute))
            currMinute = 0;
        return currMinute;
    };
    DateTimePickerComponent.prototype.refreshCalendarDates = function () {
        if (!this.selectedDate)
            this.selectedDate = new Date();
        if (!this.selectedMonth)
            this.selectedMonth = this.selectedDate.getMonth() + 1;
        if (!this.selectedYear)
            this.selectedYear = this.selectedDate.getFullYear();
        if (!this.selectedHour) {
            this.selectedHour = this.selectedDate.getHours();
            if (this.selectedHour >= 12) {
                if (this.selectedHour > 12)
                    this.selectedHour -= 12;
                this.selectedAMPM = 'PM';
            }
            else {
                this.selectedAMPM = 'AM';
            }
        }
        if (!this.selectedMinute) {
            var minute = this.selectedDate.getMinutes();
            if (this.minuteStep > 1) {
                while (minute % this.minuteStep != 0) {
                    minute--;
                }
            }
            this.selectedMinute = minute.toString();
            this.formatMinute();
        }
        var startDate = new Date(this.selectedMonth.toString() + "/01/" + this.selectedYear.toString());
        while (startDate.getDay() > 0) {
            startDate.setDate(startDate.getDate() - 1);
        }
        this.calendarDates = [];
        for (var i = 0; i < 42; i++) {
            var weekNum = Math.floor(i / 7);
            if (!this.calendarDates[weekNum])
                this.calendarDates[weekNum] = [];
            this.calendarDates[weekNum][i % 7] = new Date(startDate.getTime());
            startDate.setDate(startDate.getDate() + 1);
        }
    };
    DateTimePickerComponent.prototype.formatMinute = function () {
        var currMinute = this.getMinuteInt();
        this.selectedMinute = "00".substring(0, 2 - currMinute.toString().length) + currMinute.toString();
    };
    DateTimePickerComponent.prototype.selectNow = function () {
        this.updateDateTimeControls(new Date());
    };
    DateTimePickerComponent.prototype.updateDateTimeControls = function (newDateTime) {
        this.selectedDate = newDateTime;
        this.selectedMonth = null;
        this.selectedYear = null;
        this.selectedHour = null;
        this.selectedMinute = null;
        this.selectedAMPM = null;
        this.refreshCalendarDates();
    };
    DateTimePickerComponent.prototype.blurEditor = function () {
        if (!this.inputChanged)
            return;
        this.inputChanged = false;
        if (this.formattedDate) {
            var date = new Date(this.formattedDate);
            if (isNaN(date.getTime())) {
                // might be just a time string
                var valid = false;
                var datePart = this.innerValue ? moment(this.innerValue).format('YYYY/MM/DD') : '1900/01/01';
                date = new Date(datePart + ' ' + this.formattedDate);
                valid = !isNaN(date.getTime());
                if (!valid) {
                    this.innerValue = null;
                    this.formatDate();
                    return;
                }
            }
            this.updateDateTimeControls(date);
            this.persistDate();
        }
    };
    DateTimePickerComponent.prototype.datesAreEqual = function (date) {
        if (!this.selectedDate)
            return false;
        return this.selectedDate.getDate() == date.getDate()
            && this.selectedDate.getMonth() == date.getMonth()
            && this.selectedDate.getFullYear() == date.getFullYear();
    };
    DateTimePickerComponent.prototype.addMonth = function (backwards) {
        this.selectedMonth += (backwards ? -1 : 1);
        if (this.selectedMonth <= 0) {
            this.selectedMonth = 12;
            this.selectedYear--;
        }
        else if (this.selectedMonth > 12) {
            this.selectedMonth = 1;
            this.selectedYear++;
        }
        this.refreshCalendarDates();
    };
    DateTimePickerComponent.prototype.addYear = function (backwards) {
        this.selectedYear += (backwards ? -1 : 1);
        this.refreshCalendarDates();
    };
    DateTimePickerComponent.prototype.addHour = function (backwards) {
        this.selectedHour += (backwards ? -1 : 1);
        var toggleAMPM = false;
        if (!backwards) {
            if (this.selectedHour > 12) {
                this.selectedHour = 1;
            }
            else if (this.selectedHour > 11) {
                toggleAMPM = true;
            }
        }
        else {
            if (this.selectedHour < 1) {
                this.selectedHour = 12;
            }
            else if (this.selectedHour == 11) {
                toggleAMPM = true;
            }
        }
        if (toggleAMPM) {
            this.selectedAMPM = this.selectedAMPM == 'AM' ? 'PM' : 'AM';
        }
    };
    DateTimePickerComponent.prototype.selectDate = function (date, fromInput) {
        if (fromInput === void 0) { fromInput = false; }
        if (!fromInput && ((this.minDate && date < this.minDate) || (this.maxDate && date > this.maxDate)))
            return;
        this.selectedDate = date;
        if (this.selectOnCalendarClick || fromInput) {
            this.persistDate(true, fromInput);
        }
    };
    DateTimePickerComponent.prototype.persistDate = function (alreadySelected, fromInput) {
        if (alreadySelected === void 0) { alreadySelected = false; }
        if (fromInput === void 0) { fromInput = false; }
        // add hours minutes, seconds
        this.dropdownVisible = false;
        var selectedDate = null;
        if (!this.hideDate) {
            if (!alreadySelected)
                this.selectDate(this.selectedDate);
            selectedDate = new Date(this.selectedDate.getTime());
        }
        else {
            selectedDate = new Date("1900/01/01");
        }
        if (!this.hideTime) {
            if (!fromInput) {
                var hourToAdd = this.selectedHour;
                if (this.selectedAMPM == 'PM' && hourToAdd < 12) {
                    hourToAdd += 12;
                }
                if (this.selectedAMPM == 'AM' && hourToAdd == 12) {
                    hourToAdd = 0;
                }
                selectedDate.setHours(hourToAdd);
                selectedDate.setMinutes(this.selectedMinute);
            }
        }
        else {
            selectedDate.setHours(0);
            selectedDate.setMinutes(0);
        }
        this.innerValue = selectedDate;
        this.formatDate();
        this.onChangeCallback(this.innerValue);
    };
    DateTimePickerComponent.prototype.addMinute = function (backwards) {
        var currMinute = this.getMinuteInt();
        currMinute += (backwards ? -1 : 1) * (this.minuteStep || 1);
        if (currMinute < 0) {
            currMinute = 60 - (this.minuteStep || 1);
            this.addHour(true);
        }
        else if (currMinute > 59) {
            currMinute = 0;
            this.addHour(false);
        }
        this.selectedMinute = currMinute.toString();
        this.formatMinute();
    };
    DateTimePickerComponent.prototype.writeValue = function (value) {
        if (value === undefined || value == null) {
            this.innerValue = null;
        }
        else if (value !== this.innerValue) {
            this.innerValue = value;
        }
        this.refreshCalendarDates();
        this.formatDate();
    };
    DateTimePickerComponent.prototype.registerOnChange = function (fn) {
        this.onChangeCallback = fn;
    };
    DateTimePickerComponent.prototype.registerOnTouched = function (fn) {
        this.onTouchedCallback = fn;
    };
    return DateTimePickerComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], DateTimePickerComponent.prototype, "placeholder", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], DateTimePickerComponent.prototype, "hideDate", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], DateTimePickerComponent.prototype, "hideTime", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], DateTimePickerComponent.prototype, "selectOnCalendarClick", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Date)
], DateTimePickerComponent.prototype, "minDate", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Date)
], DateTimePickerComponent.prototype, "maxDate", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], DateTimePickerComponent.prototype, "minuteStep", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], DateTimePickerComponent.prototype, "required", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String),
    __metadata("design:paramtypes", [String])
], DateTimePickerComponent.prototype, "dateFormat", null);
DateTimePickerComponent = __decorate([
    core_1.Component({
        moduleId: module.id,
        selector: 'datetime-picker',
        template: "<div class=\"datetime-picker id_{{uniqueId}}\">\n\t<div class=\"datetime-picker-input-container id_{{uniqueId}}\">\n\t\t<input type=\"text\" placeholder=\"{{placeholder}}\" [(ngModel)]=\"formattedDate\" (ngModelChange)=\"inputChanged=true\" (blur)=\"blurEditor()\" [required]=\"required\" />\n\t\t<div class=\"glyphicon glyphicon-calendar datetime-picker-clickable datetime-picker-calendar-icon\" (click)=\"dropdownVisible=!dropdownVisible\"></div>\n\t</div>\n\t<div class=\"datetime-picker-dropdown {{hideDate ? 'datetime-picker-timeonly-dropdown' : ''}} id_{{uniqueId}}\" *ngIf=\"dropdownVisible\">\n\t\t<div class=\"datetime-picker-container id_{{uniqueId}}\">\n\t\t\t<div class=\"datetime-picker-controls-panel row\" *ngIf=\"!hideDate\">\n\t\t\t\t<div class=\"col-md-4 datetime-picker-clear-right\">\n\t\t\t\t\t<select [(ngModel)]=\"selectedMonth\" (change)=\"refreshCalendarDates()\">\n\t\t\t\t\t\t<option *ngFor=\"let mo of months\" [ngValue]=\"mo.number\">{{mo.name.substring(0, 3)}}</option>\n\t\t\t\t\t</select>\n\t\t\t\t</div>\n\t\t\t\t<div class=\"col-md-4 datetime-picker-date-panel\">\n\t\t\t\t\t<input type=\"number\" [(ngModel)]=\"selectedYear\" (change)=\"refreshCalendarDates()\" />\n\t\t\t\t\t<div class=\"datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top\" (click)=\"addYear()\">\n\t\t\t\t\t</div>\n\t\t\t\t\t<div class=\"datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom\" (click)=\"addYear(true)\">\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\t\t\t\t<div class=\"col-md-1\"></div>\n\t\t\t\t<div class=\"col-md-1 datetime-picker-clickable datetime-picker-clear-right\">\n\t\t\t\t\t<span class=\"glyphicon glyphicon-triangle-left\" (click)=\"addMonth(true)\"></span>\n\t\t\t\t</div>\n\t\t\t\t<div class=\"col-md-1 datetime-picker-clickable\">\n\t\t\t\t\t<span class=\"glyphicon glyphicon-triangle-right\" (click)=\"addMonth()\"></span>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t\t<div class=\"datetime-picker-inner\" *ngIf=\"!hideDate\">\n\t\t\t\t<table class=\"datetime-picker-calendar-table id_{{uniqueId}}\">\n\t\t\t\t\t<tr class=\"datetime-picker-calendar-header-row\">\n\t\t\t\t\t\t<td *ngFor=\"let day of dayNames\" class=\"datetime-picker-calendar-header\">\n\t\t\t\t\t\t\t{{day.substring(0, 2)}}\n\t\t\t\t\t\t</td>\n\t\t\t\t\t</tr>\n\t\t\t\t\t<tr *ngFor=\"let weekNumber of weekNumbers\">\n\t\t\t\t\t\t<td *ngFor=\"let date of calendarDates[weekNumber]\" [ngClass]=\"'datetime-picker-calendar-day ' + ((!minDate || date >= minDate) && (!maxDate || date <= maxDate) ? 'datetime-picker-clickable ' : 'datetime-picker-disabled ') + (datesAreEqual(date) ? 'datetime-picker-selected' : '')\"\n\t\t\t\t\t\t\t\t(click)=\"selectDate(date)\">\n\t\t\t\t\t\t\t{{date | date: 'd'}}\n\t\t\t\t\t\t</td>\n\t\t\t\t\t</tr>\n\t\t\t\t</table>\n\t\t\t</div>\n\t\t\t<div class=\"datetime-picker-controls-panel row\" *ngIf=\"!hideTime\">\n\t\t\t\t<div class=\"col-md-12 datetime-picker-time-panel id_{{uniqueId}}\">\n\t\t\t\t\t<input type=\"text\" [(ngModel)]=\"selectedHour\" (keydown)=\"hourMinuteKeydown(12, selectedHour)\" />\n\t\t\t\t\t<div class=\"datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top\" (click)=\"addHour()\">\n\t\t\t\t\t</div>\n\t\t\t\t\t<div class=\"datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom\" (click)=\"addHour(true)\">\n\t\t\t\t\t</div>\n\t\t\t\t\t<input type=\"text\" [(ngModel)]=\"selectedMinute\" (keydown)=\"hourMinuteKeydown(59, selectedMinute)\" (blur)=\"formatMinute()\" />\n\t\t\t\t\t<div class=\"datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top\" (click)=\"addMinute()\">\n\t\t\t\t\t</div>\n\t\t\t\t\t<div class=\"datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom\" (click)=\"addMinute(true)\">\n\t\t\t\t\t</div>\n\t\t\t\t\t<select [(ngModel)]=\"selectedAMPM\">\n\t\t\t\t\t\t<option>AM</option>\n\t\t\t\t\t\t<option>PM</option>\n\t\t\t\t\t</select>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t\t<div class=\"datetime-picker-controls-panel row\">\n\t\t\t\t<div class=\"col-md-12 datetime-picker-buttons-panel id_{{uniqueId}}\">\n\t\t\t\t\t<button class=\"btn btn-default\" (click)=\"selectNow()\">\n\t\t\t\t\t\tNow\n\t\t\t\t\t</button>\n\t\t\t\t\t&nbsp;\n\t\t\t\t\t<button class=\"btn btn-primary\" (click)=\"persistDate()\">\n\t\t\t\t\t\tSelect\n\t\t\t\t\t</button>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</div>\n\t</div>\n</div>",
        providers: [exports.CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR],
        styleUrls: ['datetime-picker.css']
    }),
    __metadata("design:paramtypes", [core_1.NgZone])
], DateTimePickerComponent);
exports.DateTimePickerComponent = DateTimePickerComponent;
exports.MIN_VALIDATOR = {
    provide: forms_1.NG_VALIDATORS,
    useExisting: core_1.forwardRef(function () { return DateTimePickerMinValidator; }),
    multi: true
};
var DateTimePickerMinValidator = (function () {
    function DateTimePickerMinValidator() {
    }
    DateTimePickerMinValidator.prototype.validate = function (c) {
        if (!this.dateTimePickerMin)
            return null;
        if (c.value) {
            var dt = new Date(c.value);
            if (!isNaN(dt.getTime()) && dt < this.dateTimePickerMin) {
                return {
                    min: true
                };
            }
        }
        return null;
    };
    return DateTimePickerMinValidator;
}());
__decorate([
    core_1.Input("minDate"),
    __metadata("design:type", Date)
], DateTimePickerMinValidator.prototype, "dateTimePickerMin", void 0);
DateTimePickerMinValidator = __decorate([
    core_1.Directive({
        selector: '[minDate]',
        providers: [exports.MIN_VALIDATOR]
    })
], DateTimePickerMinValidator);
exports.DateTimePickerMinValidator = DateTimePickerMinValidator;
exports.MAX_VALIDATOR = {
    provide: forms_1.NG_VALIDATORS,
    useExisting: core_1.forwardRef(function () { return DateTimePickerMaxValidator; }),
    multi: true
};
var DateTimePickerMaxValidator = (function () {
    function DateTimePickerMaxValidator() {
    }
    DateTimePickerMaxValidator.prototype.validate = function (c) {
        if (!this.dateTimePickerMax)
            return null;
        if (c.value) {
            var dt = new Date(c.value);
            if (!isNaN(dt.getTime()) && dt > this.dateTimePickerMax) {
                return {
                    max: true
                };
            }
        }
        return null;
    };
    return DateTimePickerMaxValidator;
}());
__decorate([
    core_1.Input("maxDate"),
    __metadata("design:type", Date)
], DateTimePickerMaxValidator.prototype, "dateTimePickerMax", void 0);
DateTimePickerMaxValidator = __decorate([
    core_1.Directive({
        selector: '[maxDate]',
        providers: [exports.MAX_VALIDATOR]
    })
], DateTimePickerMaxValidator);
exports.DateTimePickerMaxValidator = DateTimePickerMaxValidator;
//# sourceMappingURL=datetime-picker.component.js.map