using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * TODO : Ini settings management (just not used for the moment). Code is in SongProperties class.
*/
namespace EditorOnFireFileParser {

    public class ChartProperties {
        public string projectRevisionNumber;
        public bool timingFormat;
        public int timeDivision = 0; // Unused for the moment in EOF

        public ChartProperties(byte[] file) {
            // Get project revision number
            // Get 4 bytes that describe the number and join them into a string with a comprehension list
            projectRevisionNumber = String.Join(".", (from header in file.Skip(16).Take(4) select Convert.ToString(header)).ToArray());

            timingFormat = Convert.ToBoolean(file.Skip(20).Take(1).First());

            timeDivision = EOFUtility.bytesToInt32(file.Skip(21).Take(4).ToArray());
        }
    }

    public class SongProperties {
        // Store the index of the last byte used for song properties section
        // It will be used to know where the next section is starting
        public int endByteIndex;

        int numberOfIniStrings;
        int numberOfIniBoolean;
        int numberOfIniNumber;

        int nextByteindex = 25;

        IniString[] iniStrings;
        IniBoolean[] iniBooleans;
        IniNumber[] iniNumbers;

        public SongProperties(byte[] file) {
            numberOfIniStrings = EOFUtility.bytesToInt16(file.Skip(25).Take(2).ToArray());
            nextByteindex += 2;

            if (numberOfIniStrings != 0) {
                // TODO : iniStrings data extraction
                // iniStrings = getIniStrings(file.Skip(25).Take(2).ToArray(), ref nextByteindex);
            }

            numberOfIniBoolean = EOFUtility.bytesToInt16(file.Skip(nextByteindex).Take(2).ToArray());
            nextByteindex += 2;
            if (numberOfIniBoolean != 0) {
                // TODO : iniBooleans data extraction
                // iniBooleans = getIniBooleans(file.Skip(nextByteindex).Take(2).ToArray(), ref nextByteindex);
            }

            numberOfIniNumber = EOFUtility.bytesToInt16(file.Skip(nextByteindex).Take(2).ToArray());
            nextByteindex += 2;

            if (numberOfIniNumber != 0) {
                iniNumbers = getIniNumbers(file, ref nextByteindex, numberOfIniNumber);
            }

            endByteIndex = nextByteindex;
        }

        IniString[] getIniStrings(byte[] file, ref int nextByteIndex) {
            nextByteIndex = 0;
            return null;
        }

        IniBoolean[] getIniBooleans(byte[] file, ref int nextByteIndex) {
            nextByteIndex = 0;
            return null;
        }

        IniNumber[] getIniNumbers(byte[] file, ref int nextByteIndex, int numberOfIniNumber) {
            List<IniNumber> iniNumbers = new List<IniNumber>();

            file.Skip(nextByteindex).Take(2).ToArray();

            for (int i = 0; i < numberOfIniNumber; i++) {
                iniNumbers.Add(new IniNumber(file.Skip(nextByteindex).Take(5).ToArray()));
                nextByteIndex += 5;
            }

            return iniNumbers.ToArray();
        }
    }

    /*
        *	1 byte:		INI string type (0=Custom,1=Album,2=Artist,3=Title,4=Frettist,5=Genre,6=Year,7=Loading Text,8=Album,#=Icon ID,#=Unlock ID,#=Unlock ID required,#=Unlock text string,...)
        !	The following Icon IDs are supportedly natively in FoFiX: rb1,rb2,rbdlc,rbtpk,gh1,gh2,gh2dlc,gh3,gh3dlc,gh80s,gha,ghm,ph1,ph2,ph3,ph4,phm.  Custom icon strings can be used in FoFiX.
        !	Unlock text string is the text that FoFiX will display if this chart is not unlocked
        *	2 bytes:	INI string length
        *	[varies:]	INI string
    */
    // TODO : IniString class implementation
    public class IniString {
        int stringType;
        int stringLength;
        string str;

        public IniString(byte[] file) {

        }
    }

    /*
        *	1 byte:		INI boolean type (0=Reserved,128-255=DISALLOWED,1=Lyrics present,2=Eighth note HOPO,3=Guitar fret hand pos of 0,4=Bass fret hand pos of 0,5=Tempo map locked,6=Expert+ drums disabled,7=Click/drag disabled,8=Export RS chord techniques,9=Unshare drum phrasing,10=Highlight non grid snapped notes,11=Accurate T/S,12=Highlight notes in arpeggios,13=Suppress DD warnings)
        !	The MSB is the boolean status of the INI setting (1 is True), allowing for an explicit "value = False" setting to be stored if necessary
        !	The low 7 bits represents the boolean INI setting in question, numbered from 0 to 127 (number 0 reserved for future use)
        !	Tutorial songs are hidden during quickplay in FoFiX
    */
    // TODO : IniBoolean class implementation
    public class IniBoolean {

    }

