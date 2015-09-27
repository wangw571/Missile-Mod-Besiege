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
                .Properties(new BlockProperties().Key1("Boom", "k").Key2("Launch", "x")
                                                 .Burnable(10)
                                                 .CanBeDamaged(3)
                                                 .ToggleModeEnabled("Extra Lift Force", false)
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
        // public static Vector3 currentAimingTarget;
        public static GameObject currentAimingTarget;
        private bool launched = false;
        public Transform pusher;
        private float launchtime;
        private bool boomnow;
        private bool toggleEnd;
        private Vector3 diff;
        private bool uped = false;

        protected override void OnSimulateStart()
        {
            sv = 30;
            boomnow = false;
            toggleEnd = false;
        }
        protected override void OnSimulateFixedUpdate()
        {
            float angle = 0;
            float myrts = this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg;
            currentAimingTarget = LaserAim.currentAimingTarget;
            if (myrts >= 180) { myrts = myrts - 360; }
            //Debug.Log(myrts /*Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg*/ /*(this.transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg)*/+ "myD");
            if (AddPiece.isSimulating)
            {
                if (launched == false)
                {
                        if (Input.GetKey(this.GetComponent<MyBlockInfo>().key2))
                        {
                        if (currentAimingTarget != null)
                        {
                            launched = true;
                            //Debug.Log("launched!");
                        }
                        else { Debug.Log("Null!!"); }
                    }
                    else { launched = false; }
                }
                if (launched == true && boomnow == false)
                {
                    if (Input.GetKey(this.GetComponent<MyBlockInfo>().key1))
                    {
                        boomnow = true;
                    }

                    launchtime += Time.fixedDeltaTime;
                    //   Debug.Log((currentTarget.transform.position - this.transform.forward) + "fwd");
                    //    Debug.Log((currentTarget.transform.position - this.transform.position) + "pos");
                    this.GetComponent<FireTag>().Ignite();
                    rigidbody.AddForce(transform.forward * sv);
                    if (this.GetComponent<MyBlockInfo>().toggleModeEnabled == true ^ this.transform.position.y < 2)//Extra Lift
                    {
                        rigidbody.AddForce(new Vector3(0, 0.3f * sv, 0),ForceMode.VelocityChange);
                    }
                    //diff = (currentAimingTarget - this.transform.position);
                    diff = (currentAimingTarget.transform.position - this.transform.position);
                    //Debug.Log(Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg);
                    if (launchtime > this.GetComponent<MyBlockInfo>().sliderValue)//Targeting
                    {
                        if (Math.Abs(launchtime-1) < 0.2  ^ currentAimingTarget.transform.position.y - this.transform.position.y <= -20) { this.transform.LookAt(currentAimingTarget.transform.position); }
                        
                        angle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg - myrts);
                        //Debug.Log(angle + "toD");
                        if (currentAimingTarget.transform.position.y >= this.transform.position.y)
                        {
                           // Debug.Log("UP!");
                            this.transform.LookAt(new Vector3(this.transform.position.x, currentAimingTarget.transform.position.y, this.transform.position.z));
                            uped = true;
                        }
                        else  if (uped == true)
                        {
                            this.transform.LookAt(currentAimingTarget.transform.position);
                            uped = false;
                        }
                        if (angle > 0){
                            if (angle < 2)
                            {
                                this.transform.Rotate(Vector3.up * (1f * (angle / 2))/*, ForceMode.Acceleration*/); /*Debug.Log("pp");*/
                            }
                            else { this.transform.Rotate(Vector3.up * 1f/*, ForceMode.Acceleration*/); /*Debug.Log("p"); */}
                        }
                        else {
                            if (angle > -2)
                            {
                                this.transform.Rotate(Vector3.up * (-1f * (angle / -2))/*, ForceMode.Acceleration*/); /*Debug.Log("nn");*/
                            }
                            else { /*rigidbody.AddTorque*/this.transform.Rotate(Vector3.up *-1f/*, ForceMode.Acceleration*/);/*Debug.Log("n"); */}
                        }
                        
                        //rigidbody.AddTorque(new Vector3(diff.x, 0, diff.z).normalized);
                        toggleEnd = true;
                        if (Vector3.Distance(transform.position, currentAimingTarget.transform.position) <= 3) { boomnow = true; }
                    }
                    if (launchtime > 10) { boomnow = true; }
                }
                if (this.GetComponent<BlockHealthBar>().health == 0 && launched == false)
                {
                    boomnow = true;
                }

                if (boomnow == true)
                {
                    //      Debug.Log("boom!");
                    GameObject component = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Object.FindObjectOfType<AddPiece>().blockTypes[23].gameObject);
                    component.transform.position = this.transform.position;
                    UnityEngine.Object.Destroy(component.GetComponent<Rigidbody>());
                    component.AddComponent<Rigidbody>();
                    component.gameObject.AddComponent<KillIfEditMode>();
                    component.GetComponent<ExplodeOnCollide>().radius = 7f;
                    component.GetComponent<FireTag>().Ignite();
                    UnityEngine.Object.DestroyImmediate(this.gameObject);
                }
            }
            else {}
                //Physics stuff
            }
        void OnCollisionEnter(Collision collision)
        {
            if (launched == true && toggleEnd == true && launchtime > 0.1)
            {
                boomnow = true;
            }
        }
        protected override void OnSimulateExit()
        {
            currentAimingTarget = null;
            launched = false;
        }
    }


    }





