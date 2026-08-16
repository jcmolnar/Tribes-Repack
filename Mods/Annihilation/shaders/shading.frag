#version 140

uniform sampler2D tex0;
uniform sampler2D tex1;
uniform sampler2D tex2;
uniform float cutoff;

void main()
{            

    vec4 textured = vec4(texture2D(tex0,gl_TexCoord[0].st).rgb, 1.0);
	vec4 specular = vec4(texture2D(tex2,gl_TexCoord[0].st).rgb, 1.0);
	vec4 diffuse = vec4(texture2D(tex1,gl_TexCoord[0].st).rgb, 1.0);
    gl_FragColor = ((diffuse/1.5)+0.4) * textured + specular/3.0;
}