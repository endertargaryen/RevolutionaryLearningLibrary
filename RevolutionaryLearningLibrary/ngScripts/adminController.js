angular.module("adminApp").controller("adminController", function ($scope, $http)
{
	/*
	 * Constants
	 */

	$scope.adminTab =
	{
		requests: 1,
		checkouts: 2,
		addInventory: 3
	};

	/*
	 * Variables
	 */

	$scope.navbar = 
	[
		{
			"name": "Requests",
			"id": $scope.adminTab.requests
		},
		{
			"name": "Checkouts",
			"id": $scope.adminTab.checkouts
		},
		{
			"name": "Add Inventory",
			"id": $scope.adminTab.addInventory
		}
	];

	$scope.curTab = $scope.adminTab.requests;
	$scope.hasError = false;
	$scope.errorMessage = "";

	/*
	 * Functions
	 */

	$scope.changeTab = function (newTab)
	{
		$scope.curTab = newTab;
	};

	$scope.loadData = function ()
	{
		$http.get("/Admin/GetInitialData").
		then(function success(data)
		{
			$scope.requests = data.data.Requests;
			$scope.checkouts = data.data.Checkouts;
		},
		function error(data)
		{
			$scope.hasError = true;
			$scope.errorMessage = data.data;
		})
	};

	$scope.checkItemIn = function (item)
	{
		$http.post("/Admin/CheckItemIn", item).then(
		function success(data)
		{
			$scope.loadData();
		},
		function error(data)
		{
			$scope.hasError = true;
			$scope.errorMessage = data.data;
		});
	};

	$scope.checkItemOut = function (item)
	{
		$http.post("/Admin/CheckItemOut", item).then(
		function success(data)
		{
			$scope.loadData();
		},
		function error(data)
		{
			$scope.hasError = true;
			$scope.errorMessage = data.data;
		});
	};

	// initial data load
	$scope.loadData();
});