using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Forge.Rendering.GUI;
using ImGuiNET;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

namespace Forge
{//TODO modify imgui to accept opentk vectors
    internal partial class MainWindow
    {
        private float _secondaryMenuBarOffset = 0;
        private static readonly GuiStyle StyleRef = new GuiStyle();

        private GuiStyle guiStyle;
        //private UTF8Encoding _utf8 = new UTF8Encoding();

        private void SubmitGui()
        {
            ImGui.NewFrame();

            //Make a new GUI window with the same size as the OpenGL window
            //ImGui.SetNextWindowSize(new Vector2(Width, Height), SetCondition.FirstUseEver);
            //ImGui.SetNextWindowPosCenter(SetCondition.Always);
			

            ImGui.PushStyleColor(ColorTarget.WindowBg, new Vector4(0, 0, 0, 0));
            ImGui.BeginWindow("Main Window", ref WindowOpen, WindowFlags.NoResize | WindowFlags.NoTitleBar | WindowFlags.NoMove);
            ImGui.PopStyleColor();

            //Create main menu bar
            SubmitMenuBar();
            
            //Create the menu bar below the main menu bar
            SubmitSecondaryMenuBar();

            //Popup windows
            ImGui.PushStyleVar(StyleVar.WindowMinSize, new Vector2(80, 80));
            if (ShowConsole)
            {
                ImGui.BeginWindow("Console", ref ShowConsole, WindowFlags.Default);
                //ImGui.Text(stringWriter.ToString());
                ImGui.EndWindow();
            }

            if (ShowOpenGLConsole)
            {
                ImGui.BeginWindow("OpenGL Console", ref ShowOpenGLConsole, WindowFlags.Default);
                //Create callback
                //GetGlDebugMessages();
                
                ImGui.EndWindow();
            }
            
            //ShowStyleEditor();

            ImGui.PopStyleVar();
            ImGui.EndWindow();
            
        }

        private void SubmitSecondaryMenuBar()
        {
            //Set position under main menu bar
            //ImGui.SetNextWindowPos(new Vector2(0, _secondaryMenuBarOffset), SetCondition.Always);
            //ImGui.SetNextWindowSize(new Vector2(Width, 40), SetCondition.Always);

            ImGui.PushStyleVar(StyleVar.WindowPadding, new Vector2(5f, 5f));
            ImGui.PushStyleVar(StyleVar.WindowRounding, 0);
            Style style = ImGui.GetStyle();

            ImGui.BeginWindow("inner window",
                WindowFlags.NoMove | WindowFlags.NoTitleBar | WindowFlags.NoResize | WindowFlags.NoScrollbar |
                WindowFlags.NoScrollWithMouse);

            //Calculate the height the button should be so that it is vertically centered on the bar
            var padding = style.FramePadding;
            float centeredHeight = ImGui.GetWindowHeight() - 2 * padding.Y;

            ImGui.SameLine(0, 5f);

            if (ImGui.Button("Btn1", new Vector2(centeredHeight, centeredHeight)))
            {

            }

            ImGui.SameLine();
            
            if (ImGui.Button(("Button2")))
            {

            }

            ImGui.SameLine();

            if (ImGui.Button("test"))
            {
                
            }

            ImGui.PopStyleVar(2);
            ImGui.EndWindow();
            
        }

