using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.ThalassosResurrector;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosResurrectCandidate : PendingAction, ISkills
    {
        public ThalassosResurrectCandidate(int maker, int pos) : base(maker)
        {
            Maker = maker;
            Target = pos;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void CompleteAction()
        {
            var selector =
                BoardViewer.Ins.GetOrInstantiateUI<ThalassosResurrector>(IngameSubmenus.ThalassosResurrector);
            selector.Load(Maker, Target, this);
        }
    }
}