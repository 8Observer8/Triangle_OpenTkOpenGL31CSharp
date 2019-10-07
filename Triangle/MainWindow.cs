
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Triangle
{
    class MainWindow : GameWindow
    {
        private int _programId;
        private bool _canDraw = false;

        public MainWindow(): base(256, 256, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.515f, 0.415f, 0.921f, 1f);

            Title = "C#, OpenGL 3.1";

            var shaderProgram = new ShaderProgram(
                "Assets/Shaders/VertexShader.glsl",
                "Assets/Shaders/FragmentShader.glsl");
            _programId = shaderProgram.Id;

            if (!InitVertexBuffers()) return;

            Matrix4 modelMatrix =
                Matrix4.CreateScale(0.5f) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(20f)) *
                Matrix4.CreateTranslation(0.5f, 0.5f, 0f);

        int uModelMatrixLocation = GL.GetUniformLocation(_programId, "uModelMatrix");
            if (uModelMatrixLocation == -1)
            {
                Debug.Assert(false, "Failed to get uModelMatrix");
                return;
            }

            GL.UniformMatrix4(uModelMatrixLocation, false, ref modelMatrix);

            _canDraw = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (_canDraw)
            {
                Draw();
            }

            SwapBuffers();
        }

        private void Draw()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        private bool InitVertexBuffers()
        {
            float[] vertices = new float[]
            {
                -0.5f, -0.5f,
                0.5f, -0.5f,
                0f, 0.5f
            };

            int vbo;
            GL.GenBuffers(1, out vbo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            int aPosition = GL.GetAttribLocation(_programId, "aPosition");
            if (aPosition == -1)
            {
                Debug.Assert(false, "Failed to get aPosition");
                return false;
            }

            GL.VertexAttribPointer(aPosition, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(aPosition);

            return true;
        }
    }
}
