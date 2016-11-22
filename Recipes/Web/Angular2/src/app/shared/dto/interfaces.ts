// !!!! DO NOT MAKE CHANGES IN HERE HERE THEY WILL GET OVERWRITTEN WHEN TEMPLATE GETS GENERATED !!!!
import { IEntity } from './entity-base';

export interface IIngredient extends IEntity {
	ingredientName: string;
}

export interface IIngredientMeasurementAlternate extends IEntity {
	toIngredientMeasurement: IIngredientMeasurement;
	multiplier: number;
}

export interface IIngredientMeasurement extends IEntity {
	ingredient: IIngredient;
	measurement: IMeasurement;
	caloriesPer: number;
	carbohydratesPer: number;
	fatPer: number;
	saturatedFatPer: number;
	sugarsPer: number;
	proteinPer: number;
}

export interface IIngredient extends IEntity {
	ingredientName: string;
}

export interface IMeasurement extends IEntity {
	measurementName: string;
}

export interface IIngredientMeasurement extends IEntity {
	ingredient: IIngredient;
	measurement: IMeasurement;
	caloriesPer: number;
	carbohydratesPer: number;
	fatPer: number;
	saturatedFatPer: number;
	sugarsPer: number;
	proteinPer: number;
}

export interface IIngredient extends IEntity {
	ingredientName: string;
}

export interface IMeasurement extends IEntity {
	measurementName: string;
}

export interface IMeasurement extends IEntity {
	measurementName: string;
}

export interface IRecipeCover extends IEntity {
	recipeName: string;
	rating: number;
	ingredients: Array<string>;
	imageURLs: Array<string>;
	recipeURL: string;
	recipeSourceID: number;
}

export interface IRecipeSource extends IEntity {
	recipeSourceName: string;
}

export interface IRecipeImage extends IEntity {
	imageURL: string;
	localImagePath: string;
	sequence: number;
}

export interface IRecipeIngredientMeasurement extends IEntity {
	ingredientMeasurement: IIngredientMeasurement;
	alternates: Array<IIngredientMeasurementAlternate>;
	quantity: number;
	isOptional: boolean;
}

export interface IIngredientMeasurement extends IEntity {
	ingredient: IIngredient;
	measurement: IMeasurement;
	caloriesPer: number;
	carbohydratesPer: number;
	fatPer: number;
	saturatedFatPer: number;
	sugarsPer: number;
	proteinPer: number;
}

export interface IIngredient extends IEntity {
	ingredientName: string;
}

export interface IMeasurement extends IEntity {
	measurementName: string;
}

export interface IIngredientMeasurementAlternate extends IEntity {
	toIngredientMeasurement: IIngredientMeasurement;
	multiplier: number;
}

export interface IRecipeImage extends IEntity {
	imageURL: string;
	localImagePath: string;
	sequence: number;
}

export interface IRecipeIngredientMeasurement extends IEntity {
	ingredientMeasurement: IIngredientMeasurement;
	alternates: Array<IIngredientMeasurementAlternate>;
	quantity: number;
	isOptional: boolean;
}

export interface IIngredientMeasurement extends IEntity {
	ingredient: IIngredient;
	measurement: IMeasurement;
	caloriesPer: number;
	carbohydratesPer: number;
	fatPer: number;
	saturatedFatPer: number;
	sugarsPer: number;
	proteinPer: number;
}

export interface IIngredient extends IEntity {
	ingredientName: string;
}

export interface IMeasurement extends IEntity {
	measurementName: string;
}

export interface IIngredientMeasurementAlternate extends IEntity {
	toIngredientMeasurement: IIngredientMeasurement;
	multiplier: number;
}

export interface IRecipeSearch extends IEntity {
	recipeName: string;
	ingredients: string;
}

export interface IRecipeSource extends IEntity {
	recipeSourceName: string;
}

export interface IUser extends IEntity {
	userName: string;
	password: string;
}

export interface IUserRecipe extends IEntity {
	isBookmarked: boolean;
	rating: number;
	notes: string;
	allowEdit: boolean;
}

