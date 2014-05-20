﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;

using Rock.Attribute;
using Rock.Model;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Activates a new activity for a given activity type
    /// </summary>
    [WorkflowAttribute( "Test Attribute", "An optional attribute to test the value of before performing action.", false, "", "Compare", 0 )]
    [ComparisonField( "Compare Type", "Type of comparison to perform between Test Attribute and Compare Text or Compare Attribute.", false, "", "Compare", 1 )]
    [WorkflowTextOrAttribute( "Compare With Text", "Attribute Value", "An optional text value or attribute to compare the Test Attribute with before performing action.", false, "", "Compare", 2, "CompareTo" )]
    public abstract class CompareAction : ActionComponent
    {
        /// <summary>
        /// Compares test value to compare values and returns true if they match comparison
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        protected bool TestCompare( WorkflowAction action )
        {
            Guid guid = GetAttributeValue( action, "TestAttribute" ).AsGuid();
            if ( guid.IsEmpty() )
            {
                return true;
            }

            string testValue = GetWorklowAttributeValue( action, guid );
            if ( testValue != null )
            {
                var compare = GetAttributeValue( action, "CompareType" );
                if ( !string.IsNullOrWhiteSpace( compare ) )
                {
                    var compareType = compare.ConvertToEnum<ComparisonType>( ComparisonType.EqualTo );

                    var compareValue = GetAttributeValue( action, "CompareTo" );
                    guid = compareValue.AsGuid();
                    if ( guid.IsEmpty() )
                    {
                        return testValue.CompareTo( compareValue, compareType );
                    }
                    else
                    {
                        compareValue = GetWorklowAttributeValue( action, guid );
                        return testValue.CompareTo( compareValue, compareType );
                    }
                }
            }

            return false;
        }

    }
}