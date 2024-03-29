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

public class freeverbDeviceInterface : deviceInterface {
    freeverbSignalGenerator signal;
    public dial size, damping, width, mix;
    public basicSwitch freeze;
    public omniJack omniJackIn, omniJackOut, omniJackModSize, omniJackModFreeze, omniJackModMix;
    public AudioSource speaker;

    private dial[] dials;

    public override void Awake() {
        base.Awake();
        signal = GetComponent<freeverbSignalGenerator>();
        dials = new dial[3] { size, damping, width };
    }

    void Update() {
        if (omniJackIn.signal != signal.sigIn) signal.sigIn = omniJackIn.signal;
        if (omniJackModSize.signal != signal.sigModSize) signal.sigModSize = omniJackModSize.signal;
        if (omniJackModFreeze.signal != signal.sigModFreeze) signal.sigModFreeze = omniJackModFreeze.signal;
        if (omniJackModMix.signal != signal.sigModMix) signal.sigModMix = omniJackModMix.signal;

        signal.SetParam(size.percent, (int)freeverbSignalGenerator.Param.P_ROOMSIZE);
        signal.SetParam(damping.percent, (int)freeverbSignalGenerator.Param.P_DAMPING);
        signal.SetParam(width.percent, (int)freeverbSignalGenerator.Param.P_WIDTH);
        signal.SetParam(freeze.switchVal ? 1 : 0, (int)freeverbSignalGenerator.Param.P_FREEZE);
        signal.SetParam(Utils.equalPowerCrossfadeGain(mix.percent), (int)freeverbSignalGenerator.Param.P_WET);
        signal.SetParam(Utils.equalPowerCrossfadeGain(1 - mix.percent), (int)freeverbSignalGenerator.Param.P_DRY);
    }

    public override InstrumentData GetData() {
        FreeverbData data = new FreeverbData();
        data.deviceType = DeviceType.Freeverb;
        GetTransformData(data);

        data.jackInID = omniJackIn.transform.GetInstanceID();
        data.jackOutID = omniJackOut.transform.GetInstanceID();
        data.cSizeID = omniJackModSize.transform.GetInstanceID();
        data.cFreezeID = omniJackModFreeze.transform.GetInstanceID();
        data.cMixID = omniJackModMix.transform.GetInstanceID();

        data.size = size.percent;
        data.damping = damping.percent;
        data.mix = mix.percent;
        data.width = width.percent;
        data.freeze = freeze.switchVal;

        return data;
    }

    public override void Load(InstrumentData d) {
        FreeverbData data = d as FreeverbData;
        base.Load(data);
        omniJackIn.ID = data.jackInID;
        omniJackOut.ID = data.jackOutID;
        omniJackModSize.ID = data.cSizeID;
        omniJackModFreeze.ID = data.cFreezeID;
        omniJackModMix.ID = data.cMixID;

        size.setPercent(data.size);
        damping.setPercent(data.size);
        mix.setPercent(data.mix);
        width.setPercent(data.width);
        freeze.setSwitch(data.freeze, true);
    }
}

public class FreeverbData : InstrumentData {
    public float size, damping, mix, width;
    public bool freeze;
    public int jackOutID, jackInID;
    public int cSizeID, cFreezeID, cMixID;
}