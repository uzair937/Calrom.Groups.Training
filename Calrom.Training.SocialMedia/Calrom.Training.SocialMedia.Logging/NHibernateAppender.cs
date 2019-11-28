// -----------------------------------------------------------------------
// <copyright file="NHibernateAppender.cs" company="Calrom Limited">
// Copyright (c) Calrom Limited. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Calrom.Training.SocialMedia.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// This log4net appender is used for outputting NHibernate sql statements in a sql management studio friendly format.
    /// This means you should be able to copy the sql output from this appender and run it directly.  Normally in the NHibernate
    /// output there is parameterized sql that must be manually edited to run it.
    /// http://stackoverflow.com/questions/11015005/execute-nhibernate-generated-prepared-statements-in-sql-server-management-studio
    /// http://gedgei.wordpress.com/2011/09/03/logging-nhibernate-queries-with-parameters/
    /// </summary>
    public class NHibernateAppender : ForwardingAppender
    {
        /// <summary>
        /// The parameter regex
        /// @"@p\d+(?=[,);\s])(?!\s*=)" - Test
        /// </summary>
        private const string ParamRegex = @"@p\d+(?=[,);\s])(?!\s*=)";

        /// <summary>
        /// The parameter value regex
        /// </summary>
        private const string ParamValueRegex = @".*{0}\s*=\s*(.*?)\s*[\[].*";

        /// <summary>
        /// The unique identifier regex
        /// </summary>
        private const string GuidRegex = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b";

        /// <summary>
        /// The date regex
        /// </summary>
        private const string DateRegex = @"\b([0-3][0-9])/([0-1][0-9])/([0-9]{4})\s([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])\b";

        /// <summary>
        /// The bool regex
        /// match 0 and 1 too = \b([Vv]+(erdade(iro)?)?|[Ff]+(als[eo])?|[Tt]+(rue)?|0|[\+\-]?1)\b
        /// </summary>
        private const string BoolRegex = @"\b([Vv]+(erdade(iro)?)?|[Ff]+(als[eo])?|[Tt]+(rue))\b";

        /// <summary>
        /// Appends the specified logging event.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            var loggingEventData = loggingEvent.GetLoggingEventData();

            if (loggingEventData.Message.Contains("@p"))
            {
                StringBuilder messageBuilder = new StringBuilder();

                string message = loggingEventData.Message;
                var queries = Regex.Split(message, @"command\s\d+:");
                for (int i = 0; i <= queries.Length - 1; i++)
                {
                    messageBuilder.Append(this.ReplaceQueryParametersWithValues(queries[i]));

                    if (i > 0)
                    {
                        // for batch quries add an empty line.
                        messageBuilder.Append(Environment.NewLine);
                    }
                }

                loggingEventData.Message = messageBuilder.ToString();
            }

            base.Append(new LoggingEvent(loggingEventData));
        }

        /// <summary>
        /// Replaces the query parameters with values.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>returns string</returns>
        private string ReplaceQueryParametersWithValues(string query)
        {
            try
            {
                string returnQuery = Regex.Replace(
                    query,
                    ParamRegex,
                    match =>
                    {
                        Regex parameterValueRegex = new Regex(string.Format(ParamValueRegex, match));
                        var parameterValue = parameterValueRegex.Match(query).Groups[1].ToString();
                        if (string.IsNullOrEmpty(parameterValue))
                        {
                            // Fix for large xmls or multiline string values
                            // Extract value using substring.
                            // No need to replace single quote because string value always wrapped in single quote.
                            var startParameter = string.Format("{0} = ", match.ToString());
                            var startParameterIndex = query.IndexOf(startParameter) + startParameter.Length;
                            var endParameterIndex = query.IndexOf(" [Type: String ", startParameterIndex);
                            if (endParameterIndex > 0 && startParameterIndex < query.Length && endParameterIndex < query.Length)
                            {
                                parameterValue = query.Substring(startParameterIndex, endParameterIndex - startParameterIndex);
                            }
                        }
                        else
                        {
                            int length = !string.IsNullOrEmpty(parameterValue) ? parameterValue.Length : 0;
                            if (length == Guid.Empty.ToString().Length)
                            {
                                // Place single quotes around all Guids in the sql string
                                parameterValue = Regex.Replace(parameterValue, GuidRegex, "'$0'", RegexOptions.IgnoreCase);
                            }
                            else if (length == DateTime.MinValue.ToString().Length)
                            {
                                // Place single quotes around all Dates in the sql string
                                parameterValue = Regex.Replace(parameterValue, DateRegex, "'$0'", RegexOptions.IgnoreCase);
                            }
                            else if (length == true.ToString().Length || length == false.ToString().Length)
                            {
                                // Place single quotes around all Bools in the sql string
                                parameterValue = Regex.Replace(parameterValue, BoolRegex, "'$0'", RegexOptions.IgnoreCase);
                            }
                        }

                        return parameterValue;
                    });

                int parameterListIndex = returnQuery.LastIndexOf("@p0");
                if (parameterListIndex != -1)
                {
                    // Truncate the paramter list off the end since we are substituting the actual values in the regular expression above
                    // The -1 also cuts off the semicolon at the end
                    return returnQuery.Substring(0, parameterListIndex).Trim();
                }

                return returnQuery.Trim();
            }
            catch (Exception ex)
            {
                // if any exception has ocurred than return query as it is.
                System.Diagnostics.Debug.WriteLine("ReplaceQueryParametersWithValues - Error - ", ex.Message);
            }

            // if any exception has ocurred than return query as it is.
            return query;
        }
    }
}
