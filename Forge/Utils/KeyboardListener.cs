using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Forge
{
	internal partial class MainWindow
	{
		private bool zPressed = false;
		private bool leftBracketPressed = false;
		private bool rightBracketPressed = false;
		private bool gPressed = false;

		//Poll keyboard events
		public void PollKeyboard()
		{
			//Hit escape to quit
			//if (Keyboard[Key.Escape])
			//    Exit();
			
			//Keyboard controls
			if (Keyboard.GetState()[Key.W])
				cam.Position += cam.speed * cam.front;

			if (Keyboard.GetState()[Key.S])
				cam.Position -= cam.speed * cam.front;

			if (Keyboard.GetState()[Key.A])
				cam.Position -= Vector3.Normalize(Vector3.Cross(cam.front, cam.up)) * cam.speed;

			if (Keyboard.GetState()[Key.D])
				cam.Position += Vector3.Normalize(Vector3.Cross(cam.front, cam.up)) * cam.speed;

			//Toggle cam control
			if (Keyboard.GetState()[Key.Z])
			{
				//Prevents camera from toggling states if z is being held down
				if (!zPressed)
				{
					CenterMouse();
					camActive = !camActive; // ! Toggles boolean
					CursorVisible = !CursorVisible; //Toggles cursor boolean
				}
				zPressed = true;
			}
			else
				zPressed = false;

			if (Keyboard.GetState()[Key.BracketLeft])
			{
				//Prevents from activating if button is held down
				if (!leftBracketPressed)
				{
					if (gridSpace != 1)
					{
						gridSpace /= 2;
						grid.Dispose();
						grid = new RenderObject(MeshFactory.CreateGrid(gridSpace));
					}
				}
				leftBracketPressed = true;
			}
			else
				leftBracketPressed = false;

			//Increase grid by 2x
			if (Keyboard.GetState()[Key.BracketRight])
			{
				//Prevents from activating if button is held down
				if (!rightBracketPressed)
				{
					if (gridSpace != 512)
					{
						gridSpace *= 2;
						grid = new RenderObject(MeshFactory.CreateGrid(gridSpace));
					}
				}
				rightBracketPressed = true;
			}
			else
				rightBracketPressed = false;

			//Decrease grid by 1/2
			if (Keyboard.GetState()[Key.G])
			{
				//Prevents from activating if held down
				if (!gPressed)
				{
					drawGrid = !drawGrid; //Toggle grid
				}
				gPressed = true;
			}
			else
				gPressed = false;

			//Speed up cam by 10x
			if (Keyboard.GetState()[Key.ShiftLeft])
			{
				cam.speed = cam.originalSpeed * 10;
			}
			else
			{
				cam.speed = cam.originalSpeed;
			}
		}
	}
}
