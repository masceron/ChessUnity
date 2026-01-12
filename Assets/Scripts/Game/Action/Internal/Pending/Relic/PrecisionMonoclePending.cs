using System.Linq;
using Game.Action.Relics;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class PrecisionMonoclePending : Action, System.IDisposable, IRelicAction
    {
        private PrecisionMonocle precisionMonocle;
        private const int EvasionProbabilityDecrease = 5;
        public PrecisionMonoclePending(PrecisionMonocle pm, int maker, bool pos = false) : base(maker)
        {
            precisionMonocle = pm;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var allyColor = BoardUtils.PieceOn(Maker).Color;
            var markedEnemy = BoardUtils.FindPiecesWithEffectName(!allyColor, "effect_marked");
            var markedAlly = BoardUtils.FindPiecesWithEffectName(allyColor, "effect_marked");

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
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            precisionMonocle.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
            Dispose();
        }

        public void Dispose()
        {
            precisionMonocle = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}