        /// <summary>
        /// Layout for the menu bar
        /// </summary>
        private void SubmitMenuBar()
        {
            ImGui.PushStyleVar(StyleVar.WindowPadding, new Vector2(4, 4));
            ImGui.PushStyleColor(ColorTarget.ModalWindowDarkening, new Vector4(1, .5f, .25f, .25f));

            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                
                if (ImGui.MenuItem("New", "Ctrl+N"))
                {
                }

                //This expands the drowdown menu's width
                ImGui.SameLine(300);
                ImGui.Text("");

                if (ImGui.MenuItem("Open...", "Ctrl+O"))
                {
                    
                }

                if (ImGui.MenuItem("Close"))
                {
                    
                }

                if (ImGui.MenuItem("Save", "Ctrl+S", false, false))
                {
                    
                }

                if (ImGui.MenuItem("Save As...", false))
                {
                    
                }
                
                ImGui.Separator();

                if (ImGui.MenuItem("Exit"))
                {
                    
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Undo", "Ctrl+Z", false, false))
                {
                }
                if (ImGui.MenuItem("Redo", "Ctrl+Y", false, false))
                {
                    
                }


                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("View"))
            {
                if (ImGui.MenuItem("Wireframe Mode", "", WireframeEnabled, true))
                {
                    WireframeEnabled = !WireframeEnabled;
                    Console.WriteLine("Wireframe Mode:" + WireframeEnabled);
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Options"))
            {
                if (ImGui.MenuItem("Debug Mode", "", DebugEnabled, true))
                {
                    DebugEnabled = !DebugEnabled;
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

            if (DebugEnabled)
            {
                if (ImGui.BeginMenu("Debug"))
                {
                    if (ImGui.MenuItem("Console"))
                    {
                        ShowConsole = !ShowConsole;
                    }

                    if (ImGui.MenuItem("OpenGL Console"))
                    {
                        ShowOpenGLConsole = !ShowOpenGLConsole;
                    }
                    ImGui.EndMenu();
                }
            }
            ImGui.PopStyleVar();
            ImGui.PopStyleColor();
            //Get height so secondary menu bar can be placed directly below
            _secondaryMenuBarOffset = ImGui.GetWindowHeight();
            ImGui.EndMainMenuBar();

        }

        //Source https://gist.github.com/dougbinks/8089b4bbaccaaf6fa204236978d165a9#file-imguiutils-h-L9-L93
        private void SetUpImguiStyle()
        {
            Style imguiStyle = ImGui.GetStyle();

            guiStyle = new GuiStyle();

            guiStyle.ReadFromFile(new Uri(_baseDirectory + "/Resources/Styles/Default.json"));

            guiStyle.ApplyStyle(ref imguiStyle);
        }

        private void ShowStyleEditor()
        {
            ImGui.BeginWindow("Style Editor");

            //If guiStyle has changed, show button to revert changes
            if (!guiStyle.Equals(StyleRef))
            {
                ImGui.SameLine();
                if (ImGui.Button("Revert Style"))
                {
                    guiStyle = StyleRef;
                    //Style imguiStyle = ImGui.GetStyle();
                    //guiStyle.ApplyStyle(ref imguiStyle);
                }
            }

            if (ImGui.CollapsingHeader("Rendering", "renderingOptions", true, true))
            {
                ImGui.Checkbox("AntiAliased Lines", ref guiStyle.AntiAliasedLines);
                ImGui.Checkbox("AntiAliased Shapes", ref guiStyle.AntiAliasedShapes);
                //ImGui.SliderFloat("Curve Tessellation Tolerence", ref guiStyle.CurveTessellationTolerance, 0, 10, guiStyle.CurveTessellationTolerance.ToString(), 1);
                ImGui.SliderFloat("Global Alpha", ref guiStyle.Alpha, 0.2f, 1, Math.Round(guiStyle.Alpha, 2).ToString(), 1);
            }

            if (ImGui.CollapsingHeader("Settings", "settings", true, true))
            {
                Int2 winPadding = new Int2();
                ImGui.SliderInt2("Window Padding", ref winPadding, 0, 20, "Test");
                
            }
            
            Style imguiStyle = ImGui.GetStyle();
            guiStyle.ApplyStyle(ref imguiStyle);
            ImGui.EndWindow();
        }

        //Source https://www.unknowncheats.me/forum/direct3d/189635-imgui-guiStyle-settings.html
        //private void SetUpImguiStyle()
        //{
        //    Style guiStyle = ImGui.GetStyle();

        //    guiStyle.WindowRounding = 0.0f;
        //    guiStyle.WindowPadding = new Vector2(15, 15);
        //    guiStyle.FramePadding = new Vector2(5, 5);
        //    guiStyle.FrameRounding = 0.0f;
        //    guiStyle.ItemSpacing = new Vector2(12, 8);
        //    guiStyle.ItemInnerSpacing = new Vector2(8, 6);
        //    guiStyle.IndentSpacing = 25.0f;
        //    guiStyle.ScrollbarSize = 15.0f;
        //    guiStyle.ScrollbarRounding = 9.0f;
        //    guiStyle.GrabMinSize = 5.0f;
        //    guiStyle.GrabRounding = 3.0f;

        //    guiStyle.SetColor(ColorTarget.Text, new Vector4(0.80f, 0.80f, 0.83f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.TextDisabled, new Vector4(0.24f, 0.23f, 0.29f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.WindowBg, new Vector4(0.06f, 0.05f, 0.07f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ChildWindowBg, new Vector4(0.07f, 0.07f, 0.09f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.Border, new Vector4(0.80f, 0.80f, 0.83f, 0.88f));
        //    guiStyle.SetColor(ColorTarget.BorderShadow, new Vector4(0.92f, 0.91f, 0.88f, 0.00f));
        //    guiStyle.SetColor(ColorTarget.FrameBg, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.FrameBgHovered, new Vector4(0.24f, 0.23f, 0.29f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.FrameBgActive, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.TitleBg, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.TitleBgCollapsed, new Vector4(1.00f, 0.98f, 0.95f, 0.75f));
        //    guiStyle.SetColor(ColorTarget.TitleBgActive, new Vector4(0.07f, 0.07f, 0.09f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.MenuBarBg, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ScrollbarBg, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ScrollbarGrab, new Vector4(0.80f, 0.80f, 0.83f, 0.31f));
        //    guiStyle.SetColor(ColorTarget.ScrollbarGrabHovered, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ScrollbarGrabActive, new Vector4(0.06f, 0.05f, 0.07f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ComboBg, new Vector4(0.19f, 0.18f, 0.21f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.CheckMark, new Vector4(0.80f, 0.80f, 0.83f, 0.31f));
        //    guiStyle.SetColor(ColorTarget.SliderGrab, new Vector4(0.80f, 0.80f, 0.83f, 0.31f));
        //    guiStyle.SetColor(ColorTarget.SliderGrabActive, new Vector4(0.06f, 0.05f, 0.07f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.Button, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ButtonHovered, new Vector4(0.24f, 0.23f, 0.29f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ButtonActive, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.Header, new Vector4(0.10f, 0.09f, 0.12f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.HeaderHovered, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.HeaderActive, new Vector4(0.06f, 0.05f, 0.07f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.Column, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ColumnHovered, new Vector4(0.24f, 0.23f, 0.29f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ColumnActive, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ResizeGrip, new Vector4(0.00f, 0.00f, 0.00f, 0.00f));
        //    guiStyle.SetColor(ColorTarget.ResizeGripHovered, new Vector4(0.56f, 0.56f, 0.58f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.ResizeGripActive, new Vector4(0.06f, 0.05f, 0.07f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.CloseButton, new Vector4(0.40f, 0.39f, 0.38f, 0.16f));
        //    guiStyle.SetColor(ColorTarget.CloseButtonHovered, new Vector4(0.40f, 0.39f, 0.38f, 0.39f));
        //    guiStyle.SetColor(ColorTarget.CloseButtonActive, new Vector4(0.40f, 0.39f, 0.38f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.PlotLines, new Vector4(0.40f, 0.39f, 0.38f, 0.63f));
        //    guiStyle.SetColor(ColorTarget.PlotLinesHovered, new Vector4(0.25f, 1.00f, 0.00f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.PlotHistogram, new Vector4(0.40f, 0.39f, 0.38f, 0.63f));
        //    guiStyle.SetColor(ColorTarget.PlotHistogramHovered, new Vector4(0.25f, 1.00f, 0.00f, 1.00f));
        //    guiStyle.SetColor(ColorTarget.TextSelectedBg, new Vector4(0.25f, 1.00f, 0.00f, 0.43f));
        //    guiStyle.SetColor(ColorTarget.ModalWindowDarkening, new Vector4(1.00f, 0.98f, 0.95f, 0.73f));

        //}
    }
}
