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

public class gainDeviceInterface : deviceInterface {
  public omniJack input, output;
  dial ampDial;
  gainSignalGenerator signal;
  basicSwitch activeSwitch;
  float lastAmpValue = -1f;

  public override void Awake() {
    base.Awake();
    ampDial = GetComponentInChildren<dial>();
    activeSwitch = GetComponentInChildren<basicSwitch>();
    signal = GetComponent<gainSignalGenerator>();
  }

  void Update() {

    if (ampDial.percent != lastAmpValue)
    {
      signal.amp = Mathf.Pow(2, ampDial.percent * 6); // 36db gain, 6 db per duplication, log(36) * 6 = 6 * 6 = 36
      lastAmpValue = ampDial.percent;
    }

    signal.active = activeSwitch.switchVal;
    if (signal.incoming != input.signal) signal.incoming = input.signal;
  }

  public override InstrumentData GetData() {
    GainData data = new GainData();
    data.deviceType = DeviceType.Gain;
    GetTransformData(data);

    data.dialState = ampDial.percent;
    data.switchState = activeSwitch.switchVal;

    data.jackInID = input.transform.GetInstanceID();
    data.jackOutID = output.transform.GetInstanceID();

    return data;
  }

  public override void Load(InstrumentData d) {
    GainData data = d as GainData;
    base.Load(data);

    input.ID = data.jackInID;
    output.ID = data.jackOutID;

    ampDial.setPercent(data.dialState);
    activeSwitch.setSwitch(data.switchState);
  }
}

public class GainData : InstrumentData {
  public float dialState;
  public bool switchState;
  public int jackOutID;
  public int jackInID;
  public int jackControlID;
}
