
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;

namespace Triangle
{
    class ShaderProgram
    {
        public int Id { get; set; }

        public ShaderProgram(
            string vertexShaderPath, string fragmentShaderPath)
        {
            string vertexShaderSource = null;
            string fragmentShaderSource = null;

            try
            {
                vertexShaderSource = File.ReadAllText(vertexShaderPath);
                fragmentShaderSource = File.ReadAllText(fragmentShaderPath);
            }
            catch (Exception)
            {
                Debug.Assert(false, "Failed to get shader files");
                return;
            }

            int vShader = CreateShaderId(vertexShaderSource, ShaderType.VertexShader);
            int fShader = CreateShaderId(fragmentShaderSource, ShaderType.FragmentShader);

            Id = GL.CreateProgram();
            GL.AttachShader(Id, vShader);
            GL.AttachShader(Id, fShader);
            GL.LinkProgram(Id);
            GL.UseProgram(Id);

            int ok;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out ok);
            if (ok == 0)
            {
                Id = -1;
                string errorMessage = GL.GetProgramInfoLog(Id);
                Debug.Assert(false, errorMessage);
                return;
            }
        }

        private int CreateShaderId(string shaderSource, ShaderType shaderType)
        {
            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            int ok;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out ok);
            if (ok == 0)
            {
                string errorMessage = GL.GetShaderInfoLog(shader);
                Debug.Assert(false, errorMessage);
                return -1;
            }

            return shader;
        }
    }
}
