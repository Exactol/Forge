using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

//TODO implement EBO and indexed draw calls
namespace Forge
{
    public class RenderObject : IDisposable
    {
        private bool _initialized;
        private readonly int VAO; //Vertex Array Object
        private readonly int VBO; //Vertex Buffer Object for vertices
        //private readonly int _colorVBO; //Vertex Buffer Object for color
        //private readonly int _normalVBO; //Vertex Buffer Object for normals
        private readonly int VEO; //Vertex Element Object

        private readonly int verticeCount;
        //private readonly int colorCount;
        //private readonly int normalCount; //TODO add to disposables
        //private readonly bool _normalsEnabled = false; //TODO i think this can be removed

        //public RenderObject(Vector4[] vertices, Color4[] colors, Vector3[] normals)
        //{
        //    verticeCount = vertices.Length;
        //    colorCount = colors.Length;
        //    normalCount = normals.Length;

        //    Console.WriteLine("\n-----Creating VBO and VAO with stored normals-----\n");

        //    //Generate buffers, VAO, and VEO
        //    VAO = GL.GenVertexArray();
        //    GL.BindVertexArray(VAO);

        //    VBO = GL.GenBuffer();
        //    _colorVBO = GL.GenBuffer();
        //    _normalVBO = GL.GenBuffer();
        //    VEO = GL.GenBuffer();

        //    //Bind VBO, _colorVBO, _normalVBO and VAO
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

        //    GL.BufferData(
        //        BufferTarget.ArrayBuffer,
        //        16 * verticeCount,
        //        vertices,
        //        BufferUsageHint.StaticDraw
        //        );
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, _colorVBO);

