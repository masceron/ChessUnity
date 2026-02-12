using MemoryPack;
using Game.Common;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class AdrenalineRadiatorExcute : Action, IRelicAction
    {
        public AdrenalineRadiatorExcute(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            BoardUtils.PieceOn(Maker).SkillCooldown = 0;
        }
    }
}