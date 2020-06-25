namespace System.IO.Compression
{
  public enum ZlibError
  {
    VersionError = -6,
    BufferError = -5,
    MemoryError = -4,
    DataError = -3,
    StreamError = -2,
    FileError = -1,
    Okay = 0,
    StreamEnd = 1,
    NeedDictionary = 2
  }
}