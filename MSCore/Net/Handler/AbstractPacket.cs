
using MSCore.Net.Handler.SendPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCore.Client;

namespace MSCore.Net.Handler
{
    public abstract class AbstractPacket : IPacket
    {
        public int Opcode {  get;  protected set; }
        
        public abstract byte[] toArray();
    }
}
