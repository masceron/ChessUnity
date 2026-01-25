using Autotiles3D;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class MirrorButterflyFishPassive : Effect, IAfterPieceActionEffect
    {
        private const int Chance = 100;
        public MirrorButterflyFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_mirror_butterfly_fish_passive")
        {
        }
        

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ISkills skill)
                return;

            var maker = BoardUtils.PieceOn(action.Maker);
            var target = BoardUtils.PieceOn(action.Target);

            if (target != Piece)
                return;

            if (maker == null || maker is not IPieceWithSkill pieceWithSkill || !MatchManager.Roll(Chance))
                return;
            
            // Block this action.
            
            // Mirror skill
            action.Maker = Piece.Pos;
            action.Target = maker.Pos;

        }
    }
}