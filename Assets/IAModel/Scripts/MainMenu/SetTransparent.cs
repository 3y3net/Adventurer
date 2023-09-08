using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransparent : MonoBehaviour {

    public List<Material> materials = new List<Material>();
    [Range(0, 1)]
    public float alpha = 1f;

    float lastAlpha;

    private void Start()
    {
        lastAlpha = alpha;
    }

    // Update is called once per frame
    void Update () {
        if (lastAlpha != alpha)
        {
            foreach (Material mat in materials)
            {
                Color col = mat.GetColor("_Color");
                col.a = alpha;
                mat.SetColor("_Color", col);
            }
            lastAlpha = alpha;
        }
	}
}
