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

public class wirePanelComponentInterface : componentInterface {
  public pauseMenu rootMenu;
  public menuManager menuMgr;
  public slider glowSlider;

  public UIpanel[] panels;
  int curSelect = 0;

  public uiPanelSinglePress handlepanel, jackpanel, midipanel;

  Color colorGreen = Color.HSVToRGB(.4f, 230f / 255, 118f / 255);
  Color colorRed = Color.HSVToRGB(0f, 230f / 255, 118f / 255);

  void Start() {
    //midipanel.newColor(colorGreen);
    //jackpanel.newColor(colorGreen);
    //handlepanel.newColor(colorGreen);

    glowSlider.setPercent(masterControl.instance.glowVal);
    for (int i = 0; i < panels.Length; i++) {
      panels[i].keyHit(i == curSelect);
    }

    if (PlayerPrefs.GetInt("midiOut") == 1) {
      string s = "DISABLE MIDI OUT";
      midipanel.label.text = s;
      midipanel.newColor(Color.HSVToRGB(0f, 230f / 255, 118f / 255));
    }

  }

  void Update() {
    if (glowSlider.percent != masterControl.instance.glowVal) {
      masterControl.instance.setGlowLevel(glowSlider.percent);
    }
  }

  public override void hit(bool on, int ID = -1) {
    if (!on) return;
    if (ID == -2) //okay
    {
      rootMenu.cancelFileMenu(); //not right
      transform.gameObject.SetActive(false);
    } else if (ID == 3) {
      bool b = !masterControl.instance.handlesEnabled;
      masterControl.instance.toggleHandles(b);
      string s = b ? "ENABLE POS LOCK" : "DISABLE POS LOCK";
      handlepanel.label.text = s;
      handlepanel.newColor(b ? colorGreen : colorRed);
    } else if (ID == 4) {
      bool b = !masterControl.instance.jacksEnabled;
      masterControl.instance.toggleJacks(b);
      string s = b ? "ENABLE JACK LOCK" : "DISABLE JACK LOCK";
      jackpanel.label.text = s;
      jackpanel.newColor(b ? colorGreen : colorRed);
    } else if (ID == 5) {
      bool b = !menuMgr.midiOutEnabled;
      menuMgr.toggleMidiOut(b);
      string s = b ? "DISABLE MIDI OUT" : "ENABLE MIDE OUT";
      midipanel.label.text = s;
      midipanel.newColor(b ? colorRed : colorGreen);
    } else {
      curSelect = ID;
      masterControl.instance.updateWireSetting(curSelect);
      for (int i = 0; i < panels.Length; i++) {
        if (i != ID) panels[i].keyHit(false);
      }
    }
  }
}
