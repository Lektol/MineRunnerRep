using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public double x;
    public Vector3 target_pos;

    void Start()
    {
        StartCoroutine(Fo());
    }

    double Z(double x)
    {
        return Math.Sqrt(x);
    }

    void Update()
    {
        target_pos = new Vector3((float)x, 5, 2*(float)Z(x));
        transform.localPosition = Vector3.MoveTowards(transform.position, target_pos, 5*Time.deltaTime);
    }

    IEnumerator Fo()
    {
        yield return new WaitForSeconds(0.2f);
        x++;
        if(x <= 20) StartCoroutine(Fo());
    }
    
}
