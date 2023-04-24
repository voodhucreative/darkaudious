using System;
using static Darkaudious.Helpers.Fonts;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms.Shapes;
using Plugin.SimpleAudioPlayer;
using static Darkaudious.Helpers.AudioManager;

namespace Darkaudious.Helpers
{
	public class AudioManager
	{

        ISimpleAudioPlayer AudioPlayer;

        public enum StandardNotes
        {
            _A,
            _ASharp,
            _B,
            _C,
            _CSharp,
            _D,
            _DSharp,
            _E,
            _F,
            _FSharp,
            _G,
            _GSharp
        }

        public int CurrentNote;
        public enum Notes
        {
            ShortA,
            ShortASharp,
            ShortB,
            ShortC,
            ShortCSharp,
            ShortD,
            ShortDSharp,
            ShortE,
            ShortF,
            ShortFSharp,
            ShortG,
            ShortGSharp,
            MidA,
            MidASharp,
            MidB,
            MidC,
            MidCSharp,
            MidD,
            MidDSharp,
            MidE,
            MidF,
            MidFSharp,
            MidG,
            MidGSharp,
            LongA,
            LongASharp,
            LongB,
            LongC,
            LongCSharp,
            LongD,
            LongDSharp,
            LongE,
            LongF,
            LongFSharp,
            LongG,
            LongGSharp
        }

        public int NoteLength;
        public enum NoteLengths
        {
            Short, Mid, Long
        }

        public int PhraseFeel;
        public enum PhraseFeels
        {
            Major, Minor, Dischordant, Random
        };

        public int TriadSequence;
        public enum TriadSequences
        {
            SS,
            SM,
            SL,
            MS,
            MM,
            ML,
            LS,
            LM,
            LL,
            SSS,
            SSM,
            SSL,
            SMS,
            SMM,
            SML,
            SLS,
            SLM,
            SLL,
            MSS,
            MSM,
            MSL,
            MMS,
            MMM,
            MML,
            MLL,
            LSS,
            LSM,
            LSL,
            LMS,
            LMM,
            LML,
            LLS,
            LLM,
            LLL
        }

        public Dictionary<int, string> NoteDictionary = new Dictionary<int, string>
        {
            [(int)Notes.ShortA] = "ShortA",
            [(int)Notes.ShortASharp] = "ShortASharp",
            [(int)Notes.ShortB] = "ShortB",
            [(int)Notes.ShortC] = "ShortC",
            [(int)Notes.ShortCSharp] = "ShortCSharp",
            [(int)Notes.ShortD] = "ShortD",
            [(int)Notes.ShortDSharp] = "ShortDSharp",
            [(int)Notes.ShortE] = "ShortE",
            [(int)Notes.ShortF] = "ShortF",
            [(int)Notes.ShortFSharp] = "ShortFSharp",
            [(int)Notes.ShortG] = "ShortG",
            [(int)Notes.ShortGSharp] = "ShortGSharp",
            [(int)Notes.MidA] = "MidA",
            [(int)Notes.MidASharp] = "MidASharp",
            [(int)Notes.MidB] = "MidB",
            [(int)Notes.MidC] = "MidC",
            [(int)Notes.MidCSharp] = "MidCSharp",
            [(int)Notes.MidD] = "MidD",
            [(int)Notes.MidDSharp] = "MidDSharp",
            [(int)Notes.MidE] = "MidE",
            [(int)Notes.MidF] = "MidF",
            [(int)Notes.MidFSharp] = "MidFSharp",
            [(int)Notes.MidG] = "MidG",
            [(int)Notes.MidGSharp] = "MidGSharp",
            [(int)Notes.LongA] = "LongA",
            [(int)Notes.LongASharp] = "LongASharp",
            [(int)Notes.LongB] = "LongB",
            [(int)Notes.LongC] = "LongC",
            [(int)Notes.LongCSharp] = "LongCSharp",
            [(int)Notes.LongD] = "LongD",
            [(int)Notes.LongDSharp] = "LongDSharp",
            [(int)Notes.LongE] = "LongE",
            [(int)Notes.LongF] = "LongF",
            [(int)Notes.LongFSharp] = "LongFSharp",
            [(int)Notes.LongG] = "LongG",
            [(int)Notes.LongGSharp] = "LongGSharp",
        };

