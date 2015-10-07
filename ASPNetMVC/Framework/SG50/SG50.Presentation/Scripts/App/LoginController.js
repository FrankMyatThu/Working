var app = angular.module('AppLogin', []);
app.controller('LoginController', function ($scope, $http, $window) {    
    $scope.greeting = { text: 'Hello' };
    $scope.person = {
        "UserName": "myat",        
        "Password": "Ent3rP@ss"
    };
    $scope.sendForm = function () {
        console.log("$scope.person\n " + JSON.stringify($scope.person));
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
                $scope.message = 'Login Successfully';
                $window.sessionStorage.setItem("JWTToken", data);
                $scope.person = {};                
                $window.location.href = '../Home/Home';                
            }
        }).error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error';
        });
    };
});
