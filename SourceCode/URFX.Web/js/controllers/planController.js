'use strict'

app.controller('planController', ['$scope','$rootScope', 'modalService', 'planService', '$modal',
function ($scope,$rootScope, modalService, planService,$modal) {
    $scope.isUpdate = false;
    $scope.isAdd = false;
    $scope.PlanList = [];
    $scope.alerts = [];
    getAllPlans();
    $scope.PlanModel = {
        ApplicationFee: 0,
        CreatedDate: new Date(),
        Description: null,
        Detail: null,
        IsActive: true,
        PerVisitPercentage: 0,
        PlanId: 0,
        TeamRegistrationFee: 0,
        TeamRegistrationType: 0,
        PlanFeatures:null
    }
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.closepopAlert = function (index) {
        $scope.popalerts.splice(index, 1);
    };
    var modalInstance = '';
    var openModal = function () {
        $scope.alerts = [];
        modalInstance = $modal.open({
            templateUrl: 'tpl/administration/configuration/plan/plan.html',
            scope: $scope,
            size:'md'
        })
    }
    $scope.closeModal = function () {
        if (modalInstance) {
            modalInstance.close('dismiss');
        }
    }
    function getAllPlans(){
$rootScope.showLoader = true;
    planService.getAllPlans().then(function (response) {
        $scope.searchText;
        //console.log(response)
        $scope.PlanList = response;
$rootScope.showLoader = false;
    },function(error){});
    }
    $scope.openAdd = function () {
        $scope.isUpdate = false;
        $scope.popalerts = [];
        $scope.isAdd = true;
        $scope.PlanModel = {
            ApplicationFee: 0,
            CreatedDate: new Date(),
            Description: null,
            Detail: null,
            IsActive: true,
            PerVisitPercentage: 0,
            PlanId: 0,
            TeamRegistrationFee: 0,
            TeamRegistrationType: 0,
        }
        openModal();
    }
    $scope.selectPlan = function (plan) {
        $scope.isUpdate = true;
        $scope.isAdd = false;
        $scope.PlanModel = angular.copy(plan);
        openModal();
    }

    $scope.addPlan = function () {
        planService.addPlan($scope.PlanModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Plan has been added successfully.' });
            //$scope.PlanList.push(response);
            getAllPlans();
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        });
    }
    $scope.updatePlan = function () {
        planService.updatePlan($scope.PlanModel).then(function (response) {
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: 'Plan has been updated successfully.' });
            var index = $scope.PlanList.map(function (elem) { return elem.PlanId }).indexOf($scope.PlanModel.PlanId);
            //$scope.PlanList[index] = response;
           
            $scope.PlanList[index].ApplicationFee=$scope.PlanModel.ApplicationFee;
            $scope.PlanList[index].CreatedDate=$scope.PlanModel.CreatedDate;
            $scope.PlanList[index].Description=$scope.PlanModel.Description;
            $scope.PlanList[index].Detail=$scope.PlanModel.Detail;
            $scope.PlanList[index].IsActive=$scope.PlanModel.IsActive;
            $scope.PlanList[index].PerVisitPercentage=$scope.PlanModel.PerVisitPercentage;
            $scope.PlanList[index].TeamRegistrationFee=$scope.PlanModel.TeamRegistrationFee;
            $scope.PlanList[index].TeamRegistrationType = $scope.PlanModel.TeamRegistrationType;
            $scope.PlanList[index].PlanFeatures = $scope.PlanModel.PlanFeatures;
            $scope.closeModal();
        }, function (error) {
            $scope.popalerts = [];
            $scope.popalerts.push({ type: 'warning', msg: error.Message });
        })
    }
    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Plan?',
        bodyText: 'Are you sure you want to delete this plan?'
    };
    $scope.deletePlan = function (plan) {
        modalOptions.headerText = "Delete Plan?";
        modalService.showModal({}, modalOptions).then(function (result) {
            planService.deletePlan(plan.PlanId).then(function (response) {
                var index = $scope.PlanList.map(function (elem) { return elem.PlanId }).indexOf(plan.PlanId);
                //$scope.PlanList.splice(index, 1);
                getAllPlans();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: 'Plan has been deleted successfully.' });
            }, function (error) {
                $scope.alerts = [];
                $scope.alerts.push({ type: 'warning', msg: error.Message });
            })
        });
    }

    $scope.clear = function () {
       
        $scope.searchText = "";
        
    }
}]);