        public int SoundSource;
        public enum SoundSources
        {
            Piano, ToyPiano, Tone, Noise
        }

        public int SelectedInstrument=-1;
        

        public AudioManager()
        {
            //SelectedInstrument = (int)SoundSources.Piano;
            SoundSource = (int)SoundSources.Noise;
            AudioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
        }

        public void SetSoundSource(int soundSource)
        {
            switch(soundSource)
            {
                case (int)SoundSources.Noise:
                case (int)SoundSources.Tone:
                    SoundSource = soundSource;
                    break;
                default:
                    if (SelectedInstrument < 0)
                    {
                        SelectedInstrument = soundSource;
                        SoundSource = SelectedInstrument;
                    }
                    break;
            }
        }

        public void SetInstrument(int soundSource)
        {
            SelectedInstrument = soundSource;
            SoundSource = SelectedInstrument;
        }

        public List<int> GetMajorSequence()
        {
            List<int> notes = new List<int>();

            int[] triad = null;

            triad = GetMajorTriad((int)StandardNotes._A, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._D, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._D, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._E, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._E, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._D, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMajorTriad((int)StandardNotes._A, 1); foreach (int n in triad) { notes.Add(n); }

            return notes;
        }

        public List<int> GetMinorSequence()
        {
            List<int> notes = new List<int>();

            int[] triad = null;

            triad = GetMinorTriad((int)StandardNotes._C, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._F, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._F, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._G, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._G, 0); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._F, 1); foreach (int n in triad) { notes.Add(n); }
            triad = GetMinorTriad((int)StandardNotes._C, 1); foreach (int n in triad) { notes.Add(n); }

            
            

            return notes;
        }

        public List<int> GetSong()
        {
            List<int> notes = new List<int>();

            foreach (int n in GetMajorSequence()) { notes.Add(n); }
            foreach (int n in GetMinorSequence()) { notes.Add(n); }
            foreach (int n in GetMajorSequence()) { notes.Add(n); }

            return notes;

        }

        public string GetAudioFileName(int note, bool randomiseOctave)
        {
            string fileToPlay = "";
            int octave = Numbers.GetNextRandom(1, 7);
            
            if (!randomiseOctave)
            {
                octave = 3;
            }

            if (SoundSource == (int)SoundSources.ToyPiano)
            {
                octave = 1;
            }

            switch (note)
            {
                case (int)Notes.ShortA:
                case (int)Notes.MidA:
                case (int)Notes.LongA:
                    fileToPlay = "A"+ octave + ".mp3";
                    break;
                case (int)Notes.ShortASharp:
                case (int)Notes.MidASharp:
                case (int)Notes.LongASharp:
                    fileToPlay = "Bb" + octave + ".mp3";
                    break;
                case (int)Notes.ShortB:
                case (int)Notes.MidB:
                case (int)Notes.LongB:
                    fileToPlay = "B" + octave + ".mp3";
                    break;
                case (int)Notes.ShortC:
                case (int)Notes.MidC:
                case (int)Notes.LongC:
                    fileToPlay = "C" + octave + ".mp3";
                    break;
                case (int)Notes.ShortCSharp:
                case (int)Notes.MidCSharp:
                case (int)Notes.LongCSharp:
                    fileToPlay = "Db" + octave + ".mp3";
                    break;
                case (int)Notes.ShortD:
                case (int)Notes.MidD:
                case (int)Notes.LongD:
                    fileToPlay = "D" + octave + ".mp3";
                    break;
                case (int)Notes.ShortDSharp:
                case (int)Notes.MidDSharp:
                case (int)Notes.LongDSharp:
                    fileToPlay = "Eb" + octave + ".mp3";
                    break;
                case (int)Notes.ShortE:
                case (int)Notes.MidE:
                case (int)Notes.LongE:
                    fileToPlay = "E" + octave + ".mp3";
                    break;
                case (int)Notes.ShortF:
                case (int)Notes.MidF:
                case (int)Notes.LongF:
                    fileToPlay = "F" + octave + ".mp3";
                    break;
                case (int)Notes.ShortFSharp:
                case (int)Notes.MidFSharp:
                case (int)Notes.LongFSharp:
                    fileToPlay = "Gb" + octave + ".mp3";
                    break;
                case (int)Notes.ShortG:
                case (int)Notes.MidG:
                case (int)Notes.LongG:
                    fileToPlay = "G" + octave + ".mp3";
                    break;
                case (int)Notes.ShortGSharp:
                case (int)Notes.MidGSharp:
                case (int)Notes.LongGSharp:
                    fileToPlay = "Ab" + octave + ".mp3";
                    break;

            }

            if (SoundSource == (int)SoundSources.ToyPiano)
            {
                fileToPlay = NoteDictionary[note] + ".mp3";
            }

            if (SoundSource == (int)SoundSources.Noise)
            {
                string noiseType = "n";

                if (Numbers.GetNextRandom(100) > 50)
                {
                    noiseType = "ma";
                }
                fileToPlay = noiseType + Numbers.GetNextRandom(9) + ".mp3";
            }

            Console.WriteLine("Playing: " + fileToPlay);




            return fileToPlay;
        }

