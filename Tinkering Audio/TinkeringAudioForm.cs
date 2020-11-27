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
// Copyright (c) 2020 Daisy Baker and Hayley Davies                          \\
// Contact: db246020@falmouth.ac.uk or cd230099@falmouth.ac.uk               \\
//                                                                           \\
// Most sample audio distributed with this project is Copyright of Otis Hull \\
// Contact: oh249978@falmouth.ac.uk                                          \\
//                                                                           \\
// Licensed under GNU GENERAL PUBLIC LICENSE (the "License")                 \\
// you may not use this file except in compliance with the License           \\
// You may obtain a copy of the License at:                                  \\
// https://www.gnu.org/licenses/gpl-3.0.en.html                              \\
//                                                                           \\
// Unless required by applicable law or agreed to in writing, software       \\
// distributed under the License is distributed on an "AS IS" BASIS,         \\
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  \\
// See the License for the specific language governing permissions and       \\
// limitations under the License.                                            \\
//                                                                           \\
// For this project we decided on the GNU GENERAL PUBLIC LICENSE. This is    \\
// because the GNU license is intended for open-source software that         \\
// guarantees end users the freedom to run, study, share and modify the      \\
// software as they please. However, for both the user's and authors,        \\
// it is required that modified versions be marked as changed so  that any   \\
// problems will not be attributed incorrectly to the author. The second     \\
// reason being that the GNU prohibits any closed-soure software using       \\
// this source code this so it can remain free to everyone, and when         \\
// referring to the software as free, we are referring to the freedom        \\
// of the software.                                                          \\
//***************************************************************************\\
#endregion

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

/// <summary>
/// the namespace which stores all the code for the tinkering audio project
/// </summary>
namespace TinkeringAudio {
    /// <summary>
    /// the form class itself
    /// </summary>
    public partial class TinkeringAudioForm : Form {

        // the different types of wave that can be used to generate audio
        public enum WaveType {
            SquareWave,
            SineWave,
            TriangleWave,
            SawtoothWave
        }

        // the differently supported bitrates
        public enum Bitrates {
            kbit32 = 32000,
            kbit96 = 96000,
            kbit128 = 128000,
            kbit160 = 160000,
            kbit192 = 192000,
            kbit256 = 256000,
            kbit320 = 320000
        }

        #region DECLARING VARIABLES
        // the sample rate is how many samples taken each second
        public int sampleRate = 16000;

        // the number of channels the audio has
        public int channelCount = 1;

        // 2 to power of 15 is 32768 - the maximum value we want
        public readonly int MAX_VALUE = (int)Math.Pow(2, 15);

        // double var to hold volume level
        private double volume = 0.8;

        // allows ability to treat function like variable
        public delegate double WaveFunction(double frequency, int position);
        public WaveFunction waveFunction = null;

        // variables to store the wave information
        private WaveOut waveOut = null;
        private IWaveProvider waveProvider = null;

        // variables to store the note information
        private List<double> notes;
        private double[] noteDuration;

        // the audio file io class (for saving and loading)
        private AudioFileIO audioFileIO;

        // the audio manipulation class (for manipulating the audio)
        private AudioManipulation audioManipulation;

        // information to store the loaded wave
        private byte[] loadedWaveByte;
        private List<int> loadedWaveInt;

        // save information
        private Bitrates save_bitrate = Bitrates.kbit192;
        #endregion

        #region FORM INITIALISATION AND LOADS
        // initialise the form
        public TinkeringAudioForm() {
            InitializeComponent();

            // create a new audio file io directing to this class instance as the sender
            audioFileIO = new AudioFileIO(sender: this);

            // create a new audio manipulation directing to this class instance as the sender
            audioManipulation = new AudioManipulation(sender: this);
        }

