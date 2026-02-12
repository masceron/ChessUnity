using Game.Common;
using Game.Effects.Traits;
using ZLinq;

namespace Game.Action.Relics
{
    public class PrecisionMonocleAction : Action, IRelicAction
    {
        private const int EvasionProbabilityDecrease = 5;
        private const string EffectName = "effect_marked";

        public PrecisionMonocleAction(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var allyColor = BoardUtils.PieceOn(Maker).Color;
            var markedEnemy = BoardUtils.FindPiecesWithEffectName(!allyColor, EffectName);
            var markedAlly = BoardUtils.FindPiecesWithEffectName(allyColor, EffectName);

            foreach (var evasion in markedEnemy.Select(enemy => enemy.Effects.OfType<Evasion>().FirstOrDefault()).Where(evasion => evasion != null))
            {
                evasion.Strength -= EvasionProbabilityDecrease;
                if (evasion.Strength < 0) evasion.Strength = 0;
            }
            
            foreach (var evasion in markedAlly.Select(ally => ally.Effects.OfType<Evasion>().FirstOrDefault()).Where(evasion => evasion != null))
            {
                evasion.Strength -= EvasionProbabilityDecrease;
                if (evasion.Strength < 0) evasion.Strength = 0;
            }
        }
    }
}