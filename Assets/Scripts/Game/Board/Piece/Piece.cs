using UnityEngine;
using static Game.Common.BoardUtils;

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
        Archelon,
        Thalassos,
        Pufferfish,
        Swordfish,
        Lionfish,
        MorayEel,
        Stingray,
        Seahorse
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
        Commander,
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piece : MonoBehaviour
    {
        private int rank;
        private int file;
        private bool color;
        
        public void Spawn(int pos, bool c)
        {
            rank = RankOf(pos);
            file = FileOf(pos);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            transform.position = new Vector3(rank, 1.64f, file);
            
            var angles = new Quaternion
            {
                eulerAngles = !c
                    ? new Vector3(-90, 0, 0)
                    : new Vector3(-90, 0, 180)
            };

            transform.rotation = angles;
        }
        
        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;
            
            transform.position = new Vector3(rank, transform.position.y, file);
        }
    }
}
