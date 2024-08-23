using System.Text.RegularExpressions;

namespace FundingSouq.Assessment.Api.Infrastructure;

public class KebabParameterTransformer: IOutboundParameterTransformer
{
    public string TransformOutbound(object value) => value is not null 
        ? Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower() // to kebab 
        : null; 
}