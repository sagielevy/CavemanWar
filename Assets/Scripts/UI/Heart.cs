using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();    
    }
}
