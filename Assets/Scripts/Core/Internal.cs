using System.Runtime.InteropServices;

namespace Core
{
    public class Internal
    {
        [DllImport("libInternal")]
        private static extern int hello();

        public int InternalHello()
        {
            return hello();
        }
    }
}
