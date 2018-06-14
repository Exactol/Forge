using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using Vector2 = System.Numerics.Vector2;

namespace Forge.remove
{
    class Program
    {
        static void Main(string[] args)
        {
            using(MainWindow mw = new MainWindow())
            {
                mw.Run(60);
            }
        }

        sealed class MainWindow : GameWindow
        {
            public MainWindow()
                : base(1280, 720, GraphicsMode.Default,
                      "IMGUI Test", GameWindowFlags.Default, DisplayDevice.Default,
                      4, 3, GraphicsContextFlags.ForwardCompatible
                      )
            {
                Console.SetOut(stringWriter);
                Console.WriteLine("-----System Info-----\n");
                Console.WriteLine("GL version: " + GL.GetString(StringName.Version));
                Console.WriteLine("Shading language version: " + GL.GetString(StringName.ShadingLanguageVersion));
                Console.WriteLine("Vendor: {0} \n", GL.GetString(StringName.Vendor));
            }

            private bool _windowOpen = false;
            private bool _debugEnabled = false;
            private bool _wireframeEnabled = false;
            private int fontTexture;
            private int scaleFactor;
            
            static int shaderHandle = 0, vertHandle = 0, fragHandle = 0;
            static int attribLocationTex = 0, attribLocationProjMtx = 0;
            static int attribLocationPosition = 0, attribLocationUV = 0, attribLocationColor = 0;
            static int vboHandle = 0, vaoHandle = 0, elementsHandle = 0;
            StringWriter stringWriter = new StringWriter();
            
            private unsafe void CreateDeviceObjects()
            {
                int lastTexture, lastArrayBuffer, lastVertexArray;
                GL.GetInteger(GetPName.TextureBinding2D,  out lastTexture);
                GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBuffer);
                GL.GetInteger(GetPName.VertexArrayBinding, out lastVertexArray);

                const string vertShader = "" +
                    "#version 330\n" +
                    "uniform mat4 ProjMtx;\n" +
                    "" +
                    "in vec2 Position;\n" +
                    "in vec2 UV;\n" +
                    "in vec4 Color;\n" +
                    "" +
                    "out vec2 Frag_UV;\n" +
                    "out vec4 Frag_Color;\n" +
                    "" +
                    //"out vec4 vertPos;\n" +
                    "" +
                    "void main()\n" +
                    "{\n" +
                    "   Frag_UV = UV;\n" +
                    "   Frag_Color = Color;\n" +
                    "   gl_Position = ProjMtx * vec4(Position.xy, 0, 1);\n" +
                    //"   vertPos = gl_Position;" +
                    "}\n";

                const string fragShader = "" +
                    "#version 330\n" +
                    "" +
                    "uniform sampler2D Texture;\n" +
                    "in vec2 Frag_UV;\n" +
                    "in vec4 Frag_Color;\n" +
                    //"in vec4 vertPos;\n" +
                    "out vec4 Out_Color;\n" +
                    "" +
                    "void main()\n" +
                    "{\n" +
                    "   Out_Color = Frag_Color * texture(Texture, Frag_UV.st);\n" +
                    "}\n";

                shaderHandle = GL.CreateProgram();
                vertHandle = GL.CreateShader(ShaderType.VertexShader);
                fragHandle = GL.CreateShader(ShaderType.FragmentShader);

                GL.ShaderSource(vertHandle, vertShader);
                GL.ShaderSource(fragHandle, fragShader);

                GL.CompileShader(vertHandle);
                GL.CompileShader(fragHandle);

                GL.AttachShader(shaderHandle, vertHandle);
                GL.AttachShader(shaderHandle, fragHandle);

                if (GL.GetShaderInfoLog(vertHandle) != "")
                {
                    Console.WriteLine("Vert Error: " + GL.GetShaderInfoLog(vertHandle));
                }
                if (GL.GetShaderInfoLog(fragHandle) != "")
                {
                    Console.WriteLine("Frag Error: " + GL.GetShaderInfoLog(fragHandle));
                }
                
                
                GL.LinkProgram(shaderHandle);

                if (GL.GetProgramInfoLog(shaderHandle) != "")
                {
                    Console.WriteLine("Program Error: "+GL.GetProgramInfoLog(shaderHandle));
                }
                

