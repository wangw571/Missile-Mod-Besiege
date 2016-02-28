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
        public override Version Version { get { return new Version("3.5"); } }
        public override string Name { get { return "Missile Mod"; } }
        public override string DisplayName { get { return "Missile Mod"; } }
        public override string BesiegeVersion { get { return "v0.25"; } }
        public override string Author { get { return "覅是"; } }
        protected Block NormalMissile = new Block()
            ///模块ID
            .ID(503)

            ///模块名称
            .BlockName("Missile")

            ///模型信息
            .Obj(new List<Obj> { new Obj("k191.obj", //Obj
                                         "NormalMissile.png", //贴图
                                         new VisualOffset(new Vector3(1f, 1f, 1f), //Scale
                                                          new Vector3(0f, 0f, 0f), //Position
                                                          new Vector3(0f, 0f, 0f)))//Rotation
            })

            ///在UI下方的选模块时的模样
            .IconOffset(new Icon(new Vector3(1.30f, 1.30f, 1.30f),  //Scale
                                 new Vector3(-0.11f, -0.13f, 0.00f),  //Position
                                 new Vector3(85f, 90f, 270f))) //Rotation

            ///没啥好说的。
            .Components(new Type[] {
                                    typeof(missile),
            })

            ///给搜索用的关键词
            .Properties(new BlockProperties().SearchKeywords(new string[] {
                                                             "Missile",
                                                             "导弹",
                                                             "War",
                                                             "Weapon"
                                             })
            .Burnable(1f)//能否燃烧
            //.CanBeDamaged(3)//连接点强度（大概）
            )
            ///质量
            .Mass(0.5f)

            ///是否显示碰撞器（在公开你的模块的时候记得写false）
            .ShowCollider(false)

            ///碰撞器
            .CompoundCollider(new List<ColliderComposite> {
                ColliderComposite.Box(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)),
                ColliderComposite.Capsule(0.35f,1.0f,Direction.Z,new Vector3(0,0,0.8f),Vector3.zero),/*
                                ColliderComposite.Sphere(0.49f,                                //radius
                                                         new Vector3(-0.10f, -0.05f, 0.27f),   //Position
                                                         new Vector3(0f, 0f, 0f))              //Rotation
                                                         .IgnoreForGhost(),                    //Do not use this collider on the ghost

                                ColliderComposite.Capsule(0.33f,                               //radius
                                                          1.33f,                               //length
                                                          Direction.Y,                         //direction
                                                          new Vector3(-0.52f, 0.38f, 0.30f),   //position
                                                          new Vector3(5f, 0f, -5f)),           //rotation                                
                                
                                ColliderComposite.Box(new Vector3(0.65f, 0.65f, 0.25f),        //scale
                                                      new Vector3(0f, 0f, 0.25f),              //position
                                                      new Vector3(0f, 0f, 0f)),                //rotation
                                
                                ColliderComposite.Sphere(0.5f,                                  //radius
                                                         new Vector3(-0.10f, -0.05f, 0.35f),    //Position
                                                         new Vector3(0f, 0f, 0f))               //Rotation
                                                         .Trigger().Layer(2)
                                                         .IgnoreForGhost(),                     //Do not use this collider on the ghost
                              //ColliderComposite.Box(new Vector3(0.35f, 0.35f, 0.15f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)).Trigger().Layer(2).IgnoreForGhost(),   <---Example: Box Trigger on specific Layer*/
            })

            ///你的模块是不是可以忽视强搭
            //.IgnoreIntersectionForBase()

            ///载入资源
            .NeededResources(new List<NeededResource> {
                                new NeededResource(ResourceType.Audio,"missleLaunch.ogg")
            })

            ///连接点
            .AddingPoints(new List<AddingPoint> {
                               (AddingPoint)new BasePoint(false, false)         //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.5f),        //粘连距离
                              //new AddingPoint(new Vector3(0f, 0f, 0.5f), new Vector3(-90f, 0f, 0f),true).SetStickyRadius(0.3f), 和底点差不多，但是要设位置
            });
        protected Block StopMotionMissile = new Block()
            ///模块ID
            .ID(506)

            ///模块名称
            .BlockName("Motion Stopper Missile")

            ///模型信息
            .Obj(new List<Obj> { new Obj("k191.obj", //Obj
                                         "StopMotionMissile.png", //贴图
                                         new VisualOffset(new Vector3(1f, 1f, 1f), //Scale
                                                          new Vector3(0f, 0f, 0f), //Position
                                                          new Vector3(0f, 0f, 0f)))//Rotation
            })

            ///在UI下方的选模块时的模样
            .IconOffset(new Icon(new Vector3(1.30f, 1.30f, 1.30f),  //Scale
                                 new Vector3(-0.11f, -0.13f, 0.00f),  //Position
                                 new Vector3(85f, 90f, 270f))) //Rotation

            ///没啥好说的。
            .Components(new Type[] {
                                    typeof(StopMotionMissileScript),
            })

            ///给搜索用的关键词
            .Properties(new BlockProperties().SearchKeywords(new string[] {
                                                             "Missile",
                                                             "导弹",
                                                             "Bubble",
                                                             "泡泡",
                                                             "War",
                                                             "Weapon"
                                             })
            //.Burnable(3f)//能否燃烧
            //.CanBeDamaged(3)//连接点强度（大概）
            )
            ///质量
            .Mass(0.5f)

            ///是否显示碰撞器（在公开你的模块的时候记得写false）
            .ShowCollider(false)

            ///碰撞器
            .CompoundCollider(new List<ColliderComposite> {
                ColliderComposite.Box(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)),
                ColliderComposite.Capsule(0.35f,1.0f,Direction.Z,new Vector3(0,0,0.8f),Vector3.zero),/*
                                ColliderComposite.Sphere(0.49f,                                //radius
                                                         new Vector3(-0.10f, -0.05f, 0.27f),   //Position
                                                         new Vector3(0f, 0f, 0f))              //Rotation
                                                         .IgnoreForGhost(),                    //Do not use this collider on the ghost

                                ColliderComposite.Capsule(0.33f,                               //radius
                                                          1.33f,                               //length
                                                          Direction.Y,                         //direction
                                                          new Vector3(-0.52f, 0.38f, 0.30f),   //position
                                                          new Vector3(5f, 0f, -5f)),           //rotation                                
                                
                                ColliderComposite.Box(new Vector3(0.65f, 0.65f, 0.25f),        //scale
                                                      new Vector3(0f, 0f, 0.25f),              //position
                                                      new Vector3(0f, 0f, 0f)),                //rotation
                                
                                ColliderComposite.Sphere(0.5f,                                  //radius
                                                         new Vector3(-0.10f, -0.05f, 0.35f),    //Position
                                                         new Vector3(0f, 0f, 0f))               //Rotation
                                                         .Trigger().Layer(2)
                                                         .IgnoreForGhost(),                     //Do not use this collider on the ghost
                              //ColliderComposite.Box(new Vector3(0.35f, 0.35f, 0.15f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)).Trigger().Layer(2).IgnoreForGhost(),   <---Example: Box Trigger on specific Layer*/
            })

            ///你的模块是不是可以忽视强搭
            //.IgnoreIntersectionForBase()

            ///载入资源
            .NeededResources(new List<NeededResource> {
                                new NeededResource(ResourceType.Audio,"missleLaunch.ogg")
            })

            ///连接点
            .AddingPoints(new List<AddingPoint> {
                               (AddingPoint)new BasePoint(false, false)         //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.5f),        //粘连距离
                              //new AddingPoint(new Vector3(0f, 0f, 0.5f), new Vector3(-90f, 0f, 0f),true).SetStickyRadius(0.3f), 和底点差不多，但是要设位置
            });
        protected Block InvicipleMissile = new Block()
            .ID(504)
            .BlockName("Inviciple Missile")
            .Obj(new List<Obj> { new Obj("k191.obj","InvicipleMissile.png",new VisualOffset(new Vector3(1f, 1f, 1f),new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)))
            })
            .IconOffset(new Icon(new Vector3(1.30f, 1.30f, 1.30f), new Vector3(-0.11f, -0.13f, 0.00f), new Vector3(85f, 90f, 270f)))
            .Components(new Type[] {
                                    typeof(InvicipleMissile),
            })
            .Properties(new BlockProperties().SearchKeywords(new string[] {
                                                             "Missile",
                                                             "导弹",
                                                             "无敌",
                                                             "War",
                                                             "Weapon"})
            )
            .Mass(0.5f)
            .ShowCollider(false)
            .CompoundCollider(new List<ColliderComposite> {
                ColliderComposite.Box(new Vector3(0.7f, 0.7f, 1.3f), new Vector3(0f, 0f, 0.8f), new Vector3(0f, 0f, 0f)),
                ColliderComposite.Capsule(0.35f,1.0f,Direction.Z,new Vector3(0,0,0.8f),Vector3.zero), })
            //.IgnoreIntersectionForBase()
            .NeededResources(new List<NeededResource> { new NeededResource(ResourceType.Audio, "missleLaunch.ogg"), new NeededResource(ResourceType.Mesh, "Motion Stopper Bubble.obj") })
            .AddingPoints(new List<AddingPoint> {
                               (AddingPoint)new BasePoint(false, false).Motionable(false,false,false) .SetStickyRadius(0.5f),});
        public override void OnLoad()
        {
            LoadBlock(NormalMissile);//加载该模块
            LoadBlock(InvicipleMissile);
            LoadBlock(StopMotionMissile);
        }
        public override void OnUnload() { }
    }


    public class missile : BlockScript
    {
        protected MKey Key1;
        protected MKey Key2;
        protected MSlider 延迟;
        protected MToggle 不聪明模式;

        private float sv;
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
        private bool 转为俯冲姿态;
        private bool 检测错过;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null
        private AudioSource Audio;
        private GameObject Trail;
        private bool countedBurning = false;

        public override void SafeAwake()
        {
            Key1 = AddKey("Lock On", //按键信息
                                 "Locked",           //名字
                                 KeyCode.T);       //默认按键

            Key2 = AddKey("Launch", //按键信息
                                 "Launch",           //名字
                                 KeyCode.X);       //默认按键

            延迟 = AddSlider("Delay for detecting impact and target",       //滑条信息
                                    "Delay",       //名字
                                    0f,            //默认值
                                    0f,          //最小值
                                    5f);           //最大值

            不聪明模式 = AddToggle("Disable Smart Attack",   //toggle信息
                                       "NoSA",       //名字
                                       false);             //默认状态
        }

        protected virtual IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }
        public override void OnSave(BlockXDataHolder data)
        {
            SaveMapperValues(data);
        }
        public override void OnLoad(BlockXDataHolder data)
        {
            LoadMapperValues(data);
            if (data.WasSimulationStarted) return;
        }
        protected override void OnSimulateStart()
        {
            foreach (Transform t in Machine.Active().SimulationMachine)
            {
                //get the ones that are a HornBlock
                if (t.GetComponent<missile>())
                {
                    /*//and if they use the save keyboard button to play their sound
                    if (t.GetComponent<missile>().Key1.KeyCode[0] == Key1.KeyCode[0] && t.GetComponent<missile>().Key2.KeyCode[0] == Key2.KeyCode[0])
                    {
                        //Add their volume to a coefficient we need for later
                        延迟.Value = t.GetComponent<missile>().延迟.Value;
                        不聪明模式.IsActive = t.GetComponent<missile>().不聪明模式.IsActive;
                    }*/
                }
            }
            sv = 1f;
            svplus = 1f;
            弹道高度 = 0;
            炸 = false;
            countedBurning = false;
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
            Trail.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Particles/Additive"));
            Trail.GetComponent<TrailRenderer>().material.color = Color.yellow;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);
            //Trail.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Trail.GetComponent<TrailRenderer>().autodestruct = true;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = resources["missleLaunch.ogg"].audioClip;
            Audio.loop = false;
            Audio.volume = 0.7f;
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Key1.IsDown)
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

                if (Key2.IsDown)
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        Trail.GetComponent<TrailRenderer>().time = 0.05f;
                        Ray 阻碍检测ray = new Ray(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position));
                        if (Physics.Raycast(阻碍检测ray, out hitt2, Mathf.Infinity))
                        {
                            //Debug.DrawLine(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position), Color.red);
                            if (hitt2.transform.gameObject.name != currentTarget.name ^ !不聪明模式.IsActive)
                            {
                                mode = 0;
                                //Debug.Log(hitt2.transform.name);
                            }
                        }
                        if (resources.ContainsKey("missleLaunch.ogg"))
                        {
                            Audio.volume = 0.007f * 100 /Vector3.Distance(Camera.main.transform.position,this.transform.position);
                            Audio.Play();
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
                    if (!currentTarget)
                    {
                        targetPoint = Vector3.up*1000;
                    }
                    else { targetPoint = currentTarget.transform.position - difftgt; }
                    launchtime += Time.fixedDeltaTime;
                    //this.GetComponent<FireTag>().Ignite();//点火
                    GetComponent<Rigidbody>().AddForce(transform.forward * sv);
                    sv += svplus;
                    svplus *= 0.99f;
                    if (sv > 350) { sv = 350; }
                    diff = (targetPoint - this.transform.position);
                    Trail.GetComponent<TrailRenderer>().time = 12 / this.GetComponent<Rigidbody>().velocity.magnitude;
                    if (launchtime > 延迟.Value)//Targeting
                    {
                        if (不聪明模式.IsActive) { mode = 0; }
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
                            BombExplosion.transform.FindChild("PyroclasticPuff").FindChild("Point light").GetComponent<Light>().range *= 2;
                            BombExplosion.transform.FindChild("PyroclasticPuff").FindChild("Point light").GetComponent<Light>().intensity *= 2;
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
        protected MKey Key1;
        protected MKey Key2;
        protected MToggle NoBlocking;

        public float sv;
        public float svplus;
        private bool toggleEnabled;
        private RaycastHit hitt;
        private GameObject currentTarget;
        private bool 发射 = false;
        private float launchtime;
        private float TotalTime = 0;
        private Vector3 diff;
        private AudioSource Audio;
        private GameObject Trail;
        private int FrameCount = 0;
        private Vector3 Before10Frames;

        public override void SafeAwake()
        {

            Key1 = AddKey("Lock On", //按键信息
                                 "Locked",           //名字
                                 KeyCode.T);       //默认按键

            Key2 = AddKey("Launch", //按键信息
                                 "Launch",           //名字
                                 KeyCode.X);       //默认按键

            NoBlocking = AddToggle("Destroy everything on the way",   //toggle信息
                                       "NoBK",       //名字
                                       false);             //默认状态
        }

        protected virtual IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }
        public override void OnSave(BlockXDataHolder data)
        {
            SaveMapperValues(data);
        }
        public override void OnLoad(BlockXDataHolder data)
        {
            LoadMapperValues(data);
            if (data.WasSimulationStarted) return;
        }
        protected override void OnSimulateStart()
        {
            foreach (Transform t in Machine.Active().SimulationMachine)
            {
                //get the ones that are a HornBlock
                if (t.GetComponent<InvicipleMissile>())
                {
                    //and if they use the save keyboard button to play their sound
                    /*if (t.GetComponent<InvicipleMissile>().Key1.KeyCode[0] == Key1.KeyCode[0] && t.GetComponent<InvicipleMissile>().Key2.KeyCode[0] == Key2.KeyCode[0])
                    {
                        NoBlocking.IsActive = t.GetComponent<InvicipleMissile>().NoBlocking.IsActive;
                    }*/
                }
            }
            sv = 1f;
            svplus = 1.13f;
            toggleEnabled = NoBlocking.IsActive;
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
            Trail.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Particles/Additive"));
            Trail.GetComponent<TrailRenderer>().material.color = Color.yellow;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);

            Before10Frames = this.transform.position;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = resources["missleLaunch.ogg"].audioClip;
            Audio.loop = false;
            Audio.volume = 0.7f;
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Key1.IsDown)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                {
                    currentTarget = hitt.transform.gameObject;
                }
            }
            if (发射 == false)
            {

                if (Key2.IsDown)
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        this.GetComponent<Rigidbody>().useGravity = false;
                        Audio.volume = 0.013f * 100 / Vector3.Distance(Camera.main.transform.position, this.transform.position);
                        Audio.Play();
                    }
                    else { 发射 = false; }
                }
            }//按键
        }

        protected override void OnSimulateFixedUpdate()
        {
            TotalTime += Time.fixedDeltaTime;
            this.GetComponent<Rigidbody>().mass = 0.1f;
            this.GetComponent<Rigidbody>().drag = 0.1f;
            this.GetComponent<Rigidbody>().angularDrag = 0.1f;
            if (AddPiece.isSimulating)
            {
                if (发射 == true)
                {
                    launchtime += Time.fixedDeltaTime;
                    //this.GetComponent<FireTag>().Ignite();//点火
                    try
                    {
                        this.transform.LookAt(currentTarget.transform.position);
                    }
                    catch { Destroy(this); }
                    diff = (currentTarget.transform.position - this.transform.position);
                    this.GetComponent<Rigidbody>().velocity = (this.transform.forward * (diff.magnitude + 5 + currentTarget.GetComponent<Rigidbody>().velocity.magnitude));
                    sv *= svplus;
                    svplus *= 0.99999f;
                    if (sv > 500) { sv = 500; }
                    Trail.GetComponent<TrailRenderer>().time = 12 / this.GetComponent<Rigidbody>().velocity.magnitude;
                    if (!currentTarget) { Destroy(this); }
                    if (FrameCount == 10)
                    {
                            this.GetComponent<Rigidbody>().position += (new Vector3((UnityEngine.Random.value - 0.5f) * 10, (UnityEngine.Random.value - 0.5f) * 10, (UnityEngine.Random.value - 0.5f) * 10));
                        
                        Before10Frames = this.transform.position;
                        FrameCount = 0;
                    }
                    else
                    {
                        ++FrameCount;
                    }
                }
            }
            //Physics stuff

        }
        void OnCollisionEnter(Collision collision)
        {
            if (toggleEnabled)
            {
                if (发射 == true && launchtime > 0.2 && collision.gameObject.name != "MissileTrail" && !collision.gameObject.name.Contains("loor"))
                {
                    Destroy(collision.gameObject);
                }
            }
            else
            {
                if (发射 == true && launchtime > 0.2 && collision.gameObject == currentTarget)
                {
                    发射 = false;
                    Destroy(collision.gameObject);
                    currentTarget = null;
                }
            }
        }
    }

    public class StopMotionMissileScript : BlockScript
    {
        protected MKey Key1;
        protected MKey Key2;
        protected MSlider 延迟;
        protected MToggle 不聪明模式;

        private bool hasFrozen = false;
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
        private float sliderValve;
        private bool 转为俯冲姿态;
        private bool 检测错过;
        private float 弹道高度;
        private int mode;//0-top attack 1-low height high target  2- high height low target 3-follow  4-null
        public Texture NanoTexture;
        private AudioSource Audio;
        private GameObject Trail;
        private bool countedBurning = false;

        public override void SafeAwake()
        {

            Key1 = AddKey("Lock On", //按键信息
                                 "Locked",           //名字
                                 KeyCode.T);       //默认按键

            Key2 = AddKey("Launch", //按键信息
                                 "Launch",           //名字
                                 KeyCode.X);       //默认按键

            延迟 = AddSlider("Delay for detecting impact and target",       //滑条信息
                                    "Delay",       //名字
                                    0f,            //默认值
                                    0f,          //最小值
                                    5f);           //最大值

            不聪明模式 = AddToggle("Disable Smart Attack",   //toggle信息
                                       "NoSA",       //名字
                                       false);             //默认状态
        }

        public override void OnPrefabCreation()
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
        protected virtual IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }
        public override void OnSave(BlockXDataHolder data)
        {
            SaveMapperValues(data);
        }
        public override void OnLoad(BlockXDataHolder data)
        {
            LoadMapperValues(data);
            if (data.WasSimulationStarted) return;
        }
        protected override void OnSimulateStart()
        {
            foreach (Transform t in Machine.Active().SimulationMachine)
            {
                //get the ones that are a HornBlock
                if (t.GetComponent<StopMotionMissileScript>())
                {
                    /*//and if they use the save keyboard button to play their sound
                    if (t.GetComponent<StopMotionMissileScript>().Key1.KeyCode[0] == Key1.KeyCode[0] && t.GetComponent<StopMotionMissileScript>().Key2.KeyCode[0] == Key2.KeyCode[0])
                    {
                        //Add their volume to a coefficient we need for later
                        sliderValve = 延迟.Value;
                        不聪明模式.IsActive = t.GetComponent<StopMotionMissileScript>().不聪明模式.IsActive;
                    }*/
                }
            }

            sv = 1f;
            svplus = 1f;
            弹道高度 = 0;
            炸 = false;
            countedBurning = false;
            sliderValve = 延迟.Value;
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
            Trail.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Particles/Additive")); ;
            Trail.transform.localScale = Vector3.zero;
            Trail.transform.SetParent(this.transform);
            //Trail.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //Trail.GetComponent<TrailRenderer>().autodestruct = true;

            发射 = false;

            Audio = this.gameObject.AddComponent<AudioSource>();
            Audio.clip = resources["missleLaunch.ogg"].audioClip;
            Audio.loop = false;
            Audio.volume = 1;

            try
            {
                NanoTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Resources/Nano Texture.png").texture;
            }
            catch { Debug.Log("No Nano Texture!"); }
        }
        protected override void OnSimulateUpdate()
        {
            //Trail.GetComponent<TrailRenderer>().material.color = Color.white;
            if (Key1.IsDown)
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

                if (Key2.IsDown)
                {
                    if (currentTarget != null)
                    {
                        发射 = true;
                        Trail.GetComponent<TrailRenderer>().time = 0.05f;
                        Audio.volume = 0.007f * 100 / Vector3.Distance(Camera.main.transform.position, this.transform.position);
                        Audio.Play();
                        Ray 阻碍检测ray = new Ray(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position));
                        if (Physics.Raycast(阻碍检测ray, out hitt2, Mathf.Infinity))
                        {
                            //Debug.DrawLine(this.transform.position + (targetPoint - this.transform.position).normalized, (targetPoint - this.transform.position), Color.red);
                            if (hitt2.transform.gameObject.name != currentTarget.name ^ 不聪明模式.IsActive)
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
        /*void Update()
        {
            if (!AddPiece.isSimulating)
            {
                foreach (GameObject MotionStopperBubble in FindObjectsOfType<GameObject>())
                {
                    try
                    {
                        if (MotionStopperBubble.name == "Motion Stopper Bubble" ^ MotionStopperBubble.name == "Sphere")
                        {
                            Destroy(MotionStopperBubble.gameObject);
                        }
                    }
                    catch { }
                }
            }
        }*/
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
                        if (不聪明模式.IsActive) { mode = 0; }
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
                            Vector3 导弹速度 = this.GetComponent<Rigidbody>().velocity * 0.7f;
                            Vector3 目标速度 = currentTarget.GetComponent<Rigidbody>().velocity * 1.5f;
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
                            diff = (hitPoint - this.transform.position);
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

                if (炸 == true && !hasFrozen)
                {
                    hasFrozen = true;
                    foreach (GameObject CloseEnoughToStop in FindObjectsOfType<GameObject>())
                    {
                        try
                        {
                            if (Vector3.Distance(CloseEnoughToStop.transform.position, this.transform.position) < 9)
                            {
                                CloseEnoughToStop.GetComponent<Rigidbody>().mass *= 200;
                                CloseEnoughToStop.GetComponent<Rigidbody>().drag = 4000;
                                CloseEnoughToStop.GetComponent<Rigidbody>().angularDrag = 4000;
                                foreach (Material M in CloseEnoughToStop.GetComponent<Renderer>().materials)
                                {
                                    M.color = Color.cyan;
                                }
                                if (CloseEnoughToStop.GetComponent<MyBlockInfo>().blockName == "BOMB")
                                {
                                    CloseEnoughToStop.GetComponent<ExplodeOnCollide>().hasExploded = true;
                                    CloseEnoughToStop.GetComponent<ExplodeOnCollide>().radius = 0.01F;
                                }
                                else if (CloseEnoughToStop.GetComponent<MyBlockInfo>().blockName == "GRENADE")
                                {
                                    CloseEnoughToStop.GetComponent<ControllableBomb>().hasExploded = true;
                                    CloseEnoughToStop.GetComponent<ControllableBomb>().radius = 0.01f;
                                }
                            }
                        }
                        catch { }
                    }
                    Vector3 currentLocation = new Vector3(this.transform.position.x, this.transform.position.y - 8, this.transform.position.z); ;
                    GameObject CyanCoverBall = new GameObject("Motion Stopper Bubble", new Type[] { typeof(MeshRenderer), typeof(MeshFilter) });
                    Destroy(CyanCoverBall.GetComponent<Rigidbody>());
                    CyanCoverBall.GetComponent<MeshFilter>().mesh = resources["Motion Stopper Bubble.obj"].mesh;
                    CyanCoverBall.transform.position = currentLocation;
                    CyanCoverBall.GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");//Maybe "Custom/TranspDiffuseRim"
                    CyanCoverBall.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = NanoTexture;
                    CyanCoverBall.AddComponent<MeshCollider>();
                    CyanCoverBall.GetComponent<MeshCollider>().sharedMesh = resources["Motion Stopper Bubble.obj"].mesh;
                    CyanCoverBall.GetComponent<MeshCollider>().enabled = true;
                    CyanCoverBall.GetComponent<MeshCollider>().convex = true;
                    CyanCoverBall.GetComponent<MeshCollider>().isTrigger = true;
                    CyanCoverBall.transform.SetParent(this.transform);
                    CyanCoverBall.AddComponent<StopMotionBubble>();
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    //Destroy(Trail);
                }
            }
            else
            {
                try
                {
                    if (炸 == true)
                    {
                        Destroy(GameObject.Find("Motion Stopper Bubble"));
                    }
                }
                catch { Debug.Log("Unsuccessful!"); }
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

    public class StopMotionBubble : MonoBehaviour
    {
        private float SizeMutiplier = 0.3f;
        void FixedUpdate()
        {
            if (AddPiece.isSimulating)
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + 0.05f, this.transform.eulerAngles.z);
                //this.transform.localScale *= 1 + SizeMutiplier;
                SizeMutiplier *= 0.9f;
            }
            else { Destroy(this); }
        }

        void OnTriggerStay(Collider collision)
        {
            if (collision.attachedRigidbody != null && AddPiece.isSimulating)
            {
                collision.attachedRigidbody.mass += 33;
                collision.attachedRigidbody.drag += 20;
                collision.attachedRigidbody.angularDrag += 40;
                collision.attachedRigidbody.velocity *= 0.8f;
                collision.attachedRigidbody.angularVelocity *= 0.8f;
                collision.attachedRigidbody.useGravity = false;
            }
        }
        void OnTriggerExit(Collider collision)
        {
            if (collision.attachedRigidbody != null && AddPiece.isSimulating)
            {
                collision.attachedRigidbody.mass = collision.attachedRigidbody.mass % 33 + 0.1f;
                collision.attachedRigidbody.useGravity = true;
                collision.attachedRigidbody.drag = collision.attachedRigidbody.drag % 20;
                collision.attachedRigidbody.angularDrag = collision.attachedRigidbody.angularDrag % 40;
            }
        }

    }
}

