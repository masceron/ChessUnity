using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingGurnard : Commons.PieceLogic, IPieceWithSkill
    {
        public FlyingGurnard(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        { 
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var push = Color ? 1 : -1;
                    
                    var frontRank = rank + push;
                    if (VerifyBounds(frontRank))
                    {
                        var frontIndex = IndexOf(frontRank, file);
                        if (VerifyIndex(frontIndex))
                        {
                            list.Add(new FlyingGurnardActive(Pos, frontIndex));
                        }
                    }
                    
                    var backRank = rank - push;
                    if (VerifyBounds(backRank))
                    {
                        var backIndex = IndexOf(backRank, file);
                        if (VerifyIndex(backIndex))
                        {
                            list.Add(new FlyingGurnardActive(Pos, backIndex));
                        }
                    }
                    
                    var leftFile = file - 1;
                    if (VerifyBounds(leftFile))
                    {
                        var leftIndex = IndexOf(rank, leftFile);
                        if (VerifyIndex(leftIndex))
                        {
                            list.Add(new FlyingGurnardActive(Pos, leftIndex));
                        }
                    }
                    
                    var rightFile = file + 1;
                    if (VerifyBounds(rightFile))
                    {
                        var rightIndex = IndexOf(rank, rightFile);
                        if (VerifyIndex(rightIndex))
                        {
                            list.Add(new FlyingGurnardActive(Pos, rightIndex));
                        }
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