using Game.Common;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class AdrenalineRadiatorExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private AdrenalineRadiatorExecute()
        {
        }

        public AdrenalineRadiatorExecute(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            BoardUtils.PieceOn(Maker).SkillCooldown = 0;
        }
    }
}