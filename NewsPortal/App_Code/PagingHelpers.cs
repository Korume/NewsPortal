using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.App_Code
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, int lastPageIndex, int currentPageIndex, Func<int, string> pageUrl)
        {
            const int boundaryPagesQuantity = 5;
            const int centerPagesQuantity = 2;
            const int farPagesQuantity = 2;

            var result = new StringBuilder();
            if (currentPageIndex == 0)

            {
                result.Append(CreateSpan("Previous"));
            }
            else
            {
                result.Append(CreateLink("Previous", currentPageIndex - 1, pageUrl));
            }
            bool isPagingHaveGap = currentPageIndex + centerPagesQuantity * 2 < lastPageIndex || 
                currentPageIndex - centerPagesQuantity * 2 >= 0;
            if (isPagingHaveGap)
            {
                bool isCurrentPageIndexInLeft = currentPageIndex <= boundaryPagesQuantity;
                bool isCurrentPageIndexInRight = currentPageIndex >= lastPageIndex - boundaryPagesQuantity;

                if (isCurrentPageIndexInLeft)
                {
                    result.Append(CreateLinksInInterval(0, currentPageIndex + centerPagesQuantity, currentPageIndex, pageUrl));
                    result.Append(CreateSpan("..."));
                    result.Append(CreateLinksInInterval(lastPageIndex - farPagesQuantity + 1, lastPageIndex, currentPageIndex, pageUrl));
                }
                else if (isCurrentPageIndexInRight)
                {
                    result.Append(CreateLinksInInterval(0, farPagesQuantity - 1, currentPageIndex, pageUrl));
                    result.Append(CreateSpan("..."));
                    result.Append(CreateLinksInInterval(currentPageIndex - centerPagesQuantity, lastPageIndex, currentPageIndex, pageUrl));
                }
                else
                {
                    result.Append(CreateLinksInInterval(0, farPagesQuantity - 1, currentPageIndex, pageUrl));
                    result.Append(CreateSpan("..."));
                    result.Append(CreateLinksInInterval(currentPageIndex - centerPagesQuantity, currentPageIndex + centerPagesQuantity, currentPageIndex, pageUrl));
                    result.Append(CreateSpan("..."));
                    result.Append(CreateLinksInInterval(lastPageIndex - farPagesQuantity + 1, lastPageIndex, currentPageIndex, pageUrl));
                }
            }
            else
            {
                result.Append(CreateLinksInInterval(0, lastPageIndex, currentPageIndex, pageUrl));
            }

            if (currentPageIndex == lastPageIndex)
            {
                result.Append(CreateSpan("Next"));
            }
            else
            {
                result.Append(CreateLink("Next", currentPageIndex + 1, pageUrl));
            }
            return MvcHtmlString.Create(result.ToString());
        }

        private static string CreateLinksInInterval(int from, int to, int currentPageIndex, Func<int, string> pageUrl)
        {
            var links = new StringBuilder();
            for (int i = from; i <= to; i++)
            {
                string tag = null;
                if (currentPageIndex == i)
                {
                    tag = CreateSpan((i + 1).ToString());
                }
                else
                {
                    tag = CreateLink((i + 1).ToString(), i, pageUrl);
                }
                links.Append(tag);
            }
            return links.ToString();
        }

        private static string CreateSpan(string innerHtml)
        {
            var tag = new TagBuilder("span");
            tag.InnerHtml = innerHtml;
            return tag.ToString();
        }

        private static string CreateLink(string innerHtml, int transitionPageIndex, Func<int, string> pageUrl)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", pageUrl(transitionPageIndex));
            tag.InnerHtml = innerHtml;
            return tag.ToString();
        }
    }
}
