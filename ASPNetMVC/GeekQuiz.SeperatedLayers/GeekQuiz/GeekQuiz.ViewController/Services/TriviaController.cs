using GeekQuiz.Model.BusinessLogic;
using GeekQuiz.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeekQuiz.ViewController.Services
{
    public class TriviaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            QuestionAnswer _QuestionAnswer = new QuestionAnswer();
            TriviaQuestion _TriviaQuestion = _QuestionAnswer.GetQuestion("myatthu1986@yahoo.com");            
            return Request.CreateResponse(HttpStatusCode.OK, _TriviaQuestion);
        }
    }
}
