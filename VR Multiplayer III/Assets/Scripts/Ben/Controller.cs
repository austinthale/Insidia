using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;


public class Controller : MonoBehaviour {

    public SteamVR_TrackedController _controller;
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;
    public Vector2 touchSpot;
    public GameObject dino;
    public GameObject bullet;
    public GameObject cameraHead;
    public Transform nose;
    public Transform vrRig;

    public List<Transform> allFocus;

    public static Action<GameObject> AddAsTargetUI;
    public static Action Unsubscribe;

    public float moveSpeed;
    public float rotateSpeed;
    public float jumpSpeed = 5;
    public float bulletSpeed = 500;
    public float frameCount;
    public float jumpAmount;
    public float forwardJmpSpeed = 50;
    public bool touching;
    public bool pulledTrigger;
    public bool gripped;
    public bool meleeMode;
    public bool switching;

    public Transform focus;
    
    public bool clicked;

    public Shader Outlineable;
    public float outlineSize;


    private void OnEnable()
    {

        _controller = GetComponent<SteamVR_TrackedController>();
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        _controller.TriggerClicked += HandleTriggerClicked;
        _controller.TriggerUnclicked += HandleTriggerUnclicked;
        _controller.PadTouched += LPadTouched;
        _controller.PadUntouched += LPadUntouched;
        _controller.PadClicked += RHandlePadClicked;
        _controller.PadUnclicked += RHandlePadClickUp;
        _controller.Gripped += HandleGripped;
        _controller.Ungripped += HandleUngripped;
        
        _controller.MenuButtonClicked += MenuButtonHandler;
        //_controller.MenuButtonUnclicked += MenuButtonUnclicked;
        touching = false;
        pulledTrigger = false;
        clicked = false;
        meleeMode = true;
        switching = false;

        dino = Mover.thisDino;

        //_controller.TriggerUnclicked += HandleTriggerUnclicked;
        
        //following = false;
        //melee = false;
        //_controller.Ungripped += HandleUngripped;            
        }


    private void HandleUngripped(object sender, ClickedEventArgs e)
    {
        
    }

    private void HandleGripped(object sender, ClickedEventArgs e)
    {
        if (!gripped)
        {
            gripped = true;
            WeaponLockOn_KH.setFocus(this);
        }else if(gripped)
        {
            gripped = false;
            if (focus != null)
            {
                focus.GetComponent<Renderer>().material.SetFloat("_OutlineTransparency", 0);
                focus = null;
            }
            if (AddAsTargetUI != null)
                AddAsTargetUI(null);
        }
        //Mover.CallMover(this);
//		if(AddAsTargetUI != null && focus.gameObject != null)
//        	AddAsTargetUI(focus.gameObject);
    }

    

    private void RHandlePadClickUp(object sender, ClickedEventArgs e)
    {
        clicked = false;
        
    }

    private void RHandlePadClicked(object sender, ClickedEventArgs e)
    {
        
        clicked = true;
        Mover.CallMover(this);
        //StartCoroutine(Jump());
    }

    private void LPadUntouched(object sender, ClickedEventArgs e)
    {
        touching = false;
        
        
    }

    private void LPadTouched(object sender, ClickedEventArgs e)
    {
        
        touching = true;
		if(Mover.CallMover != null)
        	Mover.CallMover(this);

    }

    private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
    {
        pulledTrigger = false;
        if(meleeMode)
        {
            BaseMelee.ReleaseMeleeAttack();
            SoundEngine_JR.meleeSound();
        }

    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        pulledTrigger = true;
        if(meleeMode)
        {
            BaseMelee.StartMeleeCount();
        }
        if (!meleeMode)
        {
            if(Shoot.shooter != null)
                Shoot.shooter();
            SoundEngine_JR.shootSound();
        }
    }

    private void MenuButtonHandler(object sender, ClickedEventArgs e)
    {
        if(meleeMode && !switching)
        {
            meleeMode = false;
            //StartCoroutine(SwitchWait());
            return;
        }else if(!meleeMode && !switching)
        {
            meleeMode = true;
            //StartCoroutine(SwitchWait());
            return;
        }
    }

    private void OnDisable()
    {
        _controller.TriggerClicked -= HandleTriggerClicked;
        _controller.TriggerUnclicked -= HandleTriggerUnclicked;
        _controller.PadTouched -= LPadTouched;
        _controller.PadUntouched -= LPadUntouched;
        _controller.PadClicked -= RHandlePadClicked;
        _controller.PadUnclicked -= RHandlePadClickUp;
        _controller.Gripped -= HandleGripped;
        _controller.Ungripped -= HandleUngripped;
    }

    IEnumerator SwitchWait()
    {
        switching = true;
        yield return new WaitForSeconds(.5f);
        switching = false;

    }

    

    //IEnumerator MoveController()
    //{
    //    yield return new WaitForFixedUpdate();
    //    device = SteamVR_Controller.Input((int)trackedObject.index);
    //    touchSpot = new Vector2(device.GetAxis().x, device.GetAxis().y);
    //    if (touchSpot != new Vector2(0, 0))
    //    {
    //        dino.transform.Translate(0, 0, (touchSpot.y * moveSpeed) * Time.deltaTime);
    //        dino.transform.Rotate(0, (touchSpot.x * rotateSpeed) * Time.deltaTime, 0);
    //    }
    //    if (touching)
    //        StartCoroutine(MoveController());

    //}

    //IEnumerator ShootGun()
    //{
    //    yield return new WaitForFixedUpdate();

    //    bullet.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

    //    yield return new WaitForSeconds(1);


    //    bullet.transform.position = nose.GetComponent<Transform>().position;
    //    bullet.transform.localEulerAngles = nose.localEulerAngles;
    //}

    //IEnumerator Jump()
    //{
    //    yield return new WaitForFixedUpdate();
    //    dino.transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    //    if (clicked == true && frameCount < jumpAmount)
    //        StartCoroutine(Jump());
    //}



    //For the sake of debuging on a non VR controller


    /*void Update()
    {
        if(Input.GetButtonDown("Horizontal")|| Input.GetButtonDown("Vertical"))
        {
            touching = true;
            Mover.CallMover(this); 
        }
        if (!Input.GetButtonDown("Horizontal")&& !Input.GetButtonDown("Vertical"))
        {
            touching = false;
        }
        if(Input.GetKeyDown("Space"))
        {
            clicked = true;
            Mover.CallMover(this);
        }
        if (Input.GetKeyUp("Space"))
        {
            clicked = false;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            pulledTrigger = true;
            Shoot.shooter();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            pulledTrigger = false;
        }
        if(Input.GetButtonDown("Fire2"))
        {
            gripped = true;
            Mover.CallMover(this);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            gripped = false;
        }
    }*/


}
