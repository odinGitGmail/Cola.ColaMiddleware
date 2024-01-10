namespace Cola.ColaMiddleware.ColaVersioning;

public class ColaVersioningOption
{
    /// <summary>
    /// 默认 true  在Api请求的响应头部，会追加当前Api支持的版本     api-supported-versions: 1.0 
    /// </summary>
    public bool ReportApiVersions { get; set; } = true;

    /// <summary>
    /// 默认 true  标记当客户端没有指定版本号的时候，是否使用默认版本号 
    /// </summary>
    public bool AssumeDefaultVersionWhenUnspecified { get; set; } = true;

    /// <summary>
    /// The major version - default 1
    /// </summary>
    public int MajorVersion { get; set; } = 1;
    
    /// <summary>
    /// The optional minor version - default 0
    /// </summary>
    public int MinorVersion { get; set; } = 0;

    /// <summary>
    /// HttpHeaderName 请求头(HTTP Header)中使用版本控制  default: x-api-version
    /// </summary>
    public string HttpHeaderName { get; set; } = "x-api-version";

}