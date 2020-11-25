#COMP120 Tinkering Audio
#Contract 2 - Ambient Platformers Audio Generation (Diegetic Audio)

For this project we decided on the GNU GENERAL PUBLIC LICENSE. This is because the GNU license is intended for
open-source software that guarantees end users the freedom to run, study, share and modify the software as they
please. However, for both the user's and authors, it is required that modified versions be marked as changed so 
thatany problems will not be attributed incorrectly to the author. The second reason being that the GNU prohibits 
any closed-soure software using this source code this so it can remain free to everyone, and when referring to 
the software as free, we are referring to the freedom of the software.

Our contract requires us to generate ambient audio tracks so that a player feels as though they're in 
differentareas of the game's setting. Hayley Davies and myself have implimented 7 algorithms which 
will be used to meet the criteria on this contract.

This is a form program loads audio files from the user, and then manipulates the sound files use the algorithms.
This is a form program loads audio files from the user, and then manipulates the sound files using these algorithms:

1. Audio Splicing     - This function allows two audio samples to be combined together into a single 
			audio file, we have chosen this because we have multiple different audio 
			samples which could be used well together, using audio splicing we can easily 
			combine the two into one list for easy playability.

2. Adding Echos       - This function adds can echo to an audio sample by extending the audio by x
			amount of seconds and adding the delayed copy to the orginal. We have chosen
			this function because echos are very effective for creating specific 
			ambient enviroments such as a cave. The same audio sample wouldn't sound as 
			obvious it was a cave without echo.

3. Normalisation      - This function will find the loudest part of the audio and normalize the rest
			of the audio to have the same amplitude. We have chosen this because it will 
			be very useful ensureing spliced audio files do not sound completely different
			in volume to one another, to essentially hide that the files have been spliced.

4. Resampling         - This function will resample an audio sample changing the sample rate, this has
			been chosen because it would be useful when making different ambient areas, 
			using this the audio could sound slightly grainier.

5. Scaling Amplitude  - This function can be used to effect the amplitude of an audio file, either
			making the audio louder or quieter. This is useful because we can easily control
			the default volume of a sound file to effciently create ambient noise.

6. Tone Combine       - This function combines two audio files so they will be played at the same time,
			we have chosen this so we can use multiple different sounds and still be able to
			play them at the same time on top of one another.

7. White Noise        - This function creates white noise using a randomized number of freqencies within
			a set range. This has been chosen because it is useful for ambient affect in 
			particular enviroments such as the ocean and cave enviroment as neither have 
			specific sounds but are not silent either, within ocean white noise also sounds
			similar to distant crashing waves.

Instructions:
1. Open visual studio and load the form file
2. Press start on visual studio and run the form.
3. Press Load File and choose a file to load

