using Game.Tile;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Quiets;
using Game.Action.Internal;

namespace Game.Action.Skills
{
    public class CutthroatEelActive  : Action, ISkills
    {
        Bleeding bleeding;
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public CutthroatEelActive(int maker, int target, Bleeding effect) : base(maker)
        {
            Target = target;
            bleeding = effect;
        }
        protected override void ModifyGameState()
        {
            PieceLogic makerPiece = PieceOn(Maker);
            int direction = PieceOn(Maker).Color ? 1 : -1;
            ActionManager.EnqueueAction(new NormalMove(Maker, IndexOf(RankOf(Target) + direction, FileOf(Target))));
            if (bleeding.Strength >= 4)
            {
                bleeding.Strength -= 1;
            }
            else
            {
                ActionManager.EnqueueAction(new KillPiece(Target));
                SetFormation(makerPiece.Pos, new FogOfWar(makerPiece.Color));
            }
        }
        
    }
}