namespace Leet.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[TestCase("()[]{}", true)]
		[TestCase("(]", false)]
		[TestCase("()", true)]
		[TestCase("[", false)]
		[TestCase("]", false)]
		public void TestValid(string s, bool expected)
		{
			var actual = ValidParentheses.IsValid(s);
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}