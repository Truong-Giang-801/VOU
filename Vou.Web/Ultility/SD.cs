namespace Vou.Web.Ultility
{
	public class SD
	{	
		public static string BrandAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
		public const string RoleAdmin = "ADMIN";
        public const string RoleBrand = "BRAND";
        public const string RoleUser = "USER";


        public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}
