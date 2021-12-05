using System;
using System.Runtime.InteropServices;

namespace Kutuphane.ViewModel
{
    public static class IdleTimeFinder
    {
        public static long GetLastInputTime()
        {
            LASTINPUTINFO lastInPut = new();
            lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
            if (!GetLastInputInfo(ref lastInPut))
            {
                throw new Exception(GetLastError().ToString());
            }

            return lastInPut.dwTime;
        }

        public static long IdleTimeSecond()
        {
            LASTINPUTINFO lastinputinfo = new();
            lastinputinfo.cbSize = (uint)Marshal.SizeOf(lastinputinfo);
            GetLastInputInfo(ref lastinputinfo);
            return (((Environment.TickCount & int.MaxValue) - (lastinputinfo.dwTime & int.MaxValue)) & int.MaxValue) / 1000;
        }

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    }

    internal struct LASTINPUTINFO
    {
        public uint cbSize;

        public uint dwTime;
    }
}