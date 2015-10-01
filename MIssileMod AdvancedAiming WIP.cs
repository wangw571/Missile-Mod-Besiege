using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;

namespace Blocks
{
    public class MissileMod : BlockMod
    {
        public override Version Version { get { return new Version("1.0"); } }
        public override string Name { get { return "MissileMod"; } }
        public override string DisplayName { get { return "Missile Mod"; } }
        public override string BesiegeVersion { get { return "v0.11"; } }
        public override string Author { get { return "覅是"; } }
        protected Block missile = new Block()
                .ID(503)
                .TextureFile("outUV1.png")
                .BlockName("Missile")
                .Obj(new List<Obj> { new Obj("k191.obj", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })
                .Scripts(new Type[] { typeof(missile) })
                .Properties(new BlockProperties().Key1("Aiming", "t").Key2("Launch", "x")
                                                 .Burnable(10)
                                                 .CanBeDamaged(3)
                                                 .ToggleModeEnabled("Smart Attack", true)
                                                 .Slider("Delay for detecting impact and target",0,5,0)
                                                 )
                .Mass(0.5f)
                .IconOffset(new Icon(1f, new Vector3(0f, 0f, 0f), new Vector3(-90f, 45f, 0f)))//第一个float是图标缩放，五六七是我找的比较好的角度
                .ShowCollider(false)
                .AddingPoints(new List<AddingPoint> {new BasePoint(true,false)})
                .CompoundCollider(new List<ColliderComposite>{ /*new ColliderComposite (0.5f, 1f, 0, new Vector3(0, 0, 0.7f), new Vector3(0, 0, 0)),*/new ColliderComposite(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)) })
                .NeededResources(new List<NeededResource>()//需要的资源，例如音乐
                
            );
        public override void OnLoad()
        {
            LoadFancyBlock(missile);//加载该模块
        }
        public override void OnUnload() { }
    }


    public class missile : BlockScript
    {
        public float sv;
        private RaycastHit hitt;
        private RaycastHit hitt2;
        private GameObject currentTarget;
        private Vector3 lookhere;
        private float yC;//yChange
        private float xC;
        private float zC;
        private bool 发射 = false;
        private float launchtime;
        private bool 炸;
        private Vector3 diff;
        private bool uped = false;
        private string key1;
        private string key2;
        private float sliderValve;
        private bool 转为俯冲姿态;
        private float xCC = 0;
        private float yCC = 0;
        private float zCC = 0;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null



