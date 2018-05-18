using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public globalFlock myFlock;
    public GameObject fishPrefab;

    static int numFish = 20;
    //声明鱼群数组
    public static GameObject[] allFish = new GameObject[numFish];
    public Vector3 goalPos;
    //鱼群活动范围
    public Vector3 swimLimits = new Vector3(2, 2, 2);
    //生成范围
    //public static int tankSize = 1;
    //目标位置
    //public static Vector3 goalPos = Vector3.zero;

    public void FishSpeed(float speedMult)
    {
        Debug.Log(speedMult);
        for(int i = 0; i < numFish; i++)
        {
            allFish[i].GetComponent<Flock>().speedMult = speedMult;
        }
    }
    //限制鱼群游动的区域
    void  OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 0.5f, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(swimLimits.x*2,swimLimits.y*2,swimLimits.z*2));
        Gizmos.color = new Color(1,1,1,1);
        Gizmos.DrawSphere(goalPos, 0.1f);

    }
    void Start()
    {
        myFlock = this;
        goalPos = this.transform.position;
        RenderSettings.fogColor = Camera.main.backgroundColor;
        RenderSettings.fogDensity = 0.03f;
        RenderSettings.fog = true;
        //通过遍历鱼随机设置他们的位置
        for (int i = 0; i < numFish; i++)
        {
            //位置
            Vector3 pos = new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            //Vector3 pos = new Vector3(1.0f,1.2f,1.3f);
            //一 预设 二实例化预设坐标 三实例化预设角度
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) < 50)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                  Random.Range(-swimLimits.y, swimLimits.y),
                                                   Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}

