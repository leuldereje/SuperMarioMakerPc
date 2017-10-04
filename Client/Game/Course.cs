using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;
using CommonLib.Extensions;
using CommonLib.Utils;
using Client.Annotations;
using Client.Components;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverQueried.Global

namespace Client.Game {
  public sealed class Course : INotifyPropertyChanged {
    private byte[] _data;
    private byte[] _dataSub;
    private byte[] _dataSound;
    private byte[] _dataThumb0;
    private byte[] _dataThumb1;
    private string _title;
    private BitmapImage _thumbnail;
    private BitmapImage _thumbnailWide;

    private Course(byte[] courseData, byte[] courseSubData, byte[] soundData, byte[] thumbnail0Data, byte[] thumbnail1Data) {
      _data = courseData;
      _dataSub = courseSubData;
      _dataSound = soundData;
      _dataThumb0 = thumbnail0Data;
      _dataThumb1 = thumbnail1Data;

      Parse();
    }

    // properties
    public DirectoryInfo Directory { get; private set; }

    public string Title {
      get => _title;
      set {
        _title = value;
        OnPropertyChanged();
      }
    }

    public string Author { get; private set; }

    public GameStyle GameStyle { get; private set; }

    public CourseTheme CourseTheme { get; private set; }

    public CourseTheme CourseThemeSub { get; private set; }

    public ushort Time { get; private set; }

    public AutoScroll AutoScroll { get; private set; }

    public AutoScroll AutoScrollSub { get; private set; }

    public ushort Width { get; private set; }

    public ushort WidthSub { get; private set; }

    public List<byte[]> Blocks { get; } = new List<byte[]>();

    public List<byte[]> BlocksSub { get; } = new List<byte[]>();

    public List<byte[]> Sounds { get; } = new List<byte[]>();

    public List<byte[]> SoundsSub { get; } = new List<byte[]>();

    public Sound Sound { get; private set; }

    // ui properties
    public BitmapImage Thumbnail {
      get => _thumbnail;
      set {
        _thumbnail = value;
        OnPropertyChanged();
      }
    }

    public BitmapImage ThumbnailWide {
      get => _thumbnailWide;
      set {
        _thumbnailWide = value;
        OnPropertyChanged();
      }
    }

    // static methods
    public static Course FromDirectory(string directoryPath) {
      var courseData = File.ReadAllBytes(Path.Combine(directoryPath, "course_data.cdt"));
      var courseDataSub = File.ReadAllBytes(Path.Combine(directoryPath, "course_data_sub.cdt"));
      var soundData = File.ReadAllBytes(Path.Combine(directoryPath, "sound.bwv"));
      var thumbnail0Data = File.ReadAllBytes(Path.Combine(directoryPath, "thumbnail0.tnl"));
      var thumbnail1Data = File.ReadAllBytes(Path.Combine(directoryPath, "thumbnail1.tnl"));

      var result = new Course(courseData, courseDataSub, soundData, thumbnail0Data, thumbnail1Data) {
        Directory = new DirectoryInfo(directoryPath)
      };

      return result;
    }

    public static Course FromArchive(string filePath) {
      var files = ZipUtils.GetFiles(filePath);

      var courseData = files["course_data.cdt"];
      var courseDataSub = files["course_data_sub.cdt"];
      var soundData = files["sound.bwv"];
      var thumbnail0Data = files["thumbnail0.tnl"];
      var thumbnail1Data = files["thumbnail1.tnl"];

      var result = new Course(courseData, courseDataSub, soundData, thumbnail0Data, thumbnail1Data);

      return result;
    }

    // methods
    public void Reload() {
      if (Directory != null) {
        var directoryPath = Directory.FullName;

        _data = File.ReadAllBytes(Path.Combine(directoryPath, "course_data.cdt"));
        _dataSub = File.ReadAllBytes(Path.Combine(directoryPath, "course_data_sub.cdt"));
        _dataSound = File.ReadAllBytes(Path.Combine(directoryPath, "sound.bwv"));
        _dataThumb0 = File.ReadAllBytes(Path.Combine(directoryPath, "thumbnail0.tnl"));
        _dataThumb1 = File.ReadAllBytes(Path.Combine(directoryPath, "thumbnail1.tnl"));
      }

      Parse();
    }

