using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Chrysos : Commons.PieceLogic, IPieceWithSkill
    {
        public int Coin = 10;

        public Chrysos(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlayersCoin(this)));
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var pieceBoard = PieceBoard();
                    for (var i = 0; i < BoardSize; i++)
                    {
                        var piece = pieceBoard[i];
                        if (piece == null || piece.Color != Color) continue;

                        var upgradableTo = UpgradableTo(piece.PieceRank);
                        if (upgradableTo.Item1 == PieceRank.None) continue;
                        
                        if (Coin >= upgradableTo.Item2) list.Add(new ChrysosUpgradeCandidate(this, piece));
                    }
                }
                // query for AI
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }

        public static (PieceRank, int) UpgradableTo(PieceRank from)
        {
            return from switch
            {
                PieceRank.Swarm => (PieceRank.Common, 1),
                PieceRank.Common => (PieceRank.Elite, 3),
                PieceRank.Elite => (PieceRank.Champion, 5),
                PieceRank.Champion => (PieceRank.Champion, 6),
                _ => (PieceRank.None, 0)
            };
            
        }
    }
}