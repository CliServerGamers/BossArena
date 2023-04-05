using BossArena;
using BossArena.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;



public class MenuMainUI : UIPanelBase
{
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void ToJoinMenu()
    {
        Manager.UIChangeMenuState(GameState.JoinMenu);
    }

    public void ToOptionMenu()
    {
        //Manager.UIChangeMenuState(GameState.OptionMenu);
    }
    }
