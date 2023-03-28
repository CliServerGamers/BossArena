using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class Projectile : EntityBase
    {
        Vector2 projectileDestination;

        protected override void FixedUpdate()
        {
            if (!IsOwner || !IsAlive) return;
            UnityEngine.Debug.Log("Moving");
            // Projectile has infinite range, so we constantly update its destination.
            //findDestination();

            //transform.position *= baseMoveSpeed;
            //transform.forward += baseMoveSpeed;

            transform.position += transform.forward * baseMoveSpeed;

            Vector3 fwd = transform.rotation * Vector3.forward;
            Debug.DrawRay(transform.position, fwd, Color.red, 0f, true);
        }


        //public void findDestination()
        //{
        //    projectileDestination = transform.position * 3;

        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsOwner) return;
            IsAlive = false;
            this.GetComponent<Collider>().enabled = false;
            //Return to pool
        }
    }
}
