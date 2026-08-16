#version 140

uniform sampler2D tex0;

uniform float cutoff;

uniform int useNormalsF;
uniform vec4 ldir;

in vec4 l_dir;
in vec4 eye;
in vec3 normal;

in vec3 v_V;
in vec3 v_P;
 
void main() {
	vec3 N = normalize(cross(dFdy(v_P), dFdx(v_P))); // N is the world normal

	if(useNormalsF == 1) {
		N = normalize(normal);
	}
	
    float brightness = (texture2D(tex0, gl_TexCoord[0].st).b);

	vec3 V = normalize(v_V);
	vec3 R = reflect(V, N);
    vec3 e = normalize(vec3(eye));
	vec3 L = normalize(vec3(l_dir.xyz));

	vec4 ambient = gl_FrontMaterial.ambient;
	float fog = (gl_FragCoord.z/gl_FragCoord.w) * 50.0;
	vec4 diffuse = vec4(1.0,1.0,1.0,texture2D(tex0,gl_TexCoord[0].st).a) * max(dot(L, N), 0.0);
	    // set the specular term to black
    vec4 spec = vec4(0.0,0.0,0.0, texture2D(tex0, gl_TexCoord[0].st).a);
	vec4 spec2 = vec4(0.0,0.0,0.0, texture2D(tex0, gl_TexCoord[0].st).a);
	
	vec4 specular = vec4(0.7,0.8,0.9,(texture2D(tex0, gl_TexCoord[0].st).a));
	vec4 specular2 = vec4(1.0,0.6,0.3,(texture2D(tex0, gl_TexCoord[0].st).a));
	
	float shininess =  10.0 / max(texture2D(tex0, gl_TexCoord[0].st).g, max(texture2D(tex0, gl_TexCoord[0].st).r, texture2D(tex0, gl_TexCoord[0].st).b));
	float shininess2 = shininess * 1.5;
	vec3 L2 = L * vec3(-1.0,1.0,-1.0);
	
	float intSpec = max(dot(L,N), 0.0);
	float intSpec2 = max(dot(L2,N), 0.0);
	
	spec = specular * pow(intSpec,shininess);
	spec2 = specular2 * pow(intSpec2,shininess2);
	
	if(gl_FragCoord.z/gl_FragCoord.w > 100.0) {
		float f = gl_FragCoord.z/gl_FragCoord.w - 100.0;
		diffuse = diffuse + f/100.0;
		diffuse.r = diffuse.r > 1.0 ? 1.0 : diffuse.r;
		diffuse.g = diffuse.g > 1.0 ? 1.0 : diffuse.g;
		diffuse.b = diffuse.b > 1.0 ? 1.0 : diffuse.b;
		diffuse.b = diffuse.a > 1.0 ? 1.0 : diffuse.a;
		
		spec = spec - f/100.0;
		spec.r = spec.r < 0.0 ? 0.0 : spec.r;
		spec.g = spec.g < 0.0 ? 0.0 : spec.g;
		spec.b = spec.b < 0.0 ? 0.0 : spec.b;
		spec.a = spec.a < 0.0 ? 0.0 : spec.a;

		spec2 = spec2 - f/100.0;
		spec2.r = spec2.r < 0.0 ? 0.0 : spec2.r;
		spec2.g = spec2.g < 0.0 ? 0.0 : spec2.g;
		spec2.b = spec2.b < 0.0 ? 0.0 : spec2.b;
		spec2.a = spec2.a < 0.0 ? 0.0 : spec2.a;
	}
	
	if(useNormalsF == 0) {
		gl_FragData[1] = vec4(1.0,1.0,1.0,1.0);
	} else {
		gl_FragData[1] = diffuse; 
	}
	
	vec4 spec_result = clamp(spec + spec2, 0.0, 1.0);
	gl_FragData[0] = spec_result;
	
}