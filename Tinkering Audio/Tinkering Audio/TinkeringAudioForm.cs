//***************************************************************************\\
//           ██████  ██████  ███    ███ ██████    ██ ██████   ██████         \\
//          ██      ██    ██ ████  ████ ██   ██  ███      ██ ██  ████        \\
//          ██      ██    ██ ██ ████ ██ ██████    ██  █████  ██ ██ ██        \\
//          ██      ██    ██ ██  ██  ██ ██        ██ ██      ████  ██        \\
//           ██████  ██████  ██      ██ ██        ██ ███████  ██████         \\
//                                                                           \\
//    ████████ ██ ███    ██ ██   ██ ███████ ██████  ██ ███    ██  ██████     \\
//       ██    ██ ████   ██ ██  ██  ██      ██   ██ ██ ████   ██ ██          \\
//       ██    ██ ██ ██  ██ █████   █████   ██████  ██ ██ ██  ██ ██   ███    \\
//       ██    ██ ██  ██ ██ ██  ██  ██      ██   ██ ██ ██  ██ ██ ██    ██    \\
//       ██    ██ ██   ████ ██   ██ ███████ ██   ██ ██ ██   ████  ██████     \\
//                                                                           \\
//                     █████  ██    ██ ███████  ██  ██████                   \\
//                    ██   ██ ██    ██ ██    ██ ██ ██    ██                  \\
//                    ███████ ██    ██ ██    ██ ██ ██    ██                  \\
//                    ██   ██ ██    ██ ██    ██ ██ ██    ██                  \\
//                    ██   ██  ██████  ███████  ██  ██████                   \\
//***************************************************************************\\
#region Copyright & License Information
//***************************************************************************\\
// Copyright 2020 Daisy Baker and Hayley Davies                              \\
// Contact: db246020@falmouth.ac.uk or cd230099@falmouth.ac.uk               \\
//                                                                           \\
// Any sample audio distributed with this project is Copyright of Otis Hull  \\
// Contact: oh249978@falmouth.ac.uk                                          \\
//                                                                           \\
// Licensed under <FILL IN LICENSE> (the "License")                          \\
// you may not use this file except in compliance with the License           \\
// You may obtain a copy of the License at:                                  \\
// <URL TO LICENSE>                                                          \\
//                                                                           \\
// Unless required by applicable law or agreed to in writing, software       \\
// distributed under the License is distributed on an "AS IS" BASIS,         \\
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  \\
// See the License for the specific language governing permissions and       \\
// limitations under the License.                                            \\
//***************************************************************************\\
#endregion

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;

namespace TinkeringAudio {
    public partial class TinkeringAudioForm : Form 
    {
        #region DECLARING VARIABLES
        // the sample rate is how many samples taken each second - 44100 because thats how many samples per sec humans can hear
        private readonly int SAMPLE_RATE = 44100;

        // 2 to power of 15 is 32768 - the maximum value we want
        private readonly int MAX_VALUE = (int)Math.Pow(2, 15);

        // double var to hold volume level
        private double volume = 0.08;

        // allows ability to treat function like variable
        private delegate double WaveFunction(double frequency, int position);
        private WaveFunction waveFunction = null;

        // variables to store the wave information
        private WaveOut waveOut = null;
        private IWaveProvider waveProvider = null;

        // variables to store the note information
        private List<double> notes;
        private double[] noteDuration;
        #endregion

        #region FORM INITIALISATION AND LOADS
        // initialise the form
        public TinkeringAudioForm() {
            InitializeComponent();
        }

        // when the form loads
        private void TinkeringAudioForm_Load(object sender, EventArgs e) {
            // set the wave function
            this.waveFunction = SquareWave;

            // populate notes with notes
            notes = PopulateNotes(440, -16, 8, 2);

            // set the possible note durations
            noteDuration = new double[] { 0.15, 0.2, 0.3, 0.4 };
        }
        #endregion

