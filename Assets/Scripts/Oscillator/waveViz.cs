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
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;



public class waveViz : MonoBehaviour {

  public Renderer displayRenderer;

  RenderTexture offlineTexture;
  Material offlineMaterial;

  Texture2D onlineTexture;
  public Material onlineMaterial;

  public int waveWidth = 256;
  public int waveHeight = 64;
  public int period = 512; // should not be higher than buffer size, otherwise looped data?
  
  FilterMode fm = FilterMode.Bilinear;
  int ani = 4;

  IntPtr ringBufferPtr;

  ///Writes n samples to the ring buffer.
  [DllImport("SoundStageNative")]
  static extern void RingBuffer_Write(float[] src, int n, IntPtr x);

  ///Writes samples to the ringbuffer with a specified stride. If the stride is 1, all samples are written to the ringbuffer. If stride < 1, some samples are skipped. If stride > 1, some samples are written more than once (=padded)f. No interpolation is performed. Returns the difference between old and new writeptr.
  [DllImport("SoundStageNative")] 
  static extern int RingBuffer_WritePadded(float[] src, int n, float stride, IntPtr x);
  
  ///Reads n samples from the ring buffer
  [DllImport("SoundStageNative")] 
  static extern void RingBuffer_Read(float[] dest, int n, int offset, IntPtr x);

  //Reads n samples with a specific stride
  [DllImport("SoundStageNative")]
  static extern void RingBuffer_ReadPadded(float[] dest, int n, int offset, float stride, IntPtr x);

  ///Reads n samples from the ring buffer and adds the values to the dest array.
  //static extern void RingBuffer_ReadAndAdd(float* dest, int n, int offset, struct RingBuffer *x);

  ///Resizes the buffer. This includes a memory re-allocation, so use with caution!
  //static extern void RingBuffer_Resize(int n, struct RingBuffer *x);
  [DllImport("SoundStageNative")] 
  static extern IntPtr RingBuffer_New(int n);

  ///Frees all resources.
  [DllImport("SoundStageNative")] 
  static extern void RingBuffer_Free(IntPtr x);

  float[] renderBuffer;
  
  void Awake() {

    ringBufferPtr = RingBuffer_New(waveWidth);
    renderBuffer = new float[waveWidth];
    
    offlineMaterial = new Material(Shader.Find("GUI/Text Shader"));
    offlineMaterial.hideFlags = HideFlags.HideAndDontSave;
    offlineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    // get a RenderTexture and set it as target for offline rendering
    offlineTexture = new RenderTexture(waveWidth, waveHeight, 24);
    offlineTexture.useMipMap = true;
    
    // these are the ones that you will actually see
    onlineTexture = new Texture2D(waveWidth, waveHeight, TextureFormat.RGBA32, true);
    onlineMaterial = Instantiate(onlineMaterial);
    displayRenderer.material = onlineMaterial;
    onlineMaterial.SetTexture(Shader.PropertyToID("_MainTex"), onlineTexture);

  }
  
  void Start() {
    onlineMaterial.mainTexture = onlineTexture;
  }
  
  public void storeBuffer(float[] buffer){
    // this should be oversampled!
    RingBuffer_Write(buffer, period, ringBufferPtr);
  }

  void Update() {

    if(displayRenderer.isVisible){ 
      RenderGLToTexture(waveWidth, waveHeight, offlineMaterial);
      if(onlineTexture.filterMode != fm) onlineTexture.filterMode = fm;
      if(onlineTexture.anisoLevel != ani) onlineTexture.anisoLevel = ani;
    }

  }

  void RenderGLToTexture(int width, int height, Material material)
  {

    RingBuffer_Read(renderBuffer, renderBuffer.Length, 0, ringBufferPtr);

    // always re-set offline render here, in case another script had its finger on it in the meantime
    RenderTexture.active = offlineTexture;

    GL.Clear(false, true, Color.black);

    material.SetPass(0);
    GL.PushMatrix();
    GL.LoadPixelMatrix(0, width, height, 0);
    GL.Color(new Color(1, 1, 1, 1f));
    GL.Begin(GL.LINE_STRIP);

    for(int i = 0; i < renderBuffer.Length; i++){
      GL.Vertex3(i, (1f - (renderBuffer[i] + 1f) * 0.5f) * height, 0); // 3px padding
    }

    GL.End();
    GL.PopMatrix();

    // blit/copy the offlineTexture into the onlineTexture (on GPU)
    Graphics.CopyTexture(offlineTexture, onlineTexture);

  }

  public void OnDestroy()
  {
    // cleanup manually, since the GC isn't managing the GPU
    if (offlineTexture != null)
    {
      offlineTexture.Release();
      Destroy(offlineTexture);
    }
    
    RingBuffer_Free(ringBufferPtr);
  }

}
