using BlazorAppDataLayer.Models;
using BlazorAppDataLayer.Repositories;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorTestProject.Components.Pages.PageBases
{
    public enum TriviaGameState
    {
        Title,
        ReadingQuestion,
        AnsweringQuestion,
        Results
    }
    public class TriviaGameBase : SubGameBase
    {
        public static TriviaGameState State { get; set; }
        protected static List<TriviaQuestion> TriviaQuestions {  get; set; }
        protected static TriviaQuestion ActiveTriviaQuestion { get; set; }
        protected static Random rnd = new Random();
        protected static int elapsedMS { get; set; }
        protected static int CorrectIndex { get; set; }
        protected static List<string> QuestionText { get; set; }
        protected int SelectedIndex { get; set; }
        protected static int SentResults { get; set; }
        
        public static void Shuffle<T>(IList<T> list) //"Borrowed" from stack overflow
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;               
                int k = rnd.Next(n + 1);               
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
                if (k == CorrectIndex)
                {
                    CorrectIndex = n;
                }
                else if(n == CorrectIndex)//These two statements ensure that if the correct answer is one that got shuffled, that the correct index changes with it
                {
                    CorrectIndex = k;
                }
            }
        }
        protected static void StartGame()
        {
            QuestionText = new List<string>();
            TriviaQuestions = MainGameBase.TriviaQuestions;
            TriviaQuestionRepository a = new TriviaQuestionRepository();
            TriviaQuestions = a.GetAll().ToList();
            State = TriviaGameState.ReadingQuestion;
            GetNewQuestion();
        }
        protected static void GetNewQuestion()
        {
            if(TriviaQuestions.Count == 0) //Well uhh, that shouldn't happen if I put enough work into making these, but we need a failsafe just in case.
            {
                TriviaQuestionRepository a = new TriviaQuestionRepository();
                TriviaQuestions = a.GetAll().ToList();
            }
            ActiveTriviaQuestion = TriviaQuestions[rnd.Next(0, TriviaQuestions.Count)];
            TriviaQuestions.Remove(ActiveTriviaQuestion);
            QuestionText.Clear();
            QuestionText.Add(ActiveTriviaQuestion.CorrectAnswer);
            QuestionText.Add(ActiveTriviaQuestion.WrongAnswer1);
            QuestionText.Add(ActiveTriviaQuestion.WrongAnswer2);
            QuestionText.Add(ActiveTriviaQuestion.WrongAnswer3);
            CorrectIndex = 0;
            Shuffle(QuestionText);
            State = TriviaGameState.ReadingQuestion;
            elapsedMS = 0;
        }
        public override void UpdateGameStatic(int elapsedTime)
        {
            elapsedMS += elapsedTime;
            switch (State)
            {
                case TriviaGameState.ReadingQuestion:
                    if(elapsedMS > 5000)
                    {
                        elapsedMS = 0;
                        State = TriviaGameState.AnsweringQuestion;
                    }
                    break;
                case TriviaGameState.AnsweringQuestion:
                    if (elapsedMS > 10000)
                    {
                        State = TriviaGameState.Results;
                       
                    }
                    break;
            }
        }
        public override void UpdateGame(int elapsedTime)
        {
            if (elapsedMS > 10000 && (State == TriviaGameState.AnsweringQuestion || State == TriviaGameState.Results) && SentResults < MainGameBase.UsersList.Count)
            {
                SendResults();
                SentResults++;               
            }
        }
        public override void SendResults()
        {          
            MainGameBase.ReceiveTriviaResults(GameUser, SelectedIndex == CorrectIndex);
            SelectedIndex = -1;            
        }

        protected void SetTriviaAnswer(int index)
        {
            SelectedIndex = index;
        }
    }
}
