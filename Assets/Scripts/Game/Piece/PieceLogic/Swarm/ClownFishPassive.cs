using Game.Effects;
using Game.Action;
using Game.Action.Internal;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Swarm
{
    public class ClownFishPassive : Effect, IEndTurnEffect
    {   
        public EndTurnEffectType EndTurnEffectType { get; }
        private readonly (int, int)[] distanceToAnotherPiece = new (int, int)[8]
        {
            (2, 0), (2, 2), (0, 2), (-2, 2),
            (-2, 0), (-2, -2), (0, -2), (2, -2)
        };
        public ClownFishPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.ClownFishPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }
        
        public void OnCallEnd(Action.Action lastMainAction)
        {
            var rank = RankOf(Piece.Pos);
            var file = FileOf(Piece.Pos);

            for (var i = 0; i < distanceToAnotherPiece.Length; i++)
            {
                var anotherPiecePos = IndexOf(rank + distanceToAnotherPiece[i].Item1, file + distanceToAnotherPiece[i].Item2);
                var anotherPiece = PieceOn(anotherPiecePos);

                if (anotherPiece is { Type: PieceType.ClownFish })
                {
                    var middlePiecePos = IndexOf(rank + distanceToAnotherPiece[i].Item1 / 2,
                        file + distanceToAnotherPiece[i].Item2 / 2);
                    var middlePiece = PieceOn(middlePiecePos);

                    if (middlePiece != null && middlePiece.Color != Piece.Color)
                    {
                        ActionManager.EnqueueAction(new KillPiece(middlePiecePos));
                    }
                }

            }
        }
    }
}