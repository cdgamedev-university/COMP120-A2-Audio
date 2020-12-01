// the usings for the program
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using NAudio;
using NAudio.Wave;
using NAudio.MediaFoundation;
using NAudio.Utils;

namespace Tinkering_Audio {
    /// <summary>
    /// class for handling the audio manipulation
    /// </summary>
    public class AudioManipulation {
        /// <summary>
        /// how to audio manipulation should be initialised
        /// </summary>
        /// <param name="sender">the main form class itself</param>
        public AudioManipulation(TinkeringAudioForm sender) {
            m_sender = sender;
        }

        // declare the member sender for the initial form
        TinkeringAudioForm m_sender;

        #region INDIVIDUAL MANIPULATION

        #region AudioSplicing
        /// <summary>
        /// function to generate a list of ints which splices two audio segments together
        /// </summary>
        /// <param name="audioSample0">the first audio sample</param>
        /// <param name="audioSample1">the second audio sample</param>
        /// <returns>the two spliced audio samples</returns>
        public List<int> AudioSplicing(List<int> audioSample0, List<int> audioSample1) {
            Console.WriteLine("[RUNNING]: Audio Splicing");

            // declare list for the spliced sounds and set it to first audio sample
            List<int> splicedList = audioSample0;

            // add the second audio sample to the end of the list
            splicedList.AddRange(audioSample1);

            // return the list
            return splicedList;
        }
        #endregion

        #region AddingEchos
        /// <summary>
        /// function to add echos
        /// </summary>
        /// <param name="inputList">the sample to add an echo to</param>
        /// <param name="delayInSeconds">the offfset of the echo</param>
        /// <returns>return the sample with its added echo</returns>
        public List<int> AddingEchos(List<int> inputList, int delayInSeconds) {

            // required: 
            // 1 =< t
            // 1 =< (S) sampleRate; 

            // there is an input list s, where the input is extended by t seconds
            // combines input list with delayed copy of itself

            // the duration of the echo added to the end
            int echoDuration = delayInSeconds * m_sender.sampleRate;

            // create a new list for the echo
            List<int> echoList = new List<int>(inputList.Count + echoDuration);

            // run through the input and add the echo to the end
            for (int i = 0; i < inputList.Count + echoDuration; i++) {
                // set the value to 0
                int value = 0;

                // if the current sample is less than the input sample length
                if (i < inputList.Count) {
                    // add the input list at index to the value
                    value += inputList[i];
                }

                // if the echo should be playing
                if (i - echoDuration > 0) {
                    // add the input list at index - echo duration to the value
                    value += inputList[i - echoDuration];
                }

                // add the value to the echo list
                echoList.Add(value);
            }

            Console.WriteLine("[RUNNING]: Add Echoes");
            // return the echo list
            return echoList;
        }
        #endregion

        #region Normalisation
        /// <summary>
        /// this function will normalize the sound which would try to make the volume of the sound even throughout the audio clip
        /// </summary>
        /// <param name="sample">the audio sample to be normalized</param>
        /// <returns>the normalized sound clip</returns>
        public List<int> Normalisation(List<int> sample) {
            Console.WriteLine("[RUNNING]: Normalisation");

            // declare and set the maximum volume of the sample
            int maxAmplitudeOfSample = sample.Max();

            // declare and set the ratio of the amplitude compared the the max aplitude
            int amplitudeRatio = (m_sender.MAX_VALUE - 1) / maxAmplitudeOfSample;

            // run through the values of the sample
            for (int i = 0; i < (sample.Count); i++) {
                // declare and set the normalized sample value
                int normalizedValue = amplitudeRatio * sample[i];

                // set the sample at the i position to the normalized sample value
                sample[i] = normalizedValue;
            }

            // return the sample
            return sample;
        }
        #endregion

        #region Resample
        /// <summary>
        /// a function to resample a audio sample using a factor, audScale, to scale the audio.
        /// </summary>
        /// <param name="audSample"></param>
        /// <param name="audScale"></param>
        /// <returns>the resampled list</returns>
        public List<int> Resample(List<int> audSample, double audScale) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Resample");

            // the modified audioscale 
            double modAudioScale = 1.0 / audScale;

