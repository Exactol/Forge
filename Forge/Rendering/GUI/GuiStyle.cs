using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Controls;
using ImGuiNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Forge.Rendering.GUI
{
    class GuiStyle
    {
        //Style information
        public string StyleName = "Default Style Name";
        public string StyleDescription = "Default Description";
        public float TargetVersion = 1.0f;
        public float StyleVersion = 1.0f;

        //Styles
        public Vector2 WindowPadding;
        public Vector2 FramePadding;
        public Vector2 WindowMinSize;
        public float Alpha;
        public bool AntiAliasedLines;
        public bool AntiAliasedShapes;
        public float ChildWindowRounding;
        public float ColumnsMinSpacing;
        public float CurveTessellationTolerance;
        public Vector2 DisplaySafeAreaPadding;
        public Vector2 DisplayWindowPadding;
        public float FrameRounding;
        public float GrabMinSize;
        public float GrabRounding;
        public float IndentSpacing;
        public Vector2 ItemInnerSpacing;
        public Vector2 ItemSpacing;
        public float ScrollbarRounding;
        public float ScrollbarSize;
        public Vector2 TouchExtraPadding;
        public float WindowRounding;
        public Align WindowTitleAlign;

        //Colors
        public Vector4 Border;
        public Vector4 BorderShadow;
        public Vector4 Button;
        public Vector4 ButtonActive;
        public Vector4 ButtonHovered;
        public Vector4 CheckMark;
        public Vector4 ChildWindowBg;
        public Vector4 CloseButton;
        public Vector4 CloseButtonActive;
        public Vector4 CloseButtonHovered;
        public Vector4 Column;
        public Vector4 ColumnActive;
        public Vector4 ColumnHovered;
        public Vector4 ComboBg;
        public Vector4 FrameBg;
        public Vector4 FrameBgActive;
        public Vector4 FrameBgHovered;
        public Vector4 Header;
        public Vector4 HeaderActive;
        public Vector4 HeaderHovered;
        public Vector4 MenuBarBg;
        public Vector4 ModalWindowDarkening;
        public Vector4 PlotHistogram;
        public Vector4 PlotHistogramHovered;
        public Vector4 PlotLines;
        public Vector4 PlotLinesHovered;
        public Vector4 ResizeGrip;
        public Vector4 ResizeGripActive;
        public Vector4 ResizeGripHovered;
        public Vector4 ScrollbarBg;
        public Vector4 ScrollbarGrab;
        public Vector4 ScrollbarGrabActive;
        public Vector4 ScrollbarGrabHovered;
        public Vector4 SliderGrab;
        public Vector4 SliderGrabActive;
        public Vector4 Text;
        public Vector4 TextDisabled;
        public Vector4 TextSelectedBg;
        public Vector4 TitleBg;
        public Vector4 TitleBgActive;
        public Vector4 TitleBgCollapsed;
        public Vector4 TooltipBg;
        public Vector4 WindowBg;

        //Initially set everything to default Imgui values
        public GuiStyle()
        {
            Style style = ImGui.GetStyle();
            WindowPadding = style.WindowPadding;
            FramePadding = style.FramePadding;
            WindowMinSize = style.WindowMinSize;
            Alpha = style.Alpha;
            AntiAliasedLines = style.AntiAliasedLines;
            AntiAliasedShapes = style.AntiAliasedShapes;
            ChildWindowRounding = style.ChildWindowRounding;
            ColumnsMinSpacing = style.ColumnsMinSpacing;
            CurveTessellationTolerance = style.CurveTessellationTolerance;
            DisplaySafeAreaPadding = style.DisplaySafeAreaPadding;
            DisplayWindowPadding = style.DisplayWindowPadding;
            FrameRounding = style.FrameRounding;
            GrabMinSize = style.GrabMinSize;
            GrabRounding = style.GrabRounding;
            IndentSpacing = style.IndentSpacing;
            ItemInnerSpacing = style.ItemInnerSpacing;
            ItemSpacing = style.ItemSpacing;
            ScrollbarRounding = style.ScrollbarRounding;
            ScrollbarSize = style.ScrollbarSize;
            TouchExtraPadding = style.TouchExtraPadding;
            WindowRounding = style.WindowRounding;
            WindowTitleAlign = style.WindowTitleAlign;

            Border = style.GetColor(ColorTarget.Border);
            BorderShadow = style.GetColor(ColorTarget.BorderShadow);
            Button = style.GetColor(ColorTarget.Button);
            ButtonActive = style.GetColor(ColorTarget.ButtonActive);
            ButtonHovered = style.GetColor(ColorTarget.ButtonHovered);
            CheckMark = style.GetColor(ColorTarget.CheckMark);
            ChildWindowBg = style.GetColor(ColorTarget.ChildWindowBg);
            CloseButton = style.GetColor(ColorTarget.CloseButton);
            CloseButtonActive = style.GetColor(ColorTarget.CloseButtonActive);
            CloseButtonHovered = style.GetColor(ColorTarget.CloseButtonHovered);
            Column = style.GetColor(ColorTarget.Column);
            ColumnActive = style.GetColor(ColorTarget.ColumnActive);
            ColumnHovered = style.GetColor(ColorTarget.ColumnHovered);
            ComboBg = style.GetColor(ColorTarget.ComboBg);
            FrameBg = style.GetColor(ColorTarget.FrameBg);
            FrameBgActive = style.GetColor(ColorTarget.FrameBgActive);
            FrameBgHovered = style.GetColor(ColorTarget.FrameBgHovered);
            Header = style.GetColor(ColorTarget.Header);
            HeaderActive = style.GetColor(ColorTarget.HeaderActive);
            HeaderHovered = style.GetColor(ColorTarget.HeaderHovered);
            MenuBarBg = style.GetColor(ColorTarget.MenuBarBg);
            ModalWindowDarkening = style.GetColor(ColorTarget.ModalWindowDarkening);
            PlotHistogram = style.GetColor(ColorTarget.PlotHistogram);
            PlotHistogramHovered = style.GetColor(ColorTarget.PlotHistogramHovered);
            PlotLines = style.GetColor(ColorTarget.PlotLines);
            PlotLinesHovered = style.GetColor(ColorTarget.PlotLinesHovered);
            ResizeGrip = style.GetColor(ColorTarget.ResizeGrip);
            ResizeGripActive = style.GetColor(ColorTarget.ResizeGripActive);
            ResizeGripHovered = style.GetColor(ColorTarget.ResizeGripHovered);
            ScrollbarBg = style.GetColor(ColorTarget.ScrollbarBg);
            ScrollbarGrab = style.GetColor(ColorTarget.ScrollbarGrab);
            ScrollbarGrabActive = style.GetColor(ColorTarget.ScrollbarGrabActive);
            ScrollbarGrabHovered = style.GetColor(ColorTarget.ScrollbarGrabHovered);
            SliderGrab = style.GetColor(ColorTarget.SliderGrab);
            SliderGrabActive = style.GetColor(ColorTarget.SliderGrabActive);
            Text = style.GetColor(ColorTarget.Text);
            TextDisabled = style.GetColor(ColorTarget.TextDisabled);
            TextSelectedBg = style.GetColor(ColorTarget.TextSelectedBg);
            TitleBg = style.GetColor(ColorTarget.TitleBg);
            TitleBgActive = style.GetColor(ColorTarget.TitleBgActive);
            TitleBgCollapsed = style.GetColor(ColorTarget.TitleBgCollapsed);
            WindowBg = style.GetColor(ColorTarget.WindowBg);

        }

        //Read style from json
        public void ReadFromFile(Uri filepath)
        {
            //Create text reader of json file
            TextReader textReader = new StreamReader(filepath.LocalPath);
            //Parse json
            JObject json = JObject.Parse(textReader.ReadToEnd());
            var guiStyle = json["GuiStyle"];
           
            //Read json properties, if not null set field to the property
            if (guiStyle["StyleName"] != null) StyleName = (string) guiStyle["StyleName"];
            if (guiStyle["StyleDescription"] != null) StyleDescription = (string) guiStyle["StyleDescription"];
            if (guiStyle["TargetVersion"] != null) TargetVersion = (float) guiStyle["TargetVersion"];
            if (guiStyle["StyleVersion"] != null) StyleVersion = (float) guiStyle["StyleVersion"];

            //Set style properties
            var styleVar = guiStyle["StyleVar"];
            if (styleVar["WindowPadding"] != null)
            {
                WindowPadding.X = (float)styleVar["WindowPadding"]["X"];
                WindowPadding.Y = (float)styleVar["WindowPadding"]["Y"];
            }
            if (styleVar["FramePadding"] != null)
            {
                FramePadding.X = (float) styleVar["FramePadding"]["X"];
                FramePadding.Y = (float) styleVar["FramePadding"]["Y"];
            }
            if (styleVar["WindowMinSize"] != null)
            {
                WindowMinSize.X = (float) styleVar["WindowMinSize"]["X"];
                WindowMinSize.Y = (float) styleVar["WindowMinSize"]["Y"];
            }
            if (styleVar["Alpha"] != null) Alpha = (float) styleVar["Alpha"];
            if (styleVar["AntiAliasedLines"] != null) AntiAliasedLines = (bool) styleVar["AntiAliasedLines"];
            if (styleVar["AntiAliasedShapes"] != null) AntiAliasedShapes = (bool) styleVar["AntiAliasedShapes"];
            if (styleVar["ChildWindowRounding"] != null) ChildWindowRounding = (float) styleVar["ChildWindowRounding"];
            if (styleVar["ColumnsMinSpacing"] != null) ColumnsMinSpacing = (float) styleVar["ColumnsMinSpacing"];
            if (styleVar["CurveTessellationTolerance"] != null) CurveTessellationTolerance = (float) styleVar["CurveTessellationTolerance"];
            if (styleVar["DisplayAreaPadding"] != null)
            {
                DisplaySafeAreaPadding.X = (float) styleVar["DisplaySafeAreaPadding"]["X"];
                DisplaySafeAreaPadding.Y = (float) styleVar["DisplaySafeAreaPadding"]["Y"];
            }
            if (styleVar["DisplayWindowPadding"] != null)
            {
                DisplayWindowPadding.X = (float) styleVar["DisplayWindowPadding"]["X"];
                DisplayWindowPadding.Y = (float) styleVar["DisplayWindowPadding"]["Y"];
            }
            if (styleVar["FrameRounding"] != null) FrameRounding = (float) styleVar["FrameRounding"];
            if (styleVar["GrabMinSize"] != null) GrabMinSize = (float) styleVar["GrabMinSize"];
            if (styleVar["GrabRounding"] != null) GrabRounding = (float) styleVar["GrabRounding"];
            if (styleVar["IndentSpacing"] != null) IndentSpacing = (float) styleVar["IndentSpacing"];
            if (styleVar["ItemInnerSpacing"] != null)
            {
                ItemInnerSpacing.X = (float) styleVar["ItemInnerSpacing"]["X"];
                ItemInnerSpacing.Y = (float) styleVar["ItemInnerSpacing"]["Y"];
            }
            if (styleVar["ItemSpacing"] != null)
            {
                ItemSpacing.X = (float)styleVar["ItemSpacing"]["X"];
                ItemSpacing.Y = (float)styleVar["ItemSpacing"]["Y"];
            }
            if (styleVar["ScrollbarRounding"] != null) ScrollbarRounding = (float) styleVar["ScrollbarRounding"];
            if (styleVar["ScrollbarSize"] != null) ScrollbarSize = (float) styleVar["ScrollbarSize"];
            if (styleVar["TouchExtraPadding"] != null)
            {
                TouchExtraPadding.X = (float) styleVar["TouchExtraPadding"]["X"];
                TouchExtraPadding.Y = (float) styleVar["TouchExtraPadding"]["Y"];
            }
            if (styleVar["WindowRounding"] != null) WindowRounding = (float) styleVar["WindowRounding"];

            if (styleVar["WindowTitleAlign"] != null)
            {
                int align = (int) styleVar["WindowTitleAlign"];
                
                switch (align)
                {
                    case 1:
                        WindowTitleAlign = Align.Left;
                        break;
                    case 2:
                        WindowTitleAlign = Align.Center;
                        break;
                    case 4:
                        WindowTitleAlign = Align.Right;
                        break;
                    case 8:
                        WindowTitleAlign = Align.Top;
                        break;
                    case 9:
                        WindowTitleAlign = Align.Default;
                        break;
                    case 16:
                        WindowTitleAlign = Align.VCenter;
                        break;
                }
            }

            //Set color properties
            var colorStyle = guiStyle["ColorVar"];
            if (colorStyle["Border"] != null)
            {
                Border.X = (float) colorStyle["Border"]["X"];
                Border.Y= (float)colorStyle["Border"]["Y"];
                Border.Z = (float)colorStyle["Border"]["Z"];
                Border.W = (float)colorStyle["Border"]["W"];
            }
            if (colorStyle["BorderShadow"] != null)
            {
                BorderShadow.X = (float)colorStyle["BorderShadow"]["X"];
                BorderShadow.Y = (float)colorStyle["BorderShadow"]["Y"];
                BorderShadow.Z = (float)colorStyle["BorderShadow"]["Z"];
                BorderShadow.W = (float)colorStyle["BorderShadow"]["W"];
            }
            if (colorStyle["Button"] != null)
            {
                Button.X = (float)colorStyle["Button"]["X"];
                Button.Y = (float)colorStyle["Button"]["Y"];
                Button.Z = (float)colorStyle["Button"]["Z"];
                Button.W = (float)colorStyle["Button"]["W"];
            }
            if (colorStyle["ButtonActive"] != null)
            {
                ButtonActive.X = (float)colorStyle["ButtonActive"]["X"];
                ButtonActive.Y = (float)colorStyle["ButtonActive"]["Y"];
                ButtonActive.Z = (float)colorStyle["ButtonActive"]["Z"];
                ButtonActive.W = (float)colorStyle["ButtonActive"]["W"];
            }
            if (colorStyle["ButtonHovered"] != null)
            {
                ButtonHovered.X = (float)colorStyle["ButtonHovered"]["X"];
                ButtonHovered.Y = (float)colorStyle["ButtonHovered"]["Y"];
                ButtonHovered.Z = (float)colorStyle["ButtonHovered"]["Z"];
                ButtonHovered.W = (float)colorStyle["ButtonHovered"]["W"];
            }
            if (colorStyle["CheckMark"] != null)
            {
                CheckMark.X = (float)colorStyle["CheckMark"]["X"];
                CheckMark.Y = (float)colorStyle["CheckMark"]["Y"];
                CheckMark.Z = (float)colorStyle["CheckMark"]["Z"];
                CheckMark.W = (float)colorStyle["CheckMark"]["W"];
            }
            if (colorStyle["ChildWindowBg"] != null)
            {
                ChildWindowBg.X = (float)colorStyle["ChildWindowBg"]["X"];
                ChildWindowBg.Y = (float)colorStyle["ChildWindowBg"]["Y"];
                ChildWindowBg.Z = (float)colorStyle["ChildWindowBg"]["Z"];
                ChildWindowBg.W = (float)colorStyle["ChildWindowBg"]["W"];
            }
            if (colorStyle["CloseButton"] != null)
            {
                CloseButton.X = (float)colorStyle["CloseButton"]["X"];
                CloseButton.Y = (float)colorStyle["CloseButton"]["Y"];
                CloseButton.Z = (float)colorStyle["CloseButton"]["Z"];
                CloseButton.W = (float)colorStyle["CloseButton"]["W"];
            }
            if (colorStyle["CloseButtonActive"] != null)
            {
                CloseButtonActive.X = (float)colorStyle["CloseButtonActive"]["X"];
                CloseButtonActive.Y = (float)colorStyle["CloseButtonActive"]["Y"];
                CloseButtonActive.Z = (float)colorStyle["CloseButtonActive"]["Z"];
                CloseButtonActive.W = (float)colorStyle["CloseButtonActive"]["W"];
            }
            if (colorStyle["CloseButtonHovered"] != null)
            {
                CloseButtonHovered.X = (float)colorStyle["CloseButtonHovered"]["X"];
                CloseButtonHovered.Y = (float)colorStyle["CloseButtonHovered"]["Y"];
                CloseButtonHovered.Z = (float)colorStyle["CloseButtonHovered"]["Z"];
                CloseButtonHovered.W = (float)colorStyle["CloseButtonHovered"]["W"];
            }
            if (colorStyle["Column"] != null)
            {
                Column.X = (float)colorStyle["Column"]["X"];
                Column.Y = (float)colorStyle["Column"]["Y"];
                Column.Z = (float)colorStyle["Column"]["Z"];
                Column.W = (float)colorStyle["Column"]["W"];
            }
            if (colorStyle["ColumnActive"] != null)
            {
                ColumnActive.X = (float)colorStyle["ColumnActive"]["X"];
                ColumnActive.Y = (float)colorStyle["ColumnActive"]["Y"];
                ColumnActive.Z = (float)colorStyle["ColumnActive"]["Z"];
                ColumnActive.W = (float)colorStyle["ColumnActive"]["W"];
            }
            if (colorStyle["ColumnHovered"] != null)
            {
                ColumnHovered.X = (float)colorStyle["ColumnHovered"]["X"];
                ColumnHovered.Y = (float)colorStyle["ColumnHovered"]["Y"];
                ColumnHovered.Z = (float)colorStyle["ColumnHovered"]["Z"];
                ColumnHovered.W = (float)colorStyle["ColumnHovered"]["W"];
            }
            if (colorStyle["ComboBg"] != null)
            {
                ComboBg.X = (float)colorStyle["ComboBg"]["X"];
                ComboBg.Y = (float)colorStyle["ComboBg"]["Y"];
                ComboBg.Z = (float)colorStyle["ComboBg"]["Z"];
                ComboBg.W = (float)colorStyle["ComboBg"]["W"];
            }
            if (colorStyle["FrameBg"] != null)
            {
                FrameBg.X = (float)colorStyle["FrameBg"]["X"];
                FrameBg.Y = (float)colorStyle["FrameBg"]["Y"];
                FrameBg.Z = (float)colorStyle["FrameBg"]["Z"];
                FrameBg.W = (float)colorStyle["FrameBg"]["W"];
            }
            if (colorStyle["FrameBgActive"] != null)
            {
                FrameBgActive.X = (float)colorStyle["FrameBgActive"]["X"];
                FrameBgActive.Y = (float)colorStyle["FrameBgActive"]["Y"];
                FrameBgActive.Z = (float)colorStyle["FrameBgActive"]["Z"];
                FrameBgActive.W = (float)colorStyle["FrameBgActive"]["W"];
            }
            if (colorStyle["FrameBgHovered"] != null)
            {
                FrameBgHovered.X = (float)colorStyle["FrameBgHovered"]["X"];
                FrameBgHovered.Y = (float)colorStyle["FrameBgHovered"]["Y"];
                FrameBgHovered.Z = (float)colorStyle["FrameBgHovered"]["Z"];
                FrameBgHovered.W = (float)colorStyle["FrameBgHovered"]["W"];
            }
            if (colorStyle["Header"] != null)
            {
                Header.X = (float)colorStyle["Header"]["X"];
                Header.Y = (float)colorStyle["Header"]["Y"];
                Header.Z = (float)colorStyle["Header"]["Z"];
                Header.W = (float)colorStyle["Header"]["W"];
            }
            if (colorStyle["HeaderActive"] != null)
            {
                HeaderActive.X = (float)colorStyle["HeaderActive"]["X"];
                HeaderActive.Y = (float)colorStyle["HeaderActive"]["Y"];
                HeaderActive.Z = (float)colorStyle["HeaderActive"]["Z"];
                HeaderActive.W = (float)colorStyle["HeaderActive"]["W"];
            }
            if (colorStyle["HeaderHovered"] != null)
            {
                HeaderHovered.X = (float)colorStyle["HeaderHovered"]["X"];
                HeaderHovered.Y = (float)colorStyle["HeaderHovered"]["Y"];
                HeaderHovered.Z = (float)colorStyle["HeaderHovered"]["Z"];
                HeaderHovered.W = (float)colorStyle["HeaderHovered"]["W"];
            }
            if (colorStyle["MenuBarBg"] != null)
            {
                MenuBarBg.X = (float)colorStyle["MenuBarBg"]["X"];
                MenuBarBg.Y = (float)colorStyle["MenuBarBg"]["Y"];
                MenuBarBg.Z = (float)colorStyle["MenuBarBg"]["Z"];
                MenuBarBg.W = (float)colorStyle["MenuBarBg"]["W"];
            }
            if (colorStyle["ModalWindowDarkening"] != null)
            {
                ModalWindowDarkening.X = (float)colorStyle["ModalWindowDarkening"]["X"];
                ModalWindowDarkening.Y = (float)colorStyle["ModalWindowDarkening"]["Y"];
                ModalWindowDarkening.Z = (float)colorStyle["ModalWindowDarkening"]["Z"];
                ModalWindowDarkening.W = (float)colorStyle["ModalWindowDarkening"]["W"];
            }
            if (colorStyle["PlotHistogram"] != null)
            {
                PlotHistogram.X = (float)colorStyle["PlotHistogram"]["X"];
                PlotHistogram.Y = (float)colorStyle["PlotHistogram"]["Y"];
                PlotHistogram.Z = (float)colorStyle["PlotHistogram"]["Z"];
                PlotHistogram.W = (float)colorStyle["PlotHistogram"]["W"];
            }
            if (colorStyle["PlotHistogramHovered"] != null)
            {
                PlotHistogramHovered.X = (float)colorStyle["PlotHistogramHovered"]["X"];
                PlotHistogramHovered.Y = (float)colorStyle["PlotHistogramHovered"]["Y"];
                PlotHistogramHovered.Z = (float)colorStyle["PlotHistogramHovered"]["Z"];
                PlotHistogramHovered.W = (float)colorStyle["PlotHistogramHovered"]["W"];
            }
            if (colorStyle["PlotLines"] != null)
            {
                PlotLines.X = (float)colorStyle["PlotLines"]["X"];
                PlotLines.Y = (float)colorStyle["PlotLines"]["Y"];
                PlotLines.Z = (float)colorStyle["PlotLines"]["Z"];
                PlotLines.W = (float)colorStyle["PlotLines"]["W"];
            }
            if (colorStyle["PlotLinesHovered"] != null)
            {
                PlotLinesHovered.X = (float)colorStyle["PlotLinesHovered"]["X"];
                PlotLinesHovered.Y = (float)colorStyle["PlotLinesHovered"]["Y"];
                PlotLinesHovered.Z = (float)colorStyle["PlotLinesHovered"]["Z"];
                PlotLinesHovered.W = (float)colorStyle["PlotLinesHovered"]["W"];
            }
            if (colorStyle["ResizeGrip"] != null)
            {
                ResizeGrip.X = (float)colorStyle["ResizeGrip"]["X"];
                ResizeGrip.Y = (float)colorStyle["ResizeGrip"]["Y"];
                ResizeGrip.Z = (float)colorStyle["ResizeGrip"]["Z"];
                ResizeGrip.W = (float)colorStyle["ResizeGrip"]["W"];
            }
            if (colorStyle["ResizeGripActive"] != null)
            {
                ResizeGripActive.X = (float)colorStyle["ResizeGripActive"]["X"];
                ResizeGripActive.Y = (float)colorStyle["ResizeGripActive"]["Y"];
                ResizeGripActive.Z = (float)colorStyle["ResizeGripActive"]["Z"];
                ResizeGripActive.W = (float)colorStyle["ResizeGripActive"]["W"];
            }
            if (colorStyle["ResizeGripHovered"] != null)
            {
                ResizeGripHovered.X = (float)colorStyle["ResizeGripHovered"]["X"];
                ResizeGripHovered.Y = (float)colorStyle["ResizeGripHovered"]["Y"];
                ResizeGripHovered.Z = (float)colorStyle["ResizeGripHovered"]["Z"];
                ResizeGripHovered.W = (float)colorStyle["ResizeGripHovered"]["W"];
            }
            if (colorStyle["ScrollbarBg"] != null)
            {
                ScrollbarBg.X = (float)colorStyle["ScrollbarBg"]["X"];
                ScrollbarBg.Y = (float)colorStyle["ScrollbarBg"]["Y"];
                ScrollbarBg.Z = (float)colorStyle["ScrollbarBg"]["Z"];
                ScrollbarBg.W = (float)colorStyle["ScrollbarBg"]["W"];
            }
            if (colorStyle["ScrollbarGrab"] != null)
            {
                ScrollbarGrab.X = (float)colorStyle["ScrollbarGrab"]["X"];
                ScrollbarGrab.Y = (float)colorStyle["ScrollbarGrab"]["Y"];
                ScrollbarGrab.Z = (float)colorStyle["ScrollbarGrab"]["Z"];
                ScrollbarGrab.W = (float)colorStyle["ScrollbarGrab"]["W"];
            }
            if (colorStyle["ScrollbarGrabActive"] != null)
            {
                ScrollbarGrabActive.X = (float)colorStyle["ScrollbarGrabActive"]["X"];
                ScrollbarGrabActive.Y = (float)colorStyle["ScrollbarGrabActive"]["Y"];
                ScrollbarGrabActive.Z = (float)colorStyle["ScrollbarGrabActive"]["Z"];
                ScrollbarGrabActive.W = (float)colorStyle["ScrollbarGrabActive"]["W"];
            }
            if (colorStyle["ScrollbarGrabHovered"] != null)
            {
                ScrollbarGrabHovered.X = (float)colorStyle["ScrollbarGrabHovered"]["X"];
                ScrollbarGrabHovered.Y = (float)colorStyle["ScrollbarGrabHovered"]["Y"];
                ScrollbarGrabHovered.Z = (float)colorStyle["ScrollbarGrabHovered"]["Z"];
                ScrollbarGrabHovered.W = (float)colorStyle["ScrollbarGrabHovered"]["W"];
            }
            if (colorStyle["SliderGrab"] != null)
            {
                SliderGrab.X = (float)colorStyle["SliderGrab"]["X"];
                SliderGrab.Y = (float)colorStyle["SliderGrab"]["Y"];
                SliderGrab.Z = (float)colorStyle["SliderGrab"]["Z"];
                SliderGrab.W = (float)colorStyle["SliderGrab"]["W"];
            }
            if (colorStyle["SliderGrabActive"] != null)
            {
                SliderGrabActive.X = (float)colorStyle["SliderGrabActive"]["X"];
                SliderGrabActive.Y = (float)colorStyle["SliderGrabActive"]["Y"];
                SliderGrabActive.Z = (float)colorStyle["SliderGrabActive"]["Z"];
                SliderGrabActive.W = (float)colorStyle["SliderGrabActive"]["W"];
            }
            if (colorStyle["Text"] != null)
            {
                Text.X = (float)colorStyle["Text"]["X"];
                Text.Y = (float)colorStyle["Text"]["Y"];
                Text.Z = (float)colorStyle["Text"]["Z"];
                Text.W = (float)colorStyle["Text"]["W"];
            }
            if (colorStyle["TextDisabled"] != null)
            {
                TextDisabled.X = (float)colorStyle["TextDisabled"]["X"];
                TextDisabled.Y = (float)colorStyle["TextDisabled"]["Y"];
                TextDisabled.Z = (float)colorStyle["TextDisabled"]["Z"];
                TextDisabled.W = (float)colorStyle["TextDisabled"]["W"];
            }
            if (colorStyle["TextSelectedBg"] != null)
            {
                TextSelectedBg.X = (float)colorStyle["TextSelectedBg"]["X"];
                TextSelectedBg.Y = (float)colorStyle["TextSelectedBg"]["Y"];
                TextSelectedBg.Z = (float)colorStyle["TextSelectedBg"]["Z"];
                TextSelectedBg.W = (float)colorStyle["TextSelectedBg"]["W"];
            }
            if (colorStyle["TitleBg"] != null)
            {
                TitleBg.X = (float)colorStyle["TitleBg"]["X"];
                TitleBg.Y = (float)colorStyle["TitleBg"]["Y"];
                TitleBg.Z = (float)colorStyle["TitleBg"]["Z"];
                TitleBg.W = (float)colorStyle["TitleBg"]["W"];
            }
            if (colorStyle["TitleBgActive"] != null)
            {
                TitleBgActive.X = (float)colorStyle["TitleBgActive"]["X"];
                TitleBgActive.Y = (float)colorStyle["TitleBgActive"]["Y"];
                TitleBgActive.Z = (float)colorStyle["TitleBgActive"]["Z"];
                TitleBgActive.W = (float)colorStyle["TitleBgActive"]["W"];
            }
            if (colorStyle["TitleBgCollapsed"] != null)
            {
                TitleBgCollapsed.X = (float)colorStyle["TitleBgCollapsed"]["X"];
                TitleBgCollapsed.Y = (float)colorStyle["TitleBgCollapsed"]["Y"];
                TitleBgCollapsed.Z = (float)colorStyle["TitleBgCollapsed"]["Z"];
                TitleBgCollapsed.W = (float)colorStyle["TitleBgCollapsed"]["W"];
            }
            if (colorStyle["ToolTipBg"] != null)
            {
                TooltipBg.X = (float)colorStyle["ToolTipBg"]["X"];
                TooltipBg.Y = (float)colorStyle["ToolTipBg"]["Y"];
                TooltipBg.Z = (float)colorStyle["ToolTipBg"]["Z"];
                TooltipBg.W = (float)colorStyle["ToolTipBg"]["W"];
            }
            if (colorStyle["WindowBg"] != null)
            {
                WindowBg.X = (float)colorStyle["WindowBg"]["X"];
                WindowBg.Y = (float)colorStyle["WindowBg"]["Y"];
                WindowBg.Z = (float)colorStyle["WindowBg"]["Z"];
                WindowBg.W = (float)colorStyle["WindowBg"]["W"];
            }

        }

        public void ApplyStyle(ref Style style)
        {
            style.WindowPadding = WindowPadding;
            style.FramePadding = FramePadding;
            style.WindowMinSize = WindowMinSize;
            style.Alpha = Alpha;
            style.AntiAliasedLines = AntiAliasedLines;
            style.AntiAliasedShapes = AntiAliasedShapes;
            style.ChildWindowRounding = ChildWindowRounding;
            style.ColumnsMinSpacing = ColumnsMinSpacing;
            style.CurveTessellationTolerance = CurveTessellationTolerance;
            style.DisplaySafeAreaPadding = DisplaySafeAreaPadding;
            style.DisplayWindowPadding = DisplayWindowPadding;
            style.FrameRounding = FrameRounding;
            style.GrabMinSize = GrabMinSize;
            style.GrabRounding = GrabRounding;
            style.IndentSpacing = IndentSpacing;
            style.ItemInnerSpacing = ItemInnerSpacing;
            style.ItemSpacing = ItemSpacing;
            style.ScrollbarRounding = ScrollbarRounding;
            style.ScrollbarSize = ScrollbarSize;
            style.TouchExtraPadding = TouchExtraPadding;
            style.WindowRounding = WindowRounding;
            style.WindowTitleAlign = WindowTitleAlign;

            style.SetColor(ColorTarget.Border, Border );
            style.SetColor(ColorTarget.BorderShadow, BorderShadow);
            style.SetColor(ColorTarget.Button, Button);
            style.SetColor(ColorTarget.ButtonActive, ButtonActive);
            style.SetColor(ColorTarget.ButtonHovered, ButtonHovered);
            style.SetColor(ColorTarget.CheckMark, CheckMark);
            style.SetColor(ColorTarget.ChildWindowBg, ChildWindowBg);
            style.SetColor(ColorTarget.CloseButton, CloseButton);
            style.SetColor(ColorTarget.CloseButtonActive, CloseButtonActive);
            style.SetColor(ColorTarget.CloseButtonHovered, CloseButtonHovered);
            style.SetColor(ColorTarget.Column, Column);
            style.SetColor(ColorTarget.ColumnActive, ColumnActive);
            style.SetColor(ColorTarget.ColumnHovered, ColumnHovered);
            style.SetColor(ColorTarget.ComboBg, ComboBg);
            style.SetColor(ColorTarget.FrameBg, FrameBg);
            style.SetColor(ColorTarget.FrameBgActive, FrameBgActive);
            style.SetColor(ColorTarget.FrameBgHovered, FrameBgHovered);
            style.SetColor(ColorTarget.Header, Header);
            style.SetColor(ColorTarget.HeaderActive, HeaderActive);
            style.SetColor(ColorTarget.HeaderHovered, HeaderHovered);
            style.SetColor(ColorTarget.MenuBarBg, MenuBarBg);
            style.SetColor(ColorTarget.ModalWindowDarkening, ModalWindowDarkening);
            style.SetColor(ColorTarget.PlotHistogram, PlotHistogram);
            style.SetColor(ColorTarget.PlotHistogramHovered, PlotHistogramHovered);
            style.SetColor(ColorTarget.PlotLines, PlotLines);
            style.SetColor(ColorTarget.PlotLinesHovered, PlotLinesHovered);
            style.SetColor(ColorTarget.ResizeGrip, ResizeGrip);
            style.SetColor(ColorTarget.ResizeGripActive, ResizeGripActive);
            style.SetColor(ColorTarget.ResizeGripHovered, ResizeGripHovered);
            style.SetColor(ColorTarget.ScrollbarBg, ScrollbarBg);
            style.SetColor(ColorTarget.ScrollbarGrab, ScrollbarGrab);
            style.SetColor(ColorTarget.ScrollbarGrabActive, ScrollbarGrabActive);
            style.SetColor(ColorTarget.ScrollbarGrabHovered, ScrollbarGrabHovered);
            style.SetColor(ColorTarget.SliderGrab, SliderGrab);
            style.SetColor(ColorTarget.SliderGrabActive, SliderGrabActive);
            style.SetColor(ColorTarget.Text, Text);
            style.SetColor(ColorTarget.TextDisabled, TextDisabled);
            style.SetColor(ColorTarget.TextSelectedBg, TextSelectedBg);
            style.SetColor(ColorTarget.TitleBg, TitleBg);
            style.SetColor(ColorTarget.TitleBgActive, TitleBgActive);
            style.SetColor(ColorTarget.TitleBgCollapsed, TitleBgCollapsed);
            style.SetColor(ColorTarget.WindowBg, WindowBg);
        }
    }
}
