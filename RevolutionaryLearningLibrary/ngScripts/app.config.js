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

/*components*/

// site name component
app.component('siteName',
{
	template: '{{$ctrl.name}}',
	controller: function ()
	{
		this.name = "Revolutionary Learning Library";
	}

// error component
}).component('error',
{
	bindings:
	{
		errorMessage: '<',
		hasError: '<'
	},
	controllerAs: 'model',
	template: '<div ng-show="model.hasError"><label class="text-danger col-md-offset-2">{{model.errorMessage}}</label></div>',
});