using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class TankBasicAbility : TargetedAbilityBase
    {

        // Need to have reference to Parent Player Prefab
        [SerializeField]
        private GameObject PlayerPrefab;

        // Need to have reference to Taunt Prefab
        [SerializeField]
        private GameObject TauntPrefab;
        private CircleCollider2D TauntPrefabCollider;
        private SpriteRenderer TauntPrefabSpriteRenderer;

        private bool basicActivated = false;
        private bool withinTauntRange = false;

        // Use for checking elapsed time while ulted.
        private float timeStart;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            basicActivated = true;
            timeStart = Time.time;
            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            // Get Collider

            // Apply Taunt Debuff for each enemy in collider
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            TauntPrefabCollider.enabled = true;
            TauntPrefabSpriteRenderer.enabled = true;
            //TauntPrefab.SetActive(true);
        }

        protected override void Update()
        {

            //UnityEngine.Debug.Log("HELLLLL");

            // Every frame, check for cooldowns, set bool accordingly.
            checkCooldown();

            // Update MousePosition
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Check MousePosition distance from Player for TauntRange
            if (Vector2.Distance(currentMousePosition, PlayerPrefab.transform.position) < range)
            {
                withinTauntRange = true;
            } else
            {
                withinTauntRange = false;
            }

            TauntPrefab.transform.position = calculateBasicAbilityCursor();

            // Every Frame, check for Key: Q, Key Up or Key Down.
            if (Input.GetKeyDown(KeyCode.Q) && basicActivated == false)
            {
                DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetKeyUp(KeyCode.Q) && basicActivated == false)
            {
                // Apply Effect first, then deactivate it
                ActivateAbility();
                TauntPrefabCollider.enabled = false;
                TauntPrefabSpriteRenderer.enabled = false;
                //TauntPrefab.SetActive(false);
            }
            

        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;

            // Get Collider
            TauntPrefabCollider = TauntPrefab.transform.GetComponent<CircleCollider2D>();

            // Get SpriteRenderer
            TauntPrefabSpriteRenderer = TauntPrefab.transform.GetComponent<SpriteRenderer>();

            // Initially Disable
            TauntPrefabCollider.enabled = false;
            TauntPrefabSpriteRenderer.enabled = false;

            //TauntPrefab.SetActive(false);
        }

        protected Vector3 calculateBasicAbilityCursor()
        {
            Vector3 playerPos = PlayerPrefab.transform.position;

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

                float focusX = playerPos.x + Mathf.Cos(angle)*range;
                float focusY = playerPos.y + Mathf.Sin(angle)*range;

                Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
                //UnityEngine.Debug.Log(focusCursorPosition);

                return focusCursorPosition;

            }
            return cursorPosition;

        }

        private void OnDrawGizmos()
        {

        }

        public void checkCooldown()
        {
            if (Time.time - timeStart >= coolDownDelay)
            {
                // Enough time has passed, set ultimatedActivated as off.
                basicActivated = false;
            }
        }

    }
}