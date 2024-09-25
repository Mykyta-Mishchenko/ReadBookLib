using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ReadBookLib.Models.ViewModels;

namespace ReadBookLib.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-paging-model")]
    public class PageLinkTagHelper: TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public PagingInfo? PagePagingModel { get; set; }
        public string? PageAction { get; set; }

        public bool PageClassDefault {  get; set; }
        public string PageClass { get; set; } = "btn";
        public string PageClassNormal { get; set; } = "btn-outline-dark";
        public string PageClassSelected { get; set; } = "btn-primary";

        // This method specified only for actions which has a parameter like "booksPage"
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PagePagingModel?.TotalPages == 1) return;
            if(ViewContext!=null && PagePagingModel != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");
                
                for(int i=1;i<= PagePagingModel.TotalPages;i++) { 
                    TagBuilder tag = new TagBuilder("a");
                    // the problem is below
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new {booksPage = i});
                    if (PageClassDefault)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i== PagePagingModel.CurrentPage 
                                        ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
                output.Content.AppendHtml(result.InnerHtml);
            }
        }

    }
}
