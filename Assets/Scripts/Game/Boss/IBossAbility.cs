using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Boss
{
    public interface IBossAbility
    {
        public NodeState RunAbility();
        public void InitializeNode(Node node);
    }
}
