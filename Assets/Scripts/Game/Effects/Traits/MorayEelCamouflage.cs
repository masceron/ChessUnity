using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MorayEelCamouflage : Effect, IEndTurnTrigger
    {
        private Camouflage _already;

        public MorayEelCamouflage(PieceLogic piece) : base(-1, 1, piece, "effect_moray_eel_camouflage")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var rank = RankOf(Piece.Pos);
            var file = FileOf(Piece.Pos);

            if (rank >= 1)
            {
                var idx = IndexOf(rank - 1, file);
                if (!IsActive(idx))
                {
                    if (_already != null) return;

                    _already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(_already));
                    return;
                }
            }

            if (rank < MaxLength - 1)
            {
                var idx = IndexOf(rank + 1, file);
                if (!IsActive(idx))
                {
                    if (_already != null) return;

                    _already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(_already));
                    return;
                }
            }

            if (file >= 1)
            {
                var idx = IndexOf(rank, file - 1);
                if (!IsActive(idx))
                {
                    if (_already != null) return;

                    _already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(_already));
                    return;
                }
            }

            if (file < MaxLength - 1)
            {
                var idx = IndexOf(rank, file + 1);
                if (!IsActive(idx))
                {
                    if (_already != null) return;

                    _already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(_already));
                    return;
                }
            }

            if (_already == null) return;

            ActionManager.EnqueueAction(new RemoveEffect(_already));
            _already = null;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 25;
        }
    }
}