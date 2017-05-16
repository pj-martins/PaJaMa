import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule, RouterOutletMap } from '@angular/router';
import { DemoGridComponent } from './demo/demo-grid.component';
import { DemoTreeComponent } from './demo/demo-tree.component';
import { DemoEditorsComponent } from './demo/demo-editors.component';
import { DemoModalComponent } from './demo/demo-modal.component';
import { SandboxComponent } from './sandbox/sandbox.component';

const appRoutes: Routes = [
	{
		path: 'grid',
		component: DemoGridComponent
	},
	{
		path: 'tree',
		component: DemoTreeComponent
	},
	{
		path: 'editors',
		component: DemoEditorsComponent
	},
	{
		path: 'modal',
		component: DemoModalComponent
	},
	{
		path: 'sandbox',
		component: SandboxComponent
	},
	{
		path: '',
		redirectTo: 'grid',
		pathMatch: 'full'
	}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes, { useHash: true });