using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class scfs : Agent
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

    int stage = 0;
    // Use this for initialization
    public override void InitializeAgent()
    {
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

        msvel = Vector3.zero;
        msang = mars.transform.localEulerAngles;
        mspos = mars.transform.position;
        msangvel = Vector3.zero;
        mssvel = Vector3.zero;
        mssang = marss.transform.localEulerAngles;
        msspos = marss.transform.position;
        mssangvel = Vector3.zero;

        etvel = Vector3.zero;
        etang = earth.transform.localEulerAngles;
        etpos = earth.transform.position;
        etangvel = Vector3.zero;
        etsvel = Vector3.zero;
        etsang = earths.transform.localEulerAngles;
        etspos = earths.transform.position;
        etsangvel = Vector3.zero;
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
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.DrawRay(transform.position, -transform.forward * 100000, Color.white);

        if (stage == 0)
        {
            scrb.AddForce(Vector3.forward * 100);//첫 추진 100 지구 대기권을 벗어나면 적당한 속도로 화성쪽으로 다가가게됨
            //scrb.AddForceAtPosition(Vector3.forward * 0.1f, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z));
        }
        if (stage != 0)
        {
            SetReward(0.1f);//최저시급
            if (Physics.Raycast(transform.position, -transform.forward, out rayhit, 100000.0f))
            {
                /*
                if (rayhit.collider.tag == "target")//If Laser hit target
                {
                    raydirection = true;//It is good
                    SetReward(1f);
                    Debug.Log("Great");
                }
                */
                if (rayhit.collider.tag == "subtarget")//If Laser hit subtarget
                {
                    SetReward(0.5f);
                    Debug.Log("Good");
                }
                if (rayhit.collider.tag == "subtarget2")//If Laser hit subtarget
                {
                    SetReward(0.5f);
                    Debug.Log("Good");
                }
                if (rayhit.collider.tag == "subtarget" && rayhit.collider.tag == "subtarget2")//If Laser hit subtarget
                {
                    SetReward(1f);
                    Debug.Log("Great");
                }

            }
            if (!Physics.Raycast(transform.position, -transform.forward, out rayhit, 100000.0f))
            {
                raydirection = false;//It is good
                Debug.Log("Bad");
            }
        }

        if (transform.position.x == 0)//블랙박스에서 텔레포트 == 지구 대기권돌파
        {
            stage = 1;//우주환경, 학습시작
            spvel = scrb.velocity;//현재 속도저장
            sppos = transform.position;//현재 위치 저장
            spang = transform.localEulerAngles;//현재 회전벡터 저장
            spangvel = scrb.angularVelocity;//현재 각속도 저장

            msvel = marsvel.velocity;//현재 속도저장
            mspos = mars.transform.position;//현재 위치 저장
            msang = mars.transform.localEulerAngles;//현재 회전벡터 저장
            msangvel = marsvel.angularVelocity;//현재 각속도 저장

            etvel = earthvel.velocity;//현재 속도저장
            etpos = earth.transform.position;//현재 위치 저장
            etang = earth.transform.localEulerAngles;//현재 회전벡터 저장
            etangvel = earthvel.angularVelocity;//현재 각속도 저장

            mssvel = marssvel.velocity;//현재 속도저장
            msspos = marss.transform.position;//현재 위치 저장
            mssang = marss.transform.localEulerAngles;//현재 회전벡터 저장
            mssangvel = marssvel.angularVelocity;//현재 각속도 저장

            etsvel = earthsvel.velocity;//현재 속도저장
            etspos = earths.transform.position;//현재 위치 저장
            etsang = earths.transform.localEulerAngles;//현재 회전벡터 저장
            etsangvel = earthsvel.angularVelocity;//현재 각속도 저장
        }
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)//AI
        {
            var actionx = 0.1f * Mathf.Clamp(vectorAction[0], 0, 1f);//Yaw 제어
            var actiony = 0.1f * Mathf.Clamp(vectorAction[1], 0, 1f);//Pitch 제어
            var action = Mathf.Clamp(vectorAction[2], 0, 100f);

            if (transform.position.y < mars.transform.position.y && stage == 1)//고도를 높여야할때
            {
                scrb.AddForceAtPosition(Vector3.forward * actiony, new Vector3(transform.position.x, transform.position.y + 100, transform.position.z - 2));//고도를 위로
            }
            if (transform.position.y > mars.transform.position.y && stage == 1)//고도를 낮춰야할때
            {
                scrb.AddForceAtPosition(Vector3.forward * actiony, new Vector3(transform.position.x + 2, transform.position.y + 100, transform.position.z + 2));//고도를 아래로
            }
            if (transform.position.x > mars.transform.position.x && stage == 1)//좌회전이 필요할때
            {
                scrb.AddForceAtPosition(Vector3.forward * actionx, new Vector3(transform.position.x + 2, transform.position.y + 100, transform.position.z));//좌회전
            }
            if (transform.position.x < mars.transform.position.x && stage == 1)//우회전이 필요할때
            {
                scrb.AddForceAtPosition(Vector3.forward * actionx, new Vector3(transform.position.x - 2, transform.position.y + 100, transform.position.z));//우회전
            }
            if (stage == 1)
                scrb.AddForce(transform.forward * action);
            //자세제어를 위해서 추진방향으로 힘을주었을때에, 왼쪽에 힘을주면 우회전 뒤에힘을주면 위로, 앞에힘을주면 밑으로, 오른쪽은 좌회전임
            //scrb.AddForceAtPosition(Vector3.forward * 0.1f, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z));
        }
        if (transform.position.z > mars.transform.position.z)//화성을 지나쳐가면 경고
        {
            SetReward(-0.5f);
            if (transform.position.z > mars.transform.position.z + 1000)//화성을 지나쳐가면 경고
            {
                SetReward(-1f);
                Done();
            }
        }
    }
    public override void AgentReset()
    {
        Debug.Log("리셋!");
        scrb.velocity = spvel;
        scrb.angularVelocity = spangvel;
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
