import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: 'moment' })
export class MomentPipe implements PipeTransform {
	transform(value: any, format: string) {
		if (!value)
			return '';
		let dt = moment(value);
		if (dt.get("year") == 1) return '';
		return dt.format(format);
	}
}