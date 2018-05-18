using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public globalFlock myManager;
    public float speed = 0.001f;
    //旋转速度
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    //鱼之间的距离
    float neighbourDistance = 3.0f;
    //鱼的转向
    bool turning = false;

    public float speedMult = 1;

    void Start()
    {
        //速度随机生成
        speed = Random.Range(0.5f, 1);
    }

   
    void Update()
    {
        //获得边界框
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);
        //判断鱼的活动范围
        //if(Vector3.Distance(transform.position,Vector3.zero)>=globalFlock.tankSize)
        if(!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if(turning)
        {
            Vector3 direction = myManager.transform.position- transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1)*speedMult;
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speedMult);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        //团队速度
        float gSpeed = 0.1f;

        Vector3 goalPos = myManager.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);

                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //鱼群大小大于0
        if (groupSize > 0)
        {
            //计算平均群的中心以及速度
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize*speedMult;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime);
            }
        }
    }
}
