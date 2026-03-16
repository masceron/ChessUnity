using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
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
                        foreach (var (rankOf, fileOf) in MoveEnumerators.AroundUntil(rank, file, 1))
                        {
                            var index = IndexOf(rankOf, fileOf);
                            if (!IsActive(index)) continue;
                            list.Add(new BlackcapBassletActive(Pos, index));
                        }
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