                attribLocationTex = GL.GetUniformLocation(shaderHandle, "Texture");
                attribLocationProjMtx = GL.GetUniformLocation(shaderHandle, "ProjMtx");
                attribLocationPosition = GL.GetAttribLocation(shaderHandle, "Position");
                attribLocationUV = GL.GetAttribLocation(shaderHandle, "UV");
                attribLocationColor = GL.GetAttribLocation(shaderHandle, "Color");

                //Console.WriteLine(GL.GetError());

                GL.GenBuffers(1, out vboHandle);
                GL.GenBuffers(1, out elementsHandle);

                GL.GenVertexArrays(1, out vaoHandle);
                GL.BindVertexArray(vaoHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);

                GL.EnableVertexAttribArray(attribLocationPosition);
                GL.EnableVertexAttribArray(attribLocationUV);
                GL.EnableVertexAttribArray(attribLocationColor);

                //Console.WriteLine(GL.GetError());

                GL.VertexAttribPointer(
                    attribLocationPosition,
                    2,
                    VertexAttribPointerType.Float,
                    false,
                    sizeof(DrawVert),
                    new IntPtr(DrawVert.PosOffset)
                    );

                GL.VertexAttribPointer(
                    attribLocationUV,
                    2,
                    VertexAttribPointerType.Float,
                    false,
                    sizeof(DrawVert),
                    new IntPtr(DrawVert.UVOffset)
                    );

                //new IntPtr(&(((DrawVert*)0)->uv))

                GL.VertexAttribPointer(
                    attribLocationColor,
                    4,
                    VertexAttribPointerType.UnsignedByte,
                    true,
                    sizeof(DrawVert),
                    new IntPtr(DrawVert.ColOffset)
                    );

                //Fonts
                IO io = ImGui.GetIO();

                io.FontAtlas.AddFontFromFileTTF("Roboto-Regular.ttf", 15);

                FontTextureData texData = io.FontAtlas.GetTexDataAsRGBA32();

                fontTexture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, fontTexture);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    texData.Width,
                    texData.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    new IntPtr(texData.Pixels));

                io.FontAtlas.SetTexID(fontTexture);
                io.FontAtlas.ClearTexData();

                GL.BindTexture(TextureTarget.Texture2D, 1);

