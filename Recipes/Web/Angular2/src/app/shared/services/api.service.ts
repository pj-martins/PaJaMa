// !!!! DO NOT MAKE CHANGES IN HERE HERE THEY WILL GET OVERWRITTEN WHEN TEMPLATE GETS GENERATED !!!!

import { IRecipeSource, IIngredient, IRecipeCover } from '../dto/interfaces';
import { Recipe } from '../dto/entities';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { DataService, Items, GetArguments } from './data.service';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class ApiService extends DataService {
	constructor(protected http: Http) {
		super(http);
	}

	getRecipeSources(args?: GetArguments): Observable<Items<IRecipeSource>> {
		return super.getEntities<IRecipeSource>(`recipeSource`, args);
	}
	getRecipeSourcesOData(args?: GetArguments): Observable<Items<IRecipeSource>> {
		return super.getEntitiesOData<IRecipeSource>(`recipeSource`, args);
	}
	getRecipeSource(id: number): Observable<IRecipeSource> {
		return super.getEntity<IRecipeSource>(`recipeSource`, id);
	}
	getIngredients(args?: GetArguments): Observable<Items<IIngredient>> {
		return super.getEntities<IIngredient>(`ingredient`, args);
	}
	getIngredientsOData(args?: GetArguments): Observable<Items<IIngredient>> {
		return super.getEntitiesOData<IIngredient>(`ingredient`, args);
	}
	getIngredient(id: number): Observable<IIngredient> {
		return super.getEntity<IIngredient>(`ingredient`, id);
	}
	insertRecipe(dto: Recipe): Observable<Recipe> {
		return super.insertEntity<Recipe>(`recipe`, dto);
	}
	updateRecipe(id: number, dto: Recipe): Observable<Recipe> {
		return super.updateEntity<Recipe>(`recipe`, id, dto);
	}
	deleteRecipe(id: number): Observable<boolean> {
		return super.deleteEntity(`recipe`, id);
	}
	getRecipes(args?: GetArguments): Observable<Items<Recipe>> {
		return super.getEntities<Recipe>(`recipe`, args);
	}
	getRecipesOData(args?: GetArguments): Observable<Items<Recipe>> {
		return super.getEntitiesOData<Recipe>(`recipe`, args);
	}
	getRecipe(id: number): Observable<Recipe> {
		return super.getEntity<Recipe>(`recipe`, id);
	}
	searchRecipes(includes: string, excludes: string, rating: number, bookmarked: boolean, recipeSourceID: number, picturesOnly: boolean, page: number, pageSize: number): Observable<Items<IRecipeCover>> {
		return super.getItems<IRecipeCover>(`recipeSearch/searchRecipes?includes=${includes}&excludes=${excludes}&rating=${rating}&bookmarked=${bookmarked}&recipeSourceID=${recipeSourceID}&picturesOnly=${picturesOnly}&page=${page}&pageSize=${pageSize}`);
	}
	getRandomRecipes(random: number): Observable<Items<IRecipeCover>> {
		return super.getItems<IRecipeCover>(`recipeSearch/getRandomRecipes?random=${random}`);
	}
	getRecipeSearchs(args?: GetArguments): Observable<Items<IRecipeCover>> {
		return super.getEntities<IRecipeCover>(`recipeSearch`, args);
	}
	getRecipeSearchsOData(args?: GetArguments): Observable<Items<IRecipeCover>> {
		return super.getEntitiesOData<IRecipeCover>(`recipeSearch`, args);
	}
	getRecipeSearch(id: number): Observable<IRecipeCover> {
		return super.getEntity<IRecipeCover>(`recipeSearch`, id);
	}
}
