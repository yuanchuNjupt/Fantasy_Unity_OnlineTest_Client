#if UNITY_EDITOR
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

namespace FixedPhysics.Bounds
{
    [ExecuteInEditMode]
    public class BoxColliderBounds : MonoBehaviour , IColliderBounds
    {
        private static Material _lineMaterial;
        private Vector3 _offset;
        private Vector3 _size;
        
        public void UpdateRenderPosition(FixedIntVector3 logicPos)
        {
            transform.position = logicPos.ToVector3();
        }

        public void UpdateRenderSize(FixedIntVector3 size)
        {
            _size = size.ToVector3();
        }

        public void UpdateRenderOffset(FixedIntVector3 offset)
        {
            _offset = offset.ToVector3();
        }
        
        public void UpdateRenderRotation(FixedIntVector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation.ToVector3());
        }
        
        
       
        void OnRenderObject()
        {
            CreateLineMaterial();
            if (_lineMaterial != null)
            {
                _lineMaterial.SetPass(0);
            }

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            var c = _offset;
            var size = _size;
            float rx = size.x / 2f;
            float ry = size.y / 2f;
            float rz = size.z / 2f;
            Vector3 p0, p1, p2, p3;
            Vector3 p4, p5, p6, p7;
            p0 = c + new Vector3(-rx, -ry, rz);
            p1 = c + new Vector3(rx, -ry, rz);
            p2 = c + new Vector3(rx, -ry, -rz);
            p3 = c + new Vector3(-rx, -ry, -rz);

            p4 = c + new Vector3(-rx, ry, rz);
            p5 = c + new Vector3(rx, ry, rz);
            p6 = c + new Vector3(rx, ry, -rz);
            p7 = c + new Vector3(-rx, ry, -rz);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p1);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p2);
            GL.Vertex(p3);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p3);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p4);
            GL.Vertex(p5);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p5);
            GL.Vertex(p6);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p6);
            GL.Vertex(p7);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p4);
            GL.Vertex(p7);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p4);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p1);
            GL.Vertex(p5);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p2);
            GL.Vertex(p6);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p3);
            GL.Vertex(p7);
            GL.End();
            GL.PopMatrix();
        }

        static void CreateLineMaterial()
        {
            if (!_lineMaterial)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                _lineMaterial = new Material(shader);
                _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                _lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        public void OnRelease()
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}
#endif