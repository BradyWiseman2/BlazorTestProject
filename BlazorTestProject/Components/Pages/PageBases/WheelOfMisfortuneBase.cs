namespace BlazorTestProject.Components.Pages.PageBases
{
    public class WheelOfMisfortuneBase : SubGameBase
    {
        //protected static System.Timers.Timer Updatetimer;
        protected static int elapsedMS;
        protected static int WheelRotation;
        protected static string WheelRotationCSS { get { return WheelRotation.ToString() + "deg"; } }
        static WheelOfMisfortuneBase()
        {

        }

        private static void Updatetimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
          
        }

        public override void UpdateGame(int elapsedTime)
        {
            base.UpdateGame(elapsedTime);
        }
        public override void UpdateGameStatic(int elapsedTime)
        {
            elapsedMS += elapsedTime;
            WheelRotation += elapsedTime;
            if (WheelRotation > 360)
            {
                WheelRotation = 0;
            }
            if (elapsedMS > 1000)
            {                
                elapsedMS = 0;
            }
        }
        public override void SendResults()
        {
            base.SendResults();
        }
    }
}
