#if UNITY_EDITOR
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

namespace FixedPhysics.Bounds
{
    [ExecuteInEditMode]
    public class SphereColliderBounds : MonoBehaviour , IColliderBounds
    {
        private Vector3 _offset;
        private float _radius;
        public Color color = new Color(1, 0, 0.1f);
        private static Material _material;
        
        public void UpdateRenderPosition(FixedIntVector3 logicPos)
        {
            transform.position = logicPos.ToVector3();
        }

        public void UpdateRenderRadius(FixedInt radius)
        {
            _radius = radius.RenderFloat;
        }
        
        public void UpdateRenderOffset(FixedIntVector3 offset)
        {
            _offset = offset.ToVector3();
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
            int segments = 20;
            float thetaSegment = Mathf.PI / segments;
            float phiSegment = 2 * Mathf.PI / segments;

            for (int i = 0; i <= segments; i++)
            {
                float theta = i * thetaSegment;
                for (int j = 0; j <= segments; j++)
                {
                    float phi = j * phiSegment;
                    GL.Begin(GL.LINES);
                    Vector3 start = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi),
                        Mathf.Cos(theta)) * 0.5f + _offset;
                    if (i < segments)
                    {
                        Vector3 end = new Vector3(Mathf.Sin(theta + thetaSegment) * Mathf.Cos(phi),
                                          Mathf.Sin(theta + thetaSegment) * Mathf.Sin(phi),
                                          Mathf.Cos(theta + thetaSegment)) * 0.5f +
                                      _offset;
                        GL.Vertex(start);
                        GL.Vertex(end);
                    }

                    GL.End();
                }
            }

            GL.PopMatrix();
        }

        public void OnRelease()
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}
#endif