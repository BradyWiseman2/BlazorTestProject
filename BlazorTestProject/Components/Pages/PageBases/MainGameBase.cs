using Microsoft.AspNetCore.Components;
using BlazorAppDataLayer;
using BlazorAppDataLayer.Models;
using BlazorAppDataLayer.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
namespace BlazorTestProject.Components.Pages.PageBases
{
    public enum GamesList
    {
        Trivia,
        Mashing
    }
    public class MainGameBase : ComponentBase, IDisposable
    {
        protected static GamesList ActiveGameCode;
        public SubGameBase ActiveGame;
        protected static System.Timers.Timer Updatetimer;
        public static List<MainGameBase> UsersList;
        protected User ClientUser;
        public string UserName;
        protected static int testint;
        static MainGameBase()
        {
            UsersList = new List<MainGameBase>();
            Updatetimer = new System.Timers.Timer();
            Updatetimer.Interval = 30;
            Updatetimer.Elapsed += Updatetimer_Elapsed;
            Updatetimer.Start();
        }
        public MainGameBase()
        {
            UsersList.Add(this);
            ActiveGameCode = GamesList.Mashing;
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
            testint += (int)Updatetimer.Interval;
            if(testint > 5000)
            {
                testint = 0;                              
                ActiveGameCode = GamesList.Trivia;
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
