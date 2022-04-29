// Copyright � 2017, 2020-2022 Logan Olson, Google LLC, James Surine, Ludwig Zeller, Hannes Barfuss
//
// This file is part of SoundStage Lab.
//
// SoundStage Lab is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SoundStage Lab is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SoundStage Lab.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class midiCCout : signalGenerator {
  public int ID;

  signalGenerator incoming;
  public omniJack input;

  public Transform percentQuad;
  public Renderer[] glowRends;
  public TextMesh label;

  public float curValue = 0f;
  bool updateDesired = true;

  public bool ccMessageDesired = false;

  float hue = 0;

  public override void Awake() {
    base.Awake();
    input = GetComponentInChildren<omniJack>();
  }

  void Start() {
    glowRends[2].material.SetFloat("_EmissionGain", .3f);
  }

  public void SetAppearance(string s, float h) {
    label.text = s;
    hue = h;
    for (int i = 0; i < glowRends.Length; i++) {
      glowRends[i].material.SetColor("_TintColor", Color.HSVToRGB(hue, .5f, .7f));
    }
  }

  void Update() {
    if (input.signal != incoming) incoming = input.signal;

    if (updateDesired) {
      float val = (curValue + 1) / 2f;
      percentQuad.localScale = new Vector3(val, 1, 1);
      percentQuad.localPosition = new Vector3((1 - val) / 2f, 0, 0);
      updateDesired = false;
    }
  }

  public override void processBuffer(float[] buffer, double dspTime, int channels) {
    if (incoming == null) return;
    incoming.processBuffer(buffer, dspTime, channels);
    if (curValue != buffer[buffer.Length - 1]) {
      curValue = buffer[buffer.Length - 1];
      updateDesired = true;
      ccMessageDesired = true;
    }
  }
}
