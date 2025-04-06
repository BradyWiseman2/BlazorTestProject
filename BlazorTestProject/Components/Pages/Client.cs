using Microsoft.AspNetCore.Components;
using BlazorAppDataLayer;
using BlazorAppDataLayer.Models;
using BlazorAppDataLayer.Repositories;
using BlazorTestProject.Components.Pages.PageBases;
namespace BlazorTestProject.Components.Pages
{
    public enum MashingGameState
    {
        Starting,
        Ongoing,
        Ended
    }
    
    public class ClientBase : SubGameBase, IDisposable
    {
        protected static List<ClientBase> Clients = new List<ClientBase>();
        protected static List<MainGameBase> mainGameBases = MainGameBase.UsersList;
        protected static int currentCount = 0;
        protected static System.Timers.Timer Secondtimer;
        protected static System.Timers.Timer Updatetimer;
        protected static int ElapsedSeconds;
        protected static int ElapsedMs;
        protected static int BlockAnimation;
        protected static string BlockPath { get { return $"Resources/Mashing/Other/Block{BlockAnimation/6}.png"; } }       
        protected static ClientBase GameHost { get; set; }
        protected static MashingGameState State { get; set; }
        protected static Dictionary<int, string> AnimationDictionary;
        protected static Dictionary<int, string> PlayerDictionary;
        protected int ClientNum;
        protected int _ClientCount;
        protected int _AnimationFrame;
        protected int _PlayerNumber;
        protected string _ClientName;
        public MainGameBase _User {  get; set; }
        protected static int ClientsConnected { get { return Clients.Count(); } }
        public string ClientName { get { if (_User != null) return _User.UserName; else return ""; } }
        public string CharacterPath { get { return $"Resources/Mashing/{PlayerDictionary[_PlayerNumber]}/" +
                                                 $"{AnimationDictionary[_AnimationFrame]}"; } }
        public string JumpHeight { get { return (-(int)(Math.Sin(_AnimationFrame - 1) * 15)).ToString()+"px"; } }
        public int ClientCount {  get { return _ClientCount; } }
        public static List<ClientBase> SortedClients { get { return Clients.OrderBy(o => -o.ClientCount).ToList(); } }
        
        static ClientBase()
        {
            //Secondtimer = new System.Timers.Timer();
            //Secondtimer.Interval = 1000;
            //Secondtimer.Elapsed += Timer_Elapsed;
            //Secondtimer.Start();
            //Updatetimer = new System.Timers.Timer();
            //Updatetimer.Interval = 20;
            //Updatetimer.Elapsed += Updatetimer_Elapsed;
            //Updatetimer.Start();
            State = MashingGameState.Ended;
            
            AnimationDictionary = new Dictionary<int, string>();
            AnimationDictionary.Add(0, "Idle.png");
            AnimationDictionary.Add(1, "Jump.png");
            AnimationDictionary.Add(2, "Jump.png");
            AnimationDictionary.Add(3, "Jump.png");
            AnimationDictionary.Add(4, "Jump.png");
            AnimationDictionary.Add(5, "Victory.png");

            PlayerDictionary = new Dictionary<int, string>();
            PlayerDictionary.Add(1, "Mario");
            PlayerDictionary.Add(2, "Luigi");
            PlayerDictionary.Add(3, "Toad");
            PlayerDictionary.Add(4, "Toadette");

            
        }
    
        public override void UpdateGame(int elapsedTime)
        {
            if (BlockAnimation != 24)
            {
                BlockAnimation++;
            }
            else
            {
                BlockAnimation = 6;
            }
            
        }
        public override void UpdateGameStatic(int elapsedTime)
        {
            ElapsedMs += elapsedTime;
            if (ElapsedMs >= 1000)
            {
                ElapsedSeconds++;
                ElapsedMs = 0;
                if (ElapsedSeconds % 10 == 0)
                {
                    GC.Collect(); //We are not engaging in good memory management practices
                }
                if (State == MashingGameState.Starting && ElapsedSeconds == 3)
                {
                    State = MashingGameState.Ongoing;
                }
                if (State == MashingGameState.Ongoing && ElapsedSeconds == 13)
                {
                    State = MashingGameState.Ended;
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        Clients[i]._AnimationFrame = 0;
                    }
                    SortedClients[0]._AnimationFrame = 5;
                }
            }
            if (_AnimationFrame != 0 && State == MashingGameState.Ongoing)
            {
                _AnimationFrame++;
                if (_AnimationFrame == 5)
                {
                    _AnimationFrame = 0;
                }
            }
            Update();
        }       
        public ClientBase()
        {
           
            if (Clients.Count == 0)
            {
                GameHost = this;
            }
            Clients.Add(this);
            ClientNum = Clients.Count;
            _ClientName = "Client " + ClientNum;
            _PlayerNumber = ((ClientNum-1) % 4)+1;
        }      
        protected void IncrementCount()
        {
            if (State == MashingGameState.Ongoing)                        
            {
                currentCount++;
                _ClientCount++;
                _AnimationFrame = 1;
            }
        }
        protected void StartGame()
        {
            foreach (ClientBase a in Clients)
            {
                a._ClientCount = 0;
                a._AnimationFrame = 0;
            }
            State = MashingGameState.Starting;
            ElapsedSeconds = 0;
            Update();
            
        }
        protected static void Update()
        {
            for(int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i]!= null)
                {
                    Clients[i].InvokeAsync(Clients[i].StateHasChanged);                   
                }                
            }
        }
        void IDisposable.Dispose()
        {
            Clients.Remove(this);
            if(GameHost == this && Clients.Count > 0)
            {
                GameHost = Clients[0];
            }
            if (Clients.Count == 0)
            {
                currentCount = 0;
                ElapsedSeconds = 0;
                State = MashingGameState.Ended;
            }
            Update();
        }
    }
}
