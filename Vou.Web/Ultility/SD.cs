namespace Vou.Web.Ultility
{
	public class SD
	{	
		public static string BrandAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
		public const string RoleAdmin = "ADMIN";
        public const string RoleBrand = "BRAND";
        public const string RoleUser = "PLAYER";
        public const string TokenCookie = "JWTToken";

		public const string Activated = "Activated";
		public const string Deactivated = "Deactivated";
        public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}
