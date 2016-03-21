(function () {
    'use strict';
    app.controller('clientPaymentController', ['$rootScope', '$scope', '$location', '$state', 'clientPaymentService', 'ngAuthSettings', '$timeout', 'localStorageService', '$window', '$cookies', '$translate', '$remember', '$http', '$stateParams',
        function ($rootScope, $scope, $location, $state, clientPaymentService, ngAuthSettings, $timeout, localStorageService, $window, $cookies, $translate, $remember, $http, $stateParams) {
            
            var ToatalValue = $stateParams.totalValue;
            var clientId = $stateParams.clientId;
            var jobId = $stateParams.jobId;


            $scope.payClientPayment = function () {
              $rootScope.showLoader = true;
                $scope.Model = {
                    TotalValue: ToatalValue,
                    UserId:clientId,
                    JobId:jobId,
                    Language: ngAuthSettings.currentLanguage,
                    Currency: "SAR",
                }
                clientPaymentService.payClientPayment($scope.Model).then(function (response) {
                    var form = $('<form></form>');
                    form.attr("action", "https://sbcheckout.payfort.com/FortAPI/paymentPage");
                    form.attr("method", "POST");
                    form.attr("style", "display:none;");
                    addFormFields(form, response);
                    var body = angular.element(document).find('body').eq(0);

                    body.append(form);
                    form.submit();
                    form.remove();
$rootScope.showLoader = false;
                }, function (response) {
$rootScope.showLoader = false;

                });

            };
            function addFormFields(form, data) {
                if (data != null) {
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "access_code").val(data.AccessCode);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "amount").val(data.TotalValue);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "currency").val(data.Currency);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "language").val(data.Language);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "customer_email").val(data.Email);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "merchant_identifier").val(data.MerchantIdentifier);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "command").val(data.Command);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "merchant_reference").val(data.MerchantReference);
                    form.append(input);
                    var input = $("<input></input>").attr("type", "hidden").attr("name", "signature").val(data.SecretKey);
                    form.append(input);
                  }


            }

        }]);
}());