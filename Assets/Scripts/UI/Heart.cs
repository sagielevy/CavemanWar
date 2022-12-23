using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();    
    }

    public void LoseHeart()
    {
        image.enabled = false;
    }

    public void ResetHeart()
    {
        image.enabled = true;
    }
}