    /*
        *	1 byte:		INI number type (0=Reserved,1=Cassette color,2=Band difficulty level,3=HOPO frequency,4=User specified chart version #,...)
        *	4 bytes:	INI number value
        !	Cassette color is an 8 bit intensity each for Red, Green and Blue
        !	HOPO frequency is 0-5, and is used if the player's "Song HOPO Freq" FoFiX setting is set to "Auto"
    */

    // TODO : Rework to use a Color variable if the type is 1
    public class IniNumber {
        public string type;
        public int value;

        public IniNumber(byte[] section) {
            // type = Convert.ToString(section.Take(1).ToArray());
            type = String.Join("", (from header in section.Take(1) select Convert.ToString(header)).ToArray());

            // type = EOFUtility.bytesToChar(section.Take(1).ToArray());
            value = EOFUtility.bytesToInt32(section.Skip(1).Take(4).ToArray());
        }
    }

    public class ChartData {
        public int endByteIndex;

        int numberOfOGGProfiles = 0; // 2 bytes
        List<OGGProfil> OGGProfiles = new List<OGGProfil>();

        int numberOfBeats = 0; // 4 bytes
        List<Beat> beats = new List<Beat>();

        int numberOfTextEvents = 0; // 4 bytes
        List<TextEvent> textEvents = new List<TextEvent>();

        int numberOfCustomDataBlock = 0; // 4 bytes
        List<CustomDataBlock> customDataBlocks = new List<CustomDataBlock>();

        int nextByteIndex;

        public ChartData(byte[] file, int startByteIndex) {
            nextByteIndex = startByteIndex;

            // OGG data extraction
            numberOfOGGProfiles = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            for (int i = 0; i < numberOfOGGProfiles; i++) {
                OGGProfiles.Add(new OGGProfil(file, ref nextByteIndex));
            }

            // Beat data extraction
            numberOfBeats = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray()); ;
            nextByteIndex += 4;

            for (int i = 0; i < numberOfBeats; i++) {
                beats.Add(new Beat(file, ref nextByteIndex));
            }

            // Text event data extraction
            numberOfTextEvents = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray()); ;
            nextByteIndex += 4;

            for (int i = 0; i < numberOfTextEvents; i++) {
                textEvents.Add(new TextEvent(file, ref nextByteIndex));
            }

