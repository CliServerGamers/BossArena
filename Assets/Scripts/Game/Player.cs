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
        private Rigidbody2D rb;
        private ParticleSystem ps;
        private float horizVelocity;
        private float vertVelocity;
        [SerializeField]
        public Archetype Archetype { get; private set; }
        //Since this isn't a monobehaviour, we can't simply use gameObject to reference the attached gameobject
        //So we kinda have to do this
        //(That or i'm just dumb lol)
        public GameObject playerObj;

        public Player(Archetype archetype) : base()
        {
            Archetype = archetype;
        }

        protected override void Start()
        {
            base.Start();
            //SetHealth(Archetype.MaxHealth);
            Debug.Log(playerObj);
            rb = playerObj.GetComponent<Rigidbody2D>();
            ps = playerObj.GetComponent<ParticleSystem>();
        }

        protected override void Update()
        {
            horizVelocity = Input.GetAxisRaw("Horizontal");
            vertVelocity = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown(KeyCode.Space))
            //Make the player dash a short distance on spacebar press
            {
                var psemit = ps.emission;
                psemit.enabled = true;
                ps.Play();
            }
        }
        
        protected override void FixedUpdate()
        {
            //Actually moving the player by changing their rigidbody velocity
            rb.velocity = new Vector2(horizVelocity * currentMoveSpeed, vertVelocity * currentMoveSpeed);
        }

        protected override void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            //Make the player dash a short distance on spacebar press
            {
                dash();
            }
        }

        void dash()
        {
            playerObj.transform.position += new Vector3(horizVelocity * 3, vertVelocity * 3, 0);
        }
        
    }


}
