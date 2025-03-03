using Microsoft.AspNetCore.Components;

namespace BlazorTestProject.Components.Pages
{
    public class ClientBase : ComponentBase, IDisposable
    {
        protected static List<ClientBase> Clients = new List<ClientBase>();
        protected static int currentCount = 0;
        protected static System.Timers.Timer timer;
        protected static int ElapsedSeconds;
        protected int ClientNum;
        protected int _ClientCount;
        protected string _ClientName;       
        protected static int ClientsConnected { get { return Clients.Count(); } }
        public string ClientName { get { return _ClientName; } }
        public int ClientCount {  get { return _ClientCount; } }
        static ClientBase()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();            
        }
        public ClientBase()
        {
            Clients.Add(this);
            ClientNum = Clients.Count;
            _ClientName = "Client " + ClientNum;
        }
        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            ElapsedSeconds++;
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
            if (Clients.Count == 0)
            {
                currentCount = 0;
                ElapsedSeconds = 0;
            }
            Update();
        }
    }
}
