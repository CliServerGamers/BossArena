using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class GenericProjectileAbility : TargetedAbilityBase, IDrawIndicator
    {

        // Need to have reference to Projectile Prefab
        [SerializeField]
        private GameObject projectilePrefab;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {

            // TODO: 
            // 1 Scaled Range to be larger than the Arena (Probably by 5)
            //Vector2 projectileDestination = new Vector2(currentMousePosition.x, currentMousePosition.y) * 10;

            //UnityEngine.Debug.Log("ProjectileDestination: " + projectileDestination);

            Vector3 focusCursor = calculateFocusCursor();

            Quaternion rot = new Quaternion();

            rot.SetFromToRotation(focusCursor - parentPlayer.transform.position, mainCamera.ScreenToWorldPoint(mosPos.Value) - parentPlayer.transform.position);

            // 2. Instantiate Projectile
            if (IsServer)
            {

                spawnProjectile(focusCursor, rot);
            }
            else if (IsOwner)
            {
                spawnProjectileServerRpc(focusCursor, rot);
            }

            //Fetch from pool

        }

        private void spawnProjectile(Vector3 focusCursor, Quaternion rot)
        {
            GameObject currentProjectile = GameObject.Instantiate(projectilePrefab, focusCursor, rot);
            currentProjectile.GetComponent<NetworkObject>().Spawn();
        }

        [ServerRpc]
        private void spawnProjectileServerRpc(Vector3 focusCursor, Quaternion rot)
        {
            spawnProjectile(focusCursor, rot);
        }

        public override void ApplyEffect()
        {
            throw new System.NotImplementedException();
        }

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            //Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            Vector3 targetWorldLocation = mainCamera.ScreenToWorldPoint(targetLocation);
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = targetWorldLocation;

            // Calculate Focus Cursor
            // Later on 'focusCursor' Vector will be used for a Sprite
            Vector2 focusCursor = calculateFocusCursor();
        }

        // Update is called once per frame
        protected override void Update()
        {
            // Pass in World Position of Mouse
            //DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));

            //// Listen for Mouse Event (LMB Down)
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Debug.Log("peepeepoopoo");
            //    ActivateAbility(Input.mousePosition);

            //}
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            Vector3 focusCursor = calculateFocusCursor();
            drawFocusCursor(focusCursor);
            Gizmos.DrawLine(focusCursor, currentMousePosition);
            //Gizmos.DrawSphere(currentMousePosition - parentPlayer.transform.position, 0.5f);
            //Gizmos.color = new Color(0,0f, 1.0f, 1f);
            //Gizmos.DrawSphere(focusCursor - parentPlayer.transform.position, 0.5f);
            //Gizmos.color = new Color(0, 0f, 0.0f, 1f);
            //Gizmos.DrawSphere(currentMousePosition, 0.5f);
        }

        protected Vector3 calculateFocusCursor()
        {
            Vector3 playerPos = parentPlayer.transform.position;

            float angle = Mathf.Atan2(currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

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
