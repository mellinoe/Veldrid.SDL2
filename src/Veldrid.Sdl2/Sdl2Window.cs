﻿using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Veldrid.Sdl2.Sdl2Native;

namespace Veldrid.Sdl2
{
    // TODO: Move this into Veldrid itself, or a Veldrid extension library.
    public unsafe class Sdl2Window : Window
    {
        private readonly IntPtr _window;
        private IntPtr _handle;

        private Rectangle _bounds;

        public Sdl2Window(string title, int x, int y, int width, int height, SDL_WindowFlags flags)
        {
            _window = SDL_CreateWindow(title, x, y, width, height, flags);
        }

        public int Width { get => GetWindowSize().Width; set => SetWindowSize(value, Height); }
        public int Height { get => GetWindowSize().Height; set => SetWindowSize(Width, value); }

        public IntPtr Handle => GetUnderlyingWindowHandle();

        public string Title { get => SDL_GetWindowTitle(_window); set => SDL_SetWindowTitle(_window, value); }
        public WindowState WindowState
        {
            get
            {
                SDL_WindowFlags flags = SDL_GetWindowFlags(_window);
                WindowState state = WindowState.Normal;
                if ((flags & (SDL_WindowFlags.Borderless | SDL_WindowFlags.Fullscreen)) == (SDL_WindowFlags.Borderless | SDL_WindowFlags.Fullscreen))
                {
                    return WindowState.BorderlessFullScreen;
                }
                else if ((flags & SDL_WindowFlags.Minimized) == SDL_WindowFlags.Minimized)
                {
                    return WindowState.Minimized;
                }
                else if ((flags & SDL_WindowFlags.Fullscreen) == SDL_WindowFlags.Fullscreen)
                {
                    return WindowState.FullScreen;
                }
                else if ((flags & SDL_WindowFlags.Maximized) == SDL_WindowFlags.Maximized)
                {
                    return WindowState.Maximized;
                }

                return WindowState.Normal;
            }
            set
            {
                switch (value)
                {
                    case WindowState.Normal:
                        SDL_SetWindowFullscreen(_window, SDL_FullscreenMode.Windowed);
                        break;
                    case WindowState.FullScreen:
                        SDL_SetWindowFullscreen(_window, SDL_FullscreenMode.Fullscreen);
                        break;
                    case WindowState.Maximized:
                        SDL_MaximizeWindow(_window);
                        break;
                    case WindowState.Minimized:
                        SDL_MinimizeWindow(_window);
                        break;
                    case WindowState.BorderlessFullScreen:
                        SDL_SetWindowFullscreen(_window, SDL_FullscreenMode.FullScreenDesktop);
                        break;
                    default:
                        throw new InvalidOperationException("Illegal WindowState value: " + value);
                }
            }
        }

        public bool Exists => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Vector2 ScaleFactor => throw new NotImplementedException();

        public Rectangle Bounds => throw new NotImplementedException();

        public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Focused => throw new NotImplementedException();

        public event Action Resized;
        public event Action Closing;
        public event Action Closed;
        public event Action FocusLost;
        public event Action FocusGained;

        public Point ClientToScreen(Point p)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public InputSnapshot GetInputSnapshot()
        {
            throw new NotImplementedException();
        }

        public Point ScreenToClient(Point p)
        {
            throw new NotImplementedException();
        }

        private Size GetWindowSize()
        {
            int w, h;
            SDL_GetWindowSize(_window, &w, &h);
            return new Size(w, h);
        }

        private void SetWindowSize(int width, int height)
        {
            SDL_SetWindowSize(_window, width, height);
        }

        private IntPtr GetUnderlyingWindowHandle()
        {
            SDL_SysWMinfo wmInfo;
            SDL_GetVersion(&wmInfo.version);
            SDL_GetWMWindowInfo(_window, &wmInfo);
            if (wmInfo.subsystem == SysWMType.Windows)
            {
                Win32WindowInfo win32Info = Unsafe.Read<Win32WindowInfo>(&wmInfo.info);
                return win32Info.window;
            }

            return _window;
        }

        private bool GetWindowBordered() => (SDL_GetWindowFlags(_window) & SDL_WindowFlags.Borderless) == 0;
        private void SetWindowBordered(bool value) => SDL_SetWindowBordered(_window, value ? 1u : 0u);
    }
}
