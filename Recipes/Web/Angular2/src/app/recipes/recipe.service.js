"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var data_service_1 = require('../shared/services/data.service');
var RecipeService = (function () {
    function RecipeService(dataService) {
        this.dataService = dataService;
    }
    RecipeService.prototype.getRandomRecipes = function () {
        var args = new data_service_1.Arguments();
        args.parameters = new Array();
        args.parameters.push(new data_service_1.Parameter("random", "20"));
        return this.dataService.getEntities('RecipeSearch', args);
    };
    RecipeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [data_service_1.DataService])
    ], RecipeService);
    return RecipeService;
}());
exports.RecipeService = RecipeService;
//# sourceMappingURL=recipe.service.js.map