using nRank.console.FileProcessors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nRankTests.FileReader
{
    [TestFixture]
    class PairInformationTableReaderTests
    {
        [Test]
        public void DummyTest()
        {
            var reader = new PairInformationTableReader();

            string executableLocation = Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().Location);
            reader.Read(Path.Combine(executableLocation, "Houses11.isf"));
        }

        [Test]
        public void ConfigReadTest()
        {
            var reader = new ConfigurationReader();
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var config = reader.ReadConfiguration(Path.Combine(executableLocation, "Houses11\\experiment.properties"));
        }
    }
}
