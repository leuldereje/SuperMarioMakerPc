using System;
using System.Collections.Generic;
using System.Security.Cryptography;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SuggestBaseTypeForParameter

namespace Client.Components {
  // slightly modified from: https://github.com/damieng/DamienGKit/blob/master/CSharp/DamienG.Library/Security/Cryptography/Crc32.cs
  public class Crc32 : HashAlgorithm {
    private static uint[] _defaultTable;

    private readonly uint _seed;
    private readonly uint[] _table;
    private uint _hash;

    public Crc32()
      : this(Constants.Crc32.DEFAULT_POLYNOMIAL, Constants.Crc32.DEFAULT_SEED) { }

    public Crc32(uint polynomial, uint seed) {
      _table = InitializeTable(polynomial);
      _seed = _hash = seed;
    }

    // properties
    public override int HashSize => 32;

    // overrides
    public override void Initialize() {
      _hash = _seed;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize) {
      _hash = CalculateHash(_table, _hash, array, ibStart, cbSize);
    }

    protected override byte[] HashFinal() {
      var hashBuffer = UInt32ToBigEndianBytes(~_hash);
      HashValue = hashBuffer;
      return hashBuffer;
    }
    
    // static methods
    public static uint Compute(byte[] buffer) {
      return Compute(Constants.Crc32.DEFAULT_SEED, buffer);
    }

    public static uint Compute(uint seed, byte[] buffer) {
      return Compute(Constants.Crc32.DEFAULT_POLYNOMIAL, seed, buffer);
    }

    public static uint Compute(uint polynomial, uint seed, byte[] buffer) {
      return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
    }

    private static uint[] InitializeTable(uint polynomial) {
      if (polynomial == Constants.Crc32.DEFAULT_POLYNOMIAL && _defaultTable != null) {
        return _defaultTable;
      }

      var createTable = new uint[256];

      for (var i = 0; i < 256; i++) {
        var entry = (uint) i;
        for (var j = 0; j < 8; j++)
          if ((entry & 1) == 1) {
            entry = (entry >> 1) ^ polynomial;
          }
          else {
            entry = entry >> 1;
          }
            
        createTable[i] = entry;
      }

      if (polynomial == Constants.Crc32.DEFAULT_POLYNOMIAL)
        _defaultTable = createTable;

      return createTable;
    }

    private static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size) {
      var result = seed;

      for (var i = start; i < start + size; i++) {
        result = (result >> 8) ^ table[buffer[i] ^ result & 0xff];
      }
        
      return result;
    }

    private static byte[] UInt32ToBigEndianBytes(uint uint32) {
      var result = BitConverter.GetBytes(uint32);

      if (BitConverter.IsLittleEndian)
        Array.Reverse(result);

      return result;
    }
  }
}