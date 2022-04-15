﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShop : MonoBehaviour
{
    protected void Start()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
