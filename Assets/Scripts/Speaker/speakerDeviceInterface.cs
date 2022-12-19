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

public class speakerDeviceInterface : deviceInterface {
  public int ID = -1;
  public omniJack input;
  speaker output;
  public basicSwitch channelSwitcher;
  public GameObject speakerRim;
  public AudioSource audio;

  SpeakerData data;

  public override void Awake() {
    base.Awake();
    output = GetComponent<speaker>();
    input = GetComponentInChildren<omniJack>();
    speakerRim.GetComponent<Renderer>().material.SetFloat("_EmissionGain", .45f);
    speakerRim.SetActive(false);
  }

  void Start() {
    audio.spatialize = true;
  }

  public void Activate(int[] prevIDs) {
    ID = prevIDs[0];
    input.ID = prevIDs[1];
  }

  float lastScale = 0;

  void Update() {
    if (output.incoming != input.signal) {
      output.incoming = input.signal;
      if (output.incoming == null) speakerRim.SetActive(false);
      else speakerRim.SetActive(true);
    }

    if (output.incoming != null) {
      if (lastScale != transform.localScale.x) {
        lastScale = transform.localScale.x;
        output.volume = Mathf.Pow(lastScale + .2f, 2);
      }
    }

    output.leftOn = channelSwitcher.switchVal;

  }

  public override InstrumentData GetData() {
    SpeakerData data = new SpeakerData();
    data.deviceType = menuItem.deviceType.Speaker;
    GetTransformData(data);
    data.jackInID = input.transform.GetInstanceID();
    data.channelState = channelSwitcher.switchVal;
    return data;
  }

  public override void Load(InstrumentData d) {
    SpeakerData data = d as SpeakerData;

    transform.localPosition = data.position;
    transform.localRotation = data.rotation;
    transform.localScale = data.scale;

    ID = data.ID;
    input.ID = data.jackInID;
    channelSwitcher.setSwitch(data.channelState, true);

  }
}

public class SpeakerData : InstrumentData {
  public int jackInID;
  public bool channelState;
}