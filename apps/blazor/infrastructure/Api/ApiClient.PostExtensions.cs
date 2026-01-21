using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.Blazor.Infrastructure.Api
{
    public partial interface IApiClient
    {
        Task<PostPpeReceivingReportResponse> PostPpeReceivingReportEndpointAsync(string version, Guid id, CancellationToken cancellationToken = default);
    }

    public partial class ApiClient : IApiClient
    {
        public virtual async Task<PostPpeReceivingReportResponse> PostPpeReceivingReportEndpointAsync(string version, Guid id, CancellationToken cancellationToken = default)
        {
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("api/v");
            urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
            urlBuilder_.Append("/ppe-receiving/");
            urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
            urlBuilder_.Append("/post");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using var request_ = new HttpRequestMessage();
                request_.Method = HttpMethod.Post;
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                
                PrepareRequest(client_, request_, urlBuilder_);
                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                PrepareRequest(client_, request_, url_);

                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = new Dictionary<string, IEnumerable<string>>();
                    foreach (var item_ in response_.Headers)
                        headers_[item_.Key] = item_.Value;
                    if (response_.Content?.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PostPpeReceivingReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                        throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }
    }

    public partial class PostPpeReceivingReportResponse
    {
        public Guid Id { get; set; }
        public string? ReportNumber { get; set; }
        public string? Status { get; set; }
    }
}
