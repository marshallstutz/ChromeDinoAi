using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dino
{
    public class ChromeWrapper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        // the keystroke signals. you can look them up at the msdn pages
        private static uint WM_KEYDOWN = 0x100, WM_KEYUP = 0x101;

        // the reference to the chrome process
        private Process chromeProcess;

        //public ChromeWrapper(string url)
        //{
        //    Process.Start("chrome.exe", url); //no need to keep reference to this process, because if chrome is already opened, this is NOT the correct reference.
        //    Thread.Sleep(600);

        //    Process[] procsChrome = Process.GetProcessesByName("chrome");
        //    foreach (Process chrome in procsChrome)
        //    {
        //        if (chrome.MainWindowHandle == IntPtr.Zero)// the chrome process must have a window
        //            continue;
        //        chromeProcess = chrome; //now you have a handle to the main chrome (either a new one or the one that was already open).
        //        return;
        //    }
        //}

        public void setChromeProcess(Process chrome)
        {
            chromeProcess = chrome;
        }

        public void SendKey(char key)
        {
            try
            {
                if (chromeProcess.MainWindowHandle != IntPtr.Zero)
                {
                    SendMessage(chromeProcess.MainWindowHandle, ChromeWrapper.WM_KEYUP, (IntPtr)(char)40, IntPtr.Zero);
                    SendMessage(chromeProcess.MainWindowHandle, ChromeWrapper.WM_KEYUP, (IntPtr)(char)38, IntPtr.Zero);
                    SendMessage(chromeProcess.MainWindowHandle, ChromeWrapper.WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
                }
            }
            catch (Exception e) //without the GetProcessesByName you'd get an exception.
            {
            }
        }
        public void NoKey()
        {
            try
            {
                if (chromeProcess.MainWindowHandle != IntPtr.Zero)
                {
                    SendMessage(chromeProcess.MainWindowHandle, ChromeWrapper.WM_KEYUP, (IntPtr)(char)40, IntPtr.Zero);
                    SendMessage(chromeProcess.MainWindowHandle, ChromeWrapper.WM_KEYUP, (IntPtr)(char)38, IntPtr.Zero);
                }
            }
            catch (Exception e) //without the GetProcessesByName you'd get an exception.
            {
            }
        }
    }
}
