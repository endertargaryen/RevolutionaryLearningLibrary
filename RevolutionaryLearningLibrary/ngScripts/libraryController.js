﻿angular.module("libraryApp").controller("libraryController", function ($scope, $http)
{
	/* 
	 * variables
	 */ 
	$scope.error = false;
	$scope.errorMessage = "";

	// default filter settings
	$scope.filter =
		{
			"CategoryId": 1,
			"AgeGroupId": 0,
			"SubjectId": 0,
			"LocationId": 0,
			"CategoryId": 1
		};
	
	$scope.selectedAgeGroup = null;
	$scope.selectedSubject = null;
	$scope.selectedLocation = null;
	$scope.selectedCategory = null;
	$scope.items = [];
	$scope.selectedItem = null;
	$scope.isPickingCategory = false;
	$scope.checkoutItems = [];

	// initialization
	$scope.init = function (subjects, ageGroups, locations, categories)
	{
		$scope.subjects = JSON.parse(subjects);
		$scope.ageGroups = JSON.parse(ageGroups);
		$scope.locations = JSON.parse(locations);
		$scope.categories = JSON.parse(categories);

		$scope.selectedCategory = findItemInArray($scope.categories, "ID", $scope.filter.CategoryId);
	}

	/*
	 * functions
	 */
	$scope.isItemInCart = function (item)
	{
		var existingItem = findItemInArray($scope.checkoutItems, "ID", item.ID);

		return (existingItem !== null);
	};

	$scope.addItemToCart = function (item)
	{
		var existingItem = findItemInArray($scope.checkoutItems, "ID", item.ID);

		if (existingItem === null)
		{
			$scope.checkoutItems.push(item);
		}

		$("#itemModal").modal('hide');
	};

	$scope.removeItemFromCart = function (item)
	{
		$scope.checkoutItems.splice($scope.checkoutItems.indexOf(item), 1);
	};

	$scope.toggleCategoryDropDownDisplay = function (retrieveData)
	{
		$scope.isPickingCategory = !$scope.isPickingCategory;

		if (retrieveData)
		{
			$scope.getItems();
		}
	};

	$scope.clearFilter = function ()
	{
		$scope.selectedAgeGroup = null;
		$scope.selectedSubject = null;
		$scope.selectedLocation = null;
		$scope.isPickingCategory = false;

		// default filter settings
		$scope.filter =
			{
				"CategoryId": 1,
				"AgeGroupId": 0,
				"SubjectId": 0,
				"LocationId": 0
			};

		$scope.selectedCategory = findItemInArray($scope.categories, "ID", $scope.filter.CategoryId);

		$scope.getItems();
	};

	$scope.showCartModal = function ()
	{
		showModal("cartModal");
	};

	$scope.showItemModal = function (item)
	{
		$scope.selectedItem = item;

		showModal("itemModal");
	};

	/*
	 * AJAX
	 */

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

		if ($scope.selectedCategory !== null)
		{
			$scope.filter.CategoryId = $scope.selectedCategory.ID;
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
	 

	/*
	 * window onload
	 */
	$scope.getItems();
});