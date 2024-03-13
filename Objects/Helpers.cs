using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Objects
{
    public static class Helpers
    {
        public static Microsoft.Xna.Framework.Vector3 toXNA(this BEPUutilities.Vector3 vec)
        {
            return new Microsoft.Xna.Framework.Vector3(vec.X, vec.Y, vec.Z);
        }
        public static BEPUutilities.Matrix toBEPU(this Microsoft.Xna.Framework.Matrix mat)
        {
            BEPUutilities.Matrix ret = new BEPUutilities.Matrix();

            ret.M11 = mat.M11;
            ret.M12 = mat.M12;
            ret.M13 = mat.M13;
            ret.M14 = mat.M14;
            ret.M21 = mat.M21;
            ret.M22 = mat.M22;
            ret.M23 = mat.M23;
            ret.M24 = mat.M24;
            ret.M31 = mat.M31;
            ret.M32 = mat.M32;
            ret.M33 = mat.M33;
            ret.M34 = mat.M34;
            ret.M41 = mat.M41;
            ret.M42 = mat.M42;
            ret.M43 = mat.M43;
            ret.M44 = mat.M44;

            return ret;
        }

        public static Microsoft.Xna.Framework.Matrix toXNA(this BEPUutilities.Matrix mat)
        {
            Microsoft.Xna.Framework.Matrix ret = new Microsoft.Xna.Framework.Matrix();

            ret.M11 = mat.M11;
            ret.M12 = mat.M12;
            ret.M13 = mat.M13;
            ret.M14 = mat.M14;
            ret.M21 = mat.M21;
            ret.M22 = mat.M22;
            ret.M23 = mat.M23;
            ret.M24 = mat.M24;
            ret.M31 = mat.M31;
            ret.M32 = mat.M32;
            ret.M33 = mat.M33;
            ret.M34 = mat.M34;
            ret.M41 = mat.M41;
            ret.M42 = mat.M42;
            ret.M43 = mat.M43;
            ret.M44 = mat.M44;

            return ret;
        }

        public static Microsoft.Xna.Framework.Matrix[] toXNA(this BEPUutilities.Matrix[] mats)
        {
            List<Microsoft.Xna.Framework.Matrix> ret = new List<Microsoft.Xna.Framework.Matrix>();

            foreach (BEPUutilities.Matrix mat in mats)
            {
                ret.Add(mat.toXNA());
            }

            return ret.ToArray();
        }

        public static void UpdateMinMax(this Microsoft.Xna.Framework.Vector3 vert, ref Microsoft.Xna.Framework.Vector3 currentMax, ref Microsoft.Xna.Framework.Vector3 currentMin)
        {
            if (vert.X > currentMax.X)
            {
                currentMax.X = vert.X;
            }
            else if (vert.X < currentMin.X)
            {
                currentMin.X = vert.X;
            }
            if (vert.Y > currentMax.Y)
            {
                currentMax.Y = vert.Y;
            }
            else if (vert.Y < currentMin.Y)
            {
                currentMin.Y = vert.Y;
            }
            if (vert.Z > currentMax.Z)
            {
                currentMax.Z = vert.Z;
            }
            else if (vert.Z < currentMin.Z)
            {
                currentMin.Z = vert.Z;
            }
        }

        #region From https://gamedev.stackexchange.com/questions/109516/xna-get-model-height-in-location
        public static Microsoft.Xna.Framework.Matrix CreateTransform(ModelBone bone)
        {
            if (bone == null)
                return Microsoft.Xna.Framework.Matrix.Identity;

            return bone.Transform * CreateTransform(bone.Parent);
        }

        public static (List<CollisionTriangle>, Microsoft.Xna.Framework.Vector3, Microsoft.Xna.Framework.Vector3) ExtractMeshPart(ModelMeshPart meshPart, Microsoft.Xna.Framework.Matrix transform)
        {

            VertexDeclaration declaration = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] vertexElements = declaration.GetVertexElements();
            VertexElement vertexPosition = new VertexElement();
            Microsoft.Xna.Framework.Vector3 maxPoint = new Microsoft.Xna.Framework.Vector3();
            Microsoft.Xna.Framework.Vector3 minPoint = new Microsoft.Xna.Framework.Vector3();

            foreach (VertexElement element in vertexElements)
            {
                if (element.VertexElementUsage == VertexElementUsage.Position && element.VertexElementFormat == VertexElementFormat.Vector3)
                    vertexPosition = element;
            }

            Microsoft.Xna.Framework.Vector3[] allVertex = new Microsoft.Xna.Framework.Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData<Microsoft.Xna.Framework.Vector3>(
                            meshPart.VertexOffset * declaration.VertexStride + vertexPosition.Offset,
                            allVertex,
                            0,
                            meshPart.NumVertices,
                            declaration.VertexStride);

            short[] indices = new short[meshPart.PrimitiveCount * 3];
            meshPart.IndexBuffer.GetData<short>(meshPart.StartIndex * 2, indices, 0, meshPart.PrimitiveCount * 3);

            for (int i = 0; i != allVertex.Length; ++i)
            {
                Microsoft.Xna.Framework.Vector3.Transform(ref allVertex[i], ref transform, out allVertex[i]);
                
            }

            List<CollisionTriangle> triangles = new List<CollisionTriangle>();

            for (int i = 0; i < indices.Length; i += 3)
            {
                CollisionTriangle triangle = new CollisionTriangle();
                triangle.v[0] = allVertex[indices[i]];
                triangle.v[1] = allVertex[indices[i + 1]];
                triangle.v[2] = allVertex[indices[i + 2]];

                triangles.Add(triangle);
            }

            return (triangles, maxPoint, minPoint);
        }

        public static bool InsideTriangle(Microsoft.Xna.Framework.Vector3 pos, CollisionTriangle tri)
        {
            Microsoft.Xna.Framework.Vector3 vec1 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[0], pos);
            Microsoft.Xna.Framework.Vector3 vec2 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[1], pos);

            vec1.Normalize();
            vec2.Normalize();

            double dotV = Microsoft.Xna.Framework.Vector3.Dot(vec1, vec2);
            double angle = (float)Math.Acos(dotV);

            vec1 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[1], pos);
            vec2 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[2], pos);

            vec1.Normalize();
            vec2.Normalize();

            dotV = Microsoft.Xna.Framework.Vector3.Dot(vec1, vec2);
            angle = (float)Math.Acos(dotV);

            vec1 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[2], pos);
            vec2 = Microsoft.Xna.Framework.Vector3.Subtract(tri.v[0], pos);

            vec1.Normalize();
            vec2.Normalize();

            dotV = Microsoft.Xna.Framework.Vector3.Dot(vec1, vec2);
            angle = (float)Math.Acos(dotV);

            float tolerance = 0.001f;

            if (angle > (Math.PI * 2) - tolerance)
                return true;

            return false;
        }
        #endregion
    }
}
