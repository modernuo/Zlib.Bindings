using System.Runtime.InteropServices;

namespace System.IO.Compression
{
  public class WindowsZlibCompressor : ICompressor
  {
    public string Version => SafeNativeMethods.zlibVersion();

    public ZlibError Compress(in int dest, ref ulong destLength, in int source, ulong sourceLength) =>
      SafeNativeMethods.compress(dest, ref destLength, source, sourceLength);

    public ZlibError Compress(in int dest, ref ulong destLength, in int source, ulong sourceLength,
      ZlibQuality quality) => SafeNativeMethods.compress2(dest, ref destLength, source, sourceLength, quality);

    public ZlibError Decompress(in int dest, ref ulong destLength, in int source, ulong sourceLength) =>
      SafeNativeMethods.uncompress(dest, ref destLength, source, sourceLength);

    public ulong CompressBound(ulong sourceLength) => SafeNativeMethods.compressBound(sourceLength);

    private static class SafeNativeMethods
    {
      [DllImport("zlib", CharSet = CharSet.Unicode)]
      internal static extern string zlibVersion();

      [DllImport("zlib")]
      internal static extern ZlibError compress(in int dest, ref ulong destLength, in int source, ulong sourceLength);

      [DllImport("zlib")]
      internal static extern ZlibError compress2(in int dest, ref ulong destLength, in int source, ulong sourceLength,
        ZlibQuality quality);

      [DllImport("zlib")]
      internal static extern ZlibError uncompress(in int dest, ref ulong destLength, in int source, ulong sourceLength);

      [DllImport("zlib")]
      internal static extern ulong compressBound(ulong sourceLen);
    }
  }
}