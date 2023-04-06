using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    class HealZone : EntityBase
    {
        public int lifetime;
        public float healAmount;
        protected override void Start()
        {
            base.Start();
            SetHealth(lifetime);
        }
        
        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            CurrentHealth.Value--;
            if (CurrentHealth.Value <= 0)
            {
                despawn();
            }
        }

        protected override void HandleTrigger(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (IsServer)
                {
                    collision.gameObject.GetComponent<Player>().TakeDamageClientRpc(-healAmount);
                }
                despawn(); 
            }
        }

        private void despawn()
        {
            if (IsServer)
            {
                gameObject.GetComponent<NetworkObject>().Despawn();
                Destroy(gameObject);
            }
        }
    }
}