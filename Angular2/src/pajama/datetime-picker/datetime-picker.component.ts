﻿import { Component, Input, Output, EventEmitter, OnInit, forwardRef, NgZone, Directive, Attribute, ElementRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, AbstractControl, NG_VALIDATORS, Validator, FormControl } from '@angular/forms';
import * as moment from 'moment'
import { Utils } from '../shared';

@Component({
	moduleId: module.id,
	selector: 'datetime-picker',
	template: `
<div class='datetime-picker'>
	<div class="input-button-container component id_{{uniqueId}}">
		<button class="input-button datetime-picker-button id_{{uniqueId}}" (click)="showDropdown()">
			<div class="datetime-picker-calendar-icon glyphicon glyphicon-calendar id_{{uniqueId}}"></div>
		</button>
	</div>
	<div class="datetime-picker-dropdown component {{hideDate ? 'datetime-picker-timeonly-dropdown' : ''}} id_{{uniqueId}}" *ngIf="dropdownVisible">
		<div class="datetime-picker-container id_{{uniqueId}}">
			<div class="datetime-picker-controls-panel" *ngIf="!hideDate">
				<div class="datetime-picker-date-panel id_{{uniqueId}}">
					<select [(ngModel)]="selectedMonth" (change)="refreshCalendarDates()">
						<option *ngFor="let mo of months" [ngValue]="mo.number" class="month-option id_{{uniqueId}}">{{mo.name.substring(0, 3)}}</option>
					</select>
					<input type="number" [(ngModel)]="selectedYear" (change)="refreshCalendarDates()" />
					<div class="arrow-up-down-container">
						<div class="datetime-picker-top-spinner datetime-picker-clickable icon-arrow-up-black spinner-arrows" (click)="addYear()">
						</div>
						<div class="datetime-picker-bottom-spinner datetime-picker-clickable icon-arrow-down-black spinner-arrows" (click)="addYear(true)">
						</div>
					</div>
					<div class="arrow-left-right-container">
						<div class="icon-arrow-left-black icon-small datetime-picker-clickable" (click)="addMonth(true)"></div>
						<div class="icon-arrow-right-black icon-small datetime-picker-clickable" (click)="addMonth()"></div>
					</div>
				</div>
			</div>
			<div class="datetime-picker-inner" *ngIf="!hideDate">
				<table class="datetime-picker-calendar-table id_{{uniqueId}}" cellspacing=0>
					<tr class="datetime-picker-calendar-header-row">
						<td *ngFor="let day of dayNames" class="datetime-picker-calendar-header">
							{{day.substring(0, 2)}}
						</td>
					</tr>
					<tr *ngFor="let weekNumber of weekNumbers">
						<td *ngFor="let date of calendarDates[weekNumber]" [ngClass]="'datetime-picker-calendar-day ' + ((!minDate || date >= minDate) && (!maxDate || date <= maxDate) ? 'datetime-picker-clickable ' : 'datetime-picker-disabled ') + (datesAreEqual(date) ? 'datetime-picker-selected' : '')"
								(click)="selectDate(date)">
							{{date | date: 'd'}}
						</td>
					</tr>
				</table>
			</div>
			<div class="datetime-picker-controls-panel" *ngIf="!hideTime">
				<div class="datetime-picker-time-panel id_{{uniqueId}}">
					<input type="text" [(ngModel)]="selectedHour" (keydown)="hourMinuteKeydown(12, selectedHour)" />
					<div class="arrow-up-down-container">
						<div class="datetime-picker-top-spinner datetime-picker-clickable icon-arrow-up-black spinner-arrows" (click)="addHour()">
						</div>
						<div class="datetime-picker-bottom-spinner datetime-picker-clickable icon-arrow-down-black spinner-arrows" (click)="addHour(true)">
						</div>
					</div>
					<input type="text" [(ngModel)]="selectedMinute" (keydown)="hourMinuteKeydown(59, selectedMinute)" (blur)="formatMinute()" />
					<div class="arrow-up-down-container">
						<div class="datetime-picker-top-spinner datetime-picker-clickable icon-arrow-up-black spinner-arrows" (click)="addMinute()">
						</div>
						<div class="datetime-picker-bottom-spinner datetime-picker-clickable icon-arrow-down-black spinner-arrows" (click)="addMinute(true)">
						</div>
					</div>
					<select [(ngModel)]="selectedAMPM">
						<option class="ampm-option id_{{uniqueId}}" [ngValue]="'AM'">AM</option>
						<option class="ampm-option id_{{uniqueId}}" [ngValue]="'PM'">PM</option>
					</select>
				</div>
			</div>
			<div class="datetime-picker-controls-panel">
				<div class="datetime-picker-buttons-panel id_{{uniqueId}}">
					<button class="btn btn-small btn-primary" (click)="selectNow()">
						Now
					</button>
					&nbsp;
					<button class="btn btn-small btn-primary" (click)="persistDate()">
						Select
					</button>
				</div>
			</div>
		</div>
	</div>
</div>
`,
	styleUrls: ['../styles.css', 'datetime-picker.css']
})
export class DateTimePickerComponent implements OnInit { // implements ControlValueAccessor, OnInit {

	@Input() hideDate: boolean;
	@Input() hideTime: boolean;
	selectOnCalendarClick: boolean;
	minDate: Date;
	maxDate: Date;
	minuteStep: number;

	dateChanged = new EventEmitter<Date>();

	private _lock = false;
	private _innerValue: Date;
	private get innerValue(): Date {
		return this._innerValue;
	}
	private set innerValue(v: Date) {
		this._innerValue = v;
		if (!this._lock)
			this.dateChanged.emit(v);
	}

