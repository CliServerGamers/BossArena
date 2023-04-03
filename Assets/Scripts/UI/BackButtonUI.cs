using UnityEngine;

namespace BossArena.UI
{
    /// <summary>
    /// For navigating the main menu.
    /// </summary>
    public class BackButtonUI : UIPanelBase
    {

        public void ToJoinMenu()
        {
            Manager.UIChangeMenuState(GameState.JoinMenu);
        }

        public void ToMenu()
        {
            Manager.UIChangeMenuState(GameState.Menu);
        }
    }
}
