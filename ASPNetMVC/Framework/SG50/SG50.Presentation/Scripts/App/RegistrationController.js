/*
var appname = angular.module('AppRegistration', []);
appname.controller('RegistrationController', ['$scope',
    function ($scope) {
        $scope.greeting = { text: 'Hello' };
    }
]);
*/



var app = angular.module('AppRegistration', []);
app.controller('RegistrationController', function ($scope, $http) {
    $scope.greeting = { text: 'Hello' };
    $scope.person = {};
    $scope.sendForm = function () {
        console.log("submit click.");
        //$http({
        //    method: 'POST',
        //    url: '/Account/SignUp',
        //    data: $scope.person,
        //    headers: {
        //        'RequestVerificationToken': $scope.antiForgeryToken
        //    }
        //}).success(function (data, status, headers, config) {
        //    $scope.message = '';
        //    if (data.success == false) {
        //        var str = '';
        //        for (var error in data.errors) {
        //            str += data.errors[error] + '\n';
        //        }
        //        $scope.message = str;
        //    }
        //    else {
        //        $scope.message = 'Saved Successfully';
        //        $scope.person = {};
        //    }
        //}).error(function (data, status, headers, config) {
        //    $scope.message = 'Unexpected Error';
        //});
    };
});
