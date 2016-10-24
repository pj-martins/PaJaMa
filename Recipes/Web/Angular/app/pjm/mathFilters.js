(function () {
	angular.module('pjm')
	.filter('sum', function ($filter, $parse) {
		return function (collection, property) {

			if (!angular.isArray(collection)) {
				return 0;
			}

			var sum = 0;
			for (var i = 0; i < collection.length; i++) {
				sum += $parse(property)(collection[i]);
			}
			return sum;
		}
	});

	angular.module('pjm')
	.filter('avg', function ($filter, $parse) {
		return function (collection, property) {

			if (!angular.isArray(collection) || collection.length < 1) {
				return 0;
			}

			var sum = 0;
			for (var i = 0; i < collection.length; i++) {
				sum += $parse(property)(collection[i]);
			}
			return sum / collection.length;
		}
	});

	angular.module('pjm')
	.filter('min', function ($filter, $parse) {
		return function (collection, property) {

			if (!angular.isArray(collection) || collection.length < 1) {
				return 0;
			}

			var get = $parse(property);
			var min = get(collection[0]);
			for (var i = 1; i < collection.length; i++) {
				if (get(collection[i]) < min)
					min = get(collection[i]);
			}
			return min;
		}
	});

	angular.module('pjm')
	.filter('max', function ($filter, $parse) {
		return function (collection, property) {

			if (!angular.isArray(collection) || collection.length < 1) {
				return 0;
			}

			var get = $parse(property);
			var max = get(collection[0]);
			for (var i = 1; i < collection.length; i++) {
				if (get(collection[i]) > max)
					max = get(collection[i]);
			}
			return max;
		}
	});

}());