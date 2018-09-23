using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class scft2 : Agent
{
    Rigidbody scrb;
    GameObject mars;
    GameObject marstrack;
    GameObject earth;
    Rigidbody earthvel;
    Rigidbody marsvel;
    Rigidbody earthsvel;
    Rigidbody marssvel;
    GameObject earths;
    GameObject marss;
    float DtoR;
    int fail=0;
    Vector3 spvel;
    Vector3 spang;
    Vector3 sppos;
    Vector3 spangvel;

    Vector3 msvel;
    Vector3 msang;
    Vector3 mspos;
    Vector3 msangvel;

    Vector3 etvel;
    Vector3 etang;
    Vector3 etpos;
    Vector3 etangvel;

    Vector3 mssvel;
    Vector3 mssang;
    Vector3 msspos;
    Vector3 mssangvel;

    Vector3 etsvel;
    Vector3 etsang;
    Vector3 etspos;
    Vector3 etsangvel;

    RaycastHit rayhit;
    bool raydirection;
    float varvel = 0.1f;
    int stage = 0;
    // Use this for initialization
    public override void InitializeAgent()
    {
        DtoR = 180 / Mathf.PI;
        earth = GameObject.Find("earth");

        scrb = GetComponent<Rigidbody>();
        marstrack = GameObject.Find("targetcol");
        mars = GameObject.Find("mars");
        marss = GameObject.Find("mars_revolve");
        earths = GameObject.Find("earth_revolve");
        marsvel = mars.GetComponent<Rigidbody>();
        marssvel = marss.GetComponent<Rigidbody>();
        earthvel = earth.GetComponent<Rigidbody>();
        earthsvel = earths.GetComponent<Rigidbody>();

        spvel = Vector3.zero;
        spang = transform.localEulerAngles;
        sppos = transform.position;
        spangvel = Vector3.zero;

        msvel = marsvel.velocity;
        msang = mars.transform.localEulerAngles;
        mspos = mars.transform.position;
        msangvel = marsvel.angularVelocity;
        mssvel = marssvel.velocity;
        mssang = marss.transform.localEulerAngles;
        msspos = marss.transform.position;
        mssangvel = marssvel.angularVelocity;

        etvel = earthvel.velocity;
        etang = earth.transform.localEulerAngles;
        etpos = earth.transform.position;
        etangvel = earthvel.angularVelocity;
        etsvel = earthsvel.velocity;
        etsang = earths.transform.localEulerAngles;
        etspos = earths.transform.position;
        etsangvel = earthsvel.angularVelocity;
    }
    public override void CollectObservations()
    {
        AddVectorObs(transform.position);
        AddVectorObs(scrb.velocity);
        AddVectorObs(mars.transform.position);
        AddVectorObs(marsvel.velocity);
        AddVectorObs(earth.transform.position);
        AddVectorObs(earthvel.velocity);
        AddVectorObs(marss.transform.position);
        AddVectorObs(marssvel.velocity);
        AddVectorObs(earths.transform.position);
        AddVectorObs(earthsvel.velocity);
    }
    /*
    private void OnDrawGizmos()
    {
        float maxdistance = mars.transform.position.magnitude - transform.position.magnitude;

        bool ishit = Physics.SphereCast(transform.position, 10, -transform.forward, out rayhit, maxdistance);
        Gizmos.color = Color.red;
        if (ishit)
        {
            Gizmos.DrawRay(transform.position, -transform.forward * rayhit.distance);
            Gizmos.DrawWireSphere(transform.position - transform.forward * rayhit.distance, 10 / 2);
        }
        else
        {
            Gizmos.DrawRay(transform.position, -transform.forward * maxdistance);
        }
    }
    */
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.DrawRay(transform.position, -transform.forward * (mars.transform.position.magnitude - transform.position.magnitude), Color.white);

        if (stage == 0)
        {
            scrb.AddForce(Vector3.forward * 500);//첫 추진 100 지구 대기권을 벗어나면 적당한 속도로 화성쪽으로 다가가게됨
         
        }
        else
        {
            SetReward(0.1f);//최저시급
            
            if (Physics.Raycast(transform.position, -transform.forward, out rayhit, (mars.transform.position.magnitude - transform.position.magnitude)))
            {

                if (rayhit.collider.tag == "target")//If Laser hit target
                {
                    raydirection = true;//It is good
                    SetReward(1f);
                    Debug.Log("Great");
                    fail = 1;
                }
            }
            if (!Physics.Raycast(transform.position, -transform.forward, out rayhit, 100000.0f))
            {
                raydirection = false;//It is good
                Debug.Log("Bad");
                SetReward(-0.1f);
            }
        }
        
        if (transform.position.y > -5000 && stage == 0)//블랙박스에서 텔레포트 == 지구 대기권돌파
        {

            stage = 1;//우주환경, 학습시작
            
            msvel = marsvel.velocity;//현재 속도저장
            mspos = mars.transform.position;//현재 위치 저장
            msang = mars.transform.localEulerAngles;//현재 회전벡터 저장
            msangvel = marsvel.angularVelocity;//현재 각속도 저장
            
            mssvel = marssvel.velocity;//현재 속도저장
            msspos = marss.transform.position;//현재 위치 저장
            mssang = marss.transform.localEulerAngles;//현재 회전벡터 저장
            mssangvel = marssvel.angularVelocity;//현재 각속도 저장
            
        }
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)//AI
        {
            var actionx = Mathf.Clamp(vectorAction[0], 0, 1f);//Yaw 제어
            var actiony = Mathf.Clamp(vectorAction[1], 0, 1f);//Pitch 제어
            var action = Mathf.Clamp(vectorAction[2], -1f, 1f);
            Mathf.Clamp(transform.localEulerAngles.x, -90, 90);

            if (stage == 1)
            {
                if (transform.position.magnitude < mars.transform.position.magnitude)//화성이 기체보다 더 멀리있을때
                {
                    Vector3 dt = mars.transform.position - transform.position;
                    if (dt.magnitude < 1000)//화성과 기체의 거리가 1000보다 가까울때
                    {
                        scrb.AddForce(Vector3.back * 100);
                        //Debug.Log(scrb.velocity);
                        if (scrb.velocity.z < 5)//거의 정지하다시피 천천히 오게하자
                        {
                            spvel = scrb.velocity;
                            spang = transform.localEulerAngles;
                            sppos = transform.position;
                            spangvel = Vector3.zero;

                            msvel = marsvel.velocity;
                            msang = mars.transform.localEulerAngles;
                            mspos = mars.transform.position;
                            msangvel = marsvel.angularVelocity;


                            etvel = earthvel.velocity;
                            etang = earth.transform.localEulerAngles;
                            etpos = earth.transform.position;
                            etangvel = earthvel.angularVelocity;

                            stage = 2;

                        }
                    }
                }
            }
            if(stage ==2)
            {
                SetReward(0.5f);
                float distance = Mathf.Sqrt(Mathf.Pow(mars.transform.position.x - transform.position.x, 2) + Mathf.Pow(mars.transform.position.y - transform.position.y, 2) + Mathf.Pow(mars.transform.position.z - transform.position.z, 2));
                float pitch = Mathf.Asin((mars.transform.position.y - transform.position.y) / distance);
                float yaw = Mathf.Asin((mars.transform.position.x - transform.position.x) / distance);
                if (distance < 15)
                    stage = 3;
                else
                    scrb.AddForce(new Vector3(mars.transform.position.x - transform.position.x, mars.transform.position.y + 10 - transform.position.y, mars.transform.position.z - transform.position.z) * varvel);
                
                transform.localEulerAngles = new Vector3(pitch * 57.2958f, 180 + yaw * 57.2958f,0);
            }
            if(stage==3)
            {
                float distance = Mathf.Sqrt(Mathf.Pow(mars.transform.position.x - transform.position.x, 2) + Mathf.Pow(mars.transform.position.y - transform.position.y, 2) + Mathf.Pow(mars.transform.position.z - transform.position.z, 2));
                float yaw = Mathf.Asin((mars.transform.position.x - transform.position.x) / distance);
                transform.position = new Vector3(mars.transform.position.x, mars.transform.position.y + 10, mars.transform.position.z);
                transform.localEulerAngles = new Vector3(0, 270 + yaw * 57.2958f, 0);
                
            }
        }
        
        if (transform.position.z > mars.transform.position.z)//화성을 지나쳐가면 경고
        {
            SetReward(-0.5f);
            
            if (transform.position.z > mars.transform.position.z + 100)//화성을 너무지나치면 리셋
            {
                SetReward(-1f);
                Done();
                //Debug.Log("1");
            }
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="target")
        {
            scrb.velocity = Vector3.zero;
            stage = 3;
        }
    }
    */
    public override void AgentReset()
    {
        Debug.Log("리셋!");
        stage = 0;
        scrb.velocity = spvel;
        scrb.angularVelocity = Vector3.zero;
        transform.position = sppos;
        transform.localEulerAngles = spang;
        marsvel.velocity = msvel;
        marsvel.angularVelocity = msangvel;
        mars.transform.position = mspos;
        mars.transform.localEulerAngles = msang;
        earthvel.velocity = etvel;
        earthvel.angularVelocity = etangvel;
        earth.transform.position = etpos;
        earth.transform.localEulerAngles = etang;
        varvel = 0.1f;
        marssvel.velocity = mssvel;
        marssvel.angularVelocity = mssangvel;
        marss.transform.position = msspos;
        mars.transform.localEulerAngles = mssang;
        earthsvel.velocity = etsvel;
        earthsvel.angularVelocity = etsangvel;
        earths.transform.position = etspos;
        earths.transform.localEulerAngles = etsang;
    }
}
