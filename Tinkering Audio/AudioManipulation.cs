//***************************************************************************************************\\
//                       ██████  ██████  ███    ███ ██████   ██ ██████   ██████                      \\
//                      ██      ██    ██ ████  ████ ██   ██ ███      ██ ██  ████                     \\
//                      ██      ██    ██ ██ ████ ██ ██████   ██  █████  ██ ██ ██                     \\
//                      ██      ██    ██ ██  ██  ██ ██       ██ ██      ████  ██                     \\
//                       ██████  ██████  ██      ██ ██       ██ ███████  ██████                      \\
//                                                                                                   \\
//                  ████████ ██ ███    ██ ██   ██ ███████ ██████  ██ ███    ██  ██████               \\
//                     ██    ██ ████   ██ ██  ██  ██      ██   ██ ██ ████   ██ ██                    \\
//                     ██    ██ ██ ██  ██ █████   █████   ██████  ██ ██ ██  ██ ██   ███              \\
//                     ██    ██ ██  ██ ██ ██  ██  ██      ██   ██ ██ ██  ██ ██ ██    ██              \\
//                     ██    ██ ██   ████ ██   ██ ███████ ██   ██ ██ ██   ████  ██████               \\
//                                                                                                   \\
//           █████  ██    ██ ██████  ██  ██████          █████  ██    ██ ██████  ██  ██████          \\
//          ██   ██ ██    ██ ██   ██ ██ ██    ██ ██     ██   ██ ██    ██ ██   ██ ██ ██    ██         \\
//          ███████ ██    ██ ██   ██ ██ ██    ██        ███████ ██    ██ ██   ██ ██ ██    ██         \\
//          ██   ██ ██    ██ ██   ██ ██ ██    ██ ██     ██   ██ ██    ██ ██   ██ ██ ██    ██         \\
//          ██   ██  ██████  ██████  ██  ██████         ██   ██  ██████  ██████  ██  ██████          \\
//                                                                                                   \\
//  ███    ███  █████  ███    ██ ██ ██████  ██    ██ ██       █████  ████████ ██  ██████  ███    ██  \\
//  ████  ████ ██   ██ ████   ██ ██ ██   ██ ██    ██ ██      ██   ██    ██    ██ ██    ██ ████   ██  \\
//  ██ ████ ██ ███████ ██ ██  ██ ██ ██████  ██    ██ ██      ███████    ██    ██ ██    ██ ██ ██  ██  \\
//  ██  ██  ██ ██   ██ ██  ██ ██ ██ ██      ██    ██ ██      ██   ██    ██    ██ ██    ██ ██  ██ ██  \\
//  ██      ██ ██   ██ ██   ████ ██ ██       ██████  ███████ ██   ██    ██    ██  ██████  ██   ████  \\
//                                                                                                   \\
// Copyright (c) 2020 Daisy Baker and Hayley Davies                                                  \\
// Contact: db246020@falmouth.ac.uk or cd230099@falmouth.ac.uk                                       \\
//***************************************************************************************************\\

