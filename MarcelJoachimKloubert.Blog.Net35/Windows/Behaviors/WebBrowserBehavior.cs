// s. http://blog.marcel-kloubert.de


using System;
using System.Windows;
using System.Windows.Controls;

namespace MarcelJoachimKloubert.Blog.Windows.Behaviors
{
    /// <summary>
    /// Attached Properties für <see cref="WebBrowser" /> Controls.
    /// </summary>
    public static class WebBrowserBehavior
    {
        #region Fields (1)

        /// <summary>
        /// HTML Attached Property
        /// </summary>
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(WebBrowserBehavior),
            new FrameworkPropertyMetadata(OnHtmlChanged));

        #endregion Fields

        #region Methods (3)

        // Public Methods (2) 

        /// <summary>
        /// Gibt den derzeit gebundenen HTM-Quelltext zurück.
        /// </summary>
        /// <param name="browser">Der zugrundeliegende Browser.</param>
        /// <returns>Der aktuelle HTML-Quelltext.</returns>
        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser browser)
        {
            return (string)browser.GetValue(HtmlProperty);
        }

        /// <summary>
        /// Legt den derzeit gebundenen HTM-Quelltext fest.
        /// </summary>
        /// <param name="browser">Der zugrundeliegende Browser.</param>
        /// <param name="html">Der neue Quelltext.</param>
        public static void SetHtml(WebBrowser browser, string html)
        {
            browser.SetValue(HtmlProperty, html);
        }
        // Private Methods (1) 

        private static void OnHtmlChanged(DependencyObject obj,
                                          DependencyPropertyChangedEventArgs e)
        {
            var browser = (WebBrowser)obj;

            var html = e.NewValue
                        .AsString(handleDBNullAsNull: true) ?? string.Empty;

            if (html.IsNullOrWhiteSpace())
            {
                browser.Navigate(new Uri("about:blank"));
            }
            else
            {
                browser.NavigateToString(html);
            }
        }

        #endregion Methods
    }
}