        public bool IsPlaying()
        {
            return AudioPlayer.IsPlaying;
        }

        public void PlayMelodyNote(int note)
        {
            if (AudioPlayer.IsPlaying)
            {
                return;
            }

            //play note notes[note]
            if (note >= NoteDictionary.Count)
            {
                note = NoteDictionary.Count - 1;
            }
            Console.WriteLine("Play: " + NoteDictionary[note]);

            try
            {
                //AudioPlayer.Volume = volume;
                AudioPlayer.Load(NoteDictionary[note] + ".mp3");
                AudioPlayer.Play();
            }
            catch (Exception e)
            {

            }
        }

        public void PlayNote(int note, bool randomiseOctave)
        {
            if (AudioPlayer.IsPlaying)
            {
                return;
            }

            //play note notes[note]
            if (note >= NoteDictionary.Count)
            {
                note = NoteDictionary.Count - 1;
            }
            Console.WriteLine("Play: " + NoteDictionary[note]);

            try
            {
                //AudioPlayer.Volume = volume;
                AudioPlayer.Load(GetAudioFileName(note, randomiseOctave));
                AudioPlayer.Play();
            }
            catch (Exception e)
            {

            }

        }

       

        public void PlayNote(int length, int note, bool randomiseOctave)
        {
            if (note >= NoteDictionary.Count)
            {
                note = NoteDictionary.Count - 1;
            }
            PlayNote(GetNote(length, note), randomiseOctave);
        }

        public int GetNote(int length, int note)
        {
            int noteId = 0;
            noteId = note + (length*12);

            Console.WriteLine("Length: " + length + ", Note: " + note);
            Console.WriteLine("NoteId: " + noteId);

            return noteId;
        }

        public int GetStandardNote(int rootNote)
        {
            int standardNote = 0;

            switch (rootNote)
            {
                case (int)Notes.ShortA:
                case (int)Notes.MidA:
                case (int)Notes.LongA:
                    standardNote = (int)StandardNotes._A;
                    break;
                case (int)Notes.ShortASharp:
                case (int)Notes.MidASharp:
                case (int)Notes.LongASharp:
                    standardNote = (int)StandardNotes._ASharp;
                    break;
                case (int)Notes.ShortB:
                case (int)Notes.MidB:
                case (int)Notes.LongB:
                    standardNote = (int)StandardNotes._B;
                    break;
                case (int)Notes.ShortC:
                case (int)Notes.MidC:
                case (int)Notes.LongC:
                    standardNote = (int)StandardNotes._C;
                    break;
                case (int)Notes.ShortCSharp:
                case (int)Notes.MidCSharp:
                case (int)Notes.LongCSharp:
                    standardNote = (int)StandardNotes._CSharp;
                    break;
                case (int)Notes.ShortD:
                case (int)Notes.MidD:
                case (int)Notes.LongD:
                    standardNote = (int)StandardNotes._D;
                    break;
                case (int)Notes.ShortDSharp:
                case (int)Notes.MidDSharp:
                case (int)Notes.LongDSharp:
                    standardNote = (int)StandardNotes._DSharp;
                    break;
                case (int)Notes.ShortE:
                case (int)Notes.MidE:
                case (int)Notes.LongE:
                    standardNote = (int)StandardNotes._E;
                    break;
                case (int)Notes.ShortF:
                case (int)Notes.MidF:
                case (int)Notes.LongF:
                    standardNote = (int)StandardNotes._F;
                    break;
                case (int)Notes.ShortFSharp:
                case (int)Notes.MidFSharp:
                case (int)Notes.LongFSharp:
                    standardNote = (int)StandardNotes._FSharp;
                    break;
                case (int)Notes.ShortG:
                case (int)Notes.MidG:
                case (int)Notes.LongG:
                    standardNote = (int)StandardNotes._G;
                    break;
                case (int)Notes.ShortGSharp:
                case (int)Notes.MidGSharp:
                case (int)Notes.LongGSharp:
                    standardNote = (int)StandardNotes._GSharp;
                    break;
            }

            return standardNote;
        }

