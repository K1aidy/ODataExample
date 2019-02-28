using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataExample.Context.Entity
{
	[Table("cities")]
	public class City
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("name")]
		public string CityName { get; set; }
	}
}
