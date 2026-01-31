using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTest : MonoBehaviour
{
    public GameObject testObject;
    bool isBurning = false;
    /*float burnMinValue = 0;
    float burnMaxValue = 0;*/
    float burnTransision = 0.0f;
    float currentBurnGrade = 0;
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("_BurnGrade",-0.01f);
        //burnMaxValue = testObject.GetComponent<Renderer>().material.GetFloat("_BurnGrade");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //currentBurnGrade = testObject.GetComponent<Renderer>().material.GetFloat("_BurnGrade");
           
            currentBurnGrade = Shader.GetGlobalFloat("_BurnGrade");
            Debug.Log(currentBurnGrade);

            if(isBurning){
               /* while(currentBurnGrade > burnMinValue){
                    testObject.GetComponent<Renderer>().material.SetFloat("_BurnGrade",Mathf.Lerp(burnMinValue,burnMaxValue,burnTransision));
                    //Shader.SetGlobalFloat("_BurnGrade",Mathf.Lerp(burnMinValue,burnMaxValue,burnTransision));
                    burnTransision += 0.1f * Time.deltaTime;
                    currentBurnGrade = testObject.GetComponent<Renderer>().material.GetFloat("_BurnGrade");
                    //currentBurnGrade = Shader.GetGlobalFloat("_BurnGrade");
                    Debug.Log(currentBurnGrade);
                }*/
                Shader.SetGlobalFloat("_BurnGrade",-0.01f);
                isBurning = false;
            }else{
               /* while(currentBurnGrade < burnMaxValue){
                    testObject.GetComponent<Renderer>().material.SetFloat("_BurnGrade",Mathf.Lerp(burnMaxValue,burnMinValue,burnTransision));
                    //Shader.SetGlobalFloat("_BurnGrade",Mathf.Lerp(burnMinValue,burnMaxValue,burnTransision));
                    burnTransision += 0.1f * Time.deltaTime;
                    currentBurnGrade = testObject.GetComponent<Renderer>().material.GetFloat("_BurnGrade");
                    //currentBurnGrade = Shader.GetGlobalFloat("_BurnGrade");
                    Debug.Log(currentBurnGrade);
                }*/
                Shader.SetGlobalFloat("_BurnGrade",1);
                isBurning = true;
            }
            burnTransision = 0;
            //Debug.Log(testObject.GetComponent<Renderer>().material.GetFloat("_BurnGrade"));
        }
    }
}
