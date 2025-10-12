using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Action.Skills;
using Game.Relics;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Temperantia: PieceLogic, IRelicCarriable, IPieceWithDoubleSelectionSkill
    {
        public int firstSelection { get; set; }
        public Temperantia(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, TemperantiaMoves.Quiets, TemperantiaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
            UndyingDevotion trait = new UndyingDevotion(this);
            ActionManager.ExecuteImmediately(new ApplyEffect(trait));

            //Chưa chọn mục tiêu nào
            firstSelection = -1;

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                //Selection đầu tiên: chọn quân đồng minh
                if (firstSelection == -1){
                    List<PieceLogic> allies = FindPiece<PieceLogic>(Color);
                    foreach(PieceLogic ally in allies){
                        if (ally.Equals(this)) { continue; }
                        list.Add(new TemperantiaSwap(this.Pos, ally.Pos));
                    }
                    
                }
                else{
                    List<PieceLogic> enemies = FindPiece<PieceLogic>(!Color);
                    foreach (PieceLogic enemy in enemies)
                    {
                        list.Add(new TemperantiaSwap(this.Pos, firstSelection, enemy.Pos));
                    }
                    //Reset trạng thái chọn mục tiêu
                    firstSelection = -1;
                }
                
            };
            CarriedRelic = carriedRelic;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public SkillsDelegate SecondSelection{ get; set;  }
        public RelicLogic CarriedRelic { get; set; }
    }
}