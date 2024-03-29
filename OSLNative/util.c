// This file is part of OpenSoundLab, which is based on SoundStage VR.
//
// Copyright © 2020-2023 GPLv3 Ludwig Zeller OpenSoundLab
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
// Copyright © 2020 Apache 2.0 Maximilian Maroe SoundStage VR
// Copyright © 2019-2020 Apache 2.0 James Surine SoundStage VR
// Copyright © 2017 Apache 2.0 Google LLC SoundStage VR
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

// Created by Hannes

#include "util.h"
#include <math.h>
#include <assert.h>
#include <string.h>
#include <stdio.h>
#include <time.h>
#include "lookup_tables.h"
#if defined(ANDROID) || defined(__ANDROID__) || defined(__APPLE__)
#include <sys/time.h>
#endif
#ifdef _WIN32
#pragma message "Target platform is Windows."
/* Windows does not support C99 standard, so variable length arrays are not supported and are emulated using _malloca and _freea. */
#include <malloc.h>
#include <stddef.h>
#endif

#ifdef __APPLE__
#define __APPLE_VDSP 1
#pragma message "Target platform is Apple."
#include <Accelerate/Accelerate.h>
#endif

#if defined(ANDROID) || defined(__ANDROID__)
    #pragma message "Target platform is Android."
#endif

#if __aarch64__
    #pragma message "Target architecture is aarch64."
#elif __arm__
    #pragma message "Target architecture is armv7."
#endif

#if __ARM_NEON
    #pragma message "ARM Neon is available and will be used."
    #include <arm_neon.h>
    /*#if __ARM_FEATURE_FMA
        #pragma message "Fused floating-point multiply-accumulate is supported."
    #endif
    #if __ARM_BIG_ENDIAN
        #pragma message "Target is big-endian."
    #else
        #pragma message "Target is little-endian."
    #endif
    #if __ARM_FEATURE_CLZ
        #pragma message "Count leading zeros feature is supported."
    #endif*/
    #if __ARM_NEON_FP
        #pragma message "Hardware floating-point is available."
    #endif
#else
    #pragma message "ARM Neon is not available on the target platform."
#endif

/* Unfortunately, the math functions of Ne10 are only available for armv7. The DSP functions are available for aarch64 too. To use Ne10, have to call ne10_init(); once to set the function pointers. */
#if __NE10
    #pragma message "Using Ne10 library."
    #include "NE10.h"
#endif

