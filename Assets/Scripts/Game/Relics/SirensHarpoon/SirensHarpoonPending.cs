
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Relics.SirensHarpoon
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : Action.Action, IPendingAble, System.IDisposable
    {
        public static PieceLogic FirstTarget;
        private SirensHarpoon _sirensHarpoon;
        public SirensHarpoonPending(SirensHarpoon s, int maker, bool pos = false) : base(maker, pos)
        {
            _sirensHarpoon = s;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null)
            {
                FirstTarget = hovering;
                TileManager.Ins.Select(FirstTarget.Pos);
                ActionManager.ExecuteImmediately(new ApplyEffect(new Controlled(2, FirstTarget)));
                TileManager.Ins.UnmarkAll();
                
                if (BoardViewer.SelectingFunction == 0)
                {
                    BoardViewer.Selecting = -1;
                    _sirensHarpoon.SetCooldown();
                    MatchManager.Ins.InputProcessor.UpdateRelic();
                    ResetTargets();
                }
            }
        }

        
        private static void ResetTargets()
        {
            FirstTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();

            _sirensHarpoon = null;

            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }

}