    public void Save(string directoryPath) {
      File.WriteAllBytes(Path.Combine(directoryPath, "course_data.cdt"), _data);
      File.WriteAllBytes(Path.Combine(directoryPath, "course_data_sub.cdt"), _dataSub);
      File.WriteAllBytes(Path.Combine(directoryPath, "sound.bwv"), Sound.Data);
      File.WriteAllBytes(Path.Combine(directoryPath, "thumbnail0.tnl"), _dataThumb0);
      File.WriteAllBytes(Path.Combine(directoryPath, "thumbnail1.tnl"), _dataThumb1);

      Directory = new DirectoryInfo(directoryPath);
    }

    public void SaveAs(string filePath) {
      var files = new Dictionary<string, byte[]> {
        {"course_data.cdt", _data},
        {"course_data_sub.cdt", _dataSub},
        {"sound.bwv", _dataSound},
        {"thumbnail0.tnl", _dataThumb0},
        {"thumbnail1.tnl", _dataThumb1}
      };

      ZipUtils.ZipFiles(files, filePath);
    }

    public void SetThumbnail(string filePath) {
      var image = Image.FromFile(filePath).Resize(Constants.Tnl.TNL_DIMENSION[1][0], Constants.Tnl.TNL_DIMENSION[1][1]);
      var imageData = image.ToBytes(ImageFormat.Jpeg);

      if (imageData.Length > Constants.Tnl.TNL_JPEG_MAX_SIZE) {
        throw new Exception($"Images must be less than {Constants.Tnl.TNL_JPEG_MAX_SIZE / 1000.0} KB.");
      }

      var lengthData = BitConverter.GetBytes(((uint) imageData.Length).ToUInt32BE());
      var paddingData = ByteUtils.Allocate(Constants.Tnl.TNL_SIZE - imageData.Length - 8);

      var contentData = ByteUtils.Allocate(Constants.Tnl.TNL_SIZE - 4, lengthData, imageData, paddingData);

      var checksum = Crc32.Compute(contentData).ToUInt32BE();
      var checksumData = ByteUtils.Allocate(4, BitConverter.GetBytes(checksum));

      _dataThumb1 = ByteUtils.Allocate(Constants.Tnl.TNL_SIZE, checksumData, contentData);

      if (Directory != null) {
        Save(Directory.FullName);
      }

      Reload();
    }

    // data parsing
    private void Parse() {
      Sound = new Sound(_dataSound);

      ParseTitle();
      ParseAuthor();
      ParseGameStyle();
      ParseCourseTheme();
      ParseCourseThemeSub();
      ParseTime();
      ParseAutoScroll();
      ParseAutoScrollSub();
      ParseWidth();
      ParseWidthSub();
      ParseBlocks();
      ParseBlocksSub();
      ParseSounds();
      ParseSoundsSub();
      ParseThumbnail0();
      ParseThumbnail1();
    }

    private void ParseTitle() {
      var bytes = _data.Slice(Constants.Course.NAME_OFFSET, Constants.Course.NAME_OFFSET + Constants.Course.NAME_LENGTH);

      var result =
        Encoding.Unicode.GetString(bytes)
          .Trim()
          .TakeWhile(c => c != '\0')
          .Aggregate(string.Empty, (current, c) => current + c);

      Title = result;
    }

    private void ParseAuthor() {
      var bytes = _data.Slice(Constants.Course.MAKER_OFFSET, Constants.Course.MAKER_OFFSET + Constants.Course.MAKER_LENGTH);

      var result =
        Encoding.Unicode.GetString(bytes)
          .Trim()
          .TakeWhile(c => c != '\0')
          .Aggregate(string.Empty, (current, c) => current + c);

      Author = result;
    }

    private void ParseGameStyle() {
      var bytes = _data.Slice(Constants.Course.GAME_STYLE_OFFSET, Constants.Course.GAME_STYLE_OFFSET + 2);

      var str =
        Encoding.UTF8.GetString(bytes)
          .Trim()
          .TakeWhile(c => c != '\0')
          .Aggregate(string.Empty, (current, c) => current + c);

      switch (str.ToLower()) {
        case "m1":
          GameStyle = GameStyle.SuperMarioBros;
          break;
        case "m3":
          GameStyle = GameStyle.SuperMarioBros3;
          break;
        case "mw":
          GameStyle = GameStyle.SuperMarioWorld;
          break;
        case "wu":
          GameStyle = GameStyle.NewSuperMarioBrosU;
          break;
        default:
          throw new ArgumentOutOfRangeException($"Unknown game style: {str}");
      }
    }

