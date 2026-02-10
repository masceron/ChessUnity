using static Game.Common.BoardUtils;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;

namespace Game.Action.Skills
{
    public class ArcticBrittleStarActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -5;
            return 0;
        }
        public ArcticBrittleStarActive(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Execute Arctic Brittle Star");
            Formation AnchorIce = new AnchorIce(PieceOn(Maker).Color);
            AnchorIce.SetDuration(3);
            FormationManager.Ins.SetFormation(Target, AnchorIce);
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}