            // Custom data block extraction
            numberOfCustomDataBlock = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray()); ;
            nextByteIndex += 4;

            for (int i = 0; i < numberOfCustomDataBlock; i++) {
                customDataBlocks.Add(new CustomDataBlock(file, ref nextByteIndex));
            }

            endByteIndex = nextByteIndex;
        }
    }

    /*
        *	2 bytes:	OGG file name string length
	        [varies:]	OGG file name string
        *	2 bytes:	Original audio file name string length
        *	[varies]:	Original audio file name
        *	2 bytes:	OGG profile description string length
        *	[varies:]	OGG profile description string
	        4 bytes:	MIDI delay for this profile (milliseconds)
        *	4 byte:		OGG profile flags (such as whether the file was originally provided as an OGG or if it was re-encoded, is being mixed with active OGG profile audio, etc)
    */
    public class OGGProfil {
        string name;
        string filename;
        string description;
        int delay;
        int flags;

        public OGGProfil(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            name = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char) character).ToString()).ToArray());
            nextByteIndex += nameLength;

            // Not used by EOF the moment, always 0
            int filenameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            int oggProfileDescriptionLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            description = String.Join("", (from character in file.Skip(nextByteIndex).Take(oggProfileDescriptionLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += oggProfileDescriptionLength;

            delay = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;
        }
    }

    /*
            4 bytes:	PPQN
	        4 bytes:	Position (in milliseconds or delta ticks)
	        4 bytes:	Beat flags (highest byte stores a custom TS numerator, second highest byte stores a custom TS denominator)
        *	1 byte:		Key signature (If negative, the value defines the number of flats present, ie. -2 is Bb.  If positive, the value defines the number of sharps present, ie. 4 is E)
    */
    public class Beat {
        public int ppqn;
        public int position;
        public int flags;
        public byte key;

        public Beat(byte[] file, ref int nextByteIndex) {
            ppqn = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            position = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            key = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;
        }
    }

    /*
        *	2 bytes:	Text event string length
        *	[varies:]	Text event string
	        4 bytes:	Associated beat number
        *	2 bytes:	Associated track number (0 if it applies to the entire chart, such as generic section markers)
	        2 bytes:	Text event flags (1 = Rocksmith phrase)
    */
    public class TextEvent {
        public string description;
        public int beat;
        public int trackNumber;
        public int flags;

        public TextEvent(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            description = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            beat = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            trackNumber = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            flags = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;
        }
    }

    /*
        *	4 bytes:	The number of bytes this custom data block consumes (including the block ID and the custom data itself)
        *	4 bytes:	Custom data block ID (1 = Raw MIDI data, 2 = Floating point beat timings, 3 = Start/end points, 0xFFFFFFFF = Empty debugging block)
        *	[varies:]	(Custom data, can store anything)
    */
    public class CustomDataBlock {
        public int id;
        public byte[] data;

        public CustomDataBlock(byte[] file, ref int nextByteIndex) {
            int numberOfBytes = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            id = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            data = file.Skip(nextByteIndex).Take(numberOfBytes - 4).ToArray();
            nextByteIndex += numberOfBytes - 4;
        }
    }

    public class TrackData {
        public int endByteIndex;
        public List<Track> tracks = new List<Track>();

        int nextByteIndex;

        public TrackData(byte[] file, int startByteIndex) {
            nextByteIndex = startByteIndex;

            int numberOfTracks = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfTracks; i++) {
                string format = getTrackFormat(file, nextByteIndex);

                // Available formats in EOF file description
                // 0=Global,1=Legacy,2=Vocal,3=Pro Keys,4=Pro Guitar/Bass,5=Variable Lane Legacy,...
                switch (format) {
                    case "0":
                        GeneralTrack generalTrack = new GeneralTrack(file, ref nextByteIndex);
                        tracks.Add(generalTrack);
                        break;
                    case "1":
                        LegacyTrack legacyTrack = new LegacyTrack(file, ref nextByteIndex);
                        tracks.Add(legacyTrack);
                        break;
                    case "2":
                        VocalTrack vocalTrack = new VocalTrack(file, ref nextByteIndex);
                        tracks.Add(vocalTrack);
                        break;
                    case "3":
                        ProKeysTrack proKeysTrack = new ProKeysTrack(file, ref nextByteIndex);
                        tracks.Add(proKeysTrack);
                        break;
                    case "4":
                        ProGuitarTrack proGuitarTrack = new ProGuitarTrack(file, ref nextByteIndex);
                        tracks.Add(proGuitarTrack);
                        break;
                    case "5":
                        LaneLegacyTrack laneLegacyTrack = new LaneLegacyTrack(file, ref nextByteIndex);
                        tracks.Add(laneLegacyTrack);
                        break;
                }
            }

            endByteIndex = nextByteIndex;
        }

        string getTrackFormat(byte[] file, int startByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(startByteIndex).Take(2).ToArray());
            startByteIndex += 2;

            startByteIndex += nameLength;
            byte format = file.Skip(startByteIndex).Take(1).First();

            startByteIndex += 1;
            return Convert.ToString(format);
        }
    }

    public class Track {
        public string name;
        public byte format;
        public byte behavior;
        public byte type;
        public byte difficulty;
        public int flags;
        public string alternateName;

        protected List<Section> sections = new List<Section>();

        int nextByteIndex;

        public Track(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            name = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            format = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            behavior = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            type = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            difficulty = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            flags = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            int trackComplianceFlag = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            getTrackData(file, ref nextByteIndex);

            sections = getSections(file, ref nextByteIndex);

            getCustomDataBlocks(file, ref nextByteIndex);
        }

        virtual protected void getTrackData(byte[] file, ref int nextByteIndex) {
        }

        protected List<Section> getSections(byte[] file, ref int nextByteIndex) {
            List<Section> sections = new List<Section>();

            int numberOfSection = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            for (int i = 0; i < numberOfSection; i++) {
                sections.Add(new Section(file, ref nextByteIndex));
            }

            return sections;
        }

        protected void getCustomDataBlocks(byte[] file, ref int nextByteIndex) {
            int numberOfCustomDataBlock = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfCustomDataBlock; i++) {
                // TODO : Manage custom data block
                int tmp = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
                nextByteIndex += 4;

                nextByteIndex += 4;

                nextByteIndex += tmp - 4;
            }
        }
    }

    public class Section {
        int type;
        List<SectionChunk> sections = new List<SectionChunk>();

        public Section(byte[] file, ref int nextByteIndex) {
            type = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            int numberOfSectionChunk = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfSectionChunk; i++) {
                sections.Add(new SectionChunk(file, ref nextByteIndex));
            }
        }
    }

    /*
        *			2 bytes:	Section name string length
        *			[varies:]	Section name string
        *			1 byte:		The type/difficulty that this phrase applies to (0xFF if it applies to all difficulties instead of just one, such as with a solo phrase)
        *			4 bytes:	Start position of section (in milliseconds or delta ticks)
        *			4 bytes:	End position of section (in milliseconds or delta ticks, this is used to store other data for sections that have no end position, such as for fret hand positions and bookmarks)
        *			4 bytes:	Section flags
    */
    public class SectionChunk {
        string name;
        byte difficulty;
        int startPosition;
        int endPosition;
        byte[] flags;

        public SectionChunk(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            name = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            difficulty = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            startPosition = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            endPosition = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = file.Skip(nextByteIndex).Take(4).ToArray();
            nextByteIndex += 4;
        }
    }

    public class GeneralTrack : Track {
        public GeneralTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
        }
    }

    // TODO : Track data
    /*
        *	1 byte:		The number of lanes, keys, etc. used in this track (5 for a standard legacy track, 6 for legacy track with open strum, etc.)
	        4 bytes:	Number of notes
	        NOTE CHUNK, for each note
	        {}
    */
    public class LegacyTrack : Track {
        byte numberOfLanes;
        int numberOfNotes;
        List<LegacyNote> notes = new List<LegacyNote>();

        public LegacyTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
            numberOfLanes = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            numberOfNotes = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfNotes; i++) {
                notes.Add(new LegacyNote(file, ref nextByteIndex));

                // Debug.Log(notes[i].bitFlag);
            }
        }
    }

    /*
        * 2 bytes:	Chord name string length
        * [varies:]	Chord name string
		1 byte:		Note type (difficulty)
		1 byte:		Note bitflag (on/off statuses)
		4 bytes:	Note position (in milliseconds or delta ticks)
		4 bytes:	Note length (in milliseconds or delta ticks)
        * 4 bytes:	Note flags
        [4 bytes:]	Extended note flags (if the MSB of the note flags field is set, another 4 byte flag field follows, and if its MSB is set, another 4 byte flag field, etc)
    */
    public class LegacyNote {
        public string chordName;
        public byte difficulty;
        public byte bitFlag;
        public int position;
        public int length;
        public byte[] flags;

        public LegacyNote(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            chordName = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            difficulty = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            bitFlag = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            position = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            length = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = file.Skip(nextByteIndex).Take(4).ToArray();
            nextByteIndex += 4;

            if(flags[0] == 2) {
                Debug.Log("STAR POWER !");
            }
        }
    }

    /*
        1 byte:		Tone set number assigned to this track (ie. 0=Grand Piano, 1=MIDI device, ...)
        4 bytes:	Number of lyrics
        LYRIC CHUNK, for each lyric:
        {}
    */
    public class VocalTrack : Track {
        byte toneSet;
        List<LyricChunk> lyrics = new List<LyricChunk>();

        public VocalTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
            toneSet = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            int numberOfLyrics = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfLyrics; i++) {
                lyrics.Add(new LyricChunk(file, ref nextByteIndex));
            }
        }
    }

    /*
        2 bytes:	Lyric text string length
	    [varies:]	Lyric text string
	    1 byte:		Lyric set number (0=PART VOCALS, 1=HARM1, 2=HARM2...)
	    1 byte:		Note pitch
	    4 bytes:	Lyric position (in milliseconds or delta ticks)
	    4 bytes:	Lyric length (in milliseconds or delta ticks)
	    2 bytes:	Lyric flags
	    [4 bytes:]	Extended lyric flags (if the MSB of the note flags field is set, another 4 byte flag field follows, and if its MSB is set, another 4 byte flag field, etc)
    */
    public class LyricChunk {
        string name;
        byte lyricSet;
        byte notePitch;
        int position;
        int length;
        byte[] flags;

        public LyricChunk(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            name = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            lyricSet = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            notePitch = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            position = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            length = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = file.Skip(nextByteIndex).Take(2).ToArray();
            nextByteIndex += 2;
        }
    }

    /*
        *	1 byte:		The highest fret number allowed in this track (17 for Mustang, 22 for Squier)
        *	1 byte:		The number of strings on this track's guitar (ie. 4 for standard bass guitar, 6 for standard guitar, for now, the max number supported is 8)
        *	PRO GUITAR TUNING CHUNK, for each string (starting from the string for lane 1):
        *	{
        *		1 byte:		(Signed value) The number of half steps above or below (negative value) the standard tuning for this string
        *	}
        !		The first string defined refers to the lowest guage string (low E on a standard guitar)
        !		The tuning information will be written to song.ini from lowest to highest guage, ie. "real_guitar_tuning (-2 0 0 0 0 0)" would indicate drop D tuning

        *	4 bytes:	Number of notes
        *	PRO GUITAR NOTE CHUNK, for each note
        *	{
        *		2 bytes:	Chord name string length
        *		[varies:]	Chord name string
        *		1 byte:		Chord number
        *		1 byte:		Note type (difficulty)
        *		1 byte:		Note bitflag (string use statuses: set=played, reset=not played)
        !				Bit 0 refers to lane 1 (ie. string 6, low E), bit 5 refers to lane 6 (ie. string 1, high E), consistent with guitar terminology
        !		1 byte:		Ghost bitflag (specifies which lanes in the note are treated as "ghost" notes, ie. for Arpeggio phrases)
        *		FRET CHUNK, for each set bit in the guitar note bitflag
        *		{
        *			1 byte:		Fret # tab (0=Open strum, #=Fret # pressed, 0xFF=Muted (no fret specified), if MSB is set, the string is considered muted)
        *		}
        *		1 byte:		Legacy bitflags (the 5 least significant bits represent the lanes that would be set when pasting this note into a legacy track)
        !			If the MSB is set, this bitmask was user-defined, otherwise it will be dynamically defined during the paste operation (basically just keep lanes 1-5 set/clear based on the pro guitar note's lanes)
        *		4 bytes:	Note position (in milliseconds or delta ticks)
        *		4 bytes:	Note length (in milliseconds or delta ticks)
        *		4 bytes:	Note flags (allow 32 flags for an unforsee-able number of techniques such as tapping, bending, etc)
		        [1 byte:]	Slide ending fret (if the Rocksmith notation flag, decimal value 33554432, is set and this note is flagged as having an up/down slide, value 4096 or 8192)
		        [1 byte:]	Bend strength in half/quarter steps (if the Rocksmith notation flag, decimal value 33554432, is set and this note is flagged as a bend, value 2097152)
				        (If the MSB of this value is set, the other 7 bits define the number of quarter steps the bend is, otherwise the value is in half steps)
		        [1 byte:]	Unpitched slide ending fret (if this note is flagged as having an unpitched slide, value 256)
		        [4 bytes:]	Extended note flags (if the MSB of the note flags field is set, another 4 byte flag field follows, and if its MSB is set, another 4 byte flag field, etc)
        *	}

    */
    // Not used, we just move the byte index but we didn't store data
    public class ProGuitarTrack : Track {
        public ProGuitarTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
            nextByteIndex += 1;
            byte numberOfStrings = file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            for (int i = 0; i < numberOfStrings; i++) {
                nextByteIndex += 1;
            }

            int numberOfNotes = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            for (int i = 0; i < numberOfNotes; i++) {
                int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
                nextByteIndex += 2;
                nextByteIndex += nameLength;
                nextByteIndex += 1; // Chord number
                nextByteIndex += 1; // Difficulty
                nextByteIndex += 1; // Note bitflag
                Debug.Log(nextByteIndex);
                nextByteIndex += 1; // Ghost bitflag

                /*
                for (int i = 0; i < bitInGhostBitflag; i++) {

                }
                */

                nextByteIndex += 1; // Legacy bitflag
                nextByteIndex += 4; // Note position
                nextByteIndex += 4; // Note length
                nextByteIndex += 4; // Note flags
            }
        }
    }

    // TODO : Track data
    // Unused for the moment in our project
    public class ProKeysTrack : Track {
        public ProKeysTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
        }
    }

    // TODO : Track data
    // Unused for the moment in our project
    public class LaneLegacyTrack : Track {
        public LaneLegacyTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }

        override protected void getTrackData(byte[] file, ref int nextByteIndex) {
        }
    }

    public class EOFFileReader : MonoBehaviour {

        void Start() {
            // Read file
            byte[] file = File.ReadAllBytes("Assets/EditorOnFireFileParser/Resources/notes_2.eof");

            // Get file header
            char[] fileHeader = getFileHeader(file);

            // Get chart properties
            ChartProperties chartProperties = new ChartProperties(file);

            // Get song properties
            SongProperties songProperties = new SongProperties(file);

            // Get chart data
            ChartData chartData = new ChartData(file, songProperties.endByteIndex);

            // Get tracks data
            TrackData tracks = new TrackData(file, chartData.endByteIndex);

            Debug.Log(tracks.endByteIndex);
        }

        char[] getFileHeader(byte[] file) {
            return (from header in file.Take(8) select Convert.ToChar(header)).ToArray();
        }
    }
}