                //Restore
                GL.BindTexture(TextureTarget.Texture2D, lastTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBuffer);
                GL.BindVertexArray(lastVertexArray);
            }

            protected override unsafe void OnLoad(EventArgs e)
            {
                GL.ClearColor(Color4.BlueViolet);
                
                //ImGui.GetIO().FontAtlas.AddDefaultFont();
                _windowOpen = true;

                CreateDeviceObjects();

                scaleFactor = Width / 1280;

                textInputBufferLength = 1024;
                textInputBuffer = Marshal.AllocHGlobal(textInputBufferLength);
                long* ptr = (long*)textInputBuffer.ToPointer();

                for (int i = 0; i < 1024/sizeof(long); i++)
                {
                    ptr[i] = 0;
                }

                SetUpImguiStyle(true, 1.0f);

                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.DebugOutput);
                
            }

            //Source https://gist.github.com/dougbinks/8089b4bbaccaaf6fa204236978d165a9#file-imguiutils-h-L9-L93
            private unsafe void SetUpImguiStyle(bool styleDark, float alpha)
            {
                Style style = ImGui.GetStyle();
                
                style.Alpha = 1.0f;
                style.FrameRounding = 0f;
                style.WindowRounding = 0f;
                style.ChildWindowRounding = 0f;
                style.AntiAliasedLines = true;
                //style.AntiAliasedShapes = true;
                
                style.SetColor(ColorTarget.Text, new System.Numerics.Vector4(0.00f, 0.00f, 0.00f, 1.00f));
                style.SetColor(ColorTarget.TextDisabled, new System.Numerics.Vector4(0.60f, 0.60f, 0.60f, 1.00f));
                style.SetColor(ColorTarget.WindowBg, new System.Numerics.Vector4(0.94f, 0.94f, 0.94f, 0.94f));
                //style.SetColor(ColorTarget.ChildWindowBg, new System.Numerics.Vector4(0.00f, 0.00f, 0.00f, 0.00f));
                style.SetColor(ColorTarget.Border, new System.Numerics.Vector4(0.00f, 0.00f, 0.00f, 0.39f));
                style.SetColor(ColorTarget.BorderShadow, new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.10f));
                style.SetColor(ColorTarget.FrameBg, new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.94f));
                style.SetColor(ColorTarget.FrameBgHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.40f));
                style.SetColor(ColorTarget.FrameBgActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.67f));
                style.SetColor(ColorTarget.TitleBg, new System.Numerics.Vector4(0.96f, 0.96f, 0.96f, 1.00f));
                style.SetColor(ColorTarget.TitleBgCollapsed, new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.51f));
                style.SetColor(ColorTarget.TitleBgActive, new System.Numerics.Vector4(0.82f, 0.82f, 0.82f, 1.00f));
                style.SetColor(ColorTarget.MenuBarBg, new System.Numerics.Vector4(0.86f, 0.86f, 0.86f, 1.00f));
                style.SetColor(ColorTarget.ScrollbarBg, new System.Numerics.Vector4(0.98f, 0.98f, 0.98f, 0.53f));
                style.SetColor(ColorTarget.ScrollbarGrab, new System.Numerics.Vector4(0.69f, 0.69f, 0.69f, 1.00f));
                style.SetColor(ColorTarget.ScrollbarGrabHovered, new System.Numerics.Vector4(0.59f, 0.59f, 0.59f, 1.00f));
                style.SetColor(ColorTarget.ScrollbarGrabActive, new System.Numerics.Vector4(0.49f, 0.49f, 0.49f, 1.00f));
                //style.SetColor(ColorTarget.ComboBg, new System.Numerics.Vector4(0.86f, 0.86f, 0.86f, 0.99f));
                style.SetColor(ColorTarget.CheckMark, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f));
                style.SetColor(ColorTarget.SliderGrab, new System.Numerics.Vector4(0.24f, 0.52f, 0.88f, 1.00f));
                style.SetColor(ColorTarget.SliderGrabActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f));
                style.SetColor(ColorTarget.Button, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.40f));
                style.SetColor(ColorTarget.ButtonHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f));
                style.SetColor(ColorTarget.ButtonActive, new System.Numerics.Vector4(0.06f, 0.53f, 0.98f, 1.00f));
                style.SetColor(ColorTarget.Header, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.31f));
                style.SetColor(ColorTarget.HeaderHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.80f));
                style.SetColor(ColorTarget.HeaderActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f));
                //style.SetColor(ColorTarget.Column, new System.Numerics.Vector4(0.39f, 0.39f, 0.39f, 1.00f));
                //style.SetColor(ColorTarget.ColumnHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.78f));
                //style.SetColor(ColorTarget.ColumnActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f));
                style.SetColor(ColorTarget.ResizeGrip, new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.50f));
                style.SetColor(ColorTarget.ResizeGripHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.67f));
                style.SetColor(ColorTarget.ResizeGripActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.95f));
                style.SetColor(ColorTarget.CloseButton, new System.Numerics.Vector4(0.59f, 0.59f, 0.59f, 0.50f));
                style.SetColor(ColorTarget.CloseButtonHovered, new System.Numerics.Vector4(0.98f, 0.39f, 0.36f, 1.00f));
                style.SetColor(ColorTarget.CloseButtonActive, new System.Numerics.Vector4(0.98f, 0.39f, 0.36f, 1.00f));
                style.SetColor(ColorTarget.PlotLines, new System.Numerics.Vector4(0.39f, 0.39f, 0.39f, 1.00f));
                style.SetColor(ColorTarget.PlotLinesHovered, new System.Numerics.Vector4(1.00f, 0.43f, 0.35f, 1.00f));
                style.SetColor(ColorTarget.PlotHistogram, new System.Numerics.Vector4(0.90f, 0.70f, 0.00f, 1.00f));
                style.SetColor(ColorTarget.PlotHistogramHovered, new System.Numerics.Vector4(1.00f, 0.60f, 0.00f, 1.00f));
                style.SetColor(ColorTarget.TextSelectedBg, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.35f));
                style.SetColor(ColorTarget.ModalWindowDarkening, new System.Numerics.Vector4(0.20f, 0.20f, 0.20f, 0.35f));
                
            }

            private static void SetOpenTkKeyMappings()
            {
                IO io = ImGui.GetIO();
                io.KeyMap[GuiKey.Tab] = (int)Key.Tab;
                io.KeyMap[GuiKey.LeftArrow] = (int)Key.Left;
                io.KeyMap[GuiKey.RightArrow] = (int)Key.Right;
                io.KeyMap[GuiKey.UpArrow] = (int)Key.Up;
                io.KeyMap[GuiKey.DownArrow] = (int)Key.Down;
                io.KeyMap[GuiKey.PageUp] = (int)Key.PageUp;
                io.KeyMap[GuiKey.PageDown] = (int)Key.PageDown;
                io.KeyMap[GuiKey.Home] = (int)Key.Home;
                io.KeyMap[GuiKey.End] = (int)Key.End;
                io.KeyMap[GuiKey.Delete] = (int)Key.Delete;
                io.KeyMap[GuiKey.Backspace] = (int)Key.BackSpace;
                io.KeyMap[GuiKey.Enter] = (int)Key.Enter;
                io.KeyMap[GuiKey.Escape] = (int)Key.Escape;
                io.KeyMap[GuiKey.A] = (int)Key.A;
                io.KeyMap[GuiKey.C] = (int)Key.C;
                io.KeyMap[GuiKey.V] = (int)Key.V;
                io.KeyMap[GuiKey.X] = (int)Key.X;
                io.KeyMap[GuiKey.Y] = (int)Key.Y;
                io.KeyMap[GuiKey.Z] = (int)Key.Z;
                
            }

            private static void UpdateModifiers(KeyboardKeyEventArgs e)
            {
                IO io = ImGui.GetIO();
                io.AltPressed = e.Alt;
                io.CtrlPressed = e.Control;
                io.ShiftPressed = e.Shift;
            }

            protected override void OnKeyDown(KeyboardKeyEventArgs e)
            {
                ImGui.GetIO().KeysDown[(int)e.Key] = true;
                UpdateModifiers(e);
            }

            protected override void OnKeyUp(KeyboardKeyEventArgs e)
            {
                ImGui.GetIO().KeysDown[(int)e.Key] = false;
                UpdateModifiers(e);
            }

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                ImGui.AddInputCharacter(e.KeyChar);
            }


            protected override void OnResize(EventArgs e)
            {
                float aspectRatio = Height / Width;

                GL.Viewport(0, 0, Width, Height);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {

            }

            private unsafe int OnTextEdited(TextEditCallbackData* data)
            {
                char currentEventChar = (char)data->EventChar;
                return 0;
            }


            //string titleInput = "Temp";
            float wheelPos;
            private IntPtr textInputBuffer;
            private int textInputBufferLength;

            private bool showConsole = false;
            private bool showOpenGLConsole = false;

            protected override unsafe void OnRenderFrame(FrameEventArgs e)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                Title = $"IMGUI Test FPS: {1f / e.Time:0}";
                
                IO io = ImGui.GetIO();
                
                io.DisplaySize = new System.Numerics.Vector2(Width, Height);
                //io.DisplayFramebufferScale = new System.Numerics.Vector2(scaleFactor);
                io.DeltaTime = (1f/60f);
                
                UpdateImguiInput(io);
                
                ImGui.NewFrame();
                
                SubmitGui();

                GL.Viewport(0,0, Width, Height);

                ImGui.Render();

                ProcessEvents();

                DrawData* data = ImGui.GetDrawData();
                RenderImDrawData(data);

                SwapBuffers();
            }

            private unsafe void UpdateImguiInput(IO io)
            {
                MouseState cursorState = Mouse.GetCursorState();
                MouseState mouseState = Mouse.GetState();

                if (Bounds.Contains(cursorState.X, cursorState.Y))
                {
                    Point windowPoint = PointToClient(new Point(cursorState.X, cursorState.Y));
                    io.MousePosition = new System.Numerics.Vector2(windowPoint.X/io.DisplayFramebufferScale.X, windowPoint.Y/io.DisplayFramebufferScale.Y);  
                }
                else
                {
                    io.MousePosition = new System.Numerics.Vector2(-1f, -1f);
                }
                
                io.MouseDown[0] = mouseState.LeftButton == ButtonState.Pressed;
                io.MouseDown[1] = mouseState.RightButton == ButtonState.Pressed;
                io.MouseDown[2] = mouseState.MiddleButton == ButtonState.Pressed;
                
                float newWheelPos = mouseState.WheelPrecise;
                float deltaWheelPos = newWheelPos - wheelPos;
                wheelPos = newWheelPos;
                io.MouseWheel = deltaWheelPos;
            }

            private unsafe void SubmitGui()
            {
                //ImGui.SetNextWindowSize(new System.Numerics.Vector2(Width, Height), SetCondition.FirstUseEver);

                //ImGui.SetNextWindowPosCenter(SetCondition.Always);

                ImGui.BeginWindow("Test", ref _windowOpen, WindowFlags.NoResize | WindowFlags.NoTitleBar | WindowFlags.NoMove);

                ImGui.BeginMainMenuBar();
                
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Menu item", "", false, true))
                    {
                        
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Edit", "", false, true))
                    {

                    }
                    ImGui.EndMenu();
                }
                
                if (ImGui.BeginMenu("View"))
                {
                    if (ImGui.Checkbox("Wireframe Mode", ref _wireframeEnabled))
                    {
                        Console.WriteLine("Wireframe Mode:" + _wireframeEnabled);
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Options"))
                {
                    if (ImGui.Checkbox("Debug Mode", ref _debugEnabled))
                    {

                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Tools"))
                {
                    if (ImGui.MenuItem("Edit", "", false, true))
                    {

                    }
                    ImGui.EndMenu();
                }
                
                if (ImGui.BeginMenu("Windows"))
                {

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    if (ImGui.MenuItem("Edit", "", false, true))
                    {

                    }
                    ImGui.EndMenu();
                }

                if (_debugEnabled)
                {
                    if (ImGui.BeginMenu("Debug"))
                    {
                        if (ImGui.MenuItem("Console"))
                        {
                            showConsole = !showConsole;
                        }

                        if (ImGui.MenuItem("OpenGL Console"))
                        {
                            showOpenGLConsole = !showOpenGLConsole;
                        }
                        ImGui.EndMenu();                        
                    }

                }

                ImGui.EndMainMenuBar();

                //ImGui.SetNextWindowPos(new Vector2(0, 20), SetCondition.Always);
                //ImGui.SetNextWindowSize(new Vector2(Width, 30), SetCondition.Always);

                ImGui.PushStyleVar(StyleVar.WindowPadding, new Vector2(5f, 5f));

                ImGui.BeginWindow("inner window", WindowFlags.NoMove | WindowFlags.NoTitleBar | WindowFlags.NoResize | WindowFlags.NoScrollbar | WindowFlags.NoScrollWithMouse);
                
                if (ImGui.Button("Btn1", new Vector2(50, 25)))
                {

                }

                ImGui.SameLine();

                if (ImGui.Button("Btn2"))
                {
                    
                }
                
                ImGui.EndWindow();

                ImGui.PopStyleVar();

                ImGui.BeginChildFrame(1 ,new Vector2(50,50), WindowFlags.AlwaysAutoResize);

                ImGui.Text("Outside");
                ImGui.Separator();
                ImGui.Spacing();
                ImGui.Spacing();
                ImGui.Spacing();
                ImGui.Spacing();
                ImGui.Spacing();
                ImGui.Text("Seperator");

                ImGui.EndWindow();

                if (showConsole)
                {
                    ImGui.BeginWindow("Console", ref showConsole, WindowFlags.Default);
                    ImGui.Text(stringWriter.ToString());
                    ImGui.EndWindow();
                }

                if (showOpenGLConsole)
                {
                    ImGui.BeginWindow("OpenGL Console", ref showOpenGLConsole, WindowFlags.Default);
                    //Create callback
                    GetGlDebugMessages();

                    ImGui.EndWindow();
                }
            }

            
            private unsafe void RenderImDrawData(DrawData* drawData)
            {

                IO io = ImGui.GetIO();

                int fbWidth = (int)(io.DisplaySize.X * io.DisplayFramebufferScale.X);
                int fbHeight = (int)(io.DisplaySize.Y * io.DisplayFramebufferScale.Y);

                if (fbWidth != 0 || fbHeight != 0)
                {
                    ImGui.ScaleClipRects(drawData, io.DisplayFramebufferScale);
                    
                    int lastActiveTexture; GL.GetInteger(GetPName.ActiveTexture, out lastActiveTexture);
                    //Console.WriteLine(GL.GetError());
                    GL.ActiveTexture(TextureUnit.Texture0);
                    //Console.WriteLine(GL.GetError());
                    int lastProgram; GL.GetInteger(GetPName.CurrentProgram, out lastProgram);
                    //Console.WriteLine(GL.GetError());
                    int lastTexture; GL.GetInteger(GetPName.TextureBinding2D, out lastTexture);
                    //Console.WriteLine(GL.GetError());
                    int lastArrayBuffer; GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBuffer);
                    int lastElementArrayBuffer; GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBuffer);
                    int lastVertexArray; GL.GetInteger(GetPName.VertexArrayBinding, out lastVertexArray);
                    int lastBlendSrcRgb; GL.GetInteger(GetPName.BlendSrcRgb, out lastBlendSrcRgb);
                    int lastBlendDstRgb; GL.GetInteger(GetPName.BlendDstRgb, out lastBlendDstRgb);
                    int lastBlendSrcAlpha; GL.GetInteger(GetPName.BlendSrcAlpha, out lastBlendSrcAlpha);
                    int lastBlendDstAlpha; GL.GetInteger(GetPName.BlendDstAlpha, out lastBlendDstAlpha);
                    int lastBlendEquationRgb; GL.GetInteger(GetPName.BlendEquationRgb, out lastBlendEquationRgb);
                    int lastBlendEquationAlpha; GL.GetInteger(GetPName.BlendEquationAlpha, out lastBlendEquationAlpha);
                    //Console.WriteLine(GL.GetError());
                    int[] lastViewport = new int[4]; GL.GetInteger(GetPName.Viewport, lastViewport);
                    int[] lastScissorBox = new int[4]; GL.GetInteger(GetPName.ScissorBox, lastScissorBox);
                    //Console.WriteLine(GL.GetError());
                    bool lastEnableBlend = GL.IsEnabled(EnableCap.Blend);
                    bool lastEnableCullFace = GL.IsEnabled(EnableCap.CullFace);
                    bool lastEnableDepthTest = GL.IsEnabled(EnableCap.DepthTest);
                    bool lastEnableScissorTest = GL.IsEnabled(EnableCap.ScissorTest);

                    GL.Enable(EnableCap.Blend);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    GL.Disable(EnableCap.CullFace);
                    GL.Disable(EnableCap.DepthTest);
                    GL.Enable(EnableCap.ScissorTest);
                    
                    GL.Viewport(0, 0, fbWidth, fbHeight);
                    Matrix4 orthoProjection = new Matrix4(
                        2.0f / io.DisplaySize.X, 0.0f, 0.0f, 0.0f,
                        0.0f, 2.0f / -io.DisplaySize.Y, 0.0f, 0.0f,
                        0.0f, 0.0f, -1.0f, 0.0f,
                        -1.0f, 1.0f, 0.0f, 1.0f);

                    GL.UseProgram(shaderHandle);
                    GL.Uniform1(attribLocationTex, 0);
                    GL.UniformMatrix4(attribLocationProjMtx,false, ref orthoProjection);
                    GL.BindVertexArray(vaoHandle);
                    
                    for (int i = 0; i < drawData->CmdListsCount; i++)
                    {
                        NativeDrawList* cmdList = drawData->CmdLists[i];
                        ushort* idxBuffer = (ushort*)0;
                        
                        GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
                        GL.BufferData(BufferTarget.ArrayBuffer, cmdList->VtxBuffer.Size * sizeof(DrawVert), new IntPtr(cmdList->VtxBuffer.Data), BufferUsageHint.StreamDraw );

                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsHandle);
                        GL.BufferData(BufferTarget.ElementArrayBuffer, cmdList->IdxBuffer.Size * sizeof(ushort*), new IntPtr(cmdList->IdxBuffer.Data), BufferUsageHint.StreamDraw);

                        for (int cmd_i = 0; cmd_i < cmdList->CmdBuffer.Size; cmd_i++ )
                        {
                            DrawCmd* pcmd = &((DrawCmd*)cmdList->CmdBuffer.Data)[cmd_i];

                            if (pcmd->UserCallback != IntPtr.Zero)
                            {
                                Console.WriteLine("User callback not implemented");
                                throw new NotImplementedException();
                            }
                            else
                            {
                                //GL.BindTexture(TextureTarget.Texture2D, (int)pcmd->TextureId);
                                GL.BindTexture(TextureTarget.Texture2D, pcmd->TextureId.ToInt32());
                                GL.Scissor(
                                    (int)pcmd->ClipRect.X,
                                    (int)(fbHeight - pcmd->ClipRect.W),
                                    (int)(pcmd->ClipRect.Z - pcmd->ClipRect.X),
                                    (int)(pcmd->ClipRect.W - pcmd->ClipRect.Y));

                                GL.DrawElements(PrimitiveType.Triangles, (int)pcmd->ElemCount, DrawElementsType.UnsignedShort, new IntPtr(idxBuffer));
                            }

                            idxBuffer += pcmd->ElemCount;
                        }
                    }

                    GL.UseProgram(lastProgram);
                    GL.BindTexture(TextureTarget.Texture2D, lastTexture);
                    //Console.WriteLine(GL.GetError());
                    GL.ActiveTexture((TextureUnit)lastActiveTexture);
                    //Console.WriteLine(GL.GetError());
                    GL.BindVertexArray(lastVertexArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBuffer);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, lastElementArrayBuffer);
                    GL.BlendEquationSeparate((BlendEquationMode)lastBlendEquationRgb, (BlendEquationMode)lastBlendEquationAlpha);
                    GL.BlendFuncSeparate((ArbDrawBuffersBlend)lastBlendSrcRgb, (ArbDrawBuffersBlend)lastBlendDstRgb, (ArbDrawBuffersBlend)lastBlendSrcAlpha, (ArbDrawBuffersBlend)lastBlendDstAlpha);

                    if (lastEnableBlend) GL.Enable(EnableCap.Blend); else GL.Disable(EnableCap.Blend);
                    if (lastEnableCullFace) GL.Enable(EnableCap.CullFace); else GL.Disable(EnableCap.CullFace);
                    if (lastEnableDepthTest) GL.Enable(EnableCap.DepthTest); else GL.Disable(EnableCap.DepthTest);
                    if (lastEnableScissorTest) GL.Enable(EnableCap.ScissorTest); else GL.Disable(EnableCap.ScissorTest);
                    
                    GL.Viewport(lastViewport[0], lastViewport[1], lastViewport[2], lastViewport[3]);
                    GL.Scissor(lastScissorBox[0], lastScissorBox[1], lastScissorBox[2], lastScissorBox[3]);

                    if (GL.GetError() != ErrorCode.NoError)
                    {
                        Console.WriteLine(GL.GetError());
                        //Debugger.Break();
                    }

                }
            }

            private string GetGlDebugMessages()
            {
                int maxMessageLength = (int)All.MaxDebugMessageLength;

                DebugSource sources;
                DebugType types;
                DebugSeverity severity;
                int ids;
                int lengths;
                StringBuilder msgLog = new StringBuilder();

                //while (GL.GetDebugMessageLog(1, maxMessageLength, out sources, out types, out ids, out severity,  out lengths, msgLog) != 0)
                //{
                    
                //}
                return ("");
            }

        }
    }
}

