using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    class Projectile : EntityBase
    {


        protected override void FixedUpdate()
        {
            if (!IsOwner || !IsAlive)
                return;
            //UnityEngine.Debug.Log("Moving");
            // Projectile has infinite range, so we constantly update its destination.
            //findDestination();

            //transform.position *= baseMoveSpeed;
            //transform.forward += baseMoveSpeed;
            Vector3 movement = transform.right * baseMoveSpeed;
            rb.velocity = new Vector2(movement.x, movement.y);

            Vector3 fwd = transform.rotation * Vector3.right;
            Debug.DrawRay(transform.position, fwd, Color.red, 0f, true);
        }


        //public void findDestination()
        //{
        //    projectileDestination = transform.position * 3;

        //}

        protected override void HandleCollision(Collision2D collision)
        {
            Debug.Log($"{OwnerClientId}: Hit");
            IsAlive = false;
            this.GetComponent<Collider2D>().enabled = false;
            var tempMonoArray = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in tempMonoArray)
            {
                if (monoBehaviour is not EntityBase)
                {
                    //Return to Pool
                    continue;
                }
                if(monoBehaviour is IFriendly)
                {
                    ((IFriendly) monoBehaviour).HitFriendlyServerRpc(OwnerClientId);
                }
                    Despawn();
            }


        }

        void Despawn()
        {
            if (IsServer)
            {
                Debug.Log($"{OwnerClientId}: Despawn");
                this.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                Debug.Log($"{OwnerClientId}: Despawn RPC");
                DespawnServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DespawnServerRpc()
        {
            this.GetComponent<NetworkObject>().Despawn();

        }
    }
}
