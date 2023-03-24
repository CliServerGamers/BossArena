using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace BossArena.game
{
    abstract class TargetedAbilityBase : AbilityBase
    {
        [SerializeField]
        bool isSelfTarget;

        [SerializeField]
        float radius;

        public abstract void DrawAbilityIndicator(Vector3 targetLocation);

        private void getEntitiesInArea(Vector3 mosPos)
        {
            var colliders = Physics2D.OverlapCircleAll(mosPos, radius);
            foreach (var collider in colliders)
            {
                Debug.Log($"{collider.gameObject.name} is nearby");
            }
        }
    }
}
