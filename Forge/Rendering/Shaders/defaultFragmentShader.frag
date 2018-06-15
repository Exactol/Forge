#version 440 core

in vec4 vs_color;
in vec4 Normal;
in vec3 FragPos;

out vec4 frag_color;



float LinearizeDepth(float z)
{
  float n = 10.0; // camera z near
  float f = 10000.0; // camera z far
  return (2.0 * n) / (f + n - z * (f - n));	
}

void main(void) {
	//Ambient
	float ambientStrength = 1.0f;
	vec3 lightColor = vec3(1.0f, 0.42f, 0.18f);
	vec3 ambient = ambientStrength * lightColor;

	vec4 normalized = normalize(Normal);
	vec3 norm = vec3(normalized.x, normalized.y, normalized.z);

	vec3 result = (ambient + norm)* vec3(vs_color.x, vs_color.y, vs_color.z);



	frag_color = vec4(result, 1.0f);
}