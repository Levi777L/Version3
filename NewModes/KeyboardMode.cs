using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UnityEngine;

public class KeyboardMode : IMode
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    public static void Hook() {
        _hookID = SetHook(_proc);
    }

    public static void Unhook() {
        UnhookWindowsHookEx(_hookID);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static bool active = false;
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (active) {
                string s = ((Keys)vkCode).ToString().ToLower();
                switch (s) {
                    case "space":
                        keyboard.Space();
                        break;
                    case "back":
                        keyboard.BackSpace();
                        break;
                    case "return":
                        manager.keyboardString = keyboard.Enter();
                        break;
                    case "oemperiod":
                        keyboard.AddChar(".");
                        break;
                    case "oemquestion":
                        keyboard.AddChar("?");
                        break;
                    case "d1":
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            keyboard.AddChar("!");
                        else
                            keyboard.AddChar("1");
                        break;
                    case "d2":
                    case "d3":
                    case "d4":
                    case "d5":
                    case "d6":
                    case "d7":
                    case "d8":
                    case "d9":
                    case "d0":
                        keyboard.AddChar(s.Replace("d", ""));
                        break;
                    default:
                        if (s.Length == 1)
                        {
                            keyboard.AddChar(s);
                        }
                        else {
                            //UnityEngine.Debug.Log(s);
                        }
                        break;
                }
            }//endIf
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private static KeyboardMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;
    private static Keyboard keyboard;
    private static float timer = 0f;

    public static KeyboardMode Instance()
    {
        if (instance == null)
        {
            instance = new KeyboardMode();
            manager = SL.Get<GameManager>();
            control = SL.Get<IVRControl>();
            shared = SL.Get<WorldBuilderMain>();
            keyboard = SL.Get<Keyboard>();
        }
        return instance;
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        switch (n.fileName) {
            case "Enter":
                manager.keyboardString = keyboard.Enter();
                manager.keyboard.gameObject.SetActive(false);
                break;
            case "Backspace":
                keyboard.BackSpace();
                break;
            case "Space":
                keyboard.Space();
                break;
            default:
                keyboard.AddChar(n.fileName);
                break;                
        }
    }

    public void SetupMode()
    {
        active = true;
    }

    public void SoftUnload()
    {
        active = false;
    }

    public void IUpdate()
    {
        
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Keyboard;

        timer += manager.lastDeltaTime;
        if (timer > .3f)
        {
            keyboard.ToggleCaret();
            timer = 0;
        }

        if (control.LB())
        {
            keyboard.BackSpace();
            return;
        }

        if (control.RB2()) {
            keyboard.Space();
            return;
        }
    }
}
