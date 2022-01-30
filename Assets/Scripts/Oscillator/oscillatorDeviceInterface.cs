// Copyright 2017 Google LLC
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

public class oscillatorDeviceInterface : deviceInterface
{

  public int ID = -1;
  bool active = false;
  bool lfo = false;

  // interfaces
  public basicSwitch lfoSwitch;
  public dial freqDial, ampDial;
  public waveViz viz;
  public omniJack signalOutput, freqExpInput, freqLinInput, ampInput, syncInput, pwmInput;
  public sliderNotched waveSlider;
  public AudioSource speaker;

  // current values
  float freqPercent, ampPercent, wavePercent;

  oscillatorSignalGenerator signal;
  int bufferSize;

  public override void Awake()
  {
    base.Awake();
    AudioConfiguration configuration = AudioSettings.GetConfiguration();
    bufferSize = configuration.dspBufferSize;

    signal = GetComponent<oscillatorSignalGenerator>();
    active = true;
    viz.sampleStep = lfo ? bufferSize : 1;

    UpdateLFO();
    UpdateAmp();
    UpdateWave();
  }

  void Update()
  {
    if (!active) return;

    // update changed inputs
    if (lfo != !lfoSwitch.switchVal) UpdateLFO();
    if (freqPercent != freqDial.percent) UpdateFreq();
    if (ampPercent != ampDial.percent) UpdateAmp();
    if (wavePercent != waveSlider.percent) UpdateWave();

    // update inputs
    if (signal.freqExpGen != freqExpInput.signal) signal.freqExpGen = freqExpInput.signal;
    if (signal.freqLinGen != freqLinInput.signal) signal.freqLinGen = freqLinInput.signal;
    if (signal.ampGen != ampInput.signal) signal.ampGen = ampInput.signal;
    if (signal.syncGen != syncInput.signal) signal.syncGen = syncInput.signal;
    if (signal.pwmGen != pwmInput.signal) signal.pwmGen = pwmInput.signal;
  }

  void UpdateLFO()
  {
    lfo = !lfoSwitch.switchVal;
    viz.sampleStep = lfo ? bufferSize : 1;

    signal.lfo = lfo;
    UpdateFreq();
    //speaker.volume = lfo ? 0f : 1f;
  }

  void UpdateFreq()
  {
    freqPercent = freqDial.percent;
    if (lfo)
    {
      signal.frequency = 2f * Mathf.Pow(2, Utils.map(freqPercent, 0f, 1f, -8f, 8f)); // 2Hz base, 16 octaves range
    }
    else
    {
      signal.frequency = 261.6256f * Mathf.Pow(2, Utils.map(freqPercent, 0f, 1f, -5f, 5f)); // C4, 10 octaves range
    }
    // though this would make viz more adaptive, but it shows garbage.
    //viz.period = Mathf.RoundToInt(Utils.map(signal.frequency, 0f, 10000f, 1f, bufferSize));
  }

  void UpdateAmp()
  {
    ampPercent = ampDial.percent;
    signal.amplitude = ampPercent * ampPercent;
  }

  void UpdateWave()
  {
    wavePercent = waveSlider.percent;
    signal.analogWave = waveSlider.percent;
  }

  public override InstrumentData GetData()
  {
    OscillatorData data = new OscillatorData();
    data.deviceType = menuItem.deviceType.Oscillator;
    GetTransformData(data);

    data.lfo = lfo;
    data.amp = ampPercent;
    data.freq = freqPercent;
    data.wave = wavePercent;

    data.jackOutID = signalOutput.transform.GetInstanceID();
    data.jackInAmpID = ampInput.transform.GetInstanceID();
    data.jackInFreqExpID = freqExpInput.transform.GetInstanceID();
    data.jackInFreqLinID = freqLinInput.transform.GetInstanceID();
    data.jackInSyncID = syncInput.transform.GetInstanceID();
    data.jackInPwmID = pwmInput.transform.GetInstanceID();

    return data;
  }

  public override void Load(InstrumentData d)
  {
    OscillatorData data = d as OscillatorData;
    base.Load(data);

    freqDial.setPercent(data.freq);
    ampDial.setPercent(data.amp);
    waveSlider.setVal(Mathf.RoundToInt(data.wave * 3));
    lfoSwitch.setSwitch(!data.lfo);

    ID = data.ID;
    signalOutput.ID = data.jackOutID;
    ampInput.ID = data.jackInAmpID;
    freqExpInput.ID = data.jackInFreqExpID;
    freqLinInput.ID = data.jackInFreqLinID;
    syncInput.ID = data.jackInSyncID;
    pwmInput.ID = data.jackInPwmID;
  }
}

public class OscillatorData : InstrumentData
{
  public float amp;
  public float freq;
  public bool lfo;
  public float wave;
  public int jackOutID;
  public int jackInAmpID;
  public int jackInFreqExpID;
  public int jackInFreqLinID;
  public int jackInSyncID;
  public int jackInPwmID;
}

