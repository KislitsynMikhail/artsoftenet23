namespace Api.Controllers.Dto.Response;

/// <summary>
/// 
/// </summary>
public record TestGetResponse
{
    /// <summary>
    /// 
    /// </summary>
    public bool IsSuccessed { get; init; } 
    
    /// <summary>
    /// 
    /// </summary>
    public int RandomNumber { get; init; }
}