        #region SAVING AND LOADING FILES
        void LoadAudioClip() {
            // create a new file dialog (pop up window to browse windows explorer)
            OpenFileDialog openFileDialog = new OpenFileDialog {
                // set the initial directory to be the C drive
                InitialDirectory = "C:\\",
                // add some filters to the dialog
                Filter = "Supported audio files (*.asf, *.wma, *.wmv, *.aac, *.adts, *.mp3, *.wav)|*.asf;*.wma;*.wmv;*.aac;*.adts;*.mp3;*.wav|" // supported file type
                + "All files (*.*)|*.*",                                                                                                              // all files
                FilterIndex = 1,                // start the filter index at 1 (supported files)
                RestoreDirectory = true,        // enable restoring directory when the dialog is closed
                Title = "Load an audio file..." // change the title to something more fitting
            };

            // open the file dialog, if the OK button is pressed
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Stopwatch s = Stopwatch.StartNew();

                // try to run the following
                try {
                    // set the path to the file name
                    string path = openFileDialog.FileName;

                    // create a new file reader for the file
                    WaveFileReader audioFileReader = new WaveFileReader(path);
                    // create a new wave out
                    waveOut = new WaveOut();
                    // intialize the wave out using the audio file reader
                    waveOut.Init(audioFileReader);
                    // play the wave out
                    waveOut.Play();
                }
                // if there is a format exception
                catch (FormatException) {
                    // set the message of the message box
                    string message = "This file doesn't appear to be a supported audio format. Please choose a different file.";
                    // set the caption of the message box
                    string caption = "Audio Import Error";

                    // set the message box to have only the ok button shown
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    // show the message box with the above details
                    MessageBox.Show(message, caption, buttons);

                    // restart the function
                    LoadAudioClip();
                }
            }
        }

        void SaveAudioClip() {

        }
        #endregion

        #region GENERATE FUNCTIONS
        /// <summary>
        /// function to generate silence
        /// </summary>
        /// <param name="durationInSeconds">how long the silence should play in seconds</param>
        /// <returns>returns notes as a List of ints</returns>
        private List<int> GenerateSilence(double durationInSeconds) {
            // calculate the duration of the sample
            int sampleDuration = (int)(durationInSeconds * SAMPLE_RATE);

            // declare the silence as a new List of ints
            List<int> silence = new List<int>();

            // run through the list and set all samples to 0
            for (int i = 0; i < sampleDuration; i++) {
                silence.Add(0);
            }

            // return the newly generated silence
            return silence;
        }

        /// <summary>
        /// function to generate a random tone
        /// </summary>
        /// <param name="durationInSeconds">how long the tones should play for</param>
        /// <param name="waveFunction">the wave function to generate the sounds</param>
        /// <param name="frequencies">the freequencies to play</param>
        /// <returns>returns a tone as a List of ints</returns>
        private List<int> GenerateTone(double durationInSeconds, WaveFunction waveFunction, double[] frequencies) {
            // calculate the duration of the sample
            int sampleDuration = (int)(durationInSeconds * SAMPLE_RATE);

            // delcare the tone as a new List<int>
            List<int> tone = new List<int>();

            // declare the value with the short type
            short value;

            // run through the duration of the clip
            for (int i = 0; i < sampleDuration; i++) {
                // set value to 0
                value = 0;

                // run through the the frequencies
                for (int j = 0; j < frequencies.Length; j++) 
                {
                    // adjust the value of the tone
                    value += (short)(MAX_VALUE * volume * waveFunction.Invoke(frequencies[j], i));
                }
                // add the value to the list of tone
                tone.Add(BitConverter.GetBytes(value)[0]);
            }
            // return the tone
            return tone;
        }

        /// <summary>
        /// generate a random melody
        /// </summary>
        /// <param name="countOfNotesToPlay">the number of notes to play</param>
        /// <returns>a list of ints for the melody</returns>
        private List<int> GenerateRandomMelody(int countOfNotesToPlay) {
            // generate a new random
            Random prng = new Random();

            // define a new list
            List<int> melody = new List<int>();

            // add .1 seconds of silence at the start
            melody.AddRange(GenerateSilence(0.1));

            // create a new double to store the frequency
            double frequency;

            // run through for the amount of notes to play
            for (int i = 0; i < countOfNotesToPlay; i++) {
                // choose a random frequency
                frequency = GetRandomElement(notes, prng);

                // add the frequency with a random length to melody
                melody.AddRange(GenerateTone(GetRandomElement(noteDuration, prng), this.waveFunction, new double[] {frequency}));
            }

            // return the melody
            return melody;
        }

