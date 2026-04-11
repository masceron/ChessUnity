using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class RustyParrotfish : Commons.PieceLogic, IPieceWithSkill
    {
        public RustyParrotfish(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new RustyParrotfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                    for (var i = 0; i < BoardUtils.BoardSize; ++i)
                    {
                        var piece = BoardUtils.PieceOn(i);
                        if (piece == null || piece.Color == Color) continue;
                        if (!BoardUtils.HasFormation(i)) continue;

                        //Làm lại
                        //list.Add(new RustyParrotfishActive(Pos, piece.Pos));
                    }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; set; }
    }
}