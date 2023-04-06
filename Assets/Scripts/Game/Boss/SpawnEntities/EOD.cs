using BossArena.game;
using System;
using UnityEngine;

namespace BossArena.game
{
     class EOD : EntityBase, IGoop
    {
        // set size, decay value, damage tick

        [SerializeField]
        private const float START_DAMAGE = 15;

        private float currentDamage;

        private float decay = 0.4f;

        SpriteRenderer rend;

        public const float MAX_HEALTH = 500.0f;

        protected override void Start()
        {
            SetHealth(MAX_HEALTH);
            rend = GetComponent<SpriteRenderer>();
            currentDamage = START_DAMAGE;

            // Get all colliders on objects with the specified tag
            BoxCollider2D colliderToIgnore = GameObject.FindGameObjectWithTag("Boss").GetComponent<BoxCollider2D>();

            // Ignore collisions between the current object's collider and all colliders on the specified tag
            Collider2D currentCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(currentCollider, colliderToIgnore, true);

            // make it behind everything because the unity editor being jank
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
        }

        protected override void Update()
        {
            // have the damage lose effects as the object begins to decay
            if (currentDamage > 0)
            {
                currentDamage = currentDamage / START_DAMAGE;
            }

            // lose health
            SetHealth(CurrentHealth.Value - decay);
            //Debug.Log(CurrentHealth.Value);
            if (CurrentHealth.Value < 0)
            {
                // only destroy objects on server
                if (IsServer)
                {
                    Destroy(this.gameObject);
                }
            }

            // maps the opacity to the percentage of health lost from range (50 - 100)%
            Color spriteColor = rend.color;
            spriteColor.a = ((CurrentHealth.Value / MAX_HEALTH) / 2) + 0.5f;
            rend.color = spriteColor;
        }

        protected override void HandleTrigger(Collider2D collision)
        {
            if (!IsServer) return;
            // reduce player health upon collision
            GameObject gameObject = collision.gameObject;
            Component component = gameObject.GetComponent<EntityBase>();
            if (component != null && component is Player)
            {
                Player player = (Player)component;
                player.TakeDamageClientRpc(currentDamage);
            }
        }
    }
}
