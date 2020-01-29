using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Célula : MonoBehaviour
{

    static bool MouseDown = false;
    public Color CorPadrão;

    void Start()
    {
        CorPadrão = transform.GetChild(1).GetComponent<Image>().color;
    }

    void OnMouseEnter()
    {
        if (!MouseDown) return;
        GetComponentInParent<Grid>().SelecaoUltimaCelula(gameObject);
    }

    void OnMouseExit()
    {
        transform.GetChild(1).GetComponent<Image>().color = CorPadrão;
    }

    void OnMouseDown()
    {
        MouseDown = true;
        GetComponentInParent<Grid>().PontoInicial(gameObject);
    }

    void OnMouseUp()
    {
        MouseDown = false;
        GetComponentInParent<Grid>().VerificarPalavra();
    }
}
