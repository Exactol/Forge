using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Forge
{
	/// <summary>
	/// TODO
	/// </summary>
	public struct Edge
	{
		public Vector3 vert1; //First vertex
		public Vector3 vert2; //Second vertex

		public Edge Reverse()
		{
			//Create a temporary edge with reversed verts
			Edge tempEdge;

			tempEdge.vert1 = this.vert2;
			tempEdge.vert2 = this.vert1;

			return (tempEdge);
		}
	}

	/// <summary>
	/// 20 Byte lump which holds information about the plane's distance from origin, normal vector, and type
	/// </summary>
	public struct PlaneLump
	{
		public Vector3 normal; //normal Vector
		public float dist; //distance from origin
		public int type; //plane identifier

		public void FillLump(byte[] input)
		{
			normal.X = BitConverter.ToSingle(input, 0);
			normal.Z = BitConverter.ToSingle(input, 4);
			normal.Y = BitConverter.ToSingle(input, 8); //OpenGL uses y as the up vector, while source uses Z
			dist = BitConverter.ToSingle(input, 12);
			type = BitConverter.ToInt32(input, 16);
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public struct FaceLump
	{
		public ushort planeNum; //The plane number
		public byte side; //Faces opposite of node's plane direction
		public byte onNode; //1 if in node, 0 if in leaf
		public int firstEdge; //Index into surfedges
		public short numEdges; //Number of surfedges
		public short texInfo; //Texture info
		public short dispInfo; //Displacement info
		public short surfaceFogVolumeId;
		public byte[] styles; //Switchable lightmap info
		public int lightOffset; //Offset into light lump
		public float area; //Face area in units^2
		public int[] lightmapTexturesMinsInLuxels; //Texture lighting info
		public int[] lightmapTextureSizeInLuxels; //Texture lighting info
		public int originalFace; //Original face this was split from
		public ushort numPrims;
		public ushort firstPrimID;
		public uint smoothingGroups; //Lightmap smoothing groups

		public PlaneLump plane;
		public List<Edge> edgesContained;

		//Fill lump info from byte array
		public void FillLump(byte[] input)
		{
			this.planeNum = BitConverter.ToUInt16(input, 0);
			this.side = input[2];
			this.onNode = input[3];
			this.firstEdge = BitConverter.ToInt32(input, 4);
			this.numEdges = BitConverter.ToInt16(input, 8);
			this.texInfo = BitConverter.ToInt16(input, 10);
			this.dispInfo = BitConverter.ToInt16(input, 12);
			this.surfaceFogVolumeId = BitConverter.ToInt16(input, 14);
			this.styles = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				this.styles[i] = input[16 + i];
			}
			this.lightOffset = BitConverter.ToInt32(input, 18);
			this.area = BitConverter.ToSingle(input, 22);
			this.lightmapTexturesMinsInLuxels = new int[2];
			this.lightmapTexturesMinsInLuxels[0] = BitConverter.ToInt32(input, 28);
			this.lightmapTexturesMinsInLuxels[1] = BitConverter.ToInt32(input, 32);
			this.lightmapTextureSizeInLuxels = new int[2];
			this.lightmapTextureSizeInLuxels[0] = BitConverter.ToInt32(input, 36);
			this.lightmapTextureSizeInLuxels[1] = BitConverter.ToInt32(input, 40);
			this.originalFace = BitConverter.ToInt32(input, 44);
			this.numPrims = BitConverter.ToUInt16(input, 48);
			this.firstPrimID = BitConverter.ToUInt16(input, 50);
			this.smoothingGroups = BitConverter.ToUInt32(input, 52);


			//Copy the portion of the edge list relevant to this face
			edgesContained = new List<Edge>();

			foreach (int index in BSP.surfList.GetRange(firstEdge, numEdges))
			{
				if (index > 0)
				{
					edgesContained.Add(BSP.edgeList[index]);
				} //If index is -, reverse the direction of edges
				else
				{
					edgesContained.Add(BSP.edgeList[Math.Abs(index)].Reverse());
				}
			}

			plane = BSP.planeList[planeNum];
			//foreach (var item in edgesContained)
			//{
			//    Console.WriteLine(item.vert1);
			//    Console.WriteLine(item.vert2+"\n");
			//}

		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public struct GameLumpDirectory
	{
		public int id;
		public ushort flags;
		public ushort version;
		public int fileOffset;
		public int fileLength;

		public void FillLump(byte[] input)
		{
			this.id = BitConverter.ToInt32(input, 0);
			this.flags = BitConverter.ToUInt16(input, 4);
			this.version = BitConverter.ToUInt16(input, 6);
			this.fileOffset = BitConverter.ToInt32(input, 8);
			this.fileLength = BitConverter.ToInt32(input, 12);
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	public struct HeaderLump
	{
		public int lumpArrayNumber;
		public int fileOffset;
		public int fileLength;
		public int version;
		public byte[] fourCC;
		public byte[] data;

		//Overload to manually set lump array number
		public HeaderLump(int arrayNum)
		{
			this.lumpArrayNumber = arrayNum;
			this.fileOffset = 0;
			this.fileLength = 0;
			this.version = 0;
			this.fourCC = new byte[4];
			this.data = new byte[this.fileLength];
		}

		//Fill the lump with info from byte array
		public void FillLump(byte[] input)
		{
			this.fileOffset = BitConverter.ToInt32(input, 0);
			this.fileLength = BitConverter.ToInt32(input, 4);
			this.version = BitConverter.ToInt32(input, 8);
			Array.Copy(input, 12, this.fourCC, 0, 4);
			
			//TODO uncompress bsps
			if (fourCC.Any(x => x != default(int)))
			{
				Console.WriteLine("Compressed lumps are not supported");
				Console.WriteLine("Four CC value: " + string.Join(" ", this.fourCC));
				Console.Read();
				Environment.Exit(2);
			}

			//Store binary reader original position to reset it later
			long oldPos = BSP.bspReader.BaseStream.Position;

			//Move binary reader to the starting position of 
			BSP.bspReader.BaseStream.Position = fileOffset;

			//Fill with data
			this.data = BSP.bspReader.ReadBytes(this.fileLength);

			//Reset to original position
			BSP.bspReader.BaseStream.Position = oldPos;
		}
	}

	//Vertex structure TODO add normals?
	public struct Vertex
	{
		public const int size = (4 + 4 + 4) * 4; //Size of struct in bytes

		private readonly Vector4 position;
		private readonly Color4 color;
		private readonly Vector4 normal;

		public Vertex(Vector4 pos, Color4 col)
		{
			position = pos;
			color = col;
			normal = Vector4.Zero;
		}

		public Vertex(Vector4 pos, Color4 col, Vector3 norm)
		{
			position = pos;
			color = col;
			normal = new Vector4(norm.X, norm.Y, norm.Z, 1.0f);
		}

		public Vertex(Vector3 pos, Color4 col, Vector3 norm)
		{
			position = new Vector4(pos.X, pos.Y, pos.Z, 1.0f);
			color = col;
			normal = new Vector4(norm.X, norm.Y, norm.Z, 1.0f);
		}
		public Vertex(Vector3 pos, Color4 col)
		{
			position = new Vector4(pos.X, pos.Y, pos.Z, 1.0f); //Convert vec3 to vec4
			color = col;
			normal = Vector4.Zero;
		}
	}

	public struct Line
	{
		public const int size = (4 + 4) * 4;

		private readonly Vector2 position;
		private readonly Color4 color;

		public Line(Vector2 pos, Color4 col)
		{
			position = pos;
			color = col;
		}
	}
}
