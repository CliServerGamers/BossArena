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
        [field:SerializeField]
        public float MaxHealth { get; protected set; }
        [field: SerializeField]
        public float CurrentHealth { get; set; }
        [field: SerializeField]
        public bool IsAlive { get; protected set; }
        [field: SerializeField]
        public EntityState State { get; set; }
        [field: SerializeField]
        public NetworkVariable<int> ThreatLevel = new NetworkVariable<int>();
        [SerializeField]
        protected float baseMoveSpeed;
        protected float currentMoveSpeed;

        protected virtual void Start()
        {
            SetHealth(100);
            IsAlive = true;
            State = EntityState.DEFUALT;
            ThreatLevel.Value = 0;
            currentMoveSpeed = baseMoveSpeed;
        }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }

        protected virtual void LateUpdate() { }

        protected void SetHealth(float health)
        {
            MaxHealth = health;
            CurrentHealth = health;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            HandleCollision(collision);
        }
        protected abstract void HandleCollision(Collision2D collision);
    }
}