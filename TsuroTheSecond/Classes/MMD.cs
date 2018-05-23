using System;
using TsuroTheSecond;
namespace TsuroTheSecond
{
    public class MMD
    {
        NetworkRelay networkRelay;
        Identifier identifier;
        Wrapper wrapper;
        
        public MMD(Server server)
        {
            this.networkRelay = new NetworkRelay();
            this.identifier = new Identifier();
            this.wrapper = new Wrapper(server);
        }
    }
}
