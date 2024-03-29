﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

namespace Rock.Plugin.HotFixes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.Plugin.Migration" />
    [MigrationNumber( 38, "1.7.0" )]
    public class LabelFieldType : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.UpdateFieldType( "Label", "Labels that can be printed during check-in", "Rock", "Rock.Field.Types.LabelFieldType", Rock.SystemGuid.FieldType.LABEL );

            Sql( $@"
    DECLARE @BinaryFileFieldTypeId int = ( SELECT TOP 1 [Id] FROM [FieldType] WHERE [Guid] = '{Rock.SystemGuid.FieldType.BINARY_FILE}' )
    DECLARE @LabelFieldTypeId int = ( SELECT TOP 1 [Id] FROM [FieldType] WHERE [Guid] = '{Rock.SystemGuid.FieldType.LABEL}' )
    
    UPDATE A SET [FieldTypeId] = @LabelFieldTypeId
    FROM [Attribute] A 
	INNER JOIN [AttributeQualifier] Q 
		ON Q.[AttributeId] = A.[Id]
		AND Q.[Key] = 'binaryFileType'
		AND Q.[Value] = '{Rock.SystemGuid.BinaryFiletype.CHECKIN_LABEL}'
	WHERE A.[FieldTypeId] = @BinaryFileFieldTypeId
" );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}
