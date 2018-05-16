angular.module("libraryApp").controller("libraryController", function ($scope, $http)
{
	// variables
	$scope.error = false;
	$scope.errorMessage = "";

	$scope.init = function(subjects, ageGroups, locations)
	{
		var blank =
			{
				"ID": 0,
				"Name": "",
				"StatusCode": 200,
				"StatusCodeSuccess": true,
				"StatusMessage": null
			};

		$scope.subjects = [];
		//$scope.subjects.push(blank);
		$scope.subjects = $scope.subjects.concat(JSON.parse(subjects));

		$scope.ageGroups = [];
		//$scope.ageGroups.push($.extend(blank));
		$scope.ageGroups = $scope.ageGroups.concat(JSON.parse(ageGroups));

		$scope.locations = [];
		//$scope.locations.push($.extend(blank));
		$scope.locations = $scope.locations.concat(JSON.parse(locations));
	}

	// default filter settings
	$scope.filter =
		{
			"CategoryId": 1,
			"AgeGroupId": 0,
			"SubjectId": 0,
			"LocationId": 0
		};
	
	$scope.categoryText = "Books";

	$scope.selectedAgeGroup = null;
	$scope.selectedSubject = null;
	$scope.selectedLocation = null;
	$scope.items = [];

	// functions
	$scope.getItems = function()
	{
		if ($scope.selectedSubject !== null)
		{
			$scope.filter.SubjectId = $scope.selectedSubject.ID;
		}

		if ($scope.selectedAgeGroup !== null)
		{
			$scope.filter.AgeGroupId = $scope.selectedAgeGroup.ID;
		}

		if ($scope.selectedLocation !== null)
		{
			$scope.filter.LocationId = $scope.selectedLocation.ID;
		}

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

	$scope.clearFilter = function ()
	{
		$scope.selectedAgeGroup = null;
		$scope.selectedSubject = null;
		$scope.selectedLocation = null;
		
		// default filter settings
		$scope.filter =
			{
				"CategoryId": 1,
				"AgeGroupId": 0,
				"SubjectId": 0,
				"LocationId": 0
			};

		$scope.getItems();
	};

	// load initial data
	$scope.getItems();
});