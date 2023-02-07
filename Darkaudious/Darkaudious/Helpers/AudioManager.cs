using System;
namespace Darkaudious.Helpers
{
	public class AudioManager
	{
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


        public int GetNote(int length, int note)
        {
            int noteId = 0;

            noteId = note * length;
            
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
                case (int)StandardNotes._A:
                    notes = new int [] { (int)StandardNotes._A, (int)StandardNotes._CSharp, (int)StandardNotes._E };
                    break;
                case (int)StandardNotes._ASharp:
                    notes = new int[] { (int)StandardNotes._ASharp, (int)StandardNotes._D, (int)StandardNotes._F };
                    break;
                case (int)StandardNotes._B:
                    notes = new int[] { (int)StandardNotes._B, (int)StandardNotes._DSharp, (int)StandardNotes._FSharp };
                    break;
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
                case (int)StandardNotes._A:
                    notes = new int[] { (int)StandardNotes._A, (int)StandardNotes._C, (int)StandardNotes._E };
                    break;
                case (int)StandardNotes._ASharp:
                    notes = new int[] { (int)StandardNotes._ASharp, (int)StandardNotes._CSharp, (int)StandardNotes._F };
                    break;
                case (int)StandardNotes._B:
                    notes = new int[] { (int)StandardNotes._B, (int)StandardNotes._D, (int)StandardNotes._FSharp };
                    break;
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

        public int [] GetRandonTriad()
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

        public AudioManager()
		{
		}
	}
}

