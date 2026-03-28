using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class EeriePresencePassive : Effect, IAfterPieceActionTrigger
    {
        public EeriePresencePassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_eerie_presence_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMaker() != Piece || action is not ICaptures || action.Result != ResultFlag.Success) return;

            var (rank, file) = RankFileOf(action.Target);

            var piece1 = PieceOn(IndexOf(rank, file - 1));
            var piece2 = PieceOn(IndexOf(rank, file + 1));

            if (piece1 != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, piece1), PieceOn(action.Maker)));

            if (piece2 != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, piece2), PieceOn(action.Maker)));
        }
    }
}