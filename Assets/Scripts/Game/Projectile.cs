using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class Projectile : EntityBase
    {
        Vector2 projectileDestination;

        protected override void Update()
        {
            UnityEngine.Debug.Log("Moving");
            // Projectile has infinite range, so we constantly update its destination.
            findDestination();

            transform.position *= baseMoveSpeed;

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void findDestination()
        {
            projectileDestination = transform.position * 3;

        }

    }
}
