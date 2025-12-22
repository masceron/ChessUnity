using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class LongSnoutedSeahorse : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public LongSnoutedSeahorse(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            //TODO: Change quite and capture movesets
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this || pOn.Effects.All(effect => effect.EffectName != "effect_bound")) continue;
                        list.Add(new LongSnoutedSeahorseActive(Pos, index));
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
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Pos == Pos || !IsActive(IndexOf(rankOff, fileOff))) continue;
                            list.Add(new LongSnoutedSeahorseActive(Pos, index));
                        }
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}