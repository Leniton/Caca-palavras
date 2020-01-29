using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerarGrid : MonoBehaviour
{
    public int GridSize;
    GameObject Celula;

    public GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[GridSize,GridSize];
        Celula = transform.GetChild(0).gameObject;

        CriarGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CriarGrid()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int u = 0; u < GridSize; u++)
            {
                if(!(i == 0 && u == 0))
                {
                    Vector3 posicao = new Vector3(transform.position.x + 20 * i, transform.position.y - 20 * u, 0);
                    GameObject g = Instantiate(Celula, posicao, Quaternion.identity,transform);
                    grid[i, u] = g;
                }
            }
        }
    }
}
