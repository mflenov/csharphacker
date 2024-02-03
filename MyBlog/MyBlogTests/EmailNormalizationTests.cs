using System;
namespace MyBlogTests
{
    public class EmailNormalizationTests
    {
        [Test]
        public void Md5Test()
        {
            Assert.AreEqual("test", MyBlog.BL.General.BlHelpers.NormilizeEmail("test"));
            Assert.AreEqual("test@gmail.com", MyBlog.BL.General.BlHelpers.NormilizeEmail("test@gmail.com"));
            Assert.AreEqual("test@gmail.com", MyBlog.BL.General.BlHelpers.NormilizeEmail("t.e.s.t@gmail.com"));
            Assert.AreEqual("test@gmail.com", MyBlog.BL.General.BlHelpers.NormilizeEmail("test+1@gmail.com"));
            Assert.AreEqual("test@gmail.com", MyBlog.BL.General.BlHelpers.NormilizeEmail("t.e.st+234@gmail.com"));
        }
    }
}