        public int[] GetTriad(int feel, int rootNote, int inversion)
        {
            int[] notes = null;
            
            switch (feel)
            {
                case (int)PhraseFeels.Major:
                    notes = GetMajorTriad(GetStandardNote(rootNote), inversion);
                    break;
                case (int)PhraseFeels.Minor:
                    notes = GetMinorTriad(GetStandardNote(rootNote), inversion);
                    break;
                case (int)PhraseFeels.Dischordant:
                    notes = GetDischordantTriad(GetStandardNote(rootNote), inversion);
                    break;
                default:
                    notes = GetRandomTriad(GetStandardNote(rootNote), inversion);
                    break;
            }

            return notes;
        }

        public int[] GetTriadInversion(int[] triad, int inversionNum)
        {
            int[] notes = new int[3];

            switch(inversionNum)
            {
                case 1:
                    notes[0] = triad[1];
                    notes[1] = triad[2];
                    notes[2] = triad[0];
                    break;
                case 2:
                    notes[0] = triad[2];
                    notes[1] = triad[0];
                    notes[2] = triad[1];
                    break;
                default:
                    notes = triad;
                    break;
            }
            return notes;
        }

        public int[] GetMajorTriad(int rootNote, int inversion)
        {
            int[] notes = new int[3];
            switch(rootNote)
            {
                case (int)StandardNotes._C:
                    notes = new int[] { (int)StandardNotes._C, (int)StandardNotes._E, (int)StandardNotes._G };
                    break;
                case (int)StandardNotes._CSharp:
                    notes = new int[] { (int)StandardNotes._CSharp, (int)StandardNotes._F, (int)StandardNotes._GSharp };
                    break;
                case (int)StandardNotes._D:
                    notes = new int[] { (int)StandardNotes._D, (int)StandardNotes._FSharp, (int)StandardNotes._A };
                    break;
                case (int)StandardNotes._DSharp:
                    notes = new int[] { (int)StandardNotes._DSharp, (int)StandardNotes._E, (int)StandardNotes._ASharp };
                    break;
                case (int)StandardNotes._E:
                    notes = new int[] { (int)StandardNotes._E, (int)StandardNotes._GSharp, (int)StandardNotes._B };
                    break;
                case (int)StandardNotes._F:
                    notes = new int[] { (int)StandardNotes._F, (int)StandardNotes._A, (int)StandardNotes._C };
                    break;
                case (int)StandardNotes._FSharp:
                    notes = new int[] { (int)StandardNotes._FSharp, (int)StandardNotes._ASharp, (int)StandardNotes._CSharp };
                    break;
                case (int)StandardNotes._G:
                    notes = new int[] { (int)StandardNotes._G, (int)StandardNotes._B, (int)StandardNotes._D };
                    break;
                case (int)StandardNotes._GSharp:
                    notes = new int[] { (int)StandardNotes._GSharp, (int)StandardNotes._C, (int)StandardNotes._DSharp };
                    break;
                case (int)StandardNotes._A:
                    notes = new int[] { (int)StandardNotes._A, (int)StandardNotes._CSharp, (int)StandardNotes._E };
                    break;
                case (int)StandardNotes._ASharp:
                    notes = new int[] { (int)StandardNotes._ASharp, (int)StandardNotes._D, (int)StandardNotes._F };
                    break;
                case (int)StandardNotes._B:
                    notes = new int[] { (int)StandardNotes._B, (int)StandardNotes._DSharp, (int)StandardNotes._FSharp };
                    break;
            }

            if (inversion == 3) // random
            {
                return GetTriadInversion(notes, Numbers.GetNextRandom(0, 2));
            }

            if (inversion < 3) // 0, 1, 2
            {
                return GetTriadInversion(notes, inversion);
            }

            return notes; // ignore inversion parameter
        }

