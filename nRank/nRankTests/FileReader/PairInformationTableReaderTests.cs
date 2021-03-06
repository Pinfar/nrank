﻿using nRank.console.FileProcessors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void DummyTest2()
        {
            var reader = new PCTReader();

            string executableLocation = Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().Location);
            reader.Read(Path.Combine(executableLocation, "Houses7_partialPCT.isf"));
        }

        [Test]
        public void ConfigReadTest()
        {

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            var reader = new ConfigurationReader();
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var config = reader.ReadConfiguration(Path.Combine(executableLocation, "Houses11\\experiment.properties"));
        }
    }
}
