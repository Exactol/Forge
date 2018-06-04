#version 440 core

layout (location = 0) in vec4 position;
layout (location = 1) in vec4 colorIn;
layout (location = 2) in vec3 normalIn;

//layout (location = 20) uniform mat4 projection;
//layout (location = 21) uniform mat4 view;
//layout (location = 22) uniform mat4 model;

out Vertex
{ 
	vec4 normal;
	vec4 color;
} vertex;


void main(void) {
   gl_Position = position;

   //mvp = projection * view * model;
   vertex.normal = vec4(normalIn, 0);
   vertex.color = colorIn;
}