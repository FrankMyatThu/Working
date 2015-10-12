var app = angular.module('AppRegistration', []);
var directiveId_ngMatch = 'ngMatch';
app.directive(directiveId_ngMatch, ['$parse', function ($parse) {

    var directive = {
        link: link,
        restrict: 'A',
        require: '?ngModel'
    };
    return directive;

    function link(scope, elem, attrs, ctrl) {
        // if ngModel is not defined, we don't need to do anything
        if (!ctrl) return;
        if (!attrs[directiveId_ngMatch]) return;

        var firstValue = $parse(attrs[directiveId_ngMatch]);

        var validator = function (value) {
            var temp = firstValue(scope),
            v = value === temp;
            ctrl.$setValidity('match', v);
            return value;
        }

        ctrl.$parsers.unshift(validator);
        ctrl.$formatters.push(validator);
        attrs.$observe(directiveId_ngMatch, function () {
            validator(ctrl.$viewValue);
        });

    }
}]);
app.controller('RegistrationController', function ($scope, $http, $window) {        
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
        $http({
            method: 'POST',
            url: 'https://localhost:44305/api/accounts/CreateUser',
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
