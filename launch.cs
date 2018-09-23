using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.IO;
//using System;
//python 3.5ver/Anaconda 4.2.0ver/Geforce를 사용할 경우에 Cuda8.0 Cudnn5.1ver/
//anaconda prompt를 관리자권한으로 실행, pip install Tensorflow-GPU
//pip install numpy scipy pandas scitki-learn
//경로 들어가서 python python\learn.py –-run-id=stdml –-train

public class launch : Agent {

    GameObject mars;
    Rigidbody marsrb;

    GameObject earth;
    Rigidbody earthrb;

    GameObject moon;

    Rigidbody moonrb;

    GameObject moonrev;
    Rigidbody moonrevrb;

    Rigidbody roketrb;
    int stage = 0;
    int count = 0;
    int success = 0;
    int fail = 0;

    int min = 0;
    float sec = 0;

    int lnmin = 0;
    float lnsec = 0;

    float timer=0;
    int addsec = 0;

    int start = 0;
    string result;
    string launchingtime;
    int printdata;

    string[] readdata;

    string m_strPath = "Assets/learningdata/data.txt";

    Vector3 mspos, msvel, msang, msangvel;
    Vector3 earthpos, earthvel, earthang, earthangvel;
    Vector3 mnpos, mnvel, mnang, mnangvel;
    Vector3 moonrevpos, moonrevvel, moonrevang, moonrevangvel;

    float rforce = 160;

    int initstate = 0;
    public override void InitializeAgent()
    {
        

        roketrb = GetComponent<Rigidbody>();

        mars = GameObject.Find("Mars");
        earth = GameObject.Find("Earth");
        moon = GameObject.Find("Moon");
        moonrev = GameObject.Find("moonrev");

        marsrb = mars.GetComponent<Rigidbody>();
        earthrb = earth.GetComponent<Rigidbody>();
        moonrb = moon.GetComponent<Rigidbody>();
        moonrevrb = moonrev.GetComponent<Rigidbody>();

        transform.position = new Vector3(earth.transform.position.x, earth.transform.position.y + 5, earth.transform.position.z);

        mspos = mars.transform.position;
        msvel = marsrb.velocity;
        msang = mars.transform.localEulerAngles;
        msangvel = marsrb.angularVelocity;

        earthpos = earth.transform.position;
        earthang = earth.transform.localEulerAngles;
        earthvel = earthrb.velocity;
        earthangvel = earthrb.angularVelocity;

        moonrevpos = moonrev.transform.position;
        moonrevang = moonrev.transform.localEulerAngles;
        moonrevvel = moonrevrb.velocity;
        moonrevangvel = moonrevrb.angularVelocity;
    }
    public override void CollectObservations()
    {
        AddVectorObs(mars.transform.position);
        AddVectorObs(marsrb.velocity);
        AddVectorObs(earth.transform.position);
        AddVectorObs(earthrb.velocity);
        /*
        AddVectorObs(moon.transform.position);
        AddVectorObs(moonrb.velocity);
        */
        AddVectorObs(moonrev.transform.position);
        AddVectorObs(moonrevrb.velocity);
        
    }
    int i = 0;
    int k = 0;
    int j = 0;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        initstate = 1;
        readdata = new string[1000];
        if (brain.brainType == BrainType.Internal)//학습데이터 가져올때
        {
            FileStream fs = new FileStream(m_strPath, FileMode.Open);//create는 덮어쓰기
            
            StreamReader sr = new StreamReader(fs);
            
            for(i=0; i<1000; i++)
            {
                readdata[i] = sr.ReadLine();
                if (readdata[i].StartsWith("end"))
                {
                    k = i;
                    break;
                }
            }

            
            //min = System.Convert.ToInt32(sr.ReadLine());
            /*
            launchingtime = readdata[j];//언제쏘는가
            result = readdata[j + 1];
            lnmin = int.Parse(readdata[j+2]);
            lnsec = float.Parse(readdata[j+3]);
            */
            if (j + 4 != k)
                j = j + 4;

            sr.Close();
            fs.Close();

            
            if (printdata == 1)//출력버튼눌렀을때
            {
                //발사시간 데이터가 존재하는지
                for(i=0;i<k;i++)
                {
                    if(int.Parse(readdata[4*i]) == min * 60 + Mathf.FloorToInt(sec))//일치하는 데이터가있으면
                    {
                        min = int.Parse(readdata[4*i+2]);
                        sec = float.Parse(readdata[4*i + 3]);
                        result = readdata[4*i + 1];
                    }
                }
            }
        }

