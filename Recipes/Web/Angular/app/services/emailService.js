(function() {
	var emailService = function ($http, appSettings) {
	    this.sendEmail = function (recipe, email) {
			$http.post(appSettings.apiUrl + '/api/email',
				{
					Recipe: recipe,
					To: email.emailTo,
					Subject: email.emailSubject,
					Message: email.emailMessage,
					ActiveImageURL: recipe.getActiveImageURL()
				})
				.success(function (data, status, headers, config) {
					alert('Emailed');
				})
				.error(function (data, status, headers, config) {
					alert('error: \n' + JSON.stringify(data) + '\n' + JSON.stringify(status) + '\n' + JSON.stringify(headers) + '\n' + JSON.stringify(config));
				});
		};


	}

	angular.module('recipeSearch').service('emailService', emailService);
}());