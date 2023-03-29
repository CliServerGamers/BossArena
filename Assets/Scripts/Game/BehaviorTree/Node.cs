using Assets.Scripts.Game.Boss;
using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Game.BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [CreateAssetMenu]
    public class Node : ScriptableObject
    {

        [SerializeField]
        protected GameObject ability;

        [SerializeField]
        public GameObject boss;

        protected NodeState state;
        public Node parent;
        protected List<Node> children;
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            GameObject basicAttack = Instantiate(ability);
            basicAttack.GetComponent<NetworkObject>().Spawn();
            basicAttack.transform.SetParent(boss.transform, false);
            basicAttack.GetComponent<IBossAbility>().InitializeNode(this);
            children = new List<Node>();
            parent = null;
        }

        public Node(List<Node> children) {
            GameObject basicAttack = Instantiate(ability);
            basicAttack.GetComponent<NetworkObject>().Spawn();
            basicAttack.transform.SetParent(boss.transform, false);
            basicAttack.GetComponent<IBossAbility>().InitializeNode(this);
            this.children = new List<Node>();
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate()
        {
            IBossAbility abilityComponent = ability.GetComponent<IBossAbility>();
            state = abilityComponent.RunAbility();
            return state;
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }

}
