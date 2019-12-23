namespace Dsu.PLCConvertor.Common
{
    public enum PLCVendor
    {
        LSIS,
        Omron,
        Mistsubish,
    }

    public static class PLCVenderExtension
    {
        public static bool IsMPushModel(this PLCVendor targetType) => targetType != PLCVendor.Omron;
    }
}
