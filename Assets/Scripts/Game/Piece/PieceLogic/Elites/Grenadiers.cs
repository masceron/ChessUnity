using Game.Action;
using Game.Action.Internal;
using Game.Movesets;
using Game.Effects.Debuffs;
using Game.Action.Skills;
using Game.Managers;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Grenadiers : PieceLogic, IPieceWithSkill
    {
        public Grenadiers(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(-1, 50, this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 3))
                {
                    int startingSizeX = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.x) / 2;
                    int startingSizeY = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.y) / 2;
                    
                    if(file < startingSizeX || file >= startingSizeX + MatchManager.Ins.startingSize.x
                         || rank < startingSizeY || rank >= startingSizeY + MatchManager.Ins.startingSize.y 
                            || TileManager.Ins.IsTileEmpty(IndexOf(rank, file))) continue;
                    list.Add(new GrenadiersActive(Pos, IndexOf(rank, file)));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}