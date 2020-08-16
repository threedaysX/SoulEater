using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour, IDamageGenerator
{
    public float disappearSpeed;   // 文字消失的速度
    public float disappearTime;    // 文字要花多久時間消失

    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Color normalColor;
    private Color criticalColor;   

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        criticalColor = new Color32(255, 51, 0, 255);
        normalColor = new Color32(186, 236, 255, 255);
    }

    private void Update()
    {
        if (disappearTimer > 0)
        {
            disappearTimer -= Time.deltaTime;
        }
        else
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            if (textColor.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetupDamage(bool isCritical, int damageAmount, Color color)
    {
        textMesh.SetText(damageAmount.ToString());

        if (isCritical)
        {
            textMesh.fontSize = 12;
            textMesh.faceColor = criticalColor;
        }
        else
        {
            textMesh.fontSize = 8;
            textMesh.faceColor = normalColor;
        }

        textColor = textMesh.color;
        disappearTimer = disappearTime;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }
}
