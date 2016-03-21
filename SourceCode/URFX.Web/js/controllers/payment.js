'use strict';

app.controller('PaymentController', ['$scope', '$rootScope', '$location', '$timeout', 'authService', 'categoryService', 'subService', 'subCategory', 'cityService', '$state', 'planService', 'districtService', '$stateParams', '$http', 'ngAuthSettings', 'pagingservice','$filter','modalService',
function ($scope, $rootScope, $location, $timeout, authService, categoryService, subService, subCategory, cityService, $state, planService, districtService, $stateParams, $http, ngAuthSettings, pagingservice, $filter, modalService) {  
    
    $scope.alerts = [];
    $scope.SelectedPlan;
    $scope.NumberOfTeams = 1;
    $scope.itemPlans = [];

    var serviceProviderId = $stateParams.serviceProviderId;
    
    
    var sPageURL =decodeURIComponent(window.location.search.substring(1));
    if (sPageURL != "") {
        SavePlanForServiceProvider();
    }
  
    if ($stateParams.Id != ""&&$stateParams.Id !=undefined) {
 
   showDetails($stateParams.Id);
    }

    function getUrlParameter(param, dummyPath) {
        
        var sPageURL = decodeURIComponent(window.location.search.substring(1)),
            sURLVariables = sPageURL.split(/[&||?]/),
            res;

        for (var i = 0; i < sURLVariables.length; i += 1) {
            var paramName = sURLVariables[i],
                sParameterName = (paramName || '').split('=');

            if (sParameterName[0] === param) {
                res = sParameterName[1];
            }
        }

        return res;
    }

    function SavePlanForServiceProvider() {        
        $scope.transcationHistoryModel = {
            CardNumber: getUrlParameter("card_number"),
            Status: getUrlParameter("status"),
            ResponseCode: getUrlParameter("response_code"),
            ResponseMessage: getUrlParameter("response_message"),
            CartId: getUrlParameter("merchant_reference"),
            Eci: getUrlParameter("eci"),
            FortId: getUrlParameter("fort_id"),
            CustomerEmail: getUrlParameter("customer_email"),
            CustomerIp: getUrlParameter("customer_ip"),
            Amount: getUrlParameter("amount"),
            Command: getUrlParameter("command"),
            PaymentOption: getUrlParameter("payment_option"),
            ExpiryDate: getUrlParameter("expiry_date"),
            Signature: getUrlParameter("signature"),
            MerchantReference: getUrlParameter("merchant_reference"),
   }
      
        $location.search({});
        planService.savePlanForServiceProvider($scope.transcationHistoryModel).then(function (response) {
            if (response != null) {
                if (response.URFXPaymentType == 0) {
                    if (response.Status == 14) {
                        //window.location.href = '/App/#/sentemailsuccessfull';
                        window.location.href = "/App/#/serviceprovider/" + response.UserId + "/profile"
                        $scope.app.currentUser.planId = response.PlanId;
                    }
                    else {
                        window.location.href = "/App/#/serviceprovider/" + response.UserId + "/payment"
                        $scope.alerts = [];
                        $scope.alerts.push({ type: 'warning', msg: "Transaction failed." });
                    }
                }
                else {
                    $state.go('signin');
                }
            }
            
                
            
            
        });

    }

    $scope.cancelSignUp = function () {
        $location.path('/login');
    };


    $scope.PayNow = function () {
      
        $scope.Model = {
            PlanId: $scope.selectedPlan,
            TotalValue: $scope.TotalValue,
            NumberOfTeams: $scope.NumberOfTeams,
            Language: ngAuthSettings.currentLanguage,
            Currency: "SAR",
        }
        if ($scope.TotalValue > 0){
        planService.payNow($scope.Model).then(function (response) {
          
            var form = $('<form></form>');
            form.attr("action", "https://sbcheckout.payfort.com/FortAPI/paymentPage");
            form.attr("method", "POST");
            form.attr("style", "display:none;");
            addFormFields(form, response);
            var body = angular.element(document).find('body').eq(0);

            body.append(form);
            form.submit();
            form.remove();

        });
        }
        else{
          $scope.alerts = [];
          $scope.alerts.push({ type: 'warning', msg: "Enter valid payment amount." });
}
        
        

    }

    function addFormFields(form, data) {
        if (data != null) {
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_store").val("14900");
            //form.append(input); 
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_amount").val(data.TotalValue);
            //form.append(input); 
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_currency").val("SAR");
            //form.append(input); 
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_test").val("1");
            //form.append(input);                                                  
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_timestamp").val("0");
            //form.append(input);                                                  
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_cart").val(data.CartId);
            //form.append(input);                                                  
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_desc").val("Items");
            //form.append(input);                                                  
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_extra").val("none");
            //form.append(input);                                                  
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "ivp_signature").val(data.SecretKey);
            //form.append(input);  
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
            //var input = $("<input></input>").attr("type", "hidden").attr("name", "return_url").val(data.ReturnUrl);
            //form.append(input);



        }


    }

    $scope.Refund = function (cartId) {
       
        //angular.forEach($scope.SelectedTransactions, function (value, key) {
        //    if (value.CartId == cartId) {
                
        //        $scope.refundModel = [];
        //        $scope.refundModel = value;
        //    }

        //})
        planService.refund(cartId).then(function (response) {
           
           //var form = $('<form></form>');
            //form.attr("action", "https://sbpaymentservices.payfort.com/FortAPI/paymentApi");
            //form.attr("method", "POST");
            //form.attr("style", "display:none;");
            //addFormFieldsForRefund(form, response);
            //var body = angular.element(document).find('body').eq(0);
            //body.append(form);
            //form.submit();
            //form.remove();
        });
       


    }

    function addFormFieldsForRefund(form, data) {
        if (data != null) {
            var input = $("<input></input>").attr("type", "hidden").attr("name", "access_code").val(data.AccessCode);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "amount").val(data.Amount);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "currency").val(data.Currency);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "language").val(data.Language);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "merchant_identifier").val(data.MerchantIdentifier);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "command").val(data.Command);
            form.append(input);
            var input = $("<input></input>").attr("type", "hidden").attr("name", "signature").val(data.Signature);
            form.append(input);
          }


    }

    function PaymntView() {

        $scope.showPaymentPage = true;
        $scope.showRegisterPage = false;
        planService.getAllPlans().then(function (response) {

            if ($scope.itemPlans.length == 0) {
                angular.forEach(response, function (value, key) {
                    this.push(value);
                }, $scope.itemPlans)

            }


            $scope.SelectedPlan = $scope.itemPlans[0].PlanId;
            $scope.checked = true;
            $scope.SetValuesForPlan($scope.SelectedPlan);
        });

    }

    $scope.SetValuesForPlan = function (selectedPlan) {
     
        $scope.selectedPlan = selectedPlan;
        angular.forEach($scope.itemPlans, function (value, key) {


            if (value.PlanId == selectedPlan) {
                $scope.TeamCost = value.TeamRegistrationFee;
                $scope.ApplicationFee = value.ApplicationFee;
                $scope.TotalValue = value.TeamRegistrationFee + value.ApplicationFee;

            }

            
        });

    }

    $scope.SetValuesForPlanForNumerOfEmployee = function (NumberOfTeams) {
    
        $scope.NumberOfTeams = NumberOfTeams;
        angular.forEach($scope.itemPlans, function (value, key) {


            if (value.PlanId == $scope.selectedPlan) {
                $scope.TeamCost = value.TeamRegistrationFee;
                $scope.ApplicationFee = value.ApplicationFee;
                $scope.TotalValue = value.TeamRegistrationFee + value.ApplicationFee;

            }


        });
        if ($scope.NumberOfTeams != null && $scope.NumberOfTeams != "" && $scope.NumberOfTeams != 0) {
            $scope.TeamCost = $scope.TeamCost * $scope.NumberOfTeams;
            $scope.TotalValue = $scope.TeamCost + $scope.ApplicationFee;
        }
        
    }

    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Employee?',
        bodyText: 'Are you sure you want to delete this employee?'
    };

    PaymntView();
    $scope.deleteTransaction=function(transactionId){
   
        modalOptions.headerText = "Delete Record?";
        modalOptions.bodyText="Are you sure you want to delete this transaction?"
        modalService.showModal({}, modalOptions).then(function (result){
        planService.deleteTransaction(transactionId).then(function () {
            
            var foundItem = $filter('filter')($scope.SelectedTransactions, { CartId: transactionId }, true)[0];


            var index = $scope.SelectedTransactions.indexOf(foundItem);

            $scope.SelectedTransactions.splice(index, 1);

            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: " Transaction has been deleted successfully." });
        })
        })
        
    }

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    function GetAllTransaction() {
    $rootScope.showLoader = true;
        paging();
        
    }


    //pagging work
    $scope.itemsPerPage = pagingservice.pagesize;
    $scope.currentPage = 1;
    $scope.items = [];
    $scope.range = function () {
        $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
        $scope.showPaging = '';
        if ($scope.itemsPerPage == "") {
            $scope.itemsPerPage = pagingservice.pagesize;
        }
        if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
            $scope.showPaging = true;
        }
        else {
            $scope.showPaging = false;
        }
        if (Math.ceil($scope.totalcount / $scope.itemsPerPage) <= 5) {
            var rangeSize = Math.ceil($scope.totalcount / $scope.itemsPerPage);
        }
        else {
            var rangeSize = 5;
        }
        var ret = [];

        var start;
        start = $scope.currentPage;
        if (start > $scope.pageCount() - rangeSize) {
            start = $scope.pageCount() - rangeSize + 1;
        }

        for (var i = start; i < start + rangeSize; i++) {
            ret.push(i);
        }

        return ret;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage--;
            paging();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 1 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.totalcount / $scope.itemsPerPage);
    };

    $scope.nextPage = function () {

        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
            paging();
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {

        $scope.currentPage = n;
        paging();
    };

    $scope.firstPageDisabled = function () {
        return $scope.currentPage === 1 ? "disabled" : "";
    };
    $scope.lastPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };
    $scope.firstPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage = 1;
            paging();
        }
    }
    $scope.lastPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage = $scope.pageCount();
            paging();
        }
    }

    $scope.searchRecord = function () {

        $scope.currentPage = 1;
        paging();
    }
     $scope.clear = function () {
        
     $scope.SearchText="";
     paging();
    }
    function paging () {
        
        var pagingModel = {
            CurrentPageIndex: $scope.currentPage,
            Pagesize: $scope.itemsPerPage,
            SearchText: $scope.SearchText
        }

        planService.paging(pagingModel).then(function (response) {

            if (response.data != null) {
                //$scope.unSelect();
                $scope.SelectedTransactions = response.data;
                $scope.totalcount = response.totalRecords;
                if ($scope.currentPage == 1) {
                    $scope.itemCount = $scope.SelectedTransactions.length;
                    $scope.startRecord = 1;

                }
                else {

                    if ($scope.SelectedTransactions.length < $scope.itemsPerPage) {
                        $scope.itemCount = ($scope.itemsPerPage * $scope.currentPage) - $scope.itemsPerPage + $scope.SelectedTransactions.length;
                        $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1;
                    }
                    else {
                        $scope.itemCount = ($scope.itemsPerPage * ($scope.currentPage));//$scope.itemCount + data.data.length;
                        $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1; //data.data.length + $scope.startRecord +1;
                    }
                }

                $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
                if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
                    $scope.showPaging = true;
                }
                else {
                    $scope.showPaging = false;
                }
               startTimer();
            }
        },
             function (error) {
                 $scope.errormessage = "Error happend ";
                 startTimer();
             }
             );
              
    }
    //End work
    if (serviceProviderId == null) {
        GetAllTransaction();
    }
    
    function showDetails(transactionId){
    $rootScope.showLoader = true;
    planService.showDetails(transactionId).then(function(response){
       
        $scope.Details = response;
        $scope.PaymentType=response.URFXPaymentTypeString;
         startTimer();
})
}

    //function for checked and refresh
    //$(function () {

    //    $('#chkAll').click(function (event) {

    //        if (this.checked) {
    //            $('.SingleChk').each(function () {
    //                this.checked = true;
    //            });

    //        } else {
    //            $('.SingleChk').each(function () {
    //                this.checked = false;
    //            });
    //        }
    //        $scope.$apply(function () {
    //            $scope.refreshIsMultipleDelete();
    //        });

    //    });

    //});
    ////end function

    ////function for selectaunselect checkbox
    //$scope.selectUnselectAll = function () {

    //    if ($('input[name="SingleChk"]').length == $('input[name="SingleChk"]:checked').length) {

    //        $('input:checkbox[name="chkAll"]').attr("checked", "checked");
    //    }
    //    else {
    //        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    //    }
    //    $scope.refreshIsMultipleDelete();
    //}
    ////end function

    ////function for fill values for selected items
    //$scope.fillselectedServiceProviders = function () {

    //    $scope.selectedServiceProviders = [];
    //    $('input[name=SingleChk]').each(function () {
    //        if (this.checked) {
    //            $scope.selectedServiceProviders.push($(this).val());
    //        }
    //    });

    //}
    ////end function

    ////function for refresh if multiple select
    //$scope.refreshIsMultipleDelete = function () {

    //    $scope.fillselectedServiceProviders();
    //    if ($scope.selectedServiceProviders.length > 0) {
    //        $scope.IsMultipleDelete = true;
    //    }
    //    else {
    //        $scope.IsMultipleDelete = false;
    //    }
    //}

    //$scope.unSelect = function () {
    //    $('input:checkbox[name="chkAll"]').removeAttr("checked");
    //}

     var startTimer = function () {
       
         var timer = $timeout(function () {
             $timeout.cancel(timer);
             $rootScope.showLoader = false;
         }, 2000);
     }

}]);