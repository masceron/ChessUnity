using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class RibbonEel : Commons.PieceLogic, IPieceWithSkill
    {
        public RibbonEel(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(-1,this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Diurnal(-1, 1, this, "effect_diurnal")));

            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                    for (var i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        var p = BoardUtils.PieceOn(i);
                        if (p == null || p.Color == Color) continue;
                        //Làm lại
                        //list.Add(new DiurnalPending(Pos, p.Pos));
                    }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}