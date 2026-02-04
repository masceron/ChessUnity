using Game.Common;
using Game.Effects.Traits;
using ZLinq;

namespace Game.Action.Relics
{
    public class PrecisionMonocleAction : Action, IRelicAction
    {
        private const int EvasionProbabilityDecrease = 5;
        private readonly string effectName = "effect_marked";
        
        public PrecisionMonocleAction(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var allyColor = BoardUtils.PieceOn(Maker).Color;
            var markedEnemy = BoardUtils.FindPiecesWithEffectName(!allyColor, effectName);
            var markedAlly = BoardUtils.FindPiecesWithEffectName(allyColor, effectName);

            foreach (var enemy in markedEnemy)
            {
                Evasion evasion = enemy.Effects.OfType<Evasion>().FirstOrDefault();
                if (evasion != null)
                {
                    evasion.Probability -= EvasionProbabilityDecrease;
                    if (evasion.Probability < 0) evasion.Probability = 0;
                }
            }
            
            foreach (var ally in markedAlly)
            {
                Evasion evasion = ally.Effects.OfType<Evasion>().FirstOrDefault();
                if (evasion != null)
                {
                    evasion.Probability -= EvasionProbabilityDecrease;
                    if (evasion.Probability < 0) evasion.Probability = 0;
                }
            }
        }
    }
}