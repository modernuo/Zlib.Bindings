namespace System.IO.Compression;

public interface ICompressor
{
    string Version { get; }
    ZlibError Compress(in int dest, ref ulong destLength, in int source, ulong sourceLength);
    ZlibError Compress(in int dest, ref ulong destLength, in int source, ulong sourceLength, ZlibQuality quality);
    ZlibError Decompress(in int dest, ref ulong destLength, in int source, ulong sourceLength);
    ulong CompressBound(ulong sourceLength);
}