            //declares list for the resampled sound
            List<int> resampledList = new List<int>();

            // if modified audio scale is greater than 1 then
            if (modAudioScale > 1) {
                // while the statement i is less than the length of audSample is true execute loop
                for (int i = 0; i < (audSample.Count); i++) {
                    // declare double value var
                    int value = 0;

                    // while the statement j is less than modified audio scale is true execute loop
                    for (int j = 0; j < modAudioScale; j++) {
                        // audio sample i+j incriment is added to value
                        value += audSample[i + j];
                    }
                    // value is divided by the modified audio scale
                    value = (int)(value / modAudioScale);

                    // adds value onto the resampled list
                    resampledList.Add(value);
                }
            }

            // if modified audio scale is less than 1 then
            else {
                // declaring the index var
                int index = 0;

                // declaring the l var
                double indexLocation = 0.0;

                // declaring m as 1 divided by the audio scale
                double incrementAmount = 1.0 / audScale;

                // while the statement index is less than the length of audiosample do this
                do {
                    // add audio sample[index] to the resampled list
                    resampledList.Add(audSample[index]);

                    // m is added into l
                    indexLocation += incrementAmount;

                    // index is now declared as l
                    index = Convert.ToInt32(indexLocation);

                } while (index < (audSample.Count));
            }
            // return the resampled list
            return resampledList;
        }
        #endregion

        #region Scaling Amplitude
        /// <summary>
        /// this function will scale the amplitude of a audio sample to either make it louder or quieter
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="amplifyingFactor"></param>
        /// <returns>returns the sample with scaled amplitudes</returns>
        public List<int> ScalingAmplitude(List<int> sample, double amplifyingFactor) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Scaling Amplitude");

            // while the statement i is less than the length of audio sample then execute loop
            for(int i =0; i < sample.Count; i++) {
                // declare the value by multiplying the sample's value by the amplitude factor
                int value = (int)(sample[i] * amplifyingFactor);

                // ensure that the value is within range
                value = Math.Max(0, value);
                value = Math.Min(m_sender.MAX_VALUE, value);

                // set the sample at position i to the valye
                sample[i] = value;
            }

