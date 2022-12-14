using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class RunModel : MonoBehaviour{
    
    [SerializeField] private NNModel modelAsset;
    private Model m_RuntimeModel;

    void Start(){
        m_RuntimeModel = ModelLoader.Load(modelAsset);  
    }

   
}
