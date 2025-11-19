using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Chrysos: Commons.PieceLogic, IPieceWithSkill
    {
        public byte Coin = 10;

        public Chrysos(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlayersCoin(this)));
            Skills = list =>
            {
                if (SkillCooldown > 0) return;
                var pieceBoard = PieceBoard();
                for (var i = 0; i < BoardSize; i++)
                {
                    var piece = pieceBoard[i];
                    if (piece == null || piece.Color != Color) continue;
                
                    var upgradableTo = UpgradableTo(piece.PieceRank);
                    if (upgradableTo == PieceRank.None) continue;
                
                    var cost = CalculateCost(piece.PieceRank, upgradableTo);
                    if (Coin >= cost)
                    {
                        list.Add(new ChrysosUpgradeCandidate(Pos, i, cost));
                    }
                }
            };
        }

        public static PieceRank UpgradableTo(PieceRank from)
        {
            return from switch
            {
                PieceRank.Swarm => PieceRank.Common,
                PieceRank.Common => PieceRank.Elite,
                PieceRank.Elite or PieceRank.Champion => PieceRank.Champion,
                _ => PieceRank.None
            };
        }

        private static int CalculateCost(PieceRank from, PieceRank to)
        {
            switch (to)
            {
                case PieceRank.Common:
                    return 1;
                case PieceRank.Elite:
                    return 3;
                case PieceRank.Champion:
                    return from switch
                    {
                        PieceRank.Elite => 5,
                        PieceRank.Champion => 6,
                        _ => -1
                    };
                case PieceRank.Commander:
                case PieceRank.Construct:
                case PieceRank.Summoned:
                case PieceRank.None:
                case PieceRank.Swarm:
                default:
                    break;
            }

            return -1;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}