using Microsoft.AspNetCore.Components;

namespace BlazorTestProject.Components.Pages
{
    public enum MashingGameState
    {
        Starting,
        Ongoing,
        Ended
    }
    
    public class ClientBase : ComponentBase, IDisposable
    {
        protected static List<ClientBase> Clients = new List<ClientBase>();
        protected static int currentCount = 0;
        protected static System.Timers.Timer Secondtimer;
        protected static System.Timers.Timer Updatetimer;
        protected static int ElapsedSeconds;
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
        protected static int ClientsConnected { get { return Clients.Count(); } }
        public string ClientName { get { return _ClientName; } }
        public string CharacterPath { get { return $"Resources/Mashing/{PlayerDictionary[_PlayerNumber]}/" +
                                                 $"{AnimationDictionary[_AnimationFrame]}"; } }
        public string JumpHeight { get { return (-(int)(Math.Sin(_AnimationFrame - 1) * 30)).ToString()+"px"; } }
        public int ClientCount {  get { return _ClientCount; } }
        public static List<ClientBase> SortedClients { get { return Clients.OrderBy(o => -o.ClientCount).ToList(); } }
        
        static ClientBase()
        {
            Secondtimer = new System.Timers.Timer();
            Secondtimer.Interval = 1000;
            Secondtimer.Elapsed += Timer_Elapsed;
            Secondtimer.Start();
            Updatetimer = new System.Timers.Timer();
            Updatetimer.Interval = 20;
            Updatetimer.Elapsed += Updatetimer_Elapsed;
            Updatetimer.Start();
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

        private static void Updatetimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if(BlockAnimation != 24)
            {
                BlockAnimation++;
            }
            else
            {
                BlockAnimation = 6;
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
        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            ElapsedSeconds++;
            if (State == MashingGameState.Starting && ElapsedSeconds == 3)
            {
                State = MashingGameState.Ongoing;
            }
            if(State == MashingGameState.Ongoing && ElapsedSeconds == 13)
            {
                State = MashingGameState.Ended;
                for (int i = 0; i < Clients.Count; i++)
                {
                    Clients[i]._AnimationFrame = 0;
                }
                SortedClients[0]._AnimationFrame = 5;
            }                   
        }
        protected void Tick()
        {
            if(_AnimationFrame!= 0 && State == MashingGameState.Ongoing)
            {
                _AnimationFrame++;
                if(_AnimationFrame == 5 )
                {
                    _AnimationFrame = 0;
                }
            }
        }
        protected void StartGame()
        {
            foreach (ClientBase a in Clients)
            {
                a._ClientCount = 0;
            }
            State = MashingGameState.Starting;
            ElapsedSeconds = 0;
            Secondtimer.Stop();
            Secondtimer.Start();
            Update();
        }
        protected static void Update()
        {
            for(int i = 0; i < Clients.Count; i++)
            {
                Clients[i].Tick();
                Clients[i].InvokeAsync(Clients[i].StateHasChanged);
            }
        }
        void IDisposable.Dispose()
        {
            Clients.Remove(this);
            if(GameHost == this && Clients.Count >0)
            {
                GameHost = Clients[0];
            }
            if (Clients.Count == 0)
            {
                currentCount = 0;
                ElapsedSeconds = 0;
            }
            Update();
        }
    }
}