        protected override void OnSimulateStart()
        {
            sv = 0;
            弹道高度 = 0;
            炸 = false;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            sliderValve = this.GetComponent<MyBlockInfo>().sliderValue;
            转为俯冲姿态 = false;
            mode = 4;
        }
        protected override void OnSimulateFixedUpdate()
        {
            float angle = 0;
            float myrts = this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg;
            if (myrts >= 180) { myrts = myrts - 360; }
            //Debug.Log(Physics.gravity); 32.8
            Debug.Log(mode + "Mode");
            Debug.Log(myrts);
            /*ExplosionEffect[] vector3E = UnityEngine.Object.FindObjectsOfType<ExplosionEffect>();
            for (int i = 0; i < (int)vector3E.Length; i++)
            {
                vector3E[i].maxSize = new Vector3(70f, 70f, 70f);
            }*/

            //Debug.Log(myrts /*Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg*/ /*(this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg)*/+ "myD");
            if (AddPiece.isSimulating)
            {
                if (Input.GetKey(key1))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                    {
                        currentTarget = hitt.transform.gameObject;
                        Debug.Log("Target Acquired!");
                        Debug.Log(Vector3.Angle(new Vector3(currentTarget.transform.position.x, this.transform.position.x, currentTarget.transform.position.z), currentTarget.transform.position).ToString());
                    }
                }
                if (发射 == false)
                {

                    if (Input.GetKey(key2))
                    {
                        if (currentTarget != null)
                        {
                            发射 = true;
                            Ray 阻碍检测ray = new Ray(this.transform.position, this.transform.forward);
                            if (Physics.Raycast(阻碍检测ray, out hitt2, Mathf.Infinity))
                            { if (hitt2.transform.gameObject != currentTarget ^ !this.GetComponent<MyBlockInfo>().toggleModeEnabled)
                                {
                                    mode = 0;
                                }
                            }
                        }
                        else { 发射 = false; }
                    }
                }//按键
                    if (发射 == true && 炸 == false)
                    {
                    Debug.Log(Vector3.Angle(new Vector3(currentTarget.transform.position.x, this.transform.position.x, currentTarget.transform.position.z), currentTarget.transform.position).ToString());

                    launchtime += Time.fixedDeltaTime;
                        this.GetComponent<FireTag>().Ignite();//点火
                        rigidbody.AddForce(transform.forward * sv);
                        sv += 1;
                    if (sv > 200) { sv = 200; }
                        diff = (currentTarget.transform.position - this.transform.position);
                        //Debug.Log(Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg);
                        angle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg/* - myrts*/);


                        if (launchtime > sliderValve)//Targeting
                        {
                    if (mode == 4) {//运行模式判断
                        if (currentTarget.transform.position.y > 5) { mode = 1; Debug.Log(mode + "Mode"); }
                        else if (Math.Abs(currentTarget.rigidbody.velocity.x) + Math.Abs(currentTarget.rigidbody.velocity.y) + Math.Abs(currentTarget.rigidbody.velocity.z) > 15) { mode = 3; Debug.Log(mode + "Mode"); }
                        else if (currentTarget.transform.position.y < 5) { mode = 2; Debug.Log(mode + "Mode"); }
                        else { mode = 0; }
                    }


                    if (mode == 1) {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 10) { 转为俯冲姿态 = true; }
                                yC = 弹道高度 - this.transform.position.y;
                                //角度计算
                                if (Math.Abs(myrts) > 90)
                                {
                                    if (Math.Abs(angle - myrts) < 2)//正负问题
                                    {
                                        xC = 10;
                                        zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                    }
                                    else
                                    {
                                        xC = 10;
                                        zC = (float)Math.Tan(2 + myrts) * xC;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(angle - myrts) < 2)
                                    {
                                        xC = -10;
                                        zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                    }
                                    else
                                    {
                                        xC = -10;
                                        zC = (float)Math.Tan(15 + myrts) * xC;
                                    }
                                }//计算完毕
                                if (this.rigidbody.velocity.y < -2) { sv += 2; }
                                else if (this.rigidbody.velocity.y > 7) { sv -= 1; }
                            }
                            else if (!转为俯冲姿态) { 弹道高度 = 5;/*尝试每隔1y就检测有没有障碍物*/ }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                yC = diff.y;
                                //角度计算
                                if (Math.Abs(myrts) > 90)
                                {
                                    if (Math.Abs(angle - myrts) < 2)//正负问题
                                    {
                                        xC = 10;
                                        zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                    }
                                    else
                                    {
                                        xC = 10;
                                        zC = (float)Math.Tan(2 + myrts) * xC;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(angle - myrts) < 2)
                                    {
                                        xC = -10;
                                        zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                    }
                                    else
                                    {
                                        xC = -10;
                                        zC = (float)Math.Tan(15 + myrts) * xC;
                                    }
                                }//计算完毕
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15) { 转为俯冲姿态 = false; if (diff.y > 7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//低高度模式


                    else if (mode == 2)
                    { if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                        {
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 10) { 转为俯冲姿态 = true; }
                            yC = 弹道高度 - this.transform.position.y;
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 2)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(2 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 2)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }//计算完毕
                            if (this.rigidbody.velocity.y < -2) { sv += 2; }
                            else if (this.rigidbody.velocity.y > 7) { sv -= 1; }
                        }
                        else if (!转为俯冲姿态) { 弹道高度 = this.transform.position.y; }//当没有在俯冲而且弹道高度=0的时候
                        else {yC = diff.y;
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 2)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(2 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 2)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }//计算完毕
                        }//开始俯冲，将目光放在目标上
                        if(Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15) { 转为俯冲姿态 = false; if (diff.y > 7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                    }//终点俯冲模式


                    else if (mode == 3) {

                        Vector3 hitPoint = Vector3.zero;//存放命中点坐标
                                                        //假设飞机物体是aircraft,炮塔物体是gun 两者间的方向向量就是两种世界坐标相减
                        Vector3 D = this.transform.position - currentTarget.transform.position;
                        //用飞机transform的TransformDirection方法把前进方向变换到世界坐标，就是飞机飞行的世界方向向量了
                        Vector3 targetDirection = currentTarget.transform.TransformDirection(Vector3.forward);
                        //再用Vector3.Angle方法求出与飞机前进方向之间的夹角
                        float THETA = Vector3.Angle(D, targetDirection);
                        Vector3 导弹速度 = this.rigidbody.velocity;
                        Vector3 目标速度 = currentTarget.rigidbody.velocity;
                        float DD = D.magnitude;//D是飞机炮塔间方向向量，D的magnitued就是两种间距离
                        float A = 1 - Mathf.Pow((float)(Math.Sqrt(导弹速度.x * 导弹速度.x + 导弹速度.z * 导弹速度.z) / Math.Sqrt(目标速度.x * 目标速度.x + 目标速度.z * 目标速度.z)), 2);//假设炮弹的速度是gunVeloctiy飞机的飞行线速度是aircraftVeloctiy
                        float B = -(2 * DD * Mathf.Cos(THETA * Mathf.Deg2Rad));//要变换成弧度
                        float C = DD * DD;
                        float DELTA = B * B - 4 * A * C;
                        if (DELTA >= 0)
                        {//如果DELTA小于0，无解
                            float F1 = (-B + Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                            float F2 = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                            if (F1 < F2)//取较小的一个
                                F1 = F2;
                            //命中点位置等于 飞机初始位置加上计算出F边长度乘以飞机前进的方向向量，这个乘法等于把前进的距离变换成世界坐标的位移
                            hitPoint = currentTarget.transform.position + targetDirection * F1;
                        }//一大堆式子
                        diff = (hitPoint - this.transform.position);
                        angle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg/* - myrts*/);
                        if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                        {
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 10) { 转为俯冲姿态 = true; }
                            yC = 弹道高度 - this.transform.position.y;
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 2)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(2 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 2)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }//计算完毕
                            if (this.rigidbody.velocity.y < -2) { sv += 2; }
                            else if (this.rigidbody.velocity.y > 7) { sv -= 1; }
                        }
                        else if (!转为俯冲姿态) { 弹道高度 = hitPoint.y; }//当没有在俯冲而且弹道高度=0的时候
                        else
                        {
                            yC = -diff.y;
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 2)//正负问题
                                {
                                    xC = yC;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = yCC;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 2)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }//计算完毕
                        }//开始俯冲，将目光放在目标上
                        if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15) { 转为俯冲姿态 = false; if (diff.y > 7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态

                    }//跟随+预瞄模式

                    else if (mode == 0)
                    {//攻顶模式
                        if (转为俯冲姿态 == true)
                        {
                            yC -= yCC;
                            yCC *= 1.2f;
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 2)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(2 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 2)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 2) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }//计算完毕
                        }
                        else if (Vector3.Distance(transform.position, currentTarget.transform.position) >= 5)
                        {
                            this.transform.LookAt(new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 100, currentTarget.transform.position.z));
                            //角度计算
                            if (Math.Abs(myrts) > 90)
                            {
                                if (Math.Abs(angle - myrts) < 5)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle + myrts / 5) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(Math.Abs(angle) / angle * 15 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (Math.Abs(angle - myrts) < 5)
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(angle + myrts / 5) * xC;
                                }
                                else
                                {
                                    xC = -10;
                                    zC = (float)Math.Tan(Math.Abs(angle) / angle * 15 + myrts) * xC;
                                }
                            }//计算完毕
                            if (this.rigidbody.velocity.y < 12 && this.transform.position.y < currentTarget.transform.position.y + 100) { sv += 2; }
                        }//爬升


                        if (!转为俯冲姿态 && Math.Abs(this.transform.position.y - currentTarget.transform.position.y + 100) < 5)
                        {
                            转为俯冲姿态 = true;
                            yCC = 1;
                        }
                    }

                            if (this.transform.position.y - currentTarget.transform.position.y > 20)
                                lookhere = this.transform.position;
                            lookhere.y += yC;
                            lookhere.x += xC;
                            lookhere.z += zC;
                            this.transform.LookAt(lookhere);
                        }
                            if (this.transform.rigidbody.velocity.y < -3) { sv += 2; }
                            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= 3) { 炸 = true; }
                        




                        if (launchtime > 10) { 炸 = true; }
                    }
                    if (this.IsBurning() && 发射 == false)
                    {
                        炸 = true;
                    }

                    if (炸 == true)
                    {
                        //      Debug.Log("boom!");
                        GameObject component = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Object.FindObjectOfType<AddPiece>().blockTypes[23].gameObject);
                        component.transform.position = this.transform.position;
                        UnityEngine.Object.Destroy(component.GetComponent<Rigidbody>());
                        component.AddComponent<Rigidbody>();
                        component.gameObject.AddComponent<KillIfEditMode>();
                        component.GetComponent<ExplodeOnCollide>().radius = 10f;
                        component.GetComponent<FireTag>().Ignite();
                        UnityEngine.Object.DestroyImmediate(this.gameObject);
                    }
                }
                else { }
                //Physics stuff
            
        }
        void OnCollisionEnter(Collision collision)
        {
            if (发射 == true && launchtime > 0.2)
            {
                炸 = true;
            }
        }
        protected override void OnSimulateExit()
        {
            currentTarget = null;
            发射 = false;
        }
    }


    }
