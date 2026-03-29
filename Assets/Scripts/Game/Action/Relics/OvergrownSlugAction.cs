using Game.Action.Internal;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class OvergrownSlugAction : Action, IRelicAction
    {
        private const string EffectName = "effect_poison";

        [MemoryPackConstructor]
        private OvergrownSlugAction()
        {
        }

        public OvergrownSlugAction(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetFrom());
            var caller = GetMaker() as PieceLogic;

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color != caller.Color) continue;
                    var poison = p.Effects.Find(effect => effect.EffectName == EffectName);
                    if (poison != null)
                    {
                        poison.Strength--;
                        if (poison.Strength <= 0) ActionManager.EnqueueAction(new RemoveEffect(poison));
                    }
                }
            }
        }
    }
}