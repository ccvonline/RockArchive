﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Rock.Services.NuGet
{
    public static class NuGetExtensionsMethods
    {
        public static string Flatten( this IEnumerable<PackageDependencySet> dependencySets )
        {
            var dependencies = new List<dynamic>();

            foreach ( var dependencySet in dependencySets )
            {
                if ( dependencySet.Dependencies.Count == 0 )
                {
                    dependencies.Add(
                        new
                        {
                            Id = (string)null,
                            VersionSpec = (string)null,
                            TargetFramework =
                        dependencySet.TargetFramework == null ? null : VersionUtility.GetShortFrameworkName( dependencySet.TargetFramework )
                        } );
                }
                else
                {
                    foreach ( var dependency in dependencySet.Dependencies.Select( d => new { d.Id, d.VersionSpec, dependencySet.TargetFramework } ) )
                    {
                        dependencies.Add(
                            new
                            {
                                dependency.Id,
                                VersionSpec = dependency.VersionSpec == null ? null : dependency.VersionSpec.ToString(),
                                TargetFramework =
                            dependency.TargetFramework == null ? null : VersionUtility.GetShortFrameworkName( dependency.TargetFramework )
                            } );
                    }
                }
            }
            return FlattenDependencies( dependencies );
        }

        public static string Flatten( this ICollection<PackageDependency> dependencies )
        {
            return
                FlattenDependencies(
                    dependencies.Select(
                        d => new
                        {
                            d.Id,
                            VersionSpec = d.VersionSpec.ToStringSafe(),
                            TargetFramework = "" //d.TargetFramework.ToStringSafe()
                        } ) );
        }

        private static string FlattenDependencies( IEnumerable<dynamic> dependencies )
        {
            return String.Join(
                "|", dependencies.Select( d => String.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}:{2}", d.Id, d.VersionSpec, d.TargetFramework ) ) );
        }
    }
}
