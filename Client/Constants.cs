using CommonLib.Extensions;

namespace Client {
  public static class Constants {
    public static class Process {
      public const string CEMU = "cemu";
    }

    public static class System {
      public const string COURSE_PACKAGE_FILE_EXT = ".smmcp";
      public const string COURSE_PACKAGE_FILE_EXT_KEY = "SuperMarioMaker";
      public const string COURSE_PACKAGE_FILE_EXT_DESCRIPTION = "Super Mario Maker Course Package";
    }

    public static class Web {
      public const string CEMU_URL = "http://cemu.info/";
    }

    public static class Crc32 {
      public const uint DEFAULT_POLYNOMIAL = 0xedb88320u;
      public const uint DEFAULT_SEED = 0xffffffffu;
    }

    public static class Keyboard {
      public const int WH_KEYBOARD_LL = 13;
      public const int WM_KEYDOWN = 0x100;
      public const int WM_KEYUP = 0x101;
      public const int WM_SYSKEYDOWN = 0x104;
      public const int WM_SYSKEYUP = 0x105;
      public const byte VK_SHIFT = 0x10;
      public const byte VK_CONTROL = 0x11;
      public const byte VK_ALT = 0x12;
      public const byte VK_CAPITAL = 0x14;
      public const byte VK_NUMLOCK = 0x90;
    }

    // see: https://github.com/Tarnadas/cemu-smm/blob/master/src/course.js
    public static class Course {
      public const int SIZE = 0x15000;
      public const int CRC_LENGTH = 0x10;
      public static readonly byte[] CRC_PRE_BUF = "000000000000000B".HexToBytes();
      public static readonly byte[] CRC_POST_BUF = new byte[4];
      public const int TIMESTAMP_0_OFFSET = 0x1;
      public const int TIMESTAMP_1_OFFSET = 0x14;
      public const int NAME_OFFSET = 0x29;
      public const int NAME_LENGTH = 0x40;
      public const int MAKER_OFFSET = 0x92;
      public const int MAKER_LENGTH = 0x14;
      public const int GAME_STYLE_OFFSET = 0x6A;
      public const int THEME_OFFSET = 0x6D;
      public const int TIME_OFFSET = 0x70;
      public const int AUTO_SCROLL_OFFSET = 0x72;
      public const int WIDTH_OFFSET = 0x76;
      public const uint BLOCK_AMOUNT_OFFSET = 0xEE;
      public const int BLOCKS_OFFSET = 0xF0;
      public const int SOUND_OFFSET = 0x145F0;
      public const int SOUND_END_OFFSET = 0x14F50;
    }

    // see: https://github.com/Tarnadas/cemu-smm/blob/master/src/block.js
    public static class Block {
      public const int SIZE = 0x20;
      public const int COORDINATE_MULTIPLIER = 0xA0;
      public const ushort LOC_X_OFFSET = 2;
      public const ushort LOC_Y_OFFSET = 8;
      public const int LOC_X_MAX = 0x95B0;
      public const int LOC_Y_MAX = 0x1090;
      public const byte DIMENSION_OFFSET = 0xA;
      public const byte ORIENTATION_OFFSET = 0xF;
      public const ushort Z_INDEX_OFFSET = 0x6;
      public const ushort ENTITY_TYPE_OFFSET = 0x4;
      public const byte TYPE_OFFSET = 0x18;
      public const byte LINK_OFFSET = 0xD;
      public const ushort ID_OFFSET = 0X1A;
      public const byte COSTUME_OFFSET = 0x1E;

      public static readonly byte[] BLOCK_DEFAULT =
        "00 00 00 00 00 00 00 00 00 00 00 00 06 00 08 40 06 00 00 40 00 00 00 00 00 FF FF FF FF FF FF FF"
          .Replace(" ", string.Empty)
          .HexToBytes();
    }

    // see: https://github.com/Tarnadas/cemu-smm/blob/master/src/sound.js
    public static class Sound {
      public const int SIZE = 8;
      public const ushort LOC_X_OFFSET = 3;
      public const ushort LOC_Y_OFFSET = 4;
      public const ushort TYPE_OFFSET = 0;
      public const ushort VARIATION_OFFSET = 2;
      public static readonly byte[] SOUND_DEFAULT = "FFFF00FFFF000000".HexToBytes();
    }

    // see: https://github.com/Tarnadas/cemu-smm/blob/master/src/tnl.js
    public static class Tnl {
      public const ushort TNL_SIZE = 0xC800;
      public const ushort TNL_JPEG_MAX_SIZE = 0xC7F8;

      public static readonly int[][] TNL_DIMENSION = {
        new[] {720, 81},
        new[] {320, 240}
      };

      public static readonly int[] TNL_ASPECT_RATIO = {
        TNL_DIMENSION[0][0] / TNL_DIMENSION[0][1],
        TNL_DIMENSION[1][0] / TNL_DIMENSION[1][1]
      };

      public static readonly double[] TNL_ASPECT_RATIO_THRESHOLD = {3.5, 0.3};
    }
  }

  public enum GameStyle {
    SuperMarioBros,
    SuperMarioBros3,
    SuperMarioWorld,
    NewSuperMarioBrosU
  }

  public enum CourseTheme {
    Ground,
    Underground,
    Castle,
    Airship,
    Underwater,
    GhostHouse
  }

  public enum AutoScroll {
    Disabled,
    Slow,
    Medium,
    Fast
  }

  public enum EntityType : int {
    Static = 0,
    Living = 1,
    Platform = 0xFFFF
  }
}