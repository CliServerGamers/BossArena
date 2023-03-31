using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    class HealZone : EntityBase
    {

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            CurrentHealth--;
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        protected override void HandleCollision(Collision2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                Destroy(gameObject);
                //collision.gameObject.GetComponent<Player>().CurrentHealth += 10;
            }
        }
    }
}