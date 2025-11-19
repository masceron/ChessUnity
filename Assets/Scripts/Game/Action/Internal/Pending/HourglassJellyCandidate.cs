using Game.Action.Skills;
using Game.Common;
using Game.Piece;

namespace Game.Action.Internal.Pending
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJellyCandiate: Action, IPendingAble, IInternal, ISkills
    {
        private PieceConfig config;

        
        public HourglassJellyCandiate(int maker, int to, int cost) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            var cr = BoardUtils.PieceOn(to);

        }

        public void CompleteAction()
        {
            // Di chuyển quân địch về vị trí ban đầu 
        }

        protected override void ModifyGameState()
        {}
    }
}