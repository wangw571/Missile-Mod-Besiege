using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;

namespace Blocks
{
    public class LaserAimMissileMod : BlockMod
    {
        public override Version Version { get { return new Version("2.0"); } }
        public override string Name { get { return "LaserAimMissileMod :Advanced Aiming(No rotating limit)"; } }
        public override string DisplayName { get { return "Laser-Aim Missile Mod"; } }
        public override string BesiegeVersion { get { return "v0.11"; } }
        public override string Author { get { return "覅是"; } }
        protected Block missile = new Block()
                .ID(509)
                .TextureFile("outUV1.png")
                .BlockName("Laser-Aim Missile")
                .Obj(new List<Obj> { new Obj("k191.obj", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })
                .Scripts(new Type[] { typeof(missile) })
                .Properties(new BlockProperties().Key1("Boom", "k").Key2("Launch", "x")
                                                 .Burnable(10)
                                                 .CanBeDamaged(3)
                                                 .ToggleModeEnabled("Disable Smart Attack", false)
                                                 .Slider("Delay for detecting impact and target",0,5,0)
                                                 )
                .Mass(0.5f)
                .IconOffset(new Icon(1f, new Vector3(0f, 0f, 0f), new Vector3(-90f, 45f, 0f)))//第一个float是图标缩放，五六七是我找的比较好的角度
                .ShowCollider(false)
                .AddingPoints(new List<AddingPoint> {new BasePoint(true,false)})
                .CompoundCollider(new List<ColliderComposite>{ /*new ColliderComposite (0.5f, 1f, 0, new Vector3(0, 0, 0.7f), new Vector3(0, 0, 0)),*/new ColliderComposite(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)) })
                .NeededResources(new List<NeededResource> { new NeededResource(ResourceType.Audio, "missleLaunch.ogg") }//需要的资源，例如音乐

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
        public float svplus;
        private Vector3 targetPoint;
        private Vector3 tempTargetPoint;
        private GameObject currentAimingTarget;
        private bool 发射 = false;
        private float launchtime;
        private bool 炸;
        private Vector3 diff;
        private string key1;
        private string key2;
        private float sliderValve;
        private bool 转为俯冲姿态;
        private bool 检测错过;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null
        private AudioSource Audio;

        protected override void OnSimulateStart()
        {
            sv = 1;
            svplus = 1;
            弹道高度 = 0;
            炸 = false;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            sliderValve = this.GetComponent<MyBlockInfo>().sliderValue;
            转为俯冲姿态 = false;
            mode = 4;
            检测错过 = false;
            currentAimingTarget = null;
            tempTargetPoint = Vector3.zero;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Resources/missleLaunch.ogg").audioClip;
            Audio.loop = false;
            Audio.volume = 200;
        }
        protected override void OnSimulateFixedUpdate()
        {
            currentAimingTarget = LaserAim.currentAimingTarget;
            Debug.Log(currentAimingTarget);
                targetPoint = LaserAim.targetPoint;
            if (AddPiece.isSimulating)
            {
                if (发射 == false)
                {
                    if (Input.GetKey(key2))
                    {
                        if (currentAimingTarget != null)
                        {
                            发射 = true;
                            Audio.Play();
                        }
                        else { 发射 = false; }
                    }
                }
                if (发射 == true && 炸 == false)
                {
                    if (Input.GetKey(key1))
                    {
                        炸 = true;
                    }
                    launchtime += Time.fixedDeltaTime;
                    this.GetComponent<FireTag>().Ignite();//点火
                    rigidbody.AddForce(transform.forward * sv);
                    sv += svplus;
                    svplus *= 0.99f;
                    if (sv > 100) { sv = 100; }
                    diff = (targetPoint - this.transform.position);
                    if (launchtime > sliderValve)//Targeting
                    {
                        if (mode == 4)
                        {//运行模式判断
                            if (targetPoint.y > 10) { mode = 1; Debug.Log(mode + "Mode"); }
                            else if (Math.Abs(currentAimingTarget.rigidbody.velocity.x) + Math.Abs(currentAimingTarget.rigidbody.velocity.y) + Math.Abs(currentAimingTarget.rigidbody.velocity.z) > 15) { mode = 3; Debug.Log(mode + "Mode"); }
                            else if (-diff.y < 10 && targetPoint.y > 3) { mode = 2; Debug.Log(mode + "Mode"); }
                            else { mode = 0; Debug.Log(mode + "Mode"); }
                            if (this.GetComponent<MyBlockInfo>().toggleModeEnabled) { mode = 0; Debug.Log(mode + "Mode"); }
                        }//稍稍修改一下就好
                        

                        if (mode == 1)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 30) { 转为俯冲姿态 = true; }
                                this.rigidbody.AddForce(new Vector3(0, -(this.transform.position.y - 弹道高度), 0));
                                this.transform.LookAt(targetPoint);
                            }
                            else if (!转为俯冲姿态)
                            {
                                弹道高度 = 10;
                                int i = 0;
                                do
                                {
                                    RaycastHit hitt3;
                                    Ray 检测ray = new Ray(new Vector3(this.transform.position.x, 弹道高度, this.transform.position.z), transform.InverseTransformDirection(targetPoint));
                                    if (Physics.Raycast(检测ray, out hitt3, Mathf.Infinity))
                                    {
                                        if (hitt3.transform.gameObject != currentAimingTarget) { 弹道高度 += 1; Debug.Log("higher!"); continue; }
                                    }
                                    i++;
                                    if (i >= 100) { break; }
                                } while (弹道高度 == 0);
                            }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 13) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y > 7) { mode = 2; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//低高度模式


                        else if (mode == 2)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 10) { 转为俯冲姿态 = true; }
                                this.rigidbody.AddForce(new Vector3(0, -(this.transform.position.y - 弹道高度), 0));
                                this.transform.LookAt(targetPoint);
                            }
                            else if (!转为俯冲姿态) { 弹道高度 = this.transform.position.y; if (this.transform.position.y < 10) { 弹道高度 += 9; } }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 13) { 检测错过 = true; }

                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y < -7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//终点俯冲模式


