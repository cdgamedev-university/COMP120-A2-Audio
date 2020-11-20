using System;
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

        // double var to hold volume lvl
        private double volume = 0.08;

        // allows ability to treat function like variable
        private delegate double WaveFunction(double frequency, int position);
        private WaveFunction waveFunction = null;

        // 
        private WaveOut waveOut = null;
        private IWaveProvider waveProvider = null;

        // 
        private List<double> notes;
        private double[] noteDuration;
        #endregion

        // initialise form
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
        /// 
        /// </summary>
        /// <param name="baseNote"></param>
        /// <param name="startNote"></param>
        /// <param name="endNote"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        private List<double> PopulateNotes(double baseNote, int startNote, int endNote, int increment) 
        {
            double ESTIMATOR = Math.Pow(2.0, (1.0 / 12.0));

            List<double> notes = new List<double>();

            for (int i = startNote; i < endNote; i += increment) 
            {
                notes.Add(baseNote * Math.Pow(ESTIMATOR, i));
            }

            return notes;
        }

        private IWaveProvider convertToWaveProvider16(List<int> sample, int sampleRate, int channelCount) 
        {
            byte[] byteBuffer = new byte[sample.Count * 2];

            int byteArrayIndex = 0;
            short value;

            for (int i = 0; i < sample.Count; i++) 
            {
                if (sample[i] > MAX_VALUE) 
                {
                    value = (short)MAX_VALUE;
                } 
                
                else if (sample[i] < -MAX_VALUE) 
                {
                    value = (short)-MAX_VALUE;
                } 
                
                else 
                {
                    value = (short)sample[i];
                }

                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[0];
                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[1];
            }

            IWaveProvider waveProvider = new RawSourceWaveStream(new MemoryStream(byteBuffer), new WaveFormat(sampleRate, 16, channelCount));

            return waveProvider;
        }
        #endregion

        #region WAVE TYPES
        
        // this is a wave which amplitude alternates at a freq between fixed min and max values
        private double SquareWave(double frequency, int position) 
        {
            double value = Math.Sin(2.0 * Math.PI * frequency * (position / (double) SAMPLE_RATE));

            if (value > 0) 
            {
                return 1.0;
            } 

            else 
            {
                return -1.0;
            }
        }

        // a smooth wave with peridoic oscillation
        private double SineWave(double frequency, int position) 
        {
            return Math.Sin(2.0 * Math.PI * frequency * (position / (double)SAMPLE_RATE));
        }

        // a non-sinusoidal wave with a triangular shape 
        private double TriangleWave(double frequency, int position) 
        {
            double value = ((2.0 * MAX_VALUE * volume) / Math.PI) *Math.Asin(Math.Sin(2.0 * Math.PI * frequency * position));
            return value;
        }

        // another non-sinusoidal wave which ramps upwards, drops down and repeats
        private double SawtoothWave(double frequency, int position) 
        {
            double value = (-(2.0 * MAX_VALUE * volume) / Math.PI) * Math.Atan((1 / Math.Tan(Math.PI * frequency * position)));
            return value;
        }
        #endregion

        #region BUTTON FUNCTIONS
        private void btn_GenerateMelody_Click(object sender, EventArgs e) 
        {
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), SAMPLE_RATE, 1);

            waveOut = new WaveOut();
            waveOut.Init(waveProvider);
            waveOut.Play();
        }

        private void btn_SaveMelody_Click(object sender, EventArgs e) 
        {
            waveProvider = convertToWaveProvider16(GenerateRandomMelody(12), SAMPLE_RATE, 1);

            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");

            WaveFileWriter.CreateWaveFile(filename, waveProvider);
        }

        private void btn_GenerateWhiteNoise_Click(object sender, EventArgs e) 
        {
            WaveOut waveOut = new WaveOut();
            waveOut.Init(convertToWaveProvider16(GenerateWhiteNoise(12), SAMPLE_RATE, 1));
            waveOut.Play();
        }
        #endregion

        #region MELODY BUTTONS

        private double AudioSplicing (double audSamp1, double audSamp2)
        {
            // declare list
            List<int> note;

            for (int i = 0; i < (audSamp1); i++)
            {
                // (S) audSamp1i is appened to n
                
            }

            for (int j = 0, j < (audSamp2); j++)
            {
                // (T) audSamp2j is appended to n
            }

            return note;

        }   

        private double AddingEchos (List<int> inputList, double seconds)
        {
            // required: 
            // 1 =< t
            // 1 =< (S) SAMPLE_RATE; 

            // there is an input list s, where the input is extended by t seconds
            // combines input list with delayed copy of itself

            List<int> note = ();
            double o = seconds * SAMPLE_RATE;

            for (int i = 0; i < (inputList.Count) + o; i++)
            {
                double v = 0;

                if (i < Len(sample)
                {
                    v = v + i;
                }

                if (i - o > 0)
                {
                    v = v + i;
                }

                note = v;
            }

            return note;

        }

        private double Normalisation (double audSamp)
        {

            return 0.0;
        }

        private double Resample (double audSamp, int audScale)
        {

            return 0.0;
        }

        private double ScalingAmplitude (double audSamp, double volFactor)
        {

            return 0.0;
        }

        private double ToneCombine (double duration, double freq, double w)
        {

            return 0.0;
        }

        private double WhiteNoise (double t, double resultantVol)
        {

            return 0.0;
        }

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
    }
}
