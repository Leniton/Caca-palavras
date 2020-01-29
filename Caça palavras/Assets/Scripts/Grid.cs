using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Grid : MonoBehaviour
{
    public int GridSize;
    GameObject Celula;

    GameObject[,] grid;

    //seleção de letras
    int XPrimeira, YPrimeira;
    int XUltima, YUltima;
    GameObject UltimaCelula;

    void Start()
    {
        grid = new GameObject[GridSize,GridSize];
        Celula = transform.GetChild(0).gameObject;
        grid[0, 0] = Celula;
        CriarGrid();
        EscolherPosicaoDasPalavras();
        PreencherEspacos();
    }

    void CriarGrid()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int u = 0; u < GridSize; u++)
            {
                if(!(i == 0 && u == 0))
                {
                    Vector3 posicao = new Vector3(transform.position.x + 1 * i, transform.position.y - 1 * u, 0);
                    GameObject GO = Instantiate(Celula, posicao, Quaternion.identity,transform);

                    GO.name = "Célula[" + i + "," + u + "]";

                    //Escala diferente entre o grid e as letras, precisa do reajuste
                    //Vector3 PosDoTexto = Celula.transform.GetChild(2).position;
                    //GO.transform.GetChild(2).position = new Vector3(PosDoTexto.x + 16.6f * i, PosDoTexto.y - 16.6f * u, 0);

                    grid[i, u] = GO;
                }
            }
        }
    }

    void EscolherPosicaoDasPalavras()
    {
        string palavra = "AMARELO";

        //PosicionarPalavras(palavra, 2, 5, 2 + palavra.Length - 1, 5);

        List<string> listaPalavras = GetComponent<Palavras>().Lista();
        for (int i = 0; i < listaPalavras.Count; i++)
        {
            var PosicaoPalavra = DecidirLocal(listaPalavras[i]);
            PosicionarPalavras(listaPalavras[i],
                PosicaoPalavra.Item1, PosicaoPalavra.Item2, PosicaoPalavra.Item3, PosicaoPalavra.Item4);
        }
    }

    void PosicionarPalavras(string palavra, int iniX, int iniY, int fimX, int fimY)
    {
        if (iniX == fimX)
        {
            for (int i = 0; i < palavra.Length; i++)
            {
                grid[iniX, iniY + i].GetComponentInChildren<TextMeshProUGUI>().text = new string(palavra[i], 1);
            }
        }
        else if (iniY == fimY)
        {
            for (int i = 0; i < palavra.Length; i++)
            {
                grid[iniX + i, iniY].GetComponentInChildren<TextMeshProUGUI>().text = new string(palavra[i], 1);
            }
        }
        else if (iniY - iniX == fimY - fimX)
        {
            for (int i = 0; i < palavra.Length; i++)
            {
                grid[iniX + i, iniY + i].GetComponentInChildren<TextMeshProUGUI>().text = new string(palavra[i], 1);
            }
        }
        else
        {
            Debug.LogError("algo de errado nas coordenadas");
        }
    }

    (int,int,int,int) DecidirLocal(string palavra)
    {
        //posição da primeira(ou última) letra da palavra
        int PosicaoX = 0;
        int PosicaoY = 0;

        //direção que a palavra vai ser escrita (o contrário vai ser corrigido antes do "PosicionarPalavras")
        int DirecaoX = 0;
        int DirecaoY = 0;

        int FinalX = 0;
        int FinalY = 0;

        //variáveis de correção da direção da palavra
        //posição da primeira letra
        int MenorIdX = 0;
        int MenorIdY = 0;
        //posição da última letra
        int MaiorIdX = 0;
        int MaiorIdY = 0;

        int looptest = 0;

        do //repete apenas se devido a outras palavras essa posição e direção não são possíveis
        {
            EscolherPosicao(ref PosicaoX, ref PosicaoY);

            do
            {
                EscolherDirecao(ref DirecaoX, ref DirecaoY);

                FinalX = PosicaoX + palavra.Length * DirecaoX;
                FinalY = PosicaoY + palavra.Length * DirecaoY;

                MaiorIdX = Mathf.Max(PosicaoX, FinalX);
                MaiorIdY = Mathf.Max(PosicaoY, FinalY);

                MenorIdX = Mathf.Min(PosicaoX, FinalX);
                MenorIdY = Mathf.Min(PosicaoY, FinalY);

            } while (!PossivelPosicao(FinalX, FinalY));
            looptest++;
            if (looptest == 200)
            {
                print("infinito");
            }
        } while (!CaminhoLivre(palavra, MenorIdX, MenorIdY, MaiorIdX, MaiorIdY) || looptest == 200);

        /*print("tamanho da palavra: " + palavra.Length);
        print("posição inicial: " + PosicaoX + "," + PosicaoY);
        print("posição final: " + FinalX + "," + FinalY);
        print("direçãoX: " + DirecaoX + " || direçãoY: " + DirecaoY);*/

        return (MenorIdX, MenorIdY, MaiorIdX, MaiorIdY);
    }

    void EscolherDirecao(ref int X, ref int Y)
    {
        do
        {
            X = Random.Range(-1, 2);
            Y = Random.Range(-1, 2);
        } while ((X == 0 && Y == 0) || X * Y == -1);
    }

    void EscolherPosicao(ref int X, ref int Y)
    {
        X = Random.Range(0, GridSize);
        Y = Random.Range(0, GridSize);
    }

    bool PossivelPosicao(int ultimoX, int ultimoY)
    {
        bool b = false;

        if ((ultimoX < GridSize && ultimoY < GridSize)&&
           (ultimoX > 0 && ultimoY > 0))
        {
            b = true;
        }

        return b;
    }

    bool CaminhoLivre(string palavra, int inicialX, int inicialY, int finalX, int finalY)
    {
        bool livre = true;

        if (inicialX == finalX)
        {
            for (int i = inicialY; i < finalY; i++)
            {
                string letra = grid[inicialX, i].GetComponentInChildren<TextMeshProUGUI>().text;
                if (letra != "*")
                {
                    if(letra[0] != palavra[i - inicialY])
                    {
                        livre = false;
                    }
                }
            }
            //print("vertical: " + livre);
        }
        else if (inicialY == finalY)
        {
            for (int i = inicialX; i < finalX; i++)
            {
                string letra = grid[i, inicialY].GetComponentInChildren<TextMeshProUGUI>().text;
                if (letra != "*")
                {
                    if (letra[0] != palavra[i - inicialX])
                    {
                        livre = false;
                    }
                }
            }
            //print("horizontal: " + livre);
        }
        else if (inicialX - inicialY == finalX - finalY)
        {
            int yAtual = inicialY;
            for (int i = inicialX; i < finalX; i++)
            {
                string letra = grid[i, yAtual].GetComponentInChildren<TextMeshProUGUI>().text;
                if (letra != "*")
                {
                    if (letra[0] != palavra[i - inicialX])
                    {
                        livre = false;
                    }
                }
                yAtual++;
            }
            //print("diagonal: " + livre);
        }
        else
        {
            print("inconclusivo.");
            livre = false;
        }

        return livre;
    }

    void PreencherEspacos()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int u = 0; u < GridSize; u++)
            {
                if (grid[i, u].GetComponentInChildren<TextMeshProUGUI>().text == "*")
                {
                    char letra = (char)Random.Range(65, 91);
                    grid[i,u].GetComponentInChildren<TextMeshProUGUI>().text = new string(letra, 1);
                }
            }
        }
    }

    (int,int) PegarPosicao(GameObject alvo)
    {
        int finalX, finalY;
        finalX = GridSize;
        finalY = GridSize;
        for (int i = 0; i < GridSize; i++)
        {
            for (int u = 0; u < GridSize; u++)
            {
                if(grid[i,u] == alvo)
                {
                    finalX = i;
                    finalY = u;
                    return (finalX, finalY);
                }
            }
        }
        return (finalX, finalY);
    }

    public void PontoInicial(GameObject inicial)
    {
        var ID = PegarPosicao(inicial);
        if(ID.Item1 < GridSize && ID.Item2 < GridSize)
        {
            XPrimeira = ID.Item1;
            YPrimeira = ID.Item2;
            //print("a primeira letra esta em " + XPrimeira + "," + YPrimeira);

            inicial.transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
        }
    }

    public void SelecaoUltimaCelula(GameObject selecionada)
    {
        //marcando a última célula em caso de ele soltar o clique em cima dela
        UltimaCelula = selecionada;
        var ID = PegarPosicao(UltimaCelula);
        if (ID.Item1 < GridSize && ID.Item2 < GridSize)
        {
            XUltima = ID.Item1;
            YUltima = ID.Item2;
        }

        selecionada.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.yellow;
        AtualizarSelecao();
        //print("ultima célula é a" + UltimaCelula);
    }

    public void VerificarPalavra()
    {
        //print("ultima letra está em " + XUltima + "," + YUltima);
        string palavra = "";

        if(XPrimeira == XUltima)
        {
            int maior = Mathf.Max(YPrimeira, YUltima);
            int menor = Mathf.Min(YPrimeira, YUltima);

            for (int i = menor; i <= maior; i++)
            {
                palavra += grid[XPrimeira, i].GetComponentInChildren<TextMeshProUGUI>().text;
            }

            print(palavra);

            if (GetComponent<Palavras>().ChecarPalavra(palavra))
            {
                for (int i = menor; i <= maior; i++)
                {
                    grid[XPrimeira, i].GetComponent<Célula>().CorPadrão = Color.green;
                    grid[XPrimeira, i].transform.GetChild(1).GetComponent<Image>().color = Color.green;
                }
                GetComponent<Palavras>().MarcarPalavra(palavra);
            }
            else
            {
                XPrimeira = GridSize;
                YPrimeira = GridSize;
                LimparSelecao();
            }
        }
        else if(YPrimeira == YUltima)
        {
            int maior = Mathf.Max(XPrimeira, XUltima);
            int menor = Mathf.Min(XPrimeira, XUltima);

            for (int i = menor; i <= maior; i++)
            {
                palavra += grid[i, YPrimeira].GetComponentInChildren<TextMeshProUGUI>().text;
            }

            print(palavra);

            if (GetComponent<Palavras>().ChecarPalavra(palavra))
            {
                for (int i = menor; i <= maior; i++)
                {
                    grid[i, YPrimeira].GetComponent<Célula>().CorPadrão = Color.green;
                    grid[i, YPrimeira].transform.GetChild(1).GetComponentInChildren<Image>().color = Color.green;
                }
                GetComponent<Palavras>().MarcarPalavra(palavra);
            }
            else
            {
                XPrimeira = GridSize;
                YPrimeira = GridSize;
                LimparSelecao();
            }
        }
        else if(YPrimeira - XPrimeira == YUltima - XUltima)
        {
            int xmaior = Mathf.Max(XPrimeira, XUltima);
            int xmenor = Mathf.Min(XPrimeira, XUltima);

            int ymenor = Mathf.Min(YPrimeira, YUltima);
            int yAtual = ymenor;

            for (int i = xmenor; i <= xmaior; i++)
            {
                palavra += grid[i, yAtual].GetComponentInChildren<TextMeshProUGUI>().text;
                yAtual++;
            }

            print(palavra);

            if (GetComponent<Palavras>().ChecarPalavra(palavra))
            {
                yAtual = ymenor;
                for (int i = xmenor; i <= xmaior; i++)
                {
                    grid[i, yAtual].GetComponent<Célula>().CorPadrão = Color.green;
                    grid[i, yAtual].transform.GetChild(1).GetComponentInChildren<Image>().color = Color.green;
                    yAtual++;
                }
                GetComponent<Palavras>().MarcarPalavra(palavra);
            }
            else
            {
                XPrimeira = GridSize;
                YPrimeira = GridSize;
                LimparSelecao();
            }
        }
        else
        {
            Debug.LogError("não foi possivel formar uma sequencia de letras");
            XPrimeira = GridSize;
            YPrimeira = GridSize;
            LimparSelecao();
        }
    }

    void LimparSelecao()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int u = 0; u < GridSize; u++)
            {
                if (i == XPrimeira && u == YPrimeira)
                {
                    continue;
                }
                Color cor = grid[i, u].GetComponent<Célula>().CorPadrão;
                grid[i, u].transform.GetChild(1).GetComponent<Image>().color = cor;
            }
        }
    }

    void AtualizarSelecao()
    {
        LimparSelecao();

        if (XPrimeira == XUltima)
        {
            int maior = Mathf.Max(YPrimeira, YUltima);
            int menor = Mathf.Min(YPrimeira, YUltima);

            for (int i = menor; i <= maior; i++)
            {
                grid[XPrimeira, i].transform.GetChild(1).GetComponentInChildren<Image>().color = Color.yellow;
            }
        }
        else if (YPrimeira == YUltima)
        {
            int maior = Mathf.Max(XPrimeira, XUltima);
            int menor = Mathf.Min(XPrimeira, XUltima);

            for (int i = menor; i <= maior; i++)
            {
                grid[i, YPrimeira].transform.GetChild(1).GetComponentInChildren<Image>().color = Color.yellow;
            }
        }
        else if (YPrimeira - XPrimeira == YUltima - XUltima)
        {
            int xmaior = Mathf.Max(XPrimeira, XUltima);
            int xmenor = Mathf.Min(XPrimeira, XUltima);

            int ymenor = Mathf.Min(YPrimeira, YUltima);
            int yAtual = ymenor;

            for (int i = xmenor; i <= xmaior; i++)
            {
                grid[i, yAtual].transform.GetChild(1).GetComponentInChildren<Image>().color = Color.yellow;
                yAtual++;
            }
        }
        else
        {
            //Debug.LogError("não foi possivel colorir de as células");
        }
    }
}
