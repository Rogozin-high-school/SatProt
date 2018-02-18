using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatProt
{
    class OutMsg
    {
        public byte Type;
        public byte Status;
        public List<byte[]> Payload = new List<byte[]>();

        public OutMsg(byte t, byte s)
        {
            Type = t;
            Status = s;
        }

        public void AddParameter(byte[] x)
        {
            Payload.Add(x);
        }

        public void AddInt(int x)
        {
            AddParameter(BitConverter.GetBytes(x));
        }

        public void AddFloat(float x)
        {
            AddParameter(BitConverter.GetBytes(x));
        }

        public byte[] GetBytes()
        {
            int contentLength = Payload.Sum(x => x.Length);

            byte[] body = new byte[1 + 1 + 4 + contentLength];

            byte[] size = BitConverter.GetBytes(contentLength);
            size.CopyTo(body, 0);

            body[4] = Type;
            body[5] = Status;

            int ptr = 6;
            foreach (byte[] p in Payload)
            {
                p.CopyTo(body, ptr);
                ptr += p.Length;
            }

            return body;
        }
    }

    class InMsg
    {
        private byte[] Body;
        public byte Type { get; protected set; }
        public byte Status { get; protected set; }
        private int ptr;
        
        public InMsg(byte[] b, int offset = 0)
        {
            ptr = 2 + offset;
            Body = b;
            Type = Body[0 + offset];
            Status = Body[1 + offset];
        }

        public int NextInt()
        {
            int v = BitConverter.ToInt32(Body, ptr);
            ptr += 4;
            return v;
        }

        public float NextFloat()
        {
            float v = BitConverter.ToSingle(Body, ptr);
            ptr += 4;
            return v;
        }
    }
}
