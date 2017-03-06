import { Pipe, PipeTransform } from '@angular/core';
import { ToCamelCasePipe } from './to-camel-case.pipe';

@Pipe({ name: 'enumToList' })
export class EnumToListPipe implements PipeTransform {
	transform(value: any) {
		let vals: string[] = [];
		let toCamelCasePipe = new ToCamelCasePipe();
		for (let e in value) {
			vals.push(toCamelCasePipe.transform(e));
		}
		return vals;
	}
}