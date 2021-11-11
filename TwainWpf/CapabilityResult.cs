using TwainWpf.TwainNative;

namespace TwainWpf
{
    public abstract class CapabilityResult
    {
        public ConditionCode ConditionCode { get; set; }

        public TwainResult ErrorCode { get; set; }
    }

    public class BasicCapabilityResult : CapabilityResult
    {
        public int RawBasicValue { get; set; }

        public bool BoolValue => RawBasicValue == 1;

        public short Int16Value => (short)RawBasicValue;

        public int Int32Value => RawBasicValue;
    }
}