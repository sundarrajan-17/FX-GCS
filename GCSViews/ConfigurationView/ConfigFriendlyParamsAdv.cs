using XagSurveillanceGCS.Utilities;

namespace XagSurveillanceGCS.GCSViews.ConfigurationView
{
    public partial class ConfigFriendlyParamsAdv : ConfigFriendlyParams
    {
        public ConfigFriendlyParamsAdv()
        {
            ParameterMode = ParameterMode = ParameterMetaDataConstants.Advanced;
        }
    }
}