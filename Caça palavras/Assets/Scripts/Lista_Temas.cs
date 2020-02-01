using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lista_Temas : MonoBehaviour
{
    [SerializeField] List<Temas> lista = new List<Temas>();

    public Temas PegarTema()
    {
        int r = Random.Range(0, lista.Count);

        Temas tema = lista[r];

        return tema;
    }
}
