var app = angular.module('AppRegistration', []);
app.controller('RegistrationController', function ($scope, $http, $window) {
    $scope.greeting = { text: 'Hello' };
    $scope.person = {
        "FirstName": "Myat",
        "LastName": "Thu",
        "Email": "myat@medialink.com.sg",
        "UserName": "myat",
        "RoleName": "Admin",
        "Password": "Ent3rP@ss",
        "ConfirmPassword": "Ent3rP@ss",
        "JoinDate": "2015/09/29"
    };
    $scope.sendForm = function () {
        console.log("$scope.person\n " + JSON.stringify($scope.person));
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
            $scope.message = 'Unexpected Error';
        });
    };
});
