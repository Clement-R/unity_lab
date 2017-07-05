﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * TODO : Ini settings management (just not used for the moment) 
 * TODO :
*/
namespace EditorOnFireFileParser {
    public class ChartProperties {
        public string projectRevisionNumber;
        public bool timingFormat;
        public int timeDivision = 0; // Unused for the moment

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
                // iniStrings = getIniStrings(file.Skip(25).Take(2).ToArray(), ref nextByteindex);
            }

            numberOfIniBoolean = EOFUtility.bytesToInt16(file.Skip(nextByteindex).Take(2).ToArray());
            nextByteindex += 2;
            if (numberOfIniBoolean != 0) {
                // iniBooleans = getIniBooleans(file.Skip(nextByteindex).Take(2).ToArray(), ref nextByteindex);
            }

            numberOfIniNumber = EOFUtility.bytesToInt16(file.Skip(nextByteindex).Take(2).ToArray());
            nextByteindex += 2;

            if (numberOfIniNumber != 0) {
                iniNumbers = getIniNumbers(file, ref nextByteindex, numberOfIniNumber);
            }
            
            endByteIndex = nextByteindex;
        }

        IniString[] getIniStrings(byte[] section, ref int nextByteIndex) {
            nextByteIndex = 0;
            return null;
        }

        IniBoolean[] getIniBooleans(byte[] section, ref int nextByteIndex) {
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
    public class IniString {
        int stringType;
        int stringLength;
        string str;

        public IniString(byte[] section) {

        }
    }

    /*
        *	1 byte:		INI boolean type (0=Reserved,128-255=DISALLOWED,1=Lyrics present,2=Eighth note HOPO,3=Guitar fret hand pos of 0,4=Bass fret hand pos of 0,5=Tempo map locked,6=Expert+ drums disabled,7=Click/drag disabled,8=Export RS chord techniques,9=Unshare drum phrasing,10=Highlight non grid snapped notes,11=Accurate T/S,12=Highlight notes in arpeggios,13=Suppress DD warnings)
        !	The MSB is the boolean status of the INI setting (1 is True), allowing for an explicit "value = False" setting to be stored if necessary
        !	The low 7 bits represents the boolean INI setting in question, numbered from 0 to 127 (number 0 reserved for future use)
        !	Tutorial songs are hidden during quickplay in FoFiX
    */
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
            type = String.Join(".", (from header in section.Take(1) select Convert.ToString(header)).ToArray());

            // type = EOFUtility.bytesToChar(section.Take(1).ToArray());
            value = EOFUtility.bytesToInt32(section.Skip(1).Take(4).ToArray());
        }
    }

    public class ChartData {
        public int endByteIndex;

        int numberOfOGGProfiles = 0; // 2 bytes
        OGGProfil[] OGGProfiles;

        int numberOfBeats = 0; // 4 bytes
        Beat[] beats;

        int numberOfTextEvents = 0; // 4 bytes
        TextEvent[] textEvents;

        int numberOfCustomDataBlock = 0; // 4 bytes
        CustomDataBlock[] customDataBlocks;

        int nextByteIndex;

        public ChartData(byte[] file, int startByteIndex) {
            nextByteIndex = startByteIndex;

            numberOfOGGProfiles = EOFUtility.bytesToInt16(file.Skip(startByteIndex).Take(2).ToArray());
            nextByteIndex += 2;

            for (int i = 0; i < numberOfOGGProfiles; i++) {
                OGGProfil profil = new OGGProfil(file, ref nextByteIndex);
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

    }

    /*
        *	2 bytes:	Text event string length
        *	[varies:]	Text event string
	        4 bytes:	Associated beat number
        *	2 bytes:	Associated track number (0 if it applies to the entire chart, such as generic section markers)
	        2 bytes:	Text event flags (1 = Rocksmith phrase)
    */
    public class TextEvent {

    }

    /*
        *	4 bytes:	The number of bytes this custom data block consumes (including the block ID and the custom data itself)
        *	4 bytes:	Custom data block ID (1 = Raw MIDI data, 2 = Floating point beat timings, 3 = Start/end points, 0xFFFFFFFF = Empty debugging block)
        *	[varies:]	(Custom data, can store anything)
    */
    public class CustomDataBlock {

    }

    public class TrackData {
        Track[] tracks;

        public TrackData() {
            int numberOfTracks = 0; // 4 bytes

            for (int i = 0; i < numberOfTracks; i++) {

            }
        }
    }

    public class Track {
        string name;
        int format;
        int behavior;
        int type;
        int difficulty;
        // flags
        string alternateName;

        Section[] sections;
    }

    public class Section {
        int type;
        SectionChunk[] sections;
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

    }

    public class LegacyTrack : Track {

    }

    public class ProGuitarTrack : Track {

    }

    public class ProKeysTrack : Track {

    }

    public class LaneLefacyTrack : Track {

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
            // TrackData tracks = new TrackData(file, );

        }

        char[] getFileHeader(byte[] file) {
            return (from header in file.Take(8) select Convert.ToChar(header)).ToArray();
        }
    }
}
