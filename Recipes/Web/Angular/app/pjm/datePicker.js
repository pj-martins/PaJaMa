(function () {

	var datePicker = function () {
		return {
			link: function ($scope, element, attrs) {
				parseDate = function (event) {
					var rawValue = event.target.value;
					var ngModel = angular.element(event.target).data('$ngModelController');
					ngModel.$setViewValue(new Date(rawValue));
					ngModel.$render();
				};

				element.bind("keydown keypress", function (event) {
					if (event.which === 13) {
						parseDate(event);
						event.preventDefault();
					}
				});

				element.bind("blur", function (event) {
					if (event.target.value != '')
						parseDate();
				});
			}
		};
	};

	angular.module('pjm').directive('datePicker', datePicker);

}());