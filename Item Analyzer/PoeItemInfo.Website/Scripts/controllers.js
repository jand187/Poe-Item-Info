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
			.when("/basetypes/edit",
			{
				controller: "baseTypesController",
				templateUrl: "Spa/partials/basetypesEdit.html"
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
	.controller("stashController", function($scope, $http) {
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
	})
	.controller("baseTypesController", function($scope, $http) {
		$scope.keyword = "";
		$scope.typeMap = [];
		$scope.GetTypeLines = function() {
			$http.get("api/baseTypes/GetTypeLines/?keyword=" + $scope.keyword)
				.success(function(data, status, headers, config) {
					$scope.typeLines = data;
				});
		};

		$scope.GetTypes = function() {
			$http.get("api/baseTypes/GetTypes")
				.success(function(data, status, headers, config) {
					$scope.types = data;
				});
		};

		$scope.AssignType = function(typeLine, type) {
			var mapItem = {
				typeLine: typeLine,
				type: type
			};

			$scope.typeMap.push(mapItem);
		};

		$scope.SaveTypeMap = function() {
			$http.post("api/baseTypes/SaveTypeMap", $scope.typeMap)
				.success(function(data, status, headers, config) {
					$scope.typeMap = [];
					$scope.GetTypeLines();
				});
		};
		$scope.GetTypeLines();
		$scope.GetTypes();
	});
