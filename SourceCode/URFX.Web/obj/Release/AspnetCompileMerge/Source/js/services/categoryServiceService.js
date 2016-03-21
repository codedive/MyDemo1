
app.factory('categoryServiceService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _getAllCategoryServices = function () {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/ServiceCategories',

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _payNow = function (Model) {
     
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'POST',
            url: serviceBase + '/api/Plans/PayNow',
            data: Model,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }
            

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _savePlanForServiceProvider = function (Model) {
     
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'POST',
            url: serviceBase + '/api/Plans/ReturnSucceesCallback',
            data: Model,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }
            

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _getAllTransactions = function () {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Plans/GetAllTransactions',

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _paging = function (pagingModel) {
       
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Post',
            url: serviceBase + '/api/Plans/Paging',
            data:pagingModel,
            headers: {    
                'Authorization': 'Bearer ' + tokenData.token,            
                'Accept-Language': ngAuthSettings.currentLanguage,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;    }

    var _refund = function (cartId) {
     
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'POST',
            url: serviceBase + '/api/Plans/Refund/?cartId=' + cartId,
            
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                

            }
            

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }
    var _addCategory = function (serviceModal) {
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'POST',
            url: serviceBase + '/api/ServiceCategories',
            transformRequest: function (data) {
                var formdata = new FormData();
                formdata.append("model", angular.toJson(data.model));
                
                //now add all of the assigned files
                if (data.serviceCategoryLogoFile != undefined) {
                    for (var i = 0; i < data.serviceCategoryLogoFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formdata.append("file" + i, data.serviceCategoryLogoFile[0]);
                    }
                }
                return formdata;
            },
            data: { model: serviceModal.model, serviceCategoryLogoFile: serviceModal.serviceCategoryLogoFile },
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                'Content-Type': undefined
            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _updateCategory = function (id, serviceModal) {
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'PUT',
            url: serviceBase + '/api/ServiceCategories/' + id,
            transformRequest: function (data) {
                var formdata = new FormData();
                formdata.append("model", angular.toJson(data.model));

                //now add all of the assigned files
                if (data.serviceCategoryLogoFile != undefined) {
                    for (var i = 0; i < data.serviceCategoryLogoFile.length; i++) {

                        //add each file to the form data and iteratively name them
                        formdata.append("file" + i, data.serviceCategoryLogoFile[0]);
                    }
                }
                return formdata;
            },
            data: { model: serviceModal.model, serviceCategoryLogoFile: serviceModal.serviceCategoryLogoFile },
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
                'Content-Type': undefined
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _deleteCategory = function (id) {
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'DELETE',
            url: serviceBase + '/api/ServiceCategories/' + id,
            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

    var _deleteTransaction=function(transactionId){
      var deferred = $q.defer();
      
        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'DELETE',
            url: serviceBase + '/api/Plans/DeleteTransaction/?Id='+transactionId,

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
}

    var _showDetails=function(transactionId){
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Plans/GetPlanById/?Id=' + transactionId,

            headers: {
                'Authorization': 'Bearer ' + tokenData.token,
                'Accept-Language': ngAuthSettings.currentLanguage,

            }

        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
}
    authServiceFactory.refund = _refund;
    authServiceFactory.paging = _paging;
    authServiceFactory.getAllTransactions = _getAllTransactions;
    authServiceFactory.payNow = _payNow;
    authServiceFactory.getAllCategoryServices = _getAllCategoryServices;
    authServiceFactory.addCategory = _addCategory;
    authServiceFactory.updateCategory = _updateCategory;
    authServiceFactory.deleteCategory = _deleteCategory;
    authServiceFactory.savePlanForServiceProvider = _savePlanForServiceProvider;
    authServiceFactory.deleteTransaction = _deleteTransaction;
    authServiceFactory.showDetails=_showDetails;
    return authServiceFactory;
}]);