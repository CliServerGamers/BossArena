using BossArena;
using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    class BossProjectile : EntityBase, IHostile
    {
        [SerializeField]
        private float projectileSpeed;
        [SerializeField]
        private float timeToLive;
        [SerializeField]
        private float HIT_DAMAGE;

        [SerializeField]
        private GameObject slimePrefab;

        protected override void HandleCollision(Collision2D collision)
        {
            IsAlive = false;
            this.GetComponent<Collider2D>().enabled = false;
            if (collision.gameObject.TryGetComponent(out IHostile hostile))
            {
                return;
            }
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
                    ((Player) monoBehaviour).TakeDamageClientRpc(HIT_DAMAGE);
                }
                DespawnAndSpawn();
            }

        }

        protected override void Update()
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive < 0)
            {
                if (IsServer)
                {
                    DespawnAndSpawn();
                }
                return;
            }
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
            //transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }
        private void DespawnAndSpawn()
        {
            // 1 in 10 chance 
            if (UnityEngine.Random.Range(0, 10) == 0)
            {
                GameObject slime = Instantiate(slimePrefab, this.transform.position, Quaternion.identity);
                slime.GetComponent<NetworkObject>().Spawn();
            }

            Despawn();
        }
    }
}