                        else if (mode == 3)
                        {

                            Vector3 hitPoint = Vector3.zero;//存放命中点坐标
                                                            //假设飞机物体是aircraft,炮塔物体是gun 两者间的方向向量就是两种世界坐标相减
                            Vector3 D = this.transform.position - targetPoint;
                            //用飞机transform的TransformDirection方法把前进方向变换到世界坐标，就是飞机飞行的世界方向向量了
                            Vector3 targetDirection = currentAimingTarget.transform.TransformDirection(Vector3.forward);
                            //再用Vector3.Angle方法求出与飞机前进方向之间的夹角
                            float THETA = Vector3.Angle(D, targetDirection);
                            Vector3 导弹速度 = this.rigidbody.velocity;
                            Vector3 目标速度 = currentAimingTarget.rigidbody.velocity;
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
                                hitPoint = targetPoint + targetDirection * F1;
                            }//一大堆式子
                            if (!转为俯冲姿态)//保持飞行
                            {
                                this.transform.LookAt(hitPoint);
                                this.rigidbody.AddForce(new Vector3(0, (this.transform.position.y - 弹道高度) / 20, 0));
                                弹道高度 = hitPoint.y + 3;
                            }
                            else
                            {
                                this.transform.LookAt(hitPoint);
                                if (Vector3.Distance(transform.position, hitPoint) <= 3) { 炸 = true; }
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 13) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; }//当超过目标太远，重设姿态

                        }//跟随+预瞄模式

                        else if (mode == 0)
                        {//攻顶模式
                            if (转为俯冲姿态 == true)
                            {
                                //this.transform.position.RotateTowards(this.transform.position,targetPoint,2f,Mathf.Infinity);
                                this.transform.LookAt(targetPoint);
                                sv = 95;
                                if (diff.y > 10) { 转为俯冲姿态 = false; }
                            }
                            else if (Vector3.Distance(transform.position, targetPoint) >= 5)
                            {
                                this.transform.LookAt(new Vector3(targetPoint.x, targetPoint.y + 100, targetPoint.z));
                                if (sv > 55) { sv = 55; }
                            }//爬升


                            if (!转为俯冲姿态 && this.transform.position.y > targetPoint.y + 100)
                            {
                                转为俯冲姿态 = true;
                                this.rigidbody.velocity /= 5;// = new Vector3(this.rigidbody.velocity.x, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
                            }
                        }
                    }


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
                    component.GetComponent<ExplodeOnCollide>().radius = 12f;
                    component.GetComponent<FireTag>().Ignite();
                    UnityEngine.Object.DestroyImmediate(this.gameObject);
                }
            }
            else {}
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
            currentAimingTarget = null;
            发射 = false;
        }
    }


    }





