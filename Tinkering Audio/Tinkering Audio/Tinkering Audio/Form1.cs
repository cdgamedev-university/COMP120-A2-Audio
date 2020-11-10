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
    public partial class TinkeringAudioForm : Form {
        private readonly int SAMPLE_RATE = 44100;
        private readonly int MAX_VALUE = (int)Math.Pow(2, 15);

        private double volume = 0.08;

        private delegate double WaveFunction(double frequency, int position);
        private WaveFunction waveFunction = null;

        private WaveOut waveOut = null;
        private IWaveProvider waveProvider = null;

        private List<double> notes;
        private double[] noteDuration;

        public TinkeringAudioForm() {
            InitializeComponent();
        }

        private List<byte> GenerateSilence(double durationInSeconds) {
            List<byte> silence = new List<byte>();

            for (int i = 0; i < (int)(durationInSeconds * SAMPLE_RATE); i++) {
                silence.Add(0);
                silence.Add(0);
            }
            return silence;
        }

        private List<byte> GenerateTone(double durationInSeconds, WaveFunction waveFunction, double[] frequencies) {
            List<byte> tone = new List<byte>();

            short value;

            for (int i = 0; i < (int)(durationInSeconds * SAMPLE_RATE); i++) {
                value = 0;
                for (int j = 0; j < frequencies.Length; j++) {
                    value += (short)(MAX_VALUE * volume * waveFunction.Invoke(frequencies[j], i));
                }
                tone.Add(BitConverter.GetBytes(value)[0]);
                tone.Add(BitConverter.GetBytes(value)[1]);
            }
            return tone;
        }

        private List<byte> GenerateRandomMelody(int countOfNotesToPlay) {
            Random rng = new Random();

            List<byte> melody = new List<byte>();
            melody.AddRange(GenerateSilence(0.1));

            double[] frequencies = new double[2];

            for (int i = 0; i < countOfNotesToPlay; i++) {
                frequencies[0] = GetRandomElement(notes, rng);
                frequencies[1] = GetRandomElement(notes, rng);

                melody.AddRange(GenerateTone(GetRandomElement(noteDuration, rng), this.waveFunction, frequencies));
            }
            return melody;
        }

        private static T GetRandomElement<T>(IEnumerable<T> enumerable, Random rng) {
            int index = rng.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }

        private List<double> PopulateNotes(double baseNote, int startNote, int endNote, int increment) {
            double ESTIMATOR = Math.Pow(2.0, (1.0 / 12.0));

            List<double> notes = new List<double>();

            for (int i = startNote; i < endNote; i += increment) {
                notes.Add(baseNote * Math.Pow(ESTIMATOR, i));
            }
            return notes;
        }

        private double SquareWave(double frequency, int position) {
            double value = Math.Sin(2.0 * Math.PI * frequency * (position / (double)SAMPLE_RATE));

            if (value > 0) {
                return 1.0;
            } else {
                return -1.0;
            }
        }

        private double SinWave(double frequency, int position) {
            return Math.Sin(2.0 * Math.PI * frequency * (position / (double)SAMPLE_RATE));
        }

        private void TinkeringAudioForm_Load(object sender, EventArgs e) {
            this.waveFunction = SquareWave;
            notes = PopulateNotes(440, -16, 8, 2);
            noteDuration = new double[] { 0.15, 0.2, 0.3, 0.4 };
        }

        private void button1_Click(object sender, EventArgs e) {
            byte[] audioSample = GenerateRandomMelody(12).ToArray();

            waveProvider = new RawSourceWaveStream(new MemoryStream(audioSample), new WaveFormat(SAMPLE_RATE, 1));

            waveOut = new WaveOut();
            waveOut.Init(waveProvider);
            waveOut.Play();
        }

        private void button2_Click(object sender, EventArgs e) {
            byte[] audioSample = GenerateRandomMelody(12).ToArray();

            waveProvider = new RawSourceWaveStream(new MemoryStream(audioSample), new WaveFormat(SAMPLE_RATE, 1));

            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");

            WaveFileWriter.CreateWaveFile(filename, waveProvider);
        }
    }
}
