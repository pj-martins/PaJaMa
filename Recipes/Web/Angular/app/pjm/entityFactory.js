(function () {

	var entityFactory = function ($http, $q) {

		var serviceBase = '/api/',
        factory = {};

		factory.buildGetEntitiesUrl = function (entityType, args) {
			var url = '';
			if (args && args.baseUrl)
				url += args.baseUrl;
			url += serviceBase + entityType + (args && args.includeCount ? '/entitiesOData' : '/entities');
			var firstIn = true;
			
			if (args && args.params) {
				for (var p in args.params) {
					url += (firstIn ? '?' : '&') + p + '=' + args.params[p];
					firstIn = false;
				}
			}

			if (args && args.filter) {
				url += (firstIn ? '?' : '&') + '$filter=' + args.filter;
				firstIn = false;
			}

			if (args && args.pageSize && args.pageSize > 0) {
				if (!args.pageNumber)
					args.pageNumber = 1;
				url += (firstIn ? '?' : '&') + '$top=' + args.pageSize + '&$skip=' + ((args.pageNumber - 1) * args.pageSize);
				firstIn = false;
			}

			if (args && args.orderBy)
				url += (firstIn ? '?' : '&') + '$orderby=' + args.orderBy;

			if (args && args.includeCount)
				url += (firstIn ? '?' : '&') + '$inlinecount=allpages';

			return url;
		};

		factory.getEntities = function (entityType, args) {
			var url = factory.buildGetEntitiesUrl(entityType, args);

			return $http.get(url).then(function (response) {
				if (args && args.includeCount)
					return { results: response.data.items, totalRecords: response.data.count }
				return { results: response.data, totalRecords: response.headers('X-InlineCount') };
			});
		};

		factory.buildGetEntityUrl = function (entityType, id, args) {
			var url = '';
			if (args && args.baseUrl)
				url += args.baseUrl;
			url += serviceBase + entityType + '/entity/' + id;
			return url;
		};

		factory.getEntity = function (entityType, id, args) {
			var url = factory.buildGetEntityUrl(entityType, id, args);
			return $http.get(url).then(function (response) {
				return response.data;
			});
		};

		factory.newEntity = function () {
			return $q.when({ id: 0 });
		};

		factory.insertEntity = function (entityType, entity, args) {
			var url = '';
			if (args && args.baseUrl)
				url += args.baseUrl;
			return $http.post(url + serviceBase + entityType + '/postEntity', entity).then(function (results) {
				entity.id = results.data.id;
				return results.data;
			});
		};

		factory.updateEntity = function (entityType, entity, args) {
			var url = '';
			if (args && args.baseUrl)
				url += args.baseUrl;
			return $http.put(url + serviceBase + entityType + '/putEntity/' + entity.id, entity).then(function (status) {
				return status.data;
			});
		};

		factory.deleteEntity = function (entityType, id, args) {
			return $http.delete(serviceBase + entityType + '/deleteEntity/' + id).then(function (status) {
				return status.data;
			});
		};

		return factory;
	};

	angular.module('pjm').factory('entityFactory', entityFactory);

}());