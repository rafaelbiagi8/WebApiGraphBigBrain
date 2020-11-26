namespace WebApiGraphBigBrain.models
{
    public class AzureAD
    {
        public string AppId { get; set; }
        public string Secret { get; set; }
        public string TenantId { get; set; }
        public string Instance { get; set; }
        public string GraphResource { get; set; }
        public string GraphResourceEndPoint { get; set; }
    }
}
