using System;
using TsuroTheSecond;
namespace TsuroTheSecond
{
    public class MMD
    {
        // Magic Middle Dude
        // The relayer, the translator, the caller.
        // The dude.
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
