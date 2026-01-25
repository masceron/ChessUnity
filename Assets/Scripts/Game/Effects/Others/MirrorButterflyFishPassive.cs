using Autotiles3D;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class MirrorButterflyFishPassive : Effect, IBeforePieceActionEffect
    {
        private const int Chance = 25;
        public MirrorButterflyFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_mirror_butterfly_fish_passive")
        {
        }
        

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ISkills skill)
                return;

            var maker = BoardUtils.PieceOn(action.Maker);
            var target = BoardUtils.PieceOn(action.Target);

            if (target != Piece)
                return;

            if (maker == null || !MatchManager.Roll(Chance))
                return;
            
            action.Target = maker.Pos;

        }
    }
}