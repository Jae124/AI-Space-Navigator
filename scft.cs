using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class scft : Agent
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
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Debug.DrawRay(transform.position, -transform.forward * (mars.transform.position.magnitude-transform.position.magnitude), Color.white);
        
        if (stage == 0)
        {
            scrb.AddForce(Vector3.forward * 500);//첫 추진 100 지구 대기권을 벗어나면 적당한 속도로 화성쪽으로 다가가게됨
            //scrb.AddForce(-transform.forward * 1);//첫 추진 100 지구 대기권을 벗어나면 적당한 속도로 화성쪽으로 다가가게됨
            //scrb.AddForceAtPosition(Vector3.forward * 0.1f, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z));
            /*
            if (Input.GetKey(KeyCode.W))//고도낮
            {
                scrb.AddForceAtPosition(Vector3.up * 1f, new Vector3(transform.position.x, transform.position.y, transform.position.z-2));
                scrb.AddForceAtPosition(Vector3.down * 1f, new Vector3(transform.position.x, transform.position.y, transform.position.z+ 2));
            }
            if (Input.GetKey(KeyCode.A))
            {
                scrb.AddForceAtPosition(Vector3.right * 1f, new Vector3(transform.position.x, transform.position.y, transform.position.z -2));
                scrb.AddForceAtPosition(Vector3.left * 1f, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z+2));
            }
            if (Input.GetKey(KeyCode.S))
            {
                scrb.AddForceAtPosition(Vector3.up * 1f, new Vector3(transform.position.x, transform.position.y, transform.position.z+2 ));
                scrb.AddForceAtPosition(Vector3.down * 1f, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2));
            }
            if (Input.GetKey(KeyCode.D))
            {
                scrb.AddForceAtPosition(Vector3.left * 1f, new Vector3(transform.position.x-2, transform.position.y, transform.position.z-2));
                scrb.AddForceAtPosition(Vector3.right *1f, new Vector3(transform.position.x +2, transform.position.y, transform.position.z+2));
            }
            if (Input.GetKey(KeyCode.Space))
            {
                scrb.AddForce(transform.forward * -0.1f);

            }
            */

        }
        //Debug.Log(transform.localEulerAngles);

        /*
        if(transform.localEulerAngles.y>180)
        {
            Debug.Log(Mathf.Sin((transform.localEulerAngles.y-180) * DtoR) * (mars.transform.position.magnitude - transform.position.magnitude));//x
        }
        else
        {
            Debug.Log(Mathf.Sin((180-transform.localEulerAngles.y) * DtoR) * (mars.transform.position.magnitude - transform.position.magnitude));//-x
        }

        if (transform.localEulerAngles.x > 0)
        {
            Debug.Log(Mathf.Sin(Mathf.Abs(transform.localEulerAngles.x) * DtoR) * (mars.transform.position.magnitude - transform.position.magnitude));//y
        }
        else
        {
            Debug.Log(Mathf.Sin(Mathf.Abs(transform.localEulerAngles.x) * DtoR) * (mars.transform.position.magnitude - transform.position.magnitude));//-y
        }
        */
        if (stage != 0)
        {
            SetReward(0.1f);//최저시급
            
            if(Physics.Raycast(transform.position,-transform.forward, out rayhit, (mars.transform.position.magnitude - transform.position.magnitude)))
            {
                
                if (rayhit.collider.tag == "target")//If Laser hit target
                {
                    raydirection = true;//It is good
                    SetReward(1f);
                    Debug.Log("Great");
                }
                
                /*
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
                if (rayhit.collider.tag == "subtarget"&& rayhit.collider.tag == "subtarget2")//If Laser hit subtarget
                {
                    SetReward(1f);
                    Debug.Log("Great");
                }
                */
                
            }
            if (!Physics.Raycast(transform.position, -transform.forward, out rayhit, 100000.0f))
            {
                raydirection = false;//It is good
                Debug.Log("Bad");
                SetReward(-0.1f);
            }
        }

        if (transform.position.y > -5000 && stage==0)//블랙박스에서 텔레포트 == 지구 대기권돌파
        {
            
            stage = 1;//우주환경, 학습시작
            /*
            spvel = scrb.velocity;//현재 속도저장
            sppos = transform.position;//현재 위치 저장
            spang = transform.localEulerAngles;//현재 회전벡터 저장
            spangvel = scrb.angularVelocity;//현재 각속도 저장
            */
            msvel = marsvel.velocity;//현재 속도저장
            mspos = mars.transform.position;//현재 위치 저장
            msang = mars.transform.localEulerAngles;//현재 회전벡터 저장
            msangvel = marsvel.angularVelocity;//현재 각속도 저장
            /*
            etvel = earthvel.velocity;//현재 속도저장
            etpos = earth.transform.position;//현재 위치 저장
            etang = earth.transform.localEulerAngles;//현재 회전벡터 저장
            etangvel = earthvel.angularVelocity;//현재 각속도 저장
            */
            mssvel = marssvel.velocity;//현재 속도저장
            msspos = marss.transform.position;//현재 위치 저장
            mssang = marss.transform.localEulerAngles;//현재 회전벡터 저장
            mssangvel = marssvel.angularVelocity;//현재 각속도 저장
            /*
            etsvel = earthsvel.velocity;//현재 속도저장
            etspos = earths.transform.position;//현재 위치 저장
            etsang = earths.transform.localEulerAngles;//현재 회전벡터 저장
            etsangvel = earthsvel.angularVelocity;//현재 각속도 저장
            */
        }
        if(brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)//AI
        {
            var actionx = Mathf.Clamp(vectorAction[0], 0, 1f);//Yaw 제어
            var actiony = Mathf.Clamp(vectorAction[1], 0, 1f);//Pitch 제어
            var action = Mathf.Clamp(vectorAction[2], -10f,0);
            Mathf.Clamp(transform.localEulerAngles.x, -90, 90);
            if(transform.position.y < mars.transform.position.y && stage== 2 && raydirection == false)//고도를 높여야할때
            {
                //scrb.AddForceAtPosition(Vector3.forward * actiony, new Vector3(transform.position.x , transform.position.y+100, transform.position.z-2));//고도를 위로

                /*
                scrb.AddForceAtPosition(Vector3.up * actiony, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2));
                scrb.AddForceAtPosition(Vector3.down * actiony, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2));
                */
                scrb.AddForce(-transform.forward * 20);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + actionx, transform.localEulerAngles.y, transform.localEulerAngles.z);
                Debug.Log("올라간드아");
                if(transform.localEulerAngles.x>180)//고도 높여도 시원찮은판에 고도 낮게 날때
                {
                    SetReward(-1f);
                }
            }
            if (transform.position.y > mars.transform.position.y && stage == 2 && raydirection ==false)//고도를 낮춰야할때
            {
                //scrb.AddForceAtPosition(Vector3.forward * actiony, new Vector3(transform.position.x + 2, transform.position.y+100, transform.position.z+2));//고도를 아래로
                /*
                scrb.AddForceAtPosition(Vector3.up *actiony, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2));
                scrb.AddForceAtPosition(Vector3.down * actiony, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2));
                */
                scrb.AddForce(transform.forward * action);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - actionx, transform.localEulerAngles.y, transform.localEulerAngles.z);
                Debug.Log("내려간드아");
                if (transform.localEulerAngles.x > 0)
                {
                    SetReward(-1f);
                }
            }
            if (transform.position.x>mars.transform.position.x && stage == 3)//좌회전이 필요할때
            {
                //scrb.AddForceAtPosition(Vector3.forward * actionx, new Vector3(transform.position.x + 2, transform.position.y+100, transform.position.z));//좌회전
                /*
                scrb.AddForceAtPosition(Vector3.right * actionx, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2));
                scrb.AddForceAtPosition(Vector3.left * actionx, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 2));
                */
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x , transform.localEulerAngles.y-actiony, transform.localEulerAngles.z);
                Debug.Log("왼쪽간드아");
                if (transform.localEulerAngles.y > 180)
                {
                    SetReward(-0.5f);
                    if (transform.localEulerAngles.y > 270)
                    {
                        SetReward(-1f);
                        Done();
                        //Debug.Log("3");
                    }
                }
            }
            if (transform.position.x < mars.transform.position.x && stage == 3)//우회전이 필요할때
            {
                //scrb.AddForceAtPosition(Vector3.forward * actionx, new Vector3(transform.position.x - 2, transform.position.y+100, transform.position.z));//우회전
                /*
                scrb.AddForceAtPosition(Vector3.left * actionx, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 2));
                scrb.AddForceAtPosition(Vector3.right * actionx, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2));
                */
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y-actiony, transform.localEulerAngles.z);
                Debug.Log("오른쪽간드아");
                if (transform.localEulerAngles.y < 180)
                {
                    SetReward(-0.5f);
                    if (transform.localEulerAngles.y < 90)
                    {
                        SetReward(-1f);
                        Done();
                        //Debug.Log("4");
                    }
                }
            }
            if (stage == 1)
            {
                if(transform.position.magnitude<mars.transform.position.magnitude)//화성이 기체보다 더 멀리있을때
                {
                    Vector3 dt = mars.transform.position - transform.position;
                    if (dt.magnitude < 1000)//화성과 기체의 거리가 1000보다 가까울때
                    {
                        scrb.AddForce(Vector3.back * 100);
                        //Debug.Log(scrb.velocity);
                        if (scrb.velocity.z<5)//거의 정지하다시피 천천히 오게하자
                        {
                            spvel = Vector3.zero;
                            spang = transform.localEulerAngles;
                            sppos = transform.position;
                            spangvel = Vector3.zero;

                            msvel = marsvel.velocity;
                            msang = mars.transform.localEulerAngles;
                            mspos = mars.transform.position;
                            msangvel = marsvel.angularVelocity;
                            /*
                            mssvel = marssvel.velocity;
                            mssang = marss.transform.localEulerAngles;
                            msspos = marss.transform.position;
                            mssangvel = marssvel.angularVelocity;
                            */

                            etvel = earthvel.velocity;
                            etang = earth.transform.localEulerAngles;
                            etpos = earth.transform.position;
                            etangvel = earthvel.angularVelocity;
                            /*
                            etsvel = earthsvel.velocity;
                            etsang = earths.transform.localEulerAngles;
                            etspos = earths.transform.position;
                            etsangvel = earthsvel.angularVelocity;
                            */
                            stage = 2;

                        }
                    }
                }


                if (/*Mathf.Abs(scrb.velocity.magnitude) < (action * -1) &&*/ stage == 2)//action의 속도로 비행
                {//고도맞추기
                    //scrb.AddForce(transform.forward * action);
                    Debug.Log("접근중");
                    scrb.AddForce(-transform.forward * 50);
                    Vector3 dt = mars.transform.position - transform.position;
                    SetReward((1000f - dt.magnitude) / 100);
                }
                
            }
            //자세제어를 위해서 추진방향으로 힘을주었을때에, 왼쪽에 힘을주면 우회전 뒤에힘을주면 위로, 앞에힘을주면 밑으로, 오른쪽은 좌회전임
            //scrb.AddForceAtPosition(Vector3.forward * 0.1f, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z));
        }
        if(transform.position.z>mars.transform.position.z)//화성을 지나쳐가면 경고
        {
            SetReward(-0.5f);
            if (transform.position.z > mars.transform.position.z+100)//화성을 너무지나치면 리셋
            {
                SetReward(-1f);
                Done();
                //Debug.Log("1");
            }
        }
    }
    public override void AgentReset()
    {
        Debug.Log("리셋!");
        stage = 0;
        scrb.velocity = Vector3.zero;
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