        // when the form loads
        private void TinkeringAudioForm_Load(object sender, EventArgs e) {
            // set the wave function
            waveFunction = SquareWave;

            // populate notes with notes
            notes = PopulateNotes(440, -16, 8, 2);

            // set the possible note durations
            noteDuration = new double[] { 0.15, 0.2, 0.3, 0.4 };

            // set the data source for the wave type
            cbox_WaveType.DataSource = Enum.GetValues(typeof(WaveType));

            // get the values from the Bitrates enum
            int[] int_bitrates = (int[])Enum.GetValues(typeof(Bitrates));

            // create a new list of strings
            List<string> bitrates = new List<string>();

            // for all of teh bitrates, add an element to display in Kbits/s
            foreach (int i in int_bitrates) {
                bitrates.Add((i / 1000).ToString() + " Kbits/s");
            }

            // set the data source as strings
            cbox_Bitrate.DataSource = bitrates;
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
            int sampleDuration = (int)(durationInSeconds * sampleRate);

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
            int sampleDuration = (int)(durationInSeconds * sampleRate);

            // delcare the tone as a new List<int>
            List<int> tone = new List<int>();

            // declare the value with the short type
            short value;

            // run through the duration of the clip
            for (int i = 0; i < sampleDuration; i++) {
                // set value to 0
                value = 0;

                // run through the the frequencies
                for (int j = 0; j < frequencies.Length; j++) {
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
                melody.AddRange(GenerateTone(GetRandomElement(noteDuration, prng), this.waveFunction, new double[] { frequency }));
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
            int sampleDuration = (int)(durationInSeconds * sampleRate);

            // create a new random
            Random prng = new Random();

            // create a new list for the noise
            List<int> noise = new List<int>();

            // run through for the sample duration
            for (int i = 0; i < sampleDuration; i++) {
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
        private static T GetRandomElement<T>(IEnumerable<T> enumerable, Random prng) {
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
        private List<double> PopulateNotes(double baseNote, int startNote, int endNote, int increment) {
            // work out the estimator
            double ESTIMATOR = Math.Pow(2.0, (1.0 / 12.0));

            // create a new list of notes
            List<double> notes = new List<double>();

            // run through the notes, increasing by increment each iteration
            for (int i = startNote; i < endNote; i += increment) {
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
            try {
                if (sample == null) {
                    throw new ArgumentNullException();
                }
                // create a new array of bytes as we are dealing with 16 bit sound
                byte[] byteBuffer = new byte[sample.Count * 2];

                // set the array index to 0
                int byteArrayIndex = 0;

                // declare a value
                short value;

                // run through the sample
                for (int i = 0; i < sample.Count; i++) {
                    // if the sample value is greater than the max value
                    // set the value to the max value
                    if (sample[i] > MAX_VALUE) {
                        value = (short)MAX_VALUE;
                    }
                    // else if the sample value is less than the minimum value
                    // set the value to the min value
                    else if (sample[i] < -MAX_VALUE) {
                        value = (short)-MAX_VALUE;
                    }
                    // otherwise set the value to the sample value
                    else {
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
            } catch (ArgumentNullException) {
                // call the exception handler to deal with the exception
                TinkeringAudioExceptionHandler.ExceptionHandler("Argument Null Exception: No Audio Loaded", TinkeringAudioExceptionHandler.ExceptionType.NoAudioLoaded);
            } catch (Exception ex) {
                // call the exception handler to deal with the exception
                TinkeringAudioExceptionHandler.ExceptionHandler(ex.ToString(), TinkeringAudioExceptionHandler.ExceptionType.UndefinedError);
            }

            // return nothing
            return null;
        }

        /// <summary>
        /// converts a byte list to an int list
        /// </summary>
        /// <param name="waveByte">the byte list of the wave</param>
        /// <returns></returns>
        private List<int> ConvertToListInt(byte[] waveByte) {
            // calculate the length of the wave @ 16 bits
            int waveLength = waveByte.Length / 2;

            // create a new int list to store the wave
            List<int> wave = new List<int>();

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

        #region AUDIO PLAYBACK
        /// <summary>
        /// function to make the audio play
        /// </summary>
        void PlayAudio() {
            // if the wave out is set to null
            if (waveOut == null) {
                // display a box to the user
                TinkeringAudioExceptionHandler.ExceptionHandler("Null Reference Exception: No Audio Loaded", TinkeringAudioExceptionHandler.ExceptionType.NoAudioLoaded_Playback);
            }
            // if the wave out is not null
            else {
                // play the sound
                waveOut.Play();
            }
        }

        /// <summary>
        /// function to make the audio stop
        /// </summary>
        void StopAudio() {
            // if the wave out isn't null
            if (waveOut != null) {
                // if the waveout is playing
                if (waveOut.PlaybackState == PlaybackState.Playing) {
                    // stop the waveout
                    waveOut.Stop();
                }
            }
        }

        /// <summary>
        /// function to make the audio pause
        /// </summary>
        void PauseAudio() {
            // if the wave out isn't null
            if (waveOut != null) {
                // if the waveout is playing
                if (waveOut.PlaybackState == PlaybackState.Playing) {
                    // stop the waveout
                    waveOut.Pause();
                }
            }
        }

        void GenerateWaveOut() {
            if (waveOut != null) {
                waveOut.Dispose();
            }
            // create the wave provider using the loaded wave
            waveProvider = convertToWaveProvider16(loadedWaveInt, sampleRate, channelCount);

            // create a new wave out
            waveOut = new WaveOut();

            // initialize the wave out using the wave provider
            waveOut.Init(waveProvider);
        }
        #endregion

        #region WAVE TYPES
        /// <summary>
        /// function to set the wave type
        /// </summary>
        /// <param name="waveType">the wavetype as an enum (which can be indexed by the dropdown)</param>
        /// <returns>the new wave</returns>
        WaveFunction SetWaveFunction(WaveType waveType) {
            // delcare a new member wave function
            WaveFunction m_waveFunction = null;

            // switch the wave type
            switch(waveType) {
                // if square wave, set to square wave
                case WaveType.SquareWave:
                    m_waveFunction = SquareWave;
                    break;
                // if sine wave, set to sine wave
                case WaveType.SineWave:
                    m_waveFunction = SineWave;
                    break;
                // if triangle wave, set to triangle wave
                case WaveType.TriangleWave:
                    m_waveFunction = TriangleWave;
                    break;
                // if sawtooth wave, set to sawtooth wave
                case WaveType.SawtoothWave:
                    m_waveFunction = SawtoothWave;
                    break;
            }

            // write to the console which wave has been chosen
            Console.WriteLine("Wave type changed to: {0}", waveType.ToString());

            // return the member wave function
            return m_waveFunction;
        }

        // this is a wave which amplitude alternates at a freq between fixed min and max values
        /// <summary>
        /// return 1 or -1 depending on the value of the wave
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns></returns>
        private double SquareWave(double frequency, int position) {
            // calculate the value
            double value = Math.Sin(2.0 * Math.PI * frequency * (position / (double)sampleRate));

            // if the value is greater than 0, return 1
            if (value > 0) {
                return 1.0;
            }
            // otherwise, return -1
            else {
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
        private double SineWave(double frequency, int position) {
            // generate the frequency from the sin wave
            return Math.Sin(2.0 * Math.PI * frequency * (position / (double)sampleRate));
        }

        // a non-sinusoidal wave with a triangular shape
        /// <summary>
        /// create the wave in the shape of a triangle wave
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns>returns a value manipulated to look like a triangle wave</returns>
        private double TriangleWave(double frequency, int position) {
            // calculate the value and return it
            double value = ((2.0 * MAX_VALUE * volume) / Math.PI) * Math.Asin(Math.Sin(2.0 * Math.PI * frequency * position));
            return value;
        }

        // another non-sinusoidal wave which ramps upwards, drops down and repeats
        /// <summary>
        /// create the wave in the shape of a sawtooth
        /// </summary>
        /// <param name="frequency">the frequency of the wave</param>
        /// <param name="position">the position of the sample</param>
        /// <returns>the manipulated value</returns>
        private double SawtoothWave(double frequency, int position) {
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
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), sampleRate, 1);

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
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), sampleRate, 1);

            // get the file name
            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");

            // sve the wave to a file
            WaveFileWriter.CreateWaveFile(filename, waveProvider);

            Console.WriteLine(filename);
        }

        /// <summary>
        /// generate white noise when the generate white noise button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GenerateWhiteNoise_Click(object sender, EventArgs e) {
            // generate a new wave out fucntion
            WaveOut waveOut = new WaveOut();

            // initialse the wave function using the white noise generator copnverted to bytes
            waveOut.Init(convertToWaveProvider16(GenerateWhiteNoise(12), sampleRate, 1));

            // play the wave out
            waveOut.Play();
        }

        /// <summary>
        /// function to control when the load audio file button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadAudioFile_Click(object sender, EventArgs e) {
            // load the wave as bytes
            loadedWaveByte = audioFileIO.LoadAudioClip();

            // if the loaded wave isnt null then convert the wave to a List<int> (as the other manipulation functions require this)
            if (loadedWaveByte != null) {
                loadedWaveInt = ConvertToListInt(loadedWaveByte);

                GenerateWaveOut();
            }

            // write to the console the channel count and sample rate
            Console.WriteLine("Channel Count: {0}, Sample Rate: {1}", channelCount, sampleRate);
        }

        /// <summary>
        /// function to cotnrol when the save audio button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAudioFile_Click(object sender, EventArgs e) {
            // create the wave provider using the loaded wave
            waveProvider = convertToWaveProvider16(loadedWaveInt, sampleRate, channelCount);

            // if the wave provider isn't null
            if (waveProvider != null) {
                // save the audio clip using the wave provider
                audioFileIO.SaveAudioClip(waveProvider, (int)save_bitrate);
            }
        }

        /// <summary>
        /// function to control when the play button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PlayAudio_Click(object sender, EventArgs e) {
            // play the audio
            PlayAudio();
        }

        /// <summary>
        /// function to handle when the stop button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StopAudio_Click(object sender, EventArgs e) {
            // stop the audio
            StopAudio();
        }

        /// <summary>
        /// function to handle when the pause button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PauseAudio_Click(object sender, EventArgs e) {
            // pause the audio
            PauseAudio();
        }
        #endregion

        #region AMBIENCE BUTTONS
        /// <summary>
        /// function to handle when the Village button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Villagebtn_Click(object sender, EventArgs e) {
            // load sound file first
            // tone combine happy beat with birds/insects
            // normalize sound file
        }

