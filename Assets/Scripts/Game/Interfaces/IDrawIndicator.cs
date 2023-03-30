using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    public interface IDrawIndicator
    {
        public abstract void DrawAbilityIndicator(Vector3 targetLocation);
    }
}
