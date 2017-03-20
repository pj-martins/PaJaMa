import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule, RouterOutletMap } from '@angular/router';
import { DemoGridComponent } from './demo/demo-grid.component';
import { DemoEditorsComponent } from './demo/demo-editors.component';

const appRoutes: Routes = [
	{
		path: 'grid',
		component: DemoGridComponent
	},
	{
		path: '',
		component: DemoEditorsComponent
	}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes, { useHash: true });