using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    abstract class EntityBase : NetworkBehaviour
    {
        [SerializeField]
        protected Rigidbody2D rb;

        [field: SerializeField]
        public NetworkVariable<float> MaxHealth = new NetworkVariable<float>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [field: SerializeField]
        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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

        protected EntityState state;

        protected virtual void Start()
        {
            SetHealth(100);
            IsAlive = true;
            State.Value = EntityState.DEFUALT;
            ThreatLevel.Value = 0;
            currentMoveSpeed = baseMoveSpeed;
            rb = GetComponent<Rigidbody2D>();

            CurrentHealth.OnValueChanged += IsDeath;
        }

        public virtual void SetState(EntityState state)
        {
            this.state = state;
        }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }

        protected virtual void LateUpdate() { }

        protected void SetHealth(float health)

        {
            if(!IsOwner) return;
            MaxHealth.Value = health;
            CurrentHealth.Value = health;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            HandleCollision(collision);
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            HandleTrigger(collision);
        }
        protected virtual void HandleCollision(Collision2D collision) { }
        protected virtual void HandleTrigger(Collider2D collision) { }

        public virtual void TakeDamage(float damage)
        {
            if (!IsOwner) return;
            if (CurrentHealth.Value - damage >= MaxHealth.Value)
            {
                CurrentHealth.Value = MaxHealth.Value;
            }
            else
            {
                CurrentHealth.Value -= damage;
            }
        }

        // Helper Function: Setting Taunted State
        protected IEnumerator setStateTaunt(float effectDuration)
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

        [ClientRpc]
        public void TakeDamageClientRpc(float damage)

        {
            TakeDamage(damage);
        }

        public virtual void IsDeath(float old, float newHealth)
        {
            if (newHealth <= 0)
            {
                State.Value = EntityState.DEAD;
                Despawn();
                //modalManager.DisplayModal("Game Over", "You Died!");
            }
        }

        protected void Despawn()
        {
            if (IsServer)
            {
                Debug.Log($"{OwnerClientId}: Despawn");
                this.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                Debug.Log($"{OwnerClientId}: Despawn RPC");
                DespawnServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DespawnServerRpc()
        {
            this.GetComponent<NetworkObject>().Despawn();

        }
    }

}

