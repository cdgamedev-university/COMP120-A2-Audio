//***********************************************************************\\
//        ██████  ██████  ███    ███ ██████   ██ ███████   ██████        \\
//       ██      ██    ██ ████  ████ ██   ██ ███       ██ ██  ████       \\
//       ██      ██    ██ ██ ████ ██ ██████   ██  ██████  ██ ██ ██       \\
//       ██      ██    ██ ██  ██  ██ ██       ██ ██       ████  ██       \\
//        ██████  ██████  ██      ██ ██       ██ ████████  ██████        \\
//                                                                       \\
//  ████████ ██ ███    ██ ██   ██ ███████ ██████  ██ ███    ██  ██████   \\
//     ██    ██ ████   ██ ██  ██  ██      ██   ██ ██ ████   ██ ██        \\
//     ██    ██ ██ ██  ██ █████   █████   ██████  ██ ██ ██  ██ ██   ███  \\
//     ██    ██ ██  ██ ██ ██  ██  ██      ██   ██ ██ ██  ██ ██ ██    ██  \\
//     ██    ██ ██   ████ ██   ██ ███████ ██   ██ ██ ██   ████  ██████   \\
//                                                                       \\
//                  █████  ██    ██ ███████  ██  ██████                  \\
//                 ██   ██ ██    ██ ██    ██ ██ ██    ██                 \\
//                 ███████ ██    ██ ██    ██ ██ ██    ██                 \\
//                 ██   ██ ██    ██ ██    ██ ██ ██    ██                 \\
//                 ██   ██  ██████  ███████  ██  ██████                  \\
//                                                                       \\
//   █████  ██    ██ ██████  ██  ██████      ███████ ██ ██      ███████  \\
//  ██   ██ ██    ██ ██   ██ ██ ██    ██     ██      ██ ██      ██       \\
//  ███████ ██    ██ ██   ██ ██ ██    ██     █████   ██ ██      █████    \\
//  ██   ██ ██    ██ ██   ██ ██ ██    ██     ██      ██ ██      ██       \\
//  ██   ██  ██████  ██████  ██  ██████      ██      ██ ███████ ███████  \\
//                                                                       \\
//                              ██  ██████                               \\
//                              ██ ██    ██                              \\
//                              ██ ██    ██                              \\
//                              ██ ██    ██                              \\
//                              ██  ██████                               \\
//                                                                       \\
// Copyright (c) 2020 Daisy Baker and Hayley Davies                      \\
// Contact: db246020@falmouth.ac.uk or cd230099@falmouth.ac.uk           \\
//***********************************************************************\\

// the packages for the program
using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace Tinkering_Audio {
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

                    Stream clip = new FileStream(path, FileMode.Open);

                    // return the loaded audio clip as a byte array
                    return DecodeAudioClip(clip);
                }
                // if there is a format exception
                catch (FormatException e) {
                    // call the exception handler to deal with the exception
                    m_sender.exceptionHandler.Handle("Expected Format Exception: Audio Import Error", ExceptionHandler.ExceptionType.AudioImportError);

                    // call the function again
                    LoadAudioClip();
                }
                // if there is an undefined error
                catch (Exception ex) {
                    // call the exception handler to deal with the exception
                    m_sender.exceptionHandler.Handle("Exception: " + ex.ToString() + " - Audio Import", ExceptionHandler.ExceptionType.UndefinedError);

                    // call the function again
                    LoadAudioClip();
                }
            }

            // return the loaded audio clip as null - in the event the dialog closes instead
            return null;
        }

        public byte[] DecodeAudioClip(Stream clip) {
            // a byte array to store the output from the Decode Audio Clip function
            byte[] buffer = null;

            IWaveProvider waveProvider = null;

            // using a MediaFoundationReader at the path
            using (var decoder = new StreamMediaFoundationReader(clip)) {
                // set the sample and wave providers
                waveProvider = decoder.ToSampleProvider().ToWaveProvider16();

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

            return buffer;
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

                    Console.WriteLine($"Saving file as: {(AudioFileFormat)SFDialog.FilterIndex}");

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
}