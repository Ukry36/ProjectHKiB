using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;
    public TextMeshProUGUI text;

    private Vector3 vector;

    public void ShowText()
    {

    }

    void Update()
    {
        vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);
        text.transform.position = vector;

        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
            Destroy(this.gameObject);
    }
}


