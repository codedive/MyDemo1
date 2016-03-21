angular.module('app')
.directive('confirm', ['$modal',function ($modal) {
    return {
   
        priority: 100,
        restrict: 'A',
        link: {
            pre: function (scope, element, attrs) {
                
                var msg = attrs.confirm || "Are you sure?";
                
                element.bind('click', function (event) {
                    
                    
                    //event.stopImmediatePropagation();
                    //event.preventDefault();
                    //var modalinstance=$modal.open({
                    //templateUrl:'tpl/confirm.html',
                    //size:'md',
                    //controller:function($scope){
                    //    $scope.msg=msg;
                    //    $scope.cancel = function () {
                            
                    //         //event.stopImmediatePropagation();
                    //         //event.preventDefault();
                    //         $scope.dismissModel();
                    //         return false;
                    //    }
                    //    $scope.ok = function () {
                    //        if (event.isImmediatePropagationStopped()) {
                    //            event.cancelBubble = false;
                    //        }
                            
                    //        $scope.dismissModel();
                    //        return true;
                    //    }
                    //    $scope.dismissModel=function(){
                    //        modalinstance.close('dismiss');
                    //    }
                    //}
                    //});
                    if (!confirm(msg)) {
                        event.stopImmediatePropagation();
                        event.preventDefault;
                    }
                });
            }
        }
    };
}]);