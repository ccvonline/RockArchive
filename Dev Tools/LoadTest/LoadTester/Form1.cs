﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Rock;

namespace LoadTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStart_Click( object sender, EventArgs e )
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            progressBar1.Show();

            int clientCount = tbClientCount.Text.AsInteger();
            int requestCountPerClient = tbRequestCount.Text.AsInteger();
            string url = tbUrl.Text;

            ConcurrentBag<double> results = new ConcurrentBag<double>();
            ConcurrentBag<Exception> exceptions = new ConcurrentBag<Exception>();

            var stopwatchTestDuration = Stopwatch.StartNew();
            long requestCount = 0;
            int threadCount = 0;
            progressBar1.Maximum = clientCount * requestCountPerClient;
            var requestUrl = new Uri( url );
            var baseUri = new Uri( requestUrl.Scheme + "://" + requestUrl.Host + ":" + requestUrl.Port.ToString() );


            Parallel.For( 1, clientCount, ( loopState ) =>
            {
                Interlocked.Increment( ref threadCount );
                try
                {
                    int requestCounter = 0;
                    var cookieContainer = new CookieContainer();
                    while ( requestCounter < requestCountPerClient )
                    {

                        var stopwatch = Stopwatch.StartNew();
                        var clientRequest = (HttpWebRequest)WebRequest.Create( url );
                        clientRequest.CookieContainer = cookieContainer;
                        clientRequest.UserAgent = "LoadTester";
                        clientRequest.Timeout = 10000;

                        using ( var response = clientRequest.GetResponse() )
                        {
                            using ( var stream = response.GetResponseStream() )
                            {
                                using ( var reader = new StreamReader( stream ) )
                                {
                                    var responseHtml = reader.ReadToEnd();
                                    stopwatch.Stop();

                                    if ( requestCounter == 1 )
                                    {
                                        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                                        htmlDoc.LoadHtml( responseHtml );
                                        var nodesWithSrc = htmlDoc.DocumentNode.DescendantsAndSelf()
                                            .Where( a => a.NodeType == HtmlAgilityPack.HtmlNodeType.Element )
                                            .Where( a => a.Attributes.Any( x => x.Name == "src" ) )
                                            .ToList();
                                        Parallel.ForEach( nodesWithSrc, ( srcNode ) =>
                                        {
                                            try
                                            {
                                                var srcRef = srcNode.Attributes["src"].Value;
                                                var srcUri = new Uri( baseUri, srcRef );
                                                var srcRequest = (HttpWebRequest)WebRequest.Create( srcUri );
                                                var srcResponse = srcRequest.GetResponse();
                                                
                                                using ( var resultStream = srcResponse.GetResponseStream() )
                                                {
                                                    using ( var resultReader = new StreamReader( resultStream ) )
                                                    {
                                                        var resultData = resultReader.ReadToEnd();
                                                    }
                                                }
                                            }
                                            catch ( Exception ex )
                                            {
                                                exceptions.Add( ex );
                                            }
                                        } );
                                    }

                                    Interlocked.Increment( ref requestCount );
                                    //if ( requestCount % 100 == 0 )
                                    {
                                        UpdateProgressBar( requestCount, threadCount );
                                    }
                                }
                            }

                        }

                        stopwatch.Stop();
                        results.Add( Math.Round(stopwatch.Elapsed.TotalMilliseconds, 3 ) );
                        requestCounter++;

                    }
                }
                catch ( Exception ex )
                {
                    exceptions.Add( ex );
                }

                Interlocked.Decrement( ref threadCount );
            } );

            UpdateProgressBar( requestCount, threadCount );
            var totalTime = results.Sum();
            var requestsPerMillisecond = requestCount / totalTime;


            var aveResponseTime = totalTime / requestCount;
            try
            {
                tbStats.Text = string.Format( @"
Median: {0}ms responseTime 
Mode: {1}ms responseTime 
Average: {2}ms responseTime 
TotalRequests: {3},
TotalTime: {4}ms
Requests/sec: {5}
Exceptions: {6}
", results.Median(),
       results.Mode(),
       results.Average(),
       results.Count(),
       stopwatchTestDuration.Elapsed.TotalMilliseconds,
       requestsPerMillisecond * 1000,
       exceptions.Count() );

                tbStats.Text = tbStats.Text.Trim();
            }
            catch ( Exception ex )
            {
                tbStats.Text = ex.Message;
            }

            tbResults.Text = exceptions.Select( a => a.Message ).ToList().AsDelimited( Environment.NewLine )
                + Environment.NewLine
                + results.ToList().AsDelimited( Environment.NewLine );

            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Hide();
        }

        /// <summary>
        /// Updates the progress bar.
        /// </summary>
        /// <param name="requestCount">The request count.</param>
        /// <param name="threadCount">The thread count.</param>
        private void UpdateProgressBar( long requestCount, long threadCount )
        {
            if ( InvokeRequired )
            {
                BeginInvoke( new Action<long,long>( UpdateProgressBar ), new object[] { requestCount, threadCount } );
                return;
            }

            progressBar1.Value = (int)Interlocked.Read( ref requestCount );
            lblThreadCount.Text = Interlocked.Read( ref threadCount ).ToString();
            lblThreadCount.Refresh();
        }
    }
}
