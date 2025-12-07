using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SphereColliderGizom : MonoBehaviour
{
    private Renderer mRender;
    public Vector3 mConter;
    public float mRadius;
    public Color color = new Color(1, 0, 0, 0.1f);
#if UNITY_EDITOR
    void Start()
    {
      
        mRender = GetComponent<Renderer>();
        mRender.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/FixIntPhysics3D/Gizmo/FixPhysicMaterial.mat");
    }

    public void SetBoxData( float radius, Vector3 conter, bool isFloowTarget = false)
    {
        this.mConter = conter;
        this.mRadius = radius;
        transform.position = conter;
        transform.localScale = Vector3.one  * radius;
    }

    void OnRenderObject()
    {
       
        if (mRender == null)
            mRender = GetComponent<Renderer>();
        mRender.sharedMaterial.color = color;
    }

#endif
}