#ifdef __cplusplus
extern "C" {
#endif
    
#if __NE10
    int Ne10_init = 0;
    void _init_Ne10(void)
    {
        ne10_result_t r = ne10_init();
        if(r == NE10_OK)
        {
            Ne10_init = 1;
        }
        else
        {
            printv("Error initializing Ne10 library.");
        }
    }
#endif
    
    void *_malloc(size_t n)
    {
        return malloc(n);
    }
    
    void _free(void *x)
    {
        free(x);
    }
    
    int _mstosmpls(float ms, float sampleRate)
    {
        return (int) ((ms/1000) * sampleRate);
    }

    int _stosmpls(float s, float sampleRate)
    {
        return (int) (s * sampleRate);
    }

    float _atodb(float a) {
        return a == 0 ? -192 : 20 * log10f(a);
    }

    float _dbtoa(float a) {
        return powf(10, a/20);
    }
    
    float _clamp(float f, float min, float max) {
      const double t = f < min ? min : f;
      return t > max ? max : t;
    }
    
    float _interpolate_linear(float a, float b, float frac)
    {
        return a + frac * (b - a); //bc (1-c)a + cb = a + c(b-a)
    }
    
    inline int16_t _float32toint16(float f)
    {
        return ((int32_t)(f * 32760.0f + 32768.5f)) - 32768;
    }
    
    void _float32toint16buffer(float* src, int16_t* dest, int n)
    {
        for(int i = 0; i < n; i++)
        {
            dest[i] = _float32toint16(src[i]);
        }
    }
    
    void _float32toint24buffer(float* src, int8_t* dest, int endianness, int n)
    {
#if __APPLE_VDSP_DONTUSE
        ///This does not work the way intended, probably bc of the stride, bc it worked when used on each sample individually...
        float scaling = 8388600.0f;
        vDSP_vsmfix24(src, 1, &scaling, (vDSP_int24*)dest, 3, n);
#else
        int32_t integer;
        for(int i = 0; i < n; i++)
        {
            integer = ((int32_t)(src[i] * 8388600.0f + 8388608.5f)) - 8388608;
            
            if(endianness == WAV_LITTLE_ENDIAN)
            {
                dest[3 * i] = (int8_t)(integer >> 16);
                dest[3 * i + 1] = (int8_t)(integer >> 8);
                dest[3 * i + 2] = (int8_t)integer;
            }
            else
            {
                dest[3 * i + 2] = (int8_t)(integer >> 16);
                dest[3 * i + 1] = (int8_t)(integer >> 8);
                dest[3 * i] = (int8_t)integer;
            }
        }
#endif
    }

    float _max(float a, float b) {
        return (a > b ? a : b);
    }

    float _min(float a, float b) {
        return (a < b ? a : b);
    }

    void _fZero(float *dest, int n)
    {
        memset(dest, 0, n*sizeof(float));
    }
    
    void _fCopy(float *src, float *dest, int n)
    {
        memcpy(dest, src,  n*sizeof(float));
    }

    void _fScale(float *src, float *dest, float factor, int n)
    {
        if(factor == 1)
            return;
#if __APPLE_VDSP_DONTUSE
        /* vDSP is slower than Neon here for small vector sizes (up to 1024) */
        vDSP_vsmul(src, 1, &factor, dest, 1, n);
/*#elif __NE10
        ne10_mulc_float(src, dest, factor, n);*/
#elif __ARM_NEON
        /* Twice as fast as naive implementation (Onone) */
        /* 4x as fast as naive implementation (O3) */
        /* 4x as fast as naive implementation (Ofast) */
        /* Surprisingly, this version is a little faster than using vmulq_n_f32.
         But it uses one register more (float32x4_t scale).
         */
        if(n % 4 == 0)
        {
            float32x4_t scale = vdupq_n_f32(factor);
            float32x4_t temp;
            for(int i = 0; i < n; i+= 4)
            {
                temp = vld1q_f32(src + i);
                temp = vmulq_f32(temp, scale);
                vst1q_f32(src + i, temp);
            }
        }
        else
            for(int i = 0; i < n; i++)
                dest[i] = factor * src[i];
#else
        for(int i = 0; i < n; i++)
        {
            dest[i] = factor * src[i];
        }
#endif
    }

    void _fMultiply(float *src1, float *src2, float *dest, int n)
    {
#if __APPLE_VDSP_DONTUSE
        /* For small vector sizes (e.g. 1024), vDSP is slower here. But for large vector sizes (e.g.1024 << 8), it is a lot faster. */
        vDSP_vmul(src1, 1, src2, 1, dest, 1, n);
#elif __ARM_NEON
        /* 1.5x as fast as naive implementation (Onone) */
        /* TODO: a bit slower than naive implementation (O3) */
        /* TODO: a bit slower than naive implementation (Ofast) */
        assert(n % 4 == 0);
        float32x4_t temp1;
        float32x4_t temp2;
        for(int i = 0; i < n; i+=4)
        {
            temp1 = vld1q_f32(src1 + i);
            temp2 = vld1q_f32(src2 + i);
            temp2 = vmulq_f32(temp1, temp2);
            vst1q_f32(dest + i, temp2);
        }
#else
        for(int i = 0; i < n; i++)
        {
            dest[i] = src1[i] * src2[i];
        }
#endif
    }

    void _fPow(float* src, float* dest, float power, int n) {
      for (int i = 0; i < n; i++) {
        dest[i] = pow(src[i], power); 
      }
    }

    void _fAdd(float *src1, float *src2, float *dest, int n)
    {
#if __APPLE_VDSP_DONTUSE
        /* For small vector sizes (e.g. 1024), vDSP is slower here. But for large vector sizes (e.g.1024 << 8), it is a lot faster. */
        vDSP_vadd(src1, 1, src2, 1, dest, 1, n);
#elif __ARM_NEON
        /* 1.5x as fast as naive implementation (Onone) */
        /* TODO: naive implementation is twice as fast (O3) */
        /* A bit slower than naive implementation (Ofast). Interestingly, the Neon version gets a speedup from O3 to Ofast, while the naive implementation does not change. */
        if(n % 4 == 0)
        {
            float32x4_t temp1;
            float32x4_t temp2;
            for(int i = 0; i < n; i+=4)
            {
                temp1 = vld1q_f32(src1 + i);
                temp2 = vld1q_f32(src2 + i);
                temp2 = vaddq_f32(temp1, temp2);
                vst1q_f32(dest + i, temp2);
            }
        }
        else
            for(int i = 0; i < n; i++)
                dest[i] = src1[i] + src2[i];
#else
        for(int i = 0; i < n; i++)
        {
            dest[i] = src1[i] + src2[i];
        }
#endif
    }

    void _fAddSingle(float* src1, float val, float* dest, int n) {
        for (int i = 0; i < n; i++)
        {
          dest[i] = src1[i] + val;
        }
    }
    
    void _fMultiplyAdd(float *src1, float *src2, float *dest, int n)
    {
#if __APPLE_VDSP_DONTUSE
        /* For small vector sizes (e.g. 1024), vDSP is slower here. But for large vector sizes (e.g.1024 << 8), it is a lot faster. */
        vDSP_vma(src1, 1, src2, 1, dest, 1, dest, 1, n);
#elif __ARM_NEON
        //TODO: This is only 17% faster than the naive implementation (Onone)
        //TODO: With O3 and Ofast, it is even a bit slower than naive implementation
        assert(n % 4 == 0);
        float32x4_t temp1, temp2, temp3;
        for(int i = 0; i < n; i+=4)
        {
            temp1 = vld1q_f32(src1 + i); //read first operand
            temp2 = vld1q_f32(src2 + i); //read second operand
            temp3 = vld1q_f32(dest + i); //read accumulator
#if __ARM_FEATURE_FMA /* use  fused multiply-add if available for greater accuracy */
            temp3 = vfmaq_f32(temp3, temp1, temp2); //multiply and add to accumulator
#else
            temp3 = vmlaq_f32(temp3, temp1, temp2); //multiply and add to accumulator
#endif
            vst1q_f32(dest + i, temp3); //write back to destination buffer
        }
#else
        for(int i = 0; i < n; i++)
        {
            dest[i] += src1[i] * src2[i];
        }
#endif
    }
    
    /* Helpful example: https://developer.arm.com/documentation/dui0472/k/Compiler-Features/Using-NEON-intrinsics */
    float _fSumOfMags(float *src, int n)
    {
#if __APPLE_VDSP
        /* Interestingly, the vDSP version here is a little slower than the naive approach, also for large vector sizes (e.g. 1024 << 8). */
        float result;
        vDSP_svemg(src, 1, &result, n);
        return result;
        /* This is another way to do it, but it was a little bit slower then vDSP_svemg: */
        //return cblas_sasum(n, src, 1);
#elif __ARM_NEON_DONTUSE
        /* TODO: 0.25x as fast as naive implementation (Ofast) */
        /* 3x as fast as naive implementation (Onone) */
        /* 5x as fast as naive implementation (O3) */
        assert(n % 4 == 0);
        float32x4_t temp;
        float32x4_t acc = vdupq_n_f32(0);
        int i, m, factor;
        
        factor = 4;
        m = n / factor;
        
        for(i = 0; i < m; i++)
        {
            //load 4 floats into a 128bit register
            temp = vld1q_f32(src + factor * i);
            //take absolute values
            temp = vabsq_f32(temp);
            //add to accumulator
            acc = vaddq_f32(acc, temp);
        }
        //sum the final 4 floats in the accumulator register the naïve way
        return (acc[0] + acc[1] + acc[2] + acc[3]);
#else
        float acc = 0;
        for(int i = 0; i < n; i++)
        {
            acc += fabs(src[i]);
        }
        return acc;
#endif
    }
    
    float _fAverageSumOfMags(float *src, int n)
    {
        return _fSumOfMags(src, n) / n;
    }
    
    void _fAbs(float *src, float* dest, int n)
    {
#if __APPLE_VDSP
        /* vDSP is 2x as fast as Neon here */
        vDSP_vabs(src, 1, dest, 1, n);
#elif __ARM_NEON
        /* 2x as fast as naive implementation (Onone) */
        /* 3.5x as fast as naive implementation (O3) */
        /* 3.5x as fast as naive implementation (Ofast) */
        assert(n % 4 == 0);
        float32x4_t temp;
        for(int i = 0; i < n; i +=4)
        {
            //load 4 floats into a 128bit register
            temp = vld1q_f32(src + i);
            //take absolute values
            temp = vabsq_f32(temp);
            //write to destination vector
            vst1q_f32(dest + i, temp);
        }
#else
        for(int i = 0; i < n; i++)
        {
            dest[i] = fabs(src[i]);
        }
#endif
    }
    
    int _nextPowOf2(int n) {
        uint32_t K = n;
        K--;
        K |= K >> 1;
        K |= K >> 2;
        K |= K >> 4;
        K |= K >> 8;
        K |= K >> 16;
        K++;
        return K;
    }
    
    int _isPowOf2(int n)
    {
        return (n != 0) && ((n & (n - 1)) == 0);
    }
    

    void _fInterleave(const float* src, float *dest, const int n, int channels)
    {
#if __APPLE_VDSP_DONTUSE
        /* vDSP is a little faster here, but vDSP_ztoc does not work if src == dest, so it is disabled for now. */
        DSPSplitComplex input = {
                .realp = src,
                .imagp = src + n / 2
            };
        vDSP_ztoc(&input, 1, (DSPComplex*)dest, 2, n/2);
#elif __ARM_NEON
        /* 3x as fast as naive implementation (Onone, O3, Ofast) */
        assert(channels == 2);
        assert(n % 8 == 0);
        
        int m = n / 2;
        const float *left = src;
        const float *right = src + m;
        float32x4x2_t temp;
        /* out of place: */
        if(src == dest)
        {
            float t[n];
            for(int i = 0; i < n; i+=8)
            {
                temp.val[0] = vld1q_f32(left + i/2);
                temp.val[1] = vld1q_f32(right + i/2);
                vst2q_f32(t + i, temp);
            }
            memcpy(dest, t,  n*sizeof(float));
        }
        /* in-place: */
        else
        {
            for(int i = 0; i < n; i+=8)
            {
                temp.val[0] = vld1q_f32(left + i/2);
                temp.val[1] = vld1q_f32(right + i/2);
                vst2q_f32(dest + i, temp);
            }
        }
#else
        int m = n / channels;
        /* out of place: */
        if(src == dest)
        {
#ifdef _WIN32
            float* t = _malloca(n * sizeof(float));
#else
            float t[n];
#endif
            for(int i = 0; i < m; i++)
                for(int j = 0; j < channels; j++)
                    t[channels * i + j] = src[j * m + i];
            memcpy(dest, t,  n*sizeof(float));
#ifdef _WIN32
            _freea(t);
#endif
        }
        /* in-place: */
        else
        {
            for(int i = 0; i < m; i++)
                for(int j = 0; j < channels; j++)
                    dest[channels * i + j] = src[j * m + i];
        }
#endif
    }
    
    void _fDeinterleave(const float* src, float *dest, const int n, int channels)
    {
#if __APPLE_VDSP_DONTUSE
        /* vDSP is a little faster here, but vDSP_ctoz does not work if src == dest, so it is disabled for now. */
        DSPSplitComplex output = {
                .realp = dest,
                .imagp = dest + n / 2
            };
            vDSP_ctoz((const DSPComplex *)src, 2, &output, 1, n / 2);
#elif __ARM_NEON
        /* 3x as fast as naive implementation (Onone, O3, Ofast) */
        assert(channels == 2);
        assert(n % 8 == 0);
        
        float32x4x2_t temp;
        int m = n / channels;
        /* out of place: */
        if(src == dest)
        {
            float t[n];
            
            for(int i = 0; i < n; i += 8)
            {
                temp = vld2q_f32(src + i);
                vst1q_f32(t + i / 2, temp.val[0]);
                vst1q_f32(t + m + i / 2, temp.val[1]);
            }
            memcpy(dest, t,  n*sizeof(float));
        }
        /* in-place: */
        else
        {
            for(int i = 0; i < n; i += 8)
            {
                temp = vld2q_f32(src + i);
                vst1q_f32(dest + i / 2, temp.val[0]);
                vst1q_f32(dest + m + i / 2, temp.val[1]);
            }
        }
#else
        int m = n / channels;
        /* out of place: */
        if(src == dest)
        {
#ifdef _WIN32
            float* t = _malloca(n * sizeof(float));
#else
            float t[n];
#endif
            for(int i = 0; i < m; i++)
                for(int j = 0; j < channels; j++)
                    t[j * m + i] = src[channels * i + j];
            memcpy(dest, t,  n*sizeof(float));
#ifdef _WIN32
            _freea(t);
#endif
        }
        /* in-place: */
        else
        {
            for(int i = 0; i < m; i++)
                for(int j = 0; j < channels; j++)
                    dest[j * m + i] = src[channels * i + j];
        }
#endif
    }
    
    void _fDryWetLogarithmic(float *dryBuf, float* wetBuf, float* dest, float dry, float wet, int n)
    {
        dry = (2*dry) - 1;
        dry = sqrtf(0.5 * (1 + dry));
        wet = (2*wet) - 1;
        wet = sqrtf(0.5 * (1 + wet));
        _fDryWetLinear(dryBuf, wetBuf, dest, dry, wet, n);
    }
    
    void _fDryWetLinear(float *dryBuf, float* wetBuf, float* dest, float dry, float wet, int n)
    {
        float ratio = dry / wet;
        _fScale(dryBuf, dest, ratio, n);
        _fAdd(wetBuf, dest, dest, n);
        _fScale(dest, dest, wet, n);
    }
    
    void _fCrossfadeLinear(float* src1, float* src2, float* dest, int n)
    {
        float frac = 0, step = 1.0f/(n-1);
        dest[0] = src1[0];
        dest[n-1] = src2[n-1];
        for(int i = 1; i < n-1; i++)
        {
            frac += step;
            dest[i] = src1[i] + frac * (src2[i] - src1[i]); //bc (1-c)a + cb = a + c(b-a)
        }
    }
    
    void _fCrossfadeLogarithmic(float* src1, float* src2, float* dest, int destructive, int n)
    {
        if(destructive)
        {
            if(n == 256)
            {
                _fMultiply(src1, xfade_log_256_desc, src1, 256);
                _fMultiply(src2, xfade_log_256_asc, src2, 256);
                _fAdd(src1, src2, dest, 256);
                return;
            }
            else if (n == 512)
            {
                _fMultiply(src1, xfade_log_512_desc, src1, 512);
                _fMultiply(src2, xfade_log_512_asc, src2, 512);
                _fAdd(src1, src2, dest, 512);
                return;
            }
            else if (n == 1024)
            {
                _fMultiply(src1, xfade_log_1024_desc, src1, 1024);
                _fMultiply(src2, xfade_log_1024_asc, src2, 1024);
                _fAdd(src1, src2, dest, 1024);
                return;
            }
        }

        //If we can't use the lookup tables, do it the naive way:
        float mult, frac1, frac2;
        int max = n-1;
        dest[0] = src1[0];
        dest[n-1] = src2[n-1];
        for(int i = 1; i < n-1; i++)
        {
            mult = 2*i / (float)max - 1; // 2x/(n-1) - 1
            frac1 = sqrtf(0.5f * (1 - mult));
            frac2 = sqrtf(0.5f * (1 + mult));
            dest[i] = frac1 * src1[i] + frac2 * src2[i];
        }
    }
    
    void _fLerp(float* src, float* dest, float gain1, float gain2, int n)
    {
        if(gain1 == gain2)
        {
            _fScale(src, dest, gain2, n);
            return;
        }
        
        float gain;
        float gainDelta = gain2 - gain1;
        float max = (float)(n-1);
        for(int i = 0; i < n; i++)
        {
            gain = gain1 + i/max * gainDelta;
            dest[i] = gain * src[i];
        }
    }
    
    void _fClamp(float *src, float min, float max, int n)
    {
        for(int i = 0; i < n; i++)
        {
            src[i] = _min(max, _max(min, src[i]));
        }
        //for (int i = 0; i < n; ++i)
        //{
        //  if (src[i] < min)
        //  {
        //    src[i] = min;
        //  }
        //  else if (src[i] > max)
        //  {
        //    src[i] = max;
        //  }
        //}
    }
    
    void _fNoise(float *buf, float amplitude, int n)
    {
        if(amplitude == 0)
            return;
        
        for(int i = 0; i < n; i++)
        {
            buf[i] = ((float)rand() / RAND_MAX * 2 - 1) * amplitude;
        }
    }
    
    void _fNoiseAdditive(float *buf, float amount, int n)
    {
        _clamp(amount, 0, 1);
        
        if(amount == 0)
            return;
        
        float inverse = 1 - amount;
        for(int i = 0; i < n; i++)
        {
            buf[i] = inverse * buf[i] + buf[i] * ((float)rand() / RAND_MAX * 2 - 1) * amount;
        }
    }
    
    void _fDownSample(float *buf, int factor, int n)
    {
        _clamp(factor, 1, 64);
        
        if(factor == 1)
            return;
        
        float sample;
        for(int i = 0; i < n; i++)
        {
            if(i % factor == 0)
                sample = buf[i];
            buf[i] = sample;
        }
    }
    
    void _fJitter(float *buf, float amount, int n)
    {
        _clamp(amount, 0, 1);
        
        if(amount == 0)
            return;
        
        float frac;
        int s1, s2;
        for(int i = 1; i < n-1; i++)
        {
            frac = amount * ( (float)rand() / RAND_MAX * 2 - 0.999f );
            s1 = (int)(i + frac); //either i or i-1
            s2 = s1 + 1; //either i or i+1
            if(frac < 0)
                frac = 1 + frac;
            
            buf[i] = buf[s1] + frac * (buf[s2] - buf[s1]); //a + c(b-a)
        }
    }
    
    void _fBitCrush(float *buf, int bitReduction, int n)
    {
        if(bitReduction == 0 || bitReduction > 32)
            return;
        
        int64_t sample;
        for(int i = 0; i < n; i++)
        {
            sample = (((int64_t)((double)buf[i] * 2147483640 + 2147483648.5)) - 2147483648);
            sample = sample >> bitReduction;
            sample = sample << bitReduction;
            buf[i] = ((float)sample) / 2147483648;
        }
    }
    
    float _expCurve(float x, float ym)
    {
        if (x <= 0) {return 0;}
        if (x >= 1) {return 1;}
        if(ym == 0.5f) {return x;}
        //y = ab^x - a
        //y = (1/b-1) * ((1/ym) - 1)^2 - (1/b-1)
        float b = powf((1 / ym) - 1, 2);
        float a = (1 / (b - 1));
        float y = a * powf(b, x) - a;
        return y;
    }
    
    float _map(float x, float start1, float stop1, float start2, float stop2, float slope)
    {
        float a = (x - start1) / (stop1 - start1); //percentage of x in old range, linear
        a = _expCurve(a, slope); //percentage of x in old range with slope applied
        return start2 + a * (stop2 - start2); //value mapped to new range
    }

#if defined(ANDROID) || defined(__ANDROID__) || defined(__APPLE__)
    double _wallTime(void){
        struct timeval time;
        if (gettimeofday(&time,NULL)){
            //  Handle error
            return 0;
        }
        return (double)time.tv_sec + (double)time.tv_usec * .000001;
    }
    
    double _cpuTime(void){
        return (double)clock() / CLOCKS_PER_SEC;
    }
    
    void _neonFeatures(void)
    {
#if __ARM_NEON
        printv("ARM Neon is supported.\n");
#if __ARM_FEATURE_FMA
        printv("Fused floating-point multiply-accumulate is supported.\n");
#endif
#if __ARM_BIG_ENDIAN
        printv("Target is big-endian.\n");
#else
        printv("Target is little-endian.\n");
#endif
#if __ARM_FEATURE_CLZ
        printv("Count leading zeros feature is supported.\n");
#endif
#if __ARM_NEON_FP
        if(__ARM_NEON_FP == 0x0e)
            printv("Half, single and double precision hardware floating point is available.\n");
        else if(__ARM_NEON_FP == 0x0c)
            printv("Single and double precision hardware floating point is available.\n");
        else if(__ARM_NEON_FP == 0x04)
            printv("Single precision hardware floating point is available.\n");
#endif
#else
        printv("ARM Neon is not supported.\n");
#endif
    }
#endif

#ifdef __cplusplus
}
#endif
