using BossArena.game;
using BossArena.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        //[SerializeField]
        //private Button OKButton;
        //public void LeaveArena()
        //{
        //    GameManager.Instance.UIChangeMenuState(GameState.JoinMenu);
        //}

        [SerializeField]
        private List<AudioClip> audioClips;

        [SerializeField]
        private AudioSource audioSrc;


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
                PlaySound("shortplayerdeath", 20.0f, 1.0f);
                DisplayModal("Game Over", "You Died!");
               
            }
        }


        public void PlaySound(string clipName, float volume, float pitch)
        {
            AudioClip clip = audioClips.Find(sound => sound.name == clipName);
            if (clip != null)
            {
                audioSrc.clip = clip;
                audioSrc.Play();
                audioSrc.volume = volume;
                audioSrc.pitch = pitch;
                return;
            }
            throw new KeyNotFoundException("Sound with the following name " + clipName + "Does not exist!");
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

