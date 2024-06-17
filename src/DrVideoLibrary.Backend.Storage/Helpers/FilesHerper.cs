namespace DrVideoLibrary.Backend.Storage.Helpers;

internal sealed class FilesHerper
{
    /// <summary>
    /// Convertir un string en Stream
    /// </summary>
    /// <param name="str">string a convertir</param>
    /// <param name="enc">Encoding a utilizar, por defecto UTF8</param>
    /// <returns></returns>
    public static Stream ToStream(string str, Encoding enc = null)
    {
        enc = enc ?? Encoding.UTF8;
        return new MemoryStream(enc.GetBytes(str ?? ""));
    }

    /// <summary>
    /// Convertir un bytes en Stream
    /// </summary>
    /// <param name="bytes">bytes a convertir</param>
    /// <returns></returns>
    public static Stream ToStream(byte[] bytes)
    {
        MemoryStream stream = new MemoryStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Position = 0;
        return stream;
    }

    public static string GenerateRandomeFileName(string extension, string id) =>
        $"{RemoveDot(id)}-{RemoveDot(Path.GetRandomFileName())}.{RemoveDot(extension)}";

    private static string RemoveDot(string path) =>
        path?.Replace(".", "") ?? "";

}
