
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
        private SirensHarpoon _sirensHarpoon;
        public SirensHarpoonPending(SirensHarpoon s, int maker, bool pos = false) : base(maker, pos)
        {
            _sirensHarpoon = s;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Controlled(1, BoardUtils.PieceOn(Maker))));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Pacified(BoardUtils.PieceOn(Maker))));
            
            _sirensHarpoon.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
        

        public void Dispose()
        {
            _sirensHarpoon = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }
    }

}
