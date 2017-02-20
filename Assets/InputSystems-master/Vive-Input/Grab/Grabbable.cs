using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FRL.IO;
using SB.Seed;
using System;

//Require a GlobalReceiver component and a Collider component on this gameObject.
[RequireComponent(typeof(Receiver))]
[RequireComponent(typeof(Collider))]
public class Grabbable : MonoBehaviour, IGlobalTriggerPressSetHandler, IPointerTriggerPressSetHandler, IGlobalTouchpadTouchHandler
{


    public bool pointerGrab = true;
    public bool colliderGrab = true;

    private new Collider collider;
    private Rigidbody rbody;


    private ViveControllerModule grabbingModule;

    private Vector3 offset;
    private Quaternion rotOffset;
    private Vector3 lockedoffset;


    //guide movement stuff
    //TODO: move them to a diff place
    private AssetWrapper wrapper;
    private GameObject guideTarget;
    private GameObject guideVisualCue;
    private Vector3 guideCenter;
    private Quaternion lockedAxis;
    private Vector3 targetpos;


    void Awake()
    {
        //Get the Collider component on this gameObject.
        collider = this.GetComponent<Collider>();
        rbody = this.GetComponent<Rigidbody>();
        wrapper = this.GetComponent<AssetWrapper>();
    }


    void OnDisable()
    {
        if (grabbingModule != null)
        {
            Release(grabbingModule);
        }
    }


    private FixedJoint AddFixedJoint(BaseInputModule module)
    {
        FixedJoint fx = module.gameObject.AddComponent<FixedJoint>();
        // fx.breakForce = 20000;
        // fx.breakTorque = 20000;
        fx.enablePreprocessing = false;
        return fx;
    }


    void Grab(ViveControllerModule module)
    {
        if (wrapper.isLocked)
            return;

        //Bind the module to this object.
        Debug.Log(HandControllers.Instance.movementState);

        //collider.isTrigger = true;
        grabbingModule = module;
        //Save the offset between the module and this object. Undo the current rotation of the module
        offset = transform.position - grabbingModule.transform.position;
        offset = Quaternion.Inverse(grabbingModule.transform.rotation) * offset;
        rotOffset = Quaternion.Inverse(grabbingModule.transform.rotation) * transform.rotation;

        //whenever you grap, save a offset position to move back and forth
        lockedoffset = grabbingModule.transform.position;
        targetpos = this.transform.position;

        //  AxisLineRenderer axisrender = gameObject.GetComponentInChildren<AxisLineRenderer>();
        guideTarget = wrapper.movementGuide;
        guideVisualCue = wrapper.visualCue;
        guideCenter = guideTarget.transform.localPosition;
        lockedAxis = guideTarget.transform.rotation;
        wrapper.isHolding = true;


        //Save the offset between the module and this object. Undo the current rotation of the module
        if (HandControllers.Instance.movementState == HandControllers.ACTIONTYPE.BASIC)
        {
            if (rbody)
            {
                var joint = AddFixedJoint(grabbingModule);
                joint.connectedBody = rbody;
                rbody.isKinematic = false;
            }
        }
        else
        {
            offset = transform.position - grabbingModule.transform.position;
            offset = Quaternion.Inverse(grabbingModule.transform.rotation) * offset;
            rotOffset = Quaternion.Inverse(grabbingModule.transform.rotation) * transform.rotation;
        }

    }

    void Hold(ViveControllerModule module)
    {
        if (wrapper.isLocked)
            return;

        Vector3 diff = grabbingModule.transform.position - lockedoffset;
        Vector3 pos = guideTarget.transform.localPosition;
      
        switch (HandControllers.Instance.movementState)
        {
            case HandControllers.ACTIONTYPE.BASIC:
                //:TODO. this is a different mode;

                // lock the rotation as you move it freely, 
                //so when you lock any axis they are still aligned moving in the right direction
                guideTarget.transform.rotation = lockedAxis; // Quaternion.Inverse(grabbingModule.transform.rotation); // lockedAxis;
                guideVisualCue.transform.rotation = lockedAxis;
                //
                break;
            case HandControllers.ACTIONTYPE.MOVEX:
                targetpos.x += (diff.x / 4);
                guideTarget.transform.localPosition = transform.InverseTransformPoint(targetpos);
                break;
            case HandControllers.ACTIONTYPE.MOVEY:
                targetpos.y += (diff.y / 4);
                guideTarget.transform.localPosition = transform.InverseTransformPoint(targetpos);
                break;
            case HandControllers.ACTIONTYPE.MOVEZ:
                targetpos.z += (diff.z / 4);
                guideTarget.transform.localPosition = transform.InverseTransformPoint(targetpos);
                break;
        }
    }

