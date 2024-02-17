// This file is part of OpenSoundLab, which is based on SoundStage VR.
//
// Copyright � 2020-2023 GPLv3 Ludwig Zeller OpenSoundLab
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// 
// Copyright � 2020 Apache 2.0 Maximilian Maroe SoundStage VR
// Copyright � 2019-2020 Apache 2.0 James Surine SoundStage VR
// Copyright � 2017 Apache 2.0 Google LLC SoundStage VR
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class manipulator : MonoBehaviour
{
  int controllerIndex = -1; // 0 => left, 1 => right
  public GameObject activeTip;
  public Transform tipL, tipR;
  
  List<Transform> hitTransforms = new List<Transform>();
  Transform selectedTransform;
  manipObject selectedObject;

  menuspawn _menuspawn;
  
  bool usingOculus = false;
  public Color onColor = Color.HSVToRGB(208 / 359f, 234 / 255f, 93 / 255f);

  OSLInput oslInput;

  void Awake()
  {
    _menuspawn = GetComponent<menuspawn>(); 
    activeTip.SetActive(false);

    oslInput = new OSLInput();
    oslInput.Patcher.Enable();
    
  }


  bool controllerVisible = true;
  public void toggleController(bool on)
  {
    controllerVisible = on;
  }

  public void SetDeviceIndex(int index)
  {
    controllerIndex = index;    
  }

  public bool isLeftController() {
    return controllerIndex == 0 ? true : false;
  }

  bool grabbing;
  public bool emptyGrab;

  public bool isGrabbing()
  {
    return (selectedObject != null);
  }

  public void showTip()
  {
    
  }

  public void invertScale()
  {
    transform.parent.localScale = new Vector3(-1, 1, 1);

  }

  void Grab(bool on)
  {
    grabbing = on;
    showTip();

    if (selectedObject != null)
    {
      selectedObject.setGrab(grabbing, transform);
      if (!on) toggleController(true);
      else if (usingOculus) selectedObject.setTouch(true);
    }
    else emptyGrab = on;
  }

  public manipObject getSelection()
  {
    if (!grabbing) return null;
    return selectedObject;
  }

  public void ForceRelease()
  {
    showTip();
    if (grabbing == true)
    {
      release();

      grabbing = false;
    }
  }

  void release()
  {
    selectedObject.setState(manipObject.manipState.none);
    toggleController(true);
  }

  public void ForceGrab(manipObject o)
  {
    if (selectedObject != null) release();

    grabbing = true;
    selectedObject = o;
    selectedObject.setGrab(grabbing, transform);
    emptyGrab = false;
    selectedTransform = o.transform;
  }

  void OnCollisionEnter(Collision coll)
  {
    manipObject o = coll.transform.GetComponent<manipObject>();
    if (o != null) o.onTouch(true, this);
  }

  void OnCollisionExit(Collision coll)
  {
    manipObject o = coll.transform.GetComponent<manipObject>();
    if (o != null) o.onTouch(false, this);
  }


  void OnCollisionStay(Collision coll)
  {
    hitTransforms.Add(coll.transform);
  }


  void FixedUpdate()
  {
    hitTransforms.Clear();
  }

  Coroutine pulseCoroutine;
  public void constantPulse(bool on)
  {
    if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
    if (on)
    {
      pulseCoroutine = StartCoroutine(pulseRoutine());
    }

  }

  IEnumerator pulseRoutine()
  {
    while (true)
    {
      hapticPulse(750);
      yield return new WaitForSeconds(0.3f);
    }
  }

  public void hapticPulse(float hapticPower = 1200f) {
     
    doHaptic(hapticPower);
  }

  public void bigHaptic(float hapticPower = 750f, float dur = 0.1f)
  {
    doHaptic(hapticPower, dur);
  }


  void doHaptic(float hapticPower = 750f, float dur = 0.1f)
  {
    if (Unity.XR.Oculus.Utils.GetSystemHeadsetType() == Unity.XR.Oculus.SystemHeadset.Meta_Quest_Pro 
    || Unity.XR.Oculus.Utils.GetSystemHeadsetType() == Unity.XR.Oculus.SystemHeadset.Meta_Link_Quest_Pro)
    {
      hapticPower = hapticPower * 2.3f;
      dur *= 0.07f;      
    }

    if (Unity.XR.Oculus.Utils.GetSystemHeadsetType() == Unity.XR.Oculus.SystemHeadset.Oculus_Quest_2)
    {
      hapticPower *= 3f;
      dur *= 0.07f;
    }

    if (Unity.XR.Oculus.Utils.GetSystemHeadsetType() == Unity.XR.Oculus.SystemHeadset.Oculus_Link_Quest_2)
    {
      hapticPower *= 4f;
      dur *= 0.07f;
    }

    

    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

    if (controllerIndex == 0)
    {
      UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, devices);
    }
    else if (controllerIndex == 1)
    {
      UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, devices);
    }

    foreach (var device in devices)
    {
      UnityEngine.XR.HapticCapabilities capabilities;
      if (device.TryGetHapticCapabilities(out capabilities))
      {
        if (capabilities.supportsImpulse)
        {
          uint channel = 0;
          float amplitude = hapticPower / 3999.0f;
          amplitude = Mathf.Clamp01(amplitude);
          device.SendHapticImpulse(channel, amplitude, dur);
        }
      }
    }
  }


  void LateUpdate()
  {
    if (grabbing) return;
    Transform candidate = null;

    if (selectedObject != null)
    {
      if (hitTransforms.Contains(selectedTransform)) candidate = selectedTransform;
    }
    else
    {
      foreach (Transform t in hitTransforms)
      {
        if (t != null)
        {
          manipObject o = t.GetComponent<manipObject>();
          if (o != null)
          {
            if (o.curState != manipObject.manipState.grabbed && o.curState != manipObject.manipState.selected)
            {
              candidate = t;
              break;
            }
          }
        }
      }
    }

    if (candidate != selectedTransform)
    {
      if (selectedObject != null) selectedObject.GetComponent<manipObject>().setSelect(false, transform);

      if (candidate != null)
      {
        candidate.GetComponent<manipObject>().setSelect(true, transform);
        if (candidate.GetComponent<handle>() != null) toggleCopy(true);
        else if (candidate.GetComponent<manipObject>().canBeDeleted) toggleDelete(true);
        hapticPulse();
        selectedTransform = candidate;
        selectedObject = candidate.GetComponent<manipObject>();
      }
      else
      {
        toggleCopy(false);
        toggleDelete(false);
        selectedTransform = null;
        selectedObject = null;
      }
    }
  }

  bool copyEnabled = false;
  void toggleCopy(bool on)
  {
    copyEnabled = on;
  }

  bool deleteEnabled = false;
  void toggleDelete(bool on)
  {
    if (multiselecting) return;
    deleteEnabled = on;
  }

  timelineGridUI multiselectGrid;

  bool multiselectEnabled = false;
  public void toggleMultiselect(bool on, timelineGridUI _grid)
  {
    if (!on && multiselecting) return;

    multiselectGrid = on ? _grid : null;
    multiselectEnabled = on;    
  }

  public void SetTrigger(bool on)
  {
    triggerDown = on;

    if (selectedObject != null)
    {
      if (selectedObject.stickyGrip && selectedObject.curState == manipObject.manipState.grabbed)
      {
        if (on) Grab(!on);
        return;
      }
    }
    Grab(on);
  }

  bool multiselecting = false;
  public void MultiselectSelection(bool on)
  {
    multiselecting = on;

    if (multiselectGrid == null)
    {
      Debug.Log("HRM..");
      return;
    }
    else
    {
      multiselectGrid.onMultiselect(on, transform);
    }
  }

  bool deleting = false;
  public void DeleteSelection(bool on)
  {
    deleting = on;
    if (on && selectedObject != null)
    {
      selectedObject.selfdelete();
    }

    if (!on)
    {
      if (selectedObject == null) toggleDelete(false);
      else if (!selectedObject.canBeDeleted) toggleDelete(false);
    }
  }

  bool copying = false;
  public void SetCopy(bool on)
  {
    if (menuManager.instance.simple) return;

    copying = on;

    if (selectedObject != null)
    {
      if (on)
      {
        if (selectedObject.GetComponent<handle>() != null) SaveLoadInterface.instance.Copy(selectedObject.transform.parent.gameObject, this);
      }
      else if (selectedObject.GetComponent<handle>() != null && !triggerDown) Grab(false);
    }
  }

  void updateProngs()
  {
    float val = 0;
    
    if (controllerIndex == 0)
    {
      val = Input.GetAxis("triggerR");
    }
    else if (controllerIndex == 1)
    {
      val = Input.GetAxis("triggerL");
    }
    else
    {
      val = 0;
    }

    tipL.localPosition = new Vector3(Mathf.Lerp(-.005f, 0, val), -.005f, -.018f);
    tipR.localPosition = new Vector3(Mathf.Lerp(.004f, -.001f, val), -.005f, -.018f);
  }


  public void changeHW(string s)
  {
    if (s == "oculus")
    {
      usingOculus = true;
    }
  }


  bool touchpadActive = false;


  public bool triggerDown = false;
  public bool pinchPinkyDown = false;
  static OVRPlugin.Controller lastControl = OVRPlugin.Controller.None;
  public bool isTrackingWorking;

  Renderer[] renderers;
  SkinnedMeshRenderer[] skinnedRenderers;

  void Start()
  {
    renderers = gameObject.transform.parent.parent.parent.GetComponentsInChildren<Renderer>();
    skinnedRenderers = gameObject.transform.parent.parent.parent.GetComponentsInChildren<SkinnedMeshRenderer>();

    if (controllerIndex == 0)
    {
      currentInput = OVRInput.Controller.LTouch; // check if correct
    }
    else if (controllerIndex == 1)
    {
      currentInput = OVRInput.Controller.RTouch;
    }
  }

  OVRPlugin.Controller currentControl = OVRPlugin.GetActiveController(); //get current controller scheme
  OVRInput.Controller currentInput = OVRInput.Controller.None;

  void Update()
  {

    Debug.Log("");
    Debug.Log("GetControllerOrientationTracked " + OVRInput.GetControllerOrientationTracked(currentInput));
    Debug.Log("GetControllerPositionTracked " + OVRInput.GetControllerPositionTracked(currentInput));
    Debug.Log("GetControllerOrientationValid " + OVRInput.GetControllerOrientationValid(currentInput));
    Debug.Log("GetControllerPositionValid " + OVRInput.GetControllerPositionValid(currentInput));
    Debug.Log("");


    isTrackingWorking = OVRInput.GetControllerOrientationTracked(currentInput) &&
      OVRInput.GetControllerPositionTracked(currentInput);


    //OVRInput.GetControllerOrientationValid(currentInput);
    //OVRInput.GetControllerPositionValid(currentInput)


    isTrackingWorking = true;
      
      foreach (Renderer childRenderer in renderers)
      {
        childRenderer.enabled = isTrackingWorking;
      }

      foreach (SkinnedMeshRenderer childRenderer in skinnedRenderers){
        childRenderer.enabled = isTrackingWorking;
      }
      
    if(!isTrackingWorking) return;

    if (controllerIndex == 0)
    {
      transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
      transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
    } 

    if(controllerIndex == 1){
      transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
      transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }

    updateProngs();
    
    bool currentControlHands = false;
    bool currentControlChanged = false;
    
    if (currentControl != lastControl)
    {
      currentControlChanged = true;
      lastControl = currentControl;
    }

    // manage copy
    if (controllerVisible)
    {
      if (oslInput.wasCopyStartedByController(controllerIndex))
      {
        if (copyEnabled) SetCopy(true);
        else if (deleteEnabled) DeleteSelection(true);
        else if (multiselectEnabled) MultiselectSelection(true);
      }
      else if (oslInput.wasCopyReleasedByController(controllerIndex))
      {
        if (copying) SetCopy(false);
        else if (deleting) DeleteSelection(false);
        else if (multiselectEnabled) MultiselectSelection(false);
      }
    }
    else if (grabbing && selectedObject != null)
    {
      if (oslInput.wasCopyStartedByController(controllerIndex)) selectedObject.setPress(true);
      if (oslInput.wasCopyReleasedByController(controllerIndex)) selectedObject.setPress(false);
    }

    // manage interaction
    if (oslInput.wasTriggerStartedByController(controllerIndex))
    {
      activeTip.SetActive(true);
      tipL.gameObject.SetActive(false);
      tipR.gameObject.SetActive(false);
      SetTrigger(true);
    }

    if (oslInput.wasTriggerReleasedByController(controllerIndex))
    {
      activeTip.SetActive(false);
      tipL.gameObject.SetActive(true);
      tipR.gameObject.SetActive(true);
      SetTrigger(false);
    }

    // manage ongoing grabbing
    if (grabbing && selectedObject != null) selectedObject.grabUpdate(transform);
    else if (selectedObject != null) selectedObject.selectUpdate(transform);
  }




}


