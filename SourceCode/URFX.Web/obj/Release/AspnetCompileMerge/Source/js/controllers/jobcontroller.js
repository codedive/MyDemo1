/// <reference path="../app.js" />
app.controller('jobcontroller', ['$scope','$rootScope', '$http', '$stateParams', '$state', 'pagingservice', 'jobService', 'employeeService', '$timeout', '$filter','localStorageService',
function ($scope,$rootScope, $http, $stateParams, $state, pagingservice, jobService, employeeService, $timeout, $filter,localStorageService) {
   
    
    $scope.SearchNewJobText = "";
    $scope.SearchCurrentJobText="";
    $scope.SearchCompletedJobText="";
    $scope.SearchRejectedJobText="";
    $scope.allJobsByServiceProviderId;
    $scope.EmployeeList;
    $scope.employeeId;
    $scope.JobId;
    var serviceProviderId = $stateParams.serviceProviderId;
    var jobId = $stateParams.jobId;
    $scope.JobId = jobId;
    var getMessages = function () {
        
    var successMsg = localStorageService.get('SuccessMsg');
    if (successMsg != null && successMsg != "" && successMsg != undefined) {
        $scope.alerts = [];
        $scope.alerts.push({ type: 'success', msg: successMsg });
        localStorageService.remove('SuccessMsg');
    }
     
    } 
    if (serviceProviderId != null) {
        getMessages();
        getJobByServiceProviderId(serviceProviderId);
       // getEmployeeByServiceProviderId(serviceProviderId,jobId);
    }
    else {
        getMessages();
        getAllJobs();
    }
    if (jobId != null) {
     
        getJobByJobId(jobId);
    }
    if (serviceProviderId != null && jobId != null) {
       
        getEmployeeByServiceProviderId(serviceProviderId, jobId);
    }
    $scope.showPaging = false;

    $scope.files = [];
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

     
    function getAllJobs() {
         $rootScope.showLoader = true;
        jobService.getAllJobs().then(function (response) {
            $scope.allJobs = response;
            $scope.allNewJobs = [];
            $scope.allCurrentJobs = [];
            $scope.allCompletedJobs = [];
            $scope.allAcceptedJobs = [];
            $scope.allRejectedJobs = [];
           // $scope.allJobsByServiceProviderId = response;

            angular.forEach(response, function (value, key) {

                if (value.Status == 0 && value.EmployeeId == null) {
                    
                    $scope.allNewJobs.push(value);
                    
                }
                else if (value.Status == 1) {
                    $scope.allRejectedJobs.push(value);
                }
                else if (value.Status == 2) {
                    $scope.allCurrentJobs.push(value);
                }
                else if (value.Status == 3) {
                    $scope.allCompletedJobs.push(value);
                    $scope.SearchCompletedJobText;
                }


            })
            startTimer();
            //$scope.totalcount = $scope.allJobs.length;
            //sessionStorage.setItem("totalCount", $scope.totalcount);
            //for (var i = 1; i <= $scope.totalcount; i++) {
            //    $scope.items.push({ id: i, name: "name " + i });
            //}
            //$scope.paging();
        })
        
    }

    function getJobByServiceProviderId(serviceProviderId) {
        $rootScope.showLoader = true;
        jobService.getJobByServiceProviderId(serviceProviderId).then(function (response) {
           
            $scope.allNewJobs = [];
            $scope.allCurrentJobs = [];
            $scope.allCompletedJobs = [];
            $scope.allAcceptedJobs = [];
            $scope.allRejectedJobs = [];
            $scope.allJobsByServiceProviderId = response;

            angular.forEach($scope.allJobsByServiceProviderId, function (value, key) {
                
                if( $scope.app.currentUser.type=="Individual"){
                if (value.Status == 0) {
                     $scope.allNewJobs.push(value);

                }
                else if (value.Status == 1) {
                    $scope.allRejectedJobs.push(value);
                }
                else if (value.Status == 2) {
                    $scope.allCurrentJobs.push(value);
                }
                else if (value.Status == 3) {
                    $scope.allCompletedJobs.push(value);
                }

}
else{
if (value.Status == 0 && value.EmployeeId == null) {
                     $scope.allNewJobs.push(value);

                }
                else if (value.Status == 1) {
                    $scope.allRejectedJobs.push(value);
                }
                else if (value.Status == 2) {
                    $scope.allCurrentJobs.push(value);
                }
                else if (value.Status == 3) {
                    $scope.allCompletedJobs.push(value);
                }
}
                


            })
            startTimer();


        })
    }

    function getEmployeeByServiceProviderId(serviceProviderId, jobId) {
       
     $rootScope.showLoader = true;
     employeeService.getEmployeeByServiceProviderId(serviceProviderId, jobId).then(function (response) {

            $scope.availableEmployees = [];
            $scope.workingEmployees = [];
            angular.forEach(response, function (value, key) {


                if (value.EmployeeStatus == 0) {

                    $scope.availableEmployees.push(value);
                }
                else if (value.EmployeeStatus == 1) {

                    $scope.workingEmployees.push(value);
                }

            })
            // $scope.EmployeeList = response;
             startTimer();
        });
    }
    // function get job detail by jobId


    function getJobByJobId(JobId) {
     $rootScope.showLoader = true;
        jobService.getJobByJobId(JobId).then(function (response) {
            
            $scope.JobName = response.Description;
            $scope.ClientName = response.ClientName;
            $scope.ServiceProviderName = response.ServiceProvider.CompanyName;
            $scope.PostedDate = response.CreatedDate;
            $scope.StartDate = response.StartDate;
            $scope.EndDate = response.EndDate;
            $scope.ServiceName = response.ServiceName;
            $scope.JobAddress = response.JobAddress;
            $scope.CityName = response.CityName;
            if (response.Status == 0) {
                $scope.Status = "New";
            }
            else if (response.Status == 2) {
                $scope.Status = "Current";
            }
            else if (response.Status == 3) {
                $scope.Status = "Completed";
            }


            $scope.Quantity = response.JobServiceMapping.Quantity;
            $scope.Comments = response.JobServiceMapping.Comments;
            $scope.JobServicePictures = response.JobServiceMapping.JobServicePictureMappings;
            startTimer();

        })
    }

    $scope.UpdateJob = function (jobId) {
     
        jobService.updateJob(jobId).then(function (response) {


            var foundItem = $filter('filter')($scope.allNewJobs, { JobId: jobId }, true)[0];


            var index = $scope.allNewJobs.indexOf(foundItem);

            $scope.allNewJobs.splice(index, 1);
            $scope.allRejectedJobs.push(foundItem);
            $scope.alerts = [];
            $scope.alerts.push({ type: 'success', msg: " Job has been rejected successfully." });
        },
  function (error) {
      $scope.alerts = [];
      $scope.alerts.push({ type: 'warning', msg: "Job has not been rejected successfully." });

  });
  
    }

    $scope.itemsPerPage = pagingservice.pagesize;

    //paging
    //
    //$scope.currentPage = 1;
    //$scope.items = [];
    //$scope.range = function () {
    //    $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
    //    $scope.showPaging = '';
    //    if ($scope.itemsPerPage == "") {
    //        $scope.itemsPerPage = pagingservice.pagesize;
    //    }
    //    if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
    //        $scope.showPaging = true;
    //    }
    //    else {
    //        $scope.showPaging = false;
    //    }
    //    if (Math.ceil($scope.totalcount / $scope.itemsPerPage) <= 5) {
    //        var rangeSize = Math.ceil($scope.totalcount / $scope.itemsPerPage);
    //    }
    //    else {
    //        var rangeSize = 5;
    //    }
    //    var ret = [];

    //    var start;
    //    start = $scope.currentPage;
    //    if (start > $scope.pageCount() - rangeSize) {
    //        start = $scope.pageCount() - rangeSize + 1;
    //    }

    //    for (var i = start; i < start + rangeSize; i++) {
    //        ret.push(i);
    //    }

    //    return ret;
    //};

    //$scope.prevPage = function () {
    //    if ($scope.currentPage > 1) {
    //        $scope.currentPage--;
    //        $scope.paging();
    //    }
    //};

    //$scope.prevPageDisabled = function () {
    //    return $scope.currentPage === 1 ? "disabled" : "";
    //};

    //$scope.pageCount = function () {
    //    return Math.ceil($scope.totalcount / $scope.itemsPerPage);
    //};

    //$scope.nextPage = function () {

    //    if ($scope.currentPage < $scope.pageCount()) {
    //        $scope.currentPage++;
    //        $scope.paging();
    //    }
    //};

    //$scope.nextPageDisabled = function () {
    //    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    //};

    //$scope.setPage = function (n) {

    //    $scope.currentPage = n;
    //    $scope.paging();
    //};

    //$scope.firstPageDisabled = function () {
    //    return $scope.currentPage === 1 ? "disabled" : "";
    //};
    //$scope.lastPageDisabled = function () {
    //    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    //};
    //$scope.firstPage = function () {
    //    if ($scope.currentPage > 1) {
    //        $scope.currentPage = 1;
    //        $scope.paging();
    //    }
    //}
    //$scope.lastPage = function () {
    //    if ($scope.currentPage < $scope.pageCount()) {
    //        $scope.currentPage = $scope.pageCount();
    //        $scope.paging();
    //    }
    //}

    //$scope.searchRecord = function () {

    //    
    //    $scope.SearchText;
    //}



    //$scope.paging = function () {

    //    var pagingModel = {
    //        CurrentPageIndex: $scope.currentPage,
    //        Pagesize: $scope.itemsPerPage,
    //        SearchText: $scope.SearchText
    //    }
    //    jobService.paging(pagingModel).then(function (response) {

    //        if (response.data != null) {
    //            $scope.unSelect();
    //            $scope.SelectedJobs = response.data;
    //            $scope.totalcount = response.totalRecords;
    //            if ($scope.currentPage == 1) {
    //                $scope.itemCount = $scope.SelectedJobs.length;
    //                $scope.startRecord = 1;

    //            }
    //            else {

    //                if ($scope.SelectedJobs.length < $scope.itemsPerPage) {
    //                    $scope.itemCount = ($scope.itemsPerPage * $scope.currentPage) - $scope.itemsPerPage + $scope.SelectedJobs.length;
    //                    $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1;
    //                }
    //                else {
    //                    $scope.itemCount = ($scope.itemsPerPage * ($scope.currentPage));//$scope.itemCount + data.data.length;
    //                    $scope.startRecord = ($scope.itemsPerPage * ($scope.currentPage - 1)) + 1; //data.data.length + $scope.startRecord +1;
    //                }
    //            }

    //            $scope.totalpage = Math.ceil($scope.totalcount / $scope.itemsPerPage);
    //            if (Math.ceil($scope.totalcount / $scope.itemsPerPage) >= 2) {
    //                $scope.showPaging = true;
    //            }
    //            else {
    //                $scope.showPaging = false;
    //            }

    //        }
    //    },
    //            function (error) {
    //                $scope.errormessage = "Error happend ";
    //            }
    //            );
    //}


    //function for checked and refresh
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

    //function for selectaunselect checkbox
    $scope.selectUnselectAll = function () {

        if ($('input[name="SingleChk"]').length == $('input[name="SingleChk"]:checked').length) {

            $('input:checkbox[name="chkAll"]').attr("checked", "checked");
        }
        else {
            $('input:checkbox[name="chkAll"]').removeAttr("checked");
        }
        $scope.refreshIsMultipleDelete();
    }
    //end function

    //function for fill values for selected items
    $scope.fillselectedServiceProviders = function () {

        $scope.selectedServiceProviders = [];
        $('input[name=SingleChk]').each(function () {
            if (this.checked) {
                $scope.selectedServiceProviders.push($(this).val());
            }
        });

    }
    //end function

    //function for refresh if multiple select
    $scope.refreshIsMultipleDelete = function () {

        $scope.fillselectedServiceProviders();
        if ($scope.selectedServiceProviders.length > 0) {
            $scope.IsMultipleDelete = true;
        }
        else {
            $scope.IsMultipleDelete = false;
        }
    }

    $scope.unSelect = function () {

        $('input:checkbox[name="chkAll"]').removeAttr("checked");
    }

    //function for delete multiple
    $scope.deleteSelectedServiceProviders = function () {
     
        $scope.IsMultipleDelete = false;
        $scope.fillselectedServiceProviders();
        //   $('#deleteAllConfirmModal').modal('hide');
        var serviceProviders = $scope.selectedServiceProviders;
        serviceProviderService.deleteSelectedServiceProviders(serviceProviders).then(function (data) {

            if (data != null) {

                if ((data.totalRecords - (($scope.currentPage - 1) * $scope.itemsPerPage)) < 1) {
                    $scope.currentPage = $scope.currentPage - 1;
                }
                $scope.getAllServiceProviders();
                $scope.alerts = [];
                $scope.alerts.push({ type: 'success', msg: data.Message });
            }
        }, function (error) {

        });
         
    }
    //end function

    function getCity() {
     $rootScope.showLoader = true;
        cityService.getCity().then(function (response) {
            //  $state.go('dashboard');

            $scope.itemCity = [];
            angular.forEach(response, function (value, key) {

                this.push(value);
            }, $scope.itemCity)

        }, function (response) {
            $scope.errorMessage = response.error_description;
        });
         $rootScope.showLoader = false;
    };

    //daypilot functions
    $scope.events = [];
    var params = {
        Start: "",
        End: "",
        Description: "",
        EmployeeId: "",
        JobId: "",
    };
    $scope.showScheduler = false;
    $scope.dayConfig = {
        startDate: new Date(),
        viewType: "Day",
        MoveBy: "None",
        onEventMove: function (args) {

            var params = {
                Start: args.newStart.toString(),
                End: args.newEnd.toString(),
                Description: args.e.data.text,
                EmployeeId: $scope.employeeId,
                Id: args.e.data.id,
                JobId: jobId,
            };
            jobService.updatescheduleForEmployee(params).then(function (response) {

            },
            function (error) {
            })
        },
        eventDeleteHandling: "Update",
        onEventDelete: function (args) {
            if (!confirm("Are you sure you want to delete this event?")) {
                args.preventDefault();
            }
        },
        onEventDeleted: function (args) {
           
            var id = args.e.data.id;
            jobService.deleteEmployeeScheduleById(id).then(function (response) {
            },
        function (error) {


        });

        },
        onTimeRangeSelected: function (args) {
            $scope.weekConfig.startDate = args.day;
            $scope.dayConfig.startDate = args.day;
            var name = prompt("New event name:", $scope.JobName);
            //dp.clearSelection();
            if (!name) return;
            var e = new DayPilot.Event({
                start: args.start,
                end: args.end,
                id: DayPilot.guid(),
                resource: args.resource,
                text: name
            });
            addEvent(e);
        }
    };

    $scope.weekConfig = {
        visible: false,
        startDate: new Date(),
        viewType: "Week",
        MoveBy: "None",
        onEventMove: function (args) {

            var params = {
                Start: args.newStart.toString(),
                End: args.newEnd.toString(),
                Description: args.e.data.text,
                EmployeeId: $scope.employeeId,
                Id: args.e.data.id,
                JobId: jobId,
            };
            jobService.updatescheduleForEmployee(params).then(function (response) {

            },
            function (error) {
            })
        },
        eventDeleteHandling: "Update",
        onEventDelete: function (args) {
            if (!confirm("Are you sure you want to delete this event?")) {
                args.preventDefault();
            }
        },
        onEventDeleted: function (args) {
            
            var id = args.e.data.id;
            jobService.deleteEmployeeScheduleById(id).then(function (response) {
            },
        function (error) {


        });

        },
        onTimeRangeSelected: function (args) {


            $scope.weekConfig.startDate = args.day;
            $scope.dayConfig.startDate = args.day;
            var name = prompt("New event name:", $scope.JobName);
            // dp.clearSelection();
            if (!name) return;
            var e = new DayPilot.Event({
                start: args.start,
                end: args.end,
                id: DayPilot.guid(),
                resource: args.resource,
                text: name
            });
            addEvent(e);
        }
    };

    $scope.showDay = function () {
        $scope.dayConfig.visible = true;
        $scope.weekConfig.visible = false;
    };

    $scope.showWeek = function () {
        $scope.dayConfig.visible = false;
        $scope.weekConfig.visible = true;
    };

    function loadEvents(e) {
        $scope.events = [];
        $timeout(function () {
           
            var params = {
                start: e.start,
                end: e.end,
                employeeId: $scope.employeeId
            };

            jobService.getEmployeeScheduleByDateAndTime(params).then(function (response) {

                $scope.showScheduler = true;
                angular.forEach(response, function (value, key) {

                    $scope.events.push(
                      {
                          start: new DayPilot.Date($scope.formatDate(value.start)),
                          end: new DayPilot.Date($scope.formatDate(value.end)),
                          id: value.id,
                          text: value.text
                      }
            );
                })

            },
       function (error) {

       });
        });
    }

    $scope.navigatorConfig = {
        selectMode: "day",
        showMonths: 2,
        skipMonths: 2,
        onTimeRangeSelected: function (args) {
            
            $scope.weekConfig.startDate = args.day;
            $scope.dayConfig.startDate = args.day;
            loadEvents(args);
        }
    };
    $scope.Confirm = function (employeeId, jobId) {
       
        jobService.Confirm(employeeId, jobId).then(function (response) {
            $state.go('app.serviceprovider.jobs', { serviceProviderId: $scope.app.currentUser.userId })
           localStorageService.set('SuccessMsg', "");
           localStorageService.set('SuccessMsg', "Job has been assigned successfully.");

        },
        function (error) {


        });
    }
    $scope.Cancel = function (employeeId, jobId) {
        
        jobService.Cancel(employeeId, jobId).then(function (response) {
            $state.go('app.serviceprovider.jobs', { serviceProviderId: $scope.app.currentUser.userId }, { msg: "Job Canceled." })


        },
        function (error) {


        });
    }
    $scope.selectedEmployee = function (employeeId) {
        
        $scope.employeeId = employeeId;
        $scope.events = [];
        var params = {
            Start: $scope.StartDate,
            End: $scope.EndDate,
            Description: $scope.ServiceName,
            EmployeeId: employeeId,
            JobId: jobId,
        };
        addEvent(params);
        jobService.getEmployeeScheduleByEmployeeId(employeeId).then(function (response) {

            $scope.showScheduler = true;
            angular.forEach(response, function (value, key) {
                $scope.events.push(
                   {
                       start: new DayPilot.Date($scope.formatDate(value.start)),
                       end: new DayPilot.Date($scope.formatDate(value.end)),
                       id: value.id,
                       text: value.text
                   }
         );
            })
           

        },
        function (error) {


        });
    }

    $scope.formatDate = function (date) {

        var Date = date;
        var day = Date.split('/')[0];
        var month = Date.split('/')[1];
        var yeartime = Date.split('/')[2];
        var year = yeartime.split(' ')[0];
        var time = yeartime.substring(5, 13);
        var convertedDate = year + '-' + month + '-' + day + 'T' + time;
        return convertedDate;
    }

    function addEvent(params) {
        
        //var params = {
        //    Start: e.data.start.toString(),
        //    End: e.data.end.toString(),
        //    Description: e.data.text,
        //    EmployeeId: $scope.employeeId,
        //    JobId: jobId,
        //};
        jobService.addscheduleForEmployee(params).then(function (response) {
           
            $scope.events.push(
                    {
                        start: new DayPilot.Date(response.data.Start),
                        end: new DayPilot.Date(response.data.End),
                        id: response.data.Id,
                        text: response.data.Description
                    }
          );
        },
        function (error) {
        });
    }
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.clearNewJob = function () {
        
        this.SearchNewJobText = "";
         this.SearchCurrentJobText="";
    this.SearchCompletedJobText="";
    this.SearchRejectedJobText="";
    }
     var startTimer = function () {
         
         var timer = $timeout(function () {
             $timeout.cancel(timer);
             $rootScope.showLoader = false;
         }, 2000);
     }
}]);