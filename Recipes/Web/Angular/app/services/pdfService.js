(function () {
	var pdfService = function (quantityDisplayService) {
		this.downloadPDF = function (recipe, imgBase64, imgContentType, includeNutrition, includeRating) {
			var doc = new jsPDF();

			var x = 15;
			var y = 25;

			doc.setFontSize(20);
			doc.text(x, y, recipe.recipeName);

			y += 5;
			doc.setFontSize(10);

			var hasImage = false;
			if (imgBase64 != null) {
				doc.addImage('data:' + imgContentType + ';base64,' + imgBase64, imgContentType.replace('image/', '').toUpperCase(),
					x, y, 65, 65);
				x += 70;
				hasImage = true;
			}


			var hasNutrition = false;
			if (includeNutrition) {
				doc.text(x, y + 5, 'Servings: ' + recipe.numberOfServings);
				y += 2;
				doc.setFontStyle('italic');
				doc.text(x, y + 10, 'Calories: ' + recipe.totalCalories);
				doc.text(x, y + 15, 'Carbohydrates: ' + recipe.totalCarbohydrates);
				doc.text(x, y + 20, 'Fat: ' + recipe.totalFat);
				doc.text(x, y + 25, 'Saturated Fat: ' + recipe.totalSaturatedFat);
				doc.text(x, y + 30, 'Protein: ' + recipe.totalProtein);
				doc.text(x, y + 35, 'Sugars: ' + recipe.totalSugars);
				doc.setFontStyle('normal');
				y += 40;
				hasNutrition = true;
			}

			if (includeRating) {
				doc.text(x, y + 2, 'Rating: ' + (Math.round(10 * recipe.rating) / 10).toString());
				y += 5;
			}

			y += hasImage && hasNutrition ? 25 : 5;
			if (y > 100)
				x = 15;
			for (var i = 0; i < recipe.recipeIngredientMeasurements.length; i++) {
				var ri = recipe.recipeIngredientMeasurements[i];
				if (!ri.include) continue;
				var activeIngredient = ri.activeAlternate == null ? ri.ingredientMeasurement : ri.activeAlternate.toIngredientMeasurement;
				doc.text(x, y, quantityDisplayService.getFriendlyQuantity(ri.quantity) + ' ' + (activeIngredient.measurement == null || activeIngredient.measurement.measurementName == '' ? '' :
					activeIngredient.measurement.measurementName + ' ') + activeIngredient.ingredient.ingredientName);
				y += 5;
			}


			y += 10;
			var directionsWidth = 110;
			if (y > 105) {
				x = 15;
				directionsWidth = 180;
			}

			var directions = recipe.directions;
			if (recipe.notes != null && recipe.notes != "") {
				directions += "\r\n\r\nNotes: " + recipe.notes;
			}

			directions += "\r\n\r\n" + recipe.recipeSource.recipeSourceName;

			doc.text(x, y, doc.splitTextToSize(directions, directionsWidth));

			doc.save(recipe.recipeName + '.pdf');
		};
	};

	angular.module('recipeSearch').service('pdfService', pdfService);
}());