//private unsafe void renderImDrawData(DrawData* drawData)
//{
//    //GL.Viewport(0, 0, Width, Height);

//    ////GL.ClearColor(Color4.Black);

//    //int lastTexture;
//    //GL.GetInteger(GetPName.TextureBinding2D, out lastTexture);

//    //GL.PushAttrib(AttribMask.EnableBit | AttribMask.ColorBufferBit | AttribMask.TransformBit);
//    //GL.Enable(EnableCap.Blend);
//    //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

//    //GL.Disable(EnableCap.CullFace);
//    //GL.Disable(EnableCap.DepthTest);
//    //GL.Disable(EnableCap.ScissorTest);

//    //GL.EnableClientState(ArrayCap.VertexArray);
//    //GL.EnableClientState(ArrayCap.TextureCoordArray);
//    //GL.EnableClientState(ArrayCap.ColorArray);
//    //GL.Enable(EnableCap.Texture2D);

//    //GL.UseProgram(0);

//    //IO io = ImGui.GetIO();
//    //ImGui.ScaleClipRects(drawData, io.DisplayFramebufferScale);

//    //GL.MatrixMode(MatrixMode.Projection);
//    //GL.PushMatrix();
//    //GL.LoadIdentity();

//    //GL.Ortho(
//    //    0.0f,
//    //    io.DisplaySize.X / io.DisplayFramebufferScale.X,
//    //    io.DisplaySize.Y / io.DisplayFramebufferScale.Y,
//    //    0.0f,
//    //    -1.0f,
//    //    1.0f);

