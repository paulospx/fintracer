using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace FinTracer
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent NoSpaces(this IHtmlHelper htmlHelper, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return HtmlString.Empty;
            }

            // Remove spaces from the content
            var noSpacesContent = content.Replace(" ",string.Empty);

            // Return the processed content as HTML-encoded output
            return new HtmlString(HtmlEncoder.Default.Encode(noSpacesContent));
        }
        public static IHtmlContent DisplayWithoutSpacesFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            System.Linq.Expressions.Expression<Func<TModel, TValue>> expression)
        {
            // Get the value from the model using DisplayForModel logic
            var rawValue = htmlHelper.DisplayFor(expression).GetString();

            // Remove spaces from the value
            if (!string.IsNullOrEmpty(rawValue))
            {
                rawValue = rawValue.Replace(" ", "-");
            }

            // Return the processed content as HTML-encoded output
            return new HtmlString(HtmlEncoder.Default.Encode(rawValue));
        }

        private static string GetString(this IHtmlContent content)
        {
            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
