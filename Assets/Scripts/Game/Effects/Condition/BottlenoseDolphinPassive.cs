using Game.Managers;
using Game.Action.Internal;
using Game.Action;
using Game.Effects.Traits;
using System.Linq;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinPassive: Effect, IEndTurnEffect
    {
        private bool surpassed, evaded;

        public BottlenoseDolphinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bottlenose_dolphin_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            surpassed = !MatchManager.Ins.GameState.IsDay && Piece.Effects.All(e => e.EffectName != "effect_surpass");
            evaded = !MatchManager.Ins.GameState.IsDay && Piece.Effects.All(e => e.EffectName != "effect_evasion");
            if (!surpassed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
                surpassed = true;
            }
            if (!evaded)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, Piece)));
                evaded = true;
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (MatchManager.Ins.GameState.IsDay && !surpassed) 
            {
                surpassed = true;
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
            }
            if (MatchManager.Ins.GameState.IsDay && !evaded)
            {
                evaded = true;
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, Piece)));
            }

            if (!MatchManager.Ins.GameState.IsDay)
            {
                var surpassEffect = Piece.Effects.Find(e => e.EffectName == "effect_surpass");
                if (surpassEffect != null)
                    ActionManager.EnqueueAction(new RemoveEffect(surpassEffect));
                    
                var evasionEffect = Piece.Effects.Find(e => e.EffectName == "effect_evasion");
                if (evasionEffect != null)
                    ActionManager.EnqueueAction(new RemoveEffect(evasionEffect));
                    
                surpassed = false;
                evaded = false;
            }


        }

    }
}