//    //GL.MatrixMode(MatrixMode.Modelview);
//    //GL.PushMatrix();
//    //GL.LoadIdentity();

//    //for (int i = 0; i < drawData->CmdListsCount; i++)
//    //{
//    //    NativeDrawList* cmd_list = drawData->CmdLists[i];
//    //    byte* vertexBuffer = (byte*)cmd_list->VtxBuffer.Data;
//    //    ushort* idxBuffer = (ushort*)cmd_list->IdxBuffer.Data;

//    //    DrawVert vert0 = *((DrawVert*)vertexBuffer);
//    //    DrawVert vert1 = *(((DrawVert*)vertexBuffer)+1);
//    //    DrawVert vert2 = *(((DrawVert*)vertexBuffer)+2);

//    //    GL.VertexPointer(2, VertexPointerType.Float, sizeof(DrawVert), new IntPtr(vertexBuffer+DrawVert.PosOffset));
//    //    GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(DrawVert), new IntPtr(vertexBuffer + DrawVert.UVOffset));
//    //    GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(DrawVert), new IntPtr(vertexBuffer + DrawVert.ColOffset));

//    //    for (int cmd_i = 0; cmd_i < cmd_list->CmdBuffer.Size; cmd_i++)
//    //    {
//    //        DrawCmd* pcmd = &(((DrawCmd*)cmd_list->CmdBuffer.Data)[cmd_i]);

