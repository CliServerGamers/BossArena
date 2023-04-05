using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace BossArena.game
{
    public class Ability_UI : MonoBehaviour
    {

        private GameObject playerPrefab;
        public AbilityBase BasicAttack;
        public AbilityBase BasicAbility;
        public AbilityBase UltimateAbility;

        // Start is called before the first frame update
        void Start()
        {
            playerPrefab = GameObject.FindWithTag("Player");
            BasicAbility = playerPrefab.GetComponent<Player>().BasicAbility;


        }

        // Update is called once per frame
        void Update()
        { 
            transform.parent.GetComponent<Button>().interactable = BasicAbility.onCoolDown.Value;
        }
    }
}