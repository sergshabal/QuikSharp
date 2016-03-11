using System;
using NUnit.Framework;
using QuikSharp.DataStructures;
using System.Threading;


namespace QuikSharp.Tests
{
    [TestFixture]
    public class EventsTests
    {
        Quik quik;
        ManualResetEvent mre;
        bool called;
        
        public EventsTests() {
            quik = new Quik();
            mre = new ManualResetEvent(false);
            called = false;
            //bool isConn = quik.Service.IsConnected().Result;
            //Assert.True(isConn, "Quik must be connected");
        }

        private void AfterTest() {
            called = false;
            mre.Reset();
        }
        
        [Test]
        public void OnAccountBalanceTest()
        {
            // Console.WriteLine("Make event in quik that change account balance");
            quik.Events.OnAccountBalance += (AccountBalance acc_bal) => { 
                mre.Set();
                called = true;
            };
            
            mre.WaitOne(10000);
            Assert.IsTrue(called, "Event OnAccountBalance not called!");
            AfterTest();
        }

        [Test]
        public void OnParamTest()
        {
            quik.Events.OnParam += (Param par) => {
                called = true;
                //Console.WriteLine(string.Format("ClassCode = {}, SecCode = {}", par.ClassCode, par.SecCode));
                mre.Set();
            };
            mre.WaitOne(10000);
            Assert.IsTrue(called, "Event OnParam not called!");
            AfterTest();
        }

        [Test]
        public void OnDisconnectedTest()
        {
            Assert.True(quik.Service.IsConnected().Result, "Quik must be connected before run this");
            quik.Events.OnDisconnected += () =>
            {
                called = true;
                mre.Set();
            };
            mre.WaitOne(60000);
            Assert.IsTrue(called, "Event OnDisconnected not called!");
            AfterTest();
        }

        [Test]
        public void OnConnectedTest()
        {
            Assert.False(quik.Service.IsConnected().Result, "Quik must be disconnected before run this");
            quik.Events.OnConnected += () =>
            {
                called = true;
                mre.Set();
            };
            mre.WaitOne(60000);
            Assert.IsTrue(called, "Event OnConnected not called!");
            AfterTest();
        }

        [Test]
        public void OnDepoLimitTest()
        {
            quik.Events.OnDepoLimit += (DepoLimitEx dl) => {
                called = true;
                mre.Set();
            };

            mre.WaitOne(60000);
            Assert.True(called, "Event OnDepoLimit not callsed!");
            AfterTest();
        }

        [Test]
        public void OnDepoLimitDeleteTest()
        {
            quik.Events.OnDepoLimitDelete += (DepoLimitEx dl) =>
            {
                called = true;
                mre.Set();
            };

            mre.WaitOne(60000);
            Assert.True(called, "Event OnDepoLimitDelete not callsed!");
            AfterTest();
        }
    }
}
