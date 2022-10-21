using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class ShutterExample : MonoBehaviour
{

    [SerializeField]
    private GameObject cutObject;

    [SerializeField]
    private Material cutMaterial;

    // Star
    // t is called before the first frame update
    void Start()
    {
        var plane = new EzySlice.Plane(new Vector3(0, 0.4f,0 ), Vector3.right);
        GameObject[] gameObjects =  cutObject.SliceInstantiate(new Vector3(0, 0.4f, 0), Vector3.up, cutMaterial);

        var objectLens = gameObjects.Length;

        Debug.Log(objectLens);
    }
}
