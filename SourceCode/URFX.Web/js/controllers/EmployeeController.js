app.controller('EmployeeController', ['$scope', '$http','serviceProviderService', function($scope, $http,serviceProviderService) {
    var getAllServiceProviders = function () {
        serviceProviderService.getAllServiceProviders().then(function (response) {
           
            $scope.gridOptions = {
                data:response,
                columns: [
                    { data: 'CompanyName', },
                    { data: 'Description' },
                    { data: 'IsActive' },                    
                ]
            };
            console.log(JSON.stringify(response));
        });
    }
    getAllServiceProviders();    
}]);
