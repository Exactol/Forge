using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ImGuiNET;
using System.IO;
using System.Reflection;

//TODO change writeline to debug

namespace Forge
{
	//TODO investigate random large memory usage
	//TODO multithreading
	class RenderWindow //TODO make disposable
	{
		public void CreateNewWindow()
		{
			//Construct a new game window with min OpenGL version of 3
			using (MainWindow mw = new MainWindow())
			{
				mw.Run(60.0);
			}
		}
	}

	sealed partial class MainWindow : GameWindow
	{
		public MainWindow()
			//Set height, width, title, and default behavior
			:base(1280, 720, GraphicsMode.Default, 
				 "Forge", GameWindowFlags.Default, DisplayDevice.Default, 
				 //OpenGL major version: 4, minor version: 0
				 4, 3, GraphicsContextFlags.Debug)
		{
			//Print out system info
			Console.WriteLine("-----System Info-----\n");
			Console.WriteLine("GL version: " + GL.GetString(StringName.Version));
			Console.WriteLine("Shading language version: " + GL.GetString(StringName.ShadingLanguageVersion));
			Console.WriteLine("Vendor: {0} \n", GL.GetString(StringName.Vendor));
		}

		//initialize camera
		public Camera cam = new Camera();
		public bool camActive = false;
		private const float versionNum = 1.0f;

		private int _defaultShaderProgram;

