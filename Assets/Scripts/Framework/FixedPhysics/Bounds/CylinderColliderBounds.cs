#if UNITY_EDITOR
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

namespace FixedPhysics.Bounds
{
    [ExecuteInEditMode]
    public class CylinderColliderBounds : MonoBehaviour
    {
        public Vector3 mCenter;
        public float mRadius;
        public float mHeight;
        public Color color = new Color(1, 0, 0.1f);
        private static Material mMaterial;

        public void SyncRenderData(FixedIntVector3 logicPos, FixedInt radius, FixedInt height, FixedIntVector3 offset)
        {
            mCenter = Vector3.zero;
            mRadius = radius.RenderFloat;
            mHeight = height.RenderFloat;
            transform.position = logicPos.ToVector3();
            transform.localScale = new Vector3(mRadius * 2, mHeight / 2, mRadius * 2); // Adjust scale to fit the cylinder dimensions
        }
        
        
        

        void OnRenderObject()
        {
            GL.PushMatrix();
            if (mMaterial == null)
            {
                mMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
                mMaterial.color = color;
            }

            mMaterial.SetPass(0);
            GL.MultMatrix(transform.localToWorldMatrix);

            DrawCylinderSides(mRadius, mHeight, 20);
            DrawCylinderCaps(mRadius, mHeight, 20);

            GL.PopMatrix();
        }

        void DrawCylinderSides(float radius, float height, int segments)
        {
            float thetaSegment = 2 * Mathf.PI / segments;

            for (int i = 0; i <= segments; i++)
            {
                float theta = i * thetaSegment;

                // Calculate points on the circle at the top and bottom of the cylinder
                Vector3 topPoint = new Vector3(Mathf.Cos(theta) * radius, height / 2, Mathf.Sin(theta) * radius) +
                                   mCenter;
                Vector3 bottomPoint = new Vector3(Mathf.Cos(theta) * radius, -height / 2, Mathf.Sin(theta) * radius) +
                                      mCenter;

                GL.Begin(GL.LINES);
                GL.Vertex(topPoint);
                GL.Vertex(bottomPoint);
                GL.End();

                if (i < segments)
                {
                    Vector3 nextTopPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + mCenter;
                    Vector3 nextBottomPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, -height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + mCenter;

                    GL.Begin(GL.LINES);
                    GL.Vertex(topPoint);
                    GL.Vertex(nextTopPoint);
                    GL.Vertex(bottomPoint);
                    GL.Vertex(nextBottomPoint);
                    GL.End();
                }
            }
        }

        void DrawCylinderCaps(float radius, float height, int segments)
        {
            float thetaSegment = 2 * Mathf.PI / segments;

            for (int i = 0; i <= segments; i++)
            {
                float theta = i * thetaSegment;

                // Top cap
                Vector3 topPoint = new Vector3(Mathf.Cos(theta) * radius, height / 2, Mathf.Sin(theta) * radius) +
                                   mCenter;
                if (i < segments)
                {
                    Vector3 nextTopPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + mCenter;
                    GL.Begin(GL.LINES);
                    GL.Vertex(new Vector3(mCenter.x, height / 2 + mCenter.y, mCenter.z));
                    GL.Vertex(topPoint);
                    GL.Vertex(topPoint);
                    GL.Vertex(nextTopPoint);
                    GL.End();
                }

                // Bottom cap
                Vector3 bottomPoint = new Vector3(Mathf.Cos(theta) * radius, -height / 2, Mathf.Sin(theta) * radius) +
                                      mCenter;
                if (i < segments)
                {
                    Vector3 nextBottomPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, -height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + mCenter;
                    GL.Begin(GL.LINES);
                    GL.Vertex(new Vector3(mCenter.x, -height / 2 + mCenter.y, mCenter.z));
                    GL.Vertex(bottomPoint);
                    GL.Vertex(bottomPoint);
                    GL.Vertex(nextBottomPoint);
                    GL.End();
                }
            }
        }

        public void OnRelease()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
#endif