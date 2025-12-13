using System.Text.Json;
using System.Text.Json.Serialization;

namespace QueueService.DTO;

public enum WindowsStatus { free, busy };

public abstract class BaseDTO { }

public partial class TalonDTO : BaseDTO
{
    public string? TalonNumber { get; set; }
    public string? ServiceCode { get; set; }
    public TimeSpan? PendingTime { get; set; }
}

public partial class WindowDTO : BaseDTO
{
    public string? WindowNumber { get; set; }
    public string? Status { get; set; } // Просто string

    [JsonIgnore]
    public WindowsStatus? StatusEnum =>
        Status?.ToLower() switch
        {
            "free" => WindowsStatus.free,
            "busy" => WindowsStatus.busy,
            _ => null
        };
}
public partial class QueueDTO : BaseDTO
{
    public required string WindowNumber { get; set; }
    public required string TalonNumber { get; set; }
}

public class DTOJsonConverter : JsonConverter<BaseDTO>
{
    public override BaseDTO? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        // Создаем копию reader для предварительного анализа
        var readerClone = reader;
        using var jsonDoc = JsonDocument.ParseValue(ref readerClone);
        var root = jsonDoc.RootElement;

        // Определяем тип по наличию полей (учитываем case-insensitive)
        var properties = root.EnumerateObject().Select(p => p.Name).ToList();

        bool hasTalonNumber = properties.Any(p =>
            p.Equals("TalonNumber", StringComparison.OrdinalIgnoreCase));
        bool hasServiceCode = properties.Any(p =>
            p.Equals("ServiceCode", StringComparison.OrdinalIgnoreCase));
        bool hasWindowNumber = properties.Any(p =>
            p.Equals("WindowNumber", StringComparison.OrdinalIgnoreCase));
        bool hasStatus = properties.Any(p =>
            p.Equals("Status", StringComparison.OrdinalIgnoreCase));

        // Десериализуем соответствующий тип
        if (hasTalonNumber && hasServiceCode)
        {
            return JsonSerializer.Deserialize<TalonDTO>(ref reader, options);
        }
        else if (hasWindowNumber && hasStatus)
        {
            return JsonSerializer.Deserialize<WindowDTO>(ref reader, options);
        }
        else if (hasWindowNumber && hasTalonNumber)
        {
            return JsonSerializer.Deserialize<QueueDTO>(ref reader, options);
        }

        throw new JsonException("Unknown DTO type");
    }

    public override void Write(
        Utf8JsonWriter writer,
        BaseDTO value,
        JsonSerializerOptions options)
    {
        // Сериализуем в зависимости от типа
        switch (value)
        {
            case TalonDTO talon:
                JsonSerializer.Serialize(writer, talon, options);
                break;
            case WindowDTO window:
                JsonSerializer.Serialize(writer, window, options);
                break;
            case QueueDTO queue:
                JsonSerializer.Serialize(writer, queue, options);
                break;
            default:
                throw new JsonException($"Unknown DTO type: {value.GetType()}");
        }
    }
}