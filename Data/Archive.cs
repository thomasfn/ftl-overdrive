using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FTLOverdrive.Data
{
    public class ArchiveFile
    {
        public string Filename { get; set; }
        public uint Offset { get; set; }
        public uint Size { get; set; }

        public override string ToString()
        {
            return Filename + " (" + Size.ToString() + ")";
        }
    }

    public class Archive
    {

        private Dictionary<string, ArchiveFile> dctFiles;

        private string filename;

        public Archive(string filename)
        {
            // Initialise
            this.filename = filename;
            dctFiles = new Dictionary<string, ArchiveFile>();
            if (!File.Exists(filename)) return;

            // Open the file to read
            using (var strm = File.OpenRead(filename))
            {
                var rdr = new BinaryReader(strm);

                // Read the header
                var filecnt = rdr.ReadUInt32();
                var offsets = new List<uint>();

                // Read the offsets
                for (int i = 0; i < filecnt; i++)
                {
                    uint offset = rdr.ReadUInt32();
                    if (offset == 0) break;
                    offsets.Add(offset);
                }

                // Read the file entries
                for (int i = 0; i < filecnt; i++)
                {
                    strm.Seek(offsets[i], SeekOrigin.Begin);

                    var f = new ArchiveFile();
                    f.Size = rdr.ReadUInt32();
                    int pathsize = rdr.ReadInt32();
                    f.Filename = Encoding.ASCII.GetString(rdr.ReadBytes(pathsize));
                    f.Offset = (uint)strm.Position;
                    dctFiles.Add(f.Filename, f);
                }
                rdr.Close();
            }
        }

        public Stream this[string filename]
        {
            get
            {
                // Locate the file
                if (!dctFiles.ContainsKey(filename)) return null;
                var file = dctFiles[filename];

                // Copy into temporary buffer
                var strm = File.OpenRead(this.filename);
                strm.Seek(file.Offset, SeekOrigin.Begin);
                var data = new byte[file.Size];
                strm.Read(data, 0, data.Length);
                strm.Close();

                // Return
                return new MemoryStream(data);
            }
        }

    }
}