        if (brain.brainType == BrainType.External)//학습중일때
        {
            start = 1;
            var actionx = Mathf.Clamp(vectorAction[0], 2, 20f);
            var actionz = Mathf.Clamp(vectorAction[1], 2, 20f);

            if (Mathf.Abs(transform.position.x) > 120 || Mathf.Abs(transform.position.z) > 120)//화성 이후의 궤도로 벗어남
            {
                SetReward(-1f);
                Done();
                fail++;
                count++;
                result = "fail";
            }

            float marsdistance = Mathf.Sqrt(Mathf.Pow(mars.transform.position.x - transform.position.x, 2) + Mathf.Pow(mars.transform.position.y - transform.position.y, 2) + Mathf.Pow(mars.transform.position.z - transform.position.z, 2));
            float earthdistance = Mathf.Sqrt(Mathf.Pow(earth.transform.position.x - transform.position.x, 2) + Mathf.Pow(earth.transform.position.y - transform.position.y, 2) + Mathf.Pow(earth.transform.position.z - transform.position.z, 2));

            if (transform.position.x > mars.transform.position.x && stage == 1)//로켓이 화성보다 x축에 가까울경우
            {//로켓이 x축반대로 가야할것이다.
                if (roketrb.velocity.x > -actionx)
                {
                    Debug.Log("왼쪽");
                    roketrb.AddForce(Vector3.left * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.x < mars.transform.position.x && stage == 1)
            {
                if (roketrb.velocity.x < actionx)
                {
                    Debug.Log("오른쪽");
                    roketrb.AddForce(Vector3.right * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.z > mars.transform.position.z && stage == 1)//로켓이 화성보다 z축에 가까울경우
            {//로켓이 z축반대로 가야할것이다.
                if (roketrb.velocity.z > -actionz)
                {
                    Debug.Log("뒤로");
                    roketrb.AddForce(Vector3.back * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.z < mars.transform.position.z && stage == 1)
            {
                if (roketrb.velocity.z < actionz)
                {
                    Debug.Log("앞으로");
                    roketrb.AddForce(Vector3.forward * rforce);
                    SetReward(0.1f);
                }
            }
            if (stage == 1 && marsdistance < 10.1f)//충분히 근접
            {
                stage = 2;
                SetReward(1f);
            }
            //Debug.Log(marsdistance);
            //귀환
            if (transform.position.x > earth.transform.position.x && stage == 2)//로켓이 화성보다 x축에 가까울경우
            {//로켓이 x축반대로 가야할것이다.
                if (roketrb.velocity.x > -actionx)
                {
                    Debug.Log("왼쪽");
                    roketrb.AddForce(Vector3.left * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.x < earth.transform.position.x && stage == 2)
            {
                if (roketrb.velocity.x < actionx)
                {
                    Debug.Log("오른쪽");
                    roketrb.AddForce(Vector3.right * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.z > earth.transform.position.z && stage == 2)//로켓이 화성보다 z축에 가까울경우
            {//로켓이 z축반대로 가야할것이다.
                if (roketrb.velocity.z > -actionz)
                {
                    Debug.Log("뒤로");
                    roketrb.AddForce(Vector3.back * rforce);
                    SetReward(0.1f);
                }
            }
            if (transform.position.z < earth.transform.position.z && stage == 2)
            {
                if (roketrb.velocity.z < actionz)
                {
                    Debug.Log("앞으로");
                    roketrb.AddForce(Vector3.forward * rforce);
                    SetReward(0.1f);
                }
            }
            if (earthdistance < 15.1f && stage == 2)//귀환완료
            {
                Debug.Log("귀환!");
                SetReward(10f);
                Done();
                success++;
                count++;
                result = "success";
            }
            if (start == 1&&stage==0)
            {
                if(Mathf.FloorToInt(timer)==0)
                {
                    transform.position = new Vector3(earth.transform.position.x, earth.transform.position.y + 5, earth.transform.position.z);
                    transform.localEulerAngles = new Vector3(90, 0, 0);
                    roketrb.velocity = Vector3.zero;
                    roketrb.angularVelocity = Vector3.zero;
                    stage = 1;
                }
                if(timer>0)
                {
                    timer = timer - Time.deltaTime;
                }
                
            }
            if(stage!=0)
            {
                sec = sec + Time.deltaTime;
                if(Mathf.FloorToInt(sec) == 60)
                {
                    sec = 0;
                    min++;
                }
            }
            //5번반복문
            if (count == 1)
            {
                FileStream fs = new FileStream(m_strPath, FileMode.Append);//create는 덮어쓰기

                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(addsec);
                sw.WriteLine(result);
                sw.WriteLine(min);
                sw.WriteLine(sec);

                sw.Close();
                fs.Close();
                Debug.Log("돌앗당");
                addsec = addsec + 30;//학습하고자하는 delta t
                
                count = 0;
            }
            
        }
    }
    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(5, 10, 150, 120), "Set launching Time");

       

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 80, 20), "Set Miniutes"))
        {
            min++;
        }

        // Make the second button.
        if (GUI.Button(new Rect(20, 70, 80, 20), "Set Seconds"))
        {
            sec++;
        }
        if (GUI.Button(new Rect(100, 50, 50, 20), "print"))
        {
            printdata = 1;
        }
        GUI.Label(new Rect(10, 100, 10, 5),timer.ToString() + "초 남았습니다.");
        GUI.Label(new Rect(10, 110, 10, 5), min.ToString()+"분"+sec.ToString()+"초");
        GUI.Label(new Rect(10, 120, 80, 20), result);
        

    }
    public override void AgentReset()
    {
        if (brain.brainType == BrainType.External)//학습중일때
        {
            timer = timer + addsec;
            min = 0;
            sec = 0;
        }
        if (initstate == 1)
        {
            mars.transform.position = mspos;
            mars.transform.localEulerAngles = msang;
            marsrb.velocity = msvel;
            marsrb.angularVelocity = msangvel;

            earth.transform.position = earthpos;
            earth.transform.localEulerAngles = earthang;
            earthrb.velocity = earthvel;
            earthrb.angularVelocity = earthangvel;
            /*
            moon.transform.position = mnpos;
            moon.transform.localEulerAngles = mnang;
            moonrb.velocity = mnvel;
            moonrb.angularVelocity = mnangvel;
            */
            moonrev.transform.position = moonrevpos;
            moonrev.transform.localEulerAngles = moonrevang;
            moonrevrb.velocity = moonrevvel;
            moonrevrb.angularVelocity = moonrevangvel;
            
        }
        stage = 0;
        transform.position = new Vector3(earth.transform.position.x, earth.transform.position.y + 5, earth.transform.position.z);
        transform.localEulerAngles = new Vector3(90, 0, 0);
        roketrb.velocity = Vector3.zero;
        roketrb.angularVelocity = Vector3.zero;
        result = "";
    }
}