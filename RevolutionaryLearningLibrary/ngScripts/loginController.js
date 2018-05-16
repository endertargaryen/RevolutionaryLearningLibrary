﻿angular.module("loginApp").controller("loginController", function ($scope, $http)
{
	// data
	$scope.init = function (loginUrl, returnUrl, redirectUrl)
	{
		$scope.loginUrl = loginUrl;
		$scope.returnUrl = returnUrl;
		$scope.redirectUrl = redirectUrl;
	};

	$scope.title = "Log In";
	$scope.email = "";
	$scope.password = "";
	$scope.error = false;
	$scope.errorMessage = "";

	// functions
	$scope.validateLogin = function (email, password)
	{
		var parameters = JSON.stringify({
			Email: email,
			Password: password
		});

		$http.post($scope.loginUrl, parameters).
			then(function success(data)
			{
				if (data.data.StatusCode !== StatusCode.OK)
				{
					$scope.error = true;
					$scope.errorMessage = "Error From Server: " + data.data.StatusCode.toString();
				}
				else
				{
					$scope.error = false;

					if ($scope.returnUrl != '')
					{
						window.location = $scope.returnUrl;
					}
					else
					{
						window.location = $scope.redirectUrl;
					}
				}
			},
			function error(data)
			{
				alert(data.data);
			});
	};

});