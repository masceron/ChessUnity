using Game.Piece.PieceLogic;
using Game.Effects.Condition;
using Game.Managers;
using Game.Action.Internal;
using Game.Action;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Effects.Debuffs;
using System.Linq;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinPassive: Effect, IEndTurnEffect
    {
        private bool Surpassed, Evaded;

        public BottlenoseDolphinPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.BottlenoseDolphinPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            Surpassed = !MatchManager.Ins.GameState.IsDay && !Piece.Effects.Any(e => e.EffectName == EffectName.Surpass);
            Evaded = !MatchManager.Ins.GameState.IsDay && !Piece.Effects.Any(e => e.EffectName == EffectName.Evasion);
            if (!Surpassed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
                Surpassed = true;
            }
            if (!Evaded)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, Piece)));
                Evaded = true;
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (MatchManager.Ins.GameState.IsDay && !Surpassed) 
            {
                Surpassed = true;
                ActionManager.EnqueueAction(new ApplyEffect(new Surpass(Piece)));
            }
            if (MatchManager.Ins.GameState.IsDay && !Evaded)
            {
                Evaded = true;
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, Piece)));
            }

            if (!MatchManager.Ins.GameState.IsDay)
            {
                var surpassEffect = Piece.Effects.Find(e => e.EffectName == EffectName.Surpass);
                if (surpassEffect != null)
                    ActionManager.EnqueueAction(new RemoveEffect(surpassEffect));
                    
                var evasionEffect = Piece.Effects.Find(e => e.EffectName == EffectName.Evasion);
                if (evasionEffect != null)
                    ActionManager.EnqueueAction(new RemoveEffect(evasionEffect));
                    
                Surpassed = false;
                Evaded = false;
            }


        }

    }
}