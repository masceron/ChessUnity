using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class EeriePresencePassive : Effect
    {
        public EeriePresencePassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_eerie_presence_passive")
        { }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;
            if (action.Result != ResultFlag.Success) return;
            
            var (rank, file) = RankFileOf(action.Target);

            var piece1 = PieceOn(IndexOf(rank, file - 1));
            var piece2 = PieceOn(IndexOf(rank, file + 1));

            if (piece1 != null)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, piece1)));
            }
            
            if (piece2 != null)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, piece2)));
            }
        }
    }
}