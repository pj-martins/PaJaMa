import { NgModule } from '@angular/core';
import { EnumToListPipe } from './enum-to-list.pipe';
import { OrderByPipe } from './order-by.pipe';
import { ToCamelCasePipe } from './to-camel-case.pipe';
import { MomentPipe } from './moment.pipe';

@NgModule({
	declarations: [
		EnumToListPipe,
		OrderByPipe,
		ToCamelCasePipe,
		MomentPipe
	],
	exports: [
		EnumToListPipe,
		OrderByPipe,
		ToCamelCasePipe,
		MomentPipe
	]
})
export class PipesModule { }