        public int[] GetMinorTriad(int rootNote, int inversion)
        {
            int[] notes = new int[3];
            switch (rootNote)
            {
                case (int)StandardNotes._C:
                    notes = new int[] { (int)StandardNotes._C, (int)StandardNotes._DSharp, (int)StandardNotes._G };
                    break;
                case (int)StandardNotes._CSharp:
                    notes = new int[] { (int)StandardNotes._CSharp, (int)StandardNotes._E, (int)StandardNotes._GSharp };
                    break;
                case (int)StandardNotes._D:
                    notes = new int[] { (int)StandardNotes._D, (int)StandardNotes._F, (int)StandardNotes._A };
                    break;
                case (int)StandardNotes._DSharp:
                    notes = new int[] { (int)StandardNotes._DSharp, (int)StandardNotes._FSharp, (int)StandardNotes._ASharp };
                    break;
                case (int)StandardNotes._E:
                    notes = new int[] { (int)StandardNotes._E, (int)StandardNotes._G, (int)StandardNotes._B };
                    break;
                case (int)StandardNotes._F:
                    notes = new int[] { (int)StandardNotes._F, (int)StandardNotes._GSharp, (int)StandardNotes._C };
                    break;
                case (int)StandardNotes._FSharp:
                    notes = new int[] { (int)StandardNotes._FSharp, (int)StandardNotes._A, (int)StandardNotes._CSharp };
                    break;
                case (int)StandardNotes._G:
                    notes = new int[] { (int)StandardNotes._G, (int)StandardNotes._ASharp, (int)StandardNotes._D };
                    break;
                case (int)StandardNotes._GSharp:
                    notes = new int[] { (int)StandardNotes._GSharp, (int)StandardNotes._B, (int)StandardNotes._DSharp };
                    break;
                case (int)StandardNotes._A:
                    notes = new int[] { (int)StandardNotes._A, (int)StandardNotes._C, (int)StandardNotes._E };
                    break;
                case (int)StandardNotes._ASharp:
                    notes = new int[] { (int)StandardNotes._ASharp, (int)StandardNotes._CSharp, (int)StandardNotes._F };
                    break;
                case (int)StandardNotes._B:
                    notes = new int[] { (int)StandardNotes._B, (int)StandardNotes._D, (int)StandardNotes._FSharp };
                    break;

            }

            if (inversion == 3) // random
            {
                return GetTriadInversion(notes, Numbers.GetNextRandom(0, 2));
            }

            if (inversion < 3) // 0, 1, 2
            {
                return GetTriadInversion(notes, inversion);
            }

            return notes; // ignore inversion parameter
        }

        public int[] GetDischordantTriad(int rootNote, int inversion)
        {
            return GetRandomTriad(rootNote, inversion); // temporary, replace with dischordant triads
        }

        public int[] GetRandomTriad(int rootNote, int inversion)
        {
            return new int[] { Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 11) };
        }


        public static int [] TwinkleMajor = new int[] // twinkle major
        {
                (int)Notes.ShortC, (int)Notes.ShortC, (int)Notes.ShortG, (int)Notes.ShortG, (int)Notes.ShortA, (int)Notes.ShortA, (int)Notes.LongG,
                (int)Notes.ShortF, (int)Notes.ShortF, (int)Notes.ShortE, (int)Notes.ShortE, (int)Notes.ShortD, (int)Notes.ShortD, (int)Notes.LongC,
        };

        public static int[] TwinkleMinor = new int[] // twinkle minor
        {
                (int)Notes.ShortC, (int)Notes.ShortC, (int)Notes.ShortG, (int)Notes.ShortG, (int)Notes.ShortGSharp, (int)Notes.ShortGSharp, (int)Notes.LongG,
                (int)Notes.ShortF, (int)Notes.ShortF, (int)Notes.ShortDSharp, (int)Notes.ShortDSharp, (int)Notes.ShortD, (int)Notes.ShortD, (int)Notes.LongC,
        };

        public static int[] DarkTheme1 = new int[] // dark theme
        {
               (int)Notes.LongB, (int)Notes.ShortG, (int)Notes.ShortFSharp, (int)Notes.LongE
        };

