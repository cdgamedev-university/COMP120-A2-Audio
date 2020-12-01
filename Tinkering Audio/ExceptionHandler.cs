//**************************************************************************\\
//          ██████  ██████  ███    ███ ██████   ██ ██████   ██████          \\
//         ██      ██    ██ ████  ████ ██   ██ ███      ██ ██  ████         \\
//         ██      ██    ██ ██ ████ ██ ██████   ██  █████  ██ ██ ██         \\
//         ██      ██    ██ ██  ██  ██ ██       ██ ██      ████  ██         \\
//          ██████  ██████  ██      ██ ██       ██ ███████  ██████          \\
//                                                                          \\
//   ████████ ██ ███    ██ ██   ██ ████████ ██████  ██ ███    ██  ██████    \\
//      ██    ██ ████   ██ ██  ██  ██       ██   ██ ██ ████   ██ ██         \\
//      ██    ██ ██ ██  ██ █████   ██████   ██████  ██ ██ ██  ██ ██   ███   \\
//      ██    ██ ██  ██ ██ ██  ██  ██       ██   ██ ██ ██  ██ ██ ██    ██   \\
//      ██    ██ ██   ████ ██   ██ ████████ ██   ██ ██ ██   ████  ██████    \\
//                                                                          \\
//                  █████  ██    ██ ███████  ██  ██████                     \\
//                 ██   ██ ██    ██ ██    ██ ██ ██    ██ ██                 \\
//                 ███████ ██    ██ ██    ██ ██ ██    ██                    \\
//                 ██   ██ ██    ██ ██    ██ ██ ██    ██ ██                 \\
//                 ██   ██  ██████  ███████  ██  ██████                     \\
//                                                                          \\
//  ███████ ██   ██  ██████ ███████ ██████  ████████ ██  ██████  ███    ██  \\
//  ██       ██ ██  ██      ██      ██   ██    ██    ██ ██    ██ ████   ██  \\
//  █████     ███   ██      █████   ██████     ██    ██ ██    ██ ██ ██  ██  \\
//  ██       ██ ██  ██      ██      ██         ██    ██ ██    ██ ██  ██ ██  \\
//  ███████ ██   ██  ██████ ███████ ██         ██    ██  ██████  ██   ████  \\
//                                                                          \\
//        ██   ██  █████  ███    ██ ███████  ██      ███████ ██████         \\
//        ██   ██ ██   ██ ████   ██ ██    ██ ██      ██      ██   ██        \\
//        ███████ ███████ ██ ██  ██ ██    ██ ██      █████   ██████         \\
//        ██   ██ ██   ██ ██  ██ ██ ██    ██ ██      ██      ██   ██        \\
//        ██   ██ ██   ██ ██   ████ ███████  ███████ ███████ ██   ██        \\
//                                                                          \\
// Copyright (c) 2020 Daisy Baker and Hayley Davies                         \\
// Contact: db246020@falmouth.ac.uk or cd230099@falmouth.ac.uk              \\
//**************************************************************************\\

// the packages for the program
using System.Windows.Forms;

// assign class under the Tinkering_Audio namespace
namespace Tinkering_Audio {
    /// <summary>
    /// class to handle known exceptions for the form
    /// </summary>
    public class ExceptionHandler {

        // enum for the types of exception
        public enum ExceptionType {
            UndefinedError,             // undefined error - for when an error isn't explicitly checked for
            AudioImportError,           // audio import error - for when an error occurs when trying to load audio files
            NoAudioLoaded_Saving,       // no audio loaded - for when the save button is pressed
            NoAudioLoaded_Playback,     // no audio loaded - for when the playback buttons are pressed
            NoAudioLoaded_ApplyEffect,  // no audio loaded - for when one of the effect buttons are pressed
            NoAudioLoaded_Conversion,   // no audio loaded - for when a conversion is attempted
            NoAudioLoaded_Splicing      // no audio loaded - for when splicing is attempted
        }

        #region EXCEPTION HANDLER
        /// <summary>
        /// function to handle exception
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="exception"></param>
        public void Handle(string caption, ExceptionType exception) {
            string message = caption + ": The program has run into a problem.\n\nPlease contact the developer with the issue.";

            // test conditions of the exception
            switch (exception) {
                // if the exception is unknown
                case ExceptionType.UndefinedError:
                    // do nothing - check first because most likely
                    break;
                // if the exception is an audio import error
                case ExceptionType.AudioImportError:
                    // set the message of the message box
                    message = $"Error: Expected Format Exception ({(int)ExceptionType.AudioImportError}).\n\nThis file doesn't appear to be a supported audio format. Please choose a different file.";
                    break;
                // if the exception is a no audio loaded error
                case ExceptionType.NoAudioLoaded_Saving:
                    // set the message of the message box
                    message = $"Error: Argument Null Exception ({(int)ExceptionType.NoAudioLoaded_Saving}).\n\nSaving not possible, there is no audio file loaded. Please load an audio file first!";
                    break;
                // if the exception is a no audio loaded for playback error
                case ExceptionType.NoAudioLoaded_Playback:
                    // set the message of the message box
                    message = $"Error: No Audio Loaded ({(int)ExceptionType.NoAudioLoaded_Playback}).\n\nPlayback not possible, there is no file loaded. Please load an audio file first!";
                    break;
                // if the exception is a no audio loaded for manipulation error
                case ExceptionType.NoAudioLoaded_ApplyEffect:
                    // set the message of the message box
                    message = $"Error: No Audio Loaded for Manipulation ({(int)ExceptionType.NoAudioLoaded_ApplyEffect}).\n\nManipulation not possible, there is no file loaded. Please load an audio file first!";
                    break;
                // if the exception is a no audio loaded for conversion error
                case ExceptionType.NoAudioLoaded_Conversion:
                    // set the message of the message box
                    message = $"Error: No Audio Loaded for Conversion ({(int)ExceptionType.NoAudioLoaded_Conversion}).\n\nManipulation not possible, there is no file loaded. Please load an audio file first!";
                    break;
                // if the exception is a no audio loaded for splicing error
                case ExceptionType.NoAudioLoaded_Splicing:
                    // set the message of the message box
                    message = $"Error: No Audio Loaded for Conversion ({(int)ExceptionType.NoAudioLoaded_Splicing}).\n\nSplicing not possible, there is no file loaded. Please load an audio file first!";
                    break;
            }

            // set the message box to have only the ok button shown
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            // show the message box with the above details
            MessageBox.Show(message, caption, buttons);
        }
        #endregion
    }
}
