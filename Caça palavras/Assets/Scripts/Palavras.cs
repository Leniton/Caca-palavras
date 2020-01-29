using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Palavras : MonoBehaviour
{
    [SerializeField] GameObject TextoBase;
    List<TextMeshProUGUI> Textos = new List<TextMeshProUGUI>();
    [Space]
    [SerializeField] List<string> ListaPalavras = new List<string>();
    [SerializeField]List<bool> Achados = new List<bool>();

    void Start()
    {
        for (int i = 0; i < ListaPalavras.Count; i++)
        {
            if (i == 0)
            {
                Textos.Add(TextoBase.GetComponent<TextMeshProUGUI>());
                Textos[i].text = ListaPalavras[i];
            }
            else
            {
                Vector3 posicao = new Vector3(TextoBase.transform.position.x, TextoBase.transform.position.y - 2 * i, 0);
                Textos.Add(Instantiate(TextoBase, posicao, Quaternion.identity,transform.parent).GetComponent<TextMeshProUGUI>());
                Textos[i].text = ListaPalavras[i];
            }
            Achados.Add(false);
        }
    }

    public List<string> Lista()
    {
        return ListaPalavras;
    }

    public bool ChecarPalavra(string palavra)
    {
        return ListaPalavras.Contains(palavra);
    }

    public void MarcarPalavra(string palavra)
    {
        if (ListaPalavras.Contains(palavra))
        {
            int ID = ListaPalavras.IndexOf(palavra);
            Achados[ID] = true;
            Textos[ID].color = Color.green;
        }
    }
}
