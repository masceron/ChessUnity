using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Grenadiers : Commons.PieceLogic, IPieceWithSkill
    {
        public Grenadiers(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, 50, this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 3))
                    {
                        var startingSizeX = (MaxLength - MatchManager.Ins.StartingSize.x) / 2;
                        var startingSizeY = (MaxLength - MatchManager.Ins.StartingSize.y) / 2;

                        if (file < startingSizeX || file >= startingSizeX + MatchManager.Ins.StartingSize.x
                             || rank < startingSizeY || rank >= startingSizeY + MatchManager.Ins.StartingSize.y
                                || TileManager.Ins.IsTileEmpty(IndexOf(rank, file))) continue;
                        list.Add(new GrenadiersActive(Pos, IndexOf(rank, file)));
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}