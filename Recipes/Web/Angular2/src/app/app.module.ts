import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { RecipesComponent } from './recipes/recipes.component';
import { RecipeService } from './recipes/recipe.service';
import { DataService } from './shared/services/data.service';
import { RouterModule } from '@angular/router';
import { routing } from './app.routing';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing],
    providers: [RecipeService, DataService],
    declarations: [AppComponent, RecipesComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
