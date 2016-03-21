
app.factory('planService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};
    var _getAllPlans = function () {
       
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'GET',
            url: serviceBase + '/api/Plans',

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

       // tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'POST',
            url: serviceBase + '/api/Plans/ReturnSucceesCallback',
            data: Model,
            headers: {
               // 'Authorization': 'Bearer ' + tokenData.token,
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
    var _addPlan = function (planModal) {
        var deferred = $q.defer();

        tokenData = localStorageService.get('authorizationData');
        $http({

            method: 'POST',
            url: serviceBase + '/api/Plans',
            data:planModal,
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

    var _updatePlan = function (planModal) {
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'PUT',
            url: serviceBase + '/api/Plans/'+planModal.PlanId,
            data: planModal,
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

    var _deletePlan = function (PlanId) {
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'DELETE',
            url: serviceBase + '/api/Plans/' + PlanId,
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
            url: serviceBase + '/api/Plans/GetTranscationHistoryById/?Id=' + transactionId,

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
    authServiceFactory.getAllPlans = _getAllPlans;
    authServiceFactory.addPlan = _addPlan;
    authServiceFactory.updatePlan = _updatePlan;
    authServiceFactory.deletePlan=_deletePlan;
    authServiceFactory.savePlanForServiceProvider = _savePlanForServiceProvider;
    authServiceFactory.deleteTransaction = _deleteTransaction;
    authServiceFactory.showDetails=_showDetails;
    return authServiceFactory;
}]);