(function () {
	var pdfService = function (quantityDisplayService) {
		this.downloadPDF = function (recipe, imgBase64, includeNutrition, includeRating) {
			var doc = new jsPDF();

			var x = 15;
			var y = 25;

			doc.setFontSize(20);
			doc.text(x, y, recipe.RecipeName);

			y += 5;
			doc.setFontSize(10);

			var hasImage = false;
			if (imgBase64 != null) {
				doc.addImage('data:' + imgBase64.ContentType + ';base64,' + imgBase64.Base64String, imgBase64.ContentType.replace('image/', '').toUpperCase(),
					x, y, 65, 65);
				x += 70;
				hasImage = true;
			}


			var hasNutrition = false;
			if (includeNutrition) {
				doc.text(x, y + 5, 'Servings: ' + recipe.NumberOfServings);
				y += 2;
				doc.setFontStyle('italic');
				doc.text(x, y + 10, 'Calories: ' + recipe.TotalCalories);
				doc.text(x, y + 15, 'Carbohydrates: ' + recipe.TotalCarbohydrates);
				doc.text(x, y + 20, 'Fat: ' + recipe.TotalFat);
				doc.text(x, y + 25, 'Saturated Fat: ' + recipe.TotalSaturatedFat);
				doc.text(x, y + 30, 'Protein: ' + recipe.TotalProtein);
				doc.text(x, y + 35, 'Sugars: ' + recipe.TotalSugars);
				doc.setFontStyle('normal');
				y += 40;
				hasNutrition = true;
			}

			if (includeRating) {
				doc.text(x, y + 2, 'Rating: ' + (Math.round(10 * recipe.Rating) / 10).toString());
				y += 5;
			}

			y += hasImage && hasNutrition ? 25 : 5;
			if (y > 100)
				x = 15;
			for (i = 0; i < recipe.RecipeIngredients.length; i++) {
				var ri = recipe.RecipeIngredients[i];
				if (ri.Exclude) continue;
				var activeIngredient = ri.ActiveAlternate == null ? ri.Ingredient : ri.ActiveAlternate.Ingredient;
				doc.text(x, y, quantityDisplayService.getFriendlyQuantity(ri.Quantity) + ' ' + (activeIngredient.MeasurementName == null || activeIngredient.MeasurementName == '' ? '' :
					activeIngredient.MeasurementName + ' ') + activeIngredient.IngredientName);
				y += 5;
			}


			y += 10;
			var directionsWidth = 110;
			if (y > 105) {
				x = 15;
				directionsWidth = 180;
			}

			var directions = recipe.Directions;
			if (recipe.Notes != null && recipe.Notes != "") {
				directions += "\r\n\r\nNotes: " + recipe.Notes;
			}

			directions += "\r\n\r\n" + recipe.Source;

			doc.text(x, y, doc.splitTextToSize(directions, directionsWidth));

			doc.save(recipe.RecipeName + '.pdf');
		};
	};

	angular.module('recipeSearch').service('pdfService', pdfService);
}());