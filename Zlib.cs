using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO.Compression;

public static class Zlib
{
    public const string WindowsLibzAssemblyName = "libz.dll";
    public const string WindowsZlibAssemblyName = "zlib.dll";
    public const string WindowsZlib1AssemblyName = "zlib1.dll";
    public const string UnixAssemblyName = "libz";

    static Zlib() => NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

    public static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        IntPtr handle;

        // Windows, do some special logic
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (NativeLibrary.TryLoad(WindowsLibzAssemblyName, assembly, searchPath, out handle))
            {
                return handle;
            }

            if (NativeLibrary.TryLoad(WindowsZlibAssemblyName, assembly, searchPath, out handle))
            {
                return handle;
            }

            if (NativeLibrary.TryLoad(WindowsZlib1AssemblyName, assembly, searchPath, out handle))
            {
                return handle;
            }

            throw new BadImageFormatException("Could not load the zlib native library.");
        }

        if (NativeLibrary.TryLoad(UnixAssemblyName, assembly, searchPath, out handle))
        {
            return handle;
        }

        throw new BadImageFormatException("Could not load the zlib native library.");
    }

    public static string Version => zlibVersion();

    public static int MaxPackSize(int sourceLength) => (int)compressBound((ulong)sourceLength);

    public static ulong MaxPackSize(ulong sourceLength) => compressBound(sourceLength);

    public static unsafe ZlibError Pack(Span<byte> dest, ref int destLength, ReadOnlySpan<byte> source, int sourceLength)
    {
        var destLengthLong = (ulong)destLength;
        fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
        {
            var e = compress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
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
            var e = compress2(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
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
            var e = compress2(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
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
            var e = uncompress(Unsafe.AsRef<int>(dPtr), ref destLengthLong, Unsafe.AsRef<int>(sPtr),
                (ulong)sourceLength);
            destLength = (int)destLengthLong;
            return e;
        }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source, ulong sourceLength)
    {
        fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
        {
            return compress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength);
        }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source,
        ZlibQuality quality)
    {
        fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
        {
            return compress2(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), (ulong)source.Length,
                quality);
        }
    }

    public static unsafe ZlibError Pack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source, ulong sourceLength,
        ZlibQuality quality)
    {
        fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
        {
            return compress2(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength, quality);
        }
    }

    public static unsafe ZlibError Unpack(Span<byte> dest, ref ulong destLength, ReadOnlySpan<byte> source,
        ulong sourceLength)
    {
        fixed (byte* dPtr = &MemoryMarshal.GetReference(dest), sPtr = &MemoryMarshal.GetReference(source))
        {
            return uncompress(Unsafe.AsRef<int>(dPtr), ref destLength, Unsafe.AsRef<int>(sPtr), sourceLength);
        }
    }

    [DllImport("libz", CharSet = CharSet.Unicode)]
    internal static extern string zlibVersion();

    [DllImport("libz")]
    internal static extern ZlibError compress(in int dest, ref ulong destLength, in int source, ulong sourceLength);

    [DllImport("libz")]
    internal static extern ZlibError compress2(in int dest, ref ulong destLength, in int source, ulong sourceLength,
        ZlibQuality quality);

    [DllImport("libz")]
    internal static extern ZlibError uncompress(in int dest, ref ulong destLength, in int source, ulong sourceLength);

    [DllImport("libz")]
    internal static extern ulong compressBound(ulong sourceLen);

}
