app.controller('complaintController', ['$scope','$rootScope', '$http', '$stateParams', 'pagingservice', 'complaintService','modalService',
function ($scope,$rootScope, $http, $stateParams, pagingservice, complaintService, modalService) {
    
    $scope.SearchText;
    $scope.itemsPerPage = pagingservice.pagesize;
    var serviceProviderId = $stateParams.serviceProviderId;
    var complaintId = $stateParams.complaintId;
    if (serviceProviderId != null) {
        getComplaintsByServiceProviderId(serviceProviderId);
    }
     if (complaintId != null) {
         ShowDetails(complaintId);
    }
    getAllComplaints();
    function getAllComplaints() {
         $rootScope.showLoader = true;
        complaintService.getAllComplaints().then(function (response) {
           
            $scope.allComplaints = response;
           $rootScope.showLoader = false;
            
        })
    }
    
    function ShowDetails(complaintId) {
       
        complaintService.showDetails(complaintId).then(function (response) {
            $scope.compliantDetails = [];
            
             $scope.compliantDetails=response;

           // getComplaintsByServiceProviderId(serviceProviderId);
           

        })
    }

    $scope.updateComplaintByComplaintId=function(complaintId) {
       
        complaintService.updateComplaintByComplaintId(complaintId).then(function (response) {
            getAllComplaints();

           // getComplaintsByServiceProviderId(serviceProviderId);
           

        })
    }

    function getComplaintsByServiceProviderId(serviceProviderId) {
$rootScope.showLoader = true;
        $scope.SearchText;
        complaintService.getComplaintByServiceProviderId(serviceProviderId).then(function (response) {
           
            $scope.allComplaintsByServiceProviderId = response;
$rootScope.showLoader = false;
          
        })
    }
        
    //function for checked and refresh
   

    //function for selectaunselect checkbox
    $scope.selectUnselectAll = function () {

        var aa = $(".SingleChk").length;
        if ($(".SingleChk").length == $('.SingleChk:checked').length) {

            $('#chkAll').prop("checked", true);
        }
        else {
            $('input:checkbox[name="chkAll"]').removeAttr("checked");
        }

        $scope.refreshIsMultipleDelete();
    }
    //end function

    $scope.fillselectedComplaints = function () {

        $scope.selectedComplaints = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.selectedComplaints.push($(this).val());
            }
        });

    }

    $scope.refreshIsMultipleDelete = function () {

        $scope.fillselectedComplaints();
        if ($scope.selectedComplaints.length >1) {
            $scope.IsMultipleDelete = true;
        }
        else {
            $scope.IsMultipleDelete = false;
        }
    }

    $scope.unSelect = function () {
        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    }


    $(function () {

        $('#chkAll').click(function (event) {

            if (this.checked) {
                $('.SingleChk').each(function () {
                    this.checked = true;
                });

            } else {
                $('.SingleChk').each(function () {
                    this.checked = false;
                });
            }
            $scope.$apply(function () {
                $scope.refreshIsMultipleDelete();
            });

        });

    });
    //end function

  

   


    var modalOptions = {
        closeButtonText: 'Cancel',
        actionButtonText: 'Delete',
        headerText: 'Delete Category service?',
        bodyText: 'Are you sure you want to delete selected complaints records?'
    };


    //function for delete multiple
    $scope.deleteSelectedComplaints = function () {

        modalOptions.headerText = "Delete Selected Service Providers?";
        
        modalService.showModal({}, modalOptions).then(function (result) {
            $scope.IsMultipleDelete = false;
            $scope.fillselectedComplaints();
            //   $('#deleteAllConfirmModal').modal('hide');
            var complaints = $scope.selectedComplaints;
            complaintService.deleteSelectedComplaints(complaints).then(function (data) {
               
                if (data != null) {
                   
                    if (serviceProviderId!=undefined)
                        getComplaintsByServiceProviderId(serviceProviderId);
                    else
                    getAllComplaints();
                    $scope.alerts = [];
                    $scope.alerts.push({ type: 'success', msg: "Selected complaints deleted successfully." });
                }
            }, function (error) {

            });
        })
        }
    //end function
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.deleteComplaint = function (obj) {
       
        modalOptions.bodyText = "Are you sure you want to delete this record?"
        modalOptions.headerText = "Delete Selected Complaint?";
        modalService.showModal({}, modalOptions).then(function (result) {
           
            complaintService.deleteComplaint(obj.ComplaintId).then(function () {
              
                $scope.alerts = [];
                $scope.alerts.push({ type: "success", msg: "Complaint record has been deleted successfully." });

            })
        })

    }

    $scope.clear = function () {
        
        $scope.SearchText = "";
        
    }

}]);