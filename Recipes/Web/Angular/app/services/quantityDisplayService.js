(function() {
	var quantityDisplayService = function () {
		this.getFriendlyQuantity = function (qty) {
			if (qty == null || qty == 0) return '';
			var friendlyQty = '';
			var tempQty = qty;
			var floorQty = Math.floor(qty);
			if (floorQty > 0) {
				friendlyQty += floorQty.toString() + " ";
				tempQty -= floorQty;
				if (tempQty == 0)
					return friendlyQty.trim();
			}
			// TODO: create elegant way to convert decimals to fractions
			if (tempQty == 0.5)
				friendlyQty += "1/2";
			else if (tempQty == 0.25)
				friendlyQty += "1/4";
			else if (tempQty == 0.75)
				friendlyQty += "3/4";
			else if (tempQty > 0.32 && tempQty < 0.34)
				friendlyQty += "1/3";
			else if (tempQty == 0.125)
				friendlyQty += "1/8";
			else if (tempQty >= 0.65 && tempQty <= 0.68)
				friendlyQty += "2/3";
			else if (tempQty == 0.2)
				friendlyQty += "1/5";
			else if (tempQty == 0.375)
				friendlyQty += "3/8";
			else if (tempQty == 0.625)
				friendlyQty += "5/8";
			else if (tempQty >= 0.165 && tempQty <= 0.168)
				friendlyQty += "1/6";
			else if (tempQty == 0.875)
				friendlyQty += "7/8";
			else
				friendlyQty = qty.toString();

			return friendlyQty.trim();
		};
	};

	angular.module('recipeSearch').service('quantityDisplayService', quantityDisplayService);
}());