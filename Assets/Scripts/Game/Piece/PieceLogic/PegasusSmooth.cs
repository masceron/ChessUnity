using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class PegasusSmooth : Commons.PieceLogic
    {
        public PegasusSmooth(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }
        
        protected override void CustomBehaviors(List<Action.Action> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is IQuiets)
                {
                    list[i] = new PegasusSmoothMove(Pos, list[i].Target);
                }
            }
        }
    }
}