﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.UI.WebControls;
using DotLiquid;
using Newtonsoft.Json;
using Rock.Model;

namespace Rock
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class ExtensionMethods
    {
        #region Object Extensions

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns></returns>
        public static string ToJson( this object obj )
        {
            return JsonConvert.SerializeObject( obj, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                } );
        }


        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="rootObj">The root obj.</param>
        /// <param name="propertyPathName">Name of the property path.</param>
        /// <returns></returns>
        public static object GetPropertyValue( this object rootObj, string propertyPathName )
        {
            var propPath = propertyPathName.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries ).ToList<string>();

            object obj = rootObj;
            Type objType = rootObj.GetType();

            while ( propPath.Any() && obj != null )
            {
                PropertyInfo property = objType.GetProperty( propPath.First() );
                if ( property != null )
                {
                    obj = property.GetValue( obj );
                    objType = property.PropertyType;
                    propPath = propPath.Skip( 1 ).ToList();
                }
                else
                {
                    obj = null;
                }
            }

            return obj;
        }

        /// <summary>
        /// Safely ToString() this item, even if it's null.
        /// </summary>
        /// <param name="obj">an object</param>
        /// <returns>The ToString or the empty string if the item is null.</returns>
        public static string ToStringSafe( this object obj )
        {
            if ( obj != null )
            {
                return obj.ToString();
            }
            return String.Empty;
        }
        #endregion

        #region Type Extensions

        /// <summary>
        /// Gets the name of the friendly type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetFriendlyTypeName( this Type type )
        {
            Rock.Data.FriendlyTypeNameAttribute attrib = type.GetTypeInfo().GetCustomAttribute<Rock.Data.FriendlyTypeNameAttribute>();
            if ( attrib != null )
            {
                return attrib.FriendlyTypeName;
            }
            else
            {
                if ( type.Namespace == null )
                {
                    // Anonymous types will not have a namespace
                    return "Item";
                }

                if ( type.Namespace.Equals( "System.Data.Entity.DynamicProxies" ) )
                {
                    type = type.BaseType;
                }

                if ( type.Namespace.Equals( "Rock.Model" ) )
                {
                    var entityType = Rock.Web.Cache.EntityTypeCache.Read( type );
                    return entityType.FriendlyName ?? SplitCase( type.Name );
                }
                else
                {
                    return SplitCase( type.Name );
                }
            }
        }

        #endregion

        #region String Extensions

        /// <summary>
        /// Splits a Camel or Pascal cased identifier into seperate words.
        /// </summary>
        /// <param name="str">The identifier.</param>
        /// <returns></returns>
        public static string SplitCase( this string str )
        {
            if ( str == null )
                return null;

            return Regex.Replace( Regex.Replace( str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2" ), @"(\p{Ll})(\P{Ll})", "$1 $2" );
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by any combination of whitespace, comma, semi-colon, or pipe characters
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="whitespace">if set to <c>true</c> [whitespace].</param>
        /// <returns></returns>
        public static string[] SplitDelimitedValues( this string str, bool whitespace = true )
        {
            if ( str == null )
                return new string[0];

            string regex = whitespace ? @"[\s\|,;]+" : @"[\|,;]+";

            char[] delimiter = new char[] { ',' };
            return Regex.Replace( str, regex, "," ).Split( delimiter, StringSplitOptions.RemoveEmptyEntries );
        }

        /// <summary>
        /// Replaces every instance of oldValue (regardless of case) with the newValue.
        /// from http://www.codeproject.com/Articles/10890/Fastest-C-Case-Insenstive-String-Replace
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="oldValue">The value to replace.</param>
        /// <param name="newValue">The value to insert.</param>
        /// <returns></returns>
        public static string ReplaceCaseInsensitive( this string str, string oldValue, string newValue )
        {
            if ( str == null )
                return null;

            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = str.ToUpper();
            string upperPattern = oldValue.ToUpper();
            int inc = ( str.Length / oldValue.Length ) *
                      ( newValue.Length - oldValue.Length );
            char[] chars = new char[str.Length + Math.Max( 0, inc )];
            while ( ( position1 = upperString.IndexOf( upperPattern,
                                              position0 ) ) != -1 )
            {
                for ( int i = position0; i < position1; ++i )
                    chars[count++] = str[i];
                for ( int i = 0; i < newValue.Length; ++i )
                    chars[count++] = newValue[i];
                position0 = position1 + oldValue.Length;
            }
            if ( position0 == 0 ) return str;
            for ( int i = position0; i < str.Length; ++i )
                chars[count++] = str[i];
            return new string( chars, 0, count );
        }

        /// <summary>
        /// Replaces every instance of oldValue with newValue.  Will continue to replace
        /// values after each replace until the oldValue does not exist.  
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="oldValue">The value to replace.</param>
        /// <param name="newValue">The value to insert.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceWhileExists( this string str, string oldValue, string newValue )
        {
            string newstr = str;

            if ( oldValue != newValue )
            {
                while ( newstr.Contains( oldValue ) )
                {
                    newstr = newstr.Replace( oldValue, newValue );
                }
            }

            return newstr;

        }
        /// <summary>
        /// Adds escape character for quotes in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeQuotes( this string str )
        {
            if ( str == null )
                return null;

            return str.Replace( "'", "\\'" ).Replace( "\"", "\\" );
        }

        /// <summary>
        /// Adds Quotes around the specified string and escapes any quotes that are already in the string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="QuoteChar">The quote character.</param>
        /// <returns></returns>
        public static string Quoted( this string str, string QuoteChar = "'")
        {
            var result = QuoteChar + str.EscapeQuotes() + QuoteChar;
            return result;
        }

        /// <summary>
        /// Returns the specified number of characters, starting at the left side of the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The desired length.</param>
        /// <returns></returns>
        public static string Left( this string str, int length )
        {
            if ( str.Length <= length )
            {
                return str;
            }
            else
            {
                return str.Substring( 0, length );
            }
        }

        /// <summary>
        /// Truncates a string after a max length and adds ellipsis.  Truncation will occur at first space prior to maxLength
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate( this string str, int maxLength )
        {
            if ( str == null )
                return null;

            if ( str.Length <= maxLength )
                return str;

            maxLength -= 3;
            var truncatedString = str.Substring( 0, maxLength );
            var lastSpace = truncatedString.LastIndexOf( ' ' );
            if ( lastSpace > 0 )
                truncatedString = truncatedString.Substring( 0, lastSpace );

            return truncatedString + "...";
        }

        /// <summary>
        /// Pluralizes the specified string.
        /// </summary>
        /// <param name="str">The string to pluralize.</param>
        /// <returns></returns>
        public static string Pluralize( this string str )
        {
            // Pluralization services handles most words, but there are some exceptions (i.e. campus)
            switch ( str )
            {
                case "Campus":
                case "campus":
                    return str + "es";
                case "CAMPUS":
                    return str + "ES";
                default:
                    var pluralizationService = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService( new System.Globalization.CultureInfo( "en-US" ) );
                    return pluralizationService.Pluralize( str );
            }
        }

        /// <summary>
        /// Singularizes the specified string.
        /// </summary>
        /// <param name="str">The string to singularize.</param>
        /// <returns></returns>
        public static string Singularize( this string str )
        {
            var pluralizationService = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService( new System.Globalization.CultureInfo( "en-US" ) );
            return pluralizationService.Singularize( str );
        }

        /// <summary>
        /// Removes any non-numeric characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AsNumeric( this string str )
        {
            return Regex.Replace( str, @"[^0-9]", "" );
        }

        /// <summary>
        /// Returns True for 'True', 'Yes', 'T', 'Y', '1'
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static bool AsBoolean( this string str )
        {
            string[] trueStrings = new string[] { "true", "yes", "t", "y", "1" };

            if ( string.IsNullOrWhiteSpace( str ) )
            {
                return false;
            }

            return trueStrings.Contains( str.ToLower() );
        }

        /// <summary>
        /// Attempts to convert string to integer.  Returns null if unsuccessful.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="emptyStringAsZero">if set to <c>true</c> [empty string as zero].</param>
        /// <returns></returns>
        public static int? AsInteger( this string str, bool emptyStringAsZero = true )
        {
            if ( !emptyStringAsZero )
            {
                if ( string.IsNullOrWhiteSpace( str ) )
                {
                    return null;
                }
            }

            int value;
            if ( int.TryParse( str, out value ) )
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to convert string to Guid.  Returns Guid.Empty if unsuccessful.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Guid AsGuid( this string str )
        {
            Guid value;
            if ( Guid.TryParse( str, out value ) )
            {
                return value;
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Attempts to convert string to decimal.  Returns null if unsuccessful.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="emptyStringAsZero">if set to <c>true</c> [empty string as zero].</param>
        /// <returns></returns>
        public static decimal? AsDecimal( this string str, bool emptyStringAsZero = true )
        {
            if ( !emptyStringAsZero )
            {
                if ( string.IsNullOrWhiteSpace( str ) )
                {
                    return null;
                }
            }

            if ( !string.IsNullOrWhiteSpace( str ) )
            {
                // strip off the currency symbol if there is one
                str = str.Replace( "$", string.Empty );
            }

            decimal value;
            if ( decimal.TryParse( str, out value ) )
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to convert string to DateTime.  Returns null if unsuccessful.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static DateTime? AsDateTime( this string str )
        {
            DateTime value;
            if ( DateTime.TryParse( str, out value ) )
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Use DotLiquid to resolve any merge codes within the content using the values 
        /// in the mergeObjects.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="mergeObjects">The merge objects.</param>
        /// <returns></returns>
        public static string ResolveMergeFields( this string content, Dictionary<string, object> mergeObjects )
        {
            if ( content == null )
                return string.Empty;

            // If there's no merge codes, just return the content
            if ( !Regex.IsMatch( content, @".*\{.+\}.*" ) )
                return content;

            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            Template template = Template.Parse( content );

            return template.Render( Hash.FromDictionary( mergeObjects ) );
        }

        /// <summary>
        /// Converts string to a HTML title "<span class='first-word'>first-word</span> rest of string"
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string FormatAsHtmlTitle( this string str )
        {
            // Remove any HTML
            string encodedStr = System.Web.HttpUtility.HtmlEncode( str );

            // split first word from rest of string
            int endOfFirstWord = encodedStr.IndexOf( " " );

            if ( endOfFirstWord != -1 )
                return "<span class='first-word'>" + encodedStr.Substring( 0, endOfFirstWord ) + " </span> " + encodedStr.Substring( endOfFirstWord, encodedStr.Length - endOfFirstWord );
            else
                return "<span class='first-word'>" + encodedStr + " </span>";
        }

        /// <summary>
        /// Converts CR (carriage return) LF (line feed) to non-encoded html breaks (br).
        /// </summary>
        /// <param name="str">a string that contains CR LF</param>
        /// <returns>a string with CRLF replaced with html <code>br</code></returns>
        public static string ConvertCrLfToHtmlBr( this string str )
        {
            if ( str == null )
            {
                return string.Empty;
            }

            return str.Replace( Environment.NewLine, "<br/>" ).Replace( "\x0A", "<br/>" );
        }

        /// <summary>
        /// Encodes any html and then converts CR (carriage return) LF (line feed) to unencoded html breaks (br).
        /// </summary>
        /// <param name="str">a string that contains unencoded HTML and CR LF</param>
        /// <returns>an html encoded string with CRLF replaced with HTML <code>br</code></returns>
        public static string EncodeHtmlThenConvertCrLfToHtmlBr( this string str )
        {
            if ( str == null )
            {
                return string.Empty;
            }

            return System.Web.HttpUtility.HtmlEncode( str ).ConvertCrLfToHtmlBr();
        }

        /// <summary>
        /// Sanitizes the HTML by removing tags.  If Scrict is true, all html tags will be removed, if false, only a blacklist of specific XSS dangerous tags and attribute values are removed.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="strict">if set to <c>true</c> [strict].</param>
        /// <returns></returns>
        public static string SanitizeHtml(this string html, bool strict = true)
        {
            if ( strict )
            {
                var allowedElements = new Dictionary<string, string[]>();
                var allowedAttributes = new Dictionary<string, string[]>();
                return new AjaxControlToolkit.Sanitizer.HtmlAgilityPackSanitizerProvider().GetSafeHtmlFragment( html, allowedElements, allowedAttributes );
            } 
            else
            {
                return Rock.Web.Utilities.HtmlSanitizer.SanitizeHtml( html );
            }
        }

        /// <summary>
        /// Returns true if the given string is a valid email address.
        /// </summary>
        /// <param name="email">The string to validate</param>
        /// <returns>true if valid email, false otherwise</returns>
        public static bool IsValidEmail( this string email )
        {
            return Regex.IsMatch( email, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" );
        }

        /// <summary>
        /// Converts the value to Type, or if unsuccessful, returns the default value of Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T AsType<T>( this string value )
        {
            var converter = TypeDescriptor.GetConverter( typeof( T ) );
            return converter.IsValid( value )
                ? (T)converter.ConvertFrom( value )
                : default( T );
        }

        /// <summary>
        /// Maskeds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Masked( this string value )
        {
            if ( value.Length > 4 )
            {
                return string.Concat( new string( '*', 12 ), value.Substring( value.Length - 4 ) );
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region Int Extensions

        /// <summary>
        /// Gets the Defined Value name associated with this id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string DefinedValue( this int? id )
        {
            if ( !id.HasValue )
                return string.Empty;

            var definedValue = Rock.Web.Cache.DefinedValueCache.Read( id.Value );
            if ( definedValue != null )
                return definedValue.Name;
            else
                return string.Empty;
        }

        #endregion

        #region Boolean Extensions

        /// <summary>
        /// A numeric 1 or 0
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int Bit( this Boolean field )
        {
            return field ? 1 : 0;
        }

        /// <summary>
        /// To the yes no.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string ToYesNo( this bool value )
        {
            return value ? "Yes" : "No";
        }

        /// <summary>
        /// To the true false.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string ToTrueFalse( this bool value )
        {
            return value ? "True" : "False";
        }

        /// <summary>
        /// Froms the true false.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool FromTrueFalse( this string value )
        {
            return value.Equals( "True" );
        }

        #endregion

        #region DateTime Extensions

        /// <summary>
        /// Returns the age at the current date
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int Age( this DateTime? start )
        {
            if ( start.HasValue )
                return start.Value.Age();
            else
                return 0;
        }

        /// <summary>
        /// Returns the age at the current date
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int Age( this DateTime start )
        {
            var now = DateTime.Today;
            int age = now.Year - start.Year;
            if ( start > now.AddYears( -age ) ) age--;

            return age;
        }

        /// <summary>
        /// The total months.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static int TotalMonths( this DateTime end, DateTime start )
        {
            return ( end.Year * 12 + end.Month ) - ( start.Year * 12 + start.Month );
        }

        /// <summary>
        /// The total years.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static int TotalYears( this DateTime end, DateTime start )
        {
            return ( end.Year ) - ( start.Year );
        }

        /// <summary>
        /// Returns a friendly elapsed time string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="condensed">if set to <c>true</c> [condensed].</param>
        /// <param name="includeTime">if set to <c>true</c> [include time].</param>
        /// <returns></returns>
        public static string ToElapsedString( this DateTime? dateTime, bool condensed = false, bool includeTime = true )
        {
            if ( dateTime.HasValue )
            {
                return ToElapsedString( dateTime.Value, condensed, includeTime );
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns a friendly elapsed time string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="condensed">if set to <c>true</c> [condensed].</param>
        /// <param name="includeTime">if set to <c>true</c> [include time].</param>
        /// <returns></returns>
        public static string ToElapsedString( this DateTime dateTime, bool condensed = false, bool includeTime = true )
        {
            DateTime start = dateTime;
            DateTime end = DateTime.Now;

            string direction = " Ago";
            TimeSpan timeSpan = end.Subtract( start );
            if ( timeSpan.TotalMilliseconds < 0 )
            {
                direction = " From Now";
                start = end;
                end = dateTime;
                timeSpan = timeSpan.Negate();
            }

            string duration = "";

            if ( timeSpan.TotalHours < 24 && includeTime )
            {
                // Less than one second
                if ( timeSpan.TotalSeconds <= 1 )
                    duration = string.Format( "1{0}", condensed ? "sec" : " Second" );

                else if ( timeSpan.TotalSeconds < 60 )
                    duration = string.Format( "{0:N0}{1}", Math.Truncate( timeSpan.TotalSeconds ), condensed ? "sec" : " Seconds" );
                else if ( timeSpan.TotalMinutes < 2 )
                    duration = string.Format( "1{0}", condensed ? "min" : " Minute" );
                else if ( timeSpan.TotalMinutes < 60 )
                    duration = string.Format( "{0:N0}{1}", Math.Truncate( timeSpan.TotalMinutes ), condensed ? "min" : " Minutes" );
                else if ( timeSpan.TotalHours < 2 )
                    duration = string.Format( "1{0}", condensed ? "hr" : " Hour" );
                else if ( timeSpan.TotalHours < 24 )
                    duration = string.Format( "{0:N0}{1}", Math.Truncate( timeSpan.TotalHours ), condensed ? "hr" : " Hours" );
            }

            if ( duration == "" )
            {
                if ( timeSpan.TotalDays <= 1 )
                    duration = string.Format( "1{0}", condensed ? "day" : " Day" );
                else if ( timeSpan.TotalDays < 31 )
                    duration = string.Format( "{0:N0}{1}", Math.Truncate( timeSpan.TotalDays ), condensed ? "days" : " Days" );
                else if ( end.TotalMonths( start ) <= 1 )
                    duration = string.Format( "1{0}", condensed ? "mon" : " Month" );
                else if ( end.TotalMonths( start ) <= 18 )
                    duration = string.Format( "{0:N0}{1}", end.TotalMonths( start ), condensed ? "mon" : " Months" );
                else if ( end.TotalYears( start ) <= 1 )
                    duration = string.Format( "1{0}", condensed ? "yr" : " Year" );
                else
                    duration = string.Format( "{0:N0}{1}", end.TotalYears( start ), condensed ? "yrs" : " Years" );
            }

            return duration + ( condensed ? "" : direction );

        }

        /// <summary>
        /// Returns a string in FB style relative format (x seconds ago, x minutes ago, about an hour ago, etc.).
        /// or if max days has already passed in FB datetime format (February 13 at 11:28am or November 5, 2011 at 1:57pm)
        /// </summary>
        /// <param name="dateTime">the datetime to convert to relative time.</param>
        /// <param name="maxDays">maximum number of days before formatting in FB date-time format (ex. November 5, 2011 at 1:57pm) </param>
        /// <returns></returns>
        public static string ToRelativeDateString( this DateTime dateTime, int? maxDays = null )
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSince = now - dateTime;

                double inSeconds = timeSince.TotalSeconds;
                double inMinutes = timeSince.TotalMinutes;
                double inHours = timeSince.TotalHours;
                double inDays = timeSince.TotalDays;
                double inWeeks = inDays / 7;
                double inMonths = inDays / 30;
                double inYears = inDays / 365;

                // Just return in FB time format if max days has passed.
                if ( maxDays.HasValue && inDays > maxDays )
                {
                    if ( now.Year == dateTime.Year )
                    {
                        return dateTime.ToString( "MMMM d at h:mmtt" ).ToLowerInvariant();
                    }
                    else
                    {
                        return dateTime.ToString( "MMMM d, yyyy at h:mmtt" ).ToLowerInvariant();
                    }
                }

                if ( Math.Round( inSeconds ) < 5 )
                {
                    return "just now";
                }
                else if ( inMinutes < 1.0 )
                {
                    return Math.Floor( inSeconds ) + " seconds ago";
                }
                else if ( Math.Floor( inMinutes ) == 1 )
                {
                    return "1 minute ago";
                }
                else if ( inHours < 1.0 )
                {
                    return Math.Floor( inMinutes ) + " minutes ago";
                }
                else if ( Math.Floor( inHours ) == 1 )
                {
                    return "about an hour ago";
                }
                else if ( inDays < 1.0 )
                {
                    return Math.Floor( inHours ) + " hours ago";
                }
                else if ( Math.Floor( inDays ) == 1 )
                {
                    return "1 day ago";
                }
                else if ( inWeeks < 1 )
                {
                    return Math.Floor( inDays ) + " days ago";
                }
                else if ( Math.Floor( inWeeks ) == 1 )
                {
                    return "1 week ago";
                }
                else if ( inMonths < 3 )
                {
                    return Math.Floor( inWeeks ) + " weeks ago";
                }
                else if ( inMonths <= 12 )
                {
                    return Math.Floor( inMonths ) + " months ago ";
                }
                else if ( Math.Floor( inYears ) <= 1 )
                {
                    return "1 year ago";
                }
                else
                {
                    return Math.Floor( inYears ) + " years ago";
                }
            }
            catch ( Exception )
            {
            }
            return "";
        }
        #endregion

        #region TimeSpan Extensions

        /// <summary>
        /// Returns a TimeSpan to HH:MM AM/PM.
        /// Examples: 1:45 PM, 12:01 AM
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        /// <returns></returns>
        public static string ToTimeString( this TimeSpan timespan )
        {
            return DateTime.Today.Add( timespan ).ToShortTimeString();
        }

        #endregion

        #region Control Extensions

        /// <summary>
        /// Loads a user control using the constructor with the parameters specified.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="userControlPath">The user control path.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <returns></returns>
        public static System.Web.UI.UserControl LoadControl( this System.Web.UI.Control control, string userControlPath, params object[] constructorParameters )
        {
            List<Type> constParamTypes = new List<Type>();
            foreach ( object constParam in constructorParameters )
                constParamTypes.Add( constParam.GetType() );

            System.Web.UI.UserControl ctl = control.Page.LoadControl( userControlPath ) as System.Web.UI.UserControl;

            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor( constParamTypes.ToArray() );

            if ( constructor == null )
                throw new MemberAccessException( "The requested constructor was not found on " + ctl.GetType().BaseType.ToString() );
            else
                constructor.Invoke( ctl, constructorParameters );

            return ctl;
        }

        /// <summary>
        /// Gets the parent RockBlock.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static Rock.Web.UI.RockBlock RockBlock( this System.Web.UI.Control control )
        {
            System.Web.UI.Control parentControl = control.Parent;
            while ( parentControl != null )
            {
                if ( parentControl is Rock.Web.UI.RockBlock )
                {
                    return (Rock.Web.UI.RockBlock)parentControl;
                }
                parentControl = parentControl.Parent;
            }
            return null;
        }

        /// <summary>
        /// Parents the update panel.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static System.Web.UI.UpdatePanel ParentUpdatePanel( this System.Web.UI.Control control )
        {
            System.Web.UI.Control parentControl = control.Parent;
            while ( parentControl != null )
            {
                if ( parentControl is System.Web.UI.UpdatePanel )
                {
                    return (System.Web.UI.UpdatePanel)parentControl;
                }
                parentControl = parentControl.Parent;
            }
            return null;
        }

        /// <summary>
        /// Gets all controls of Type recursively
        /// http://stackoverflow.com/questions/7362482/c-sharp-get-all-web-controls-on-page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlCollection">The control collection.</param>
        /// <param name="resultCollection">The result collection.</param>
        private static void GetControlListRecursive<T>( this System.Web.UI.ControlCollection controlCollection, List<T> resultCollection ) where T : System.Web.UI.Control
        {
            foreach ( System.Web.UI.Control control in controlCollection )
            {
                if ( control is T )
                {
                    resultCollection.Add( (T)control );
                }

                if ( control.HasControls() )
                {
                    GetControlListRecursive( control.Controls, resultCollection );
                }
            }
        }

        /// <summary>
        /// Gets all controls of Type recursively
        /// http://stackoverflow.com/questions/7362482/c-sharp-get-all-web-controls-on-page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static List<T> ControlsOfTypeRecursive<T>( this System.Web.UI.Control control ) where T : System.Web.UI.Control
        {
            List<T> result = new List<T>();
            GetControlListRecursive<T>( control.Controls, result );
            return result;
        }

        #endregion

        #region WebControl Extensions

        /// <summary>
        /// Adds a CSS class name to a web control
        /// </summary>
        /// <param name="webControl">The web control.</param>
        /// <param name="className">Name of the class.</param>
        public static void AddCssClass( this System.Web.UI.WebControls.WebControl webControl, string className )
        {
            string match = @"\b" + className + "\b";
            string css = webControl.CssClass;

            if ( !Regex.IsMatch( css, match, RegexOptions.IgnoreCase ) )
                webControl.CssClass = Regex.Replace( css + " " + className, @"^\s+", "", RegexOptions.IgnoreCase );
        }

        /// <summary>
        /// Removes a CSS class name from a web control.
        /// </summary>
        /// <param name="webControl">The web control.</param>
        /// <param name="className">Name of the class.</param>
        public static void RemoveCssClass( this System.Web.UI.WebControls.WebControl webControl, string className )
        {
            string match = @"\s*\b" + className + @"\b";
            string css = webControl.CssClass;

            if ( Regex.IsMatch( css, match, RegexOptions.IgnoreCase ) )
                webControl.CssClass = Regex.Replace( css, match, "", RegexOptions.IgnoreCase );
        }

        #endregion

        #region HtmlControl Extensions

        /// <summary>
        /// Adds a CSS class name to an html control
        /// </summary>
        /// <param name="htmlControl">The html control.</param>
        /// <param name="className">Name of the class.</param>
        public static void AddCssClass( this System.Web.UI.HtmlControls.HtmlControl htmlControl, string className )
        {
            string match = @"\b" + className + "\b";
            string css = htmlControl.Attributes["class"] ?? string.Empty;

            if ( !Regex.IsMatch( css, match, RegexOptions.IgnoreCase ) )
                htmlControl.Attributes["class"] = Regex.Replace( css + " " + className, @"^\s+", "", RegexOptions.IgnoreCase );
        }

        /// <summary>
        /// Removes a CSS class name from an html control.
        /// </summary>
        /// <param name="htmlControl">The html control.</param>
        /// <param name="className">Name of the class.</param>
        public static void RemoveCssClass( this System.Web.UI.HtmlControls.HtmlControl htmlControl, string className )
        {
            string match = @"\s*\b" + className + @"\b";
            string css = htmlControl.Attributes["class"] ?? string.Empty;

            if ( Regex.IsMatch( css, match, RegexOptions.IgnoreCase ) )
                htmlControl.Attributes["class"] = Regex.Replace( css, match, "", RegexOptions.IgnoreCase );
        }

        #endregion

        #region CheckBoxList Extensions

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="checkBoxList">The check box list.</param>
        /// <param name="values">The values.</param>
        public static void SetValues( this CheckBoxList checkBoxList, List<string> values )
        {
            foreach ( ListItem item in checkBoxList.Items )
            {
                item.Selected = values.Contains( item.Value, StringComparer.OrdinalIgnoreCase );
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="checkBoxList">The check box list.</param>
        /// <param name="values">The values.</param>
        public static void SetValues( this CheckBoxList checkBoxList, List<int> values )
        {
            foreach ( ListItem item in checkBoxList.Items )
            {
                int numValue = int.MinValue;
                item.Selected = int.TryParse( item.Value, out numValue ) && values.Contains( numValue );
            }
        }

        #endregion

        #region ListControl Extensions

        /// <summary>
        /// Try's to set the selected value, if the value does not exist, will set the first item in the list
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="value">The value.</param>
        public static void SetValue( this ListControl listControl, string value )
        {
            try
            {
                listControl.SelectedValue = value;
            }
            catch
            {
                if ( listControl.Items.Count > 0 )
                    listControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Sets the read only value.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="value">The value.</param>
        public static void SetReadOnlyValue( this ListControl listControl, string value )
        {
            listControl.Items.Clear();
            listControl.Items.Add( value );
        }

        /// <summary>
        /// Try's to set the selected value, if the value does not exist, will set the first item in the list
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="value">The value.</param>
        public static void SetValue( this ListControl listControl, int? value )
        {
            listControl.SetValue( value == null ? "0" : value.ToString() );
        }

        /// <summary>
        /// Binds to enum.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="insertBlankOption">if set to <c>true</c> [insert blank option].</param>
        public static void BindToEnum( this ListControl listControl, Type enumType, bool insertBlankOption = false )
        {
            var dictionary = new Dictionary<int, string>();
            foreach ( var value in Enum.GetValues( enumType ) )
            {
                dictionary.Add( Convert.ToInt32( value ), Enum.GetName( enumType, value ).SplitCase() );
            }

            listControl.DataSource = dictionary;
            listControl.DataTextField = "Value";
            listControl.DataValueField = "Key";
            listControl.DataBind();

            if ( insertBlankOption )
            {
                listControl.Items.Insert( 0, new ListItem() );
            }
        }

        /// <summary>
        /// Binds to the values of a definedType
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="definedType">Type of the defined.</param>
        /// <param name="insertBlankOption">if set to <c>true</c> [insert blank option].</param>
        public static void BindToDefinedType( this ListControl listControl, Rock.Web.Cache.DefinedTypeCache definedType, bool insertBlankOption = false )
        {
            var ds = definedType.DefinedValues
                .Select( v => new
                {
                    v.Name,
                    v.Id
                } );

            listControl.SelectedIndex = -1;
            listControl.DataSource = ds;
            listControl.DataTextField = "Name";
            listControl.DataValueField = "Id";
            listControl.DataBind();

            if ( insertBlankOption )
            {
                listControl.Items.Insert( 0, new ListItem() );
            }

        }

        /// <summary>
        /// Returns the Value as Int or null if Value is <see cref="T:Rock.Constants.None"/>
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="NoneAsNull">if set to <c>true</c>, will return Null if SelectedValue = <see cref="T:Rock.Constants.None" /> </param>
        /// <returns></returns>
        public static int? SelectedValueAsInt( this ListControl listControl, bool NoneAsNull = true )
        {
            if ( NoneAsNull )
            {
                if ( listControl.SelectedValue.Equals( Rock.Constants.None.Id.ToString() ) )
                {
                    return null;
                }
            }

            if ( string.IsNullOrWhiteSpace( listControl.SelectedValue ) )
            {
                return null;
            }
            else
            {
                return int.Parse( listControl.SelectedValue );
            }
        }

        /// <summary>
        /// Returns the value of the currently selected item.
        /// It will return NULL if either <see cref="T:Rock.Constants.None"/> or <see cref="T:Rock.Constants.All"/> is selected.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <returns></returns>
        public static int? SelectedValueAsId( this ListControl listControl )
        {
            int? result = SelectedValueAsInt( listControl );

            if ( result == Rock.Constants.All.Id )
            {
                return null;
            }

            if ( result == Rock.Constants.None.Id )
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Selecteds the value as enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SelectedValueAsEnum<T>( this ListControl listControl )
        {
            return (T)System.Enum.Parse( typeof( T ), listControl.SelectedValue );
        }

        #endregion

        #region Enum Extensions

        /// <summary>
        /// Converts to the enum value to it's string value
        /// </summary>
        /// <param name="eff">The eff.</param>
        /// <param name="SplitCase">if set to <c>true</c> [split case].</param>
        /// <returns></returns>
        public static String ConvertToString( this Enum eff, bool SplitCase = true )
        {
            if ( SplitCase )
            {
                return Enum.GetName( eff.GetType(), eff ).SplitCase();
            }
            else
            {
                return Enum.GetName( eff.GetType(), eff );
            }
        }

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDescription( this Enum value )
        {
            var type = value.GetType();
            string name = Enum.GetName( type, value );
            if ( name != null )
            {
                System.Reflection.FieldInfo field = type.GetField( name );
                if ( field != null )
                {
                    var attr = System.Attribute.GetCustomAttribute( field,
                        typeof( DescriptionAttribute ) ) as DescriptionAttribute;
                    if ( attr != null )
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="eff">The eff.</param>
        /// <returns></returns>
        public static int ConvertToInt( this Enum eff )
        {
            return Convert.ToInt32( eff );
        }

        /// <summary>
        /// Converts a string value to an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="defaultValue">The default value to use if the value cannot be parsed. Leave null to throw an exception if the value cannot be parsed. </param>
        /// <returns></returns>
        public static T ConvertToEnum<T>( this String enumValue, T? defaultValue = null ) where T : struct // actually limited to enum, but struct is the closest we can do
        {
            if ( defaultValue.HasValue )
            {
                T result;
                if ( Enum.TryParse<T>( enumValue, out result ) )
                {
                    return result;
                }
                else
                {
                    return defaultValue.Value;
                }
            }
            else
            {
                return (T)Enum.Parse( typeof( T ), enumValue.Replace( " ", "" ) );
            }
        }

        #endregion

        #region GenericCollection Extensions

        /// <summary>
        /// Concatonate the items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string AsDelimited<T>( this List<T> items, string delimiter )
        {
            List<string> strings = new List<string>();
            foreach ( T item in items )
                strings.Add( item.ToString() );
            return String.Join( delimiter, strings.ToArray() );
        }

        /// <summary>
        /// Joins a dictionary of items
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="delimter">The delimter.</param>
        /// <returns></returns>
        public static string Join( this Dictionary<string, string> items, string delimter )
        {
            List<string> parms = new List<string>();
            foreach ( var item in items )
                parms.Add( string.Join( "=", new string[] { item.Key, item.Value } ) );
            return string.Join( delimter, parms.ToArray() );
        }

        #endregion

        #region IQueryable extensions

        /// <summary>
        /// Orders the list by the name of a property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The type of object.</param>
        /// <param name="property">The property to order by.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>( this IQueryable<T> source, string property )
        {
            return ApplyOrder<T>( source, property, "OrderBy" );
        }

        /// <summary>
        /// Orders the list by the name of a property in descending order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The type of object.</param>
        /// <param name="property">The property to order by.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>( this IQueryable<T> source, string property )
        {
            return ApplyOrder<T>( source, property, "OrderByDescending" );
        }

        /// <summary>
        /// Then Orders the list by the name of a property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The type of object.</param>
        /// <param name="property">The property to order by.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>( this IOrderedQueryable<T> source, string property )
        {
            return ApplyOrder<T>( source, property, "ThenBy" );
        }

        /// <summary>
        /// Then Orders the list by a a property in descending order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The type of object.</param>
        /// <param name="property">The property to order by.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>( this IOrderedQueryable<T> source, string property )
        {
            return ApplyOrder<T>( source, property, "ThenByDescending" );
        }

        static IOrderedQueryable<T> ApplyOrder<T>( IQueryable<T> source, string property, string methodName )
        {
            string[] props = property.Split( '.' );
            Type type = typeof( T );
            ParameterExpression arg = Expression.Parameter( type, "x" );
            Expression expr = arg;
            foreach ( string prop in props )
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty( prop );
                expr = Expression.Property( expr, pi );
                type = pi.PropertyType;
            }
            Type delegateType = typeof( Func<,> ).MakeGenericType( typeof( T ), type );
            LambdaExpression lambda = Expression.Lambda( delegateType, expr, arg );

            object result = typeof( Queryable ).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2 )
                    .MakeGenericMethod( typeof( T ), type )
                    .Invoke( null, new object[] { source, lambda } );
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// Sorts the object by the specified sort property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sortProperty">The sort property.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Sort<T>( this IQueryable<T> source, Rock.Web.UI.Controls.SortProperty sortProperty )
        {
            string[] columns = sortProperty.Property.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

            IOrderedQueryable<T> qry = null;

            for ( int columnIndex = 0; columnIndex < columns.Length; columnIndex++ )
            {
                string column = columns[columnIndex].Trim();
                if ( sortProperty.Direction == System.Web.UI.WebControls.SortDirection.Ascending )
                {
                    qry = ( columnIndex == 0 ) ? source.OrderBy( column ) : qry.ThenBy( column );
                    if ( columnIndex == 0 )
                    {
                        qry = source.OrderBy( column );
                    }
                    else
                    {
                        qry = qry.ThenBy( column );
                    }
                }
                else
                {
                    qry = ( columnIndex == 0 ) ? source.OrderByDescending( column ) : qry.ThenByDescending( column );
                }
            }

            return qry;
        }

        /// <summary>
        /// Filters a Query to rows that have matching attribute value
        /// (must be in a UnitOfWorkScope codeblock)
        /// </summary>
        /// <example>
        /// using ( new Rock.Data.UnitOfWorkScope() )
        /// {
        ///     var test = new PersonService().Queryable().Where( a => a.FirstName == "Bob" ).WhereAttributeValue( "IsBaptized", "true" ).ToList();
        /// }
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="attributeKey">The attribute key.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        public static IQueryable<T> WhereAttributeValue<T>( this IQueryable<T> source, string attributeKey, string attributeValue ) where T : Rock.Data.Model<T>, new()
        {
            int entityTypeId = Rock.Web.Cache.EntityTypeCache.GetId( typeof( T ) ) ?? 0;

            var avs = new AttributeValueService().Queryable()
                .Where( a => a.Attribute.Key == attributeKey )
                .Where( a => a.Attribute.EntityTypeId == entityTypeId )
                .Where( a => a.Value == attributeValue )
                .Select( a => a.EntityId );

            var result = source.Where( a => avs.Contains( ( a as T ).Id ) );
            return result;
        }

        #endregion

        #region IHasAttributes extensions

        /// <summary>
        /// Loads the attributes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void LoadAttributes( this Rock.Attribute.IHasAttributes entity )
        {
            Rock.Attribute.Helper.LoadAttributes( entity );
        }

        /// <summary>
        /// Copies the attributes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="source">The source.</param>
        public static void CopyAttributesFrom( this Rock.Attribute.IHasAttributes entity, Rock.Attribute.IHasAttributes source )
        {
            Rock.Attribute.Helper.CopyAttributes( source, entity );
        }

        #endregion

        #region Route Extensions

        /// <summary>
        /// Pages the id.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public static int PageId( this Route route )
        {
            if ( route.DataTokens != null )
            {
                return int.Parse( route.DataTokens["PageId"] as string );
            }

            return -1;
        }

        /// <summary>
        /// Routes the id.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public static int RouteId( this Route route )
        {
            if ( route.DataTokens != null )
            {
                return int.Parse( route.DataTokens["RouteId"] as string );
            }

            return -1;
        }

        /// <summary>
        /// Adds the page route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="pageRoute">The page route.</param>
        public static void AddPageRoute( this Collection<RouteBase> routes, PageRoute pageRoute )
        {
            Route route = new Route( pageRoute.Route, new Rock.Web.RockRouteHandler() );
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens.Add( "PageId", pageRoute.PageId.ToString() );
            route.DataTokens.Add( "RouteId", pageRoute.Id.ToString() );
            routes.Add( route );
        }

        #endregion

        #region IEntity extensions

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="id">The id.</param>
        public static void RemoveEntity<T>( this List<T> list, int id ) where T : Rock.Data.IEntity
        {
            var item = list.FirstOrDefault( a => a.Id.Equals( id ) );
            if ( item != null )
            {
                list.Remove( item );
            }
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="guid">The GUID.</param>
        public static void RemoveEntity<T>( this List<T> list, Guid guid ) where T : Rock.Data.IEntity
        {
            var item = list.FirstOrDefault( a => a.Guid.Equals( guid ) );
            if ( item != null )
            {
                list.Remove( item );
            }

        }

        #endregion

        #region HiddenField Extensions

        /// <summary>
        /// Values as int.
        /// </summary>
        /// <param name="hiddenField">The hidden field.</param>
        /// <returns></returns>
        public static int ValueAsInt( this HiddenField hiddenField )
        {
            int intValue = 0;
            if ( int.TryParse( hiddenField.Value, out intValue ) )
            {
                return intValue;
            }

            return 0;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="hiddenField">The hidden field.</param>
        /// <param name="value">The value.</param>
        public static void SetValue( this HiddenField hiddenField, int value )
        {
            hiddenField.Value = value.ToString();
        }

        /// <summary>
        /// Determines whether the specified hidden field is zero.
        /// </summary>
        /// <param name="hiddenField">The hidden field.</param>
        /// <returns>
        ///   <c>true</c> if the specified hidden field is zero; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZero( this HiddenField hiddenField )
        {
            return hiddenField.Value.Equals( "0" );
        }

        #endregion
    }
}