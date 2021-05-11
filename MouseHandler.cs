using System.Runtime.InteropServices;

namespace SimplzBot
{
    public class MouseHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        public static void DoLeftMouseClick()
        {
            mouse_event(Constants.MOUSEEVENTF_LEFTDOWN | Constants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void DoRightMouseClick()
        {
            mouse_event(Constants.MOUSEEVENTF_RIGHTDOWN | Constants.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        public static void MoveCursorToPoint(int x, int y)
        {
            SetCursorPos(x, y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static POINT GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }
    }
}
