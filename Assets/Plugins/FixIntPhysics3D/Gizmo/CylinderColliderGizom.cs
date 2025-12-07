using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CylinderColliderGizom : MonoBehaviour
{
    CylinderColliderGizom colliders;
    private Renderer mRender;
    public Vector3 mConter;
    public float mRadius;
    public Color color = new Color(1, 0, 0, 0.1f);
    void Start()
    {
        mRender = GetComponent<Renderer>();
    }

    public void SetBoxData(Vector3 conter, float radius, bool isFloowTarget = false)
    {
        this.mConter = conter;
        this.mRadius = radius;
    }

    void OnRenderObject()
    {
        if (mRender==null)
            mRender = GetComponent<Renderer>();
        mRender.sharedMaterial.color = color;
    }

 
}