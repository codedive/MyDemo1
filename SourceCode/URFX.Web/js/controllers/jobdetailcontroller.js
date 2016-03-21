(function(){
'use strict'
    app.controller('jobdetailController',['$scope','$stateParams','$state','jobService',
function($scope,$stateParams,$state,jobService){
    $scope.selectedJobId=null;
    $scope.jobList=[];
    $scope.JobDetail = null;
    $scope.alerts = [];
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    if ($stateParams.jobId != null && $stateParams.jobId != "jobId") {
        $scope.selectedJobId = $stateParams.jobId;
    } else {
        $scope.alerts = [];
        $scope.alerts.push({ type: 'warning', msg: 'No Job Selected' });
        $scope.JobDetail = null;
    }
    jobService.getAllJobs().then(function(response){
        if(response!=null)
        {
            $scope.jobList=response.filter(function(elem){
                return elem.Status==3;
            })
        }
    });
    $scope.getJobByJobId = function () {
        $scope.alerts = [];
        if($scope.selectedJobId){
            jobService.getJobByJobId($scope.selectedJobId).then(function(response){
                $scope.JobDetail = response;
            }, function (error) {
                $scope.alerts.push({ type: 'warning', msg: error.Message });
            });
        }
        else{
            $scope.JobDetail = null;
            $scope.alerts.push({ type: 'warning', msg: 'No Job Selected' });
        }
        
    };
    if($scope.selectedJobId){
        $scope.getJobByJobId();
    }
    $scope.goBack = function () {
        window.history.back();
    }
}]);
})();

