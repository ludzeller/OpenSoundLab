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

public class quantizerDeviceInterface : deviceInterface {
  public omniJack input, output;
  public dial transposeDial;
  public dial octaveDial;
  public dial scaleDial; 
  quantizerSignalGenerator signal;
  

  public override void Awake() {
    base.Awake();    
    signal = GetComponent<quantizerSignalGenerator>();
  }

  void Update() {

    signal.selectedScale = Mathf.RoundToInt(scaleDial.percent * (signal.scales.Count - 1));
    signal.transpose = transposeDial.percent * 0.1f;
    signal.octave = Utils.map(octaveDial.percent, 0f, 1f, -0.4f, 0.4f); // +/- 4 octaves

    if (signal.incoming != input.signal) signal.incoming = input.signal;
  }

  public override InstrumentData GetData() {
    QuantizerData data = new QuantizerData();
    data.deviceType = DeviceType.Quantizer;
    GetTransformData(data);

    data.transposeState = transposeDial.percent;
    data.scaleState = scaleDial.percent;
    data.octaveState = octaveDial.percent;

    data.jackInID = input.transform.GetInstanceID();
    data.jackOutID = output.transform.GetInstanceID();

    return data;
  }

  public override void Load(InstrumentData d) {
    QuantizerData data = d as QuantizerData;
    base.Load(data);

    input.ID = data.jackInID;
    output.ID = data.jackOutID;

    transposeDial.setPercent(data.transposeState);
    scaleDial.setPercent(data.scaleState);
    octaveDial.setPercent(data.octaveState);
  }
}

public class QuantizerData : InstrumentData {
  public float transposeState;
  public float scaleState;
  public float octaveState;
  public int jackOutID;
  public int jackInID;
  public int jackControlID;
}
