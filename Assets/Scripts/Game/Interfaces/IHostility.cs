using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossArena.game
{
    public interface IFriendly {
        public void HitFriendlyServerRpc(ulong hitter);
    }
    public interface IHostile { }
}
