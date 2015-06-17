using GeekQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace GeekQuiz.Controllers
{
    [Authorize]
    public class TriviaController : ApiController
    {
        private TriviaContext db = new TriviaContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Get
        // GET api/Trivia
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> Get()
        {
            var userId = User.Identity.Name;

            //TriviaQuestion nextQuestion = await this.NextQuestionAsync(userId);
            TriviaQuestion nextQuestion = this.NextQuestion(userId);

            if (nextQuestion == null)
            {
                return this.NotFound();
            }

            return this.Ok(nextQuestion);
        }
        private async Task<TriviaQuestion> NextQuestionAsync(string userId)
        {
            var lastQuestionId = await this.db.TriviaAnswers
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.QuestionId)
                .Select(g => new { QuestionId = g.Key, Count = g.Count() })
                .OrderByDescending(q => new { q.Count, QuestionId = q.QuestionId })
                .Select(q => q.QuestionId)
                .FirstOrDefaultAsync();

            var questionsCount = await this.db.TriviaQuestions.CountAsync();

            var nextQuestionId = (lastQuestionId % questionsCount) + 1;
            return await this.db.TriviaQuestions.FindAsync(CancellationToken.None, nextQuestionId);            
        }

        private TriviaQuestion NextQuestion(string userId)
        {
            var lastQuestionId = this.db.TriviaAnswers
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.QuestionId)
                .Select(g => new { QuestionId = g.Key, Count = g.Count() })
                .OrderByDescending(q => new { q.Count, QuestionId = q.QuestionId })
                .Select(q => q.QuestionId)
                .FirstOrDefault();

            var questionsCount = this.db.TriviaQuestions.Count();

            var nextQuestionId = (lastQuestionId % questionsCount) + 1; 
            return this.db.TriviaQuestions
                           .Include(x => x.Options)
                           .Where(x => x.Id.Equals(nextQuestionId))
                           .Select(x => x).FirstOrDefault();
        }

        #endregion

        #region Post
        // POST api/Trivia
        [ResponseType(typeof(TriviaAnswer))]
        public async Task<IHttpActionResult> Post(TriviaAnswer answer)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            answer.UserId = User.Identity.Name;

            var isCorrect = await this.StoreAsync(answer);
            return this.Ok<bool>(isCorrect);
        }
        private async Task<bool> StoreAsync(TriviaAnswer answer)
        {
            /// Get corrected answer together with question.
            /// select TriviaQuestions.*,TriviaOptions.Title, TriviaOptions.IsCorrect  from TriviaQuestions
            /// Inner join TriviaOptions On TriviaOptions.QuestionId = TriviaQuestions.Id
            /// where TriviaOptions.IsCorrect = 'True'

            this.db.TriviaAnswers.Add(answer);

            await this.db.SaveChangesAsync();
            var selectedOption = await this.db.TriviaOptions.FirstOrDefaultAsync(o => o.Id == answer.OptionId
                && o.QuestionId == answer.QuestionId);

            return selectedOption.IsCorrect;
        }
        #endregion

    }
}
