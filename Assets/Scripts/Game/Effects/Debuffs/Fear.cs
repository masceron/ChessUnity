using Game.Action;
using Game.Action.Quiets;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Fear: Effect, IEndTurnEffect
    {
        public Fear(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Fear)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }
        
        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            var piece = PieceOn(Piece.Pos);
            var color = piece.Color;
            var push = color ? -1 : 1;
            var rankOff = rank;
            
            while (rankOff + push != rank + push * 2) {
            
                var curPos = IndexOf(rankOff + push, file);
                if (!VerifyIndex(curPos) || !IsActive(curPos) || PieceOn(curPos) != null)
                {
                    return;
                }

                rankOff += push;
            }
            
            ActionManager.EnqueueAction(new NormalMove(Piece.Pos, IndexOf(rankOff, file)));
            
            
        }
        
        
    }
}