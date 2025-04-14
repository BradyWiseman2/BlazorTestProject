using Microsoft.AspNetCore.Components;
using BlazorAppDataLayer;
using BlazorAppDataLayer.Models;
using BlazorAppDataLayer.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Mono.TextTemplating;
namespace BlazorTestProject.Components.Pages.PageBases
{
    public enum GamesList
    {
        Trivia,
        Mashing,
        Wheel
    }
    public enum PlayerState
    {
        Safe,
        Unsafe,
        Dead
    }
    public class MainGameBase : ComponentBase, IDisposable
    {
        protected static GamesList ActiveGameCode;
        public SubGameBase ActiveGame;
        protected static System.Timers.Timer Updatetimer;
        public static List<MainGameBase> UsersList;
        protected static int GameResultsReceived;
        protected static int SafePlayers;
        protected static int UnsafePlayers;
        public bool GotTriviaResults = false;
        protected User ClientUser;
        protected PlayerState PlayerState;
        public string DisplayName { get { if (ClientUser != null) return ClientUser.Username; else return ""; } }
        public string UserName;
        protected static int RoundCounter;
        public static List<TriviaQuestion> TriviaQuestions;
        protected int CurrentScore;
        public string TextColor
        {
            get
            {
                if (PlayerState == PlayerState.Safe)
                {                   
                        return "Green";                                      
                }
                if (PlayerState == PlayerState.Unsafe)
                {
                    return "Red";
                }
                return "White";

            }
        }
        static MainGameBase()
        {
            TriviaQuestions = new List<TriviaQuestion>();
            UsersList = new List<MainGameBase>();         
            Updatetimer = new System.Timers.Timer();
            Updatetimer.Interval = 30;
            Updatetimer.Elapsed += Updatetimer_Elapsed;
            Updatetimer.Start();
            ActiveGameCode = GamesList.Trivia;
        }
        public MainGameBase()
        {
            UsersList.Add(this);
            
        }        
        protected static void AdvanceRound()
        {

        }
        protected static void GoToMinigame()
        {
            if (UnsafePlayers == 0) //If everyone was right, the minigame is skipped
            {
                AdvanceRound();
            }
            else
            {
                if (UnsafePlayers == 1)
                {
                    ActiveGameCode = GamesList.Wheel;
                    for (int i = 0; i < UsersList.Count; i++)
                    {
                        UsersList[i].ActiveGame.UpdateGame((int)Updatetimer.Interval);
                    }
                }
            }
        }

        private static void Updatetimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (UsersList.Count > 0)
            {
                if (UsersList[0].ActiveGame != null)
                {
                    UsersList[0].ActiveGame.UpdateGameStatic((int)Updatetimer.Interval);
                    for (int i = 0; i < UsersList.Count; i++)
                    {
                        UsersList[i].ActiveGame.UpdateGame((int)Updatetimer.Interval);
                    }
                }
            }            
            
            Update();
        }
        protected static void Update()
        {
            for (int i = 0; i < UsersList.Count; i++)
            {
                if (UsersList[i] != null)
                { 
                    UsersList[i].InvokeAsync(UsersList[i].StateHasChanged);                  
                }
            }
        }
        public static void ReceiveResults()
        {

        }
        public static void ReceiveTriviaResults(User user, bool result)
        {   
            MainGameBase a = UsersList.Find(c => c.ClientUser == user);
            if (!a.GotTriviaResults)
            {
                if (result)
                {
                    a.PlayerState = PlayerState.Safe;
                    SafePlayers++;
                }
                else
                {
                    a.PlayerState = PlayerState.Unsafe;
                    UnsafePlayers++;
                }
                GameResultsReceived++;
                a.GotTriviaResults = true;
                if (GameResultsReceived == UsersList.Count)
                {
                    GoToMinigame();
                }
            }         
            
        }
        void IDisposable.Dispose()
        {            
            UsersList.Remove(this);           
        }
        protected void FetchUser()
        {
            UserRepository a = new UserRepository();
            ClientUser = a.GetByASPID(UserName);
        }
        
    }
}
