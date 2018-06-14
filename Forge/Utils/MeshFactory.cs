using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Forge
{
	class MeshFactory
	{
		public static Vertex[] CreateSolidCube(float side, Color4 color)
		{
			side = side / 2f; //Half side

			Vertex[] vertices =
			{
			   new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
			   new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
			   new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
			   new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
			   new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
			   new Vertex(new Vector4(-side, side, side, 1.0f),     color),

			   new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
			   new Vertex(new Vector4(side, side, -side, 1.0f),     color),
			   new Vertex(new Vector4(side, -side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, -side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, -side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, side, 1.0f),      color),

			   new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
			   new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
			   new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
			   new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
			   new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
			   new Vertex(new Vector4(side, -side, side, 1.0f),     color),

			   new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
			   new Vertex(new Vector4(-side, side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, -side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, -side, 1.0f),     color),
			   new Vertex(new Vector4(-side, side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, side, 1.0f),      color),

			   new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
			   new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
			   new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
			   new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
			   new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
			   new Vertex(new Vector4(side, side, -side, 1.0f),     color),

			   new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
			   new Vertex(new Vector4(side, -side, side, 1.0f),     color),
			   new Vertex(new Vector4(-side, side, side, 1.0f),     color),
			   new Vertex(new Vector4(-side, side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, -side, side, 1.0f),     color),
			   new Vertex(new Vector4(side, side, side, 1.0f),      color)
			};
			return vertices;
		}

		public static Vector4[] CreateSolidCube(float side)
		{
			side = side / 2f; //Half side

			Vector4[] vertices =
			{
			   new Vector4(-side, -side, -side, 1.0f),
			   new Vector4(-side, -side, side, 1.0f),
			   new Vector4(-side, side, -side, 1.0f),
			   new Vector4(-side, side, -side, 1.0f),
			   new Vector4(-side, -side, side, 1.0f),
			   new Vector4(-side, side, side, 1.0f),

			   new Vector4(side, -side, -side, 1.0f),
			   new Vector4(side, side, -side, 1.0f),
			   new Vector4(side, -side, side, 1.0f),
			   new Vector4(side, -side, side, 1.0f),
			   new Vector4(side, side, -side, 1.0f),
			   new Vector4(side, side, side, 1.0f),

			   new Vector4(-side, -side, -side, 1.0f),
			   new Vector4(side, -side, -side, 1.0f),
			   new Vector4(-side, -side, side, 1.0f),
			   new Vector4(-side, -side, side, 1.0f),
			   new Vector4(side, -side, -side, 1.0f),
			   new Vector4(side, -side, side, 1.0f),

			   new Vector4(-side, side, -side, 1.0f),
			   new Vector4(-side, side, side, 1.0f),
			   new Vector4(side, side, -side, 1.0f),
			   new Vector4(side, side, -side, 1.0f),
			   new Vector4(-side, side, side, 1.0f),
			   new Vector4(side, side, side, 1.0f),

			   new Vector4(-side, -side, -side, 1.0f),
			   new Vector4(-side, side, -side, 1.0f),
			   new Vector4(side, -side, -side, 1.0f),
			   new Vector4(side, -side, -side, 1.0f),
			   new Vector4(-side, side, -side, 1.0f),
			   new Vector4(side, side, -side, 1.0f),

			   new Vector4(-side, -side, side, 1.0f),
			   new Vector4(side, -side, side, 1.0f),
			   new Vector4(-side, side, side, 1.0f),
			   new Vector4(-side, side, side, 1.0f),
			   new Vector4(side, -side, side, 1.0f),
			   new Vector4(side, side, side, 1.0f)
			};
			return vertices;
		}

		public static Vertex[] CreatePlane(float side, Color4 color)
		{
			side = side / 2f;

			Vertex[] vertices = 
			{
				new Vertex(new Vector4(side, side, 0, 1.0f), color),
				new Vertex(new Vector4(side, -side, 0, 1.0f), color),
				new Vertex(new Vector4(-side, -side, 0, 1.0f), color),
				new Vertex(new Vector4(-side, side, 0, 1.0f), color)
			};

			return (vertices);
		}

		public static Vertex[] CreateGrid(int gridSize) //Distance between grid spacings
		{
			List<Vertex> verticeList = new List<Vertex>();

			//X Center line
			verticeList.Add(new Vertex(new Vector4(0, 0, 0, 1.0f), Color4.LawnGreen));
			verticeList.Add(new Vertex(new Vector4(gridSize, 0, 0, 1.0f), Color4.LawnGreen));

			//Y Center line
			verticeList.Add(new Vertex(new Vector4(0, 0, 0, 1.0f), Color4.DeepSkyBlue));
			verticeList.Add(new Vertex(new Vector4(0, gridSize, 0, 1.0f), Color4.DeepSkyBlue));

			//Z Center line
			verticeList.Add(new Vertex(new Vector4(0, 0, 0, 1.0f), Color4.Red));
			verticeList.Add(new Vertex(new Vector4(0, 0, gridSize, 1.0f), Color4.Red));


			//Create X grid lines
			for (int i = 0; i < 9; i++)
			{
				verticeList.Add(new Vertex(new Vector4(i*gridSize, 0, -gridSize*8, 1.0f), Color4.DimGray));
				verticeList.Add(new Vertex(new Vector4(i*gridSize, 0, gridSize*8, 1.0f), Color4.DimGray));

				verticeList.Add(new Vertex(new Vector4(-i * gridSize, 0, -gridSize*8, 1.0f), Color4.DimGray));
				verticeList.Add(new Vertex(new Vector4(-i * gridSize, 0, gridSize*8, 1.0f), Color4.DimGray));
			}

			//Create Y grid lines
			for (int i = 0; i < 9; i++)
			{
				verticeList.Add(new Vertex(new Vector4(-gridSize*8, 0, i * gridSize, 1.0f), Color4.DimGray));
				verticeList.Add(new Vertex(new Vector4(gridSize*8, 0, i * gridSize, 1.0f), Color4.DimGray));

				verticeList.Add(new Vertex(new Vector4(-gridSize*8, 0, -i * gridSize, 1.0f), Color4.DimGray));
				verticeList.Add(new Vertex(new Vector4(gridSize*8, 0, -i * gridSize, 1.0f), Color4.DimGray));
			}
			
			return verticeList.ToArray();
		}

		//Convert 2 vec3 edges to vertex
		public static Vertex[] EdgeToVertexes(List<Edge> edgeList)
		{
			List<Vertex> vertList = new List<Vertex>();

			//Console.WriteLine("\n--Starting edge conversion--\n");

			foreach (Edge edge in edgeList)
			{
				vertList.Add(new Vertex(edge.vert1, Color4.CornflowerBlue));
				vertList.Add(new Vertex(edge.vert2, Color4.IndianRed));
			}
			//Console.WriteLine("--Edge conversion complete--\n");

			return (vertList.ToArray());
		}

		//Convert vector3 to vertex
		public static Vertex[] Vector3ToVertex(List<Vector3> vecList)
		{
			List<Vertex> vertList = new List<Vertex>();

			foreach (Vector3 vec in vecList)
			{
				vertList.Add(new Vertex(vec, Color4.Green));
			}
			vertList.ToArray();
			return (vertList.ToArray());
		}

		//Extract edges from face lump struct and convert to vertex array
		public static RenderObject[] FaceLumptoVertex(List<FaceLump> faceList)
		{
			List<RenderObject> objList = new List<RenderObject>();

			//Add all edges to one list
			foreach (FaceLump face in faceList)
			{
				objList.Add(FaceLumptoVertex(face));
			}

			return (objList.ToArray());
		}
		
		//TODO remove, temp method for testing
		public static RenderObject FaceLumptoVertex(FaceLump face)
		{
			return new RenderObject((EdgeToVertexes(face.edgesContained)));
		}

		public static Vertex[] PlaneLumpToVertices(PlaneLump plane)
		{
			List<Vertex> vertList= new List<Vertex>();
			//plane.dist / plane.normal
			//plane.

			return vertList.ToArray();
		}
	}
}
