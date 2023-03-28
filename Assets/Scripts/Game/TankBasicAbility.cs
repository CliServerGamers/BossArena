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

        private bool basicActivated = false;

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
            TauntPrefabCollider = TauntPrefab.transform.GetComponent<CircleCollider2D>();

            // Apply Taunt Debuff for each enemy in collider
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            TauntPrefab.SetActive(true);
        }

        protected override void Update()
        {
            // Every frame, check for cooldowns, set bool accordingly.
            checkCooldown();

            // TODO: Given MousePosition, calculate bounds for this ability
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            TauntPrefab.transform.position = calculateBasicAbilityCursor();

            UnityEngine.Debug.Log("BasicActivated: " + basicActivated);

            // Every Frame, check for Key: Q, Key Up or Key Down.
            if (Input.GetKeyDown(KeyCode.Q) && basicActivated == false)
            {
                DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetKeyUp(KeyCode.Q) && basicActivated == false)
            {
                // Apply Effect first, then deactivate it
                ActivateAbility();
                TauntPrefab.SetActive(false);
            }
            

        }

        // Start is called before the first frame update
        void Start()
        {
            timeStart = Time.time;
            TauntPrefab.SetActive(false);
        }

        protected Vector3 calculateBasicAbilityCursor()
        {
            Vector3 playerPos = PlayerPrefab.transform.position;

            float angle = Mathf.Atan2(currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            float focusX = playerPos.x + Mathf.Cos(angle) * 3;
            float focusY = playerPos.y + Mathf.Sin(angle) * 3;

            Vector3 BasicAbilityCursorPosition = new Vector3(focusX, focusY, 0f);

            return BasicAbilityCursorPosition;
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