using BossArena.game;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
    public class EOD : EntityBase
    {

        [SerializeField]
        private const float START_DAMAGE = 10;

        private float currentDamage;

        private float decay = 0.1f;

        SpriteRenderer renderer;

        public const float MAX_HEALTH = 1000.0f;

        public void Start()
        {
            SetHealth(MAX_HEALTH);
            renderer = GetComponent<SpriteRenderer>();
            currentDamage = START_DAMAGE;
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

        void OnCollisionEnter(Collision collision)
        {
            // reduce player health upon collision
            GameObject gameObject = collision.gameObject;
            Component component = gameObject.GetComponent<EntityBase>();
            if (component != null && component is Player)
            {
                Debug.Log("Player should be taking damange by EOD");
                Player player = (Player) component;
                player.CurrentHealth -= currentDamage;
            }
        }

    }
}
