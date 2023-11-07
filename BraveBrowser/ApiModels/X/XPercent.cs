using static BraveBrowser.Helpers.Constants;

namespace BraveBrowser.ApiModels.X
{
	public class XPercent
	{
        public XActionType Name { get; set; }

        public int Percent { get; set; }

        public XPercent(XActionType name, int percent)
        {
            Name = name;
            Percent = percent;
        }
    }
}