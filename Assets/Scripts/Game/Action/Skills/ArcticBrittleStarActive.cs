using static Game.Common.BoardUtils;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;

namespace Game.Action.Skills
{
    public class ArcticBrittleStarActive : Action, ISkills
    {
        public ArcticBrittleStarActive(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Execute Arctic Brittle Star");
            (int rank, int file) = RankFileOf(Maker);
            for (int x = rank - 1; x <= rank + 1; ++x) {
                for (int y = file - 1; y <= file + 1; ++y) {
                    Formation AnchorIce = new AnchorIce(PieceOn(Maker).Color);
                    AnchorIce.SetDuration(3);
                    
                    FormationManager.Ins.SetFormation(IndexOf(x, y), AnchorIce);
                }
            }
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}