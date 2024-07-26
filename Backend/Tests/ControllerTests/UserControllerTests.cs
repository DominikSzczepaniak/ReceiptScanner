using System.Net;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Backend.Tests.ControllerTests
{
    [TestFixture]
    public static class UserControllerTests
    {
        private static string webAdress = "https://localhost:7168/api";
        public static string username;
        public static string password;

        [OneTimeSetUp]
        public static void RunBeforeTests()
        {
            username = Guid.NewGuid().ToString();
            password = Guid.NewGuid().ToString();
        }
        
        [Test, Order(1)]
        public static void UserController_RegistersUser_ReturnsOk(){
            Console.WriteLine(username + " " + password);
            var myRequest = (HttpWebRequest)WebRequest.Create(webAdress + "/User/username/password");
            myRequest.Method = "POST";

            using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
            {
                ClassicAssert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test, Order(2)]
        public static void UserController_FindsUserId_ReturnsOk()
        {
            var myRequest = (HttpWebRequest)WebRequest.Create(webAdress + "/User/username/password");
            myRequest.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
            {
                ClassicAssert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test, Order(3)]
        public static void UserController_DeletesUser_ReturnsOk()
        {
            var myRequest = (HttpWebRequest)WebRequest.Create(webAdress + "/User/username/password");
            myRequest.Method = "DELETE";

            using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
            {
                ClassicAssert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        
        
    }
}
