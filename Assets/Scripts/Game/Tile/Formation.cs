using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public enum FormationCategory
    {
        Positive,
        Negative,
        Miscellaneous,

    }
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
        NavalMines,
        SiltCloud,
        HydroidThicket,
    }

    public abstract class Formation
    {
        public int Pos { get; private set; }
        protected readonly bool Color;
        public PieceLogic PieceOnFormation { get; protected set; }
        public bool HaveDuration { get; protected set; }
        public int Duration { get; protected set; }

        protected Formation() {}
        protected Formation(bool color)
        {
            Color = color;
        }
        /// <summary>
        /// Hiện hàm này được gọi chủ động bởi FormationManager::SetFormation() nên mọi người không cần phải động tới
        /// </summary>
        /// <param name="index"></param>
        public void SetPositon(int index)
        {
            Pos = index;
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
        /// <summary>
        /// Được gọi 1 lần duy nhất sau khi tạo Formation, thường dùng để gây effect lên quân đứng sẵn trên đó
        /// </summary>
        /// <param name="piece"></param>
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


        public virtual int GetValueForAI(){return 0;}
    }
}