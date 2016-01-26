    using System;
using System.Collections;
    using System.Collections.Generic;
    using spaar.ModLoader;
    using TheGuysYouDespise;
    using UnityEngine;

namespace Blocks
{
    public class MissileMod : BlockMod
    {
        public override Version Version { get { return new Version("3.1"); } }
        public override string Name { get { return "Missile Mod"; } }
        public override string DisplayName { get { return "Missile Mod"; } }
        public override string BesiegeVersion { get { return "v0.23"; } }
        public override string Author { get { return "覅是"; } }
        protected Block missile = new Block()
                .ID(503)
                .TextureFile("NormalMissile.png")
                .BlockName("Missile")
                .Obj(new List<Obj> { new Obj("k191.obj", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })
                .Scripts(new Type[] { typeof(missile) })
                .Properties(new BlockProperties().Key1("Aiming", "t").Key2("Launch", "x")
                                                 .Burnable(3)
                                                 .ToggleModeEnabled("Disable Smart Attack", false)
                                                 .Slider("Delay for detecting impact and target", 0, 5, 0)
                                                 )
                .Mass(0.5f)
                .IconOffset(new Icon(1f, new Vector3(0f, 0f, 0f), new Vector3(-90f, 45f, 0f)))//第一个float是图标缩放，五六七是我找的比较好的角度
                .ShowCollider(false)
                .GhostCollider(new ColliderComposite(Vector3.zero, Vector3.zero, Vector3.zero))
                .AddingPoints(new List<AddingPoint> { new BasePoint(true, false) })
                .CompoundCollider(new List<ColliderComposite> { /*new ColliderComposite (0.5f, 1f, 0, new Vector3(0, 0, 0.7f), new Vector3(0, 0, 0)),*/new ColliderComposite(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)) })
                .NeededResources(new List<NeededResource> { new NeededResource(ResourceType.Audio, "missleLaunch.ogg") }//需要的资源，例如音乐

            );
        protected Block InvicipleMissile = new Block()
                    .ID(504)
                    .TextureFile("InvicipleMissile.png")
                    .BlockName("Inviciple Missile")
                    .Obj(new List<Obj> { new Obj("k191.obj", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })
                    .Scripts(new Type[] { typeof(InvicipleMissile) })
                    .Properties(new BlockProperties().Key1("Aiming", "t").Key2("Launch", "x")
                                                     .ToggleModeEnabled("Destroy everything on the way", false)
                                                     //.Slider("Speed", 0, 5, 0)
                                                     )
                    .Mass(0.5f)
                    .IconOffset(new Icon(1f, new Vector3(0f, 0f, 0f), new Vector3(-90f, 45f, 0f)))//第一个float是图标缩放，五六七是我找的比较好的角度
                    .ShowCollider(false)
                    .GhostCollider(new ColliderComposite(Vector3.zero, Vector3.zero, Vector3.zero))
                    .AddingPoints(new List<AddingPoint> { new BasePoint(true, false) })
                    .CompoundCollider(new List<ColliderComposite> { /*new ColliderComposite (0.5f, 1f, 0, new Vector3(0, 0, 0.7f), new Vector3(0, 0, 0)),*/new ColliderComposite(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)) })
                    .NeededResources(new List<NeededResource> { new NeededResource(ResourceType.Audio, "missleLaunch.ogg") }//需要的资源，例如音乐

                );
        protected Block StopMotionMissile = new Block()
                    .ID(506)
                    .TextureFile("StopMotionMissile.png")
                    .BlockName("Motion Stopper Missile")
                    .Obj(new List<Obj> { new Obj("k191.obj", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })
                    .Scripts(new Type[] { typeof(StopMotionMissileScript) })
                    .Properties(new BlockProperties().Key1("Aiming", "t").Key2("Launch", "x")
                                                     .ToggleModeEnabled("Disable Smart Attack", false)
                                                     .Slider("Delay for detecting impact and target", 0, 5, 0)
                                                     )
                    .Mass(0.5f)
                    .IconOffset(new Icon(1f, new Vector3(0f, 0f, 0f), new Vector3(-90f, 45f, 0f)))//第一个float是图标缩放，五六七是我找的比较好的角度
                    .ShowCollider(false)
                    .GhostCollider(new ColliderComposite(Vector3.zero, Vector3.zero, Vector3.zero))
                    .AddingPoints(new List<AddingPoint> { new BasePoint(true, false) })
                    .CompoundCollider(new List<ColliderComposite> { /*new ColliderComposite (0.5f, 1f, 0, new Vector3(0, 0, 0.7f), new Vector3(0, 0, 0)),*/new ColliderComposite(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)) })
                    .NeededResources(new List<NeededResource> { new NeededResource(ResourceType.Audio, "missleLaunch.ogg") }//需要的资源，例如音乐

                );
        public override void OnLoad()
        {
            LoadFancyBlock(missile);//加载该模块
            LoadFancyBlock(InvicipleMissile);
            LoadFancyBlock(StopMotionMissile);
        }
        public override void OnUnload() { }
    }


