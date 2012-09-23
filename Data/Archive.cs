using System;
using System.Collections.Generic;
using System.IO;

namespace FTLOverdrive.Data
{
    public class Archive
    {
        public class File
        {
            public string Filename { get; set; }
            public uint Offset { get; set; }
            public uint Size { get; set; }

            public override string ToString()
            {
                return Filename + " (" + Offset.ToString() + ":" + Size.ToString() + ")";
            }
        }

        private List<File> lstFiles;

        public List<File> Files { get { return lstFiles; } }

        private string filename;

        public Archive(string filename)
        {
            this.filename = filename;
            lstFiles = new List<File>();
            if (!System.IO.File.Exists(filename)) return;
            using (var strm = System.IO.File.OpenRead(filename))
            {
                strm.Seek(0x1dfc, SeekOrigin.Begin); // this offset better not change
                var rdr = new BinaryReader(strm);
                while (strm.Position < strm.Length)
                {
                    var f = new File();
                    f.Size = rdr.ReadUInt32();
                    uint tmp = rdr.ReadUInt32();
                    f.Filename = "";
                    for (uint i = 0; i < tmp; i++)
                        f.Filename += rdr.ReadChar();
                    f.Offset = (uint)strm.Position;
                    lstFiles.Add(f);
                    strm.Seek(f.Size, SeekOrigin.Current);
                }
                rdr.Close();
            }
        }

        public File this[string filename]
        {
            get
            {
                foreach (var file in lstFiles)
                    if (file.Filename == filename)
                        return file;
                return null;
            }
        }

        public Stream this[File file]
        {
            get
            {
                var strm = System.IO.File.OpenRead(filename);
                strm.Seek(file.Offset, SeekOrigin.Begin);
                var data = new byte[file.Size];
                strm.Read(data, 0, data.Length);
                strm.Close();
                return new MemoryStream(data);
            }
        }

    }
}
