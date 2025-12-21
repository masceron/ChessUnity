using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThreadPipefish : Commons.PieceLogic, IPieceWithSkill
    {
        public ThreadPipefish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);
                foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 6))
                {
                    var piece = PieceOn(IndexOf(nRank, nFile));
                    
                    if (piece == null || piece.Color != Color) continue;
                    
                    list.Add(new ThreadPipefishActive(Pos, IndexOf(nRank, nFile)));
                }
            };
        }
        
        
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
    
}