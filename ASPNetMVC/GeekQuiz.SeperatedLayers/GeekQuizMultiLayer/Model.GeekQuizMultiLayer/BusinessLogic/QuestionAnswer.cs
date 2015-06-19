using Model.GeekQuizMultiLayer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Model.GeekQuizMultiLayer.BusinessLogic
{
    public class QuestionAnswer
    {
        public TriviaQuestion GetQuestion(string UserEmail)
        {
            TriviaQuestion _TriviaQuestion = null;
            using (Entities _DB = new Entities())
            {
                var lastQuestionId = _DB.TriviaAnswers
                .Where(a => a.UserId == UserEmail)
                .GroupBy(a => a.QuestionId)
                .Select(g => new { QuestionId = g.Key, Count = g.Count() })
                .OrderByDescending(q => new { q.Count, QuestionId = q.QuestionId })
                .Select(q => q.QuestionId)
                .FirstOrDefault();

                var questionsCount = _DB.TriviaQuestions.Count();

                var nextQuestionId = (lastQuestionId % questionsCount) + 1;
                _TriviaQuestion = _DB.TriviaQuestions
                                   .Include(x => x.TriviaOptions)
                                   .Where(x => x.Id.Equals(nextQuestionId))
                                   .Select(x => x).FirstOrDefault();
            }
            return _TriviaQuestion;
        }

        public bool SetAnswer(TriviaAnswer _TriviaAnswer)
        {
            bool IsCorrect = false;
            using(Entities _DB = new Entities())
            {
                _DB.TriviaAnswers.Add(_TriviaAnswer);

                _DB.SaveChangesAsync();
                var selectedOption = _DB.TriviaOptions.FirstOrDefault(o => o.Id == _TriviaAnswer.OptionId
                    && o.QuestionId == _TriviaAnswer.QuestionId);

                IsCorrect =  selectedOption.IsCorrect;
            }
            return IsCorrect;
        }
    }
}
