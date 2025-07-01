using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionStatus { Pending, InProgress, Completed, Cancelled }


