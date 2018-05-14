var app = angular.module('rev', ['ngRoute']);

// routes
//app.config(['$locationProvider', '$routeProvider',
//	function config($locationProvider, $routeProvider)
//	{
//		$locationProvider.hashPrefix('!');

//		$routeProvider.
//			when('/index',
//			{
//				templateUrl: 'home/index',
//				controller: function () { }
//			}).
//			when('/about',
//			{
//				templateUrl: 'home/about',
//				controller: function () { }
//			});
//	}]);

// components
app.component('showSiteName',
{
	template: '{{$ctrl.name}}',
	controller: function ()
	{
		this.name = "Revolutionary Learning Library";
	}
}).component('loginError',
{
	template: '<label class="text-danger col-md-offset-2">Invalid Email or Password</label>',
	controller: 'loginController'
});