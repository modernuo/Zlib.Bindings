using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO.Compression
{
  public static class Zlib
  {
    public static readonly ICompressor Compressor;

    internal static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    internal static readonly bool IsDarwin = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    internal static readonly bool IsFreeBSD = RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
    internal static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || IsFreeBSD;
    internal static readonly bool IsUnix = IsDarwin || IsFreeBSD || IsLinux;
    
    static Zlib()
    {
      if (IsUnix)
        Compressor = new UnixZlibCompressor();
      else
        Compressor = new WindowsZlibCompressor();
    }

    public static int MaxPackSize(int sourceLength) => (int)Compressor.CompressBound((ulong)sourceLength);

    public static unsafe ZlibError Pack(Span<byte> dest, ref int destLength, ReadOnlySpan<byte> source, int sourceLength)
    {
      var destLengthLong = (ulong)destLength;
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        var e = Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
          (ulong)sourceLength);
        destLength = (int)destLengthLong;
        return e;
      }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref int destLength, ReadOnlySpan<byte> source, ZlibQuality quality)
    {
      var destLengthLong = (ulong)destLength;
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        var e = Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
          (ulong)source.Length, quality);
        destLength = (int)destLengthLong;
        return e;
      }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref int destLength, ReadOnlySpan<byte> source, int sourceLength,
      ZlibQuality quality)
    {
      var destLengthLong = (ulong)destLength;
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        var e = Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
          (ulong)sourceLength, quality);
        destLength = (int)destLengthLong;
        return e;
      }
    }

    public static unsafe ZlibError Unpack(Span<byte> dest, ref int destLength, ReadOnlySpan<byte> source, int sourceLength)
    {
      var destLengthLong = (ulong)destLength;
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        var e = Compressor.Decompress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
          (ulong)sourceLength);
        destLength = (int)destLengthLong;
        return e;
      }
    }

    public static ulong MaxPackSize(ulong sourceLength) => Compressor.CompressBound(sourceLength);

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source, ulong sourceLength)
    {
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        return Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength);
      }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source,
      ZlibQuality quality)
    {
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        return Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), (ulong)source.Length,
          quality);
      }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source, ulong sourceLength,
      ZlibQuality quality)
    {
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        return Compressor.Compress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength, quality);
      }
    }

    public static unsafe ZlibError Unpack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source,
      ulong sourceLength)
    {
      fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
      {
        return Compressor.Decompress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength);
      }
    }
  }
}
