import { Component, Input, forwardRef, Directive } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, AbstractControl, Validator, NG_VALIDATORS, FormControl } from '@angular/forms';

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => EvenValidatorComponent),
	multi: true
};

export const CUSTOM_INPUT_CONTROL_VALIDATOR: any = {
	provide: NG_VALIDATORS,
	useExisting: forwardRef(() => EvenValidatorDirective),
	multi: true
};


@Component({
	moduleId: module.id,
	selector: 'even-validator',
	template: `<div>
<input type="number" [(ngModel)]="innerValue" even-validator-directive [formControl]="formControl" />
</div>`,
	providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class EvenValidatorComponent implements ControlValueAccessor {

	protected innerValue: number;

	@Input("formControl") formControl = new FormControl();

	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;

	writeValue(value: number) {
		if (value !== this.innerValue) {
			this.innerValue = value;
		}
	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}
}

@Directive({
	selector: '[even-validator-directive]',
	providers: [CUSTOM_INPUT_CONTROL_VALIDATOR]
})
export class EvenValidatorDirective implements Validator {
	validate(c: AbstractControl) {
		console.log(c);
		if (!c.value || c.value % 2 != 0)
			return {
				notEven: true
			};

		return null;
	}
}