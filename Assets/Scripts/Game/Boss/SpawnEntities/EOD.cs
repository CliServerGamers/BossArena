using BossArena.game;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
     class EOD : EntityBase
    {
        // set size, decay value, damage tick

        [SerializeField]
        private const float START_DAMAGE = 15;

        private float currentDamage;

        private float decay = 0.4f;

        SpriteRenderer renderer;

        public const float MAX_HEALTH = 500.0f;

        protected override void Start()
        {
            Debug.Log("I am in the start method for EOD");
            SetHealth(MAX_HEALTH);
            renderer = GetComponent<SpriteRenderer>();
            currentDamage = START_DAMAGE;

            // Get all colliders on objects with the specified tag
            BoxCollider2D colliderToIgnore = GameObject.FindGameObjectWithTag("Boss").GetComponent<BoxCollider2D>();

            // Ignore collisions between the current object's collider and all colliders on the specified tag
            Collider2D currentCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(currentCollider, colliderToIgnore, true);

            // make it behind everything because the unity editor being jank
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }

        protected override void FixedUpdate()
        {
            return;
        }

        protected override void LateUpdate()
        {
            return;
        }

        protected override void Update()
        {
            // have the damage lose effects as the object begins to decay
            if (currentDamage > 0)
            {
                currentDamage = currentDamage / START_DAMAGE;
            }

            // lose health
            SetHealth(CurrentHealth - decay);
            Debug.Log(CurrentHealth);
            if (CurrentHealth < 0)
            {
                Destroy(this.gameObject);
            }

            // maps the opacity to the percentage of health lost from range (50 - 100)%
            Color spriteColor = renderer.color;
            spriteColor.a = ((CurrentHealth / MAX_HEALTH) / 2) + 0.5f;
            renderer.color = spriteColor;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            // reduce player health upon collision
            GameObject gameObject = collision.gameObject;
            Component component = gameObject.GetComponent<EntityBase>();
            if (component != null && component is Player)
            {
                Debug.Log("Player should be taking damange by EOD");
                Player player = (Player)component;
                player.CurrentHealth -= currentDamage;
            }
        }

        protected override void HandleCollision(Collision2D collision)
        {
        }
    }
}
