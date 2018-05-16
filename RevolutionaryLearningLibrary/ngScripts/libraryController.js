angular.module("libraryApp").controller("libraryController", function ($scope, $http)
{
	// variables
	$scope.error = false;
	$scope.errorMessage = "";

	// default filter settings
	$scope.filter =
		{
			"CategoryId": 1,
			"AgeGroupId": 0,
			"SubjectId": 0,
			"LocationId": 0
		};
	
	$scope.categoryText = "Books";

	// functions
	$scope.getItems = function()
	{
		$http.post("/Library/GetItems", $scope.filter).
		then(function success(data)
		{
			$scope.items = data.data;
		},
		function error(data)
		{
			alert(data.data);
		});
	};

	// load initial data
	$scope.getItems();
});