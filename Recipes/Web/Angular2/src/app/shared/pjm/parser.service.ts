import { Injectable } from '@angular/core';

// Waiting for replacement from $parse
@Injectable()
export class ParserService {
	getObjectValue(prop: string, obj: any) {
		let parts = prop.split('.');
		let tempObj = obj;
		for (let part of parts) {
			let match = part.match(/^(.*)\[(\d*)\]$/);
			if (match && match.length == 3) {
				tempObj = tempObj[match[1]][parseInt(match[2])];
			}
			else {
				tempObj = tempObj[part];
			}
			// if there are no periods then simply return the raw value
			if (!tempObj) return parts.length == 1 ? tempObj : undefined;
		}
		return tempObj;
	}
}