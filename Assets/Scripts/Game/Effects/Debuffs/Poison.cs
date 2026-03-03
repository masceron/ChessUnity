using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Poison : Effect, IEndTurnTrigger
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TimeLeft = 3;
        public byte Stack = 5;

        public Poison(int strength, PieceLogic piece) : base(-1, strength, piece, "effect_poison")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (Strength >= Stack) TimeLeft--;
            if (TimeLeft <= 0) ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Debuff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80 - Strength * 10;
        }
    }
}