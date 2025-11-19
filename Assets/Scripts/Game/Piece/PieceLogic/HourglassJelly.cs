using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJelly : Commons.PieceLogic, IPieceWithSkill
    {
        
        public HourglassJelly(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            // ActionManager.ExecuteImmediately(new ApplyEffect(new HourglassJellyEffect(this)));
            
            Skills = list =>
            {
                if (SkillCooldown != 0) { return; }
                var (rank, file) = BoardUtils.RankFileOf(Pos);

                for (var x = rank - 4; x <= rank + 4; ++x){
                    for (var y = file - 4; y <= file + 4; ++y){
                        var piece = BoardUtils.PieceOn(BoardUtils.IndexOf(x, y));
                        if (piece == null || piece.Equals(this) || piece.PreviousMoves.Count <= 0) continue;
                        list.Add(new HourglassJellyActive(Pos, piece.Pos));
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}