angular.module("passwordApp").controller("passwordController", function ($scope, $http)
{
	/*
	 * Variables
	 */

	$scope.newPassword = "";
	$scope.confirmPassword = "";
	$scope.error = false;
	$scope.errorMessage = "";

	$scope.init = function(title, curPassword)
	{
		$scope.title = title;
		$scope.currentPassword = curPassword;
	}

	/*
	 * Functions
	 */
	
	$scope.changePassword = function (curPassword, newPassword, confirmPassword)
	{
		if (newPassword !== confirmPassword)
		{
			$scope.errorMessage = "Passwords do not match";
			$scope.error = true;
			return;
		}

		var parameters = JSON.stringify({
			Email: "",
			CurrentPassword: curPassword,
			NewPassword: newPassword,
			ConfirmPassword: confirmPassword
		});

		$http.post('ChangeUserPassword', parameters).
			then(function success(data)
			{
				if (data.data.StatusCode !== StatusCode.OK)
				{
					$scope.error = true;
					$scope.errorMessage = data.data.StatusMessage;
				}
				else
				{
					$scope.error = false;
					window.location = "/Home/Index";
				}
			},
			function error(data)
			{
				$scope.error = true;
				$scope.errorMessage = "Server error.  Contact the admin to change your password";
				alert(data.data);
			});
	};
});