        //    GL.BufferData(
        //        BufferTarget.ArrayBuffer,
        //        16 * colorCount,
        //        colors,
        //        BufferUsageHint.StaticDraw
        //        );
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, _normalVBO);

        //    GL.BufferData(
        //        BufferTarget.ArrayBuffer,
        //        12 * normalCount,
        //        normals,
        //        BufferUsageHint.StaticDraw
        //        );


        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, VEO);

        //    //Tell OpenGL how the buffers are being used
        //    //Vertices
        //    //GL.NamedBufferStorage(
        //    //    VBO,                     //Target buffer
        //    //    16 * verticeCount,              //Size of buffer. vertex size * # vertices
        //    //    vertices,                       //Inputted data
        //    //    BufferStorageFlags.MapWriteBit  //Indicating we are writing to buffer
        //    //    );

        //    ////Colors
        //    //GL.NamedBufferStorage(
        //    //    _colorVBO,
        //    //    16 * colorCount,
        //    //    vertices,
        //    //    BufferStorageFlags.MapWriteBit
        //    //    );

        //    ////Normals
        //    //GL.NamedBufferStorage(
        //    //    _normalVBO,
        //    //    12 * normalCount,
        //    //    normals,
        //    //    BufferStorageFlags.MapWriteBit
        //    //    );

            
            
        //    //Position attributes 
        //    GL.VertexArrayAttribBinding(VAO, 0, 0);
        //    GL.EnableVertexArrayAttrib(VAO, 0);

        //    GL.VertexArrayAttribFormat(
        //        VAO,                            //Target array 
        //        0,                              //Attribute index, from shader location = 0
        //        4,                              //Size of attribute, vec4
        //        VertexAttribType.Float,         //Type of attribute is float
        //        false,                          //Does not need to be normalized
        //        0                               //Relative offset
        //        );

        //    //Color attributes
        //    GL.VertexArrayAttribBinding(VAO, 1, 0);
        //    GL.EnableVertexArrayAttrib(VAO, 1);

        //    GL.VertexArrayAttribFormat(
        //        VAO,                            //Target array
        //        1,                              //Attribute index, from shader location = 1
        //        4,                              //Size of attribute, vec4
        //        VertexAttribType.Float,         //Type of attribute is float
        //        false,                          //Does not need to be normalized
        //        16                              //Relative offset after vec4
        //        );

        //    //Normal vectors TODO probably can be removed in future
        //    GL.VertexArrayAttribBinding(VAO, 2, 0);
        //    GL.EnableVertexArrayAttrib(VAO, 2);

        //    GL.VertexArrayAttribFormat(
        //        VAO,                            //Target array
        //        2,                              //Attribute index, from shader location = 2
        //        3,                              //Size of attribute, vec3
        //        VertexAttribType.Float,         //Type of attribute is float
        //        false,                          //Does not need to be normalized
        //        32                              //Relative offset
        //        );

        //    //Link this all together
        //    //GL.VertexArrayVertexBuffer(VAO, 0, VBO, IntPtr.Zero, Vertex.size);

        //    _normalsEnabled = true;
        //    _initialized = true;
        //}

        public RenderObject(Vertex[] vertices)
        {
            verticeCount = vertices.Length;

            Console.WriteLine("\n-----Creating VBO and VAO-----\n");

            //Generate VBO and VAO
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            //GL.GenBuffers(1, out VEO);

            //Bind VBO and VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VEO);

            //Tell OpenGL how the buffer is being used
            GL.NamedBufferStorage(
                VBO,                            //Target buffer
                Vertex.size * vertices.Length,  //Size of buffer
                vertices,                       //Inputted data
                BufferStorageFlags.MapWriteBit  //Indicating we are writing to buffer
                );

            //Position attributes 
            GL.VertexArrayAttribBinding(VAO, 0, 0);
            GL.EnableVertexArrayAttrib(VAO, 0);

            GL.VertexArrayAttribFormat(
                VAO,                            //Target array 
                0,                              //Attribute index, from shader location = 0
                4,                              //Size of attribute, vec4
                VertexAttribType.Float,         //Type of attribute is float
                false,                          //Does not need to be normalized
                0                               //Relative offset
                );

            //Color attributes
            GL.VertexArrayAttribBinding(VAO, 1, 0);
            GL.EnableVertexArrayAttrib(VAO, 1);
            
            GL.VertexArrayAttribFormat(
                VAO,                            //Target array
                1,                              //Attribute index, from shader location = 1
                4,                              //Size of attribute, vec4
                VertexAttribType.Float,         //Type of attribute is float
                false,                          //Does not need to be normalized
                16                              //Relative offset after vec4
                );

            //Link this all together
            GL.VertexArrayVertexBuffer(VAO, 0, VBO, IntPtr.Zero, Vertex.size);

            ////Normals
            //GL.VertexAttribPointer(
            //    2,                              //Target location
            //    3,                              //Size
            //    VertexAttribPointerType.Float,  //Type of attribute is float
            //    false,                          //Does not need to be normalized
            //    6 * sizeof(float),              //Offset
            //    new IntPtr(3 * sizeof(float))   //Pointer
            //    );
            //GL.EnableVertexAttribArray(2);

            //GL.EnableVertexArrayAttrib(VEO, 2);
            //GL.VertexArrayAttribFormat(
            //    VEO,
            //    2,
            //    4,
            //    VertexAttribType.Float,
            //    false,
            //    32
            //    );

            _initialized = true;
        }

        public void Bind()
        {
            
            GL.BindVertexArray(VAO);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, VEO);
            //GL.VertexPointer(4, VertexPointerType.Float, 0, 0);

            //GL.BufferData(GL.VertexArrayElementBuffer, verticeCount, VBO, BufferUsageHint.StaticDraw);
        }

        public void Render(PrimitiveType rendertype)
        {
            GL.DrawArrays(rendertype, 0, verticeCount);
            //GL.DrawRangeElements(PrimitiveType.Lines, 0, verticeCount, verticeCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(VAO);
                    GL.DeleteBuffer(VBO);
                    _initialized = false;
                }
            }
        }
    }
}
