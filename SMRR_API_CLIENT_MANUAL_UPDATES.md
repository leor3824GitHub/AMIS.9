# SMRR API Client Manual Updates Required

## Summary
The SMRR (Supplies and Materials Receiving Report) UI has been created following the PPERR pattern. The API client needs manual updates to add implementation methods and DTOs.

## Files Created/Updated

### Backend (API)
1. **List Endpoint Handler** - `api/modules/Inventories/Inventories.Application/SuppliesAndMaterialsReceiving/List/v1/ListSuppliesAndMaterialsReceivingReportsHandler.cs`
2. **List Command/Response** - `api/modules/Inventories/Inventories.Application/SuppliesAndMaterialsReceiving/List/v1/ListSuppliesAndMaterialsReceivingReportsCommand.cs`
3. **List Endpoint** - `api/modules/Inventories/Inventories.Infrastructure/Endpoints/v1/SuppliesAndMaterialsReceiving/ListSuppliesAndMaterialsReceivingReportsEndpoint.cs`
4. **Module Registration** - Updated `InventoriesModule.cs` to include list endpoint

### Frontend (Blazor)
1. **SMRR List Page** - `apps/blazor/client/Pages/Inventories/Reports/SuppliesAndMaterialsReceivingReportList.razor`
2. **SMRR Details Page** - `apps/blazor/client/Pages/Inventories/Reports/SuppliesAndMaterialsReceivingReport.razor`
3. **Navigation Menu** - Updated `apps/blazor/client/Layout/NavMenu.razor` to add SMRR link
4. **API Client Interface** - Added method signatures in `apps/blazor/infrastructure/Api/ApiClient.cs` (IApiClient interface)

## Required Manual Steps for API Client

### Location 1: Implementation Methods (after line ~15750 in ApiClient.cs)
Add these implementation methods right after the `PostPpeReceivingReportEndpointAsync` method:

```csharp
        /// <summary>
        /// List Supplies and Materials Receiving Reports
        /// </summary>
        public virtual async System.Threading.Tasks.Task<ListSuppliesAndMaterialsReceivingReportsResponse> ListSuppliesAndMaterialsReceivingReportsEndpointAsync(string version, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (version == null)
                throw new System.ArgumentNullException("version");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    urlBuilder_.Append("api/v");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/inventories/supplies-materials-receiving/list");

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ListSuppliesAndMaterialsReceivingReportsResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await ReadAsStringAsync(response_.Content, cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <summary>
        /// Create Supplies and Materials Receiving Report
        /// </summary>
        public virtual async System.Threading.Tasks.Task<CreateSuppliesAndMaterialsReceivingReportResponse> CreateSuppliesAndMaterialsReceivingReportEndpointAsync(string version, CreateSuppliesAndMaterialsReceivingReportCommand body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (version == null)
                throw new System.ArgumentNullException("version");
            if (body == null)
                throw new System.ArgumentNullException("body");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var json_ = System.Text.Json.JsonSerializer.Serialize(body, JsonSerializerSettings);
                    var content_ = new System.Net.Http.StringContent(json_);
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    urlBuilder_.Append("api/v");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/inventories/supplies-materials-receiving");

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 201)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<CreateSuppliesAndMaterialsReceivingReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await ReadAsStringAsync(response_.Content, cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <summary>
        /// Get Supplies and Materials Receiving Report
        /// </summary>
        public virtual async System.Threading.Tasks.Task<GetSuppliesAndMaterialsReceivingReportByIdResponse> GetSuppliesAndMaterialsReceivingReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (version == null)
                throw new System.ArgumentNullException("version");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    urlBuilder_.Append("api/v");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/inventories/supplies-materials-receiving/");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<GetSuppliesAndMaterialsReceivingReportByIdResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await ReadAsStringAsync(response_.Content, cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <summary>
        /// Post a Supplies and Materials Receiving Report
        /// </summary>
        public virtual async System.Threading.Tasks.Task<PostSuppliesAndMaterialsReceivingReportResponse> PostSuppliesAndMaterialsReceivingReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (version == null)
                throw new System.ArgumentNullException("version");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    urlBuilder_.Append("api/v");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/inventories/supplies-materials-receiving/");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/post");

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<PostSuppliesAndMaterialsReceivingReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await ReadAsStringAsync(response_.Content, cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <summary>
        /// Cancel a Supplies and Materials Receiving Report
        /// </summary>
        public virtual async System.Threading.Tasks.Task<CancelSuppliesAndMaterialsReceivingReportResponse> CancelSuppliesAndMaterialsReceivingReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (version == null)
                throw new System.ArgumentNullException("version");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var urlBuilder_ = new System.Text.StringBuilder();
                    urlBuilder_.Append("api/v");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(version, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/inventories/supplies-materials-receiving/");
                    urlBuilder_.Append(System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append("/cancel");

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<CancelSuppliesAndMaterialsReceivingReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await ReadAsStringAsync(response_.Content, cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }
```

### Location 2: DTO Classes (around line ~24900-27000 in ApiClient.cs)
Add these DTO classes alongside the PPE Receiving DTOs:

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ListSuppliesAndMaterialsReceivingReportsResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("reports")]
        public System.Collections.Generic.ICollection<SuppliesAndMaterialsReceivingReportDto> Reports { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SuppliesAndMaterialsReceivingReportDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smrrNumber")]
        public string SmrrNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("sourceName")]
        public string SourceName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("transactionType")]
        public string TransactionType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("receivingDate")]
        public System.DateTime ReceivingDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("lineItemsCount")]
        public int LineItemsCount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public int Status { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("createdAt")]
        public System.DateTimeOffset CreatedAt { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreateSuppliesAndMaterialsReceivingReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid? Id { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class GetSuppliesAndMaterialsReceivingReportByIdResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smrrNumber")]
        public string SmrrNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("transactionType")]
        public string TransactionType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("notes")]
        public string Notes { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PostSuppliesAndMaterialsReceivingReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("success")]
        public bool Success { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CancelSuppliesAndMaterialsReceivingReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("success")]
        public bool Success { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }
```

## Next Steps

1. **Manually add the implementation methods** to `apps/blazor/infrastructure/Api/ApiClient.cs` after the `PostPpeReceivingReportEndpointAsync` method
2. **Manually add the DTO classes** to `apps/blazor/infrastructure/Api/ApiClient.cs` around the PPE Receiving DTOs section
3. **Rebuild the API** to ensure the new list endpoint is working
4. **Test the SMRR UI** by navigating to `/inventories/reports/smrr-list`

## API Endpoints Available

- `GET /api/v1/inventories/supplies-materials-receiving/list` - List all SMRR
- `POST /api/v1/inventories/supplies-materials-receiving` - Create SMRR
- `GET /api/v1/inventories/supplies-materials-receiving/{id}` - Get SMRR by ID
- `POST /api/v1/inventories/supplies-materials-receiving/{id}/post` - Post SMRR
- `POST /api/v1/inventories/supplies-materials-receiving/{id}/cancel` - Cancel SMRR

## UI Pages Created

- **List Page**: `/inventories/reports/smrr-list` - Browse all SMRR reports
- **Details Page**: `/inventories/reports/smrr` - Create new SMRR report
- **Navigation**: Added link in left navigation menu under Reports section
