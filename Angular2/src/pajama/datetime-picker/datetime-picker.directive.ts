import { Directive, ViewContainerRef, OnInit, ComponentFactoryResolver, TemplateRef, Input, Output, ComponentRef, ElementRef, EventEmitter, OnChanges, forwardRef } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';
import { DateTimePickerComponent } from './datetime-picker.component';
import * as moment from 'moment'

export const MAX_MIN_VALIDATOR: any = {
	provide: NG_VALIDATORS,
	useExisting: forwardRef(() => DateTimePickerDirective),
	multi: true
};

@Directive({
	selector: '[dateTimePicker][ngModel]',
	host: { '(blur)': 'blurEditor()', '(keyup)': 'keyup($event)' },
	providers: [MAX_MIN_VALIDATOR]
})
export class DateTimePickerDirective implements OnInit, OnChanges, Validator {
	private _component: ComponentRef<DateTimePickerComponent>;
	private _initialColor: string;

	protected inputChanged: boolean;

	@Input("ngModel") ngModel: any;
	@Output("ngModelChange") ngModelChange = new EventEmitter<any>();

	@Input()
	get hideDate(): boolean {
		return this._component.instance.hideDate;
	}
	set hideDate(v: boolean) {
		this._component.instance.hideDate = v;
	}

	@Input()
	get hideTime(): boolean {
		return this._component.instance.hideTime;
	}
	set hideTime(v: boolean) {
		this._component.instance.hideTime = v;
	}

	@Input()
	get selectOnCalendarClick(): boolean {
		return this._component.instance.selectOnCalendarClick;
	}
	set selectOnCalendarClick(v: boolean) {
		this._component.instance.selectOnCalendarClick = v;
	}

	@Input()
	get minDate(): Date {
		return this._component.instance.minDate;
	}
	set minDate(v: Date) {
		this._component.instance.minDate = v;
	}

	@Input()
	get maxDate(): Date {
		return this._component.instance.maxDate;
	}
	set maxDate(v: Date) {
		this._component.instance.maxDate = v;
	}

	@Input()
	get minuteStep(): number {
		return this._component.instance.minuteStep;
	}
	set minuteStep(v: number) {
		this._component.instance.minuteStep = v;
	}

	private _dateFormat: string;
	@Input()
	get dateFormat(): string {
		return this._dateFormat;
	}
	set dateFormat(f: string) {
		this._dateFormat = f;
		this.formatDate(this.ngModel);
	}

	constructor(private componentFactoryResolver: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef, private elementRef: ElementRef) { //private viewContainerRef: ViewContainerRef, private componentFactoryResolver: ComponentFactoryResolver, private templateref: TemplateRef<any>) {
		let factory = this.componentFactoryResolver.resolveComponentFactory(DateTimePickerComponent);
		this._component = this.viewContainerRef.createComponent(factory);
		this._component.instance.dateChanged.subscribe(d => {
			this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
			this.ngModelChange.emit(d);
			//this.formatDate(d);
		});
		this._initialColor = this.elementRef.nativeElement.style.color;
		// UGLY, this fires before the initial value
		// blank it first so we can format the text without flickering unformatted
		this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
		this.elementRef.nativeElement.style.width = "100%";
	}

	ngOnInit() {
		//if (this.elementRef.nativeElement.offsetWidth) {
		//	console.log(this.elementRef);
		//	this._component.instance.elementRef.nativeElement.childNodes[0].style.width = this.elementRef.nativeElement.offsetWidth + 'px';
		//	this._component.instance.elementRef.nativeElement.childNodes[0].style.marginLeft = this.elementRef.nativeElement.offsetLeft;
		//}
		if (!this.dateFormat) {
			this.dateFormat = "";
			if (!this.hideDate)
				this.dateFormat = "MM/DD/YYYY";
			if (!this.hideTime)
				this.dateFormat += " h:mm A";
			this.dateFormat = this.dateFormat.trim();
		}

		if (this.ngModel && !this.ngModel.getTime) {
			this.ngModel = new Date(this.ngModel);
			if (isNaN(this.ngModel.getTime()))
				this.ngModel = null;
		}
		this._component.instance.writeValue(this.ngModel);
		//this.formatDate(this.ngModel);
	}

	protected blurEditor() {
		if (!this.inputChanged) return;
		this.inputChanged = false;
		let formattedDate = this.elementRef.nativeElement.value;
		if (formattedDate) {
			let date = new Date(formattedDate);
			if (isNaN(date.getTime())) {
				// might be just a time string
				let valid = false;
				let curr = new Date(this.ngModel);
				if (isNaN(curr.getTime()))
					curr = new Date(1900, 1, 1);
				let datePart = this.ngModel ? moment(curr).format('YYYY/MM/DD') : '1900/01/01';
				date = new Date(datePart + ' ' + formattedDate);
				valid = !isNaN(date.getTime());
				if (!valid) {
					//this.ngModel = null;
					//this.formatDate(this.ngModel);
					this.ngModelChange.emit(null);
					return;
				}
			}

			this._component.instance.updateDateTimeControls(date);
			this._component.instance.persistDate(true);
		}
	}

	ngOnChanges(changes) {
		// user is typing
		if (this.elementRef.nativeElement == document.activeElement) {
			this.elementRef.nativeElement.style.color = this._initialColor;
			return;
		}
		this.elementRef.nativeElement.style.color = this.elementRef.nativeElement.style.backgroundColor || "white";
		this.formatDate(this.ngModel);
	}

	protected keyup(event: any) {
		let charCode = event.which || event.keyCode;
		// which other char codes?
		if ((charCode >= 48 && charCode <= 90) || (charCode >= 96 && charCode <= 111) || (charCode >= 186 && charCode <= 222) || charCode == 8) {
			this.inputChanged = true;
		}
	}

	private formatDate(date: any) {
		let formattedDate = "";
		if (!date)
			formattedDate = "";
		else
			formattedDate = moment(new Date(date)).format(this.dateFormat);
		window.setTimeout(() => {
			this.elementRef.nativeElement.value = formattedDate;
			this.elementRef.nativeElement.style.color = this._initialColor;
		}, 50);
	}

	validate(c: AbstractControl) {
		// user is typing
		if (this.elementRef.nativeElement == document.activeElement) return null;

		if (c.value && this.maxDate) {
			let dt = new Date(c.value);
			if (!isNaN(dt.getTime()) && dt > this.maxDate) {
				return {
					max: true
				};
			}
		}

		if (c.value && this.minDate) {
			let dt = new Date(c.value);
			if (!isNaN(dt.getTime()) && dt < this.minDate) {
				return {
					min: true
				};
			}
		}


		return null;
	}
}