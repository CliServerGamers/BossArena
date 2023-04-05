using System;
using System.Collections;
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

        [field:SerializeField]
        public float MaxHealth { get; protected set; }
        [field: SerializeField]
        public float CurrentHealth { get; set; }
        [field: SerializeField]
        public bool IsAlive { get; protected set; }
        [field: SerializeField]
        public NetworkVariable<EntityState> State = new NetworkVariable<EntityState>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [field: SerializeField]
        public NetworkVariable<int> ThreatLevel = new NetworkVariable<int>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField]
        protected float baseMoveSpeed;
        protected float currentMoveSpeed;

        protected virtual void Start()
        {
            SetHealth(100);
            IsAlive = true;
            State.Value = EntityState.DEFUALT;
            ThreatLevel.Value = 0;
            currentMoveSpeed = baseMoveSpeed;
            rb = GetComponent<Rigidbody2D>();
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

        public virtual void TakeDamage(float damage)
        {
            Debug.Log($"Taking {damage} points of damage");
        }

        // Helper Function: Setting Taunted State
        IEnumerator setStateTaunt(float effectDuration)
        {
            Debug.Log($"Taunted for {effectDuration} seconds");

            // Set State to 'Taunted'
            State.Value = EntityState.TAUNTED;

            // Wait duration to return
            yield return new WaitForSeconds(effectDuration);

            Debug.Log($"No longer Taunted");

            // Reset State after Taunted 
            State.Value = EntityState.DEFUALT;

        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float damage)
        {
            TakeDamage(damage);
        }

        [ServerRpc(RequireOwnership = false)]
        public void getTauntedServerRPC(float damage, float effectDuration)
        {
            // Deal Damage
            TakeDamage(damage);
            
            // Apply Taunted State, pass in effectDuration to Coroutine, then Reset State to Default
            StartCoroutine(setStateTaunt(effectDuration));
        }


    }

}

