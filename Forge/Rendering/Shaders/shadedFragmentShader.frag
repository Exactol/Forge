#version 440 core

in vec4 vs_color;
//in vec4 normal;

out vec4 frag_color;

void main(void) {
   frag_color = vs_color;
}