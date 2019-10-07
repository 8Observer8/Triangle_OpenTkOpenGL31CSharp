#version 140

in vec2 aPosition;
uniform mat4 uModelMatrix;

void main()
{
	gl_Position = uModelMatrix * vec4(aPosition, 0.0, 1.0);
}
