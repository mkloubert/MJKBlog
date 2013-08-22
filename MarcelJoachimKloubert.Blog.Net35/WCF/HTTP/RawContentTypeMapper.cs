// s. http://blog.marcel-kloubert.de


using System.ServiceModel.Channels;

namespace MarcelJoachimKloubert.Blog.Net.HTTP
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RawContentTypeMapper : WebContentTypeMapper
    {
        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="WebContentTypeMapper.GetMessageFormatForContentType(string)" />
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }

        #endregion Methods
    }
}
