using BossArena;
using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
    class BossProjectile : EntityBase
    {
        private float projectileSpeed = 7.0f;
        private float timeToLive = 1.0f;
        [SerializeField]
        private float HIT_DAMAGE = 10.0f;

        [SerializeField]
        private GameObject slimePrefab;

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
                if (monoBehaviour is Player)
                {
                    Debug.Log("Bullet hit player");
                    //((Player)monoBehaviour).TakeDamageClientRPC(HIT_DAMAGE);
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

    protected override void Update()
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive < 0)
            {
                if (IsServer)
                {
                    // 1 in 10 chance 
                    if (UnityEngine.Random.Range(0, 10) == 0)
                    {
                        GameObject slime = Instantiate(slimePrefab, this.transform.position, Quaternion.identity);
                        slime.GetComponent<NetworkObject>().Spawn();
                    }

                    Destroy(this.gameObject);
                }
                return;
            }
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
            //transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }
    }
}
