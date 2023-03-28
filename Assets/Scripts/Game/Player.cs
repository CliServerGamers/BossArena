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
        public int dodgeCooldown;
        [SerializeField]
        public Archetype Archetype;
        //Since this isn't a monobehaviour, we can't simply use gameObject to reference the attached gameobject
        //So we kinda have to do this
        //(That or i'm just dumb lol)
        public GameObject playerObj;

        //public Player(Archetype archetype) : base()
        //{
        //    Archetype = archetype;
        //}

        protected override void Start()
        {
            base.Start();
            //SetHealth(Archetype.MaxHealth);
            Debug.Log(playerObj);
            rb = playerObj.GetComponent<Rigidbody2D>();
            ps = playerObj.GetComponent<ParticleSystem>();
            dodgeCooldown = 0;
            Archetype.BasicAbility.Play
        }

        protected override void Update()
        {
            if (!IsOwner) return;


            horizVelocity = Input.GetAxisRaw("Horizontal");
            vertVelocity = Input.GetAxisRaw("Vertical");

            if(Archetype.BasicAttack is IDrawIndicator)
            {
                ((IDrawIndicator)Archetype.BasicAttack).DrawAbilityIndicator(Input.mousePosition);
            }

            //Ability Section
            if (Input.GetMouseButtonDown(0))
            {
                Archetype.BasicAttack.ActivateAbility();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ((IDrawIndicator)Archetype.BasicAttack).DrawAbilityIndicator(Input.mousePosition);
            }

            //Make the player dash a short distance on spacebar press
            if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldown < 1)
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
            timerCheck();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) return;

            //Make the player dash a short distance on spacebar press
            if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldown < 1)
            {
                dash();
                dodgeCooldown = 90;
            }

        }

        void dash()
        {
            playerObj.transform.position += new Vector3(horizVelocity * 3, vertVelocity * 3, 0);
        }

        void timerCheck()
        {
            if (dodgeCooldown > 0)
            {
                dodgeCooldown--;
            }
        }

    }


}
