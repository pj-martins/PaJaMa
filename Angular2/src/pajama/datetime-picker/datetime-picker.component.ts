﻿import { Component, Input, Output, EventEmitter, OnInit, forwardRef, NgZone } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import * as moment from 'moment'

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => DateTimePickerComponent),
	multi: true
};

@Component({
	moduleId: module.id,
	selector: 'datetime-picker',
	template: `<div class="datetime-picker id_{{uniqueId}}">
	<div class="datetime-picker-input-container id_{{uniqueId}}">
		<input type="text" name="{{name}}" placeholder="{{placeholder}}" [(ngModel)]="formattedDate" (ngModelChange)="inputChanged=true" (blur)="blurEditor()" [required]="required" />
		<div class="glyphicon glyphicon-calendar datetime-picker-clickable datetime-picker-calendar-icon" (click)="dropdownVisible=!dropdownVisible"></div>
	</div>
	<div class="datetime-picker-dropdown {{hideDate ? 'datetime-picker-timeonly-dropdown' : ''}} id_{{uniqueId}}" *ngIf="dropdownVisible">
		<div class="datetime-picker-container id_{{uniqueId}}">
			<div class="datetime-picker-controls-panel row" *ngIf="!hideDate">
				<div class="col-md-4 datetime-picker-clear-right">
					<select [(ngModel)]="selectedMonth" (change)="refreshCalendarDates()">
						<option *ngFor="let mo of months" [ngValue]="mo.number">{{mo.name.substring(0, 3)}}</option>
					</select>
				</div>
				<div class="col-md-4 datetime-picker-date-panel">
					<input type="number" [(ngModel)]="selectedYear" (change)="refreshCalendarDates()" />
					<div class="datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top" (click)="addYear()">
					</div>
					<div class="datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom" (click)="addYear(true)">
					</div>
				</div>
				<div class="col-md-1"></div>
				<div class="col-md-1 datetime-picker-clickable datetime-picker-clear-right">
					<span class="glyphicon glyphicon-triangle-left" (click)="addMonth(true)"></span>
				</div>
				<div class="col-md-1 datetime-picker-clickable">
					<span class="glyphicon glyphicon-triangle-right" (click)="addMonth()"></span>
				</div>
			</div>
			<div class="datetime-picker-inner" *ngIf="!hideDate">
				<table class="datetime-picker-calendar-table id_{{uniqueId}}">
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
			<div class="datetime-picker-controls-panel row" *ngIf="!hideTime">
				<div class="col-md-12 datetime-picker-time-panel id_{{uniqueId}}">
					<input type="text" [(ngModel)]="selectedHour" (keydown)="hourMinuteKeydown(12, selectedHour)" />
					<div class="datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top" (click)="addHour()">
					</div>
					<div class="datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom" (click)="addHour(true)">
					</div>
					<input type="text" [(ngModel)]="selectedMinute" (keydown)="hourMinuteKeydown(59, selectedMinute)" (blur)="formatMinute()" />
					<div class="datetime-picker-top-spinner datetime-picker-clickable glyphicon glyphicon-triangle-top" (click)="addMinute()">
					</div>
					<div class="datetime-picker-bottom-spinner datetime-picker-clickable glyphicon glyphicon-triangle-bottom" (click)="addMinute(true)">
					</div>
					<select [(ngModel)]="selectedAMPM">
						<option>AM</option>
						<option>PM</option>
					</select>
				</div>
			</div>
			<div class="datetime-picker-controls-panel row">
				<div class="col-md-12 datetime-picker-buttons-panel id_{{uniqueId}}">
					<button class="btn btn-default" (click)="selectNow()">
						Now
					</button>
					&nbsp;
					<button class="btn btn-primary" (click)="persistDate()">
						Select
					</button>
				</div>
			</div>
		</div>
	</div>
</div>`,
	providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR],
	styleUrls: ['datetime-picker.css']
})
export class DateTimePickerComponent implements ControlValueAccessor, OnInit {

	@Input() name: string;
	@Input() placeholder: string;
	@Input() hideDate: boolean;
	@Input() hideTime: boolean;
	@Input() selectOnCalendarClick: boolean;
	@Input() minDate: Date;
	@Input() maxDate: Date;
	@Input() minuteStep: number;
	@Input() required: boolean;

	private _dateFormat: string;
	@Input()
	get dateFormat(): string {
		return this._dateFormat;
	}
	set dateFormat(f: string) {
		this._dateFormat = f;
		this.formatDate();
	}


	private innerValue: Date;
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;
	private selectedDate: Date;
	private currentonclick: any;

	protected formattedDate: string;
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
	protected dropdownVisible: boolean;
	protected inputChanged: boolean;
	protected uniqueId = Math.floor((1 + Math.random()) * 0x10000).toString();

	constructor(private zone: NgZone) { }

	ngOnInit() {
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

	private formatDate() {
		if (!this.innerValue)
			this.formattedDate = "";
		else
			this.formattedDate = moment(new Date(this.innerValue)).format(this.dateFormat);
	}

	private getMinuteInt() {
		let currMinute = parseInt(this.selectedMinute);
		if (isNaN(currMinute))
			currMinute = 0;
		return currMinute;
	}

	protected refreshCalendarDates() {
		if (!this.selectedDate)
			this.selectedDate = new Date();

		if (!this.selectedMonth)
			this.selectedMonth = this.selectedDate.getMonth() + 1;

		if (!this.selectedYear)
			this.selectedYear = this.selectedDate.getFullYear();

		if (!this.selectedHour) {
			this.selectedHour = this.selectedDate.getHours();
			if (this.selectedHour > 12) {
				this.selectedHour -= 12;
				this.selectedAMPM = 'PM';
			}
			else {
				this.selectedAMPM = 'AM';
			}
		}

		if (!this.selectedMinute) {
			this.selectedMinute = this.selectedDate.getMinutes().toString();
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
		this.selectedDate = new Date();
		this.selectedMonth = null;
		this.selectedYear = null;
		this.selectedHour = null;
		this.selectedMinute = null;
		this.selectedAMPM = null;
		this.refreshCalendarDates();
	}

	protected blurEditor() {
		if (!this.inputChanged) return;
		this.inputChanged = false;
		let date = new Date(this.formattedDate);
		if (isNaN(date.getTime())) {
			this.innerValue = null;
			this.formatDate();
			return;
		}

		this.selectDate(date, true);
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

	protected persistDate(alreadySelected = false, fromInput = false) {
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

	}

	protected addMinute(backwards) {
		let currMinute = this.getMinuteInt();
		currMinute += (backwards ? -1 : 1) * (this.minuteStep || 1);
		if (currMinute < 0) {
			currMinute = 59;
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
		if (value === undefined || value == null) {
			this.innerValue = null;
		}
		else if (value !== this.innerValue) {
			this.innerValue = value;
		}

		this.refreshCalendarDates();
		this.formatDate();
	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}
}