using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoTema",menuName = "Tema")]
public class Temas : ScriptableObject
{
    public List<string> Palavras = new List<string>();
}
