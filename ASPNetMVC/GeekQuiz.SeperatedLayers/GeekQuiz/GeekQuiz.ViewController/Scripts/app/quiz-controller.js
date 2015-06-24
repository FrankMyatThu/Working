angular.module('QuizApp', [])
    .controller('QuizCtrl', function ($scope, $http) {
        $scope.answered = false;
        $scope.title = "loading question...";
        $scope.options = [];
        $scope.correctAnswer = false;
        $scope.working = false;

        $scope.answer = function () {           
            return $scope.correctAnswer ? 'correct' : 'incorrect';
        };

        $scope.nextQuestion = function () {
            $scope.working = true;
            $scope.answered = false;
            $scope.title = "loading question...";
            $scope.options = [];

            $http.get("/api/trivia").success(function (data, status, headers, config) {                
                $scope.options = data.TriviaOptions;
                $scope.title = data.Title;

                //angular.forEach(data.TriviaOptions, function (value, key) {
                //    alert("value = " + value + " : key = " + key);
                //});

                $scope.answered = false;
                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };

        $scope.sendAnswer = function (option) {
            $scope.working = true;
            $scope.answered = true;

            //angular.forEach(option, function (value, key) {
            //    alert("key = " + key + " : value = " + value);
            //});

            $http.post('/api/trivia', { 'questionId': option.QuestionId, 'optionId': option.Id }).success(function (data, status, headers, config) {
                //alert("data "+data);                
                $scope.correctAnswer = data;
                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };
    });