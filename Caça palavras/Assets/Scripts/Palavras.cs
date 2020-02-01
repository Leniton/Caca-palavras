using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Palavras : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TemaTexto;
    [SerializeField] GameObject TextoBase;
    List<TextMeshProUGUI> Textos = new List<TextMeshProUGUI>();
    [Space]
    Temas tema;
    [SerializeField] List<string> ListaPalavras = new List<string>();
    [SerializeField]List<bool> Achados = new List<bool>();

    void Awake()
    {
        tema = SceneM.manager.GetComponent<Lista_Temas>().PegarTema();
        TemaTexto.text = tema.name;
        ListaPalavras = tema.Palavras;

        if (ListaPalavras.Count > 6)
        {
            int QuantidadeaRemover = ListaPalavras.Count - 6;
            for (int i = 0; i < QuantidadeaRemover; i++)
            {
                ListaPalavras.RemoveAt(Random.Range(0, ListaPalavras.Count));
            }
        }
    }

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

            if (!Achados.Contains(false))
            {
                SceneM.manager.NextScene();
            }
        }
    }
}
