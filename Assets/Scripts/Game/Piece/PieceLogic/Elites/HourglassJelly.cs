using UnityEngine;
using Game.Action.Skills;
using Game.Movesets;
using Game.Common;
using Game.Managers;
namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJelly : PieceLogic, IPieceWithSkill
    {
        
        public HourglassJelly(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            // ActionManager.ExecuteImmediately(new ApplyEffect(new HourglassJellyEffect(this)));
            
            Skills = list =>
            {
                if (SkillCooldown != 0) { return; }
                (int rank, int file) = BoardUtils.RankFileOf(Pos);

                for (int x = rank - 4; x <= rank + 4; ++x){
                    for (int y = file - 4; y <= file + 4; ++y){
                        PieceLogic piece = BoardUtils.PieceOn(BoardUtils.IndexOf(x, y));
                        if (piece != null && piece.Equals(this) == false && piece.PreviousMoves.Count > 0){
                            Debug.Log("Add new candidate");
                            // Debug.Log(PieceManager.Ins.)
                            list.Add(new HourglassJellyActive(Pos, piece.Pos));
                        }
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}