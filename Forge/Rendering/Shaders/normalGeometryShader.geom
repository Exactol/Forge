#version 440 core

layout(triangles) in;

//3 lines will be generated, 6 vertices
layout(line_strip, max_vertices = 6) out;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

//float normal_length = 16.0;

in Vertex 
{
	vec4 normal;
	vec4 color;
} vertex[];


out vec4 vs_color;

void main() {
	int i;
	for (i = 0; i< gl_in.length(); i++) {
		vec3 P = gl_in[i].gl_Position.xyz;
		vec3 N = vertex[i].normal.xyz;

		gl_Position = projection * model * view * vec4(P, 1.0);
		vs_color = vertex[i].color;

		EmitVertex();
		EndPrimitive();
	}

}