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
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Forge
{
    internal partial class MainWindow //: IDisposable TODO make disposable
    {
        public bool WindowOpen = false;
        public bool DebugEnabled = false;
        public bool WireframeEnabled = false;
        public bool ShowConsole = false;
        public bool ShowOpenGLConsole = false;
        private int _fontTexture;
        private int _scaleFactor;
        private static float _wheelPos;
        private static int _guiShaderHandle = 0, _vertHandle = 0, _fragHandle = 0;
        private static int _guiAttribLocationTex = 0, _guiAttribLocationProjMtx = 0;
        private static int _attribLocationPosition = 0, _attribLocationUv = 0, _attribLocationColor = 0;
        private static int _guiVboHandle = 0, _guiVaoHandle = 0, _elementsHandle = 0;

        private const int _textInputBufferLength = 1024;
        private IntPtr _textInputBuffer;

        private const Int32 ICON_MIN_FA = 0xf000;
        private const Int32 ICON_MAX_FA = 0xf2e0;

        /// <summary>
        /// Creates the device objects, initializes GUI shaders, and the font texture.
        /// </summary>
        private unsafe void CreateDeviceObjects()
        {
            //Backup OpenGL state so it can be restored after creating new shaders/program
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

            //Create and compile shaders into program
            _guiShaderHandle = GL.CreateProgram();
            _vertHandle = GL.CreateShader(ShaderType.VertexShader);
            _fragHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(_vertHandle, vertShader);
            GL.ShaderSource(_fragHandle, fragShader);

            GL.CompileShader(_vertHandle);
            GL.CompileShader(_fragHandle);

            GL.AttachShader(_guiShaderHandle, _vertHandle);
            GL.AttachShader(_guiShaderHandle, _fragHandle);

            if (GL.GetShaderInfoLog(_vertHandle) != "")
            {
                Console.WriteLine("Vert Error: " + GL.GetShaderInfoLog(_vertHandle));
            }
            if (GL.GetShaderInfoLog(_fragHandle) != "")
            {
                Console.WriteLine("Frag Error: " + GL.GetShaderInfoLog(_fragHandle));
            }
                
                
            GL.LinkProgram(_guiShaderHandle);

            if (GL.GetProgramInfoLog(_guiShaderHandle) != "")
            {
                Console.WriteLine("Program Error: "+GL.GetProgramInfoLog(_guiShaderHandle));
            }
                
            //Get locations for Texture, Projection Matrix, Position, UV, and Color fields
            _guiAttribLocationTex = GL.GetUniformLocation(_guiShaderHandle, "Texture");
            _guiAttribLocationProjMtx = GL.GetUniformLocation(_guiShaderHandle, "ProjMtx");
            _attribLocationPosition = GL.GetAttribLocation(_guiShaderHandle, "Position");
            _attribLocationUv = GL.GetAttribLocation(_guiShaderHandle, "UV");
            _attribLocationColor = GL.GetAttribLocation(_guiShaderHandle, "Color");
            
            //Gen vbo, vao, and element array
            GL.GenBuffers(1, out _guiVboHandle);
            GL.GenBuffers(1, out _elementsHandle);

            GL.GenVertexArrays(1, out _guiVaoHandle);
            GL.BindVertexArray(_guiVaoHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _guiVboHandle);

            GL.EnableVertexAttribArray(_attribLocationPosition);
            GL.EnableVertexAttribArray(_attribLocationUv);
            GL.EnableVertexAttribArray(_attribLocationColor);

            //Setup attributes for Location, UV, and Color

            GL.VertexAttribPointer(
                _attribLocationPosition,
                2,
                VertexAttribPointerType.Float,
                false,
                sizeof(DrawVert),
                new IntPtr(DrawVert.PosOffset)
                );

            GL.VertexAttribPointer(
                _attribLocationUv,
                2,
                VertexAttribPointerType.Float,
                false,
                sizeof(DrawVert),
                new IntPtr(DrawVert.UVOffset)
                );

            GL.VertexAttribPointer(
                _attribLocationColor,
                4,
                VertexAttribPointerType.UnsignedByte,
                true,
                sizeof(DrawVert),
                new IntPtr(DrawVert.ColOffset)
                );

            //Create fonts
            IO io = ImGui.GetIO();
            FontConfig config;
            
            //Get font from file and add as RGBA32
            io.FontAtlas.AddFontFromFileTTF(new Uri(_baseDirectory + "/Resources/Fonts/Roboto-Regular.ttf").LocalPath, 18.0f);

            //TODO add fontawsome
            //Merge normal font texture with font awesome fonts
            //config.MergeMode = true;
            //io.FontAtlas.AddFontFromFileTTF(new Uri(_baseDirectory + "/Resources/Fonts/fontawesome-webfont.ttf").LocalPath, 18);
            //char[] iconRanges = new[] {ICON_MIN_FA, ICON_MAX_FA, 0 };

            FontTextureData texData = io.FontAtlas.GetTexDataAsRGBA32();

            //Create font texture
            _fontTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
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

            io.FontAtlas.SetTexID(_fontTexture);
            io.FontAtlas.ClearTexData();

            GL.BindTexture(TextureTarget.Texture2D, 1);

            //Restore old OpenGL state
            GL.BindTexture(TextureTarget.Texture2D, lastTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBuffer);
            GL.BindVertexArray(lastVertexArray);
        }

        public unsafe void InitializeGui()
        {
            CreateDeviceObjects();
            
            //Create text input buffer?
            _textInputBuffer = Marshal.AllocHGlobal(_textInputBufferLength);
            long* ptr = (long*) _textInputBuffer.ToPointer();
            for (int i = 0; i < _textInputBufferLength / sizeof(long); i++)
            {
                ptr[i] = 0;
            }

            //SetUpImguiStyle();

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.Texture2D);

            WindowOpen = true;
        }

        public unsafe void RenderGui()
        {
            //Get device IO
            IO io = ImGui.GetIO();
            io.DeltaTime = 1f / 60f;
            io.DisplaySize = new System.Numerics.Vector2(Width, Height);

            //Update Inputs
            UpdateImguiInput(io);
            
            //Submit GUI
            SubmitGui();

            //Render gui
            ImGui.Render();

            //Process operating system events
            ProcessEvents();

            //Get draw data from Imgui
            DrawData* data = ImGui.GetDrawData();

            //Render data
            RenderImDrawData(data);
        }
        
        private void UpdateImguiInput(IO io)
        {
            MouseState cursorState = Mouse.GetCursorState();
            MouseState mouseState = Mouse.GetState();
            
            if (Bounds.Contains(cursorState.X, cursorState.Y))
            {
                Point windowPoint = PointToClient(new Point(cursorState.X, cursorState.Y));
                io.MousePosition = new System.Numerics.Vector2(windowPoint.X / io.DisplayFramebufferScale.X, windowPoint.Y / io.DisplayFramebufferScale.Y);
            }
            else
            {
                io.MousePosition = new System.Numerics.Vector2(-1f, -1f);
            }

            io.MouseDown[0] = mouseState.LeftButton == ButtonState.Pressed;
            io.MouseDown[1] = mouseState.RightButton == ButtonState.Pressed;
            io.MouseDown[2] = mouseState.MiddleButton == ButtonState.Pressed;

            float newWheelPos = mouseState.WheelPrecise;
            float deltaWheelPos = newWheelPos - _wheelPos;
            _wheelPos = newWheelPos;
            io.MouseWheel = deltaWheelPos;
        }


        private static void UpdateModifiers(KeyboardKeyEventArgs e)
        {
            IO io = ImGui.GetIO();
            io.AltPressed = e.Alt;
            io.CtrlPressed = e.Control;
            io.ShiftPressed = e.Shift;
        }
        
            
        private unsafe void RenderImDrawData(DrawData* drawData)
        {
            IO io = ImGui.GetIO();

            int fbWidth = (int)(io.DisplaySize.X * io.DisplayFramebufferScale.X);
            int fbHeight = (int)(io.DisplaySize.Y * io.DisplayFramebufferScale.Y);
            
            //Don't render if window has no width or height
            if (fbWidth != 0 || fbHeight != 0)
            {
                ImGui.ScaleClipRects(drawData, io.DisplayFramebufferScale);

                //Backup previous OpenGL state
                int lastActiveTexture; GL.GetInteger(GetPName.ActiveTexture, out lastActiveTexture);

                GL.ActiveTexture(TextureUnit.Texture0);

                int lastProgram; GL.GetInteger(GetPName.CurrentProgram, out lastProgram);
                int lastTexture; GL.GetInteger(GetPName.TextureBinding2D, out lastTexture);
                int lastArrayBuffer; GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBuffer);
                int lastElementArrayBuffer; GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBuffer);
                int lastVertexArray; GL.GetInteger(GetPName.VertexArrayBinding, out lastVertexArray);
                int lastBlendSrcRgb; GL.GetInteger(GetPName.BlendSrcRgb, out lastBlendSrcRgb);
                int lastBlendDstRgb; GL.GetInteger(GetPName.BlendDstRgb, out lastBlendDstRgb);
                int lastBlendSrcAlpha; GL.GetInteger(GetPName.BlendSrcAlpha, out lastBlendSrcAlpha);
                int lastBlendDstAlpha; GL.GetInteger(GetPName.BlendDstAlpha, out lastBlendDstAlpha);
                int lastBlendEquationRgb; GL.GetInteger(GetPName.BlendEquationRgb, out lastBlendEquationRgb);
                int lastBlendEquationAlpha; GL.GetInteger(GetPName.BlendEquationAlpha, out lastBlendEquationAlpha);

                int[] lastViewport = new int[4]; GL.GetInteger(GetPName.Viewport, lastViewport);
                int[] lastScissorBox = new int[4]; GL.GetInteger(GetPName.ScissorBox, lastScissorBox);

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

                //Create ortho matrix of the viewport
                Matrix4 orthoProjection = new Matrix4(
                    2.0f / io.DisplaySize.X, 0.0f, 0.0f, 0.0f,
                    0.0f, 2.0f / -io.DisplaySize.Y, 0.0f, 0.0f,
                    0.0f, 0.0f, -1.0f, 0.0f,
                    -1.0f, 1.0f, 0.0f, 1.0f);

                //Tell OpenGL to use the gui shader program
                GL.UseProgram(_guiShaderHandle);
                GL.Uniform1(_guiAttribLocationTex, 0);
                GL.UniformMatrix4(_guiAttribLocationProjMtx,false, ref orthoProjection);
                GL.BindVertexArray(_guiVaoHandle);
                
                //Get UV, vertices, and color from Imgui draw data, then render it through OpenGL
                for (int i = 0; i < drawData->CmdListsCount; i++)
                {
                    NativeDrawList* cmdList = drawData->CmdLists[i];
                    ushort* idxBuffer = (ushort*) 0;
                    
                    GL.BindBuffer(BufferTarget.ArrayBuffer, _guiVboHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, cmdList->VtxBuffer.Size * sizeof(DrawVert), new IntPtr(cmdList->VtxBuffer.Data), BufferUsageHint.StreamDraw );

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementsHandle);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, cmdList->IdxBuffer.Size * sizeof(ushort*), new IntPtr(cmdList->IdxBuffer.Data), BufferUsageHint.StreamDraw);

                    for (int cmd_i = 0; cmd_i < cmdList->CmdBuffer.Size; cmd_i++ )
                    {
                        //DrawCmd* pcmd = &((DrawCmd*)cmdList->CmdBuffer.Data)[cmd_i];

                        //if (pcmd->UserCallback != IntPtr.Zero)
                        //{
                        //    Console.WriteLine("User callback not implemented");
                        //    throw new NotImplementedException();
                        //}
                        //else
                        //{
                        //    //GL.BindTexture(TextureTarget.Texture2D, (int)pcmd->TextureId);
                        //    GL.BindTexture(TextureTarget.Texture2D, pcmd->TextureId.ToInt32());
                        //    GL.Scissor(
                        //        (int)pcmd->ClipRect.X,
                        //        (int)(fbHeight - pcmd->ClipRect.W),
                        //        (int)(pcmd->ClipRect.Z - pcmd->ClipRect.X),
                        //        (int)(pcmd->ClipRect.W - pcmd->ClipRect.Y));

                        //    GL.DrawElements(PrimitiveType.Triangles, (int)pcmd->ElemCount, DrawElementsType.UnsignedShort, new IntPtr(idxBuffer));
                        //}

                        //idxBuffer += pcmd->ElemCount;
                    }
                }
                //Restore old OpenGL state
                GL.UseProgram(lastProgram);
                GL.BindTexture(TextureTarget.Texture2D, lastTexture);

                GL.ActiveTexture((TextureUnit)lastActiveTexture);

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

                //if (GL.GetError() != ErrorCode.NoError)
                //{
                //    Console.WriteLine(GL.GetError());
                //    //Debugger.Break();
                //}

            }
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

        ////On dispose shutdown Imgui and mark this for garbage collection
        //public void Dispose()
        //{
        //    ImGui.Shutdown();
        //    GC.SuppressFinalize(this);
        //}

    }
}
