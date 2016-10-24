(function () {
	angular.module('pjm')
		.filter('page', function () {
			return function (arr, pageSize, currentPage) {
				if (!pageSize) return arr;
				if (!currentPage) currentPage = 1;
				return (arr || []).slice((currentPage - 1) * pageSize, currentPage * pageSize);
			};
		});
	angular.module('pjm')
	.filter('unique', function ($filter, $parse) {
		return function (collection, property) {

			collection = (angular.isObject(collection)) ? toArray(collection) : collection;

			if (!angular.isArray(collection)) {
				return collection;
			}

			//store all unique identifiers
			var uniqueItems = [],
				get = $parse(property);

			return (angular.isUndefined(property))
			  //if it's kind of primitive array
			  ? collection.filter(function (elm, pos, self) {
			  	return self.indexOf(elm) === pos;
			  })
			  //else compare with equals
			  : collection.filter(function (elm) {
			  	var prop = get(elm);
			  	if (some(uniqueItems, prop)) {
			  		return false;
			  	}
			  	uniqueItems.push(prop);
			  	return true;
			  });

			//checked if the unique identifier is already exist
			function some(array, member) {
				if (angular.isUndefined(member)) {
					return false;
				}
				return array.some(function (el) {
					return angular.equals(el, member);
				});
			}

			function toArray(object) {
				return angular.isArray(object)
				  ? object
				  : Object.keys(object).map(function (key) {
				  	return object[key];
				  });
			}
		}
	});


}());