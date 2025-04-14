using Microsoft.AspNetCore.Components;
using BlazorAppDataLayer.Models;
namespace BlazorTestProject.Components.Pages.PageBases
{
    public class SubGameBase : ComponentBase
    {
        [Parameter]         
        public User GameUser { get; set; }
        public virtual void UpdateGame(int elapsedTime)
        {

        }
        public virtual void UpdateGameStatic(int elapsedTime)
        {

        }
        public virtual void SendResults()
        {

        }
    }
}
