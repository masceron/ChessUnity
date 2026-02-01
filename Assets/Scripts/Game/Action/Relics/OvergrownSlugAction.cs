using Game.Action.Internal;
using static Game.Common.BoardUtils;
namespace Game.Action.Relics
{
    public class OvergrownSlugAction : Action, IRelicAction
    {
        private readonly string effectName = "effect_poison";
        
        public OvergrownSlugAction(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);
            
            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color != caller.Color) continue;
                    var poison = p.Effects.Find(effect => effect.EffectName == effectName);
                    if (poison != null)
                    {
                        poison.Strength--;
                        if (poison.Strength <= 0)
                        {
                            ActionManager.EnqueueAction(new RemoveEffect(poison));
                        }
                    }
                }
            }
        }
    }
}