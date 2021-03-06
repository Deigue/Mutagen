using Loqui;
using Mutagen.Bethesda;

namespace Loqui
{
    public class ProtocolDefinition_Bethesda : IProtocolRegistration
    {
        public readonly static ProtocolKey ProtocolKey = new ProtocolKey("Bethesda");
        void IProtocolRegistration.Register() => Register();
        public static void Register()
        {
            LoquiRegistration.Register(Mutagen.Bethesda.Internals.MajorRecord_Registration.Instance);
            LoquiRegistration.Register(Mutagen.Bethesda.Internals.MasterReference_Registration.Instance);
        }
    }
}
