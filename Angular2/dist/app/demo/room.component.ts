import { Component } from '@angular/core';
import { Room } from '../classes/classes';
import { GridViewRowTemplateComponent } from 'pajama/gridview/gridview-templates.component'

@Component({
	moduleId: module.id,
	selector: 'room',
	templateUrl: './room.component.html'
})
export class RoomComponent extends GridViewRowTemplateComponent {
	
}