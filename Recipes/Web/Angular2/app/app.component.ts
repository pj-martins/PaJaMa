import { Component } from '@angular/core';
import { Routes, ROUTER_DIRECTIVES } from '@angular/router';
import { RecipesComponent } from './recipes/recipes.component';

@Component({
    selector: 'recipes-app',
    templateUrl: './app/app.component.html',
    directives: [ROUTER_DIRECTIVES]
})
@Routes([
    { path: '/', component: RecipesComponent }
])
export class AppComponent {
}