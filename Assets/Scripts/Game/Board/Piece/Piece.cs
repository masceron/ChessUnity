using UnityEngine;

namespace Game.Board.Piece
{
    public enum PieceType : sbyte
    {
        Velkaris,
        GuidingSiren,
        Barracuda,
        SeaUrchin,
        ElectricEel,
        FlyingFish,
        Chrysos,
        Anomalocaris,
        Archelon
    }

    public enum PieceRank : byte
    {
        None,
        Construct,
        Summoned,
        Swarm,
        Common,
        Elite,
        Champion,
        Commander
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piece : MonoBehaviour
    {
        private int rank;
        private int file;
        
        public void Spawn(int r, int f, Vector3 defaultTransform)
        {
            rank = r;
            file = f;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            transform.position = new Vector3(rank, defaultTransform.y, file);
        }
        
        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;
            
            transform.position = new Vector3(rank, transform.position.y, file);
        }
    }
}
