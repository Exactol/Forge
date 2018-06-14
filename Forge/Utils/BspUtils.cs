using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using System.Diagnostics;

namespace Forge //Todo make non static
{
	class BSP
	{
		public static string ident;
		public static int version;
		public static BinaryReader bspReader;
		public static Dictionary<string, GameLumpDirectory> gameLumpDict = new Dictionary<string, GameLumpDirectory> { };

		public static List<Vector3> vertexList = new List<Vector3>();
		public static List<Edge> edgeList = new List<Edge>();
		public static List<int> surfList = new List<int>();
		public static List<PlaneLump> planeList = new List<PlaneLump>();
		public static List<FaceLump> faceList = new List<FaceLump>();


		//Create empty header lump structs with their corresponding array position
		public static HeaderLump ENT_DATA = new HeaderLump(0);
		public static HeaderLump PLANE_DATA = new HeaderLump(1);
		public static HeaderLump TEX_DATA = new HeaderLump(2);
		public static HeaderLump VERTEXES = new HeaderLump(3);
		public static HeaderLump VISIBILITY = new HeaderLump(4);
		public static HeaderLump NODES = new HeaderLump(5);
		public static HeaderLump TEX_INFO = new HeaderLump(6);
		public static HeaderLump FACES = new HeaderLump(7);
		public static HeaderLump LIGHTING = new HeaderLump(8);
		public static HeaderLump OCCLUSION = new HeaderLump(9);
		public static HeaderLump LEAFS = new HeaderLump(10);
		public static HeaderLump FACE_IDS = new HeaderLump(11);
		public static HeaderLump EDGES = new HeaderLump(12);
		public static HeaderLump SURF_EDGES = new HeaderLump(13);
		public static HeaderLump MODELS = new HeaderLump(14);
		public static HeaderLump WORLD_LIGHTS = new HeaderLump(15);
		public static HeaderLump LEAF_FACES = new HeaderLump(16);
		public static HeaderLump LEAF_BRUSHES = new HeaderLump(17);
		public static HeaderLump BRUSHES = new HeaderLump(18);
		public static HeaderLump BRUSH_SIDES = new HeaderLump(19);
		public static HeaderLump AREAS = new HeaderLump(20);
		public static HeaderLump AREA_PORTALS = new HeaderLump(21);
		public static HeaderLump PORTALS = new HeaderLump(22);
		public static HeaderLump CLUSTERS = new HeaderLump(23);
		public static HeaderLump PORTAL_VERTS = new HeaderLump(24);
		public static HeaderLump CLUSTER_PORTALS = new HeaderLump(25);
		public static HeaderLump DISP_INFO = new HeaderLump(26);
		public static HeaderLump ORIGINAL_FACES = new HeaderLump(27);
		public static HeaderLump PHYS_COLLIDE = new HeaderLump(29);
		public static HeaderLump VERT_NORMALS = new HeaderLump(30);
		public static HeaderLump VERT_NORMALS_INDICIES = new HeaderLump(31);
		public static HeaderLump DISP_LIGHTMAP_ALPHAS = new HeaderLump(32);
		public static HeaderLump DISP_VERTS = new HeaderLump(33);
		public static HeaderLump DISP_LIGHTMAP_SAMPLE_POS = new HeaderLump(34);
		public static HeaderLump GAME_LUMP = new HeaderLump(35);
		public static HeaderLump PAKFILE = new HeaderLump(40);
		public static HeaderLump CUBEMAPS = new HeaderLump(42);
		public static HeaderLump TEX_DATA_STRING_DATA = new HeaderLump(43);
		public static HeaderLump TEX_DATA_STRING_TABLE = new HeaderLump(44);
		public static HeaderLump OVERLAYS = new HeaderLump(45);
		public static HeaderLump DISP_TRIS = new HeaderLump(48);
		public static HeaderLump PHYS_COLLIDE_SURFACE = new HeaderLump(49);

