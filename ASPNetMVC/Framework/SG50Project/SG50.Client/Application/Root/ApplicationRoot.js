var app = angular.module('ApplicationRoot', ['ngAnimate', 'ui.bootstrap']);
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push(function ($q, $rootScope, $templateCache) {
        var AjaxLoadingCount = 0;
        return {
            request: function (config) {                
                if (++AjaxLoadingCount === 1){
                    if (!$templateCache.get(config.url)) {
                        $rootScope.$broadcast('AjaxLoading:Progress');
                    }
                }   
                return config || $q.when(config);
            },
            requestError: function (rejection) {                
                if (--AjaxLoadingCount === 0) $rootScope.$broadcast('AjaxLoading:Finish');
                return $q.reject(rejection);
            },
            response: function (response) {               
                if (--AjaxLoadingCount === 0) $rootScope.$broadcast('AjaxLoading:Finish');
                return response || $q.when(response);
            },
            responseError: function (rejection) {                
                if (--AjaxLoadingCount === 0) $rootScope.$broadcast('AjaxLoading:Finish');
                return $q.reject(rejection);
            }
        };
    });
}]);
app.directive("ajaxLoadingDirective", function ($uibModal) {
    var modalInstance;
    return {
        restrict: 'EA', //E = element, A = attribute, C = class, M = comment   			
        scope: true,
        link: function (scope, elem, attrs) {
            console.log("AjaxLoadingDirective.AjaxLoading:Progress");
            scope.$on("AjaxLoading:Progress", function () {
                modalInstance = $uibModal.open({
                    animation: true,                    
                    backdrop: 'static',
                    keyboard: false,
                    windowClass: 'app-modal-window-loading',
                    template: '<img id="imgLoadingForService" src="'+ ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_ServiceLoading) +'" />'
                });
            });
            return scope.$on("AjaxLoading:Finish", function () {
                console.log("AjaxLoadingDirective.AjaxLoading:Finish");
                if (modalInstance === undefined) return;
                modalInstance.dismiss();
            });
        }
    }
});
app.controller('ApplicationRootController', function ($scope, $http, $window, $timeout, $document) {
    $scope.error;

    if (!$window.sessionStorage.getItem("JWTToken")) {
        $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Login);
    }

    var TimeOutTimerValue = ApplicationConfig.MaximumAllowedIdelTime;

    console.log("TimeOutTimerValue", TimeOutTimerValue);

    // Start a timeout
    var TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    var bodyElement = angular.element($document);

    angular.forEach(['keydown', 'keyup', 'click', 'mousemove', 'DOMMouseScroll', 'mousewheel', 'mousedown', 'touchstart', 'touchmove', 'scroll', 'focus'],
    function (EventName) {
        bodyElement.bind(EventName, function (e) { TimeOut_Resetter(e) });
    });

    $scope.CountryByClick = function () {
        $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Country);
    }

    $scope.LogoutByClick = function () {
        RemoveActiveUser();
    }

    $scope.errorHandler = function (data, status, headers, config) {
        var ErrorMessageValue = "";
        var ExceptionMessageValue = "";
        ErrorNotifier(data);
        function ErrorNotifier(data) {
            angular.forEach(data, function (value, key) {
                console.log("key = " + key + " value = " + value);
                if (key == "ExceptionMessage") {
                    ExceptionMessageValue = value;
                }
                ErrorMessageValue = value;
                if (typeof value === 'object') {
                    ErrorNotifier(value);
                }
            });
        }
        if (ExceptionMessageValue != "") {
            $scope.error = ExceptionMessageValue;
        }
        else {
            $scope.error = ErrorMessageValue;
        }
        console.log("status " + status);
        if (status == "-1") {
            $scope.error = "Service unavailable.";
        }
        if (status == "401") {
            $scope.error = "Unauthorized.";
        }    
    }

    function LogoutByTimer() {        
        if ($window.sessionStorage.getItem("JWTToken") != "")
            RemoveActiveUser();
    }

    function TimeOut_Resetter(e) {
        console.log('' + e);

        /// Stop the pending timeout
        $timeout.cancel(TimeOut_Thread);

        /// Reset the timeout
        TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    }

    function RemoveActiveUser() {        
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Logout),
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': ApplicationConfig.AntiForgeryTokenKey
            }
        }).success(function (data, status, headers, config) {
            console.log('Logout Successfully');
            //$window.sessionStorage.setItem("JWTToken", "");
            sessionStorage.clear();
            $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Login);
        }).error(function (data, status, headers, config) {
            $scope.errorHandler(data, status, headers, config);
            $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Login);
        });
    }
});