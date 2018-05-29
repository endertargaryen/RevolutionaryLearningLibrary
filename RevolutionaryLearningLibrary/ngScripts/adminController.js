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
	$scope.selectedAgeGroups = [null, null, null];
	$scope.selectedSubjects = [null, null, null];
	$scope.subLocationsToDisplay = [];

	$scope.curTab = $scope.adminTab.requests;
	$scope.hasError = false;
	$scope.errorMessage = "";

	$scope.init = function (subjects, ageGroups, locations, subLocations, categories, item)
	{
		$scope.subjects = JSON.parse(subjects);
		$scope.ageGroups = JSON.parse(ageGroups);
		$scope.locations = JSON.parse(locations);
		$scope.subLocations = JSON.parse(subLocations);
		$scope.categories = JSON.parse(categories);
		$scope.item = JSON.parse(item);
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
			"Category": null,
			"Location": null,
			"SubLocation": null
		};
	}
	
	function setEditValues(item)
	{
		// set many-to-many age groups
		for (var i = 0; i < item.Item2AgeGroup.length; i++)
		{
			if (item.Item2AgeGroup[i] !== null)
			{
				$scope.selectedAgeGroups[i] = item.Item2AgeGroup[i].AgeGroup;
			}
		}

		// set many-to-many subjects
		for (var i = 0; i < item.Item2Subject.length; i++)
		{
			if (item.Item2Subject[i] !== null)
			{
				$scope.selectedSubjects[i] = item.Item2Subject[i].Subject;
			}
		}
	};

	$scope.changeTab = function (newTab)
	{
		$scope.curTab = newTab;
	};

	$scope.locationChanged = function ()
	{
		$scope.subLocationsToDisplay = [];

		for(var i = 0; i < $scope.subLocations.length; i++)
		{
			if ($scope.subLocations[i].LocationId === $scope.item.Location.ID)
			{
				$scope.subLocationsToDisplay.push($scope.subLocations[i]);
			}
		}
	};

	/* 
	 * Server Functions
	 */

	$scope.loadData = function (itemID)
	{
		if (itemID === undefined || itemID === null)
		{
			itemID = 0;
		}

		var idDTO =
			{
				"ID": itemID
			};

		$http.post("/Admin/GetInitialData", idDTO).
		then(function success(data)
		{
			$scope.requests = data.data.Requests;
			$scope.checkouts = data.data.Checkouts;
			$scope.item = data.data.Item;

			if($scope.item.ID > 0)
			{
				setEditValues($scope.item);
			}
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
		// set IDs
		$scope.item.LocationId = $scope.item.Location.ID;
		$scope.item.CategoryId = $scope.item.Category.ID;

		if ($scope.item.SubLocation !== null)
		{
			$scope.item.SubLocationId = $scope.item.SubLocation.ID;
		}

		$scope.item.Item2AgeGroup =[];
		$scope.item.Item2Subject =[];

		// add many-to-many age groups
		for (var i = 0; i < $scope.selectedAgeGroups.length; i++)
		{
			if($scope.selectedAgeGroups[i] !== null)
			{
				$scope.item.Item2AgeGroup.push(
					{
						"AgeGroupId": $scope.selectedAgeGroups[i].ID
					});
			}
		}

		// add many-to-many subjects
		for (var i = 0; i < $scope.selectedSubjects.length; i++)
		{
			if ($scope.selectedSubjects[i] !== null)
			{
				$scope.item.Item2Subject.push(
					{
						"SubjectId": $scope.selectedSubjects[i].ID
					});
			}
		}

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

	var itemID = getParameter("itemID");
	
	$scope.loadData(parseInt(itemID));

	var tab = getParameter("tab");

	if(tab !== null)
	{
		$scope.curTab = parseInt(tab, 10);
	}
});