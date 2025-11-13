using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
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
        private int radius = 2;
        private bool isApplied = false;
        public BioluminescentBeaconPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.BioluminescentBeaconPassive)
        {
            isApplied = false;
            HandlePassive();
        }

        public List<int> GetPosInRadius()
        {
            List<int> positions = new List<int>();
            var (rank, file) = BoardUtils.RankFileOf(Piece.Pos);
            for (int i = rank - radius; i <= rank + radius; i++)
            {
                for (int j = file - radius; j <= file + radius; j++)
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

        public void HandlePassive()
        {
            if (isApplied) return;

            var posInRadius = GetPosInRadius();

            DazzlingLight dazzlingLight = new DazzlingLight(false, Piece.Color);

            foreach (var pos in posInRadius)
            {
                if (FormationManager.Ins.GetFormation(pos) != null) continue;
                FormationManager.Ins.SetFormation(pos, dazzlingLight);
                if (BoardUtils.PieceOn(pos) != null)
                {
                    FormationManager.Ins.TriggerEnter(pos);
                }
            }
            isApplied = true;
        }
    }
}

