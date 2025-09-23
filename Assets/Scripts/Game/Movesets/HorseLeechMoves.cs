using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;


namespace Game.Movesets
{
    public static class HorseLeechMoves
    {
        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var p = PieceOn(pos);
            var color = p.Color;

            var attackRange = p.AttackRange;

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
            {
                MakeCapture(rankOff, fileOff);
            }
            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                if (piece == null || 
                    piece.Color == color || 
                    Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                    return;
                
                list.Add(new HorseLeechAttack(pos, index));
            }
        }
    }

}