public class HornBlock : BlockScript
    {
        //BlockSettings
        protected MKey activateKey;
        protected MSlider pitchSlider;
        protected MSlider volumeSlider;
        protected MToggle semitoneToggle;

        //other stuff
        protected AudioSource audioSource;
        protected bool hasSound = false;
        protected float HornsWithMyKeyCoefficient = 0f;
        protected bool flipped = false;
        protected Vector3 orgScale = Vector3.one;
        protected bool steamPowered = false;
        protected float steamPoweredTimer = 10f;
        protected float lastSoundTimer = 10f;

        //for the semitone toggle we'll be using we want to know a list of intervals we can use for that
        protected float[] musicIntervals = new float[] { 1 / 1f, 16 / 15f, 9 / 8f, 6 / 5f, 5 / 4f, 4 / 3f, 64 / 45f, 3 / 2f, 8 / 5f, 5 / 3f, 16 / 9f, 15 / 8f, 2 / 1f };
        protected bool waitAFrame = true;

        //BlockLoader Specific method: is called right after the script is made - usually,
        //this is done for all blocks of this type and is safe as it waits for stuff like
        //colliders, visuals, resources and so on
        public override void SafeAwake()
        {
            //Set the blocks original scale
            orgScale = transform.localScale;

            activateKey = AddKey("Sound The Horn", //Display text
                                 "play",           //save name
                                 KeyCode.H);       //Keyboard button

            pitchSlider = AddSlider("Pitch",       //Display Text
                                    "pitch",       //save name
                                    1f,            //default value
                                    0.5f,          //minimum value
                                    2f);           //maximum value

            volumeSlider = AddSlider("Volume",       //Display Text
                                     "volume",       //save name
                                     1f,             //default value
                                     0.1f,           //minimum value
                                     1f);            //maximum value

            semitoneToggle = AddToggle("Semitone Snap",   //Display Text
                                       "semitones",       //save name
                                       true);             //default value

            //Call HandleSemiToneSnap() when the slider value changes
            pitchSlider.ValueChanged += HandleSemitoneSnap;
        }

        //BlockLoader Specific method: Is the safe 1 time called method for the prefab - that's the master gameobject, or template so to speek
        //if you need to make alterations to the block you couldn't do with the standard framework, do it here.
        public override void OnPrefabCreation()
        {
        }

        //BlockLoader Specific method: When the player presses spacebar or the simulate/play button in the upper left corner
        protected override void OnSimulateStart()
        {
            //if the sound file we loaded in the LoadExampleBlock
            if (resources.ContainsKey("warHorn.ogg"))
            {
                hasSound = true;
                //set the audio source we'll be using to the respective component, but if we don't find one, add one.
                audioSource = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
                //set the clip the audio source will be playing to the one we loaded.
                audioSource.clip = resources["warHorn.ogg"].audioClip;
            }
            //This is mostly flair:
            //For all blocks in the machine we have during simulation
            foreach (Transform t in Machine.Active().SimulationMachine)
            {
                //get the ones that are a HornBlock
                if (t.GetComponent<HornBlock>())
                {
                    //and if they use the save keyboard button to play their sound
                    if (t.GetComponent<HornBlock>().activateKey.KeyCode[0] == activateKey.KeyCode[0])
                    {
                        //Add their volume to a coefficient we need for later
                        HornsWithMyKeyCoefficient += t.GetComponent<HornBlock>().volumeSlider.Value;
                    }
                }
            }
        }

        //BlockLoader Specific method: When the player is simulating instead of building
        protected override void OnSimulateUpdate()
        {
            //as long as our block isn't destroyed
            if (!HasBurnedOut() && !Destroyed())
            {
                //when we press the key
                if (activateKey.IsPressed)
                {
                    //Play the horn sound
                    SoundTheHorn();
                }
                //Or when steam powers the horn
                else if (steamPowered)
                {
                    //We check all this stuff to stop spamming of powering the horn
                    if (!audioSource.isPlaying || steamPoweredTimer > 0.1f || lastSoundTimer > 1f)
                    {
                        //Play the horn sound
                        lastSoundTimer = 0f;
                        SoundTheHorn();
                    }
                }
                lastSoundTimer += Time.deltaTime;
                steamPoweredTimer += Time.deltaTime;
                steamPowered = false;
            }
        }

        //Checking Steam powering
        protected virtual void OnParticleCollision(GameObject other)
        {
            //ignore if we are already steam powered
            if (!steamPowered)
            {
                //if collison is with steam
                if (other.name == "SteamParticle")
                    //if it's entering from the top-ish
                    if (Vector3.Angle(-other.transform.forward, transform.forward) < 55f)
                    {
                        steamPoweredTimer = 0f;
                        steamPowered = true;
                    }
            }
        }

        //HornBlock defined method: We have made this method to play the sound we want
        public void SoundTheHorn()
        {
            StopCoroutine(ShakeScale(transform.FindChild("Vis").gameObject, 0.025f, 0.25f / pitchSlider.Value));
            StartCoroutine(ShakeScale(transform.FindChild("Vis").gameObject, 0.025f, 0.25f / pitchSlider.Value));

            if (hasSound)
            {
                //Set a couple of frames of random delay (we don't use Time.deltaTime, because if we have low fps we don't want to wait longer for the sounds to play)
                audioSource.PlayDelayed(UnityEngine.Random.Range(0f, 0.0165f * 3f));
                //Set the pitch - the brightness - of the sound to be dependent on the timescale and the slider we have for it
                audioSource.pitch = Time.timeScale * pitchSlider.Value;
                //This is the flair coefficient, this basically helps the sounds to be dimmed when a lot of blocks play at the same time
                float Coefficient = 0.6f / (HornsWithMyKeyCoefficient < 4.5f ? 1f : HornsWithMyKeyCoefficient / 3f);
                //Set the volume - the loudness - of the sound to be dependent on the slider and coefficient
                audioSource.volume = volumeSlider.Value * Coefficient;
            }
        }

        //BlockLoader Specific method: When we are done simulating, usually you don't need to do anything here,
        //as the block in simulation mode is deleted, but if you have static variables or similar you might want to update it here.
        protected override void OnSimulateExit()
        {
            //reset this flair thing for good meassure
            HornsWithMyKeyCoefficient = 0f;
        }

        //Flip the block on F, this method needs to be called "Flip"
        public void Flip()
        {
            //play the flipping sound - BlockScript predefined
            PlayFlipSound();
            //Do the flipping we need:
            FlipNoSound();
        }

        //Setting the flip needed for copying, loading and such.
        public void SetFlipNoSound(bool flip)
        {
            //we don't want to do the flip if the flipping matches
            if (flipped == flip)
                return;
            //Do the flipping we need:
            FlipNoSound();
        }

        //HornBlock defined method: We flip the block litterally
        public void FlipNoSound()
        {
            //reverse the flipping bool
            flipped = !flipped;
            //mirror the visuals of the block to be... well... mirrored
            foreach (var vis in Visuals)
                MirrorVisual(vis); //BlockScript defined method
                                   //mirror the colliders in a different way where we make sure to rotate the colliders to achieve the correct stuff
            foreach (var col in Colliders)
                MirrorCollider(col); //BlockScript defined method
        }

        //HornBlock defined method:
        //Animates a shake in the scale of a gameobject
        protected IEnumerator ShakeScale(GameObject go, float magnitude, float duration)
        {
            float elapsed = Time.deltaTime;
            duration += Time.deltaTime;

            Vector3 orgScale = go.transform.localScale;
            Vector3 fromScale = orgScale;
            Vector3 targetScale = orgScale;
            float previousCoef = 10f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                //this if statement deals with time scale,
                //for >100% time scale it's run every frame
                //but as the time scale is lower we don't want the shaking to appear much faster than fitting for the time
                //in that scenario we will sample the shaking every now and then
                //we then smoothly lerp between the samples
                if (Time.timeScale >= 0.9f || (elapsed % 0.01666f) / 0.01666f < previousCoef)
                {
                    go.transform.localScale = targetScale;
                    //dampen based on the tails of the animation
                    float percentComplete = elapsed / duration;
                    float damper = 1.0f - Mathf.Clamp(5.0f * percentComplete - 4.0f, 0.0f, 1.0f);

                    // map value to [-1, 1]
                    float x = UnityEngine.Random.value * 2.0f - 1.0f;
                    float y = UnityEngine.Random.value * 2.0f - 1.0f;
                    // dampen
                    x *= magnitude * damper;
                    y *= magnitude * damper;

                    //Set the two points to lerp between
                    fromScale = go.transform.localScale;
                    targetScale = new Vector3(orgScale.x + x, orgScale.y + y, orgScale.z);
                }
                previousCoef = (elapsed % 0.01666f) / 0.01666f;
                go.transform.localScale = Vector3.Lerp(fromScale, targetScale, (elapsed % 0.01666f) / 0.01666f);

                yield return null;
            }
            go.transform.localScale = orgScale;
            yield break;
        }

        //HornBlock defined method: make sure that the pitch only can fit a musical interval when toggled
        protected virtual void HandleSemitoneSnap(float value)
        {
            //we have to wait a frame not to create a feedback loop where when we change the value to snap to a semi tone it will then call this function again
            if (waitAFrame)
            {
                waitAFrame = false;
                float lower, upper;
                if (semitoneToggle.IsActive)
                {
                    //we go through all of the semitones
                    for (int i = 0; i < musicIntervals.Length - 1; i++)
                    {
                        lower = musicIntervals[i];
                        upper = musicIntervals[i + 1];
                        //for 2 octaves, below and above the
                        for (int octave = 1; octave <= 2; octave++)
                        {
                            //we are inversing the intervals to get the intervals below 1.0 and above
                            lower = 1f / lower;
                            upper = 1f / upper;

                            //see whether the value is between the lower and upper (lower and upper can be inverse, due to we inversing the fraction)
                            if ((value >= lower && value <= upper)
                             || (value >= upper && value <= lower))
                            {
                                //find the closer of the values
                                if (Mathf.Abs(lower - value)
                                  < Mathf.Abs(upper - value))
                                {
                                    pitchSlider.Value = lower;
                                    goto ReturnAndUpdate;
                                }
                                else
                                {
                                    pitchSlider.Value = upper;
                                    goto ReturnAndUpdate;
                                }
                            }
                        }
                    }
                    //we were outside the range of the intervals
                    //clamp the value to the absolute lower and upper
                    lower = 1f / musicIntervals[musicIntervals.Length - 1];
                    upper = musicIntervals[musicIntervals.Length - 1];
                    pitchSlider.Value = Mathf.Clamp(value, lower, upper);

                ReturnAndUpdate:
                    StopAllCoroutines();
                    StartCoroutine(UpdateMapper());
                }
            }
            else waitAFrame = true;
        }

        //HornBlock defined method
        protected virtual IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }

        //The following functions are for saving, loading, copying and pasting the values

        //Besiege Specific method
        public override void OnSave(BlockXDataHolder data)
        {
            SaveMapperValues(data);

            data.Write("flipped", flipped);
        }

        //Besiege Specific method
        public override void OnLoad(BlockXDataHolder data)
        {
            LoadMapperValues(data);

            // If the simulation just started we do not want to load
            // our flip state from the data holder.
            // The flip variable is automatically copied
            // over by Unity when the simulation starts.
            if (data.WasSimulationStarted) return;

            if (data.HasKey("flipped"))
            {
                SetFlipNoSound(data.ReadBool("flipped"));
            }
        }
    }

    
