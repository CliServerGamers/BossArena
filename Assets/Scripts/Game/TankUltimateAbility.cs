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
        [SerializeField]
        private GameObject ultimatePrefab;

        private BoxCollider2D PlayerCollider;
        //private bool ultimateActivated = false;
        // Use for checking elapsed time while ulted.
        //private float timeStart;

        private SpriteRenderer spriteRenderer;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            if (onCoolDown)
                return;
            onCoolDown = true;
            timeStart = Time.time;
            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("Ultimate Ability");
            //PlayerCollider = ultimatePrefab.transform.parent.transform.GetComponent<BoxCollider2D>();
            PlayerCollider.enabled = false;
            StartCoroutine(WaitForAbilityEnd());
        }

        IEnumerator WaitForAbilityEnd()
        {
            yield return new WaitForSeconds(5);
            PlayerCollider.enabled = true;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;
            PlayerCollider = parentPlayer.transform.GetComponent<BoxCollider2D>();
            spriteRenderer = parentPlayer.transform.GetComponent<SpriteRenderer>();
            //mainCamera=Camera.main;
        }

        protected override void Update()
        {
            //UnityEngine.Debug.Log("TIMESTART: " + timeStart);
            //UnityEngine.Debug.Log("UltimateActivated: " + onCoolDown + "\n" +
            //    "Current Time: " + Time.time + "\n" +
            //    "timeStart: " + timeStart);

            checkCooldown();



            //if (Input.GetKeyDown(KeyCode.E) && onCoolDown == false)
            //{
            //    //UnityEngine.Debug.Log("Ultimate Ability");
            //    ActivateAbility();
            //}
        }

        //public bool checkUltimateCooldown()
        //{
        //    if (Time.time - timeStart >= coolDownDelay)
        //    {
        //        // Enough time has passed, set ultimatedActivated as off.
        //        onCoolDown = false;
        //        //PlayerCollider.enabled = true;
        //        return true;
        //    } else
        //    {
        //        return false;
        //    }
        //}

    }
}
