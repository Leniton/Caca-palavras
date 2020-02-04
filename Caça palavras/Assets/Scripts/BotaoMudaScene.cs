using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoMudaScene : MonoBehaviour
{
    
    public void Proxima()
    {
        SceneM.manager.NextScene();
    }

    public void IrPara(int n)
    {
        SceneM.manager.LoadScene(n);
    }
}
