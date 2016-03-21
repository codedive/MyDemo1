(function () {
    angular.module('ngLoadingSpinner', ['angularSpinner'])
    .directive('usSpinner', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            link: function (scope, elm, attrs) {
                $rootScope.spinnerActive = false;
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (loading) {
                    $rootScope.spinnerActive = loading;
                    if (loading) {
                        document.getElementById('loaderimage').style.display = "block";
                        elm.removeClass('ng-hide');
                    } else {
                        elm.addClass('ng-hide');
                        document.getElementById('loaderimage').style.display = "none";
                    }
                });
            }
        };

    }]);
}).call(this);
