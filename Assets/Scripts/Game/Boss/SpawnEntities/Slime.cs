using Assets.Scripts.Game.BehaviorTree;
using BossArena.game;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Boss.SpawnEntities
{
    class Slime : Enemy, IHostile
    {

        [SerializeField]
        private const int MAX_HEALTH = 5;

        [SerializeField]
        private float speed = 2.0f;

        [SerializeField]
        private const float ATTACK_DAMAGE = 1.0f;

        [SerializeField]
        private const float HIT_IMPULSE = 100.0f;

        private Player targetedPlayer = null;

        protected override void Start()
        {
            SetHealth(MAX_HEALTH);
            Debug.Log(targetedPlayer);
        }

        protected override void FixedUpdate()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            int randomIndex = UnityEngine.Random.Range(0, playerObjects.Length);
            playerObjects[randomIndex].TryGetComponent<Player>(out targetedPlayer);
            if (targetedPlayer != null)
            {
                Debug.Log("WALTUH");
                Vector2 targetPosition = targetedPlayer.transform.position; // get the target object's position
                Vector2 currentPosition = transform.position; // get the current object's position

                // calculate the direction and distance to the target object
                Vector2 directionToTarget = targetPosition - currentPosition;
                float distanceToTarget = directionToTarget.magnitude;

                // if we're close enough to the target, stop moving
                if (distanceToTarget <= 0.5f)
                {
                    return;
                }

                // calculate the movement towards the target
                Vector2 movement = directionToTarget.normalized * speed * Time.deltaTime;

                // apply the movement
                this.transform.position += new Vector3(movement.x, movement.y, 0);
            }
        }

        protected override void HandleCollision(Collision2D collision)
        {
            collision.gameObject.TryGetComponent<Player>(out Player p);
            if (p != null)
            {
                //Rigidbody2D targetRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

                //// apply the impulse force in the direction away from the target
                //GetComponent<Rigidbody2D>().AddForce(-(targetRigidBody.position.normalized) * HIT_IMPULSE, ForceMode2D.Impulse);

                p.TakeDamageClientRpc(DamageToPlayer);

                Debug.Log("EPIC");
            } else
            {
                Debug.Log("CRINGE");
            }
        }

        protected override Node SetupTree()
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