        /// <summary>
        /// function to handle when the Forest button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Forestbtn_Click(object sender, EventArgs e) {
            // load sound file first
            // tone combine wind sound with audio insect sound
            // tone combine wind/insect with loaded audio sound file
            // scale amplitude up slightly on edited spliced audio
            // then finally normalize it to ensure that one spliced audio is louder than the other
        }

        /// <summary>
        /// function to handle when the Cave button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cavebtn_Click(object sender, EventArgs e) {
            // load sound file first
            // tone combine white noise over audio sample
            // tone combine echo over audio sample
            // normalize final editied audio sample
        }

        /// <summary>
        /// function to handle when the Ocean button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Oceanbtn_Click(object sender, EventArgs e) {
            try
            {
                if (loadedWaveInt == null) {
                    throw new ArgumentNullException();
                }

                List<int> whiteNoise = audioManipulation.WhiteNoise(2, 1);
                List<int> echoNoise = audioManipulation.AddingEchos(loadedWaveInt, 5);
                List<int> twoTones = audioManipulation.ToneCombine(30, whiteNoise, echoNoise);

                loadedWaveInt = twoTones;

                GenerateWaveOut();

            }
            catch(ArgumentNullException)
            {
                TinkeringAudioExceptionHandler.ExceptionHandler("Arugment Null Exception: No audio loaded", TinkeringAudioExceptionHandler.ExceptionType.NoAudioLoaded);
            }
        }
        #endregion

        #region COMBO BOX FUNCTIONS
        /// <summary>
        /// function to handle when the wave type dropdown gets changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbox_WaveType_SelectedIndexChanged(object sender, EventArgs e) {
            // cast the waveType from int to enum
            WaveType waveType = (WaveType)cbox_WaveType.SelectedIndex;
            // set the wave function
            waveFunction = SetWaveFunction(waveType);
        }

        /// <summary>
        /// function to handle when the bitrate is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbox_Bitrate_SelectedIndexChanged(object sender, EventArgs e) {
            // create an int array of bitrates based off the enum
            int[] bitrates = (int[])Enum.GetValues(typeof(Bitrates));
            // set the save bitrate based off the cbox_Bitrate value
            save_bitrate = (Bitrates)bitrates[cbox_Bitrate.SelectedIndex];
        }
        #endregion
    }

    /// <summary>
    /// class to handle known exceptions for the form
    /// </summary>
    public class TinkeringAudioExceptionHandler {

        // enum for the types of exception
        public enum ExceptionType {
            AudioImportError,       // audio import error - for when an error occurs when trying to load audio files
            UndefinedError,         // undefined error - for when an error isn't explicitly checked for
            NoAudioLoaded,          // no audio loaded - for when the save button is pressed
            NoAudioLoaded_Playback  // no audio loaded - for when the playback buttons are pressed
        }

        #region EXCEPTION HANDLER
        /// <summary>
        /// function to handle exception
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="exception"></param>
        public static void ExceptionHandler(string caption, ExceptionType exception) {
            string message = caption + ": The program has run into a problem.";

            // test conditions of the exception
            switch (exception) {
                // if the exception is unknown
                case ExceptionType.UndefinedError:
                    // do nothing - check first because most likely
                    break;
                // if the exception is an audio import error
                case ExceptionType.AudioImportError:
                    // set the message of the message box
                    message = "Error: Expected Format Exception.\n\nThis file doesn't appear to be a supported audio format. Please choose a different file.";
                    break;
                // if the exception is a no audio loaded error
                case ExceptionType.NoAudioLoaded:
                    // set the message of the message box
                    message = "Error: Argument Null Exception.\n\nSaving not possible, there is no audio file loaded. Please load an audio file first!";
                    break;
                // if the exception is a no audio loaded for playback error
                case ExceptionType.NoAudioLoaded_Playback:
                    // set the message of the message box
                    message = "Error: No Audio Loaded for Playback.\n\nPlayback not possible, there is no file loaded. Please load an audio file first!";
                    break;
            }

            // set the message box to have only the ok button shown
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            // show the message box with the above details
            MessageBox.Show(message, caption, buttons);
        }
        #endregion

        
    }

    /// <summary>
    /// class for dealing with audio input and output
    /// </summary>
    public class AudioFileIO {

        // an enum to store the different file types
        enum AudioFileFormat {
            None,   // No file format given
            AAC,    // Advanced Audio Coding
            MP3,    // MPEG Audio Layer-3
            WAV,    // Waveform Audio File Format
            WMA     // Windows Media Audio
        }

        /// <summary>
        /// how the AudioFileIO should be initialised
        /// </summary>
        /// <param name="sender">the main form class itself</param>
        public AudioFileIO(TinkeringAudioForm sender) {
            // the sender object so that it's values can be manipulated
            m_sender = sender;
        }

        // the sender as a local member variable
        public TinkeringAudioForm m_sender;

        #region SAVING AND LOADING FILES
        /// <summary>
        /// load an audio clip from a file
        /// </summary>
        /// <returns>the bytes from the file selected</returns>
        public byte[] LoadAudioClip() {
            // a byte array to store the output from the Load Audio Clip function
            byte[] buffer = null;
            // create a new file dialog (pop up window to browse windows explorer)
            OpenFileDialog OFDialog = new OpenFileDialog {
                // set the starting directory to the music folder
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                // create a filter
                Filter = "All files (*.*)|*.*"                      // all files
                + "|Advanced Audio Coding file (*.aac)|*.aac"       // Advanced Audio Coding file saving
                + "|MPEG Audio Layer-3 file (*.mp3)|*.mp3"          // MPEG Audio Layer-3 file saving
                + "|Waveform Audio File Format file (*.wav)|*.wav"  // Waveform Audio File Format file saving
                + "|Windows Media Video file (*.wma)|*.wma",        // Windows Media Audio file saving
                FilterIndex = 1, // start the filter index at 1 (all files)
                RestoreDirectory = true, // enable restoring directory when the dialog is closed
                Title = "Load an audio file..." // change the title to something more fitting
            };

            // open the file dialog, if the OK button is pressed
            if (OFDialog.ShowDialog() == DialogResult.OK) {
                // try to run the following
                try {
                    // set the path to the file name
                    string path = OFDialog.FileName;

                    // declare a new sample and wave provider as null
                    ISampleProvider sampleProvider = null;
                    IWaveProvider waveProvider = null;

                    // using a MediaFoundationReader at the path
                    using (var decoder = new MediaFoundationReader(path)) {
                        // set the sample and wave providers
                        sampleProvider = decoder.ToSampleProvider();
                        waveProvider = sampleProvider.ToWaveProvider16();

                        // create a buffer with the decoder length
                        buffer = new byte[decoder.Length];
                    }

                    // create a new waveformat using the waveprovider
                    WaveFormat waveFormat = waveProvider.WaveFormat;

                    // create a new file reader for the file
                    //WaveFileReader waveFileReader = new WaveFileReader(path);

                    // get the sample rate and channel count of the wave file
                    int sampleRate = waveFormat.SampleRate;
                    int channelCount = waveFormat.Channels;

                    // set the main class's sample rate and channel count
                    m_sender.sampleRate = sampleRate;
                    m_sender.channelCount = channelCount;
                    
                    // read the bytes from the wave provider and store it in the buffer
                    waveProvider.Read(buffer, 0, buffer.Length);

                    // return the loaded audio clip as a byte array
                    return buffer;
                }
                // if there is a format exception
                catch (FormatException e) {
                    // call the exception handler to deal with the exception
                    TinkeringAudioExceptionHandler.ExceptionHandler("Expected Format Exception: Audio Import Error", TinkeringAudioExceptionHandler.ExceptionType.AudioImportError);

                    // call the function again
                    LoadAudioClip();
                }
                // if there is an undefined error
                catch (Exception ex) {
                    // call the exception handler to deal with the exception
                    TinkeringAudioExceptionHandler.ExceptionHandler("Exception: " + ex.ToString() + " - Audio Import", TinkeringAudioExceptionHandler.ExceptionType.UndefinedError);

                    // call the function again
                    LoadAudioClip();
                }
            }

            // return the loaded audio clip as null - in the event the dialog closes instead
            return null;
        }

        /// <summary>
        /// save the audio clip to a file
        /// </summary>
        /// <param name="waveProvider">the wave provider to save to</param>
        public void SaveAudioClip(IWaveProvider waveProvider, int bitrate) {
            // initialize a new SaveFileDialog to export the audio files
            SaveFileDialog SFDialog = new SaveFileDialog {
                // set the starting directory to the music folder
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                // create a filter
                Filter = "Advanced Audio Coding file (*.aac)|*.aac" // Advanced Audio Coding file saving
                + "|MPEG Audio Layer-3 file (*.mp3)|*.mp3"          // MPEG Audio Layer-3 file saving
                + "|Waveform Audio File Format file (*.wav)|*.wav"  // Waveform Audio File Format file saving
                + "|Windows Media Video file (*.wma)|*.wma",        // Windows Media Audio file saving
                // start the filter at wav files (common format)
                FilterIndex = 3,
                // set the title
                Title = "Export audio..."
            };
            // show the dialog
            if (SFDialog.ShowDialog() == DialogResult.OK) {
                if (SFDialog.FileName != "") {
                    // set the file name based off the the directory of the SaveFileDialog
                    string filename = SFDialog.FileName;

                    // create a wave format from the waveprovider passed as a parameter
                    WaveFormat waveFormat = waveProvider.WaveFormat;

                    // create a new format for the wave
                    var outFormat = new WaveFormat(bitrate, waveFormat.Channels);
                    // declare the media type
                    MediaType mediaType = null;

                    // check the options in the filter index
                    switch ((AudioFileFormat)SFDialog.FilterIndex) {
                        // if an aac file is chosen to be saved
                        case AudioFileFormat.AAC:
                            // set the media type to the AAC encoding
                            mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_AAC, waveFormat, bitrate);
                            break;
                        // if an mp3 file is chosen to be saved
                        case AudioFileFormat.MP3:
                            // set the media type to the MP3 encoding
                            mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3, waveFormat, bitrate);
                            break;
                        // if a wav file is chosen to be saved
                        case AudioFileFormat.WAV:
                            WaveFileWriter.CreateWaveFile(filename, waveProvider);
                            break;
                        // if a wma file is chosen to be saved
                        case AudioFileFormat.WMA:
                            // set the media type to the WMA encoding
                            mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_WMAudioV9, waveFormat, bitrate);
                            break;
                    }

                    Console.WriteLine(SFDialog.FilterIndex);
                    Console.WriteLine((AudioFileFormat)SFDialog.FilterIndex);

                    // if the media type has been set
                    if (mediaType != null) {
                        // create a new resampler to resample to wave provider to the out format
                        using (var resampler = new MediaFoundationResampler(waveProvider, outFormat)) {
                            // create a new encoder
                            using (var encoder = new MediaFoundationEncoder(mediaType)) {
                                // save the file with the envoding
                                encoder.Encode(filename, waveProvider);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }

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

        #region AUDIO_ALGORITHIMS

        #region AudioSplicing
        /// <summary>
        /// function to generate a list of ints which splices two audio segments together
        /// </summary>
        /// <param name="audioSample0">the first audio sample</param>
        /// <param name="audioSample1">the second audio sample</param>
        /// <returns>the two spliced audio samples</returns>
        public List<int> AudioSplicing(List<int> audioSample0, List<int> audioSample1)
        {
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
        public List<int> AddingEchos(List<int> inputList, int delayInSeconds)
        {

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
            for (int i = 0; i < inputList.Count + echoDuration; i++)
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

                if (i % 1000 == 0) {
                    Console.WriteLine(value);
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
        public List<int> Normalisation(List<int> sample)
        {
            Console.WriteLine("[RUNNING]: Normalisation");

            // declare and set the maximum volume of the sample
            int maxAmplitudeOfSample = sample.Max();

            // declare and set the ratio of the amplitude compared the the max aplitude
            int amplitudeRatio = (m_sender.MAX_VALUE - 1) / maxAmplitudeOfSample;

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
        /// <param name="audSample"></param>
        /// <param name="audScale"></param>
        /// <returns>the resampled list</returns>
        public List<int> Resample(List<int> audSample, double audScale)
        {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Resample");

            // the modified audioscale 
            double modAudioScale = 1.0 / audScale;

            //declares list for the resampled sound
            List<int> resampledList = new List<int>();

            // if modified audio scale is greater than 1 then
            if (modAudioScale > 1)
            {
                // while the statement i is less than the length of audSample is true execute loop
                for (int i = 0; i < (audSample.Count); i++)
                {
                    // declare double value var
                    int value = 0;

                    // while the statement j is less than modified audio scale is true execute loop
                    for (int j = 0; j < modAudioScale; j++)
                    {
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
            else
            {
                // declaring the index var
                int index = 0;

                // declaring the l var
                double indexLocation = 0.0;

                // declaring m as 1 divided by the audio scale
                double incrementAmount = 1.0 / audScale;

                // while the statement index is less than the length of audiosample do this
                do
                {
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
        /// <param name="audSample"></param>
        /// <param name="ampFactor"></param>
        /// <returns>returns a newly scaled list</returns>
        public List<int> ScalingAmplitude(List<int> audSample, int ampFactor)
        {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: Scaling Amplitude");

            // delcares a new list to hold the scaled audio
            List<int> ScaledList = new List<int>();

            // while the statement i is less than the length of audio sample then execute loop
            for (int i = 0; i < (audSample.Count); i++)
            {
                //declares variable v as audio sample instance i multiplied by the amplitude factor
                int v = audSample[i] * ampFactor;

                v = Math.Max((v), v);

                v = Math.Min((v), v);

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
        public List<int> WhiteNoise(double time, int resultantVol)
        {
            // write to the console that the function is being ran
            Console.WriteLine("[RUNNING]: White Noise");

            // declaring new list for the two tones to combine in 
            List<int> WhiteList = new List<int>();

            //create a random variable which generates new random each for loop
            Random rand = new Random();

            // while the statement i is less than time multiplied by the sample rate is true execute loop
            for (int i = 0; i < (time * m_sender.sampleRate); i++)
            {
                // adds a random number between -1 and 1 to the white list
                WhiteList.Add(rand.Next(-1, 1) * resultantVol *  m_sender.MAX_VALUE);
            }
            // returns modifed white list to 
            return WhiteList;
        }
        #endregion

        #endregion
    }

}