using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Search2.Util
{
    public static class MouseHook
    {
        public static event EventHandler<Tuple<MouseButtonState, Point>> MouseLeftButton;
        public static event EventHandler<Tuple<MouseButtonState, Point>> MouseRightButton;
        public static event EventHandler<Point> MouseMove;
        public static event EventHandler<Point> MouseWheel;

        public static void Start()
        {
            _hookId = SetHook(Proc);
        }

        public static void Stop()
        {
            UnhookWindowsHookEx(_hookId);
        }

        private static readonly LowLevelMouseProc Proc = HookCallback;
        private static IntPtr _hookId = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                switch ((MouseMessages)wParam)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                        MouseLeftButton?.Invoke(null, new Tuple<MouseButtonState, Point>(MouseButtonState.Pressed, new Point(hookStruct.pt.x, hookStruct.pt.y)));
                        break;

                    case MouseMessages.WM_LBUTTONUP:
                        MouseLeftButton?.Invoke(null, new Tuple<MouseButtonState, Point>(MouseButtonState.Released, new Point(hookStruct.pt.x, hookStruct.pt.y)));
                        break;

                    case MouseMessages.WM_MOUSEMOVE:
                        MouseMove?.Invoke(null, new Point(hookStruct.pt.x, hookStruct.pt.y));
                        break;

                    case MouseMessages.WM_RBUTTONDOWN:
                        MouseRightButton?.Invoke(null, new Tuple<MouseButtonState, Point>(MouseButtonState.Pressed, new Point(hookStruct.pt.x, hookStruct.pt.y)));
                        break;

                    case MouseMessages.WM_RBUTTONUP:
                        MouseRightButton?.Invoke(null, new Tuple<MouseButtonState, Point>(MouseButtonState.Released, new Point(hookStruct.pt.x, hookStruct.pt.y)));
                        break;

                    case MouseMessages.WM_MOUSEWHEEL:
                        MouseWheel?.Invoke(null, new Point(hookStruct.pt.x, hookStruct.pt.y));
                        break;
                }
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }
}