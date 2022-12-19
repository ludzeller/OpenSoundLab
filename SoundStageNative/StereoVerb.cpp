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

#include "StereoVerb.h"

SOUNDSTAGE_API freeverb::ReverbModel* StereoVerb_New(int sampleRate) {
    return new freeverb::ReverbModel((double)sampleRate);
}
SOUNDSTAGE_API void StereoVerb_Free(freeverb::ReverbModel* x) { delete x; }
SOUNDSTAGE_API void StereoVerb_SetParam(int param, float value, freeverb::ReverbModel *x)
{
    switch (param) {
        case 0:
            x->setRoomSize(value);
            break;
        case 1:
            x->setDamping(value);
            break;
        case 2:
            x->setDryLevel(value);
            break;
        case 3:
            x->setWetLevel(value);
            break;
        case 4:
            x->setWidth(value);
            break;
        case 5:
            x->setFreezeMode(value);
            break;
        default:
            break;
    }
}
SOUNDSTAGE_API float StereoVerb_GetParam(int param, freeverb::ReverbModel *x)
{
    switch (param) {
        case 0:
            return x->getRoomSize();
            break;
        case 1:
            return x->getDamping();
            break;
        case 2:
            return x->getDryLevel();
            break;
        case 3:
            return x->getWetLevel();
            break;
        case 4:
            return x->getWidth();
            break;
        case 5:
            return x->getFreezeMode();
            break;
        default:
            return -1;
            break;
    }
    return 0;
}
SOUNDSTAGE_API void StereoVerb_Clear(freeverb::ReverbModel *x) { x->clear(); }
SOUNDSTAGE_API void StereoVerb_Process(float buffer[], int length, int channels, freeverb::ReverbModel *x)
{
    //This calls a modified FreeVerb function that operates on interleaved audio buffers and also considers the modulation buffers:
    x->processInterleaved(buffer, length, channels);
}
