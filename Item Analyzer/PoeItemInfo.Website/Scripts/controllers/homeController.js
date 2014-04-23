var itemAnalyzer = angular.module('itemAnalyzer', []);

itemAnalyzer.controller('ItemListCtrl', function ($scope, $http) {

	var theSpec = [
		{ property: "Category", value: "Unknown", operation: "eq" }
	];

	$http.post('api/items', theSpec).success(function (data) {
		$scope.items = data;
	});

});
