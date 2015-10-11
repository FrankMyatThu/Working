var app = angular.module('AppLogin', []);
app.controller('LoginController', function ($scope, $http, $window) {    
    $scope.greeting = { text: 'Hello' };    
    $scope.login = function () {  
        $scope.dataLoading = true;
        $scope.person = {
            "UserName": $scope.UserName,
            "Password": $scope.Password
        };
        console.log("$scope.person\n " + JSON.stringify($scope.person) + "\n $scope.antiForgeryToken " + $scope.antiForgeryToken);
        $http({
            method: 'POST',
            url: 'https://localhost:44305/api/accounts/UserLogin',            
            data: $scope.person,
            headers: {
                'RequestVerificationToken': $scope.antiForgeryToken
            }
        }).success(function (data, status, headers, config) {
            $scope.message = '';
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                $scope.message = str;
            }
            else {                
                $window.sessionStorage.setItem("JWTToken", data);
                $scope.person = {};                
                $window.location.href = '../Home/Home';                
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
                $scope.error = ExceptionMessageValue;
            }
            else {
                $scope.error = ErrorMessageValue;
            }            
            $scope.dataLoading = false;
        });
    };
});
