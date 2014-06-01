var itemAnalyzer = angular.module("itemAnalyzer", ["ngRoute", "ui.bootstrap"])
	.config(function($routeProvider) {
		$routeProvider
			.when("/items/list",
			{
				controller: "itemsController",
				templateUrl: "Spa/partials/itemsList.html"
			})
			.when("/stash/list",
			{
				controller: "stashController",
				templateUrl: "Spa/partials/stashList.html"
			})
			.when("/categories",
			{
				controller: "editCategoriesController",
				templateUrl: "Spa/partials/editCategories.html"
			})
			.otherwise({
				redirectTo: "/items/list"
			});
	})
	.controller("itemsController", function($scope, $http) {
		$scope.specification = {
			numberOfItems: 100
		};

		$scope.GetItems = function() {
			$http.get("api/items/GetItems")
				.success(function(data, status, headers, config) {
					$scope.items = data;
				});
		};

		$scope.GetItems = function(specification) {
			console.log(specification);
			$http.post("api/items/GetItems", specification)
				.success(function(data, status, headers, config) {
					$scope.items = data;
				});
		};
	})
	.controller("stashController", function($scope, $http, $timeout) {
		$scope.GetTab = function(index, callback) {
			$http.get("api/stash/GetTab/?stashTab=" + index)
				.success(function(data, status, headers, config) {
					$scope.tabs = data.Tabs;
					if (callback != undefined)
						callback();
				})
				.error(function(data, status, headers, config) {
					console.log("failed \r\n");
				});
		};

		$scope.UpdateButtonClick = function(tab) {
			tab.updating = true;
			$scope.GetTab(tab.Id, function() { tab.updating = false; });
		};

		$scope.GetTab(0);
	});