using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataExample.Context.Entity
{
	[Table("users")]
	public class User
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("infoid")]
		public int InfoId { get; set; }
		[Column("name")]
		public string Name { get; set; }

		[ForeignKey(nameof(InfoId))]
		public virtual UserInfo UserInfo { get; set; }
	}
}
