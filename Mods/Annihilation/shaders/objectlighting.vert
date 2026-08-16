#version 140
uniform vec4 ldir;


out vec4 eye;
out vec3 normal;
out vec4 l_dir;


out vec3 v_V;
out vec3 v_P;

void main()
{
    normal = normalize(gl_NormalMatrix * gl_Normal);
	
    eye = vec4(-1.0) * (gl_ModelViewMatrix * gl_Vertex);
	l_dir = gl_ProjectionMatrix * gl_ModelViewMatrix * (ldir); 

	
    gl_TexCoord[0] = gl_MultiTexCoord0;
    gl_Position = ftransform();


	v_P = gl_Position.xyz; // v_P is the world position
	v_V = (gl_ModelViewMatrix * gl_Vertex).xyz;
	
}   