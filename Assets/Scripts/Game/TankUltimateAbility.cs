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
        private int UltimateNewHealth;
        
        [SerializeField]
        private GameObject ultimatePrefab;

        private BoxCollider2D PlayerCollider;
        //private bool ultimateActivated = false;
        // Use for checking elapsed time while ulted.
        //private float timeStart;

        private SpriteRenderer spriteRenderer;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            //ultimateActivated = true;
            if (onCoolDown.Value)
                return;
            onCoolDown.Value = true;
            timeStart = Time.time;

            // Start rendering the ability
            spriteRenderer.enabled = true;

            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("Ultimate Ability");
            //PlayerCollider = ultimatePrefab.transform.parent.transform.GetComponent<BoxCollider2D>();
            PlayerCollider.enabled = false;

            // Set Player Health to super low
            if(parentPlayer.GetComponent<EntityBase>().CurrentHealth.Value >= UltimateNewHealth)
            {
                parentPlayer.GetComponent<EntityBase>().CurrentHealth.Value = UltimateNewHealth;
            }
            UnityEngine.Debug.Log($"Setting Player Health to {parentPlayer.GetComponent<EntityBase>().CurrentHealth}");

            StartCoroutine(WaitForAbilityEnd());
        }

        IEnumerator WaitForAbilityEnd()
        {
            yield return new WaitForSeconds(5);
            PlayerCollider.enabled = true;

            // Stop drawing the Ultimate Ability
            spriteRenderer.enabled = false;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;
            PlayerCollider = parentPlayer.transform.GetComponent<BoxCollider2D>();
            spriteRenderer = ultimatePrefab.transform.GetComponent<SpriteRenderer>();

            // Initally false 
            spriteRenderer.enabled = false;

            // Set scale of AbilityPrefab.
            ultimatePrefab.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            // Negative Z-Axis Value, "Go closer towards Camera"
            ultimatePrefab.transform.position = new Vector3(ultimatePrefab.transform.position.x, ultimatePrefab.transform.position.y, -2f);
            //mainCamera=Camera.main;
        }

        protected override void Update()
        {
            //UnityEngine.Debug.Log("TIMESTART: " + timeStart);
            //UnityEngine.Debug.Log("UltimateActivated: " + onCoolDown + "\n" +
            //    "Current Time: " + Time.time + "\n" +
            //    "timeStart: " + timeStart);

            checkCooldown();

            //float al = spriteRenderer.color.a;
            //al -= 0.001f;
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, al);


            //if (Input.GetKeyDown(KeyCode.E) && onCoolDown == false)
            //{
            //    //UnityEngine.Debug.Log("Ultimate Ability");
            //    ActivateAbility();
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

        public float calcElapsedTime()
        {
            return Time.time - timeStart;
        }
    }
}
