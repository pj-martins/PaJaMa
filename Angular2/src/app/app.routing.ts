import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule, RouterOutletMap } from '@angular/router';
import { GridViewBasicDemoComponent } from './gridview/gridviewbasicdemo.component';

const appRoutes: Routes = [
	{
		path: '',
		component: GridViewBasicDemoComponent
	}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes, { useHash: true });