using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class ClownFishPassive : Effect, IEndTurnTrigger
    {
        private static readonly (int, int)[] DistanceToAnotherPiece =
        {
            (2, 0), (2, 2), (0, 2), (-2, 2),
            (-2, 0), (-2, -2), (0, -2), (2, -2)
        };

        public ClownFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_clown_fish_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var rank = RankOf(Piece.Pos);
            var file = FileOf(Piece.Pos);

            for (var i = 0; i < DistanceToAnotherPiece.Length; i++)
            {
                var anotherPiecePos = IndexOf(rank + DistanceToAnotherPiece[i].Item1,
                    file + DistanceToAnotherPiece[i].Item2);
                var anotherPiece = PieceOn(anotherPiecePos);

                if (anotherPiece is not { Type: "piece_clown_fish" }) continue;
                var middlePiecePos = IndexOf(rank + DistanceToAnotherPiece[i].Item1 / 2,
                    file + DistanceToAnotherPiece[i].Item2 / 2);
                var middlePiece = PieceOn(middlePiecePos);

                if (middlePiece != null && middlePiece.Color != Piece.Color)
                    ActionManager.EnqueueAction(new KillPiece(Piece, middlePiece));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}