        /// <summary>
        /// function to generate white noise
        /// </summary>
        /// <param name="durationInSeconds">how long (in seconds) to play the white noise for</param>
        /// <returns>the white noise</returns>
        private List<int> GenerateWhiteNoise(int durationInSeconds) {
            // calculate the duration of the sample
            int sampleDuration = (int)(durationInSeconds * SAMPLE_RATE);

            // create a new random
            Random prng = new Random();

            // create a new list for the noise
            List<int> noise = new List<int>();

            // run through for the sample duration
            for (int i = 0; i < sampleDuration; i++) 
            {
                // generate a random value
                int value = (int)(prng.Next(-1, 1) * volume * MAX_VALUE);

                // add the value to the noise list
                noise.Add(value);
            }

            // return the random noise
            return noise;
        }
        #endregion

        #region WAVE FUNCTIONS

        /// <summary>
        /// get a random element from an array
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="enumerable">the list to choose from</param>
        /// <param name="prng">the pseudo random number generator</param>
        /// <returns></returns>
        private static T GetRandomElement<T>(IEnumerable<T> enumerable, Random prng) 
        {
            // get a random index and return the value of that index
            int index = prng.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }


        /// <summary>
        /// create a list of notes
        /// </summary>
        /// <param name="baseNote">the note which should be used as a multiplier</param>
        /// <param name="startNote">the start note</param>
        /// <param name="endNote">the end note</param>
        /// <param name="increment">how much the list should increment by</param>
        /// <returns>a List of doubles called notes</returns>
        private List<double> PopulateNotes(double baseNote, int startNote, int endNote, int increment) 
        {
            // work out the estimator
            double ESTIMATOR = Math.Pow(2.0, (1.0 / 12.0));

            // create a new list of notes
            List<double> notes = new List<double>();

            // run through the notes, increasing by increment each iteration
            for (int i = startNote; i < endNote; i += increment) 
            {
                // add a new note
                notes.Add(baseNote * Math.Pow(ESTIMATOR, i));
            }

            // return the newly generated notes
            return notes;
        }


        /// <summary>
        /// converts a waveform as int to a waveform
        /// </summary>
        /// <param name="sample">the sample to convert</param>
        /// <param name="sampleRate">the sample rate of the audio</param>
        /// <param name="channelCount">the amount of channels the soundtrack has</param>
        /// <returns>the new wave provider</returns>
        private IWaveProvider convertToWaveProvider16(List<int> sample, int sampleRate, int channelCount) {
            // create a new array of bytes as we are dealing with 16 bit sound
            byte[] byteBuffer = new byte[sample.Count * 2];

            // set the array index to 0
            int byteArrayIndex = 0;
            
            // declare a value
            short value;

            // run through the sample
            for (int i = 0; i < sample.Count; i++) 
            {
                // if the sample value is greater than the max value
                // set the value to the max value
                if (sample[i] > MAX_VALUE) 
                {
                    value = (short)MAX_VALUE;
                } 
                // else if the sample value is less than the minimum value
                // set the value to the min value
                else if (sample[i] < -MAX_VALUE) 
                {
                    value = (short)-MAX_VALUE;
                } 
                // otherwise set the value to the sample value
                else 
                {
                    value = (short)sample[i];
                }

                // add the value to the byte buffer
                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[0];
                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[1];
            }

            // create a new waveprovider using the converted bytes
            IWaveProvider waveProvider = new RawSourceWaveStream(new MemoryStream(byteBuffer), new WaveFormat(sampleRate, 16, channelCount));

            // return the wave provider
            return waveProvider;
        }
        #endregion

        #region WAVE TYPES
        
        // this is a wave which amplitude alternates at a freq between fixed min and max values
        /// <summary>
        /// return 1 or -1 depending on the value of the wave
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns></returns>
        private double SquareWave(double frequency, int position) {
            // calculate the value
            double value = Math.Sin(2.0 * Math.PI * frequency * (position / (double) SAMPLE_RATE));

            // if the value is greater than 0, return 1
            if (value > 0) 
            {
                return 1.0;
            } 
            // otherwise, return -1
            else 
            {
                return -1.0;
            }
        }

        // a smooth wave with peridoic oscillation
        /// <summary>
        /// return a smooth wave
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns>the value</returns>
        private double SineWave(double frequency, int position) 
        {
            // generate the frequency from the sin wave
            return Math.Sin(2.0 * Math.PI * frequency * (position / (double)SAMPLE_RATE));
        }

