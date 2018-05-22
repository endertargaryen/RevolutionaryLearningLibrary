
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

/*
 * Admin Controller
 */

angular.module('adminApp', []).component('siteName',
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
})
.directive('fdInput', [function ()
{
	return {
		link: function (scope, element, attrs)
		{
			element.on('change', function (evt)
			{
				var files = evt.target.files;
				scope.item.ImageName = files[0].name;
			});
		}
	}
}]);
