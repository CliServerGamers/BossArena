using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    abstract class EntityBase : NetworkBehaviour
    {
        [SerializeField]
        protected Rigidbody2D rb;

        public CallbackValue<float> MaxHealth = new CallbackValue<float>();
        public CallbackValue<float> CurrentHealth = new CallbackValue<float>();
        public bool IsAlive { get; protected set; }

        public EntityState State { get; set; }
        public int ThreatLevel { get; set; }

        [SerializeField]
        protected float baseMoveSpeed;
        protected float currentMoveSpeed;

        protected virtual void Start()
        {
            SetHealth(100);
            IsAlive = true;
            State = EntityState.DEFUALT;
            ThreatLevel = 0;
            currentMoveSpeed = baseMoveSpeed;
        }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }

        protected virtual void LateUpdate() { }

        protected void SetHealth(float health)

        {
            MaxHealth.Value = health;
            CurrentHealth.Value = health;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            HandleCollision(collision);
        }
        protected abstract void HandleCollision(Collision2D collision);
    }

}