// the packages for the program
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// assign class under the Tinkering_Audio namespace
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
            // write that the Audio Splicing sample is running
            Console.WriteLine("[RUNNING]: Audio Splicing");

            // add the second audio sample to the end of the list
            audioSample0.AddRange(audioSample1);

            // return the list
            return audioSample0;
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
            // write that the adding echos function is running
            Console.WriteLine("[RUNNING]: Add Echoes");

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
            // write that the normalisation function is running
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
        /// <param name="sample">the sample to be resampled</param>
        /// <param name="scaleFactor">the factor the sample should be manipulated by</param>
        /// <returns>the resampled list</returns>
        public List<int> Resample(List<int> sample, double scaleFactor) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Resample");

            // the modified audioscale 
            double modAudioScale = 1.0 / scaleFactor;

            //declares list for the resampled sound
            List<int> resampledList = new List<int>();

            // if modified audio scale is greater than 1 then
            if (modAudioScale > 1) {
                // while the statement i is less than the length of audSample is true execute loop
                for (int i = 0; i < (sample.Count); i++) {
                    // declare double value var
                    int value = 0;

                    // while the statement j is less than modified audio scale is true execute loop
                    for (int j = 0; j < modAudioScale; j++) {
                        // audio sample i+j incriment is added to value
                        value += sample[i + j];
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
                double incrementAmount = 1.0 / scaleFactor;

                // while the statement index is less than the length of audiosample do this
                do {
                    // add audio sample[index] to the resampled list
                    resampledList.Add(sample[index]);

                    // m is added into l
                    indexLocation += incrementAmount;

                    // index is now declared as l
                    index = Convert.ToInt32(indexLocation);

                } while (index < (sample.Count));
            }
            // return the resampled list
            return resampledList;
        }
        #endregion

        #region Scaling Amplitude
        /// <summary>
        /// this function will scale the amplitude of a audio sample to either make it louder or quieter
        /// </summary>
        /// <param name="sample">the sample that will be manipulated</param>
        /// <param name="amplifyingFactor">the factor to amplify the wave by</param>
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
        /// <param name="duration">the duration of the tone to combine</param>
        /// <param name="sample0">the first sample</param>
        /// <param name="sample1">the second sample</param>
        /// <returns>returns a combinded audio list where two tones are played simultaneously</returns>
        public List<int> ToneCombine(double duration, List<int> sample0, List<int> sample1) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Tone Combine");

            // declaring new list for the two tones to combine in
            List<int> CombinedList = new List<int>();

            if (sample0 != null && sample1 != null) {
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
            }

            // returns the modifed combined list
            return CombinedList;
        }
        #endregion

        #region White Noise
        /// <summary>
        /// this function creates white noise, does not require audio sample to edit
        /// </summary>
        /// <param name="duration">the duration of the white noise that will be generated</param>
        /// <param name="resultantVol">the volume of the white noise generated</param>
        /// <returns>returns the white list list</returns>
        public List<int> WhiteNoise(double duration, int resultantVol) {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: White Noise");

            // declaring new list for the two tones to combine in 
            List<int> WhiteList = new List<int>();

            //create a random variable which generates new random each for loop
            Random rand = new Random();

            // while the statement i is less than time multiplied by the sample rate is true execute loop
            for (int i = 0; i < (duration * m_sender.sampleRate); i++) {
                // adds a random number between -1 and 1 to the white list
                WhiteList.Add(rand.Next(-1, 1) * resultantVol * m_sender.MAX_VALUE);
            }
            // returns modifed white list to 
            return WhiteList;
        }
        #endregion

        #endregion

        #region COMPLETE EFFECTS
        /// <summary>
        /// function which applys an audio splice to the chosen clip
        /// </summary>
        /// <param name="audioClip">the starting clip to add a splice to the end of</param>
        /// <returns>returns an audio clip with more spliced onto the end</returns>
        public List<int> ApplyAudioSplice(List<int> audioClip) {
            // load an audio clip
            byte[] loadedAudioBytes = m_sender.audioFileIO.LoadAudioClip();

            // convert the byte[] to an int<List>
            List<int> loadedAudioInts = ConvertToListInt(loadedAudioBytes);

            // splice the two audio clips together
            audioClip = AudioSplicing(audioClip, loadedAudioInts);

            // return the new audio clip
            return audioClip;
        }

        /// <summary>
        /// function to apply the village effect to the audio clip
        /// </summary>
        /// <param name="audioClip">the audio clip to apply the village effect to</param>
        /// <returns>the manipulated audio clip</returns>
        public List<int> ApplyVillageEffect(List<int> audioClip) {
            // load the cicadia sounds file and convert it to a list of ints
            Stream clip = IncludedAudio.Cicadia_Sounds;
            List<int> villageBackgroundAudio = ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

            // apply a normalisation and scaling amplitude effect to the audio loaded above
            villageBackgroundAudio = Normalisation(villageBackgroundAudio);
            villageBackgroundAudio = ScalingAmplitude(villageBackgroundAudio, 0.1);

            // combine the tones in audio clip and the village background
            audioClip = ToneCombine(30, audioClip, villageBackgroundAudio);

            // return the manipulated audio clip
            return audioClip;
        }

        /// <summary>
        /// function to apply the forest effect to the audio clip
        /// </summary>
        /// <param name="audioClip">the audio clip to apply the forest effect to</param>
        /// <returns>the manipulated audio clip</returns>
        public List<int> ApplyForestEffect(List<int> audioClip) {
            // load the cicadia sounds file and convert it to a list of ints
            Stream clip = IncludedAudio.Cicadia_Sounds;
            List<int> forestBackgroundAudio = ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

            // load the wind sounds file and convert it to a list of ints
            clip = IncludedAudio.Wind;
            List<int> windAudio = ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

            // apply normalization to the forest background
            forestBackgroundAudio = Normalisation(forestBackgroundAudio);

            // apply normalization to the wind
            windAudio = Normalisation(windAudio);

            // combine the two files then scale the amplitude of the combined sound
            windAudio = ToneCombine(30, windAudio, forestBackgroundAudio);
            windAudio = ScalingAmplitude(windAudio, 0.1);

            // scale the audio clip's amplitude and combine it with the background audio
            audioClip = ScalingAmplitude(audioClip, 4);
            audioClip = ToneCombine(30, windAudio, audioClip);

            // return the manipulated audio clip
            return audioClip;
        }

        /// <summary>
        /// function to apply the cave effect to the audio clip
        /// </summary>
        /// <param name="audioClip">the audio clip to apply the cave effect to</param>
        /// <returns>the manipulated audio clip</returns>
        public List<int> ApplyCaveEffect(List<int> audioClip) {
            // load the cave drip sounds file and convert it to a list of ints
            Stream clip = IncludedAudio.Ambience_Cave_Drips;
            List<int> caveDripAudio = ConvertToListInt(m_sender.audioFileIO.DecodeAudioClip(clip));

            // apply normalization to the cave drip sound
            caveDripAudio = Normalisation(caveDripAudio);

            // combine the cave drip and audio clip and add an echo
            audioClip = ToneCombine(30, audioClip, caveDripAudio);
            audioClip = AddingEchos(audioClip, 5);

            // return the manipulated audio clip
            return audioClip;
        }

        /// <summary>
        /// function to apply the ocean effect to the audio clip
        /// </summary>
        /// <param name="audioClip">the audio clip to apply the ocean effect to</param>
        /// <returns>the manipulated audio clip</returns>
        public List<int> ApplyOceanEffect(List<int> audioClip) {
            // generate white noise
            List<int> whiteNoise = WhiteNoise(30, 1);
            
            // add an echo to the audio clip and then combine it with the white noise
            audioClip = AddingEchos(audioClip, 5);
            audioClip = ToneCombine(30, whiteNoise, audioClip);

            // resample the audio clip
            audioClip = Resample(audioClip, 0.8);

            // return the manipulated audio clip
            return audioClip;
        }
        #endregion

        #region CONVERSION TO INT LIST
        /// <summary>
        /// converts a byte list to an int list
        /// </summary>
        /// <param name="waveByte">the byte list of the wave</param>
        /// <returns></returns>
        public List<int> ConvertToListInt(byte[] waveByte) {
            // create a new int list to store the wave
            List<int> wave = new List<int>();

            // calculate the length of the wave @ 16 bits
            int waveLength = waveByte.Length / 2;

            // set the byte array index to 0
            int byteArrayIndex = 0;

            // create a new value for the bytes to be stored
            byte[] value = new byte[2];

            // run through the length of the int wave
            for (int i = 0; i < waveLength; i++) {
                // set values 0 and 1 based off the byte values
                value[0] = waveByte[byteArrayIndex++];
                value[1] = waveByte[byteArrayIndex++];

                // convert the value to 16 bit int and add that to the wave
                wave.Add(BitConverter.ToInt16(value, 0));
            }

            // return the new wave as an int list
            return wave;
        }
        #endregion
    }
}
