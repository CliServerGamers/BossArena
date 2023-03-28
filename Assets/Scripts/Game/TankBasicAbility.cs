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

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            // Apply Taunt Debuff for each enemy in collider
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            TauntPrefab.SetActive(true);
        }

        protected override void Update()
        {
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            TauntPrefab.transform.position = currentMousePosition;

            // Every Frame, check for Key: Q, Key Up or Key Down.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            } 
            else if (Input.GetKeyUp(KeyCode.Q))
            { 
                // Apply Effect first, then deactivate it
                ApplyEffect();
                TauntPrefab.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            TauntPrefab.SetActive(false);
        }

        protected Vector3 calculateFocusCursor()
        {
            Vector3 playerPos = PlayerPrefab.transform.position;

            float angle = Mathf.Atan2(currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            float focusX = playerPos.x + Mathf.Cos(angle);
            float focusY = playerPos.y + Mathf.Sin(angle);

            Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);

            return focusCursorPosition;
        }

        private void OnDrawGizmos()
        {

        }

    }
}