﻿#version 440 core

in vec4 vs_color;
out vec4 frag_color;

void main(void){
   frag_color = vs_color;
}