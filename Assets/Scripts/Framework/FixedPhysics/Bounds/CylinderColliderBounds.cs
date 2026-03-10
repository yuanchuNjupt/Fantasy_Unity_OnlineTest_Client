#if UNITY_EDITOR
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

namespace FixedPhysics.Bounds
{
    [ExecuteInEditMode]
    public class CylinderColliderBounds : MonoBehaviour , IColliderBounds
    {
        public Vector3 offset;
        public float radius;
        public float height;
        public Color color = new Color(1, 0, 0.1f);
        private static Material _material;

        public void SyncRenderData(FixedIntVector3 logicPos, FixedInt radius, FixedInt height, FixedIntVector3 offset)
        {
            this.offset = Vector3.zero;
            this.radius = radius.RenderFloat;
            this.height = height.RenderFloat;
            transform.position = logicPos.ToVector3();
            transform.localScale = Vector3.one; // 不用 localScale 缩放，直接用 radius/height 绘制
        }
        
        public void UpdateRenderOffset(FixedIntVector3 newOffset)
        {
            this.offset = newOffset.ToVector3();
        }
        
        public void UpdateRenderPosition(FixedIntVector3 logicPos)
        {
            transform.position = logicPos.ToVector3();
        }

        public void UpdateRenderRadius(FixedInt newRadius)
        {
            this.radius = newRadius.RenderFloat;
        }

        public void UpdateRenderHeight(FixedInt newHeight)
        {
            this.height = newHeight.RenderFloat;
        }
        
        
        
        
        
        

        void OnRenderObject()
        {
            GL.PushMatrix();
            if (_material == null)
            {
                _material = new Material(Shader.Find("Hidden/Internal-Colored"));
                _material.color = color;
            }

            _material.SetPass(0);
            GL.MultMatrix(transform.localToWorldMatrix);

            DrawCylinderSides(radius, height, 20);
            DrawCylinderCaps(radius, height, 20);

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
                                   offset;
                Vector3 bottomPoint = new Vector3(Mathf.Cos(theta) * radius, -height / 2, Mathf.Sin(theta) * radius) +
                                      offset;

                GL.Begin(GL.LINES);
                GL.Vertex(topPoint);
                GL.Vertex(bottomPoint);
                GL.End();

                if (i < segments)
                {
                    Vector3 nextTopPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + offset;
                    Vector3 nextBottomPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, -height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + offset;

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
                                   offset;
                if (i < segments)
                {
                    Vector3 nextTopPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + offset;
                    GL.Begin(GL.LINES);
                    GL.Vertex(new Vector3(offset.x, height / 2 + offset.y, offset.z));
                    GL.Vertex(topPoint);
                    GL.Vertex(topPoint);
                    GL.Vertex(nextTopPoint);
                    GL.End();
                }

                // Bottom cap
                Vector3 bottomPoint = new Vector3(Mathf.Cos(theta) * radius, -height / 2, Mathf.Sin(theta) * radius) +
                                      offset;
                if (i < segments)
                {
                    Vector3 nextBottomPoint = new Vector3(Mathf.Cos(theta + thetaSegment) * radius, -height / 2,
                        Mathf.Sin(theta + thetaSegment) * radius) + offset;
                    GL.Begin(GL.LINES);
                    GL.Vertex(new Vector3(offset.x, -height / 2 + offset.y, offset.z));
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