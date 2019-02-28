using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataExample.Context.Entity
{
	[Table("userinfo")]
	public class UserInfo
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("inn")]
		public string Inn { get; set; }
		[Column("cityid")]
		public int CityId { get; set; }
		[NotMapped]
		public string CityName => City?.CityName ?? string.Empty;

		[ForeignKey(nameof(CityId))]
		public virtual City City { get; set; }
	}
}
