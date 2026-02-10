using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Effects.SpecialAbility;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrilledShark : Commons.PieceLogic, IPieceWithSkill
    {
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public FrilledShark(PieceConfig cfg) : base(cfg, KnightMoves.Quiets, KnightMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Sanity(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrilledSharkPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    
                    // 8 directions: N, S, E, W, NE, NW, SE, SW
                    int[] dRank = {-1, 1, 0, 0, -1, -1, 1, 1};
                    int[] dFile = {0, 0, -1, 1, -1, 1, -1, 1};
                    var step = 4;
                    for (var dir = 0; dir < 8; dir++)
                    {
                        var r = rank + dRank[dir] * step;
                        var f = file + dFile[dir] * step;
                        
                        if (!VerifyBounds(r) || !VerifyBounds(f)) continue;
                        
                        var idx = IndexOf(r, f);
                        if (!IsActive(idx)) continue;
                        
                        var piece = PieceOn(idx);
                        // Nếu có quân đứng đúng ở ô lướt thì không lướt được
                        if (piece != null) continue;
                        list.Add(new FrilledSharkActive(Pos, dRank[dir], dFile[dir]));
                    }
                }
            };
        }
    }
}