    private void ParseCourseTheme() {
      var val = _data[Constants.Course.THEME_OFFSET];

      CourseTheme = (CourseTheme) val;
    }

    private void ParseCourseThemeSub() {
      var val = _dataSub[Constants.Course.THEME_OFFSET];

      CourseThemeSub = (CourseTheme) val;
    }

    private void ParseTime() {
      var bytes = _data.Slice(Constants.Course.TIME_OFFSET, Constants.Course.TIME_OFFSET + 2);

      var val =
        BitConverter
          .ToUInt16(bytes, 0)
          .ToUInt16BE();

      Time = val;
    }

    private void ParseAutoScroll() {
      var val = _data[Constants.Course.AUTO_SCROLL_OFFSET];

      AutoScroll = (AutoScroll) val;
    }

    private void ParseAutoScrollSub() {
      var val = _dataSub[Constants.Course.AUTO_SCROLL_OFFSET];

      AutoScrollSub = (AutoScroll) val;
    }

    private void ParseWidth() {
      var bytes = _data.Slice(Constants.Course.WIDTH_OFFSET, Constants.Course.WIDTH_OFFSET + 2);

      var val =
        BitConverter
          .ToUInt16(bytes, 0)
          .ToUInt16BE();

      Width = val;
    }

    private void ParseWidthSub() {
      var bytes = _dataSub.Slice(Constants.Course.WIDTH_OFFSET, Constants.Course.WIDTH_OFFSET + 2);

      var val =
        BitConverter
          .ToUInt16(bytes, 0)
          .ToUInt16BE();

      WidthSub = val;
    }

    private void ParseBlocks() {
      var bytes = _data.Slice((int) Constants.Course.BLOCK_AMOUNT_OFFSET, (int) Constants.Course.BLOCK_AMOUNT_OFFSET + 2);

      var blockCount =
        BitConverter
          .ToUInt16(bytes, 0)
          .ToUInt16BE();

      for (int i = 0, offset = Constants.Course.BLOCKS_OFFSET; i < blockCount; i++, offset += Constants.Block.SIZE) {
        var blockData = _data.Slice(offset, offset + Constants.Block.SIZE);
        Blocks.Add(blockData);
      }
    }

    private void ParseBlocksSub() {
      var bytes = _dataSub.Slice((int) Constants.Course.BLOCK_AMOUNT_OFFSET, (int) Constants.Course.BLOCK_AMOUNT_OFFSET + 2);

      var blockCount =
        BitConverter
          .ToUInt16(bytes, 0)
          .ToUInt16BE();

      for (int i = 0, offset = Constants.Course.BLOCKS_OFFSET; i < blockCount; i++, offset += Constants.Block.SIZE) {
        var blockData = _dataSub.Slice(offset, offset + Constants.Block.SIZE);
        BlocksSub.Add(blockData);
      }
    }

    private void ParseSounds() {
      for (var offset = Constants.Course.SOUND_OFFSET; offset < Constants.Course.SOUND_END_OFFSET; offset += Constants.Sound.SIZE) {
        if (_data[offset] == 0xFF) {
          continue;
        }

        var soundData = _data.Slice(offset, offset + Constants.Sound.SIZE);
        Sounds.Add(soundData);
      }
    }

    private void ParseSoundsSub() {
      for (var offset = Constants.Course.SOUND_OFFSET; offset < Constants.Course.SOUND_END_OFFSET; offset += Constants.Sound.SIZE) {
        if (_dataSub[offset] == 0xFF) {
          continue;
        }

        var soundData = _dataSub.Slice(offset, offset + Constants.Sound.SIZE);
        SoundsSub.Add(soundData);
      }
    }

    private void ParseThumbnail0() {
      var length =
        BitConverter
          .ToUInt32(_dataThumb0, 4)
          .ToUInt32BE();

      var jpegData = _dataThumb0.Slice(8, 8 + (int) length);

      ThumbnailWide = jpegData.ToBitmapImage();
    }

    private void ParseThumbnail1() {
      var length =
        BitConverter
          .ToUInt32(_dataThumb1, 4)
          .ToUInt32BE();

      var jpegData = _dataThumb1.Slice(8, 8 + (int) length);

      Thumbnail = jpegData.ToBitmapImage();
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}