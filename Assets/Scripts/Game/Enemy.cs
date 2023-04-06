using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
     abstract class Enemy : EntityBase, IHostile
    {
        [field: SerializeField]
        public Player CurrentTarget { get; set; }
        [field: SerializeField]
        public float threatRadius { get; private set; }

        private Node _root = null;

        protected override void Start()
        {
            _root = SetupTree();
            base.Start();
            SetHealth(MaxHealth.Value);
        }

        // no object that extends entity should be able to override this method as it takes care of running the tree
        protected override sealed void Update()
        {
            if(!IsOwner) return;
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();

        public void SelectTarget()
        {
            if (State.Value == EntityState.TAUNTED) return;
            /// Create collision box with threatRadius
            /// Get players overlapped with collision
            Collider2D[] hitCol = Physics2D.OverlapCircleAll((Vector2)transform.position, threatRadius);
            Debug.Log($"Found {hitCol.Length}");
            foreach (Collider2D col in hitCol)
            {
                if (col.gameObject.TryGetComponent(out Player player))
                {
                    if (CurrentTarget == null) CurrentTarget = player;

                    if (CurrentTarget.ThreatLevel.Value < player.ThreatLevel.Value)
                    {
                        Debug.Log($"Current Target threat level {CurrentTarget.ThreatLevel.Value} : Detected Threat level {player.ThreatLevel.Value}");
                        CurrentTarget = player;
                    }
                    Debug.Log($"Current Target {CurrentTarget}");
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void getTauntedServerRPC(float damage, float effectDuration)
        {
            // Deal Damage
            TakeDamage(damage);
            SelectTarget();
            // Apply Taunted State, pass in effectDuration to Coroutine, then Reset State to Default
            StartCoroutine(setStateTaunt(effectDuration));
        }

    }
}
