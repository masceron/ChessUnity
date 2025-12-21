using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;

namespace Game.Effects.Traits
{
    /// <summary>
    /// Bioluminescent Beacon Passive Effect
    /// 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class BioluminescentBeaconPassive : Effect
    {
        private readonly int radius = 2;
        private bool isApplied;
        public BioluminescentBeaconPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bioluminescent_beacon_passive")
        {
            isApplied = false;
            HandlePassive();
        }

        private List<int> GetPosInRadius()
        {
            var positions = new List<int>();
            var (rank, file) = BoardUtils.RankFileOf(Piece.Pos);
            for (var i = rank - radius; i <= rank + radius; i++)
            {
                for (var j = file - radius; j <= file + radius; j++)
                {
                    if (i == rank && j == file) continue;
                    if (BoardUtils.VerifyBounds(i)
                        && BoardUtils.VerifyBounds(j)
                        && BoardUtils.IsActive(BoardUtils.IndexOf(i, j)))
                    {
                        positions.Add(BoardUtils.IndexOf(i, j));
                    }
                }
            }
            return positions;
        }

        private void HandlePassive()
        {
            if (isApplied) return;

            var posInRadius = GetPosInRadius();

            var dazzlingLight = new DazzlingLight(false, Piece.Color);

            foreach (var pos in posInRadius)
            {
                if (FormationManager.Ins.GetFormation(pos) != null) continue;
                FormationManager.Ins.SetFormation(pos, dazzlingLight);
                // Trigger không cần thiết, hàm TriggerEnter được gọi tự động
                if (BoardUtils.PieceOn(pos) != null)
                {
                    FormationManager.Ins.TriggerEnter(pos);
                }
            }
            isApplied = true;
        }
    }
}

