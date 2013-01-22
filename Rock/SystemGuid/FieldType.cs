﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;

namespace Rock.SystemGuid
{
    /// <summary>
    /// System Blocks.  NOTE: Some of these are referenced in Migrations to avoid string-typos.
    /// </summary>
    public static class FieldType
    {
        /// <summary>
        /// Text field type
        /// </summary>
        public static Guid TEXT { get { return new Guid( "9C204CD0-1233-41C5-818A-C5DA439445AA" ); } }

        /// <summary>
        /// Boolean field type
        /// </summary>
        public static Guid BOOLEAN { get { return new Guid( "1EDAFDED-DFE6-4334-B019-6EECBA89E05A" ); } }

        /// <summary>
        /// Date field type
        /// </summary>
        public static Guid DATE { get { return new Guid( "6B6AA175-4758-453F-8D83-FCD8044B5F36" ); } }

        /// <summary>
        /// Defined value field type
        /// </summary>
        public static Guid DEFINED_VALUE { get { return new Guid( "59D5A94C-94A0-4630-B80A-BB25697D74C7" ); } }

        /// <summary>
        /// Single Select field type
        /// </summary>
        public static Guid SINGLE_SELECT { get { return new Guid( "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0" ); } }
    }
}