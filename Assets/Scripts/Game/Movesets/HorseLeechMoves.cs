using System.Collections.Generic;
using Game.Action.Captures;
using Game.Common;
using static Game.Common.BoardUtils;


namespace Game.Movesets
{
    public static class HorseLeechMoves
    {
        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            var (rank, file) = RankFileOf(pos);
            var p = PieceOn(pos);
            var color = p.Color;

            var attackRange = p.GetAttackRange();

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
                MakeCapture(rankOff, fileOff);

            void MakeCapture(int rankOff, int fileOff)
            {
                var index = IndexOf(rankOff, fileOff);
                var piece = PieceOn(index);
                if (piece == null && !isPlayer)
                {
                    list.Add(new HorseLeechAttack(p, piece));
                }
                else if (piece != null)
                {
                    if (piece.Color == color ||
                        Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
                        return;
                    list.Add(new HorseLeechAttack(p, piece));
                }
            }

            return 10 + 10 * attackRange;
        }
    }
}