		private int _shadedShaderProgram;
		//Gets directory of exe and then moves up 2 folders
		private readonly string _baseDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).TrimEnd(Path.DirectorySeparatorChar)).TrimEnd(Path.DirectorySeparatorChar)).TrimEnd(Path.DirectorySeparatorChar).Replace("file:\\", "");
		private readonly List<string> _defaultShader = new List<string>();
		private readonly List<string> _lineShader = new List<string>();
		private readonly List<string> _shadedShader = new List<string>();
		
		private List<RenderObject> renderObjects = new List<RenderObject>();
		public RenderObject grid;

		public int gridSpace = 16;
		public bool drawGrid = true;

		private bool _useShaded = false; //TODO turn into enum 

		private string currentMap = //"cube.bsp";
				@"C:\Program Files (x86)\Steam\steamapps\common\Team Fortress 2\tf\maps\pl_thundermountain.bsp";

		private int _lineShaderProgram;

		protected override void OnLoad(EventArgs e)
		{
			//Setup GUI
			//InitializeGui();

			

			//Add vertices to renderobjects list
			BSP.Read(currentMap);
			
			_defaultShader.Add(_baseDirectory + "\\Rendering\\Shaders\\defaultVertexShader.vert");
			_defaultShader.Add(_baseDirectory + "\\Rendering\\Shaders\\defaultFragmentShader.frag");
			_lineShader.Add(_baseDirectory + "\\Rendering\\Shaders\\lineFragmentShader.frag");
			_lineShader.Add(_baseDirectory + "\\Rendering\\Shaders\\defaultVertexShader.vert");

			//_shadedShader.Add(_baseDirectory + "/Rendering/Shaders/shadedVertexShader.vert");
			//_shadedShader.Add(_baseDirectory + "/Rendering/Shaders/shadedFragmentShader.frag");

			//_shadedShader.Add(_baseDirectory + "\\Rendering\\Shaders\\normalVertexShader.vert");
			//_shadedShader.Add(_baseDirectory + "\\Rendering\\Shaders\\normalFragmentShader.frag");
			//_shadedShader.Add(_baseDirectory + "\\Rendering\\Shaders\\normalGeometryShader.geom");

			List<FaceLump> temp = new List<FaceLump>();
			temp = BSP.faceList;
			var edgeList = BSP.edgeList;
			//Vector4[] vertices = MeshFactory.CreateSolidCube(64f);
			//List<Color4> colors = new List<Color4>();
			//List<Vector3> normal = new List<Vector3>();
			//for (int i = 0; i < vertices.Length ; i++)
			//{
			//    colors.Add(Color4.Green);
			//    normal.Add(new Vector3(1, 0, 0));
			//}


			//renderObjects.Add(new RenderObject(vertices, colors.ToArray(), normal.ToArray()));
			//renderObjects.Add(new RenderObject(MeshFactory.PlaneLumpToVertices(BSP.planeList[0])));
			//renderObjects.Add(new RenderObject(MeshFactory.FaceLumptoVertex(temp)));
			//renderObjects.Add(new RenderObject(MeshFactory.CreatePlane(32, Color4.Blue)));
			renderObjects.AddRange(MeshFactory.FaceLumptoVertex(temp));
			//renderObjects.Add(MeshFactory.FaceLumptoVertex(temp[0]));
			//renderObjects.Add(MeshFactory.FaceLumptoVertex(temp[1]));
			//renderObjects.Add(MeshFactory.FaceLumptoVertex(temp[2]));
			//renderObjects.Add(new RenderObject(MeshFactory.Vector3toVertex(vertexList)));
			//renderObjects.Add(new RenderObject(MeshFactory.EdgeToVertices(edgeList)));
			//renderObjects.Add(new RenderObject(MeshFactory.CreateSolidCube(64f, Color4.DarkSlateBlue)));

			grid = new RenderObject(MeshFactory.CreateGrid(16));

			Shader.CreateShaderProgram(ref _defaultShaderProgram, _defaultShader);
			Shader.CreateShaderProgram(ref _lineShaderProgram, _lineShader);
			//Shader.CreateShaderProgram(ref _shadedShaderProgram, _shadedShader);

			//Setup OpenGL settings
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.Multisample);
			

			//Hide cursor
			CursorVisible = true;

			//Wireframe
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.LineWidth(2);
			GL.PointSize(10);

			//Setup background color
			//GL.ClearColor(Color.SkyBlue);

			//Calculate aspect ratio
			cam.AspectRatio = (float)Width / (float)Height;

			//Calculate initial projection and view matrix
			cam.CalcProjectionMatrix();
		}

		//Game window sizing and 3D projection setup
		protected override void OnResize(EventArgs e)
		{
			//Calculate aspect ratio
			cam.AspectRatio = (float)Width / (float)Height;

			//Resize perspective view
			cam.CalcProjectionMatrix();

			//Resize viewport
			GL.Viewport(0, 0, Width, Height);

		}

		//Updates every frame  
		protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
		{
			//Get keyboard inputs
			PollKeyboard();

			//Get horizontal and vertical angles TODO move to own function?
			if (camActive)
			{
				cam.HorizontalAngle += (Mouse.X - (Width / 2)) * e.Time * cam.mouseSensitivity;
				cam.VerticalAngle += -(Mouse.Y - (Height / 2)) * e.Time * cam.mouseSensitivity;

				CenterMouse();
			}

			//Calculate the front facing vector
			cam.CalcFront();

			cam.viewMatrix = Matrix4.LookAt(cam.Position, cam.Position + Vector3.Normalize(cam.front), cam.up ).Normalized();
		}

		//Move mouse pointer to the center of the screen
		//TODO move to utils class
		public void CenterMouse()
		{
			Point p = new Point(Width / 2, Height / 2);
			System.Windows.Forms.Cursor.Position = PointToScreen(p);
		}

		protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
		{
			Title = $"Forge {versionNum} FPS: {1f / e.Time:0} Map: {currentMap}";

			//Clear screen
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			//TODO move to seperate file
			//Tell OpenGL to use our shaders
			if (_useShaded)
			{
				GL.UseProgram(_shadedShaderProgram);
			}
			else
			{
				GL.UseProgram(_defaultShaderProgram);
			}
			

			//Send projection matrix to shader
			GL.UniformMatrix4(
				20,                    //Match location in shader
				false,                 //don't transpose
				ref cam.projectionMatrix   //use projection matrix
				);

			//Send view matrix to shader
			GL.UniformMatrix4(21, false, ref cam.viewMatrix);

			//Send model matrix to shader
			GL.UniformMatrix4(22, false, ref cam.modelMatrix);

			//Render Grid
			if (drawGrid)
			{
				grid.Bind();
				grid.Render(PrimitiveType.Lines);
			}
			
			//Render each object in object array
			foreach (RenderObject renderObject in renderObjects)
			{
				renderObject.Bind();
				renderObject.Render(PrimitiveType.TriangleFan);
			}

			////Depth color pass
			//foreach (RenderObject renderObject in renderObjects)
			//{
			//	GL.UseProgram(_lineShaderProgram);
			//	renderObject.Bind();
			//	renderObject.Render(PrimitiveType.LineStrip);
			//}

			//Render ImGUI gui
			//RenderGui();
			
			//Display new frame
			SwapBuffers();
		}
		
		//protected override void OnClosing(CancelEventArgs e)
		//{
		//    gui.Dispose();
		//}
	}
}