//    //        if (pcmd->UserCallback != IntPtr.Zero)
//    //        {
//    //            throw new NotImplementedException();
//    //        } else
//    //        {
//    //            GL.BindTexture(TextureTarget.Texture2D, pcmd->TextureId.ToInt32());
//    //            GL.Scissor(
//    //                (int)pcmd->ClipRect.X,
//    //                (int)(io.DisplaySize.Y - pcmd->ClipRect.W),
//    //                (int)(pcmd->ClipRect.Z - pcmd->ClipRect.X),
//    //                (int)(pcmd->ClipRect.W - pcmd->ClipRect.Y));

//    //            ushort[] indices = new ushort[pcmd->ElemCount];

//    //            for (int z = 0; z < indices.Length; z++)
//    //            {
//    //                indices[z] = idxBuffer[z];
//    //            }
//    //            GL.DrawElements(PrimitiveType.Triangles, (int)pcmd->ElemCount, DrawElementsType.UnsignedShort, new IntPtr(idxBuffer));
//    //        }

//    //        idxBuffer += pcmd->ElemCount;
//    //    }
//    //}

//    //GL.DisableClientState(ArrayCap.ColorArray);
//    //GL.DisableClientState(ArrayCap.TextureCoordArray);
//    //GL.DisableClientState(ArrayCap.VertexArray);
//    //GL.BindTexture(TextureTarget.Texture2D, lastTexture);
//    //GL.MatrixMode(MatrixMode.Modelview);
//    //GL.PopMatrix();
//    //GL.MatrixMode(MatrixMode.Projection);
//    //GL.PopMatrix();
//    //GL.PopAttrib();
//    ////GL.ClearColor(Color4.MediumPurple);

//    IO io = ImGui.GetIO();

//    int fbWidth = (int)(io.DisplaySize.X * io.DisplayFramebufferScale.X);
//    int fbHeight = (int)(io.DisplaySize.Y * io.DisplayFramebufferScale.Y);

//    if (fbWidth != 0 || fbHeight != 0)
//    {
//        ImGui.ScaleClipRects(drawData, io.DisplayFramebufferScale);
//        GL.ActiveTexture(0);

//        int lastProgram; GL.GetInteger(GL.ActiveShaderProgram, lastProgram);

//    }
//}