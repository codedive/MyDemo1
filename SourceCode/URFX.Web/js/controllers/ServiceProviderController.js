app.controller('ServiceProviderController', ['$scope', '$http', 'serviceProviderService','$stateParams',
function ($scope, $http, serviceProviderService, $stateParams) {
    

    //$scope.filterOptions = {
    //    filterText: "",
    //    useExternalFilter: true
    //};
    //$scope.totalServerItems = 0;
    //$scope.pagingOptions = {
    //    pageSizes: [3, 10, 30],
    //    pageSize: 3,
    //    currentPage: 1
    //};
    //$scope.setPagingData = function (data, page, pageSize) {
    //    var pagedData = data.slice((page - 1) * pageSize, page * pageSize);

    //    $scope.myData = pagedData;
    //    $scope.totalServerItems = data.length;
    //    if (!$scope.$$phase) {
    //        $scope.$apply();
    //    }
    //};
    //$scope.getPagedDataAsync = function (pageSize, page, searchText) {
    //    setTimeout(function () {
    //        //var data;
    //        //if (searchText) {
    //        //    var ft = searchText.toLowerCase();

    //        //    $http.get('js/controllers/largeLoad.json').success(function (largeLoad) {    
    //        //        data = largeLoad.filter(function(item) {
    //        //            return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
    //        //        });
    //        //        $scope.setPagingData(data,page,pageSize);
    //        //    });            
    //        //} else {
    //        //    $http.get('js/controllers/largeLoad.json').success(function (largeLoad) {
    //        //        $scope.setPagingData(largeLoad,page,pageSize);
    //        //    });
    //        //}

    //        serviceProviderService.getAllServiceProviders().then(function (response) {
    //            $scope.setPagingData(response, page, pageSize);
    //        });
    //    }, 100);
    //};

    //$scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    //$scope.$watch('pagingOptions', function (newVal, oldVal) {
    //    if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
    //        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    //    }
    //}, true);
    //$scope.$watch('filterOptions', function (newVal, oldVal) {
    //    if (newVal !== oldVal) {
    //        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    //    }
    //}, true);

    $scope.getAllServiceProviders = function () {
        serviceProviderService.getAllServiceProviders().then(function (response) {            
            if (response != undefined && response != null)
                $scope.serviceProviders = response;

        });
    }
    $scope.getAllServiceProviders();
    $scope.gridOptions = {
        data: $scope.serviceProviders,
        columns: [
            { data: 'CompanyName', },
            { data: 'Description' },
            { data: 'IsActive' },
        ]
    };
    $scope.$watch('serviceProviders', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.gridOptions = {
                data: newVal,
                columns: [
                    { data: 'CompanyName', },
                    { data: 'Description' },
                    { data: 'IsActive' },
                ]
            };
        }
    }, true);    
}]);
