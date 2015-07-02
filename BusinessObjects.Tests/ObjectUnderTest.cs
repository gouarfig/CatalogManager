using BusinessObjects.BusinessRules;

namespace BusinessObjects.Tests
{
	public class ObjectUnderTest : BusinessObject
	{
		[Identifier]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Length(10, 20)]
		public string Between10To20 { get; set; }
	}
}