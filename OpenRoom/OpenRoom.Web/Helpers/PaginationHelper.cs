namespace OpenRoom.Web.Helpers
{
    public static class PaginationHelper
    {
        public static string CreatePageLink(this IUrlHelper urlHelper, int page, string actionName, string controllerName, object routeValues)
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues)
        {
            { "page", page }
        };

            return urlHelper.Action(actionName, controllerName, routeValueDictionary);
        }
    }
}