    public class missile : BlockScript
    {
        public float sv;
        public float svplus;
        private RaycastHit hitt;
        private RaycastHit hitt2;
        private GameObject currentTarget;
        private Vector3 targetPoint;
        private bool 发射 = false;
        private float launchtime;
        private bool 炸;
        private float BurningTime;
        private float TotalTime = 0;
        private Vector3 diff;
        private Vector3 difftgt;
        private string key1;
        private string key2;
        private float sliderValve;
        private bool 转为俯冲姿态;
        private bool 检测错过;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null
        private AudioSource Audio;
        private GameObject Trail;
        private bool countedBurning = false;



        protected override void OnSimulateStart()
        {
            sv = 1f;
            svplus = 1f;
            弹道高度 = 0;
            炸 = false;
            countedBurning = false;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            sliderValve = this.GetComponent<MyBlockInfo>().sliderValue;
            转为俯冲姿态 = false;
            mode = 4;
            检测错过 = false;
            currentTarget = null;

            Trail = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Trail.name = "MissileTrail";
            Destroy(Trail.GetComponent<Rigidbody>());
            Trail.transform.position = this.transform.position;
            Trail.AddComponent<TrailRenderer>();
            Trail.GetComponent<TrailRenderer>().startWidth = 0.7f;
            Trail.GetComponent<TrailRenderer>().endWidth = 1.3f;
            Trail.GetComponent<TrailRenderer>().time = 0f;
            Trail.GetComponent<TrailRenderer>().materials = Trail.GetComponent<Renderer>().materials;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);
            //Trail.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Trail.GetComponent<TrailRenderer>().autodestruct = true;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Resources/missleLaunch.ogg").audioClip;
            Audio.loop = false;
            Audio.volume = 200;
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Input.GetKey(key1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                {
                    currentTarget = hitt.transform.gameObject;
                    difftgt = currentTarget.transform.position - hitt.point;
                    targetPoint = currentTarget.transform.position - difftgt;
                    //Debug.Log("Target Acquired! ");
                }
            }
            if (发射 == false)
            {

                if (Input.GetKey(key2))
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        Trail.GetComponent<TrailRenderer>().time = 0.05f;
                        Audio.Play();
                        Ray 阻碍检测ray = new Ray(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position));
                        if (Physics.Raycast(阻碍检测ray, out hitt2, Mathf.Infinity))
                        {
                            //Debug.DrawLine(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position), Color.red);
                            if (hitt2.transform.gameObject.name != currentTarget.name ^ !this.GetComponent<MyBlockInfo>().toggleModeEnabled)
                            {
                                mode = 0;
                                //Debug.Log(hitt2.transform.name);
                            }
                        }
                    }
                    else { 发射 = false; }
                }
            }//按键
        }

        protected override void OnSimulateFixedUpdate()
        {
            TotalTime += Time.fixedDeltaTime;
            if (AddPiece.isSimulating)
            {
                if (发射 == true && 炸 == false)
                {
                    targetPoint = currentTarget.transform.position - difftgt;
                    launchtime += Time.fixedDeltaTime;
                    //this.GetComponent<FireTag>().Ignite();//点火
                    GetComponent<Rigidbody>().AddForce(transform.forward * sv);
                    sv += svplus;
                    svplus *= 0.99f;
                    if (sv > 350) { sv = 350; }
                    diff = (targetPoint - this.transform.position);
                    Trail.GetComponent<TrailRenderer>().time = 12 / this.GetComponent<Rigidbody>().velocity.magnitude;
                    if (launchtime > sliderValve)//Targeting
                    {
                        if (this.GetComponent<MyBlockInfo>().toggleModeEnabled) { mode = 0; }
                        if (mode == 4)
                        {//运行模式判断
                            if (currentTarget.GetComponent<Rigidbody>().velocity.sqrMagnitude > 25) { mode = 3; Debug.Log(mode + "Mode"); svplus = 30; }
                            else if (diff.y > 5) { mode = 1; Debug.Log(mode + "Mode"); Debug.Log(currentTarget.GetComponent<Rigidbody>().velocity.sqrMagnitude); }
                            else if (diff.y < 5) { mode = 2; Debug.Log(mode + "Mode"); }
                            else { mode = 0; Debug.Log("0Mode"); }
                        }


                        if (mode == 1)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 30) { 转为俯冲姿态 = true; }
                                //this.GetComponent<Rigidbody>().AddForce(new Vector3(0, (this.transform.position.y - 弹道高度) * -0.3f, 0),ForceMode.VelocityChange);
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else if (!转为俯冲姿态)
                            {
                                弹道高度 = 3;
                                int i = 0;
                                this.GetComponent<Rigidbody>().useGravity = true;
                                do
                                {
                                    RaycastHit hitt3;
                                    Ray 检测ray = new Ray(new Vector3(this.transform.position.x, 弹道高度, this.transform.position.z), targetPoint - new Vector3(this.transform.position.x, 弹道高度).normalized);
                                    if (Physics.Raycast(检测ray, out hitt3, Mathf.Infinity))
                                    {
                                        Debug.DrawLine(new Vector3(this.transform.position.x, 弹道高度, this.transform.position.z), targetPoint, Color.red);
                                        if (hitt3.transform.gameObject != currentTarget) { 弹道高度 += 1; Debug.Log("higher!"); continue; }
                                    }
                                    i++;
                                    if (i >= 100) { break; }
                                } while (弹道高度 == 0);
                            }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = true;
                                if (Vector3.Distance(this.transform.position, targetPoint) < 7) { 炸 = true; }
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y > 7) { mode = 2; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//低高度模式


                        else if (mode == 2)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 30) { 转为俯冲姿态 = true; }
                                //this.GetComponent<Rigidbody>().AddForce(new Vector3(0, (this.transform.position.y - 弹道高度) / 20, 0));
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else if (!转为俯冲姿态) { 弹道高度 = this.transform.position.y; if (this.transform.position.y > 5) { 弹道高度 += 5; } }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                                if (Vector3.Distance(this.transform.position, targetPoint) < 7) { 炸 = true; }
                                this.GetComponent<Rigidbody>().useGravity = true;
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y < -7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//终点俯冲模式


                        else if (mode == 3)
                        {

                            Vector3 hitPoint = targetPoint;//存放命中点坐标
                                                           //假设飞机物体是aircraft,炮塔物体是gun 两者间的方向向量就是两种世界坐标相减
                            Vector3 D = this.transform.position - targetPoint;
                            //用飞机transform的TransformDirection方法把前进方向变换到世界坐标，就是飞机飞行的世界方向向量了
                            Vector3 targetDirection = currentTarget.transform.TransformDirection(Vector3.forward);
                            //再用Vector3.Angle方法求出与飞机前进方向之间的夹角
                            float THETA = Vector3.Angle(D, targetDirection);
                            Vector3 导弹速度 = this.GetComponent<Rigidbody>().velocity;
                            Vector3 目标速度 = currentTarget.GetComponent<Rigidbody>().velocity;
                            float DD = D.magnitude;//D是飞机炮塔间方向向量，D的magnitued就是两种间距离
                            float A = 1 - Mathf.Pow((float)(Math.Sqrt(导弹速度.x * 导弹速度.x + 导弹速度.z * 导弹速度.z) / Math.Sqrt(目标速度.x * 目标速度.x + 目标速度.z * 目标速度.z)), 2);//假设炮弹的速度是gunVeloctiy飞机的飞行线速度是aircraftVeloctiy
                            float B = -(2 * DD * Mathf.Cos(THETA * Mathf.Deg2Rad));//要变换成弧度
                            float C = DD * DD;
                            float DELTA = B * B - 4 * A * C;
                            if (DELTA >= 0)
                            {//如果DELTA小于0，无解
                                float F1 = (-B + Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                                float F2 = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                                if ((F1 > F2 && F2 > 0) ^ F1 < 0)
                                {//取较小的一个正数
                                    F1 = F2;
                                }
                                //命中点位置等于 飞机初始位置加上计算出F边长度乘以飞机前进的方向向量，这个乘法等于把前进的距离变换成世界坐标的位移
                                hitPoint = targetPoint + currentTarget.GetComponent<Rigidbody>().velocity.normalized * F1;
                            }//一大堆式子
                            else { hitPoint = targetPoint; }
                            if (!转为俯冲姿态)//保持飞行,关闭重力影响
                            {
                                this.transform.LookAt(hitPoint);
                                this.GetComponent<Rigidbody>().velocity = diff.normalized * sv;
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else
                            {
                                if (diff.sqrMagnitude < 50) { 炸 = true; }
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上，使用重力影响
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; }//当超过目标太远，重设姿态

                        }//跟随+预瞄模式

                        else if (mode == 0)
                        {//攻顶模式
                            if (转为俯冲姿态 == true)
                            {
                                //this.transform.position.RotateTowards(this.transform.position,targetPoint,2f,Mathf.Infinity);
                                this.transform.LookAt(targetPoint);
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
                                this.GetComponent<Rigidbody>().velocity /= 5;// = new Vector3(this.GetComponent<Rigidbody>().velocity.x, this.GetComponent<Rigidbody>().velocity.y, this.GetComponent<Rigidbody>().velocity.z);
                            }
                        }
                    }
                    if (Vector3.Distance(transform.position, targetPoint) <= 7f)
                    {
                        this.transform.LookAt(targetPoint);
                        this.GetComponent<Rigidbody>().AddForce(diff * sv, ForceMode.VelocityChange);
                        Debug.Log("Keeping Range!");
                    }
                    if (Vector3.Distance(transform.position, targetPoint) <= 0.3f)
                    {
                        炸 = true;
                    }





                    if (launchtime > 10) { 炸 = true; }
                }
                if (this.IsBurning() && !countedBurning)
                {
                    BurningTime = TotalTime;
                    countedBurning = true;
                }
                if (BurningTime + 1 < TotalTime && IsBurning()) { 炸 = true; }

                if (炸 == true)
                {
                    GameObject component = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Object.FindObjectOfType<AddPiece>().blockTypes[23].gameObject);
                    component.transform.position = this.transform.position;
                    UnityEngine.Object.Destroy(component.GetComponent<Rigidbody>());
                    //component.AddComponent<Rigidbody>();
                    component.gameObject.AddComponent<KillIfEditMode>();
                    component.GetComponent<ExplodeOnCollide>().radius = 15f;
                    component.GetComponent<FireTag>().Ignite();
                    foreach (GameObject BombExplosion in FindObjectsOfType<GameObject>())
                    {
                        if (BombExplosion.name == "BombExplosion(Clone)")
                        {
                            BombExplosion.transform.localScale = Vector3.one;
                        }
                    }
                    DestroyImmediate(this.gameObject);
                    //Destroy(Trail);
                }
            }
            //Physics stuff

        }
        void OnCollisionEnter(Collision collision)
        {
            if (发射 == true && launchtime > 0.2 && collision.gameObject.name != "MissileTrail")
            {
                炸 = true;
            }
        }
    }


    public class InvicipleMissile : BlockScript
    {
        public float sv;
        public float svplus;
        private bool toggleEnabled;
        private RaycastHit hitt;
        private GameObject currentTarget;
        private bool 发射 = false;
        private float launchtime;
        private float TotalTime = 0;
        private Vector3 diff;
        private string key1;
        private string key2;
        private AudioSource Audio;
        private GameObject Trail;



        protected override void OnSimulateStart()
        {
            sv = 1f;
            svplus = 1.13f;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            toggleEnabled = this.GetComponent<MyBlockInfo>().toggleModeEnabled;
            currentTarget = null;

            Trail = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Trail.name = "MissileTrail";
            Destroy(Trail.GetComponent<Rigidbody>());
            Destroy(Trail.GetComponent<Collider>());
            Trail.transform.position = this.transform.position;
            Trail.AddComponent<TrailRenderer>();
            Trail.GetComponent<TrailRenderer>().startWidth = 0.7f;
            Trail.GetComponent<TrailRenderer>().endWidth = 1.3f;
            Trail.GetComponent<TrailRenderer>().time = 0f;
            Trail.GetComponent<TrailRenderer>().materials = Trail.GetComponent<Renderer>().materials;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);
            //Trail.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Trail.GetComponent<TrailRenderer>().autodestruct = true;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Resources/missleLaunch.ogg").audioClip;
            Audio.loop = false;
            Audio.volume = 200;
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Input.GetKey(key1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                {
                    currentTarget = hitt.transform.gameObject;
                    //Debug.Log("Target Acquired! ");
                }
            }
            if (发射 == false)
            {

                if (Input.GetKey(key2))
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        this.GetComponent<Rigidbody>().useGravity = false;
                        Audio.Play();
                    }
                    else { 发射 = false; }
                }
            }//按键
        }

        protected override void OnSimulateFixedUpdate()
        {
            TotalTime += Time.fixedDeltaTime;
            if (AddPiece.isSimulating)
            {
                if (发射 == true)
                {
                    launchtime += Time.fixedDeltaTime;
                    //this.GetComponent<FireTag>().Ignite();//点火
                    try {
                        this.transform.LookAt(currentTarget.transform.position);
                    }
                    catch{ Destroy(this); }
                        diff = (currentTarget.transform.position - this.transform.position);
                    this.GetComponent<Rigidbody>().AddForce((diff.normalized * diff.magnitude * 4 + diff.normalized * currentTarget.GetComponent<Rigidbody>().velocity.magnitude)/20,ForceMode.VelocityChange);
                    sv *= svplus;
                    svplus *= 0.99999f;
                    if (sv > 500) { sv = 500; }
                    Trail.GetComponent<TrailRenderer>().time = 12 / this.GetComponent<Rigidbody>().velocity.magnitude;
                    if (!currentTarget) { Destroy(this); }
                }
            }
            //Physics stuff

        }
        void OnCollisionEnter(Collision collision)
        {
            if (toggleEnabled)
            {
                if (发射 == true && launchtime > 0.2 && collision.gameObject.name != "MissileTrail")
                {
                    抹除(collision.gameObject);
                }
            }
            else
            {
                if (发射 == true && launchtime > 0.2 && collision.gameObject == currentTarget)
                {
                    抹除(collision.gameObject);
                }
            }
        }
        void 抹除(GameObject 目标物体)
        {
            if (currentTarget == 目标物体 ^ currentTarget == null)
            {
                Destroy(this.gameObject);
            }
                GameObject.Destroy(目标物体);
        }
    }

    public class StopMotionMissileScript : BlockScript
    {
        public float sv;
        public float svplus;
        private RaycastHit hitt;
        private RaycastHit hitt2;
        private GameObject currentTarget;
        private Vector3 targetPoint;
        private bool 发射 = false;
        private float launchtime;
        private bool 炸;
        private float BurningTime;
        private float TotalTime = 0;
        private Vector3 diff;
        private Vector3 difftgt;
        private string key1;
        private string key2;
        private float sliderValve;
        private bool 转为俯冲姿态;
        private bool 检测错过;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null
        private AudioSource Audio;
        private GameObject Trail;
        private bool countedBurning = false;



        protected override void OnSimulateStart()
        {
            sv = 1f;
            svplus = 1f;
            弹道高度 = 0;
            炸 = false;
            countedBurning = false;
            key1 = this.GetComponent<MyBlockInfo>().key1;
            key2 = this.GetComponent<MyBlockInfo>().key2;
            sliderValve = this.GetComponent<MyBlockInfo>().sliderValue;
            转为俯冲姿态 = false;
            mode = 4;
            检测错过 = false;
            currentTarget = null;

            Trail = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Trail.name = "MissileTrail";
            Destroy(Trail.GetComponent<Rigidbody>());
            Trail.transform.position = this.transform.position;
            Trail.AddComponent<TrailRenderer>();
            Trail.GetComponent<TrailRenderer>().startWidth = 0.7f;
            Trail.GetComponent<TrailRenderer>().endWidth = 1.3f;
            Trail.GetComponent<TrailRenderer>().time = 0f;
            Trail.GetComponent<TrailRenderer>().materials = Trail.GetComponent<Renderer>().materials;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);
            //Trail.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Trail.GetComponent<TrailRenderer>().autodestruct = true;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Resources/missleLaunch.ogg").audioClip;
            Audio.loop = false;
            Audio.volume = 200;
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Input.GetKey(key1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                {
                    currentTarget = hitt.transform.gameObject;
                    difftgt = currentTarget.transform.position - hitt.point;
                    targetPoint = currentTarget.transform.position - difftgt;
                    //Debug.Log("Target Acquired! ");
                }
            }
            if (发射 == false)
            {

                if (Input.GetKey(key2))
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        Trail.GetComponent<TrailRenderer>().time = 0.05f;
                        Audio.Play();
                        Ray 阻碍检测ray = new Ray(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position));
                        if (Physics.Raycast(阻碍检测ray, out hitt2, Mathf.Infinity))
                        {
                            //Debug.DrawLine(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position), Color.red);
                            if (hitt2.transform.gameObject.name != currentTarget.name ^ !this.GetComponent<MyBlockInfo>().toggleModeEnabled)
                            {
                                mode = 0;
                                //Debug.Log(hitt2.transform.name);
                            }
                        }
                    }
                    else { 发射 = false; }
                }
            }//按键
        }

        protected override void OnSimulateFixedUpdate()
        {
            TotalTime += Time.fixedDeltaTime;
            if (AddPiece.isSimulating)
            {
                if (发射 == true && 炸 == false)
                {
                    targetPoint = currentTarget.transform.position - difftgt;
                    launchtime += Time.fixedDeltaTime;
                    //this.GetComponent<FireTag>().Ignite();//点火
                    GetComponent<Rigidbody>().AddForce(transform.forward * sv);
                    sv += svplus;
                    svplus *= 0.99f;
                    if (sv > 350) { sv = 350; }
                    diff = (targetPoint - this.transform.position);
                    Trail.GetComponent<TrailRenderer>().time = 12 / this.GetComponent<Rigidbody>().velocity.magnitude;
                    if (launchtime > sliderValve)//Targeting
                    {
                        if (this.GetComponent<MyBlockInfo>().toggleModeEnabled) { mode = 0; }
                        if (mode == 4)
                        {//运行模式判断
                            if (currentTarget.GetComponent<Rigidbody>().velocity.sqrMagnitude > 25) { mode = 3; Debug.Log(mode + "Mode"); svplus = 30; }
                            else if (diff.y > 5) { mode = 1; Debug.Log(mode + "Mode"); Debug.Log(currentTarget.GetComponent<Rigidbody>().velocity.sqrMagnitude); }
                            else if (diff.y < 5) { mode = 2; Debug.Log(mode + "Mode"); }
                            else { mode = 0; Debug.Log("0Mode"); }
                        }


                        if (mode == 1)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 30) { 转为俯冲姿态 = true; }
                                //this.GetComponent<Rigidbody>().AddForce(new Vector3(0, (this.transform.position.y - 弹道高度) * -0.3f, 0),ForceMode.VelocityChange);
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else if (!转为俯冲姿态)
                            {
                                弹道高度 = 3;
                                int i = 0;
                                this.GetComponent<Rigidbody>().useGravity = true;
                                do
                                {
                                    RaycastHit hitt3;
                                    Ray 检测ray = new Ray(new Vector3(this.transform.position.x, 弹道高度, this.transform.position.z), targetPoint - new Vector3(this.transform.position.x, 弹道高度).normalized);
                                    if (Physics.Raycast(检测ray, out hitt3, Mathf.Infinity))
                                    {
                                        Debug.DrawLine(new Vector3(this.transform.position.x, 弹道高度, this.transform.position.z), targetPoint, Color.red);
                                        if (hitt3.transform.gameObject != currentTarget) { 弹道高度 += 1; Debug.Log("higher!"); continue; }
                                    }
                                    i++;
                                    if (i >= 100) { break; }
                                } while (弹道高度 == 0);
                            }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = true;
                                if (Vector3.Distance(this.transform.position, targetPoint) < 7) { 炸 = true; }
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y > 7) { mode = 2; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//低高度模式


                        else if (mode == 2)
                        {
                            if (弹道高度 != 0 && !转为俯冲姿态)//保持飞行
                            {
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 30) { 转为俯冲姿态 = true; }
                                //this.GetComponent<Rigidbody>().AddForce(new Vector3(0, (this.transform.position.y - 弹道高度) / 20, 0));
                                this.transform.LookAt(targetPoint);
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else if (!转为俯冲姿态) { 弹道高度 = this.transform.position.y; if (this.transform.position.y > 5) { 弹道高度 += 5; } }//当没有在俯冲而且弹道高度=0的时候
                            else
                            {
                                this.transform.LookAt(targetPoint);
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                                if (Vector3.Distance(this.transform.position, targetPoint) < 7) { 炸 = true; }
                                this.GetComponent<Rigidbody>().useGravity = true;
                            }//开始俯冲，将目光放在目标上
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; if (diff.y < -7) { mode = 1; 弹道高度 = 0; } }//当超过目标太远，重设姿态
                        }//终点俯冲模式


                        else if (mode == 3)
                        {

                            Vector3 hitPoint = targetPoint;//存放命中点坐标
                                                           //假设飞机物体是aircraft,炮塔物体是gun 两者间的方向向量就是两种世界坐标相减
                            Vector3 D = this.transform.position - targetPoint;
                            //用飞机transform的TransformDirection方法把前进方向变换到世界坐标，就是飞机飞行的世界方向向量了
                            Vector3 targetDirection = currentTarget.transform.TransformDirection(Vector3.forward);
                            //再用Vector3.Angle方法求出与飞机前进方向之间的夹角
                            float THETA = Vector3.Angle(D, targetDirection);
                            Vector3 导弹速度 = this.GetComponent<Rigidbody>().velocity;
                            Vector3 目标速度 = currentTarget.GetComponent<Rigidbody>().velocity;
                            float DD = D.magnitude;//D是飞机炮塔间方向向量，D的magnitued就是两种间距离
                            float A = 1 - Mathf.Pow((float)(Math.Sqrt(导弹速度.x * 导弹速度.x + 导弹速度.z * 导弹速度.z) / Math.Sqrt(目标速度.x * 目标速度.x + 目标速度.z * 目标速度.z)), 2);//假设炮弹的速度是gunVeloctiy飞机的飞行线速度是aircraftVeloctiy
                            float B = -(2 * DD * Mathf.Cos(THETA * Mathf.Deg2Rad));//要变换成弧度
                            float C = DD * DD;
                            float DELTA = B * B - 4 * A * C;
                            if (DELTA >= 0)
                            {//如果DELTA小于0，无解
                                float F1 = (-B + Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                                float F2 = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                                if ((F1 > F2 && F2 > 0) ^ F1 < 0)
                                {//取较小的一个正数
                                    F1 = F2;
                                }
                                //命中点位置等于 飞机初始位置加上计算出F边长度乘以飞机前进的方向向量，这个乘法等于把前进的距离变换成世界坐标的位移
                                hitPoint = targetPoint + currentTarget.GetComponent<Rigidbody>().velocity.normalized * F1;
                            }//一大堆式子
                            else { hitPoint = targetPoint; }
                            if (!转为俯冲姿态)//保持飞行,关闭重力影响
                            {
                                this.transform.LookAt(hitPoint);
                                this.GetComponent<Rigidbody>().velocity = diff.normalized * sv;
                                this.GetComponent<Rigidbody>().useGravity = false;
                            }
                            else
                            {
                                if (diff.sqrMagnitude < 50) { 炸 = true; }
                                if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) < 17) { 检测错过 = true; }
                            }//开始俯冲，将目光放在目标上，使用重力影响
                            if (Math.Abs(diff.y - Math.Sqrt(diff.x * diff.x + diff.z * diff.z)) > 15 && 转为俯冲姿态 && 检测错过) { 转为俯冲姿态 = false; 检测错过 = false; }//当超过目标太远，重设姿态

                        }//跟随+预瞄模式

                        else if (mode == 0)
                        {//攻顶模式
                            if (转为俯冲姿态 == true)
                            {
                                //this.transform.position.RotateTowards(this.transform.position,targetPoint,2f,Mathf.Infinity);
                                this.transform.LookAt(targetPoint);
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
                                this.GetComponent<Rigidbody>().velocity /= 5;// = new Vector3(this.GetComponent<Rigidbody>().velocity.x, this.GetComponent<Rigidbody>().velocity.y, this.GetComponent<Rigidbody>().velocity.z);
                            }
                        }
                    }
                    if (Vector3.Distance(transform.position, targetPoint) <= 7f)
                    {
                        this.transform.LookAt(targetPoint);
                        this.GetComponent<Rigidbody>().AddForce(diff * sv, ForceMode.VelocityChange);
                        Debug.Log("Keeping Range!");
                    }
                    if (Vector3.Distance(transform.position, targetPoint) <= 0.3f)
                    {
                        炸 = true;
                    }





                    if (launchtime > 10) { 炸 = true; }
                }
                if (this.IsBurning() && !countedBurning)
                {
                    BurningTime = TotalTime;
                    countedBurning = true;
                }
                if (BurningTime + 1 < TotalTime && IsBurning()) { 炸 = true; }

                if (炸 == true)
                {
                    foreach (GameObject CloseEnoughToStop in FindObjectsOfType<GameObject>())
                    {
                        try
                        {
                            if (Vector3.Distance(CloseEnoughToStop.transform.position, this.transform.position) < 8)
                            {
                                CloseEnoughToStop.GetComponent<Rigidbody>().isKinematic = true;
                                foreach (Material M in CloseEnoughToStop.GetComponent<Renderer>().materials)
                                {
                                    M.color = Color.cyan;
                                }
                            }
                        }
                        catch { }
                    }
                    GameObject CyanCoverBall = (GameObject)Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), this.transform.position, this.transform.rotation);
                    CyanCoverBall.GetComponent<Renderer>().material.color = Color.cyan;
                    CyanCoverBall.transform.localScale = Vector3.one * 16;
                    CyanCoverBall.name = "Motion Stopper Bubble";
                    Destroy(CyanCoverBall.GetComponent<Rigidbody>());
                    DestroyImmediate(this.gameObject);
                    //Destroy(Trail);
                }
            }
            else {
                foreach (GameObject MotionStopperBubble in FindObjectsOfType<GameObject>())
                {
                    try
                    {
                        if (MotionStopperBubble.name == "Motion Stopper Bubble")
                        {
                            Destroy(MotionStopperBubble.gameObject);
                        }
                    }
                    catch { }
                }
            }
            //Physics stuff

        }
        void OnCollisionEnter(Collision collision)
        {
            if (发射 == true && launchtime > 0.2 && collision.gameObject.name != "MissileTrail")
            {
                炸 = true;
                GameObject CyanCoverBall = (GameObject)Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), this.transform.position, this.transform.rotation);
                CyanCoverBall.GetComponent<Renderer>().material.color = Color.cyan;
                CyanCoverBall.transform.localScale = Vector3.one * 16;
                CyanCoverBall.name = "Motion Stopper Bubble";
                Destroy(CyanCoverBall.GetComponent<Collider>());
            }
        }
        protected override void OnSimulateExit()
        {
            foreach (GameObject MotionStopperBubble in FindObjectsOfType<GameObject>())
            {
                try
                {
                    if (MotionStopperBubble.name == "Motion Stopper Bubble")
                    {
                        Destroy(MotionStopperBubble.gameObject);
                    }
                }
                catch { }
            }
        }
    }
}
    
