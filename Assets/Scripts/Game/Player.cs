using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    /// </summary>
    class Player : EntityBase, IFriendly, IThreat
    {
        public Rigidbody2D rb;
        private float horizVelocity;
        private float vertVelocity;
        [SerializeField]
        public Archetype Archetype { get; private set; }

        public Player(Archetype archetype) : base()
        {
            Archetype = archetype;
        }

        protected override void Start()
        {
            base.Start();
            SetHealth(Archetype.MaxHealth);
        }

        protected override void Update()
        {
            horizVelocity = Input.GetAxisRaw("Horizontal");
            vertVelocity = Input.GetAxisRaw("Vertical");
        }
        
        protected override void FixedUpdate()
        {
            rb.velocity = new Vector2(horizVelocity * currentMoveSpeed, vertVelocity * currentMoveSpeed);
        }
        
    }


}
