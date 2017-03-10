import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule, RouterOutletMap } from '@angular/router';
import { DemoComponent } from './demo/demo.component';

const appRoutes: Routes = [
	{
		path: '',
		component: DemoComponent
	}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes, { useHash: true });