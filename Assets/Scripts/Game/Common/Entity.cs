using System.Collections.Generic;
using Game.Effects;

namespace Game.Common
{
    public abstract class Entity
    {
        public readonly int ID = BoardUtils.NextEntityID();
        public int Pos = -1;
        public readonly List<Effect> Effects = new();
    }
}