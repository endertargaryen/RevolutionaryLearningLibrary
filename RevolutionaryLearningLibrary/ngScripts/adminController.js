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

	$scope.item = getBlankItem();
	$scope.selectedAgeGroups = [0, 0, 0];

	$scope.curTab = $scope.adminTab.requests;
	$scope.hasError = false;
	$scope.errorMessage = "";

	$scope.init = function (subjects, ageGroups, locations, categories)
	{
		$scope.subjects = subjects;
		$scope.ageGroups = ageGroups;
		$scope.locations = locations;
		$scope.categories = categories;
	};

	/*
	 * Functions
	 */

	function getBlankItem()
	{
		return {
			"Name": "",
			"Description": "",
			"ImageName": "",
			"Item2AgeGroup": [],
			"Item2Subject": [],
		};
	}

	$scope.changeTab = function (newTab)
	{
		$scope.curTab = newTab;
	};

	/* 
	 * Server Functions
	 */

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

	$scope.saveItem = function ()
	{
		$http.post("/Admin/SaveItem", $scope.item).then(
		function success(data)
		{
			// save the file
			document.forms["fileSubmitForm"].submit();
		},
		function error(data)
		{
			$scope.hasError = true;
			$scope.errorMessage = data.data.StatusMessage;
		});
	};

	// initial data load
	$scope.loadData();

	var tab = getParameter("tab");

	if(tab !== null)
	{
		$scope.curTab = parseInt(tab, 10);
	}
});