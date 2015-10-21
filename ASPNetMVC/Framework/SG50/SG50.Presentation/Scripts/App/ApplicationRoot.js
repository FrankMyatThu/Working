﻿var app = angular.module('ApplicationRoot', []);
app.controller('ApplicationRootController', function ($scope, $http, $window, $timeout, $document) {

    if (!$window.sessionStorage.getItem("JWTToken")) {
        $window.location.href = '../Account/Login';
    }

    /// Global variables
    // ....

    // Timeout timer value
    //var TimeOutTimerValue = 5000; // 5 Seconds
    //var TimeOutTimerValue = 6000; // 6 Seconds
    var TimeOutTimerValue = 100000; // 10 Minutes    

    // Start a timeout
    var TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    var bodyElement = angular.element($document);

    angular.forEach(['keydown', 'keyup', 'click', 'mousemove', 'DOMMouseScroll', 'mousewheel', 'mousedown', 'touchstart', 'touchmove', 'scroll', 'focus'],
    function (EventName) {
        bodyElement.bind(EventName, function (e) { TimeOut_Resetter(e) });
    });

    $scope.LogoutByClick = function () {
        RemoveActiveUser();
    }

    function LogoutByTimer() {
        console.log('Logout');
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
            url: 'https://localhost:44305/api/accounts/UserLogout',
            headers: {
                'accept': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + $window.sessionStorage.getItem("JWTToken"),
                'RequestVerificationToken': $scope.antiForgeryToken
            }
        }).success(function (data, status, headers, config) {
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                console.log(str);
            }
            else {
                console.log('Logout Successfully');
                $window.sessionStorage.setItem("JWTToken", "");
                $window.location.href = '../Account/Login';
            }
        }).error(function (data, status, headers, config) {
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
                console.log(ExceptionMessageValue);
            }
            else {                
                console.log(ErrorMessageValue);
            }
        });
    }
});


