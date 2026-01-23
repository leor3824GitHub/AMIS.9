# SMIR API Client Manual Updates

## Overview
This document provides instructions for manually adding Supplies and Materials Issuance Report (SMIR) implementation methods and DTO classes to the `ApiClient.cs` file.

## Files Status
- ✅ Backend endpoints created and registered
- ✅ Frontend UI pages created  
- ✅ API client interface signatures added
- ⏳ API client implementation methods needed
- ⏳ API client DTO classes needed

## Implementation Methods Location
**File:** `e:\AMIS.9\apps\blazor\infrastructure\Api\ApiClient.cs`
**Location:** After line ~20800 (after `ListSuppliesAndMaterialsReceivingReportsEndpointAsync` implementation)

### Method 1: ListSuppliesAndMaterialsIssuanceReportsEndpointAsync

```csharp
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public virtual async System.Threading.Tasks.Task<ListSuppliesAndMaterialsIssuanceReportsResponse> ListSuppliesAndMaterialsIssuanceReportsEndpointAsync(string version, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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
                    urlBuilder_.Append("supplies-materials-issuance/list?");
                    urlBuilder_.Append(System.Uri.EscapeDataString("api-version")).Append("=").Append(System.Uri.EscapeDataString(System.Convert.ToString(version, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                    urlBuilder_.Length--;

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode);
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ListSuppliesAndMaterialsIssuanceReportsResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseText_ = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseText_, headers_, null);
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

### Method 2: CreateSuppliesAndMaterialsIssuanceReportEndpointAsync

```csharp
        /// <param name="version">The requested API version</param>
        /// <param name="body">The SMIR command</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public virtual async System.Threading.Tasks.Task<CreateSuppliesAndMaterialsIssuanceReportResponse> CreateSuppliesAndMaterialsIssuanceReportEndpointAsync(string version, CreateSuppliesAndMaterialsIssuanceReportCommand body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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
                    urlBuilder_.Append("supplies-materials-issuance?");
                    urlBuilder_.Append(System.Uri.EscapeDataString("api-version")).Append("=").Append(System.Uri.EscapeDataString(System.Convert.ToString(version, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                    urlBuilder_.Length--;

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode);
                        if (status_ == 201)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<CreateSuppliesAndMaterialsIssuanceReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseText_ = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseText_, headers_, null);
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

### Method 3: GetSuppliesAndMaterialsIssuanceReportEndpointAsync

```csharp
        /// <param name="version">The requested API version</param>
        /// <param name="id">The SMIR ID</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public virtual async System.Threading.Tasks.Task<GetSuppliesAndMaterialsIssuanceReportResponse> GetSuppliesAndMaterialsIssuanceReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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
                    urlBuilder_.Append("supplies-materials-issuance/{id}?");
                    urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(System.Convert.ToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append(System.Uri.EscapeDataString("api-version")).Append("=").Append(System.Uri.EscapeDataString(System.Convert.ToString(version, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                    urlBuilder_.Length--;

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode);
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<GetSuppliesAndMaterialsIssuanceReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseText_ = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseText_, headers_, null);
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

### Method 4: PostSuppliesAndMaterialsIssuanceReportEndpointAsync

```csharp
        /// <param name="version">The requested API version</param>
        /// <param name="id">The SMIR ID</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public virtual async System.Threading.Tasks.Task<PostSuppliesAndMaterialsIssuanceReportResponse> PostSuppliesAndMaterialsIssuanceReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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
                    urlBuilder_.Append("supplies-materials-issuance/{id}/post?");
                    urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(System.Convert.ToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append(System.Uri.EscapeDataString("api-version")).Append("=").Append(System.Uri.EscapeDataString(System.Convert.ToString(version, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                    urlBuilder_.Length--;

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode);
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<PostSuppliesAndMaterialsIssuanceReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseText_ = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseText_, headers_, null);
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

### Method 5: CancelSuppliesAndMaterialsIssuanceReportEndpointAsync

```csharp
        /// <param name="version">The requested API version</param>
        /// <param name="id">The SMIR ID</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public virtual async System.Threading.Tasks.Task<CancelSuppliesAndMaterialsIssuanceReportResponse> CancelSuppliesAndMaterialsIssuanceReportEndpointAsync(string version, System.Guid id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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
                    urlBuilder_.Append("supplies-materials-issuance/{id}/cancel?");
                    urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(System.Convert.ToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                    urlBuilder_.Append(System.Uri.EscapeDataString("api-version")).Append("=").Append(System.Uri.EscapeDataString(System.Convert.ToString(version, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                    urlBuilder_.Length--;

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode);
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<CancelSuppliesAndMaterialsIssuanceReportResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new ApiException<ProblemDetails>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseText_ = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseText_, headers_, null);
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

## DTO Classes Location
**File:** `e:\AMIS.9\apps\blazor\infrastructure\Api\ApiClient.cs`
**Location:** Around line ~26000 (in the DTOs section with other response classes)

Add these classes in the appropriate alphabetical order:

### DTO 1: ListSuppliesAndMaterialsIssuanceReportsResponse

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class ListSuppliesAndMaterialsIssuanceReportsResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("reports")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public System.Collections.Generic.ICollection<SuppliesAndMaterialsIssuanceReportDto>? Reports { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 2: SuppliesAndMaterialsIssuanceReportDto

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class SuppliesAndMaterialsIssuanceReportDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedToName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedToName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("transactionType")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? TransactionType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuanceDate")]
        public System.DateTime IssuanceDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("lineItemsCount")]
        public int LineItemsCount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public int Status { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("createdAt")]
        public System.DateTimeOffset CreatedAt { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 3: CreateSuppliesAndMaterialsIssuanceReportCommand

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class CreateSuppliesAndMaterialsIssuanceReportCommand
    {
        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedToName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedToName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedToAddress")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedToAddress { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuanceDate")]
        public System.DateTime IssuanceDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("transactionType")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? TransactionType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("lineItems")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public System.Collections.Generic.ICollection<IssuanceLineItemRequest>? LineItems { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedByName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedByName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedByDate")]
        public System.DateTime IssuedByDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("receivedByName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? ReceivedByName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("receivedByDate")]
        public System.DateTime ReceivedByDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("remarks")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Remarks { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 4: CreateSuppliesAndMaterialsIssuanceReportResponse

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class CreateSuppliesAndMaterialsIssuanceReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 5: GetSuppliesAndMaterialsIssuanceReportResponse

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class GetSuppliesAndMaterialsIssuanceReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedToName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedToName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuedToAddress")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? IssuedToAddress { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuanceDate")]
        public System.DateTime IssuanceDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("transactionType")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? TransactionType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public int Status { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 6: PostSuppliesAndMaterialsIssuanceReportResponse

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class PostSuppliesAndMaterialsIssuanceReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("message")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 7: CancelSuppliesAndMaterialsIssuanceReportResponse

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class CancelSuppliesAndMaterialsIssuanceReportResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("smirNumber")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? SmirNumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("message")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

### DTO 8: IssuanceLineItemRequest

```csharp
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0")]
    public partial class IssuanceLineItemRequest
    {
        [System.Text.Json.Serialization.JsonPropertyName("itemName")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? ItemName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("issuanceDate")]
        public System.DateTime IssuanceDate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("unit")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Unit { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("unitCost")]
        public decimal UnitCost { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("location")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Location { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("remarks")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? Remarks { get; set; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object?> AdditionalProperties { get; set; } = new System.Collections.Generic.Dictionary<string, object?>();
    }
```

## Next Steps
1. Manually add all 5 implementation methods to ApiClient.cs
2. Manually add all 8 DTO classes to ApiClient.cs
3. Rebuild solution: `dotnet build e:\AMIS.9\AMIS.9.sln --configuration Debug`
4. Test SMIR functionality by navigating to `/inventories/reports/smir-list`
5. Verify API calls work correctly from the UI pages

## Available API Endpoints
The following endpoints are now available for SMIR:
- **List**: `GET /api/v1/supplies-materials-issuance/list` - List all SMIR reports
- **Create**: `POST /api/v1/supplies-materials-issuance` - Create new SMIR report
- **Get**: `GET /api/v1/supplies-materials-issuance/{id}` - Get specific SMIR report
- **Post**: `POST /api/v1/supplies-materials-issuance/{id}/post` - Post draft SMIR (apply inventory changes)
- **Cancel**: `POST /api/v1/supplies-materials-issuance/{id}/cancel` - Cancel posted SMIR (reverse inventory changes)

## UI Pages Created
Two complete Blazor pages have been created for SMIR:
- **List Page**: `apps/blazor/client/Pages/Inventories/Reports/SuppliesAndMaterialsIssuanceReportList.razor`
  - Route: `/inventories/reports/smir-list`
  - Lists all SMIR reports with status, amounts, and action buttons
  
- **Create/Edit Page**: `apps/blazor/client/Pages/Inventories/Reports/SuppliesAndMaterialsIssuanceReport.razor`
  - Routes: `/inventories/reports/smir` (create) and `/inventories/reports/smir/{ReportId}` (edit)
  - Form for creating new SMIR reports with line items
  - Supports print and readonly views via query parameters

## Summary
SMIR implementation follows the same pattern as SMRR for consistency. All backend infrastructure and frontend UI is in place. Only manual API client updates are needed to complete the integration.
