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
                                                 .ToggleModeEnabled("DO NOT Attack from top", false)
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



        protected override void OnSimulateStart()
        {
            sv = 0;
            炸 = false;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            sliderValve = this.GetComponent<MyBlockInfo>().sliderValue;
            转为俯冲姿态 = false;
        }
        protected override void OnSimulateFixedUpdate()
        {
            float angle = 0;
            int mode = 1;//0-top attack 1-low height high target  2- high height low target 3-follow y
            float myrts = this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg;
            if (myrts >= 180) { myrts = myrts - 360; }


            //Debug.Log(myrts /*Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg*/ /*(this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg)*/+ "myD");
            if (AddPiece.isSimulating)
            {
                if (Input.GetKey(key1))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                    {
                        currentTarget = hitt.transform.gameObject;
                        Debug.Log("Target Acquired!");
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
                            if ((Physics.Raycast(阻碍检测ray, out hitt, Mathf.Infinity) && hitt.transform.gameObject != currentTarget) ^ !this.GetComponent<MyBlockInfo>().toggleModeEnabled)
                            {
                                mode = 0;
                            }
                        }
                        else { 发射 = false; }
                    }
                }//按键
                    if (发射 == true && 炸 == false)
                    {
                        launchtime += Time.fixedDeltaTime;
                        this.GetComponent<FireTag>().Ignite();//点火
                        rigidbody.AddForce(transform.forward * sv);
                        sv += 1;
                        diff = (currentTarget.transform.position - this.transform.position);
                        //Debug.Log(Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg);
                        angle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg/* - myrts*/);


                        if (launchtime > sliderValve)//Targeting
                        {}
                            

                            if (mode == 1) { }//低高度模式


                            else if (mode == 2) { }//终点俯冲模式


                            else if (mode == 0) {//攻顶模式
                                if (转为俯冲姿态 == true) {
                                yC -= yCC;
                                yCC *= 1.2f;
                                //角度计算
                                if (angle > 0)
                                {
                                    if (angle < 2)
                                    {
                                        this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/); 
                                    }
                                    else { this.transform.Rotate(Vector3.up * 0.5f/*, ForceMode.Acceleration*/); }
                                }
                                else
                                {
                                    if (angle > -2)
                                    {
                                        this.transform.Rotate(Vector3.up * (-0.5f * (angle / -2))/*, ForceMode.Acceleration*/);
                                    }
                                    else { /*rigidbody.AddTorque*/this.transform.Rotate(Vector3.up * -0.5f/*, ForceMode.Acceleration*/); }
                                }//计算完毕
                            }
                                else if (Vector3.Distance(transform.position, currentTarget.transform.position) >= 20)
                                {
                                    this.transform.LookAt(new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 100, currentTarget.transform.position.z));
                            //角度计算
                            if (angle - myrts > 0)
                            {
                                if (angle - myrts < 2)//正负问题
                                {
                                    xC = 10;
                                    zC = (float)Math.Tan(angle+myrts/2) * xC;    //this.transform.Rotate(Vector3.up * (0.5f * (angle / 2))/*, ForceMode.Acceleration*/);
                                }
                                else {
                                    xC = 10;
                                    zC = (float)Math.Tan(15 + myrts) * xC;
                                }
                            }
                            else
                            {
                                if (angle - myrts > -2)
                                {
                                    this.transform.Rotate(Vector3.up * (-0.5f * (angle / -2))/*, ForceMode.Acceleration*/);
                                }
                                else { /*rigidbody.AddTorque*/this.transform.Rotate(Vector3.up * -0.5f/*, ForceMode.Acceleration*/); }
                            }//计算完毕
                            if (this.rigidbody.velocity.y < 12 && this.transform.position.y < currentTarget.transform.position.y + 100) { sv += 2; }
                                }//爬升


                            if (!转为俯冲姿态 && Math.Abs(this.transform.position.y - currentTarget.transform.position.y + 100) < 5 )
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
