using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class GenericProjectileAbility : TargetedAbilityBase
    {
        // Need to have reference to Parent Player Prefab
        [SerializeField]
        private GameObject PlayerPrefab;

        // Need to have reference to Projectile Prefab
        [SerializeField]
        private GameObject projectilePrefab;
        
        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {

            // TODO: 
            // 1 Scaled Range to be larger than the Arena (Probably by 5)
            Vector2 projectileDestination = new Vector2(currentMousePosition.x, currentMousePosition.y) * 10;

            UnityEngine.Debug.Log("ProjectileDestination: " + projectileDestination);

            // 2. Instantiate Projectile
            Instantiate(projectilePrefab, calculateFocusCursor(), Quaternion.identity);

        }

        public override void ApplyEffect()
        {
            throw new System.NotImplementedException();
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = targetLocation;

            // Calculate Focus Cursor
            // Later on 'focusCursor' Vector will be used for a Sprite
            Vector2 focusCursor = calculateFocusCursor();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            UnityEngine.Debug.Log("CurrentMousePosition: " + currentMousePosition);

            // Pass in World Position of Mouse
            DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));

            // Listen for Mouse Event (LMB Down)
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("peepeepoopoo");
                ActivateAbility(Input.mousePosition);

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            drawFocusCursor(calculateFocusCursor());
        }

        protected Vector3 calculateFocusCursor()
        {
            Vector3 playerPos = PlayerPrefab.transform.position;

            float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            float focusX = playerPos.x + Mathf.Cos(angle);
            float focusY = playerPos.y + Mathf.Sin(angle);

            Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
            
            return focusCursorPosition;
        }

        protected void drawFocusCursor(Vector3 pos)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(pos, new Vector3(1, 1, 1));
        }
    }
}
