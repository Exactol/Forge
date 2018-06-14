using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace Forge
{
	class Camera //TODO make inheritable class and seperate classes that inherit cam for ortho and persp cams
	{
		public Vector3 Position;

		private float horizontalAngle = 0;
		private float verticalAngle = 0;

		public float horizontalFov = 45.0f;
		public float AspectRatio;
		private float nearClip = 0.1f;
		private float farClip = 60000000000f;

		public float originalSpeed = 50f;
		public float speed = 50f;
		public float mouseSensitivity = 5f;

		public Vector3 front;
		public Vector3 up = new Vector3(0,1,0);

		public Matrix4 projectionMatrix;
		public Matrix4 viewMatrix;
		public Matrix4 modelMatrix = Matrix4.CreateScale(5f); //Scale up world, otherwise it feels small

		public void CalcFront()
		{
			//Calculate the front direction vector
			front.X = (float)Math.Cos(MathHelper.DegreesToRadians(VerticalAngle)) * (float)Math.Cos(MathHelper.DegreesToRadians(HorizontalAngle));
			front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(VerticalAngle));
			front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(VerticalAngle)) * (float)Math.Sin(MathHelper.DegreesToRadians(HorizontalAngle));
			front.Normalize();
		}

		public void CalcProjectionMatrix()
		{
			projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(horizontalFov * (float) (Math.PI / 180f),
				AspectRatio, nearClip, farClip);
		}

		public double HorizontalAngle
		{
			get { return horizontalAngle; }
			set { horizontalAngle = (float)Utils.WrapAngle(value); }
		}

		public double VerticalAngle
		{
			get { return verticalAngle; }
			set { verticalAngle = (float)Utils.LimitAngle(value, -89, 89); }
		}
	}
}