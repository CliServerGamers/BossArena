using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
    public class GoToPlayer : Node
    {
        private Transform _transform; 

        public GoToPlayer(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, target.position, 3 * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            state = NodeState.RUNNING;
            return state;
        }

    }

}