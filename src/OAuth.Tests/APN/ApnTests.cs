using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using AssociatePublishingNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OAuth.Tests {

	/// <summary>
	/// these tests are not setup to be real unit tests. i'm using the test framework as just a way to run arbitrary code and see the results.
	/// </summary>
	[TestClass]
	public class ApnTests {

		[TestMethod]
		public void APN_BasicTest() {
			string url = "/v1/test";
			var apn = new ApnClient();

			// get and post - null params
			var task = apn.SendRequest(url, null, HttpMethod.Get);
			task.Wait();
			Trace.WriteLine(task.Result);

			task = apn.SendRequest(url, null, HttpMethod.Post);
			task.Wait();
			Trace.WriteLine(task.Result);

			// get and post - with params
			Dictionary<string, string> args = new Dictionary<string, string>();
			args.Add("guid", "test");

			task = apn.SendRequest(url, args, HttpMethod.Get);
			task.Wait();
			Trace.WriteLine(task.Result);

			task = apn.SendRequest(url, args, HttpMethod.Post);
			task.Wait();
			Trace.WriteLine(task.Result);
		}

		[TestMethod]
		public void APN_TestInstall() {
			string url = "/v1/events/install/6fd829133ac4e14a1eff2eced43292e0ce1da5da30108dec77383cb5c32a84a1";
			var apn = new ApnClient();
			var task = apn.SendRequest(url, null, HttpMethod.Put);
			task.Wait();
			Trace.WriteLine(task.Result);
		}

		[TestMethod]
		public void APN_TestHandoffTicket() {
			string url = "/v1/handoff-ticket";
			Dictionary<string, string> args = new Dictionary<string, string>();
			args.Add("campaign", "test-campaign");
			args.Add("uids", "6fd829133ac4e14a1eff2eced43292e0ce1da5da30108dec77383cb5c32a84a1");
			var apn = new ApnClient();
			var task = apn.SendRequest(url, args, HttpMethod.Get);
			task.Wait();
			Trace.WriteLine(task.Result);
		}

	}
}
