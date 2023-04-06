using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    class SupportBasicAbility : TargetedAbilityBase, IDrawIndicator
    {

        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        // Need to have reference to Taunt Prefab
        [SerializeField]
        private GameObject HealTargetPrefab;
        public GameObject HealAreaPrefab;
        public GameObject HealArea;
        private CircleCollider2D HealAreaPrefabCollider;
        private SpriteRenderer HealTargetPrefabSpriteRenderer;
        public SpriteRenderer HealAreaPrefabSpriteRenderer;

        [SerializeField]
        private int ThreatGen;

        private bool basicActivated = false;
        private bool withinHealRange = false;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            HealTargetPrefabSpriteRenderer.enabled = false;
            if (onCoolDown.Value)
                return;
            onCoolDown.Value = true;
            timeStart = Time.time;

            // Increase Threat each activation
            parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value += ThreatGen;

            ApplyEffect();
            HealAreaPrefabCollider.enabled = false;
        }
        
        public override void ApplyEffect()
        {
            // Update MousePosition
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Check MousePosition distance from Player for TauntRange
            if (Vector2.Distance(currentMousePosition, parentPlayer.transform.position) < range)
            {
                withinHealRange = true;
            }
            else
            {
                withinHealRange = false;
            }
            if (HealArea != null)
            {
                DestroyHealZone_ServerRpc();
            }
            CreateHealZone_ServerRpc(calculateBasicAbilityCursor());
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateHealZone_ServerRpc(Vector3 pos)
        {
            CreateHealZone(pos);
        }
        
        private void CreateHealZone(Vector3 pos)
        {
            if (IsServer)
            {
                HealArea = Instantiate(HealAreaPrefab, pos, Quaternion.identity);
                HealArea.GetComponent<NetworkObject>().Spawn();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyHealZone_ServerRpc()
        {
            DestroyHealZone();
        }

        private void DestroyHealZone()
        {
            if (IsServer)
            {
                HealArea.GetComponent<NetworkObject>().Despawn();
                Destroy(HealArea);
            }
        }



        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            // Update MousePosition
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Check MousePosition distance from Player for TauntRange
            if (Vector2.Distance(currentMousePosition, parentPlayer.transform.position) < range)
            {
                withinHealRange = true;
            }
            else
            {
                withinHealRange = false;
            }

            HealTargetPrefab.transform.position = calculateBasicAbilityCursor();
            
            HealTargetPrefabSpriteRenderer.enabled = true;
            //TauntPrefab.SetActive(true);
        }

        protected override void Update()
        {
            checkCooldown();

        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;

            // Get Main Camera
            mainCamera = Camera.main;

            // Get Collider
            HealAreaPrefabCollider = GetComponent<CircleCollider2D>();

            // Get SpriteRenderer
            HealTargetPrefabSpriteRenderer = GetComponent<SpriteRenderer>();

            // Initially Disable
            HealAreaPrefabCollider.enabled = false;
            HealTargetPrefabSpriteRenderer.enabled = false;

            //TauntPrefab.SetActive(false);
        }

        protected Vector3 calculateBasicAbilityCursor()
        {
            Vector3 playerPos = parentPlayer.transform.position;

            Vector3 cursorPosition = currentMousePosition;
            cursorPosition.z = 1f;

            // Mouse Cursor not in Ability Range
            if (!withinHealRange)
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



    }
}
