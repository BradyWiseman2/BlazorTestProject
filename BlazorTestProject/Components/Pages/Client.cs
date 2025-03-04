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
        protected static System.Timers.Timer timer;
        protected static int ElapsedSeconds;
        protected static ClientBase Host { get; set; }
        protected static MashingGameState State { get; set; }
        protected int ClientNum;
        protected int _ClientCount;
        protected string _ClientName;       
        protected static int ClientsConnected { get { return Clients.Count(); } }
        public string ClientName { get { return _ClientName; } }
        public int ClientCount {  get { return _ClientCount; } }
        public List<ClientBase> SortedClients { get { return Clients.OrderBy(o => -o.ClientCount).ToList(); } }
        static ClientBase()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            State = MashingGameState.Ended;
        }
        public ClientBase()
        {
            if(Clients.Count == 0)
            {
                Host = this;
            }
            Clients.Add(this);
            ClientNum = Clients.Count;
            _ClientName = "Client " + ClientNum;
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
            }
            Update();          
        }
        protected void StartGame()
        {
            foreach (ClientBase a in Clients)
            {
                a._ClientCount = 0;

            }
            State = MashingGameState.Starting;
            ElapsedSeconds = 0;
            timer.Stop();
            timer.Start();
            Update();
        }
        protected static void Update()
        {
            foreach (ClientBase a in Clients)
            {              
                a.InvokeAsync(a.StateHasChanged);
            }
        }
        void IDisposable.Dispose()
        {
            Clients.Remove(this);
            if(Host == this && Clients.Count >0)
            {
                Host = Clients[0];
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
