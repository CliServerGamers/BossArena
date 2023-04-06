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
        // Need to have reference to Camera.
        [SerializeField]
        protected Camera? mainCamera;

        [SerializeField]
        protected bool isSelfTarget;

        [SerializeField]
        protected float radius;

        // Need to have reference to Parent Player Prefab
        [SerializeField]
        protected GameObject? parentPlayer;

        protected override void Start()
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            base.Start();
            getCamera();
        }

        protected void getCamera()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }
        protected void getEntitiesInArea(Vector3 mosPos)
        {
            var colliders = Physics2D.OverlapCircleAll(mosPos, radius);
            foreach (var collider in colliders)
            {
                Debug.Log($"{collider.gameObject.name} is nearby");
            }
        }

        public void SetParent(GameObject parent) { this.parentPlayer = parent; }
    }
}
