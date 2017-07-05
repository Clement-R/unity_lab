using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * TODO : Ini settings management (just not used for the moment). Code is in SongProperties class.
 * TODO :
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
        public char key;

        public Beat(byte[] file, ref int nextByteIndex) {
            ppqn = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            position = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            flags = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            key = (char) file.Skip(nextByteIndex).Take(1).First();
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
                tracks.Add(new Track(file, ref nextByteIndex));
            }

            endByteIndex = nextByteIndex;
        }
    }

    public class Track {
        public string name;
        public char format;
        public char behavior;
        public char type;
        public char difficulty;
        public int flags;
        public string alternateName;

        List<Section> sections = new List<Section>();

        int nextByteIndex;

        public Track(byte[] file, ref int nextByteIndex) {
            int nameLength = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            name = String.Join("", (from character in file.Skip(nextByteIndex).Take(nameLength) select ((char)character).ToString()).ToArray());
            nextByteIndex += nameLength;

            format = (char)file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            behavior = (char)file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            type = (char)file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            difficulty = (char)file.Skip(nextByteIndex).Take(1).First();
            nextByteIndex += 1;

            flags = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;

            int trackComplianceFlag = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            int numberOfSection = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            for (int i = 0; i < numberOfSection; i++) {
                sections.Add(new Section(file, ref nextByteIndex));
            }

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
        SectionChunk[] sections;

        public Section(byte[] file, ref int nextByteIndex) {
            type = EOFUtility.bytesToInt16(file.Skip(nextByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            int numberOfSectionChunk = EOFUtility.bytesToInt32(file.Skip(nextByteIndex).Take(4).ToArray());
            nextByteIndex += 4;
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
        public SectionChunk() {

        }
    }

    public class LegacyTrack : Track {
        public LegacyTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }
    }

    public class ProGuitarTrack : Track {
        public ProGuitarTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }
    }

    public class ProKeysTrack : Track {
        public ProKeysTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
        }
    }

    public class LaneLefacyTrack : Track {
        public LaneLefacyTrack(byte[] file, ref int startByteIndex) : base(file, ref startByteIndex) {
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
        }

        char[] getFileHeader(byte[] file) {
            return (from header in file.Take(8) select Convert.ToChar(header)).ToArray();
        }
    }
}
