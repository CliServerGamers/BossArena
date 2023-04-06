using BossArena.game;
using BossArena.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BossArena.UI
{
    public class ModalManager : UIPanelBase
    {
        [SerializeField]
        private EntityBase entity;

        [SerializeField]
        private TextMeshProUGUI Error;

        [SerializeField]
        private TextMeshProUGUI ErrorMessage;

        public void DisplayModal(string message1, string message2)
        {
            Debug.Log("something happened");
            Show();
            Error.text = message1;
            ErrorMessage.text = message2;
        }

        public void IsPlayerDeath(float old, float newhealth)
        {
            if (newhealth <= 0)
            {
                Debug.Log("Player died");
                DisplayModal("Game Over", "You Died!");
            }
        }


        //// Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            Debug.Log("OnStart");
            StartCoroutine(InitModal());
            Hide();
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}

        IEnumerator InitModal()
        {

            if (entity is null)
            {
                yield return StartCoroutine(WaitForLocalPlayer());
            }
            InGameRunner.Instance.LocalPlayer.GetComponent<Player>().CurrentHealth.OnValueChanged += IsPlayerDeath;
        }
        IEnumerator WaitForLocalPlayer()
        {
            Debug.Log($"Waiting");
            while (InGameRunner.Instance.LocalPlayer == null)
            {
                Debug.Log($"YIELD");
                yield return null;
            }
            Debug.Log($"Player null is : {InGameRunner.Instance.LocalPlayer == null}");
            entity = InGameRunner.Instance.LocalPlayer.GetComponent<EntityBase>();
        }
    }

}

