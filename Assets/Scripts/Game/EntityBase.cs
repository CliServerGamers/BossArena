using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    public abstract class EntityBase : NetworkBehaviour
    {
        [SerializeField]
        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; set; }
        public bool IsAlive { get; protected set; }

        public EntityState State { get; set; }
        public int ThreatLevel { get; set; }

        [SerializeField]
        protected int baseMoveSpeed;
        protected int currentMoveSpeed;

        protected virtual void Start()
        {
            SetHealth(100);
            IsAlive = true;
            State = EntityState.DEFUALT;
            ThreatLevel = 0;
            currentMoveSpeed = baseMoveSpeed;
        }

        protected abstract void Update();

        protected abstract void FixedUpdate();

        protected abstract void LateUpdate();

        protected void SetHealth(int health)
        {
            MaxHealth = health;
            CurrentHealth = health;
        }
    }
}
