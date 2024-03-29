# OpenSoundLab

[[Trailer](https://youtu.be/HYn9THRtBvs)] [[Paper](https://dl.acm.org/doi/abs/10.1145/3561212.3561249)] [[Download](https://github.com/ludzeller/OpenSoundLab/releases/)] [[Newsletter](http://eepurl.com/h-9PsD)] [[Discord](https://discord.gg/Jrmg5na3Ru)]
[[Quickstart](https://docs.google.com/document/d/1c9vt-wW-JnW9davSZ76r35cd4dE6xtnyzHEhdrbueOE/edit?usp=sharing)] [[Tutorials](https://www.youtube.com/playlist?list=PLSnuTstoP7nDSK8XqfTnln1v3dH0jACu7)]

OpenSoundLab (OSL) makes modular sound patching three dimensional in a mixed reality experience using Meta Quest's passthrough mode. Patch simple or complex sounds at home, in your studio or in the field. Learn the foundations of creative sound work through video tutorials that are placed right within your patch.

OSL is a fork of Logan Olson's magnificent [SoundStage VR](https://github.com/googlearchive/soundstagevr). OSL enhances the original version so that it is better suited for performing in the context of experimental electronic sound and music, to make it feel less like a game and more like an actual sound laboratory. We recreate the experience of working in spatial setups, but without being bound to mimicking the physical past. Where are the limits of the digital realm, and where are its sweet spots?

The project received generous funding by an educational grant of the University of Applied Sciences and Arts Northwestern Switzerland ([IDCE FHNW](https://www.fhnw.ch/en/about-fhnw/schools/academy-of-art-and-design/institute-digital-communication-environments)).

This repository is work-in-progress. Please always link to this repository if you fork, deploy or otherwise redistribute it, in order to keep other users in sync with our ongoing development. 

### Installation
[OpenSoundLab-0.48](https://github.com/ludzeller/OpenSoundLab/releases/download/0.48/OpenSoundLab-0.48.zip) (BETA 2023-01-03)

At the moment you have to use [sideloading](https://uploadvr.com/sideloading-quest-how-to/) in order to install the .apk binary. You need to register as a developer with Meta, enable the developer mode on your headset with the mobile companion app and use adb or SideQuest to copy the binary to your headset via USB or WiFi. That way you can also manage samples and download your recordings. Having your headset in developer mode is therefore pretty much a requirement for using OSL. Please note that the .apk includes the video tutorials and is therefore over 1GB in size.

### Built with
Unity v2022.3.20f1, Meta XR SDK 62.

### Build instructions
Coming soon...

### Plugin development
Coming later...

### Changes in comparison to SoundStage VR

#### New features
- Mixed-reality passthrough mode
- Master bus recorder in 48kHz, 24bit, WAV
- Master bus overload warning
- AD: Two stage envelope generator with lin/exp control and CV modulation
- Delay: A highly flexible delay line, buffer can range from 1ms to 12.5s, CV modulation
- Reverb: Classic Freeverb stereo reverb with CV modulation
- Scope: Oscilloscope / spectral analyser with trigger on rise
- Added 1V/Oct tracking scheme for Oscillator, Keyboard, Sampler, etc.
- Quantizer: Featuring Sem, Maj, Min, HMaj, HMin, PMaj, PMin, Oct scales and root key, octave dials
- Gain: 36db gain module
- Polarity: Convert between unipolar and bipolar signals
- Artefact: Jitter, bit crush, downsample and noise
- Compressor: Dynamics processor with attack, release, threshold, ratio, bypass, gain and sidechaining
- DC: Bipolar signal generator
- Glide: Slope limiter
- S&H: Sample-and-hold module
- Tutorials: Player console for video tutorials
- VCA: Added amplifier with modulation and ring modulation
- Added performance menu to adjust framerate, resolution, foveated rendering and CPU/GPU levels
- Added nudging for tempo sync with other clocks
- Added navigation by dragging and scaling the complete patch

#### Improved features
- Redesigned look-and-feel and new pictograms
- CVSequencer: Added CV sequencing and both dials and modulation inputs for volume and pitch
- Filter: tracks at audio rate, allows more extreme resonances and modulations
- Oscillator: Added linear through-zero FM, reset, PWM and triangle
- Sampler: Added linear interpolation, linear through-zero FM, modulation for loop in/out, windowing
- Optimized rendering performance for Meta Quest
- Dynamic resolution scaling (including eye-tracked foveated rendering on Quest Pro)
- Switched to ARM64 and recent versions of Unity, Oculus SDK and Vulkan
- Improved performance of waveform displays by rendering them on the GPU
- TouchPad: Added latched mode
- Keyboard: Added CV and Gate outputs

#### Removed features
- Removed default samples
- Disabled Airhorn
- Disabled Maraca
- Removed oscillator from ADSR, ControlCube, Keyboard
- Disabled timeline and MIDI for XyloRoll and Keyboard until fixed


### Mailing list
Please subscribe to our [newsletter](http://eepurl.com/h-9PsD) in order to get updates about new releases.

### Discord
Join the [OpenSoundLab channel](https://discord.com/channels/1020228980583976980) on Discord in order to stay up-to-date, receive/offer support, present your experiments and get to know other OSL users.

### Project Team for OpenSoundLab
###### LEAD
Ludwig Zeller

###### PROGRAMMING
Hannes Barfuss, Ludwig Zeller

###### MENU SYMBOLS
Iman Khoshniataram 
(The menu symbols of OpenSoundLab are licensed as [CC BY-NC-ND 4.0](https://creativecommons.org/licenses/by-nc-nd/4.0/))

###### TESTING & CONSULTING
Anselm Bauer

###### SPONSORING
IDCE FHNW


### Original Credits for SoundStage VR
###### CREATED BY
Logan Olson

###### SOUND DESIGNER (SAMPLES)
Reek Havok

###### PROGRAMMING CONSULTANT
Giray Ozil

###### MUSIC CONSULTANT
Ron Fish

### Other Credits
###### INITIAL QUEST PORT 
James Surine
