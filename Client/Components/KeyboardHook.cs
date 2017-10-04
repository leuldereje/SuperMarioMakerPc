using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CommonLib.Utils;
// ReSharper disable MemberCanBePrivate.Local

namespace Client.Components {
  public static class KeyboardHook {
    private static Win32Utils.HookProc _keyboardDelegate;
    private static int _keyboardHookHandle;

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardHookStruct {
      public readonly int VirtualKeyCode;
      public readonly int ScanCode;
      public readonly int Flags;
      public readonly int Time;
      public readonly int ExtraInfo;
    }

    private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam) {
      var handled = false;

      if (nCode >= 0) {
        var keyboardHookStruct = (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

        var isShiftDown = (Win32Utils.GetKeyState(Constants.Keyboard.VK_SHIFT) & 0x80) == 0x80;
        var isControlDown = (Win32Utils.GetKeyState(Constants.Keyboard.VK_CONTROL) & 0x80) == 0x80;
        var isAltDown = (Win32Utils.GetKeyState(Constants.Keyboard.VK_ALT) & 0x80) == 0x80;

        // key down
        if (Down != null && (wParam == Constants.Keyboard.WM_KEYDOWN || wParam == Constants.Keyboard.WM_SYSKEYDOWN)) {
          var e = new KeyboardHookEventArgs(keyboardHookStruct.VirtualKeyCode, isShiftDown, isControlDown, isAltDown);

          Down.Invoke(e);

          handled = e.Handled;
        }

        // key press
        if (Press != null && wParam == Constants.Keyboard.WM_KEYDOWN) {
          var e = new KeyboardHookEventArgs(keyboardHookStruct.VirtualKeyCode, isShiftDown, isControlDown, isAltDown);

          Press.Invoke(e);

          handled = e.Handled;
        }

        // key up
        if (Up != null && (wParam == Constants.Keyboard.WM_KEYUP || wParam == Constants.Keyboard.WM_SYSKEYUP)) {
          var e = new KeyboardHookEventArgs(keyboardHookStruct.VirtualKeyCode, isShiftDown, isControlDown, isAltDown);

          Up.Invoke(e);

          handled = e.Handled;
        }
      }

      if (handled) {
        return -1;
      }

      return Win32Utils.CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
    }

    private static void EnsureSubscribedToGlobalKeyboardEvents() {
      if (_keyboardHookHandle != 0) {
        return;
      }
        
      _keyboardDelegate = KeyboardHookProc;
      _keyboardHookHandle = Win32Utils.SetWindowsHookEx(Constants.Keyboard.WH_KEYBOARD_LL, _keyboardDelegate, Win32Utils.LoadLibrary("user32.dll"), 0);

      if (_keyboardHookHandle == 0) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
    }

    private static void TryUnsubscribeFromGlobalKeyboardEvents() {
      if (Down == null && Up == null && Press == null) {
        ForceUnsunscribeFromGlobalKeyboardEvents();
      }
    }

    private static void ForceUnsunscribeFromGlobalKeyboardEvents() {
      if (_keyboardHookHandle == 0) {
        return;
      }
        
      var result = Win32Utils.UnhookWindowsHookEx(_keyboardHookHandle);

      _keyboardHookHandle = 0;
      _keyboardDelegate = null;

      if (result == 0) {
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
    }

    // events
    public static event KeyboardEventHandler KeyPress {
      add {
        EnsureSubscribedToGlobalKeyboardEvents();
        Press += value;
      }
      remove {
        Press -= value;
        TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    public static event KeyboardEventHandler KeyUp {
      add {
        EnsureSubscribedToGlobalKeyboardEvents();
        Up += value;
      }
      remove {
        Up -= value;
        TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    public static event KeyboardEventHandler KeyDown {
      add {
        EnsureSubscribedToGlobalKeyboardEvents();
        Down += value;
      }
      remove {
        Down -= value;
        TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    // internal events
    public delegate void KeyboardEventHandler(KeyboardHookEventArgs e);

    private static event KeyboardEventHandler Press;

    private static event KeyboardEventHandler Up;

    private static event KeyboardEventHandler Down;
  }
}