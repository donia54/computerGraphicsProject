using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.CodeDom;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;
        uint flageBufferID;
        uint xyzAxesBufferID;


        //3D 
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX = 0,
                     translationY = 0,
                     translationZ = 0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 batCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(1, 1, 1.0f, 0);



            float[] verts = { 
               
           // Right Side 
                //T1 (face)
                0.0f,7.0f,0.0f,
                0.500f,0.500f,0.500f,//gray

                0.0f,2.0f,0.0f,
                0.500f,0.500f,0.500f,

                1.0f,5.0f,0.0f,
                0.500f,0.500f,0.500f,

                //T2 (body)
                1.0f,5.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                0.0f,2.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                1.5f,3.8f,0.0f,
                0.200f, 0.200f, 0.200f,
                //T3 
                1.5f,3.8f,0.0f,
                0.700f, 0.700f, 0.700f,

                0.0f,2.0f,0.0f,
                0.700f, 0.700f, 0.700f,

                0.0f,-6.0f,0.0f,
                0.700f, 0.700f, 0.700f,

                //T4 (wing)
                1.5f,3.8f,0.0f,
                0.500f,0.500f,0.500f,

                0.0f,-6.0f,0.0f,
                0.500f,0.500f,0.500f,

                5.0f,9.9f,0.0f,
                0.500f,0.500f,0.500f,
                //T5
                0.0f,-6.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                5.0f,9.9f,0.0f,
                0.200f, 0.200f, 0.200f,

                4.4f,-0.4f,0.0f,
                0.200f, 0.200f, 0.200f,
                //T6
                5.0f,9.9f,0.0f,
                0.270f, 0.270f, 0.270f,

                4.4f,-0.4f,0.0f,
                0.270f, 0.270f, 0.270f,

                6.7f,1.9f,0.0f,
                0.270f, 0.270f, 0.270f,
                //T7
                5.0f,9.9f,0.0f,
                0.500f,0.500f,0.500f,

                6.7f,1.9f,0.0f,
                0.500f,0.500f,0.500f,

                9.9f,8.2f,0.0f,
                0.500f,0.500f,0.500f,

                //T8 (tail)
                0.0f,-6.0f,0.0f,
                0.0f, 0.0f, 0.0f,

                0.9f,-4.5f,0.0f,
                0.0f, 0.0f, 0.0f,

                0.0f,-9.1f,0.0f,
                0.0f, 0.0f, 0.0f,

                //T9 (ear)
                1.0f,5.0f,0.0f,
                0.0f, 0.0f, 0.0f,

                0.61f,5.8f,0.0f,
                0.0f, 0.0f, 0.0f,

                1.2f,7.6f,0.0f,
                0.0f, 0.0f, 0.0f,

            //******************************************//
            //left side
                //T1 (face)
                0.0f,7.0f,0.0f,
                0.500f,0.500f,0.500f,//gray

                0.0f,2.0f,0.0f,
                0.500f,0.500f,0.500f,

                -1.0f,5.0f,0.0f,
                0.500f,0.500f,0.500f,

                //T2 (body)
                -1.0f,5.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                0.0f,2.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                -1.5f,3.8f,0.0f,
                0.200f, 0.200f, 0.200f,
                //T3 
                -1.5f,3.8f,0.0f,
                0.700f, 0.700f, 0.700f,

                0.0f,2.0f,0.0f,
                0.700f, 0.700f, 0.700f,

                0.0f,-6.0f,0.0f,
                0.700f, 0.700f, 0.700f,

                //T4 (wing)
                -1.5f,3.8f,0.0f,
                0.500f,0.500f,0.500f,

                0.0f,-6.0f,0.0f,
                0.500f,0.500f,0.500f,

                -5.0f,9.9f,0.0f,
                0.500f,0.500f,0.500f,
                //T5
                0.0f,-6.0f,0.0f,
                0.200f, 0.200f, 0.200f,

                -5.0f,9.9f,0.0f,
                0.200f, 0.200f, 0.200f,

                -4.4f,-0.4f,0.0f,
                0.200f, 0.200f, 0.200f,
                //T6
                -5.0f,9.9f,0.0f,
                0.270f, 0.270f, 0.270f,

                -4.4f,-0.4f,0.0f,
                0.270f, 0.270f, 0.270f,

                -6.7f,1.9f,0.0f,
                0.270f, 0.270f, 0.270f,
                //T7
                -5.0f,9.9f,0.0f,
                0.500f,0.500f,0.500f,

                -6.7f,1.9f,0.0f,
                0.500f,0.500f,0.500f,

                -9.9f,8.2f,0.0f,
                0.500f,0.500f,0.500f,

                //T8 (tail)
                0.0f,-6.0f,0.0f,
                0.0f, 0.0f, 0.0f,

                -0.9f,-4.5f,0.0f,
                0.0f, 0.0f, 0.0f,

                0.0f,-9.1f,0.0f,
                0.0f, 0.0f, 0.0f,

                //T9 (ear)
                -1.0f,5.0f,0.0f,
                0.0f, 0.0f, 0.0f,

                -0.61f,5.8f,0.0f,
                0.0f, 0.0f, 0.0f,

                -1.2f,7.6f,0.0f,
                0.0f, 0.0f, 0.0f,
            };
            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f,
                0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, -100.0f,
                0.0f, 0.0f, 1.0f,
            };
            float[] flageVertices =
            {
                //T
                -90.0f,-40.0f,0.0f,
                0.5f,0.0f,0.0f,//red

                -90.0f,-80.0f,0.0f,
                0.5f,0.0f,0.0f,

                -70.0f,-60.0f,0.0f,
                0.5f,0.0f,0.0f,
                //line strip (Black)
                -90.0f,-40.0f,0.0f,
                0.0f,0.0f,0.0f,

                -80.0f,-50.0f,0.0f,
                0.0f,0.0f,0.0f,

                -40.0f,-40.0f,0.0f,
                0.0f,0.0f,0.0f,

                -40.0f,-50.0f,0.0f,
                0.0f,0.0f,0.0f,

            

                //line strip (Green)
                -90.0f,-80.0f,0.0f,
                0.0f,0.5f,0.0f,

                -40.0f,-80.0f,0.0f,
                0.0f,0.5f,0.0f,

                -40.0f,-70.0f,0.0f,
                0.0f,0.5f,0.0f,

                -80.0f,-70.0f,0.0f,
                0.0f,0.5f,0.0f,
            };

            batCenter = new vec3(-3, 2, 0);


            vertexBufferID = GPU.GenerateBuffer(verts);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);
            flageBufferID = GPU.GenerateBuffer(flageVertices);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0));  // Head is up (set to 0,-1,0 to look upside-down)
            // Model matrix: apply transformations to the model
            ModelMatrix = new mat4(1);

            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();

        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            sh.UseShader();

            // XYZ axis
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            //Flage
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, flageBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());


            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 3, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 7, 4);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            //BAT
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 54);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }
        public void Update()
        {
            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds / 1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * batCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 1, 0)));
            transformations.Add(glm.translate(new mat4(1), batCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix = MathHelper.MultiplyMatrices(transformations);

            timer.Reset();
            timer.Start();
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}