import { Pipe, PipeTransform } from '@angular/core';
import { DataColumn } from './gridview';

@Pipe({ name: 'columnCaption' })
export class ColumnCaptionPipe implements PipeTransform {

  transform(column: DataColumn) {
		if (column.caption) return column.caption;
		var fieldName = column.fieldName;
		if (!fieldName || fieldName == '') return '';
					if (fieldName.lastIndexOf('.') > 0) {
							fieldName = fieldName.substring(fieldName.lastIndexOf('.') + 1, fieldName.length);
					}
					return fieldName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) {
							return str.toUpperCase();
					});
		}
}