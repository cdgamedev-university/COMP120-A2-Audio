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
//                                                                           \\
//                    ███████  ██████  ██████  ███    ███                    \\
//                    ██      ██    ██ ██   ██ ████  ████                    \\
//                    █████   ██    ██ ██████  ██ ████ ██                    \\
//                    ██      ██    ██ ██   ██ ██  ██  ██                    \\
//                    ██       ██████  ██   ██ ██      ██                    \\
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

// the packages for the program
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

/// <summary>
/// the namespace which stores all the code for the tinkering audio project
/// </summary>
namespace Tinkering_Audio {
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
        public AudioFileIO audioFileIO;

        // the audio manipulation class (for manipulating the audio)
        private AudioManipulation audioManipulation;

        // the audio exception handler class (for dealing with exceptions nicely)
        public ExceptionHandler exceptionHandler;

        // information to store the loaded wave
        private byte[] loadedWaveByte;
        private List<int> loadedWaveInt;

        // save information
        private Bitrates save_bitrate = Bitrates.kbit192;
        #endregion

        #region FORM INITIALISATION
        // initialise the form
        public TinkeringAudioForm() {
            // initialize the form
            InitializeComponent();

            // create a new audio file io directing to this class instance as the sender
            audioFileIO = new AudioFileIO(sender: this);

            // create a new audio manipulation directing to this class instance as the sender
            audioManipulation = new AudioManipulation(sender: this);

            // create a new exception handler
            exceptionHandler = new ExceptionHandler();
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
        /// <returns>returns a random element of the enumerable</returns>
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
            } catch (NullReferenceException ex) {
                // call the exception handler to deal with the exception
                exceptionHandler.Handle($"{ex.GetType().Name}: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_Conversion);
            } catch (Exception ex) {
                // call the exception handler to deal with the exception
                exceptionHandler.Handle(ex.ToString(), ExceptionHandler.ExceptionType.UndefinedError);
            }

            // return nothing
            return null;
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
                exceptionHandler.Handle("Null Reference Exception: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_Playback);
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
        /// <returns>a 1 or 0 depending on the frequency</returns>
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
        /// <summary>
        /// function to control what should happen when the load audio file button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadAudioFile_Click(object sender, EventArgs e) {
            // load the wave as bytes
            loadedWaveByte = audioFileIO.LoadAudioClip();

            // if the loaded wave isnt null then convert the wave to a List<int> (as the other manipulation functions require this)
            if (loadedWaveByte != null) {
                loadedWaveInt = audioManipulation.ConvertToListInt(loadedWaveByte);

                GenerateWaveOut();
            }

            // write to the console the channel count and sample rate
            Console.WriteLine("Channel Count: {0}, Sample Rate: {1}", channelCount, sampleRate);
        }

        /// <summary>
        /// function to cotnrol what should happen when the save audio button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAudioFile_Click(object sender, EventArgs e) {
            if (loadedWaveInt != null) {
                // create the wave provider using the loaded wave
                waveProvider = convertToWaveProvider16(loadedWaveInt, sampleRate, channelCount);

                // if the wave provider isn't null
                if (waveProvider != null) {
                    // save the audio clip using the wave provider
                    audioFileIO.SaveAudioClip(waveProvider, (int)save_bitrate);
                }
            } else {
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_Saving);
            }
        }

        /// <summary>
        /// function to control what should happen when the play button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PlayAudio_Click(object sender, EventArgs e) {
            // play the audio
            PlayAudio();
        }

        /// <summary>
        /// function to control what should happen when the stop button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StopAudio_Click(object sender, EventArgs e) {
            // stop the audio
            StopAudio();
        }

        /// <summary>
        /// function to control what should happen when the pause button is pressed
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
        /// function to control what should happen when the Village button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Villagebtn_Click(object sender, EventArgs e) {
            // if there is a wave loaded
            if (loadedWaveInt != null) {
                // apply the village filter to the audio
                loadedWaveInt = audioManipulation.ApplyVillageEffect(loadedWaveInt);

                // generate the wave out
                GenerateWaveOut();
            }
            // if there isn't a wave loaded
            else {
                // open a message box which tells the user that no audio is loaded
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_ApplyEffect);
            }
        }

        /// <summary>
        /// function to control what should happen when the Forest button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Forestbtn_Click(object sender, EventArgs e) {
            // if there is a wave loaded
            if (loadedWaveInt != null) {
                // apply the forest filter to the audio
                loadedWaveInt = audioManipulation.ApplyForestEffect(loadedWaveInt);

                // generate the wave out
                GenerateWaveOut();
            }
            // if there isn't a wave loaded
            else {
                // open a message box which tells the user that no audio is loaded
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_ApplyEffect);
            }
        }

        /// <summary>
        /// function to control when the Cave button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cavebtn_Click(object sender, EventArgs e) {
            // if there is a wave loaded
            if (loadedWaveInt != null) {
                // apply the cave filter to the audio
                loadedWaveInt = audioManipulation.ApplyCaveEffect(loadedWaveInt);

                // generate the wave out
                GenerateWaveOut();
            }
            // if there isn't a wave loaded
            else {
                // open a message box which tells the user that no audio is loaded
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_ApplyEffect);
            }
        }

        /// <summary>
        /// function to control what should happen when the Ocean button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Oceanbtn_Click(object sender, EventArgs e) {
            // if there is a wave loaded
            if (loadedWaveInt != null) {
                // apply the ocean filter to the audio
                loadedWaveInt = audioManipulation.ApplyOceanEffect(loadedWaveInt);

                // generate the wave out
                GenerateWaveOut();
            }
            // if there isn't a wave loaded
            else {
                // open a message box which tells the user that no audio is loaded
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_ApplyEffect);
            }
        }

        /// <summary>
        /// function to control what should happen when the Audio Splice button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AudioSplice_Click(object sender, EventArgs e) {
            // if there is a wave loaded
            if (loadedWaveInt != null) {
                // apply a splicing to the audio
                loadedWaveInt = audioManipulation.ApplyAudioSplice(loadedWaveInt);

                // generate the wave out
                GenerateWaveOut();
            }
            // if there isn't a wave loaded
            else {
                // open a message box which tells the user that no audio is loaded
                exceptionHandler.Handle("Error: No Audio Loaded", ExceptionHandler.ExceptionType.NoAudioLoaded_ApplyEffect);
            }
        }
        #endregion



        #region COMBO BOX FUNCTIONS
        /// <summary>
        /// function to control when the wave type dropdown gets changed
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
        /// function to control when the bitrate is changed
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
}