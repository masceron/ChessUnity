using Game.Action.Internal.Pending.Piece;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class BlackcapBasslet : PieceLogic.Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const string BlindedEffectName = "effect_blinded";

        public BlackcapBasslet(PieceConfig cfg) : base(cfg, PawnPushMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var blindEnemies = FindPiecesWithEffectName(!Color, BlindedEffectName);
                    foreach (var enemy in blindEnemies)
                    {
                        var (rank, file) = RankFileOf(enemy.Pos);
                        var index = IndexOf(rank, file);
                        //list.Add(new BlackcapBassletPending(this, index));
                        //Làm lại
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        
                    }
                    else
                    {
                        
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}