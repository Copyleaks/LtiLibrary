using LtiLibrary.NetCore.Lis.v2;

namespace LtiLibrary.NetCore.Lti.v1
{
	internal interface IAssignmentLinkItem : IContentItem
	{
		LineItem LineItem { get; set; }
	}
}