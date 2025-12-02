using Game.Effects;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public enum FormationType
    {
        None,
        FogOfWar,
        BubbleVent,
        AnchorIce,
        DazzlingLight,
        UrchinField,
        Saprolegnia,
        Kelp,
        PredatorLair,
        NavalMines
    }

    public abstract class Formation
    {
        // public int pos{ get; private set; }
        protected readonly bool Color;
        protected PieceLogic PieceOnFormation { get; private set; }
        public bool HaveDuration { get; protected set; }
        public int Duration { get; protected set; }

        protected Formation(bool color)
        {
            Color = color;
        }

        public void SetDuration(int _duration)
        {
            Duration = _duration;
            HaveDuration = true;
        }
        public bool GetColor() => Color;
        /// <summary>
        /// Trả về FormationType tương ứng với class
        /// </summary>
        public abstract FormationType GetFormationType();

        public virtual void OnCreated(PieceLogic piece)
        {
            OnPieceEnter(piece);
        }
        public virtual void OnDestroyed(PieceLogic piece)
        {
            OnPieceExit(piece);
        }
        /// <summary>
        /// Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân đi vào vị trí
        /// </summary>
        public virtual void OnPieceEnter(PieceLogic piece)
        {
            PieceOnFormation = piece;
        }
        /// <summary>
        /// Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân rời khỏi vị trí
        /// </summary>
        public virtual void OnPieceExit(PieceLogic piece)
        {
            PieceOnFormation = null;
        }

        public virtual void OnFirstTurn(PieceLogic piece)
        {
            
        }

        protected bool ApplyEffect(PieceLogic piece, Effect effect)
        {
            if (piece.IsImmuneTo(this, effect)) {
                return false;
            }
            ActionManager.EnqueueAction(new ApplyEffect(effect));
            return true;
        }

        public virtual int GetValueForAI(){return 0;}
    }
}