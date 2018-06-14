using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Forge
{
	/// <summary>
	/// Handles shader creation, building, and reading from disk
	/// </summary>
	class Shader
	{
		//Create shader from string
		private static int CreateShader(ShaderType type, string shaderCode)
		{
			var shader = GL.CreateShader(type);

			GL.ShaderSource(shader, shaderCode);
			GL.CompileShader(shader);

			Console.WriteLine(GL.GetShaderInfoLog(shader));
			return shader;
		}

		private static string ReadFromFile(string path)
		{
			//Read shader from file and return string
			//StreamReader sr;
			string contents = "";

			try
			{
				contents = File.ReadAllText(path);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + "\n");
			}

			return (contents);
		}

		//Build shaders into a shader program
		public static void CreateShaderProgram(ref int shaderProgram, List<string> shaderSource)
		{
			Console.WriteLine("-----Building Shaders-----\n");

			//List that contains all shaders
			var shaders = new List<int>();

			//Create and compile vertex and fragment shader, then add to list
			foreach (string path in shaderSource)
			{
				//string localPath = new Uri(path).LocalPath;
				string localPath = path;
				//Get file extension
				string extension = Path.GetExtension(localPath);

				string shader;

				//Create different shaders based on file extension
				switch (extension)
				{
					//Create vertex shader
					case ".vert":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.VertexShader, shader));
						break;

					//Create fragment shader
					case ".frag":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.FragmentShader, shader));
						break;

					//Create tessellation control shader
					case ".tesc":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.TessControlShader, shader));
						break;

					//Create tessellation evaluation shader
					case ".tese":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.TessEvaluationShader, shader));
						break;

					//Create geometry control shader
					case ".geom":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.GeometryShader, shader));
						break;

					//Create geometry shader ex control shader
					case ".geomex":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.GeometryShaderExt, shader));
						break;

					//Create computation control shader
					case ".comp":
						shader = ReadFromFile(localPath);
						shaders.Add(CreateShader(ShaderType.ComputeShader, shader));
						break;
				}
			}


			//Create main shader program and attach vertex and fragment shaders
			shaderProgram = GL.CreateProgram();

			//Attach shaders to shader program
			foreach (int shader in shaders)
				GL.AttachShader(shaderProgram, shader);

			//Link program
			GL.LinkProgram(shaderProgram);

			//Write out any shader errors
			string programInfoLog = GL.GetProgramInfoLog(shaderProgram);
			Console.WriteLine("Shader errors: " + programInfoLog);

			foreach (int shader in shaders)
				GL.DetachShader(shaderProgram, shader);
		}
	}
}
