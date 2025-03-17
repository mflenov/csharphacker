using System;
namespace MyBlogTests
{
    public class EmailNormalizationTests
    {
        [Test]
        public void Md5Test()
        {
            Assert.That(MyBlog.BL.General.BlHelpers.NormilizeEmail("test"), Is.EqualTo("test"));
            Assert.That(MyBlog.BL.General.BlHelpers.NormilizeEmail("test@gmail.com"), Is.EqualTo("test@gmail.com"));
            Assert.That(MyBlog.BL.General.BlHelpers.NormilizeEmail("t.e.s.t@gmail.com"), Is.EqualTo("test@gmail.com"));
            Assert.That(MyBlog.BL.General.BlHelpers.NormilizeEmail("test+1@gmail.com"), Is.EqualTo("test@gmail.com"));
            Assert.That(MyBlog.BL.General.BlHelpers.NormilizeEmail("t.e.st+234@gmail.com"), Is.EqualTo("test@gmail.com"));
        }
    }
}

