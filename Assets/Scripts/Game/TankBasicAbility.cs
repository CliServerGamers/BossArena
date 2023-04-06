using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class TankBasicAbility : TargetedAbilityBase, IDrawIndicator
    {

        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        [SerializeField]
        private int TauntIncreaseAmount;

        // Need to have reference to Taunt Prefab
        [SerializeField]
        private GameObject TauntPrefab;

        // Need to know how long this ability lasts for
        [SerializeField]
        private float lengthOfAttackInSec;

        [SerializeField]
        private float TauntDuration;

        private CircleCollider2D TauntPrefabCollider;
        private SpriteRenderer TauntPrefabSpriteRenderer;

        private bool basicActivated = false;
        private bool withinTauntRange = false;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            TauntPrefabSpriteRenderer.enabled = false;
            //TauntPrefabCollider.enabled = true;
            if (onCoolDown.Value)
                return;
            onCoolDown.Value = true;
            timeStart = Time.time;

            // Apply Effect
            ApplyEffect();

            // Start Coroutine for end of Ability.

        }

        public override void ApplyEffect()
        {
            // Activate Prefab
            TauntPrefabCollider.enabled = true;

            // Apply Taunt Debuff for each enemy in collider
            // Actual Effect here

            StartCoroutine(WaitToDisableHitbox());

        }

        IEnumerator WaitToDisableHitbox()
        {
            yield return new WaitForSeconds(lengthOfAttackInSec);
            TauntPrefabCollider.enabled = false;
        }

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            if (onCoolDown.Value) return;

            // Update MousePosition
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Check MousePosition distance from Player for TauntRange
            if (Vector2.Distance(currentMousePosition, parentPlayer.transform.position) < range)
            {
                withinTauntRange = true;
            }
            else
            {
                withinTauntRange = false;
            }

            Vector3 newPos = calculateBasicAbilityCursor();
            transform.position = newPos;

            TauntPrefabSpriteRenderer.enabled = true;
            //TauntPrefab.SetActive(true);
        }

        protected override void Update()
        {

            //UnityEngine.Debug.Log("HELLLLL");

            // Every frame, check for cooldowns, set bool accordingly.
            checkCooldown();



            // Every Frame, check for Key: Q, Key Up or Key Down.
            //if (Input.GetKeyDown(KeyCode.Q) && basicActivated == false)
            //{
            //    DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            //}
            //else if (Input.GetKeyUp(KeyCode.Q) && basicActivated == false)
            //{
            //    // Apply Effect first, then deactivate it
            //    ActivateAbility();
            //    //TauntPrefabCollider.enabled = false;
            //    //TauntPrefabSpriteRenderer.enabled = false;
            //    //TauntPrefab.SetActive(false);
            //}


        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;

            // Get Main Camera
            mainCamera = Camera.main;

            // Get Collider
            TauntPrefabCollider = GetComponent<CircleCollider2D>();

            // Get SpriteRenderer
            TauntPrefabSpriteRenderer = GetComponent<SpriteRenderer>();

            // Initially Disable
            TauntPrefabCollider.enabled = false;
            TauntPrefabSpriteRenderer.enabled = false;

            // SUPER IMPORTANT, if you want triggers to happen from your static rigibody, set it to never sleep
            TauntPrefab.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;

            //TauntPrefab.SetActive(false);
        }

        protected Vector3 calculateBasicAbilityCursor()
        {
            Vector3 playerPos = parentPlayer.transform.position;

            Vector3 cursorPosition = currentMousePosition;
            cursorPosition.z = 1f;

            UnityEngine.Debug.Log("withinTauntRange: " + withinTauntRange);

            // Mouse Cursor not in Ability Range
            if (!withinTauntRange)
            {
                //float dx = cursorPosition.x - playerPos.x;
                //float dy = cursorPosition.y - playerPos.y;

                //float angle = Mathf.Atan2(dy, dx);

                //float dxx = (radius - TauntPrefab.transform.localScale.x) * Mathf.Cos(angle);
                //float dyy = (radius - TauntPrefab.transform.localScale.y) * Mathf.Sin(angle);

                //cursorPosition.x = playerPos.x + dxx;
                //cursorPosition.y = playerPos.y + dyy;

                float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

                //UnityEngine.Debug.Log("Transform Position: " + playerPos);

                float focusX = playerPos.x + Mathf.Cos(angle) * range;
                float focusY = playerPos.y + Mathf.Sin(angle) * range;

                Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
                //UnityEngine.Debug.Log(focusCursorPosition);

                return focusCursorPosition;

            }
            return cursorPosition;

        }

        private void OnDrawGizmos()
        {

        }

        // Automatically called when the prefab's collider collides with another collider.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");

            // Call helper function to handle the current collision.
            HandleCollision(collider);
            if (IsServer)
            {
            }
        }

        protected void HandleCollision(Collider2D collider)
        {
            if (!IsOwner) return;
            // Grab all the components under the collided colliders parent, by calling dot operator on 'gameObject'.
            var componentArray = collider.gameObject.GetComponents<MonoBehaviour>();

            foreach (var component in componentArray)
            {
                // Check if component extends IFriendly (Friendly)
                if (component is IFriendly)
                {
                    // Do nothing to friendies
                }
                // Check if component extends IHostile (Hostile)
                if (component is IHostile)
                {
                    UnityEngine.Debug.Log("Taunt Bad Man");

                    // Set Player's Threat Level to highest.
                    parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value += TauntIncreaseAmount;
                    // Sends Server RPC to taunt the collided entity. Pass in SerializedField 'damage' from AbilityBase
                    component.GetComponent<Enemy>().getTauntedServerRPC(damage, 5);

                    StartCoroutine(ResetThreatAfterTaunt(TauntDuration));

                }

            }

        }

        IEnumerator ResetThreatAfterTaunt(float TauntDuration)
        {
            yield return new WaitForSeconds(TauntDuration);
            parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value -= TauntIncreaseAmount;
            if (parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value < 0)
            {
                parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value = 0;
            }
        }

    }
}