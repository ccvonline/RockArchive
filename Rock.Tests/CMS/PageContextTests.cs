﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using NUnit.Framework;
using Rock.Model;

namespace Rock.Tests.Cms
{
    [TestFixture]
    public class PageContextTests
    {
        public class TheExportObjectMethod
        {
            [Test]
            public void ShouldCopyEntity()
            {
                var pageContext = new PageContext { Guid = Guid.NewGuid() };
                dynamic result = pageContext.ToDynamic( true );
                Assert.AreEqual( result.Guid, pageContext.Guid );
            }
        }

        public class TheExportJsonMethod
        {
            [Test]
            public void ShouldNotBeEmpty()
            {
                var pageContext = new PageContext() { Guid = Guid.NewGuid() };
                dynamic result = pageContext.ToJson( true );
                Assert.IsNotEmpty( result );
            }
        }

        public class TheImportJsonMethod
        {
            [Test]
            public void ShouldCopyPropertiesToEntity()
            {
                var obj = new
                    {
                        Guid = Guid.NewGuid(),
                        IsSystem = false
                    };

                var json = obj.ToJSON();
                var pageContext = new PageContext();
                pageContext.FromJson( json );
                Assert.AreEqual( obj.Guid, pageContext.Guid );
                Assert.AreEqual( obj.IsSystem, pageContext.IsSystem );
            }
        }
    }
}
