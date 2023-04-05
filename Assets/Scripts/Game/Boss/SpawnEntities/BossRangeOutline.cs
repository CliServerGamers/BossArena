using BossArena.game;
using UnityEngine;

namespace Assets.Scripts.Game.Boss.SpawnEntities
{
    class BossRangeOutline : EntityBase
    {
        private const float MAX_HEALTH = 1000.0f;
        private const float DAMAGE_TICK = 10f;

        private SpriteRenderer renderer;

        public void Start()
        {
            SetHealth(MAX_HEALTH);
            renderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            // lose health
            SetHealth(CurrentHealth - DAMAGE_TICK);

            if (CurrentHealth < 0)
            {
                Destroy(this.gameObject);
            }

            // maps the opacity to the percentage of health lost
            Color spriteColor = renderer.color;
            spriteColor.a = (CurrentHealth / MAX_HEALTH);
            renderer.color = spriteColor;
        }

        protected override void HandleCollision(Collision2D collision)
        {
            return;
        }
    }
}
