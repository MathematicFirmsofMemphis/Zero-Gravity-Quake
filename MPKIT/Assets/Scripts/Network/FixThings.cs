﻿using UnityEngine;
using System.Collections;

public class FixThings : MonoBehaviour
{
    public NetworkView nv;
    public Camera cam;
    public AudioListener listener;
    public GameObject fp;
    public GameObject tp;

    // Use this for initialization
    void Start()
    {
        if (nv.isMine)
        {
            cam.enabled = true;
            listener.enabled = true;
            fp.SetActive(true);
            tp.SetActive(false);
        }
        else
        {
            cam.enabled = false;
            listener.enabled = false;
            fp.SetActive(false);
            tp.SetActive(true);
        }
        this.enabled = false;
    }

    void Update()
    {
        if (nv.isMine)
        {
            cam.enabled = true;
            listener.enabled = true;
            fp.SetActive(true);
            tp.SetActive(false);
        }
        else
        {
            cam.enabled = false;
            listener.enabled = false;
            fp.SetActive(false);
            tp.SetActive(true);
        }
        this.enabled = false;
    }
}
