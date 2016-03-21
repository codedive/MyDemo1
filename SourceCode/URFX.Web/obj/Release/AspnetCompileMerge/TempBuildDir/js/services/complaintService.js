app.factory('complaintService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', '$location', function ($http, $q, localStorageService, ngAuthSettings, $location) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var authServiceFactory = {};

    var _getComplaintByServiceProviderId = function (serviceProviderId) {
      tokenData = localStorageService.get('authorizationData');
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: serviceBase + '/api/Complaints/GetComplaintByServiceProviderId/?serviceProviderId=' + serviceProviderId,
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

    var _updateComplaintByComplaintId = function (complaintId) {
    tokenData = localStorageService.get('authorizationData');
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: serviceBase + '/api/Complaints/UpdateComplaint/?complaintId=' + complaintId,
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
var _showDetails = function (complaintId) {
    tokenData = localStorageService.get('authorizationData');
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: serviceBase + '/api/Complaints/GetComplaintByComplaintId/?complaintId=' + complaintId,
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
    var _deleteSelectedComplaints = function (complaints) {
        
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/Complaints/DeleteSelectedComplaints',
            data: complaints,
            headers: {
                'Content-Type': 'application/json',
                'Accept-Language': ngAuthSettings.currentLanguage,
                 'Authorization': 'Bearer ' + tokenData.token,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    }

     var _getAllComplaints = function () {
      tokenData = localStorageService.get('authorizationData');
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: serviceBase + '/api/Complaints/GetAllComplaints',
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
     var _deleteComplaint = function (id) {
       
        var deferred = $q.defer();
        tokenData = localStorageService.get('authorizationData');
        $http({
            method: 'Delete',
            url: serviceBase + '/api/Complaints/?id='+id,
          
            headers: {
                'Content-Type': 'application / json',
                'Accept-Language': ngAuthSettings.currentLanguage,
                  'Authorization': 'Bearer ' + tokenData.token,
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
}
    
    authServiceFactory.getAllComplaints = _getAllComplaints;
    authServiceFactory.getComplaintByServiceProviderId = _getComplaintByServiceProviderId;
    authServiceFactory.updateComplaintByComplaintId = _updateComplaintByComplaintId;
    authServiceFactory.deleteSelectedComplaints = _deleteSelectedComplaints;
    authServiceFactory.deleteComplaint=_deleteComplaint;
    authServiceFactory.showDetails=_showDetails;
    return authServiceFactory;
    
}]);


