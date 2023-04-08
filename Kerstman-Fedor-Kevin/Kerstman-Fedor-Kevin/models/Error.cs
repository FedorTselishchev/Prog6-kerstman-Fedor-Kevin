using Microsoft.AspNetCore.Mvc;

namespace Kerstman_Fedor_Kevin.models
{
    public class Error
    {
        public string? Id { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(Id);
    }
}
