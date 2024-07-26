namespace DbAdapters.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="DbEnumerableAdapter{T}"/>
    /// </summary>
    [TestClass]
    public sealed class DbEnumerableAdapterUnitTests
    {
        [TestMethod]
        public void Test()
        {
            var cList = new global::System.Collections.Generic.List<string>()
            {
                "asdf",
                "qwer",
            };

            var list = cList.ToDb();
            foreach (var element in list)
            {
            }
        }
    }
}
