(function () {

	var requiredAny = function () {
		var groups = {};

		function determineIfRequired(groupName) {
			var group = groups[groupName];
			if (!group) return false;

			var keys = Object.keys(group);
			for (key in keys) {
				if (key === 'isRequired')
					continue;

				if (group[keys[key]]) return false;
			}
			
			return true;
		}

		return {
			require: '?ngModel',
			scope: {},
			link: function (scope, element, attrs, ctrl) {
				var groupName = attrs.requiredAny;
				if (!groups[groupName]) {
					groups[groupName] = { isRequired: true };
				}
				var group = scope.group = groups[groupName];

				// Clean up when the element is removed
				scope.$on('$destroy', function () {
					delete (group[scope.$id]);
					if (Object.keys(group).length <= 1) {
						delete (groups[groupName]);
					}
				});

				// Updates the validity state for the 'required' error-key based on the group's status
				function updateValidity() {
					if (group.isRequired) {
						ctrl.$setValidity('required', false);

					} else {
						ctrl.$setValidity('required', true);
					}
				}

				// Updates the group's state and this control's validity
				function validate(value) {
					group[scope.$id] = value;
					group.isRequired = determineIfRequired(groupName);
					updateValidity();
					return value;
				};

				ctrl.$formatters.push(validate);
				ctrl.$parsers.unshift(validate);
				scope.$watch('group.isRequired', updateValidity);
			}
		};
	};

	angular.module('pjm').directive('requiredAny', requiredAny);

}());