		private static void ReadHeaderLumpArray()
		{
			//loop 64 times for all 64 lumps
			for (int i = 0; i < 64; i++)
			{
				//Fill the structs with their info
				switch (i)
				{
					case 0:
						ENT_DATA.FillLump(bspReader.ReadBytes(16));
						break;
					case 1:
						PLANE_DATA.FillLump(bspReader.ReadBytes(16));
						break;
					case 2:
						TEX_DATA.FillLump(bspReader.ReadBytes(16));
						break;
					case 3:
						VERTEXES.FillLump(bspReader.ReadBytes(16));
						break;
					case 4:
						VISIBILITY.FillLump(bspReader.ReadBytes(16));
						break;
					case 5:
						NODES.FillLump(bspReader.ReadBytes(16));
						break;
					case 6:
						TEX_INFO.FillLump(bspReader.ReadBytes(16));
						break;
					case 7:
						FACES.FillLump(bspReader.ReadBytes(16));
						break;
					case 8:
						LIGHTING.FillLump(bspReader.ReadBytes(16));
						break;
					case 9:
						OCCLUSION.FillLump(bspReader.ReadBytes(16));
						break;
					case 10:
						LEAFS.FillLump(bspReader.ReadBytes(16));
						break;
					case 11:
						FACE_IDS.FillLump(bspReader.ReadBytes(16));
						break;
					case 12:
						EDGES.FillLump(bspReader.ReadBytes(16));
						break;
					case 13:
						SURF_EDGES.FillLump(bspReader.ReadBytes(16));
						break;
					case 14:
						MODELS.FillLump(bspReader.ReadBytes(16));
						break;
					case 15:
						WORLD_LIGHTS.FillLump(bspReader.ReadBytes(16));
						break;
					case 16:
						LEAF_FACES.FillLump(bspReader.ReadBytes(16));
						break;
					case 17:
						LEAF_BRUSHES.FillLump(bspReader.ReadBytes(16));
						break;
					case 18:
						BRUSHES.FillLump(bspReader.ReadBytes(16));
						break;
					case 19:
						BRUSH_SIDES.FillLump(bspReader.ReadBytes(16));
						break;
					case 20:
						AREAS.FillLump(bspReader.ReadBytes(16));
						break;
					case 21:
						AREA_PORTALS.FillLump(bspReader.ReadBytes(16));
						break;
					case 22:
						PORTALS.FillLump(bspReader.ReadBytes(16));
						break;
					case 23:
						CLUSTERS.FillLump(bspReader.ReadBytes(16));
						break;
					case 24:
						PORTAL_VERTS.FillLump(bspReader.ReadBytes(16));
						break;
					case 25:
						CLUSTERS.FillLump(bspReader.ReadBytes(16));
						break;
					case 26:
						DISP_INFO.FillLump(bspReader.ReadBytes(16));
						break;
					case 27:
						ORIGINAL_FACES.FillLump(bspReader.ReadBytes(16));
						break;
					case 29:
						PHYS_COLLIDE.FillLump(bspReader.ReadBytes(16));
						break;
					case 30:
						VERT_NORMALS.FillLump(bspReader.ReadBytes(16));
						break;
					case 31:
						VERT_NORMALS_INDICIES.FillLump(bspReader.ReadBytes(16));
						break;
					case 32:
						DISP_LIGHTMAP_ALPHAS.FillLump(bspReader.ReadBytes(16));
						break;
					case 33:
						DISP_VERTS.FillLump(bspReader.ReadBytes(16));
						break;
					case 34:
						DISP_LIGHTMAP_SAMPLE_POS.FillLump(bspReader.ReadBytes(16));
						break;
					case 35:
						GAME_LUMP.FillLump(bspReader.ReadBytes(16));
						break;
					case 40:
						PAKFILE.FillLump(bspReader.ReadBytes(16));
						break;
					case 42:
						CUBEMAPS.FillLump(bspReader.ReadBytes(16));
						break;
					case 43:
						TEX_DATA_STRING_DATA.FillLump(bspReader.ReadBytes(16));
						break;
					case 44:
						TEX_DATA_STRING_TABLE.FillLump(bspReader.ReadBytes(16));
						break;
					case 45:
						OVERLAYS.FillLump(bspReader.ReadBytes(16));
						break;
					case 48:
						DISP_TRIS.FillLump(bspReader.ReadBytes(16));
						break;
					case 49:
						PHYS_COLLIDE_SURFACE.FillLump(bspReader.ReadBytes(16));
						break;
					default:
						//Skip over 16bit lump 
						bspReader.BaseStream.Seek(16, SeekOrigin.Current);
						break;
				}
			}
		}
		
