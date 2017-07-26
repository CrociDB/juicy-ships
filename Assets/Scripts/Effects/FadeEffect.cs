using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    public Material m_Material;

    public void ShowAll()
    {
        gameObject.SetActive(true);
        m_Material.SetFloat("_FadeVal", 20f);
    }

    public void HideAll()
    {
        m_Material.SetFloat("_FadeVal", 0f);
    }

    public void FadeIn(float time)
    {
        m_Material.DOFloat(0f, "_FadeVal", time).SetEase(Ease.OutCirc);
    }

    public void FadeOut(float time)
    {
        m_Material.DOFloat(20f, "_FadeVal", time).SetEase(Ease.InQuad);
    }
}
