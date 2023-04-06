using UnityEngine;
using UnityEngine.SceneManagement;

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

        public void restartGame()
        {
            Destroy(GameObject.Find("GameManager"));
            Destroy(GameObject.Find("NetworkManager"));
            SceneManager.LoadScene(0);
        }
    }
}
