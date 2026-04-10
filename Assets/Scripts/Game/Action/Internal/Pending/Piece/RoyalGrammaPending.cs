using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal.Pending.Piece
{
    //Làm lại
    public class RoyalGrammaPending : PendingAction, ISkills
    {
        public RoyalGrammaPending(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }
    
        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override UniTask<Action> BuildAction(ITargetingContext context)
        {
            throw new NotImplementedException();
        }
    }
}