    void Release(ViveControllerModule module)
    {
        if (wrapper.isLocked)
            return;

        if (module.gameObject.GetComponent<FixedJoint>())
        {
            // 2
            module.gameObject.GetComponent<FixedJoint>().connectedBody = null;
            Destroy(module.gameObject.GetComponent<FixedJoint>());
            // 3
            // Debug.Log(module.Controller.velocity);
            rbody.velocity = module.Controller.velocity * 3;
            rbody.angularVelocity = module.Controller.angularVelocity;
            rbody.isKinematic = true;
        }



        Vector3 pos = guideTarget.transform.localPosition;
        Vector3 currentPos = this.transform.position;
       gameObject.GetComponentInChildren<AssetWrapper>().isHolding = false;
        switch (HandControllers.Instance.movementState)
        {
            case HandControllers.ACTIONTYPE.BASIC:
                //:TODO. this is a different mode;
                break;
            case HandControllers.ACTIONTYPE.MOVEX:

                currentPos.x += pos.x;
                break;
            case HandControllers.ACTIONTYPE.MOVEY:
                currentPos.y += pos.y;
                break;
            case HandControllers.ACTIONTYPE.MOVEZ:
                currentPos.z += pos.z;
                break;
        }


        //igal
        Vector3 targetPos = currentPos;
        LeanTween.move(gameObject, targetPos, 0.5f);
        //moving back on X only.
        LeanTween.moveLocal(guideTarget, guideCenter, 0.5f);

        //offset = Vector3.zero;
        grabbingModule = null;
        // collider.isTrigger = false;
    }

    /// <summary>
    /// This function is called when the trigger is initially pressed. Called once per press context.
    /// </summary>
    /// <param name="eventData">The corresponding event data for the module.</param>
    public void OnGlobalTriggerPressDown(ViveControllerModule.EventData eventData)
    {
        //Only "grab" the object if it's within the bounds of the object.
        //If the object has already been grabbed, ignore this event call.
        if (collider.bounds.Contains(eventData.viveControllerModule.transform.position) && grabbingModule == null && colliderGrab)
        {
            //Check for a GlobalGrabber if this object should expect one.     
            Grab(eventData.viveControllerModule);
        }
    }

    /// <summary>
    /// This function is called every frame between the initial press and release of the trigger.
    /// </summary>
    /// <param name="eventData">The corresponding event data for the module.</param>
    public void OnGlobalTriggerPress(ViveControllerModule.EventData eventData)
    {

        //Only accept this call if it's from the module currently grabbing this object.
        if (grabbingModule == eventData.viveControllerModule)
        {
            //Check for a GlobalGrabber if this object should expect one.
            Hold(eventData.viveControllerModule);
        }
    }

    /// <summary>
    /// This function is called when the trigger is released. Called once per press context.
    /// </summary>
    /// <param name="eventData">The corresponding event data for the module.</param>
    public void OnGlobalTriggerPressUp(ViveControllerModule.EventData eventData)
    {
        //If the grabbing module releases it's trigger, unbind it from this object.
        if (grabbingModule == eventData.viveControllerModule)
        {
            Release(eventData.viveControllerModule);
        }
    }

    void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
    {
        //Only "grab" the object if it's within the bounds of the object.
        //If the object has already been grabbed, ignore this event call.
        if (grabbingModule == null && pointerGrab)
        {
            //Check for a GlobalGrabber if this object should expect one.
            Grab(eventData.viveControllerModule);
        }
    }

    void IPointerTriggerPressHandler.OnPointerTriggerPress(ViveControllerModule.EventData eventData)
    {

        //Only accept this call if it's from the module currently grabbing this object.
        if (grabbingModule == eventData.viveControllerModule)
        {
            //Check for a GlobalGrabber if this object should expect one.
            Hold(eventData.viveControllerModule);

        }
    }

    void IPointerTriggerPressUpHandler.OnPointerTriggerPressUp(ViveControllerModule.EventData eventData)
    {
        //If the grabbing module releases it's trigger, unbind it from this object.
        if (grabbingModule == eventData.viveControllerModule)
        {
            Release(eventData.viveControllerModule);
        }
    }

    public void OnGlobalTouchpadTouch(ViveControllerModule.EventData eventData)
    {
        //  Debug.Log(eventData.touchpadAxis);  
    }
}
