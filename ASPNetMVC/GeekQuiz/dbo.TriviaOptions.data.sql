select TriviaQuestions.*,TriviaOptions.Title, TriviaOptions.IsCorrect  from TriviaQuestions
Inner join TriviaOptions On TriviaOptions.QuestionId = TriviaQuestions.Id
where TriviaOptions.IsCorrect = 'True'

