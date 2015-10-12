var app = angular.module('ApplicationRoot', []);
app.controller('ApplicationRootController', function ($scope, $http, $window, $timeout, $document) {
    console.log('starting run');

    /// Global variables
    // ....

    // Timeout timer value
    //var TimeOutTimerValue = 5000;
    //var TimeOutTimerValue = 6000;
    var TimeOutTimerValue = 50000;    

    // Start a timeout
    var TimeOut_Thread = $timeout(function () { LogoutByTimer() }, TimeOutTimerValue);
    var bodyElement = angular.element($document);

    /// Keyboard Events
    bodyElement.bind('keydown', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('keyup', function (e) { TimeOut_Resetter(e) });

    /// Mouse Events    
    bodyElement.bind('click', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousemove', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('DOMMouseScroll', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousewheel', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('mousedown', function (e) { TimeOut_Resetter(e) });

    /// Touch Events
    bodyElement.bind('touchstart', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('touchmove', function (e) { TimeOut_Resetter(e) });

    /// Common Events
    bodyElement.bind('scroll', function (e) { TimeOut_Resetter(e) });
    bodyElement.bind('focus', function (e) { TimeOut_Resetter(e) });

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
        console.log("$window.sessionStorage.getItem(\"JWTToken\") " + $window.sessionStorage.getItem("JWTToken"));
        console.log("$scope.antiForgeryToken " + $scope.antiForgeryToken);

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
            }
            else {
                console.log('Logout Successfully');
                $window.sessionStorage.setItem("JWTToken", "");
                $window.location.href = '../Account/Login';
            }
        }).error(function (data, status, headers, config) {
            console.log("data :" + data);
            console.log("status :" + status);
            console.log("headers :" + headers);
            console.log("config :" + config);
        });
    }
});


