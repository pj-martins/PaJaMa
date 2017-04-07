import { Component } from '@angular/core';
import { Room } from '../classes/classes';
import { Observable } from 'rxjs/Rx';

declare var ROOMS: Array<Room>;

@Component({
	moduleId: module.id,
	selector: 'demo-editors',
	templateUrl: './demo-editors.component.html'
})
export class DemoEditorsComponent {
	protected selectedDateTime: Date;
	protected selectedText: string;
	protected multiTextboxItems: Array<string> = ['Item 1', 'Item 2'];
	protected multiTypeaheadItems: Array<string> = [];
	protected dataSource: Array<string> = ['Alpha', 'Bravo', 'Charlie', 'Delta', 'Echo', 'Foxtrot', 'Tango', 'Zulu'];

	protected selectedRoomIDs: Array<number> = [];
	protected selectedRooms: Array<Room> = [];

	protected rooms = ROOMS;
	protected getRooms = (partial: string): Array<any> => {
		let rooms = [];
		for (let r of this.rooms) {
			if (r.roomName.toLowerCase().indexOf(partial.toLowerCase()) >= 0) {
				rooms.push(r);
			}
		}
		return rooms;
	}

	protected getRoomsObservable = (partial: string): Observable<Array<any>> => {
		let rooms = this.getRooms(partial);
		return Observable.create(o => o.next(rooms));
	}
}