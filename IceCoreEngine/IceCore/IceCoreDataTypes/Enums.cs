﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoreEngine
{
    public enum EInputType
    {
        Digital,
        Analog
    }
    public enum EInput
    {
        None = 0,
        Back = 1,
        Tab = 2,
        Enter = 3,
        CapsLock = 4,
        Escape = 5,
        Space = 6,
        PageUp = 7,
        PageDown = 8,
        End = 9,
        Home = 10,
        Left = 11,
        Up = 12,
        Right = 13,
        Down = 14,
        Select = 15,
        Print = 16,
        Execute = 17,
        PrintScreen = 18,
        Insert = 19,
        Delete = 20,
        Help = 21,
        D0 = 22,
        D1 = 23,
        D2 = 24,
        D3 = 25,
        D4 = 26,
        D5 = 27,
        D6 = 28,
        D7 = 29,
        D8 = 30,
        D9 = 31,
        A = 32,
        B = 33,
        C = 34,
        D = 35,
        E = 36,
        F = 37,
        G = 38,
        H = 39,
        I = 40,
        J = 41,
        K = 42,
        L = 43,
        M = 44,
        N = 45,
        O = 46,
        P = 47,
        Q = 48,
        R = 49,
        S = 50,
        T = 51,
        U = 52,
        V = 53,
        W = 54,
        X = 55,
        Y = 56,
        Z = 57,
        LeftWindows = 58,
        RightWindows = 59,
        Apps = 60,
        Sleep = 61,
        NumPad0 = 62,
        NumPad1 = 63,
        NumPad2 = 64,
        NumPad3 = 65,
        NumPad4 = 66,
        NumPad5 = 67,
        NumPad6 = 68,
        NumPad7 = 69,
        NumPad8 = 70,
        NumPad9 = 71,
        Multiply = 72,
        Add = 73,
        Separator = 74,
        Subtract = 75,
        Decimal = 76,
        Divide = 77,
        F1 = 78,
        F2 = 79,
        F3 = 80,
        F4 = 81,
        F5 = 82,
        F6 = 83,
        F7 = 84,
        F8 = 85,
        F9 = 86,
        F10 = 87,
        F11 = 88,
        F12 = 89,
        F13 = 90,
        F14 = 91,
        F15 = 92,
        F16 = 93,
        F17 = 94,
        F18 = 95,
        F19 = 96,
        F20 = 97,
        F21 = 98,
        F22 = 99,
        F23 = 100,
        F24 = 101,
        NumLock = 102,
        Scroll = 103,
        LeftShift = 104,
        RightShift = 105,
        LeftControl = 106,
        RightControl = 107,
        LeftAlt = 108,
        RightAlt = 109,
        BrowserBack = 110,
        BrowserForward = 111,
        BrowserRefresh = 112,
        BrowserStop = 113,
        BrowserSearch = 114,
        BrowserFavorites = 115,
        BrowserHome = 116,
        VolumeMute = 117,
        VolumeDown = 118,
        VolumeUp = 119,
        MediaNextTrack = 120,
        MediaPreviousTrack = 121,
        MediaStop = 122,
        MediaPlayPause = 123,
        LaunchMail = 124,
        SelectMedia = 125,
        LaunchApplication1 = 126,
        LaunchApplication2 = 127,
        OemSemicolon = 128,
        OemPlus = 129,
        OemComma = 130,
        OemMinus = 131,
        OemPeriod = 132,
        OemQuestion = 133,
        OemTilde = 134,
        OemOpenBrackets = 135,
        OemPipe = 136,
        OemCloseBrackets = 137,
        OemQuotes = 138,
        Oem8 = 139,
        OemBackslash = 140,
        ProcessKey = 141,
        Attn = 142,
        Crsel = 143,
        Exsel = 144,
        EraseEof = 145,
        Play = 146,
        Zoom = 147,
        Pa1 = 148,
        OemClear = 149,
        ChatPadGreen = 150,
        ChatPadOrange = 151,
        Pause = 152,
        ImeConvert = 153,
        ImeNoConvert = 154,
        Kana = 155,
        Kanji = 156,
        OemAuto = 157,
        OemCopy = 158,
        OemEnlW = 159,
        MouseLeft = 160,
        MouseRight = 161,
        MouseMiddle = 162,
        MouseScroll = 163,
        MouseXButton1 = 164,
        MouseXButton2 = 165,
        MouseX = 166,
        MouseY = 167
    }

    public enum EAnchor
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Middle,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
}