		private static void ReadEdgeLump()
		{
			Console.WriteLine("\n\n-----EDGES-----");
			Console.WriteLine("Lump offset: {0}", EDGES.fileOffset);
			Console.WriteLine("Lump length: {0}", EDGES.fileLength);
			Console.WriteLine("Lump version: {0}", EDGES.version);

			Edge temp = new Edge();

			for (int i = 0; i < EDGES.fileLength;)
			{
				temp.vert1 = vertexList[BitConverter.ToUInt16(EDGES.data, i)]; //Add vert1 from position specified in edge lump

				temp.vert2 = vertexList[BitConverter.ToUInt16(EDGES.data, i + 2)];

				edgeList.Add(temp);
				i += 4;
			}
			//Console.WriteLine(EDGES.fileLength / 4);
			//foreach (var item in edgeList)
			//{
			//    Console.WriteLine(item.vert1);
			//    Console.WriteLine(item.vert2 + "\n");
			//}
		}

		private static void ReadSurfEdgeLump()
		{
			Console.WriteLine("\n\n-----SURF EDGE-----");
			Console.WriteLine("Lump offset: {0}", SURF_EDGES.fileOffset);
			Console.WriteLine("Lump length: {0}", SURF_EDGES.fileLength);
			Console.WriteLine("Lump version: {0}", SURF_EDGES.version);

			for (int i = 0; i < SURF_EDGES.fileLength;)
			{
				surfList.Add(BitConverter.ToInt32(SURF_EDGES.data, i));
				i += 4;
			}

			//foreach (var item in surfList)
			//    Console.WriteLine(item);
		}

		private static void ReadVertexLump()
		{
			Console.WriteLine("\n\n-----VERTEX-----");
			Console.WriteLine("Lump offset: {0}", VERTEXES.fileOffset);
			Console.WriteLine("Lump length: {0}", VERTEXES.fileLength);
			Console.WriteLine("Lump version: {0}", VERTEXES.version);

			Vector3 tempVertex;

			float x, y, z;

			//Add each 12 byte vertex to list
			for (int i = 0; i < VERTEXES.fileLength;)
			{
				x = BitConverter.ToSingle(VERTEXES.data, i);
				z = BitConverter.ToSingle(VERTEXES.data, i + 4);
				y = BitConverter.ToSingle(VERTEXES.data, i + 8);

				tempVertex = new Vector3(x, y, z);

				vertexList.Add(tempVertex);

				i += 12;
			}

			//foreach (var item in vertexList)
			//{
			//    Console.WriteLine(item);
			//}
		}

		private static void ReadPlaneLump()
		{
			Console.WriteLine("\n\n-----PLANES-----");
			Console.WriteLine("Lump offset: {0}", PLANE_DATA.fileOffset);
			Console.WriteLine("Lump length: {0}", PLANE_DATA.fileLength);
			Console.WriteLine("Lump version: {0}", PLANE_DATA.version);

			byte[] tempByte = new byte[20]; //Temporary byte array to hold each plane lump which is 20 bytes

			PlaneLump tempLump = new PlaneLump();

			//Add each plane to the list
			for (int i = 0; i < PLANE_DATA.fileLength;)
			{
				Array.Copy(PLANE_DATA.data, i, tempByte, 0, 20); //copy 20 bytes to temp array

				tempLump.FillLump(tempByte);

				planeList.Add(tempLump);
				//Console.WriteLine(tempLump.dist);
				//Console.WriteLine(tempLump.normal);
				i += 20;
			}

			//foreach (var item in planeList)
			//{
			//    Console.WriteLine(item.normal);
			//}
		}

