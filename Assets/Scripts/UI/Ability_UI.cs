using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace BossArena.UI
{
    public class Ability_UI : UIPanelBase
    {


        public Image AutoIcon;
        public Image AbilityIcon;
        public Image UltIcon;

        private Player player;
        public AbilityBase BasicAttack;
        public AbilityBase BasicAbility;
        public AbilityBase UltimateAbility;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            StartCoroutine(InitAbilityBar());
        }

        IEnumerator InitAbilityBar()
        {
            if (player is null)
            {
                yield return StartCoroutine(WaitForLocalPlayer());
            }

            BasicAttack = player.GetComponent<Player>().BasicAttack;
            BasicAbility = player.GetComponent<Player>().BasicAbility;
            UltimateAbility = player.GetComponent<Player>().UltimateAbility;
        }
        IEnumerator WaitForLocalPlayer()
        {
            while (InGameRunner.Instance.LocalPlayer == null)
            {
                yield return null;
            }
             player = InGameRunner.Instance.LocalPlayer.GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            AutoIcon.fillAmount = (BasicAttack.onCoolDown.Value) ? BasicAttack.GetCoolDown() : 1;
            AbilityIcon.fillAmount = (BasicAbility.onCoolDown.Value) ? BasicAbility.GetCoolDown() : 1;
            UltIcon.fillAmount = (UltimateAbility.onCoolDown.Value) ? UltimateAbility.GetCoolDown() : 1;
        }
    }
}