	private selectedDate: Date;
	private currentonclick: any;

	protected dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
	protected weekNumbers = [0, 1, 2, 3, 4, 5];
	protected months = [
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

	protected calendarDates: Array<Array<Date>>;
	protected selectedMonth: number;
	protected selectedYear: number;
	protected selectedHour: number;
	protected selectedMinute: string;
	protected selectedAMPM: string;
	protected dropdownVisible: boolean = false;
	protected uniqueId = Utils.newGuid();

	constructor(private zone: NgZone, public elementRef: ElementRef) { }

	ngOnInit() {
		// TODO: hackish, need to find a better way to hide drop down when they click off of it, can't use blur
		// since blur will fire when the dropdown div is clicked in which case we don't want to hide the dropdown
		let self = this;
		this.currentonclick = document.onclick;
		document.onclick = (event: any) => {
			if (this.currentonclick) this.currentonclick(event);

			if (self.dropdownVisible && event.target) {
				let isInPicker = false;
				let curr = 3;
				let el = event.target;
				while (curr-- > 0 && el != null) {
					if (el.className && el.className.indexOf(`id_${this.uniqueId}`) >= 0) {
						isInPicker = true;
						break;
					}
					el = el.offsetParent;
				}
				if (!isInPicker)
					self.zone.run(() => self.dropdownVisible = false);
			}
		};
	}

	private getMinuteInt() {
		let currMinute = parseInt(this.selectedMinute);
		if (isNaN(currMinute))
			currMinute = 0;
		return currMinute;
	}

	refreshCalendarDates() {
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

		let startDate = new Date(this.selectedMonth.toString() + "/01/" + this.selectedYear.toString());
		while (startDate.getDay() > 0) {
			startDate.setDate(startDate.getDate() - 1);
		}

		this.calendarDates = [];
		for (let i = 0; i < 42; i++) {
			let weekNum = Math.floor(i / 7);
			if (!this.calendarDates[weekNum])
				this.calendarDates[weekNum] = [];
			this.calendarDates[weekNum][i % 7] = new Date(startDate.getTime());
			startDate.setDate(startDate.getDate() + 1);
		}
	}

	protected formatMinute() {
		let currMinute = this.getMinuteInt();
		this.selectedMinute = "00".substring(0, 2 - currMinute.toString().length) + currMinute.toString();
	}

	protected selectNow() {
		this.updateDateTimeControls(new Date());
	}

	protected showDropdown() {
		this.dropdownVisible = !this.dropdownVisible;
		if (this.innerValue)
			this.updateDateTimeControls(this.innerValue);
	}

	updateDateTimeControls(newDateTime: Date) {
		this.selectedDate = newDateTime;
		this.selectedMonth = null;
		this.selectedYear = null;
		this.selectedHour = null;
		this.selectedMinute = null;
		this.selectedAMPM = null;
		this.refreshCalendarDates();
	}

	protected datesAreEqual(date: Date) {
		if (!this.selectedDate) return false;
		return this.selectedDate.getDate() == date.getDate()
			&& this.selectedDate.getMonth() == date.getMonth()
			&& this.selectedDate.getFullYear() == date.getFullYear();
	}

	protected addMonth(backwards) {
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
	}

	protected addYear(backwards) {
		this.selectedYear += (backwards ? -1 : 1);
		this.refreshCalendarDates();
	}

	protected addHour(backwards) {
		this.selectedHour += (backwards ? -1 : 1);
		let toggleAMPM = false;
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
	}

	protected selectDate(date, fromInput = false) {
		if (!fromInput && ((this.minDate && date < this.minDate) || (this.maxDate && date > this.maxDate)))
			return;

		this.selectedDate = date;
		if (this.selectOnCalendarClick || fromInput) {
			this.persistDate(true, fromInput);
		}
	}

	persistDate(alreadySelected = false, fromInput = false) {
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
	}

	protected addMinute(backwards) {
		let currMinute = this.getMinuteInt();
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
	}

	writeValue(value: Date) {
		this._lock = true;
		if (value === undefined || value == null) {
			this.innerValue = null;
		}
		else if (value !== this.innerValue) {
			this.innerValue = value;
		}
		this._lock = false;
		this.refreshCalendarDates();
	}
}

//export const MIN_VALIDATOR: any = {
//	provide: NG_VALIDATORS,
//	useExisting: forwardRef(() => DateTimePickerMinValidator),
//	multi: true
//};

//@Directive({
//	selector: '[minDate]',
//	providers: [MIN_VALIDATOR]
//})
//export class DateTimePickerMinValidator implements Validator {
//	@Input("minDate") dateTimePickerMin: Date;

//	validate(c: AbstractControl) {
//		if (!this.dateTimePickerMin) return null;
//		if (c.value) {
//			let dt = new Date(c.value);
//			if (!isNaN(dt.getTime()) && dt < this.dateTimePickerMin) {
//				return {
//					min: true
//				};
//			}
//		}

//		return null;
//	}
//}


//export const MAX_VALIDATOR: any = {
//	provide: NG_VALIDATORS,
//	useExisting: forwardRef(() => DateTimePickerMaxValidator),
//	multi: true
//};

//@Directive({
//	selector: '[maxDate]',
//	providers: [MAX_VALIDATOR]
//})
//export class DateTimePickerMaxValidator implements Validator {
//	@Input("maxDate") dateTimePickerMax: Date;

//	validate(c: AbstractControl) {
//		if (!this.dateTimePickerMax) return null;
//		if (c.value) {
//			let dt = new Date(c.value);
//			if (!isNaN(dt.getTime()) && dt > this.dateTimePickerMax) {
//				return {
//					max: true
//				};
//			}
//		}

//		return null;
//	}
//}