		private static void ReadFaceLump()
		{
			Console.WriteLine("\n\n-----FACES-----");
			Console.WriteLine("Lump offset: {0}", FACES.fileOffset);
			Console.WriteLine("Lump length: {0}", FACES.fileLength);
			Console.WriteLine("Lump version: {0}\n", FACES.version);

			FaceLump tempFaceLump = new FaceLump();
			byte[] tempByte = new byte[56];

			for (int i = 0; i < FACES.fileLength;)
			{
				Array.Copy(FACES.data, i, tempByte, 0, 56); //Copy 56 bytes to tempbytearray

				tempFaceLump.FillLump(tempByte);

				faceList.Add(tempFaceLump);
				//Console.WriteLine(tempFaceLump.plane.dist);
				//Console.WriteLine(tempFaceLump.plane.normal);
				i += 56;
			}

		}


		private static void ReadHeader()
		{
			Console.WriteLine("-----HEADER-----");

			//Get 4 byte Ident code   
			for (int i = 0; i < 4; i++)
			{
				ident += bspReader.ReadChar();
			}
			Console.WriteLine("Ident code: {0}", ident);

			//Get version num
			version = bspReader.ReadInt32();
			Console.WriteLine("Version: {0}", version);

			//Get header lump array
			ReadHeaderLumpArray();

		}
		//TODO find a way to make encoded ent data public
		private static void ReadEntData()
		{
			Console.WriteLine("\n\n-----ENTITY DATA-----");
			Console.WriteLine("Lump offset: {0}", ENT_DATA.fileOffset);
			Console.WriteLine("Lump length: {0}", ENT_DATA.fileLength);
			Console.WriteLine("Lump version: {0}\n", ENT_DATA.version);

			//Decode the ent data stored in the ascii text buffer
			string entDataEncoded = System.Text.Encoding.UTF8.GetString(ENT_DATA.data, 0, ENT_DATA.data.Length);
			//Console.WriteLine("{0}", entDataEncoded);

		}


		private static void ReadGameData()
		{
			Console.WriteLine("\n\n-----GAME DATA-----");
			Console.WriteLine("Lump offset: {0}", GAME_LUMP.fileOffset);
			Console.WriteLine("Lump length: {0}", GAME_LUMP.fileLength);
			Console.WriteLine("Lump version: {0}", GAME_LUMP.version);

			//Get number of game lumps
			int lumpCount = BitConverter.ToInt32(GAME_LUMP.data, 0);
			Console.WriteLine("Lump count: {0}", lumpCount);

			//Fill 16 bit game lump directory structs
			byte[] tempArray = new byte[16];
			GameLumpDirectory tempDir = new GameLumpDirectory();

			for (int i = 0; i < lumpCount; i++)
			{
				//Fill temporary array with data. initial offset = 4 
				Array.Copy(GAME_LUMP.data, 4 + i * 16, tempArray, 0, 16);

				//Fill the lump data using temp array and add to the dictionary
				tempDir.FillLump(tempArray);
				gameLumpDict.Add((tempDir.id.ToString()), tempDir);
			}

			//Console.WriteLine("\n-Game Lumps-");

			//foreach (KeyValuePair<string, GameLumpDirectory> kvp in gameLumpDict)
			//{

			//	Console.WriteLine("\nId = {0}\nFlags = {1}\nVersion = {2}\nOffset = {3}\nLength = {4}", kvp.Value.id, kvp.Value.flags, kvp.Value.version, kvp.Value.fileOffset, kvp.Value.fileLength);
			//}

		}

		public static void Read(string mapName)
		{
			FileStream bsp;
			//Attempt to open bsp
			try
			{
				bsp = new FileStream(mapName, FileMode.Open, FileAccess.Read, FileShare.Read);
				bspReader = new BinaryReader(bsp);

				//Read the header data
				ReadHeader();

				bsp.Close();
				bsp.Dispose();
				bspReader.Close();
				bspReader.Dispose();

				ReadEntData();

				ReadGameData();

				ReadVertexLump();

				ReadEdgeLump();

				ReadSurfEdgeLump();

				ReadPlaneLump();

				ReadFaceLump();

			}
			catch (FileNotFoundException fnf)
			{
				Console.WriteLine("Error file not found: " + fnf.Message);
			}
			//Console.WriteLine(bsp);
			//Console.ReadLine();
		}
	}
}
