var app = angular.module('AppRegistration', []);
app.controller('RegistrationController', function ($scope, $http, $window) {
    $scope.greeting = { text: 'Hello' };    
    $scope.register = function () {
        $scope.dataLoading = true;
        $scope.person = {
            "FirstName": $scope.FirstName,
            "LastName": $scope.LastName,
            "Email": $scope.Email,
            "UserName": $scope.UserName,
            "RoleName": $scope.RoleName,
            "Password": $scope.Password,
            "ConfirmPassword": $scope.ConfirmPassword,
            "JoinDate": $scope.JoinDate
        };
        console.log("$scope.person\n " + JSON.stringify($scope.person) + "\n $scope.antiForgeryToken " + $scope.antiForgeryToken);
        $http({
            method: 'POST',
            url: 'https://localhost:44305/api/accounts/CreateUser',
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
                $scope.person = {};
                console.log("Saved Successfully : " + data);
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
                $scope.error = ExceptionMessageValue;
            }
            else {
                $scope.error = ErrorMessageValue;
            }
            $scope.dataLoading = false;
        });
    };
});
