import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'toCamelCase' })
export class ToCamelCasePipe implements PipeTransform {
	transform(value: string) {
		let regex = /([A-Z])/g;
		return value.replace(regex, " $1");
	}
}