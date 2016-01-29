var app = angular.module('AppLogin', []);
app.directive('autoFocus', function ($timeout) {
    /// Chrome browser's autofill issue fixed.
    return {
        restrict: 'AC',
        link: function (_scope, _element) {
            $timeout(function () {                               
                if (_scope.UserName === undefined)                 
                {   
                    return;
                }
                if (_scope.UserName == "")
                {
                    return;
                }

                try {
                    _scope.form.$setValidity(true);
                } catch (e) { }                
            }, 500);
        }
    };
});
app.controller('LoginController', function ($scope, $http, $window, $timeout) {
    $scope.Register = function () { $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Registration); }
    $scope.login = function () {  
        $scope.dataLoading = true;
        $scope.person = {
            "Email": $scope.UserName,
            "Password": $scope.Password
        };        
        $http({
            method: 'POST',
            url: ApplicationConfig.Service_Domain.concat(ApplicationConfig.Service_Login),
            data: $scope.person,
            headers: {
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
                $window.sessionStorage.setItem("JWTToken", data);
                $scope.person = {};                
                $window.location.href = ApplicationConfig.Client_Domain.concat(ApplicationConfig.Client_Home);
            }
        }).error(function (data, status, headers, config) {
            var ErrorMessageValue = "";
            var ExceptionMessageValue = "";
            ErrorNotifier(data);
            function ErrorNotifier(data) {
                angular.forEach(data, function (value, key) {
                    console.log("data key = " + key + " value = " + value);
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
            if (status == "-1"){
                $scope.error = "Service unavailable.";
            }
            
            $scope.dataLoading = false;
        });
    };
});
