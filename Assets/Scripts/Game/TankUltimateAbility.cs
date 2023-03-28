using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game 
{
    /// <summary>
    /// This script must be a componenet of "TankPrefab".
    /// </summary>
    class TankUltimateAbility : TargetedAbilityBase
    {

        // Need to have reference to Parent Player Prefab
        [SerializeField]
        private GameObject PlayerPrefab;

        private BoxCollider2D PlayerCollider;
        private bool ultimateActivated = false;
        // Use for checking elapsed time while ulted.
        private float timeStart;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("Ultimate Ability");
            PlayerCollider = transform.GetComponent<BoxCollider2D>();
            if (PlayerCollider.enabled == true)
            {
                // Ult-ing
                ultimateActivated = true;
                timeStart = Time.time;
                PlayerCollider.enabled = false; 
            } else
            {
                PlayerCollider.enabled = true;
            }
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;
        }

        protected override void Update()
        {
            UnityEngine.Debug.Log("TIMESTART: " + timeStart);
            UnityEngine.Debug.Log("UltimateActivated: " + ultimateActivated + "\n" +
                "Current Time: " + Time.time + "\n" +
                "timeStart: " + timeStart);

            checkUltimateCooldown();

            if (Input.GetKeyDown(KeyCode.E) && ultimateActivated==false)
            {
                UnityEngine.Debug.Log("Ultimate Ability");
                ActivateAbility();
            }
        }

        public bool checkUltimateCooldown()
        {
            if (Time.time - timeStart >= coolDownDelay * 2)
            {
                // Enough time has passed, set ultimatedActivated as off.
                ultimateActivated = false;
                PlayerCollider.enabled = true;
                return true;
            } else
            {
                return false;
            }
        }

    }
}