            // returns the modified audio through returning the scaled list
            return sample;
        }
        #endregion

        #region Tone Combine
        /// <summary>
        /// this function takes two tones and combines them so they play at the same time
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sample0"></param>
        /// <param name="sample1"></param>
        /// <returns>returns a combinded audio list where two tones are played simultaneously</returns>
        public List<int> ToneCombine(double duration, List<int> sample0, List<int> sample1) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Tone Combine");

            // declaring new list for the two tones to combine in
            List<int> CombinedList = new List<int>();

            int sampleDuration = (int)(duration * m_sender.sampleRate);

            // while the statement i is less than duration multiplied by the sample rate is true execute loop
            for (int i = 0; i < (sampleDuration); i++) {
                if (sample0.Count <= i) {
                    sample0.Add(0);
                }
                if (sample1.Count <= i) {
                    sample1.Add(0);
                }
                CombinedList.Add((sample0[i] + sample1[i]) / 2);
            }
            // returns the modifed combined list
            return CombinedList;
        }
        #endregion

        #region White Noise
        /// <summary>
        /// this function creates white noise, does not require audio sample to edit
        /// </summary>
        /// <param name="time"></param>
        /// <param name="resultantVol"></param>
        /// <returns>returns the white list list</returns>
        public List<int> WhiteNoise(double time, int resultantVol) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: White Noise");

            // declaring new list for the two tones to combine in 
            List<int> WhiteList = new List<int>();

            //create a random variable which generates new random each for loop
            Random rand = new Random();

            // while the statement i is less than time multiplied by the sample rate is true execute loop
            for (int i = 0; i < (time * m_sender.sampleRate); i++) {
                // adds a random number between -1 and 1 to the white list
                WhiteList.Add(rand.Next(-1, 1) * resultantVol * m_sender.MAX_VALUE);
            }
            // returns modifed white list to 
            return WhiteList;
        }
        #endregion

        #endregion

        #region COMPLETE EFFECTS
        public List<int> ApplyVillageEffect(List<int> audioClip) {
            // load sound file first
            // tone combine happy beat with birds/insects
            // normalize sound file
            try {
                if (audioClip == null) {
                    throw new ArgumentNullException();
                }

                Stream clip = IncludedAudio.Cicadia_Sounds;
                List<int> villageBackgroundAudio = m_sender.ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

                villageBackgroundAudio = Normalisation(villageBackgroundAudio);
                villageBackgroundAudio = ScalingAmplitude(villageBackgroundAudio, 0.1);

                audioClip = ToneCombine(30, audioClip, villageBackgroundAudio);
            } catch (ArgumentNullException) {
                m_sender.exceptionHandler.Handle("Error: Argument Null Exception", ExceptionHandler.ExceptionType.NoAudioLoaded_Manipulation);
            } catch (Exception ex) {
                m_sender.exceptionHandler.Handle("Undefined Error: " + ex.ToString(), ExceptionHandler.ExceptionType.UndefinedError);
            }

            return audioClip;
        }
        public List<int> ApplyForestEffect(List<int> audioClip) {
            // load sound file first
            // tone combine wind sound with audio insect sound
            // tone combine wind/insect with loaded audio sound file
            // scale amplitude up slightly on edited spliced audio
            // then finally normalize it to ensure that one spliced audio is louder than the other
            try {
                if (audioClip == null) {
                    throw new ArgumentNullException();
                }

                Stream clip = IncludedAudio.Cicadia_Sounds;
                List<int> forestBackgroundAudio = m_sender.ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));
                
                clip = IncludedAudio.Wind;
                List<int> windAudio = m_sender.ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

                forestBackgroundAudio = Normalisation(forestBackgroundAudio);
                windAudio = Normalisation(windAudio);

                windAudio = ToneCombine(30, windAudio, forestBackgroundAudio);
                windAudio = ScalingAmplitude(windAudio, 0.1);

                audioClip = ScalingAmplitude(audioClip, 4);

                audioClip = ToneCombine(30, windAudio, audioClip);
            } catch (ArgumentNullException) {
                m_sender.exceptionHandler.Handle("Error: Argument Null Exception", ExceptionHandler.ExceptionType.NoAudioLoaded_Manipulation);
            } catch (Exception ex) {
                m_sender.exceptionHandler.Handle("Undefined Error: " + ex.ToString(), ExceptionHandler.ExceptionType.UndefinedError);
            }

            return audioClip;
        }

        public List<int> ApplyCaveEffect(List<int> audioClip) {
            // load sound file first
            // tone combine white noise over audio sample
            // tone combine echo over audio sample
            // normalize final editied audio sample
            try {
                if (audioClip == null) {
                    throw new ArgumentNullException();
                }

                Stream clip = IncludedAudio.Ambience_Cave_Drips;
                List<int> caveDripAudio = m_sender.ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

                caveDripAudio = Normalisation(caveDripAudio);

                audioClip = ToneCombine(30, audioClip, caveDripAudio);

                audioClip = AddingEchos(audioClip, 5);
            } catch (ArgumentNullException) {
                m_sender.exceptionHandler.Handle("Error: Argument Null Exception", ExceptionHandler.ExceptionType.NoAudioLoaded_Manipulation);
            } catch (Exception ex) {
                m_sender.exceptionHandler.Handle("Undefined Error: " + ex.ToString(), ExceptionHandler.ExceptionType.UndefinedError);
            }

            return audioClip;
        }

        public List<int> ApplyOceanEffect(List<int> audioClip) {
            try {
                if (audioClip == null) {
                    throw new ArgumentNullException();
                }

                List<int> whiteNoise = WhiteNoise(2, 1);
                audioClip = AddingEchos(audioClip, 5);
                audioClip = ToneCombine(30, whiteNoise, audioClip);
            } catch (ArgumentNullException) {
                m_sender.exceptionHandler.Handle("Error: Argument Null Exception", ExceptionHandler.ExceptionType.NoAudioLoaded_Manipulation);
            } catch (Exception ex) {
                m_sender.exceptionHandler.Handle("Undefined Error: " + ex.ToString(), ExceptionHandler.ExceptionType.UndefinedError);
            }

            return audioClip;
        }
        #endregion
    }
}
