using GeekQuiz.Model.BusinessLogic;
using GeekQuiz.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GeekQuiz.Service.Controllers
{   
    public class TriviaController : ApiController
    {        
        public HttpResponseMessage Get()
        {
            QuestionAnswer _QuestionAnswer = new QuestionAnswer();
            TriviaQuestion _TriviaQuestion = _QuestionAnswer.GetQuestion("myatthu1986@yahoo.com");
            return Request.CreateResponse(HttpStatusCode.OK, _TriviaQuestion);
        }

        public HttpResponseMessage Post(TriviaAnswer _TriviaAnswer)
        {
            QuestionAnswer _QuestionAnswer = new QuestionAnswer();
            _TriviaAnswer.UserId = "myatthu1986@yahoo.com";
            bool IsCorrect = _QuestionAnswer.SetAnswer(_TriviaAnswer);
            return Request.CreateResponse(HttpStatusCode.OK, IsCorrect);            
        }        
    }
}