        public static int[] DarkTheme2 = new int[] // dark theme
        {
                (int)Notes.LongC, (int)Notes.ShortG, (int)Notes.ShortFSharp, (int)Notes.LongE
        };

        public static int[] DarkTheme3 = new int[] // dark theme
        {
                (int)Notes.LongE, (int)Notes.ShortG, (int)Notes.ShortFSharp, (int)Notes.LongE
        };


        public static int[] SessionMelody1 = null;
        public static int[] SessionMelody2 = null;
        public static int[] SessionMelody3 = null;
        public static int[] SessionMelody4 = null;

        public void PopulateSessionMelodys()
        {
            if (SessionMelody1 == null)
            {
                SessionMelody1 = new int[Numbers.GetNextRandom(3, 7)];

                for (int i = 0; i < SessionMelody1.Length; i++)
                {
                    SessionMelody1[i] = Numbers.GetNextRandom(0, 35);
                }
            }

            if (SessionMelody2 == null)
            {
                SessionMelody2 = new int[Numbers.GetNextRandom(4, 7)];

                for (int i = 0; i < SessionMelody2.Length; i++)
                {
                    SessionMelody2[i] = Numbers.GetNextRandom(0, 35);
                }
            }

            if (SessionMelody3 == null)
            {
                SessionMelody3 = new int[Numbers.GetNextRandom(4, 8)];

                for (int i = 0; i < SessionMelody3.Length; i++)
                {
                    SessionMelody3[i] = Numbers.GetNextRandom(0, 35);
                }
            }

            if (SessionMelody4 == null)
            {
                SessionMelody4 = new int[Numbers.GetNextRandom(5, 10)];

                for (int i = 0; i < SessionMelody4.Length; i++)
                {
                    SessionMelody4[i] = Numbers.GetNextRandom(0, 35);
                }
            }

            Console.WriteLine("Session Tunes Created");

        }

        public int[] GetSessionMelody(int num)
        {
            if (SessionMelody1 == null)
            {
                PopulateSessionMelodys();
            }

            int[] sessionMelody = SessionMelody1;

            switch (num)
            {
                case 0:
                    sessionMelody = SessionMelody1;
                    break;
                case 1:
                    sessionMelody = SessionMelody1;
                    break;
                case 2:
                    sessionMelody = SessionMelody2;
                    break;
                case 3:
                    sessionMelody = SessionMelody3;
                    break;
                case 4:
                    sessionMelody = SessionMelody4;
                    break;
                default:
                    sessionMelody = SessionMelody4;
                    break;
            }

            return sessionMelody;
        }

        public int [] GetSessionMelody()
        {
            return GetSessionMelody(Numbers.GetNextRandom(0, 3));
        }

        public int[] GetSignatureMelody()
        {
            int r = Numbers.GetNextRandom(1, 3);

            if (Numbers.GetNextRandom(1, 1000) > 990)
            {
                if (Numbers.GetNextRandom(1, 1000) > 500)
                {
                    return TwinkleMajor;
                }

                if (Numbers.GetNextRandom(1, 1000) > 10)
                {
                    return TwinkleMinor;
                }
            }

            if (r == 1) { return DarkTheme1; }
            else if (r == 2) { return DarkTheme2; }

            return DarkTheme3;
        }

        public int[] GetMinorTune()
        {
            return TwinkleMinor;
        }

        public int[] GetMajorTune()
        {
            return TwinkleMajor;
        }

        public int [] GetRandomTriad()
        {
            int[] notes = null;
            int triadType = Numbers.GetNextRandom(0, 3); // major, minor, dischordant, random
            int rootNote = Numbers.GetNextRandom(0, 11); // A, A#, B, C, D, D#, E, F, F#, G, G#
            int inversion = Numbers.GetNextRandom(0, 2);

            switch(triadType)
            {
                case (int)PhraseFeels.Major:
                    notes = GetMajorTriad(rootNote, inversion);
                    break;
                case (int)PhraseFeels.Minor:
                    notes = GetMinorTriad(rootNote, inversion);
                    break;
                case (int)PhraseFeels.Dischordant:
                    notes = GetDischordantTriad(rootNote, inversion);
                    break;
                case (int)PhraseFeels.Random:
                    notes = GetRandomTriad(rootNote, inversion);
                    break;
            }
            return notes;
        }

        
	}
}