        // a non-sinusoidal wave with a triangular shape
        /// <summary>
        /// create the wave in the shape of a triangle wave
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns>returns a value manipulated to look like a triangle wave</returns>
        private double TriangleWave(double frequency, int position) 
        {
            // calculate the value and return it
            double value = ((2.0 * MAX_VALUE * volume) / Math.PI) *Math.Asin(Math.Sin(2.0 * Math.PI * frequency * position));
            return value;
        }

        // another non-sinusoidal wave which ramps upwards, drops down and repeats
        /// <summary>
        /// create the wave in the shape of a sawtooth
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns>the manipulated value</returns>
        private double SawtoothWave(double frequency, int position) 
        {
            // calculate and return the value
            double value = (-(2.0 * MAX_VALUE * volume) / Math.PI) * Math.Atan((1 / Math.Tan(Math.PI * frequency * position)));
            return value;
        }
        #endregion

        #region BUTTON FUNCTIONS
        // button for the 
        /// <summary>
        /// generate a melody when the generate melody button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GenerateMelody_Click(object sender, EventArgs e) {
            // set the wave provider generated by the random melody function, converted to bytes
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), SAMPLE_RATE, 1);

            // create a new wave out
            waveOut = new WaveOut();
            // initialise the wave out using the wave provider
            waveOut.Init(waveProvider);
            // play the sound
            waveOut.Play();
        }

        /// <summary>
        /// save a melody when the save melody button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveMelody_Click(object sender, EventArgs e) {
            // set the wave provider generated by the random melody function, converted to bytes
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), SAMPLE_RATE, 1);

            // get the file name
            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");

            // sve the wave to a file
            WaveFileWriter.CreateWaveFile(filename, waveProvider);
        }

        /// <summary>
        /// generate white noise when the generate white noise button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GenerateWhiteNoise_Click(object sender, EventArgs e) 
        {
            // generate a new wave out fucntion
            WaveOut waveOut = new WaveOut();

            // initialse the wave function using the white noise generator copnverted to bytes
            waveOut.Init(convertToWaveProvider16(GenerateWhiteNoise(12), SAMPLE_RATE, 1));

            // play the wave out
            waveOut.Play();
        }
        #endregion

        #region MELODY BUTTONS

        #region AudioSplicing
        /// <summary>
        /// function to generate a list of ints which splices two audio segments together
        /// </summary>
        /// <param name="audioSample0">the first audio sample</param>
        /// <param name="audioSample1">the second audio sample</param>
        /// <returns>the two spliced audio samples</returns>
        private List<int> AudioSplicing (List<int> audioSample0, List<int> audioSample1)
        {
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
        private List<int> AddingEchos (List<int> inputList, int delayInSeconds)
        {
            // required: 
            // 1 =< t
            // 1 =< (S) SAMPLE_RATE; 

            // there is an input list s, where the input is extended by t seconds
            // combines input list with delayed copy of itself

            // create a new list for the echo
            List<int> echoList = new List<int>();

            // the duration of the echo added to the end
            int echoDuration = delayInSeconds * SAMPLE_RATE;

            // run through the input and add the echo to the end
            for (int i = 0; i < (inputList.Count) + echoDuration; i++)
            {
                // set the value to 0
                int value = 0;

                // if the current sample is less than the input sample length
                if (i < inputList.Count)
                {
                    // add the input list at index to the value
                    value += inputList[i];
                }

                // if the echo should be playing
                if (i - echoDuration > 0)
                {
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
        private List<int> Normalisation (List<int> sample)
        {
            // declare and set the maximum volume of the sample
            int maxAmplitudeOfSample = sample.Max();

            // declare and set the ratio of the amplitude compared the the max aplitude
            int amplitudeRatio = (MAX_VALUE - 1) / maxAmplitudeOfSample;

            // run through the values of the sample
            for (int i = 0; i < (sample.Count); i++)
            {
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
        /// <param name="audSamp"></param>
        /// <param name="audScale"></param>
        /// <returns>the resampled list</returns>
        private List<double> Resample (List<double> audSample, double audScale)
        {
            // the modified audioscale 
            double modAudScale = 1.0 / audScale;

            //declares list for the resampled sound
            List<double> resampledList = new List<double>();

            // if modified audio scale is greater than 1 then
            if (modAudScale > 1)
            {
                // while the statement i is less than the length of audSample is true execute loop
                for (int i = 0; i < (audSample.Count); i++)
                {
                    // declare double value var
                    double value = 0;

                    // while the statement j is less than modified audio scale is true execute loop
                    for (int j = 0; j < modAudScale; j++)
                    {
                        // audio sample i+j incriment is added to value
                        value += audSample[i + j];
                    }
                    // value is divided by the modified audio scale
                    value = value / modAudScale;

                    // adds value onto the resampled list
                    resampledList.Add(value);
                }
            }
            
            // if modified audio scale is less than 1 then
            else
            {
                // declaring the index var
                int index = 0;

                // declaring the l var
                double l = 0.0;

                // declaring m as 1 divided by the audio scale
                double m = 1.0 / audScale;

                // while the statement index is less than the length of audiosample do this
                do
                {
                    // add audio sample[index] to the resampled list
                    resampledList.Add(audSample[index]);

                    // m is added into l
                    l = l + m;

                    // index is now declared as l
                    index = Convert.ToInt32(l); 

                } while (index < (audSample.Count));
            }
            // return the resampled list
            return resampledList;
        }
        #endregion

        // ERROR in scaling amplitude to fix
        #region Scaling Amplitude
        /// <summary>
        /// this function will scale the amplitude of a audio sample to either make it louder or quieter
        /// </summary>
        /// <param name="audSample"></param>
        /// <param name="ampFactor"></param>
        /// <returns>returns a newly scaled list</returns>
        private List<int> ScalingAmplitude(List<int> audSample, int ampFactor)
        {
            // delcares a new list to hold the scaled audio
            List<int> ScaledList = new List<int>();

            // while the statement i is less than the length of audio sample then execute loop
            for (int i = 0; i < (audSample.Count); i++)
            {
                //declares variable v as audio sample instance i multiplied by the amplitude factor
                int v = audSample[i] * ampFactor;

                // v = Math.Max((Math.Max(v)), v);

                // v = Math.Min((Math.Min(v)), v);

                // adds v to the scaled list
                ScaledList.Add(v);
            }

            // returns the modified audio through returning the scaled list
            return ScaledList;
        }
        #endregion

        #region Tone Combine
        /// <summary>
        /// this function takes two tones and combines them so they play at the same time
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="factorFreq"></param>
        /// <param name="w"></param>
        /// <returns>returns a combinded audio list where two tones are played simultaneously</returns>
        private List<double> ToneCombine(double duration, List<double> Frequency, double w)
        {
            // This algorithm can be used to make the amplitude of a given sequence, s by some factor f.
            // d is the duration 
            // w is ???

            // declaring new list for the two tones to combine in
            List<double> CombinedList = new List<double>();

            // while the statement i is less than duration multiplied by the sample rate is true execute loop
            for (int i = 0; i < (duration * SAMPLE_RATE); i++)
            {
                // declares the v variable
                int value = 0;
                
                // while the statement i is less than the length of factor freq is true exectute loop
                for (int j = 0; i < (Frequency.Count); j++)
                {
                    // wavefunction factorfreq[j] and i is added to value
                    value += Convert.ToInt32(waveFunction(Frequency[j], i));
                }
                // adds value into the combined tone list
                CombinedList.Add(value);
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
        private List<double> WhiteNoise (double time, double resultantVol)
        {
            // declaring new list for the two tones to combine in 
            List<double> WhiteList = new List<double>();

            // while the statement i is less than time multiplied by the sample rate is true execute loop
            for (int i = 0; i < (time * SAMPLE_RATE); i++)
            {
                //create a random variable which generates new random each for loop
                Random rand = new Random();

                // adds a random number between -1 and 1 to the white list
                WhiteList.Add(rand.Next(-1, 1));
            }
            // returns modifed white list to 
            return WhiteList;
        }
        #endregion

        // must create 4 new melodies using waves to create ambient music for 

        private void Villagebtn_Click(object sender, EventArgs e)
        {
            GenerateSilence(1);
        }


        private void Forestbtn_Click(object sender, EventArgs e)
        {

        }


        private void Cavebtn_Click(object sender, EventArgs e)
        {

        }


        private void Oceanbtn_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void btn_LoadAudioFile_Click(object sender, EventArgs e) {
            LoadAudioClip();
        }
    }
}
