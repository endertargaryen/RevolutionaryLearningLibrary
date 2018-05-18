
/*
 * Login App
 */

angular.module('loginApp', []).component('siteName',
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

/*
 * Library App
 */

angular.module('libraryApp', []).component('siteName',
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
	template: '<div ng-show="model.hasError"><label class="text-danger col-md-offset-2">{{model.errorMessage}}</label></div>'
});

angular.module('passwordApp', []).component('error',
{
	bindings:
	{
		errorMessage: '<',
		hasError: '<'
	},
	controllerAs: 'model',
	template: '<div ng-show="model.hasError"><label class="text-danger col-md-offset-2">{{model.errorMessage}}</label></div>'
});